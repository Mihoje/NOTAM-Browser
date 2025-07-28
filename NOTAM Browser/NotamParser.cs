using GMap.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NOTAM_Browser
{
    internal class NotamParser
    {
        private static readonly Regex coordinateRegex = new Regex(
    @"(?i)\b(?:(\d{2})(\d{2})(\d{2})?([.,]\d+)?\s*([NS])\s*(\d{3})(\d{2})(\d{2})?([.,]\d+)?\s*([EW])|" +                  // Format 1
    @"(\d{1,2})[°\s]*?(\d{1,2}(?:[.,]\d+)?)[\'′’]?\s*([NS])[, ]+(\d{1,3})[°\s]*?(\d{1,2}(?:[.,]\d+)?)[\'′’]?\s*([EW])|" +  // Format 2
    @"([NS])\s*([+-]?\d{1,2}(?:[.,]\d+)?)[, ]+([EW])\s*([+-]?\d{1,3}(?:[.,]\d+)?))|" +                                    // Format 3
    @"(\d{2})(\d{2})([NS])(\d{3})(\d{2})([EW])\b",                                                                         // Format 4: DDMMNDDDMME
    RegexOptions.Compiled);


        private static bool IsNotamAboutAirsapce(string notamText)
        {
            Regex r = new Regex(@"Q\) *[A-Z]{4}\/Q([A].|.[A])");
            Match m = r.Match(notamText);

            return m.Success;
        }

        public static List<LatLon> ParseCoordinates(string input, bool raw = false)
        {
            if(!IsNotamAboutAirsapce(input) && !raw) return new List<LatLon>();

            int startIndex = raw ? 0 : input.IndexOf("E)");
            int endIndex = raw ? input.Length - 1 : input.LastIndexOf("F)");

            if(endIndex == -1)
            {
                endIndex = input.Length; // If F) is not found, consider the end of the string
            }

            if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
            {
                return new List<LatLon>(); // No valid coordinates found
            }

            //input = input.Substring(startIndex, endIndex - startIndex).Trim();

            var matches = coordinateRegex.Matches(input);
            var radiusMeters = ExtractRadiusInMeters(input);
            var result = new List<LatLon>();

            foreach (Match match in matches)
            {
                if(match.Index < startIndex || match.Index > endIndex)
                {
                    continue; // Skip matches outside the E) to F) range
                }
#if DEBUG
                Debug.WriteLine($"NotamParser: Found match: {match.Value} at index {match.Index}");
#endif
                if (match.Groups[1].Success) // Format: DDMM(M)SS(S)H DDDMM(M)SS(S)H
                {
                    double lat = ToDecimalDegrees(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value, match.Groups[5].Value);
                    double lon = ToDecimalDegrees(match.Groups[6].Value, match.Groups[7].Value, match.Groups[8].Value, match.Groups[9].Value, match.Groups[10].Value);
                    result.Add(new LatLon { Latitude = lat, Longitude = lon, ConvertedString = match.Value, Index = match.Index });
                }
                else if (match.Groups[11].Success) // Format: DD°MM.mm'H DDD°MM.mm"H
                {
                    double lat = ToDecimalDegrees(match.Groups[11].Value, match.Groups[12].Value, "0", null, match.Groups[13].Value);
                    double lon = ToDecimalDegrees(match.Groups[14].Value, match.Groups[15].Value, "0", null, match.Groups[16].Value);
                    result.Add(new LatLon { Latitude = lat, Longitude = lon, ConvertedString = match.Value, Index = match.Index });
                }
                else if (match.Groups[17].Success) // Format: H DD.dddd H DDD.dddd
                {
                    double lat = double.Parse(match.Groups[18].Value.Replace(',', '.'));
                    double lon = double.Parse(match.Groups[20].Value.Replace(',', '.'));
                    if (match.Groups[17].Value == "S") lat = -lat;
                    if (match.Groups[19].Value == "W") lon = -lon;
                    result.Add(new LatLon { Latitude = lat, Longitude = lon, ConvertedString = match.Value, Index = match.Index });
                }
                else if (match.Groups[21].Success) // Format: DDMMNDDDMME
                {
                    string latDeg = match.Groups[21].Value;
                    string latMin = match.Groups[22].Value;
                    string latHem = match.Groups[23].Value;

                    string lonDeg = match.Groups[24].Value;
                    string lonMin = match.Groups[25].Value;
                    string lonHem = match.Groups[26].Value;

                    double lat = ToDecimalDegrees(latDeg, latMin, "0", null, latHem);
                    double lon = ToDecimalDegrees(lonDeg, lonMin, "0", null, lonHem);

                    result.Add(new LatLon { Latitude = lat, Longitude = lon, ConvertedString = match.Value, Index = match.Index });
                }

                if (radiusMeters.HasValue)
                {
                    // Generate a circle around the coordinate if radius is specified
                    var circleCoords = GenerateCircle(result.Last(), radiusMeters.Value);
                    circleCoords.First().ConvertedString = result.Last().ConvertedString; // Keep the original point's string
                    circleCoords.First().Index = result.Last().Index; // Keep the original point's index

                    result.RemoveAt(result.Count - 1); // Remove the original point
                    result.AddRange(circleCoords);
                }
            }

            return result;
        }

        private static double ToDecimalDegrees(string deg, string min, string sec, string fraction, string hemisphere)
        {
            double degrees = double.Parse(deg);
            double minutes = string.IsNullOrEmpty(min) ? 0 : double.Parse(min);
            double seconds = string.IsNullOrEmpty(sec) ? 0 : double.Parse(sec);
            double decimalPart = string.IsNullOrEmpty(fraction) ? 0 : double.Parse(fraction.Replace(',', '.'));

            if (!string.IsNullOrEmpty(fraction))
            {
                // Apply fraction to the smallest unit available
                if (!string.IsNullOrEmpty(sec)) seconds += decimalPart;
                else if (!string.IsNullOrEmpty(min)) minutes += decimalPart;
                else degrees += decimalPart;
            }

            double result = degrees + (minutes / 60) + (seconds / 3600);

            if (hemisphere.ToUpper() == "S" || hemisphere.ToUpper() == "W")
                result *= -1;

            return result;
        }

        private static List<LatLon> GenerateCircle(LatLon center, double radiusMeters, int points = 36)
        {
            var coords = new List<LatLon>();
            var lat = center.Latitude * Math.PI / 180.0;
            var lon = center.Longitude * Math.PI / 180.0;
            var earthRadius = 6371000.0;

            for (int i = 0; i < points; i++)
            {
                double angle = 2 * Math.PI * i / points;
                double dx = radiusMeters * Math.Cos(angle);
                double dy = radiusMeters * Math.Sin(angle);

                double newLat = lat + (dy / earthRadius);
                double newLon = lon + (dx / (earthRadius * Math.Cos(lat)));

                coords.Add(new LatLon
                {
                    Latitude = newLat * 180.0 / Math.PI,
                    Longitude = newLon * 180.0 / Math.PI
                });
            }

            return coords;
        }

        public static double? ExtractRadiusInMeters(string input)
        {
            var radiusMatch = Regex.Match(input, @"\b(\d+(?:[.,]\d+)?)\s*(NM|KM|M)\b", RegexOptions.IgnoreCase);
            if (!radiusMatch.Success)
                return null;

            double value = double.Parse(radiusMatch.Groups[1].Value.Replace(',', '.'));
            string unit = radiusMatch.Groups[2].Value.ToUpper();

            switch (unit)
            {
                case "NM":
                    return value * 1852; // Nautical miles to meters
                case "KM":
                    return value * 1000; // Kilometers to meters
                case "M":
                    return value; // Meters
                default:
                    return null; // Unknown unit
            }
        }

        public static List<PointLatLng> ConvertCoordinatesForMap(List<LatLon> input)
        {
            return input.Select(ll => new PointLatLng(ll.Latitude, ll.Longitude)).ToList();
        }

        public static Tuple<double, double> GetNotamQCoordinate(Notams nos, string NotamID)
        {
            if(string.IsNullOrEmpty(NotamID) || !nos.CurrentNotams.ContainsKey(NotamID))
            {
#if DEBUG
                Debug.WriteLine($"Notams: GetNotamQCoordinate: NotamID {NotamID} not found in current NOTAMs.");
#endif
                return null; // Not found
            }

            return GetNotamQCoordinate(nos.CurrentNotams[NotamID]);
        }

        public static Tuple<double, double> GetNotamQCoordinate(string notamText)
        {
            int startIndex = notamText.IndexOf("Q)");
            int endIndex = notamText.IndexOf("A)", startIndex);

            if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
            {
#if DEBUG
                Debug.WriteLine($"Notams: GetNotamQCoordinate: Invalid NOTAM format or no coordinates found in text.");
#endif
                return null; // No valid coordinates found
            }

            string[] parts = notamText.Substring(startIndex + 2, endIndex - startIndex - 2).Split('/');

            if (parts.Length < 8)
            {
#if DEBUG
                Debug.WriteLine($"Notams: GetNotamQCoordinate: Invalid NOTAM format, expected at least 8 parts but got {parts.Length}.");
#endif
                return null; // Invalid format
            }

            string coordinatePart = parts[7].Trim();


            var c = ParseCoordinates(coordinatePart, true);

            if(c.Count == 0)
            {
#if DEBUG
                Debug.WriteLine($"Notams: GetNotamQCoordinate: No valid coordinates found in NOTAM Q part: {coordinatePart}");
#endif
                return null; // No valid coordinates found
            }

            double latitude = 0, longitude = 0;

            latitude = c[0].Latitude;
            longitude = c[0].Longitude;
            return new Tuple<double, double>(latitude, longitude);
        }
    }

    internal class LatLon
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ConvertedString { get; set; }
        public int ConvertedStringLength => ConvertedString?.Length ?? -1;
        public int Index { get; set; }
        public override string ToString() => $"{Latitude}, {Longitude}";
    }
}
