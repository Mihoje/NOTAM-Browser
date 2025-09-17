using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NOTAM_Browser.MapTypes
{
    internal class GMapLevelBlock : GMapMarker
    {
        public string LowerLimit { get; set; }
        public string UpperLimit { get; set; }
        public Brush TextBrush { get; set; } = Brushes.Black;
        public Pen OutlinePen { get; set; } = new Pen(Color.Black, 3);
        public FontFamily FontFamily { get; set; } = FontFamily.GenericSansSerif;
        public int FontHeightInMeters { get; set; } = 2000;

        public GMapLevelBlock(PointLatLng pos, string lowerLimit, string upperLimit) : base(pos)
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        public GMapLevelBlock(PointLatLng pos, string lowerLimit, string upperLimit, Brush textBrush) : this(pos, lowerLimit, upperLimit)
        {
            TextBrush = textBrush;
        }

        public GMapLevelBlock(PointLatLng pos, string lowerLimit, string upperLimit, Brush textBrush, FontFamily textFontFamily) : this(pos, lowerLimit, upperLimit, textBrush)
        {
            FontFamily = textFontFamily;
        }

        public GMapLevelBlock(PointLatLng pos, string lowerLimit, string upperLimit, Brush textBrush, FontFamily textFontFamily, int fontHeightInMeters) : this(pos, lowerLimit, upperLimit, textBrush, textFontFamily)
        {
            FontHeightInMeters = fontHeightInMeters;
        }

        public override void OnRender(Graphics g)
        {
            if (
                Overlay == null || 
                Overlay.Control == null ||
                UpperLimit == null ||
                LowerLimit == null
                )
                return;

            GMapControl control = this.Overlay.Control;

            double metersPerPixel = control.MapProvider.Projection.GetGroundResolution(
                (int)control.Zoom,
                Position.Lat
            );

            float pixelHeight = (float)(FontHeightInMeters / metersPerPixel);

            if (pixelHeight < 3) return;
            
            using(Font f = new Font(FontFamily, pixelHeight, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                SizeF lowerTextSize = g.MeasureString(LowerLimit, f);
                SizeF upperTextSize = g.MeasureString(UpperLimit, f);

                int width = (int)Math.Ceiling(Math.Max(lowerTextSize.Width, upperTextSize.Width));

                // Draw the line between
                g.DrawLine(
                    new Pen(TextBrush) { Width = (float)Math.Ceiling(10.0f / metersPerPixel) },
                    new Point(LocalPosition.X - width / 2, LocalPosition.Y),
                    new Point(LocalPosition.X + width / 2, LocalPosition.Y)
                );

                // Draw the uppper limit text
                /*g.DrawString(
                    UpperLimit,
                    f,
                    TextBrush,
                    new Point((int)(LocalPosition.X - upperTextSize.Width / 2), (int)(LocalPosition.Y - upperTextSize.Height - 100 / metersPerPixel)) // 100m on ground buffer between the text and the line
                );*/

                float lowerTextX = LocalPosition.X - lowerTextSize.Width / 2;
                float lowerTextY = (float)(LocalPosition.Y + 100 / metersPerPixel); // 100m on ground buffer from the line to the text

                g.DrawPath(OutlinePen, GetStringPath(LowerLimit, f, lowerTextX, lowerTextY));
                g.FillPath(TextBrush, GetStringPath(LowerLimit, f, lowerTextX, lowerTextY));


                // Draw the lower limit text
                /*g.DrawString(
                    LowerLimit,
                    f,
                    TextBrush,
                    new Point((int)(LocalPosition.X - lowerTextSize.Width / 2), (int)(LocalPosition.Y + 100 / metersPerPixel)) // 100m on ground buffer from the line to the text
                );*/

                float upperTextX = LocalPosition.X - upperTextSize.Width / 2;
                float upperTextY = (float)(LocalPosition.Y - upperTextSize.Height - 100 / metersPerPixel); // 100m on ground buffer from the line to the text

                g.DrawPath(OutlinePen, GetStringPath(UpperLimit, f, upperTextX, upperTextY));
                g.FillPath(TextBrush, GetStringPath(UpperLimit, f, upperTextX, upperTextY));
            }

        }

        private GraphicsPath GetStringPath(string text, Font font, float x, float y)
        {
            var path = new GraphicsPath();

            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new PointF(x, y), StringFormat.GenericDefault);

            return path;
        }
    }
}
