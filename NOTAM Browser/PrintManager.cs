using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading;
using System.Windows.Forms;
using NOTAM_Browser.MapTypes;


#if DEBUG
using System.Diagnostics;
#endif

namespace NOTAM_Browser
{
    internal class PrintManager : IDisposable
    {
        private const int DefaultPrintTimeoutSeconds = 45;

        public GMapControl MapControl { get; set; }
        public GMapControl NewControl { get; private set; }
        public int ZoomLevel { get; set; }
        public int PrintTimeoutSeconds { get; set; } = DefaultPrintTimeoutSeconds; // Default timeout for printing in seconds

        public bool IsPrintingInProgress
        {
            get
            {
                return frm != null && !frm.IsDisposed && frm.Visible;
            }
        }

        private frmPrint frm { get; set; }
        private Bitmap mapImage { get; set; }


        private bool waitingForTileLoad = true;

        public void Dispose()
        {
            if (frm != null && !frm.IsDisposed)
            {
                frm.Dispose();
            }
            if (NewControl != null && !NewControl.IsDisposed)
            {
                NewControl.Dispose();
            }
            mapImage?.Dispose();
        }

        public delegate void PrintEndedHandler(object sender);
        public event PrintEndedHandler PrintEnded;

        public PrintManager(GMapControl mapControl, int ZoomLevel) : this(mapControl)
        {
            this.ZoomLevel = ZoomLevel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintManager"/> class, which manages the printing process for a
        /// map.
        /// </summary>
        /// <remarks>The <see cref="PrintManager"/> class sets up a default zoom level and initializes a
        /// new form for managing the printing process. It also creates a new <see cref="GMapControl"/> instance
        /// specifically for printing, with customized settings. The printing form displays a status message and
        /// prevents the form from being closed directly, ensuring the printing process completes properly.</remarks>
        /// <param name="mapControl">The <see cref="GMapControl"/> instance representing the map to be printed. This control is used as the
        /// source for printing.</param>
        public PrintManager(GMapControl mapControl)
        {
            this.ZoomLevel = 10; // Default zoom level for printing
            this.MapControl = mapControl;

            //setting up the form
            frm = new frmPrint(this);

            frm.SetLabelText("Pripremam štampanje...");

            frm.FormClosing += (s, e) =>
            {
                e.Cancel = true;
                CompletePrint();
            };

            NewControl = new GMapControl
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Bearing = 0F,
                CanDragMap = false,
                EmptyTileColor = Color.Navy,
                GrayScaleMode = false,
                HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow,
                LevelsKeepInMemory = 5,
                Location = new Point(frm.Width, frm.Height),
                MarkersEnabled = true,
                MinZoom = MapControl.MinZoom,
                MaxZoom = MapControl.MaxZoom,
                MouseWheelZoomEnabled = false,
                MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter,
                Name = "printMap",
                NegativeMode = false,
                PolygonsEnabled = true,
                RetryLoadTile = 0,
                RoutesEnabled = true,
                ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer,
                SelectedAreaFillColor = Color.FromArgb(33, 65, 105, 225),
                ShowTileGridLines = false,
                ShowCenter = false,
                Size = new Size(800, 600),
                TabIndex = 0,
                Zoom = 9D,
                MapProvider = MapControl.MapProvider
            };
            NewControl.Manager.Mode = GMap.NET.AccessMode.ServerAndCache;

            frm.Controls.Add(NewControl);
        }


        /// <summary>
        /// Completes the print operation by performing necessary cleanup and signaling the end of the process.
        /// </summary>
        /// <remarks>This method hides the associated form and triggers the <see cref="PrintEnded"/> event
        /// to notify subscribers that the print operation has concluded.</remarks>
        private void CompletePrint()
        {
            frm.HideSafe();
            PrintEnded?.Invoke(this);
        }

