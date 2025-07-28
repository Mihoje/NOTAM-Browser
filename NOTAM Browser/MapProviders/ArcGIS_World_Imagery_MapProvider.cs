using System;
using GMap.NET;
using GMap.NET.MapProviders;

namespace NOTAM_Browser.MapProviders
{
    internal class ArcGIS_World_Imagery_MapProvider : GMapProvider
    {
        public static readonly ArcGIS_World_Imagery_MapProvider Instance;

        public override string Name { get; } = "ArcGIS World Imagery";

        public override PureProjection Projection => GMap.NET.Projections.MercatorProjection.Instance;

        private readonly Guid id = new Guid("3408da4d-340e-4594-a3b4-08bcf0c436c4");
        public override Guid Id => id;

        static ArcGIS_World_Imagery_MapProvider()
        {
            Instance = new ArcGIS_World_Imagery_MapProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format("http://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer/tile/{0}/{1}/{2}",
                zoom, pos.Y, pos.X);
            return GetTileImageUsingHttp(url);
        }

        public override GMapProvider[] Overlays => new GMapProvider[] { this };
    }
}
