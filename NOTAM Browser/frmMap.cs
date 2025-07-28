using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;
#if DEBUG
using System.Diagnostics;
#endif
using GMap.NET.MapProviders;
using NOTAM_Browser.MapProviders;
using NOTAM_Browser.Properties;
using System.Text.RegularExpressions;
using GMap.NET.WindowsForms;

namespace NOTAM_Browser
{
    public partial class frmMap : Form
    {
        private readonly Notams nos;
        private readonly Dictionary<string, GMapProvider> mapProviders;

        public frmMap(Notams nos)
        {
            mapProviders = new Dictionary<string, GMapProvider>
            {
                { "Google Map", GoogleMapProvider.Instance },
                { "Google Satellite", GoogleSatelliteMapProvider.Instance },
                { "Google Terrain", GoogleTerrainMapProvider.Instance },
                { "Google Hybrid", GoogleHybridMapProvider.Instance },
                { "OpenStreetMap", OpenStreetMapProvider.Instance },
                { "Bing Map", BingMapProvider.Instance },
                { "OpenCycleMap", OpenCycleMapProvider.Instance },
                { "Mapy (Treba se registrovati)", MapyMapProvider.Instance },
                { "ArcGIS World Street Map", ArcGIS_World_Street_MapProvider.Instance },
                { "ArcGIS World Topo Map", ArcGIS_World_Topo_MapProvider.Instance },
                { "ArcGIS World Shaded Relief", ArcGIS_World_Shaded_Relief_MapProvider.Instance },
                { "ArcGIS World Imagery", ArcGIS_World_Imagery_MapProvider.Instance },
            };

            this.nos = nos;

            InitializeComponent();

            cmbProvider.DataSource = new BindingSource(mapProviders, null);
            cmbProvider.DisplayMember = "Key";
            cmbProvider.ValueMember = "Value";


            string mapProviderName = Settings.Default.latestMapUsed ?? "";

#if DEBUG
            Debug.WriteLine($"frmMap: Latest map used from settings: {mapProviderName}");
#endif
            if (mapProviders.ContainsKey(mapProviderName))
            {
                cmbProvider.SelectedValue = mapProviders[mapProviderName];
                gMapControl1.MapProvider = mapProviders[mapProviderName];
            }
            else
            {
                cmbProvider.SelectedIndex = 0; // Default to the first provider if not found
                gMapControl1.MapProvider = mapProviders.ElementAt(0).Value;
            }

            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            gMapControl1.ShowCenter = false;
            gMapControl1.DisableFocusOnMouseEnter = true;

            Regex r = new Regex(@"^-?\d*\.?\d+,-?\d*\.?\d+$");

            string lastPos = Settings.Default.lastMapPosition ?? "0,0";

            Match m = r.Match(lastPos);

            if (!m.Success)
            {
                lastPos = "0,0";
            }

            string[] coords = lastPos.Split(',');

            gMapControl1.Position = new GMap.NET.PointLatLng(double.Parse(coords[0]), double.Parse(coords[1]));
            gMapControl1.Zoom = Settings.Default.lastMapZoom;

            var polys = MapManager.LoadPolys();

            if (polys != null)
            {
                foreach (var coordinateList in polys.ZoneData)
                {
                    if (coordinateList.Value == null || coordinateList.Value.Points == null) continue;

                    chkMapElements.Items.Add(coordinateList.Key, true);
                    List<GMap.NET.PointLatLng> points = new List<GMap.NET.PointLatLng>();

                    foreach (var coord in coordinateList.Value.Points)
                    {
                        if (coord.Count != 2)
                        {
#if DEBUG
                            Debug.WriteLine($"frmMap: Invalid coordinate format in zone {coordinateList.Key}: {string.Join(",", coord)}");
#endif
                            continue; // Skip invalid coordinates
                        }

                        points.Add(new GMap.NET.PointLatLng(coord[0], coord[1]));
                    }
                    // Draw the polygon on the map
                    GMap.NET.WindowsForms.GMapPolygon polygon = new GMap.NET.WindowsForms.GMapPolygon(points, coordinateList.Key);
                    polygon.Fill = Brushes.Transparent; // No fill color
                    Color c;
                    if (coordinateList.Value.Color.Count == 4)
                    {
                        c = Color.FromArgb(coordinateList.Value.Color[0], coordinateList.Value.Color[1], coordinateList.Value.Color[2], coordinateList.Value.Color[3]);
                    }
                    else
                    {
                        c = Color.FromArgb(255, 255, 255, 128); // Default to black if color is not specified correctly
                    }

                    polygon.Stroke = new Pen(c, 2); // Red border

                    var o = new GMap.NET.WindowsForms.GMapOverlay(coordinateList.Key);

                    o.Polygons.Add(polygon);

                    gMapControl1.Overlays.Add(o);

                }

            }
            else
            {
#if DEBUG
                Debug.WriteLine("frmMain: No coordinates found in the JSON file.");
#endif
            }
        }

