using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;
using GMap.NET.MapProviders;
using NOTAM_Browser.MapProviders;
using NOTAM_Browser.Properties;
using System.Text.RegularExpressions;
using GMap.NET.WindowsForms;
using NOTAM_Browser.MapTypes;
using GMap.NET;
using System.Security.Policy;
using System.Configuration;
using System.Xml.Linq;
//using static System.Net.WebRequestMethods;





#if DEBUG
using System.Diagnostics;
#endif

namespace NOTAM_Browser
{
    public partial class frmMap : Form
    {
        private readonly Notams nos;
        private readonly Dictionary<string, GMapProvider> mapProviders;
        private readonly PrintManager printManager;

        private string movingCenterNotamId;

        private bool changingVisibilityToBatch = false;

        #region "Constructor"
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

            stsStatus.Visible = false;

            changingVisibilityToBatch = true;

            chkDisplayLevels.Checked = Settings.Default.displayVerticalBoundaries;

            cmbProvider.DataSource = new BindingSource(mapProviders, null);
            cmbProvider.DisplayMember = "Key";
            cmbProvider.ValueMember = "Value";

            printManager = new PrintManager(gMapControl1);
            printManager.PrintEnded += EnableMap;

            string mapProviderName = Settings.Default.latestMapUsed ?? "";

#if DEBUG
            Debug.WriteLine($"frmMap: Latest map used from settings: {mapProviderName}");
#endif
            gMapControl1.CacheLocation = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "gmap_cache");

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

            gMapControl1.Position = new GMap.NET.PointLatLng(double.Parse(coords[0], System.Globalization.CultureInfo.InvariantCulture), double.Parse(coords[1], System.Globalization.CultureInfo.InvariantCulture));
            gMapControl1.Zoom = Settings.Default.lastMapZoom;

            this.LoadPolys();

            UpdateStatusBar();