        /// <summary>
        /// Updates the map control with new overlays, markers, polygons, and routes, applying the specified stroke
        /// size.
        /// </summary>
        /// <remarks>This method synchronizes the overlays from the existing map control to a new map
        /// control, ensuring that only visible overlays are included. It adjusts the stroke size of polygons and routes
        /// to the specified value and preserves other visual properties such as fill color and hit test visibility. If
        /// the method is called from a thread other than the UI thread, it will invoke itself on the UI
        /// thread.</remarks>
        /// <param name="strokeSize">The width of the stroke to apply to polygons and routes, in pixels. Must be a positive integer.</param>
        public void UpdateNewMapControl(int strokeSize)
        {
            if(frm.InvokeRequired)
            {
                frm.Invoke(new MethodInvoker(delegate
                {
                    UpdateNewMapControl(strokeSize);
                }));
                return;
            }

            NewControl.Overlays.Clear();

            foreach (var overlay in MapControl.Overlays)
            {
                if (!overlay.IsVisibile) continue;

                GMapOverlay newOverlay = new GMapOverlay(overlay.Id);
                foreach (var obj in overlay.Markers)
                {
                    GMapMarker newMarker;

                    if (obj is GMapLevelBlock m)
                    {
                        newMarker = new GMapLevelBlock(m.Position, m.LowerLimit, m.UpperLimit, m.TextBrush, m.FontFamily, m.FontHeightInMeters)
                        {
                            IsVisible = m.IsVisible
                        };

                        newOverlay.Markers.Add(newMarker);
                    }
                    else
                    {
                        newOverlay.Markers.Add(obj);
                    }
                }
                foreach (var obj in overlay.Polygons)
                {
                    var newPolygon = new GMapPolygon(obj.Points, obj.Name)
                    {
                        Stroke = new Pen(obj.Stroke.Color, strokeSize),
                        Fill = obj.Fill,
                        IsHitTestVisible = obj.IsHitTestVisible
                    };

                    newOverlay.Polygons.Add(newPolygon);
                }
                foreach (var obj in overlay.Routes)
                {
                    var newRoute = new GMapRoute(obj.Points, obj.Name)
                    {
                        Stroke = new Pen(obj.Stroke.Color, strokeSize),
                        IsHitTestVisible = obj.IsHitTestVisible
                    };
                    newOverlay.Routes.Add(newRoute);
                }
                NewControl.Overlays.Add(newOverlay);
            }

            NewControl.MapProvider = MapControl.MapProvider;

            NewControl.SetZoomToFitRect(MapControl.ViewArea);

            NewControl.ReloadMap();
        }


