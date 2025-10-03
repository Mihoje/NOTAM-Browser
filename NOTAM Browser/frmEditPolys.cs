using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NOTAM_Browser
{
    public partial class frmEditPolys : Form
    {
        private frmMap parent;
        
        public frmEditPolys(frmMap parent)
        {
            InitializeComponent();
            this.parent = parent;

            toolTip1.SetToolTip(txtPolyCoordinates, "Unesi koordinate iz AUP-a. Program će sam prepoznati i ispisati prepoznate koordinate u polju desno.\nUkoliko želiš da obrišeš zonu iz 'Custom' grupe, samo unesi naziv zone bez ijedne koordinate i klikni Dodaj poligon.");
        }

        private void txtPolyCoordinates_TextChanged(object sender, EventArgs e)
        {
            string fullText = txtPolyCoordinates.Text;

            var coords = NotamParser.ParseCoordinates(fullText, true);

            string output = "";

            foreach (var c in coords)
            {
                output += c.ToFullString() + Environment.NewLine;
            }

            txtPolyCoordinatesConverted.Text = output;
        }

        private void btnAddPoly_Click(object sender, EventArgs e)
        {
            string zoneName = txtPolygonName.Text.Trim();
            if (string.IsNullOrEmpty(zoneName))
            {
                MessageBox.Show("Moraš uneti naziv zone!", "Greška");
                return;
            }

            string fullText = txtPolyCoordinates.Text.Trim();

            var coords = NotamParser.ParseCoordinates(fullText, true);

            var points = new List<List<double>>();

            foreach (var c in coords)
            {
                points.Add(new List<double>()
                {
                    c.Latitude,
                    c.Longitude
                });
            }

            var poly = new ZoneData();

            if(points.Count == 0)
            {
                poly = null;
            } 
            else
            {
                poly.Points = points;

                Color c = pnlPolyColor.BackColor;

                poly.Color = new List<int>() { c.A, c.R, c.G, c.B };

                poly.LowerLimit = txtPolyLowerLimit.Text.Trim();
                poly.UpperLimit = txtPolyUpperLimit.Text.Trim();

                poly.CenterCoordinate = (poly.Points[0][0], poly.Points[0][1]);
                
                poly.Visible = true;
            }

            var pd = new PolyData()
            {
                Groups = new Dictionary<string, Group>()
                {
                    {
                        "Custom", new Group()
                        {
                            DefaultColor = Color.Red,
                            Polygons = new Dictionary<string, ZoneData>()
                            {
                                { zoneName,  poly }
                            }
                        }
                    }
                }
            };

            MapManager.AddPolys(pd);

            parent.LoadPolys();

            txtPolyCoordinates.Text = "";
            txtPolygonName.Text = "";
            txtPolyLowerLimit.Text = "";
            txtPolyLowerLimit.Text = "";
            txtPolyCoordinatesConverted.Text = "";
        }

        private void pnlColor_Click(object sender, EventArgs e)
        {
            if (!(sender is Panel)) return;

            Panel pnl = (Panel)sender;

            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = pnl.BackColor;
                cd.AllowFullOpen = true;

                if (cd.ShowDialog() == DialogResult.OK)
                {
                    pnl.BackColor = cd.Color;
                }
            }
        }

        private void txtCircleCoordinate_TextChanged(object sender, EventArgs e)
        {
            string fullText = txtCircleCoordinate.Text;

            var coords = NotamParser.ParseCoordinates(fullText, true);
            if (coords.Count == 0)
                return;
            else if (coords.Count > 1)
            {
                txtCircleCoordinateConverted.Text = "Više od jedne koordinate nađeno!";
            }
            else
                txtCircleCoordinateConverted.Text = coords[0].ToFullString();
        }

        private void btnAddCircle_Click(object sender, EventArgs e)
        {
            string zoneName = txtCircleName.Text.Trim();

            if (string.IsNullOrEmpty(zoneName))
            {
                MessageBox.Show("Moraš uneti naziv zone!", "Greška");
                return;
            }

            string fullText = txtCircleCoordinate.Text.Trim();

            var coords = NotamParser.ParseCoordinates(fullText, true);

            var points = new List<List<double>>();

            var poly = new ZoneData();

            if (coords.Count > 1)
            {
                MessageBox.Show("Prepoznato je više od jedne koordinate. Ne sme biti više od jedne koordinate.", "Greška");
                return;
            }
            else if (coords.Count == 0)
            {
                poly = null; 
            }
            else
            {
                coords = NotamParser.GenerateCircleNM(coords[0], (double)nudCircleRadius.Value);

                foreach (var coor in coords)
                {
                    points.Add(new List<double>()
                    {
                        coor.Latitude,
                        coor.Longitude
                    });
                }

                poly.Points = points;

                Color c = pnlCircleColor.BackColor;

                poly.Color = new List<int>() { c.A, c.R, c.G, c.B };

                poly.LowerLimit = txtCircleLowerLimit.Text.Trim();
                poly.UpperLimit = txtCircleUpperLimit.Text.Trim();
                poly.CenterCoordinate = (poly.Points[0][0], poly.Points[0][1]);

                poly.Visible = true;
            }

            var pd = new PolyData()
            {
                Groups = new Dictionary<string, Group>()
                {
                    {
                        "Custom", new Group()
                        {
                            DefaultColor = Color.Red,
                            Polygons = new Dictionary<string, ZoneData>()
                            {
                                { zoneName,  poly }
                            }
                        }
                    }
                }
            };

            MapManager.AddPolys(pd);

            parent.LoadPolys();

            txtCircleCoordinate.Text = "";
            txtCircleName.Text = "";
            txtCircleLowerLimit.Text = "";
            txtCircleUpperLimit.Text = "";
            txtPolyCoordinatesConverted.Text = "";
        }
    }
}