            changingVisibilityToBatch = false;
        }
        #endregion

        #region "Loading Polygons"
        private void createOverlayForZone(ZoneData zd)
        {
            List<GMap.NET.PointLatLng> points = new List<GMap.NET.PointLatLng>();

            foreach (var coord in zd.Points)
            {
                if (coord.Count != 2)
                {
#if DEBUG
                    Debug.WriteLine($"frmMap: Invalid coordinate format in zone {zd.UID}: {string.Join(",", coord)}");
#endif
                    continue; // Skip invalid coordinates
                }

                points.Add(new GMap.NET.PointLatLng(coord[0], coord[1]));
            }
            // Draw the polygon on the map
            GMap.NET.WindowsForms.GMapPolygon polygon = new GMap.NET.WindowsForms.GMapPolygon(points, zd.UID);
            polygon.Fill = Brushes.Transparent; // No fill color
            Color c;
            if (zd.Color.Count == 4)
            {
                c = Color.FromArgb(zd.Color[0], zd.Color[1], zd.Color[2], zd.Color[3]);
            }
            else
            {
                c = Color.FromArgb(255, 255, 255, 128); // Default to black if color is not specified correctly
            }

            GMapLevelBlock lb = new GMapLevelBlock(new PointLatLng(zd.CenterCoordinate.Item1, zd.CenterCoordinate.Item2), zd.LowerLimit, zd.UpperLimit)
            {
                TextBrush = new SolidBrush(c),
                IsVisible = chkDisplayLevels.Checked
            };

            polygon.Stroke = new Pen(c, 2); // Red border

            var o = new GMap.NET.WindowsForms.GMapOverlay(zd.UID);

            o.Polygons.Add(polygon);
            o.Markers.Add(lb);

            gMapControl1.Overlays.Add(o);

            o.IsVisibile = zd.Visible;
        }

        public void LoadPolys()
        {
            var polys = MapManager.LoadPolys();

            var overlaysToDelete = gMapControl1.Overlays.ToList();

            if (polys != null && polys.Groups != null)
            {
                #region "NEW LIST CODE"
                foreach (var group in polys.Groups)
                {
                    if (group.Value == null) { continue; }

                    var checkListBox = GetCheckedListForGroup(group.Key, true);

                    foreach (var poly in group.Value.Polygons)
                    {
                        if (poly.Value == null || poly.Value.Points == null)
                        {
                            string uid = $"{group.Key}_{poly.Key}";

                            var overlay = gMapControl1.Overlays.FirstOrDefault(ov => ov.Id == uid);

                            if(overlay != null)
                            {
                                overlaysToDelete.Remove(overlay);
                                gMapControl1.Overlays.Remove(overlay);
                            }

                            int ind = checkListBox.Items.IndexOf(new Tuple<string, string>(poly.Value.UID, poly.Key));

                            if(ind > -1)
                            {
                                checkListBox.Items.RemoveAt(ind);
                            }

                            continue;
                        }

                        int itemIndex = checkListBox.Items.IndexOf(new Tuple<string, string>(poly.Value.UID, poly.Key));

                        if (itemIndex > -1) //Item exists in the list
                        {
                            checkListBox.SetItemChecked(itemIndex, poly.Value.Visible);

                            var overlay = gMapControl1.Overlays.FirstOrDefault(ov => ov.Id == poly.Value.UID);

                            if(overlay == null)
                            {
                                createOverlayForZone(poly.Value);
                            }
                            else
                            {
                                overlay.IsVisibile = poly.Value.Visible;
                                overlaysToDelete.Remove(overlay);
                            }
                        }
                        else
                        {
                            checkListBox.Items.Add(new Tuple<string, string>(poly.Value.UID, poly.Key), poly.Value.Visible);
                            createOverlayForZone(poly.Value);
                        }
                    }
                }

                foreach (var overlay in overlaysToDelete)
                {
                    var parts = overlay.Id.Split('_');

                    gMapControl1.Overlays.Remove(overlay);

                    if (parts.Length < 2) continue;

                    string uid = overlay.Id;
                    string groupName = parts[0];
                    string zoneName = overlay.Id.Substring(groupName.Length + 1);

                    //eh
                    var list = GetCheckedListForGroup(groupName);
                    if(list == null)
                    {
#if DEBUG
                        Debug.WriteLine($"frmMap: Couldn't find check list for group {groupName} on LoadPolys");
#endif
                        continue;
                    }

                    list.Items.Remove(new Tuple<string, string>(overlay.Id, zoneName));
                }
                #endregion
            }
            else
            {
#if DEBUG
                Debug.WriteLine($"frmMain: No coordinates found in the JSON file.");
#endif
            }

            var notamTabList = listTabControl.Controls.Find("chk_NOTAM", true);

            if (notamTabList.Length == 0)
            {
                addTabForGroup("NOTAM");
            }

            refreshMap();
        }
        #endregion

        private void refreshMap()
        {
            if(this.Visible && gMapControl1.Visible)
                gMapControl1.ReloadMap();

            double zoom = gMapControl1.Zoom;

            if (zoom == gMapControl1.MaxZoom)
                gMapControl1.Zoom = gMapControl1.MinZoom;
            else
                gMapControl1.Zoom = gMapControl1.MaxZoom;

            gMapControl1.Zoom = zoom;
        }

        private void addTabForGroup(string groupName)
        {
            var page = new TabPage()
            {
                Name = $"tab_{groupName}",
                Text = groupName,
            };

            listTabControl.TabPages.Add(page);

            var checkListBox = new CheckedListBox()
            {
                Name = $"chk_{groupName}",
                ValueMember = "Item1",
                DisplayMember = "Item2",
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom,
                Size = new Size(page.Width, page.Height - 23)
            };

            var turnAllOnBtn = new Button()
            {
                Name = $"btnAllOn_{groupName}",
                Text = "Uključi sve",
                Size = new Size(75, 23),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Location = new Point(0, page.Height - 23)
            };

            var turnAllOffBtn = new Button()
            {
                Name = $"btnAllOff_{groupName}",
                Text = "Isključi sve",
                Size = new Size(75, 23),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(page.Width - 75, page.Height - 23)
            };

            page.Controls.Add(checkListBox); //add the listBox so it sets size properly
            page.Controls.Add(turnAllOnBtn);
            page.Controls.Add(turnAllOffBtn);

            checkListBox.ItemCheck += chkMapElements_ItemCheck;
            turnAllOnBtn.Click += btnAllOn_Click;
            turnAllOffBtn.Click += btnAllOff_Click;
            checkListBox.MouseUp += checkListBox_MouseUp;
        }

        // =================== ctxChkList show ===================
        private void checkListBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (sender == null || !(sender is CheckedListBox)) return;
                var checkListBox = sender as CheckedListBox;

                bool allowDelete = checkListBox.Name == "chk_NOTAM" || checkListBox.Name == "chk_Custom";

                bool itemSelected = checkListBox.SelectedItems.Count > 0;

                if(!(allowDelete || itemSelected))
                    return;

                delPolyToolStripMenuItem.Enabled = allowDelete && itemSelected;
                obrišiSveUListiToolStripMenuItem.Enabled = allowDelete;
                promeniBojuToolStripMenuItem.Enabled = itemSelected;
                pomeriPozicijuVertikalnihGranicaToolStripMenuItem.Enabled = allowDelete && itemSelected;
                dodajKaoCustomZonuToolStripMenuItem.Enabled = !allowDelete && itemSelected;

                ctxChkList.Show(checkListBox, e.Location);
            }
        }

        private void EnableMap(object sender)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate {
                    EnableMap(sender);
                }));
                return;
            }

            this.Enabled = true;
        }

        public void DrawNotam(string NotamID)
        {
            if (this.Enabled == false)
            {
                MessageBox.Show("Mapa je trenutno zaključana. Sačekajte da se otključa pa probajte ponovo.", "Mapa zaključana");
                return;
            }

            this.Focus();

            string notamText = nos.CurrentNotams[NotamID];

            string notamName = NotamParser.GetNotamZoneName(notamText);

            var zone = NotamParser.TryFindZone(notamText);

            if (!string.IsNullOrEmpty(notamName))           // Pronadjen je naziv zone u zagradama
                notamName = $"{notamName} ({NotamID})";
            else if (zone != null)                              // U NOTAM-u je pronadjena zona iz postojecih
                notamName = $"{zone.ZoneName} ({NotamID})";
            else                                                // nije nijedno
                notamName = NotamID;

            string UID = $"NOTAM_{notamName}";

            this.Show();

            var res = listTabControl.Controls.Find("chk_NOTAM", true);

            if (res.Length == 0 || !(res[0] is CheckedListBox)) return;

            var chkList = (CheckedListBox)res[0];

            if (chkList.Items.Contains(new Tuple<string, string>(UID, notamName)))
            {
                int index = chkList.Items.IndexOf(new Tuple<string, string>(UID, notamName));
                chkList.SetItemCheckState(index, CheckState.Checked);
                return;
            }

            var coordinates = NotamParser.ParseCoordinates(notamText);

            if (coordinates.Count == 0 && zone == null) return;

            var notamQCoordinate = NotamParser.GetNotamQCoordinate(notamText);

            var overlay = new GMapOverlay(UID);

            List<PointLatLng> poly;

            Color c = Color.Red;

            if (coordinates.Count > 0)
            {
                poly = NotamParser.ConvertCoordinatesForMap(coordinates);
            }
            else //if (zone != null) ali se podrazumeva da nije jer if iznad proverava
            {
                poly = zone.ZonePoints;

                if(zone.Color.Count == 4)
                {
                    c = Color.FromArgb(zone.Color[0], zone.Color[1], zone.Color[2], zone.Color[3]);
                }
            }


            GMapPolygon polygon = new GMapPolygon(poly, UID)
            {
                Fill = Brushes.Transparent, // No fill color
                Stroke = new Pen(c, 2) // Red border
            };

            string lowerLimit, uppperLimit;

            (lowerLimit, uppperLimit) = NotamParser.GetNotamVerticalLimits(notamText);

            PointLatLng notamQCoord = new PointLatLng() { Lat = notamQCoordinate.Item1, Lng = notamQCoordinate.Item2 };

            GMapLevelBlock lb = new GMapLevelBlock(notamQCoord, lowerLimit, uppperLimit)
            {
                TextBrush = new SolidBrush(c),
                LowerLimit = lowerLimit,
                UpperLimit = uppperLimit,
                IsVisible = chkDisplayLevels.Checked
            };

            overlay.Polygons.Add(polygon);
            overlay.Markers.Add(lb);

            gMapControl1.Overlays.Add(overlay);

            refreshMap();

            //chkMapElements.Items.Add(notamName, true);
            chkList.Items.Add(new Tuple<string, string>(UID, notamName), true);

            if (notamQCoordinate != null)
            {
                gMapControl1.Position = new GMap.NET.PointLatLng(notamQCoordinate.Item1, notamQCoordinate.Item2);
            }
#if DEBUG
            Debug.WriteLine($"frmMap: Drawn NOTAM {NotamID} (Name: {overlay.Id})");
#endif

            this.Show();

            SavePolys();
        }

        private void chkMapElements_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (changingVisibilityToBatch) return;

            int index = e.Index;
            bool isChecked = e.NewValue == CheckState.Checked;
            if (index < 0 || index >= ((CheckedListBox)sender).Items.Count) return;
            string name = ((Tuple<string, string>)((CheckedListBox)sender).Items[index]).Item1;

            var polys = MapManager.LoadPolys();
            ZoneData poly;

            if (polys != null && (poly = polys.GetPolyFromId(name)) != null)
            {
                string group = name.Substring(0, name.IndexOf('_'));

                poly.Visible = false;

                MapManager.AddZone(name, poly);
            }
            
            var overlay = gMapControl1.Overlays.FirstOrDefault(x => x.Id == name);
            if (overlay != null)
            {
                overlay.IsVisibile = isChecked;
            }