        public void DrawNotam(string NotamID)
        {
            if (chkMapElements.Items.Contains(NotamID)) return;

            string notamText = nos.CurrentNotams[NotamID];

            var coordinates = NotamParser.ParseCoordinates(notamText);

            if (coordinates.Count == 0) return;

            var notamQCoordinate = NotamParser.GetNotamQCoordinate(notamText);

            var overlay = new GMapOverlay(NotamID);

            GMapPolygon polygon = new GMapPolygon(NotamParser.ConvertCoordinatesForMap(coordinates), NotamID)
            {
                Fill = Brushes.Transparent, // No fill color
                Stroke = new Pen(Color.Red, 2) // Red border
            };

            overlay.Polygons.Add(polygon);

            gMapControl1.Overlays.Add(overlay);

            // TODO: ne pojavljuje se odmah, treba da se osveži mapa ali ovo ne radi
            gMapControl1.Zoom--;
            gMapControl1.Zoom++;

            chkMapElements.Items.Add(NotamID, true);

            if (notamQCoordinate != null)
            {
                gMapControl1.Position = new GMap.NET.PointLatLng(notamQCoordinate.Item1, notamQCoordinate.Item2);
            }

            this.Show();
        }

        private void chkMapElements_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int index = e.Index;
            bool isChecked = e.NewValue == CheckState.Checked;
            if (index < 0 || index >= chkMapElements.Items.Count) return;
            string name = chkMapElements.Items[index].ToString();

            var overlay = gMapControl1.Overlays.FirstOrDefault(x => x.Id == name);
            if (overlay != null)
            {
                overlay.IsVisibile = isChecked;
            }

#if DEBUG
            Debug.WriteLine($"frmMap: {name}; {!isChecked} -> {isChecked}");
#endif
        }

        private void btnAllOn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkMapElements.Items.Count; i++)
            {
                chkMapElements.SetItemChecked(i, true);
            }
        }

        private void btnAllOff_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkMapElements.Items.Count; i++)
            {
                chkMapElements.SetItemChecked(i, false);
            }
        }

        private void cmbProvider_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.Visible == false) return;

            var selectedProvider = cmbProvider.SelectedValue as GMapProvider;

            if (selectedProvider != null)
            {
                gMapControl1.MapProvider = selectedProvider;

                string naziv = ((KeyValuePair<string, GMapProvider>)cmbProvider.SelectedItem).Key;

                Settings.Default.latestMapUsed = naziv;
                Settings.Default.Save();
#if DEBUG
                Debug.WriteLine($"frmMap: Map provider changed to {naziv}.");
#endif
            }
        }

        private void frmMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

        private void SavePosition()
        {
            Settings.Default.lastMapPosition = $"{gMapControl1.Position.Lat},{gMapControl1.Position.Lng}";
            Settings.Default.lastMapZoom = gMapControl1.Zoom;
            Settings.Default.Save();
        }

        public void ClosingApp()
        {
#if DEBUG
            Debug.WriteLine("frmMap: ClosingApp called");
#endif

            SavePosition();

            GMap.NET.GMaps.Instance.CancelTileCaching();
            gMapControl1.Manager.CancelTileCaching();
            gMapControl1.Overlays.Clear();
            gMapControl1.Dispose();

        }

        private void btnLoadPolys_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Otvori JSON fajl sa koordinatama";
                ofd.Filter = "JSON files (*.json)|*.json";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = File.ReadAllText(ofd.FileName);
                        Zones zones = JsonConvert.DeserializeObject<Zones>(json);

                        MapManager.AddPolys(zones);

                        foreach (var z in zones.ZoneData)
                        {

                            if (z.Value == null || z.Value.Points == null)
                            {
                                if(chkMapElements.Items.Contains(z.Key))
                                {
                                    chkMapElements.Items.Remove(z.Key);
                                    gMapControl1.Overlays.Remove(gMapControl1.Overlays.FirstOrDefault(ov => ov.Id == z.Key));
                                }
                                continue;
                            }

                            if (chkMapElements.Items.Contains(z.Key))
                            {
                                gMapControl1.Overlays.Remove(gMapControl1.Overlays.FirstOrDefault(ov => ov.Id == z.Key));
                            }
                            else 
                            {
                                chkMapElements.Items.Add(z.Key, true);
                            }

                            if(z.Value == null || z.Value.Points == null) continue;


                            List<GMap.NET.PointLatLng> points = new List<GMap.NET.PointLatLng>();
                            foreach (var coord in z.Value.Points)
                            {
                                if (coord.Count != 2)
                                {
#if DEBUG
                                    Debug.WriteLine($"frmMap: Invalid coordinate format in zone {z.Key}: {string.Join(",", coord)}");
#endif
                                    continue; // Skip invalid coordinates
                                }

                                points.Add(new GMap.NET.PointLatLng(coord[0], coord[1]));
                            }
                            // Draw the polygon on the map
                            GMap.NET.WindowsForms.GMapPolygon polygon = new GMap.NET.WindowsForms.GMapPolygon(points, z.Key);
                            polygon.Fill = Brushes.Transparent; // No fill color
                            Color c;
                            if (z.Value.Color.Count == 4)
                            {
                                c = Color.FromArgb(z.Value.Color[0], z.Value.Color[1], z.Value.Color[2], z.Value.Color[3]);
                            }
                            else
                            {
                                c = Color.FromArgb(255, 255, 255, 128); // Default to black if color is not specified correctly
                            }

                            polygon.Stroke = new Pen(c, 2); // Red border

                            var o = new GMap.NET.WindowsForms.GMapOverlay(z.Key);

                            o.Polygons.Add(polygon);

                            gMapControl1.Overlays.Add(o);
                        }


                        gMapControl1.Zoom++;
                        gMapControl1.Zoom--;

                        MessageBox.Show("Mape su uspešno učitane i sačuvane. Možeš obrisati fajl koji si učitao.", "Učitane mape", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine($"frmMap: Error loading polygons from file {ofd.FileName}: {ex.ToString()}");
#endif
                        MessageBox.Show($"Greška pri učitavanju koordinata: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
