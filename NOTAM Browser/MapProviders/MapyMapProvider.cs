using System;
using GMap.NET;
using GMap.NET.MapProviders;
using NOTAM_Browser.Properties;

namespace NOTAM_Browser.MapProviders
{
    internal class MapyMapProvider : GMapProvider
    {
        public static readonly MapyMapProvider Instance;

        private readonly string APIKEY = Settings.Default.mapyApiKey ?? "";

        public override string Name { get; } = "Mapy";

        public override PureProjection Projection => GMap.NET.Projections.MercatorProjection.Instance;

        private readonly Guid id = new Guid("A04DBBD2-4727-4D43-9A15-3ACF1EDC6DAF");
        public override Guid Id => id;

        static MapyMapProvider()
        {
            Instance = new MapyMapProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format("https://api.mapy.com/v1/maptiles/basic/256/{0}/{1}/{2}?apikey={3}&lang=en",
                zoom, pos.X, pos.Y, APIKEY);
            return GetTileImageUsingHttp(url);
        }

        public override GMapProvider[] Overlays => new GMapProvider[] { this };
    }
}