#if DEBUG
            else
            {
                Debug.WriteLine($"frmMap: Overlay with name {name} not found.");
            }
#endif

#if DEBUG
            Debug.WriteLine($"frmMap: {name}; {!isChecked} -> {isChecked}");
#endif
        }

        private void btnAllOn_Click(object sender, EventArgs e)
        {
            if (sender == null || !(sender is Button)) return;

            var parent = ((Button)sender).Parent;

            if (parent == null || !(parent is TabPage)) return;

            var p = parent as TabPage;

            var lstBox = p.Controls.Find($"chk_{p.Text}", false);

            if(lstBox.Length == 0 || !(lstBox[0] is CheckedListBox)) return;

            var chkList = lstBox[0] as CheckedListBox;

            PolyData pd = MapManager.LoadPolys();

            changingVisibilityToBatch = true;

            ZoneData zd;
            GMapOverlay ovr;
            string zoneId;

            for (int i = 0; i < chkList.Items.Count; i++)
            {
                zoneId = (chkList.Items[i] as Tuple<string, string>).Item1;

                zd = pd.GetPolyFromId(zoneId);

                if (zd != null)
                    zd.Visible = true;

                ovr = gMapControl1.Overlays.FirstOrDefault(x => x.Id == zoneId);

                if(ovr != null)
                    ovr.IsVisibile = true;

                chkList.SetItemChecked(i, true);
            }

            MapManager.SaveRawData(pd, true);

            changingVisibilityToBatch = true;

#if DEBUG
            Debug.WriteLine($"frmMap: Saved all zones in {chkList.Name} to Visible");
#endif
        }

        private void btnAllOff_Click(object sender, EventArgs e)
        {
            if (sender == null || !(sender is Button)) return;

            var parent = ((Button)sender).Parent;

            if (parent == null || !(parent is TabPage)) return;

            var p = parent as TabPage;

            var lstBox = p.Controls.Find($"chk_{p.Text}", false);

            if (lstBox.Length == 0 || !(lstBox[0] is CheckedListBox)) return;

            var chkList = lstBox[0] as CheckedListBox;

            PolyData pd = MapManager.LoadPolys();

            changingVisibilityToBatch = true;

            ZoneData zd;
            GMapOverlay ovr;
            string zoneId;

            for (int i = 0; i < chkList.Items.Count; i++)
            {
                zoneId = (chkList.Items[i] as Tuple<string, string>).Item1;

                zd = pd.GetPolyFromId(zoneId);

                if (zd != null)
                    zd.Visible = false;

                ovr = gMapControl1.Overlays.FirstOrDefault(x => x.Id == zoneId);

                if (ovr != null)
                    ovr.IsVisibile = false;

                chkList.SetItemChecked(i, false);
            }

            MapManager.SaveRawData(pd, true);

            changingVisibilityToBatch = false;

#if DEBUG
            Debug.WriteLine($"frmMap: Saved all zones in {chkList.Name} to NOT Visible");
#endif
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

            SavePolys();
            SavePosition();

            GMap.NET.GMaps.Instance.CancelTileCaching();
            gMapControl1.Manager.CancelTileCaching();
            gMapControl1.Overlays.Clear();
            gMapControl1.Dispose();
            printManager.Dispose();

        }

        private void SavePolys()
        {
            PolyData pd = MapManager.LoadPolys() ?? new PolyData();

            foreach (var overlay in gMapControl1.Overlays)
            {
                string upperLimit = null;
                string lowerLimit = null;
                double centerLat = 0, centerLng = 0;

                foreach (var marker in overlay.Markers)
                {
                    if (!(marker is GMapLevelBlock)) continue;

                    var m = marker as GMapLevelBlock;

                    upperLimit = m.UpperLimit;
                    lowerLimit = m.LowerLimit;
                    centerLat = m.Position.Lat;
                    centerLng = m.Position.Lng;
                }

                foreach (var poly in overlay.Polygons)
                {
                    string UID = poly.Name;
                    string groupName = UID.Substring(0, UID.IndexOf('_'));
                    string name = UID.Substring(UID.IndexOf('_') + 1);
                    bool visible = overlay.IsVisibile;
                    Color c = poly.Stroke.Color;
                    List<List<double>> points = new List<List<double>>();

                    foreach (var coords in poly.Points)
                    {
                        points.Add(new List<double>()
                        {
                            coords.Lat,
                            coords.Lng,
                        });
                    }

                    if (!pd.Groups.ContainsKey(groupName))
                        pd.Groups.Add(groupName, new Group() { DefaultColor = c, Name = groupName });

                    var zd = new ZoneData()
                    {
                        Points = points,
                        Color = new List<int>() { c.A, c.R, c.G, c.B },
                        UpperLimit = upperLimit,
                        LowerLimit = lowerLimit,
                        CenterCoordinate = (centerLat, centerLng),
                        Visible = visible
                    };

                    if (pd.Groups[groupName].Polygons.ContainsKey(name))
                        pd.Groups[groupName].Polygons[name] = zd;
                    else
                        pd.Groups[groupName].Polygons.Add(name, zd);
                }
            }

            MapManager.SaveRawData(pd, true);
        }

        private CheckedListBox GetCheckedListForGroup(string groupName, bool createIfDoesNotExist = false)
        {
            var res = listTabControl.Controls.Find($"chk_{groupName}", true);

            if (res.Length == 0 || !(res[0] is CheckedListBox))
            {
                if (createIfDoesNotExist)
                {
                    addTabForGroup(groupName);
                    res = listTabControl.Controls.Find($"chk_{groupName}", true);

                    return res[0] as CheckedListBox;
                }
                return null;
            }

            return res[0] as CheckedListBox;
        }

        private TabPage GetTabPageForGroup(string groupName, bool createIfDoesNotExist = false)
        {
            var res = listTabControl.Controls.Find($"tab_{groupName}", true);

            if (res.Length == 0 || !(res[0] is TabPage))
            {
                if (createIfDoesNotExist)
                {
                    addTabForGroup(groupName);
                    res = listTabControl.Controls.Find($"tab_{groupName}", true);

                    return res[0] as TabPage;
                }
                return null;
            }

            return res[0] as TabPage;
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
                        PolyData polydata = JsonConvert.DeserializeObject<PolyData>(json);

                        MapManager.AddPolys(polydata, true);

                        // this is the shitties code i've seen in a while but it works for now.
                        foreach(var g in polydata.Groups)
                        {
                            if(g.Value == null)
                            {
                                var tab = GetTabPageForGroup(g.Key);

                                if (tab != null)
                                {
                                    var list = GetCheckedListForGroup(g.Key);

                                    if(list != null)
                                    {
                                        foreach (var item in list.Items)
                                        {
                                            gMapControl1.Overlays.Remove(gMapControl1.Overlays.First(ov => ov.Id == ((Tuple<string, string>)item).Item1));
                                        }
                                    }

                                    listTabControl.Controls.Remove(tab);
                                }

                                continue;
                            }

                            changingVisibilityToBatch = true;

                            var chkList = GetCheckedListForGroup(g.Key, true);

                            foreach (var z in g.Value.Polygons)
                            {
                                if (z.Value == null || z.Value.Points == null)
                                {
                                    // ovo je pakao -> $"{g.Key}_{z.Key}". Generisem UID ovde isto jer ne postoji u memoriji u tom trenutku. Ne svidja mi se ali za sad radi.
                                    if (chkList.Items.Contains(new Tuple<string, string>($"{g.Key}_{z.Key}", z.Key)))
                                    {
                                        chkList.Items.Remove(new Tuple<string, string>($"{g.Key}_{z.Key}", z.Key));
                                        gMapControl1.Overlays.Remove(gMapControl1.Overlays.First(ov => ov.Id == $"{g.Key}_{z.Key}")); // First jer svakako crash ako koristim FirstOrDefault i dobijem null
                                    }
                                    continue;
                                }

                                if (chkList.Items.Contains(new Tuple<string, string>(z.Value.UID, z.Key)))
                                {
                                    gMapControl1.Overlays.Remove(gMapControl1.Overlays.First(ov => ov.Id == z.Value.UID)); // First jer svakako crash ako koristim FirstOrDefault i dobijem null
                                }
                                else
                                {
                                    chkList.Items.Add(new Tuple<string, string>(z.Value.UID, z.Key), true);
                                }

                                if (z.Value == null || z.Value.Points == null) continue;


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
                                GMap.NET.WindowsForms.GMapPolygon polygon = new GMap.NET.WindowsForms.GMapPolygon(points, z.Value.UID);
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

                                GMapLevelBlock lb = new GMapLevelBlock(new PointLatLng(z.Value.CenterCoordinate.Item1, z.Value.CenterCoordinate.Item2), z.Value.LowerLimit, z.Value.UpperLimit)
                                {
                                    TextBrush = new SolidBrush(c),
                                    IsVisible = chkDisplayLevels.Checked
                                };

                                var o = new GMap.NET.WindowsForms.GMapOverlay(z.Value.UID);

                                o.Polygons.Add(polygon);
                                o.Markers.Add(lb);

                                o.IsVisibile = z.Value.Visible;

                                int index = chkList.Items.IndexOf(new Tuple<string, string>(z.Value.UID, z.Key));
                                if(index != -1)
                                    chkList.SetItemChecked(index, z.Value.Visible);

                                gMapControl1.Overlays.Add(o);
                            }
                        }

                        refreshMap();

                        MessageBox.Show("Mape su uspešno učitane i sačuvane", "Učitane mape", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine($"frmMap: Error loading polygons from file {ofd.FileName}: {ex.ToString()}");
#endif
                        MessageBox.Show($"Greška pri učitavanju koordinata: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        changingVisibilityToBatch = false;
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printManager.ZoomLevel = (int)numZoomLevel.Value;
            this.Enabled = false;
            printManager.DoPrint();
        }

        private void gMapControl1_OnMapZoomChanged()
        {
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            if (gMapControl1 != null)
            {
                stsZoomLevel.Text = $"Zoom: {gMapControl1.Zoom}";
                stsPosition.Text = $"Position: {gMapControl1.Position.Lat}, {gMapControl1.Position.Lng}";
            }
        }

        private void gMapControl1_OnPositionChanged(GMap.NET.PointLatLng point)
        {
            UpdateStatusBar();
        }

        private void obrišToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(ctxChkList.SourceControl is CheckedListBox)) return;

            if(
                ctxChkList.SourceControl.Name != "chk_NOTAM" &&
                ctxChkList.SourceControl.Name != "chk_Custom"
                ) return;

            var chkList = ctxChkList.SourceControl as CheckedListBox;

            if (!(chkList.SelectedItem is Tuple<string, string> selected)) return;

            chkList.Items.Remove(selected);

            string group = selected.Item1.Substring(0, selected.Item1.IndexOf('_'));
            string name = selected.Item2;

            PolyData pd = new PolyData()
            {
                Groups = new Dictionary<string, Group>()
                {
                    {
                        group, new Group(){
                            Polygons = new Dictionary<string, ZoneData>()
                            {
                                {
                                    name, null 
                                }
                            }
                        }
                    }
                }
            };

            MapManager.AddPolys(pd);

            gMapControl1.Overlays.Remove(gMapControl1.Overlays.First(o => o.Id == selected.Item1));

            refreshMap();
        }

        private void upravljajZonamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditPolys frmEditPolys = new frmEditPolys(this);
            frmEditPolys.ShowDialog();
        }

        private void chkDisplayLevels_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.displayVerticalBoundaries = chkDisplayLevels.Checked;
            Settings.Default.Save();

            foreach (var overlay in gMapControl1.Overlays)
            {
                foreach (var marker in overlay.Markers)
                {
                    if (marker is GMapLevelBlock levelBlock)
                    {
                        levelBlock.IsVisible = chkDisplayLevels.Checked;
                    }
                }
            }
        }

        private void promeniBojuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(ctxChkList.SourceControl is CheckedListBox)) return;

            var chkList = ctxChkList.SourceControl as CheckedListBox;

            if (chkList.SelectedItem == null) return;

            if (!(chkList.SelectedItem is Tuple<string, string> selected)) return;

            GMapOverlay overlay = gMapControl1.Overlays.FirstOrDefault(x => x.Id == selected.Item1);

            if(overlay == null) return;

            GMapPolygon polygon = overlay.Polygons.FirstOrDefault(p => p.Name == selected.Item1);

            if(polygon == null) return;

            var markers = overlay.Markers.OfType<GMapLevelBlock>().ToList();

            string group = selected.Item1.Substring(0, selected.Item1.IndexOf('_'));

            var polys = MapManager.LoadPolys();

            if (polys == null) return;

            Color default_color = polys.Groups[group].DefaultColor;

            Color c = polygon.Stroke.Color;

            using(ColorDialog cd = new ColorDialog())
            {
                cd.Color = c;
                cd.CustomColors = new int[] { ColorTranslator.ToWin32(default_color) };

                if (cd.ShowDialog() == DialogResult.OK)
                    c = cd.Color;
                else
                    return;
            }

            polygon.Stroke.Color = c;

            foreach (var marker in markers)
            {
                marker.TextBrush = new SolidBrush(c);
            }

            var allZones = polys.AllZones;
            
            var zd = allZones.FirstOrDefault(g => g.Key == selected.Item1);

            if (zd.Key == null || zd.Value == null)
                return;

            zd.Value.Color = new List<int>() { c.A, c.R, c.G, c.B };

            Group gp = new Group();
            gp.Polygons.Add(selected.Item2, zd.Value);

            PolyData pd = new PolyData();

            pd.Groups.Add(group, gp);

            MapManager.AddPolys(pd);

            refreshMap();
        }

        private void obrišiSveUListiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(ctxChkList.SourceControl is CheckedListBox)) return;

            if (
                ctxChkList.SourceControl.Name != "chk_NOTAM" &&
                ctxChkList.SourceControl.Name != "chk_Custom"
                ) return;

            var chkList = ctxChkList.SourceControl as CheckedListBox;

            var pd = new PolyData();

            string group, name;

            Tuple<string, string> item;

            for(int i = chkList.Items.Count - 1; i>= 0; i--)
            {
                item = chkList.Items[i] as Tuple<string, string>;

                chkList.Items.RemoveAt(i);

                group = item.Item1.Substring(0, item.Item1.IndexOf('_'));
                name = item.Item2;

                if (!pd.Groups.ContainsKey(group))
                    pd.Groups.Add(group, new Group());
                    
                pd.Groups[group].Polygons.Add(name, null);

                gMapControl1.Overlays.Remove(gMapControl1.Overlays.First(o => o.Id == item.Item1));
            }

            MapManager.AddPolys(pd);

            refreshMap();
        }

        private void gMapControl1_DoubleClick(object sender, EventArgs e)
        {
            if (movingCenterNotamId == null) return;

            Point local = gMapControl1.PointToClient(Control.MousePosition);

            PointLatLng position = gMapControl1.FromLocalToLatLng(local.X, local.Y);

            var overlay = gMapControl1.Overlays.FirstOrDefault(x => x.Id == movingCenterNotamId);
            if (overlay != null)
            {
                if(overlay.Markers.Count > 0 && overlay.Markers[0] is GMapLevelBlock block)
                {
                    block.Position = position;
                }
            }

            stsStatus.Visible = false;
            pnlMapBorder.BackColor = SystemColors.Control;

#if DEBUG
            Debug.WriteLine($"frmMap: Set new center coordinate for {movingCenterNotamId} to {position.Lat}, {position.Lng}");
#endif

            movingCenterNotamId = null;

            refreshMap();

        }

        private void pomeriPozicijuVertikalnihGranicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(ctxChkList.SourceControl is CheckedListBox)) return;

            if (
                ctxChkList.SourceControl.Name != "chk_NOTAM" &&
                ctxChkList.SourceControl.Name != "chk_Custom"
                ) return;

            var chkList = ctxChkList.SourceControl as CheckedListBox;

            if (!(chkList.SelectedItem is Tuple<string, string> selected)) return;

            var pd = MapManager.LoadPolys();

            movingCenterNotamId = pd.AllZones[selected.Item1].UID;

            pnlMapBorder.BackColor = Color.Red;

            stsStatus.ForeColor = Color.Red;
            stsStatus.Text = "Dupli klik na mapu kako biste pomerili poziciju vertikalnih granica";
            stsStatus.Visible = true;
        }

        private void dodajKaoCustomZonuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(ctxChkList.SourceControl is CheckedListBox)) return;

            if (
                ctxChkList.SourceControl.Name == "chk_NOTAM" ||
                ctxChkList.SourceControl.Name == "chk_Custom"
                ) return;

            var chkList = ctxChkList.SourceControl as CheckedListBox;

            if (!(chkList.SelectedItem is Tuple<string, string> selected)) return;

            string uLimit, lLimit;

            using(var frm = new frmLevelBlockInput())
            {
                frm.Title = "Unesite vertikalne granice";

                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                uLimit = frm.UpperLimit;
                lLimit = frm.LowerLimit;
            }

            var pd = MapManager.LoadPolys();

            var zd = pd.AllZones[selected.Item1];

            if(
                zd.Points.Count == 0 || 
                zd.Points[0].Count != 2||
                zd.Color == null ||
                zd.Color.Count != 4
                ) 
                return;

            string uid = $"Custom_{DateTime.Now.ToString("yyMMddHHmmss")} {zd.Name}";
            string name = $"{DateTime.Now.ToString("yyMMddHHmmss")} {zd.Name}";

            var overlay = gMapControl1.Overlays.FirstOrDefault(x => x.Id == uid);

            if (overlay != null)
                return;

            var lat = zd.CenterCoordinate.Item1 == 0 ? zd.Points[0][0] : zd.CenterCoordinate.Item1;
            var lon = zd.CenterCoordinate.Item2 == 0 ? zd.Points[0][1] : zd.CenterCoordinate.Item2;

            var newZd = new ZoneData
            {
                UID = uid,
                Name = name,
                CenterCoordinate = (lat, lon),
                Color = zd.Color,
                LowerLimit = lLimit,
                UpperLimit = uLimit,
                Points = zd.Points,
                Visible = zd.Visible
            };

            MapManager.AddZone(uid, newZd);

            overlay = new GMapOverlay(uid);

            var color = Color.FromArgb(zd.Color[0], zd.Color[1], zd.Color[2], zd.Color[3]);

            var marker = new GMapLevelBlock(new PointLatLng(newZd.CenterCoordinate.Item1, newZd.CenterCoordinate.Item2), newZd.LowerLimit, newZd.UpperLimit, new SolidBrush(color));

            var points = new List<PointLatLng>();

            foreach ( var point in zd.Points )
            {
                if (point.Count != 2) return;

                points.Add(new PointLatLng(point[0], point[1]));
            }

            var poly = new GMapPolygon(points, uid)
            {
                Fill = Brushes.Transparent, // No fill color
                Stroke = new Pen(color, 2) // Red border
            };

            overlay.Polygons.Add(poly);
            overlay.Markers.Add(marker);

            var chk = GetCheckedListForGroup("Custom", true);

            gMapControl1.Overlays.Add(overlay);

            int index = chk.Items.Add(new Tuple<string, string>(uid, name), true);

            listTabControl.SelectTab((TabPage)chk.Parent);

            chk.SelectedIndex = index;

            refreshMap();
        }
    }
}