        /// <summary>
        /// Executes the printing process for the map displayed in the application.
        /// </summary>
        /// <remarks>This method handles the preparation, rendering, and printing of the map using a <see
        /// cref="PrintDocument">. It adjusts the map's dimensions to fit the page while maintaining the aspect ratio,
        /// waits for the map tiles to load, and ensures the map image is ready before printing. If an error occurs
        /// during the process, the operation is canceled, and an error message is displayed to the user.</remarks>
        private void DoPrint_Thread()
        {
            try
            {
                using (PrintDocument doc = new PrintDocument())
                {
                    doc.PrintPage += (s, e) =>
                    {
                        Rectangle pageBounds = e.MarginBounds;
                        RectLatLng mapBounds = MapControl.ViewArea;

                        GPoint topLeft = MapControl.MapProvider.Projection.FromLatLngToPixel(mapBounds.LocationTopLeft, ZoomLevel);
                        GPoint bottomRight = MapControl.MapProvider.Projection.FromLatLngToPixel(mapBounds.LocationRightBottom, ZoomLevel);

                        int width = (int)(bottomRight.X - topLeft.X);
                        int height = (int)(bottomRight.Y - topLeft.Y);

                        float mapAspectRatio = (float)width / height;
                        float pageAspectRatio = (float)pageBounds.Width / pageBounds.Height;

                        int printWidth = pageBounds.Width, printHeight = pageBounds.Height;

                        if (mapAspectRatio > pageAspectRatio)
                        {
                            // Map is wider than the page
                            height = (int)(width / pageAspectRatio);
                        }
                        else
                        {
                            // Map is taller than the page
                            width = (int)(height * pageAspectRatio);
                        }

#if DEBUG
                        Debug.WriteLine($"Print map control size {width}x{height}px");
                        Debug.WriteLine($"Print page size {pageBounds.Width}x{pageBounds.Height}px");
#endif
                        frm.SetLabelText("Pripremam mapu...");

                        frm.SetControlSize(NewControl, new Size(width, height));

                        waitingForTileLoad = true;

                        NewControl.OnTileLoadComplete += OnTileLoadComplete;

                        frm.SetLabelText("Učitavam mapu...");

                        mapImage = null;
                        int fallbackCounter = 0;

                        int strokeSize = (int)Math.Min(Math.Ceiling((double)printWidth / 100), 3); // Default stroke size for polygons and routes

                        UpdateNewMapControl((int)(strokeSize * mapAspectRatio));

#if DEBUG
                        Debug.WriteLine("PrintManager: Waiting for tile load to complete...");
#endif
                        PrintTimeoutSeconds = 45; //reset na podrazumevanu vrednost

                        while (waitingForTileLoad && fallbackCounter < PrintTimeoutSeconds * 10) // Izbaci iz thread-a nakon 45 sekundi
                        {
                            Thread.Sleep(100);
#if DEBUG
                            Debug.WriteLine("PrintManager: Waiting for tile load to complete... " + fallbackCounter);
#endif
                            frm.SetLabelText($"Učitavam mapu... {PrintTimeoutSeconds - fallbackCounter / 10}s");

                            fallbackCounter++;
                        }

#if DEBUG
                        Debug.WriteLine("PrintManager: Done!");
#endif

                        frm.SetLabelText(PrintTimeoutSeconds < 0 ? "Prekidam štampanje" : "Štampam mapu...");

                        Thread.Sleep(1000); // Daj malo vremena da se mapa osveži

                        if (mapImage == null)
                        {
                            if (PrintTimeoutSeconds >= 0)
                            {
                                frm.SetLabelText("Greška pri učitavanju mape.");
                                MessageBox.Show("Greška pri učitavanju mape. Pokušajte ponovo.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            e.Cancel = true;
                            CompletePrint();
                            return;
                        }

                        Rectangle printPosition = new Rectangle(
                            pageBounds.X + (pageBounds.Width - printWidth) / 2,
                            pageBounds.Y + (pageBounds.Height - printHeight) / 2,
                            printWidth,
                            printHeight
                        );

                        e.Graphics.DrawImage(mapImage, printPosition);
                        Font f = new Font("Arial", 8, FontStyle.Bold);
                        //SizeF textSize = e.Graphics.MeasureString("Proveriti podatke!", f);
                        e.Graphics.DrawString("Proveriti podatke", f, Brushes.Red, 
                            new PointF(pageBounds.X, pageBounds.Y));
                    };

                    doc.EndPrint += (s, e) =>
                    {
                        frm.SetLabelText("Štampanje završeno");
                        CompletePrint();
                    };

                    doc.DefaultPageSettings.Landscape = MapControl.Width >= MapControl.Height; // Postavi orijentaciju stranice na osnovu dimenzija mape
                    doc.DocumentName = "Mapa";

                    using (PrintDialog pd = new PrintDialog())
                    {
                        pd.Document = doc;
                        pd.AllowSomePages = false;
                        pd.ShowNetwork = true;
                        pd.AllowPrintToFile = true;

                        if (pd.ShowDialog() == DialogResult.OK)
                        {
                            frm.SetLabelText("Pokrećem štampanje...");
                            pd.Document.Print();
                        }
                        else
                        {
                            frm.SetLabelText("Štampanje otkazano.");
                            CompletePrint();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.Write($"PrintManager: Error DoPrint_Thread. {ex.ToString()}");
#endif
                CompletePrint();
            }
        }

        /// <summary>
        /// Initiates the printing process by displaying a status message and starting a background thread.
        /// </summary>
        /// <remarks>This method updates the user interface to indicate that printing is in progress and
        /// starts a  background thread to handle the printing operation. The thread is configured to run in 
        /// single-threaded apartment (STA) mode.</remarks>
        public void DoPrint()
        {
            PrintTimeoutSeconds = DefaultPrintTimeoutSeconds; // Reset timeout to default value

            frm.SetLabelText("Čekam za štampanje");
            frm.Show();

            Thread t = new Thread(DoPrint_Thread)
            {
                IsBackground = true,
                Name = "PrintManager_DoPrint_Thread"
            };
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }


        /// <summary>
        /// Handles the completion of tile loading and performs necessary actions such as rendering the map image.
        /// </summary>
        /// <remarks>This method is triggered when tile loading is complete. It detaches the event
        /// handler, creates a bitmap representation of the map, and ensures thread-safe rendering if required. If an
        /// error occurs during the process, the map image is set to <see langword="null"/>.</remarks>
        /// <param name="elapsedMiliseconds">The time, in milliseconds, that elapsed during the tile loading process.</param>
        private void OnTileLoadComplete(long elapsedMiliseconds)
        {
            try
            {
#if DEBUG
                Debug.WriteLine($"PrintManager: OnTileLoadComplete fired! Done in {elapsedMiliseconds} ms");
#endif

                NewControl.OnTileLoadComplete -= OnTileLoadComplete;

                mapImage = new Bitmap(NewControl.Width, NewControl.Height);
                
                if (frm.InvokeRequired)
                {
                    frm.Invoke(new MethodInvoker(delegate
                    {
                        NewControl.DrawToBitmap(mapImage, new Rectangle(0, 0, NewControl.Width, NewControl.Height));
                    }));
                } 
                else
                {
                    NewControl.DrawToBitmap(mapImage, new Rectangle(0, 0, NewControl.Width, NewControl.Height));
                }

                //mapImage = frm.GetImageFromMap(NewControl);
            }
            catch(Exception ex)
            {
#if DEBUG
                Debug.Write($"PrintManager: Error OnTileLoadComplete. {ex.ToString()}");
#endif
                mapImage = null;
            }
            finally
            {
#if DEBUG
                Debug.WriteLine("PrintManager: OnTileLoadComplete finished.");
#endif
                waitingForTileLoad = false;
            }
#if DEBUG
            Debug.WriteLine("PrintManager: OnTileLoadComplete finished2.");
#endif
            waitingForTileLoad = false;

        }
    }
}
