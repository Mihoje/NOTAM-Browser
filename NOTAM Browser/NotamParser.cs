using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;


#if DEBUG
using System.Diagnostics;
#endif

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


        /// <summary>
        /// Determines whether the provided NOTAM text is related to airspace.
        /// </summary>
        /// <param name="notamText">The text of the NOTAM to analyze. Cannot be null or empty.</param>
        /// <returns><see langword="true"/> if the NOTAM text contains information related to airspace; otherwise, <see
        /// langword="false"/>.</returns>
        private static bool IsNotamAboutAirsapce(string notamText)
        {
            Regex r = new Regex(@"Q\) *[A-Z]{4}\/Q([A].|.[A])");
            Match m = r.Match(notamText);

            return m.Success;
        }

        /// <summary>
        /// Parses a NOTAM text containing coordinate data and extracts a list of latitude and longitude points.
        /// </summary>
        /// <remarks>This method supports multiple coordinate formats, including degrees, minutes, and
        /// seconds, as well as decimal degrees. If a radius is specified within the input, the method generates a
        /// circular area around the last parsed coordinate.</remarks>
        /// <param name="input">The input string containing coordinate information. This may include various formats of latitude and
        /// longitude data.</param>
        /// <param name="raw">A boolean value indicating whether the input should be processed in its entirety or only the E) part of the NOTAM. 
        /// If <see langword="true"/>, the entire input is parsed; otherwise, only the portion between
        /// specific markers is considered.</param>
        /// <returns>A list of <see cref="LatLon"/> objects representing the parsed coordinates. Returns an empty list if no
        /// valid coordinates are found.</returns>
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
                    double lat = double.Parse(match.Groups[18].Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                    double lon = double.Parse(match.Groups[20].Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
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

                /*if (radiusMeters.HasValue)
                {
                    // Generate a circle around the coordinate if radius is specified
                    var circleCoords = GenerateCircle(result.Last(), radiusMeters.Value);
                    circleCoords.First().ConvertedString = result.Last().ConvertedString; // Keep the original point's string
                    circleCoords.First().Index = result.Last().Index; // Keep the original point's index

                    result.RemoveAt(result.Count - 1); // Remove the original point
                    result.AddRange(circleCoords);
                }*/
            }

            // Sort coordinates by their appearance order in the NOTAM text
            if (result.Count > 1)
            {
                result = result.OrderBy(r => r.Index).ToList();
            }

            // --- Handle single circular areas only ---
            if (radiusMeters.HasValue && result.Count == 1)
            {
                var circle = GenerateCircle(result[0], radiusMeters.Value);
                result = circle;
            }

            // --- ARC DETECTION AND GENERATION ---

            // Find all arc definitions like “ARC OF A CIRCLE WITH A RADIUS OF 2.2NM CENTERED ON 435752N 0193426E”
            var arcRegex = new Regex(
                @"(CLOCKWISE|ANTICLOCKWISE|COUNTERCLOCKWISE)?\s*LINE.*?ARC OF A CIRCLE WITH A RADIUS OF\s+([\d.,]+)\s*NM\s+CENTERED ON\s+([0-9NSWE\s]+)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


            var arcMatches = arcRegex.Matches(input);
            if (arcMatches.Count > 0 && result.Count > 1)
            {
                var updated = new List<LatLon>(result);
                foreach (Match arc in arcMatches)
                {
                    bool clockwise = !Regex.IsMatch(arc.Groups[1].Value, "ANTI|COUNTER", RegexOptions.IgnoreCase);
                    double radiusNm = double.Parse(arc.Groups[2].Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                    string centerText = arc.Groups[3].Value;

                    var centerMatch = coordinateRegex.Match(centerText);
                    if (!centerMatch.Success) continue;

                    var center = new LatLon
                    {
                        Latitude = ToDecimalDegrees(centerMatch.Groups[1].Value, centerMatch.Groups[2].Value, centerMatch.Groups[3].Value, centerMatch.Groups[4].Value, centerMatch.Groups[5].Value),
                        Longitude = ToDecimalDegrees(centerMatch.Groups[6].Value, centerMatch.Groups[7].Value, centerMatch.Groups[8].Value, centerMatch.Groups[9].Value, centerMatch.Groups[10].Value),
                        ConvertedString = centerMatch.Value
                    };

                    // Find the closest coordinates before and after the arc text
                    var before = result.LastOrDefault(c => c.Index < arc.Index);
                    var after = result.FirstOrDefault(c => c.Index > arc.Index + arc.Length);
                    if (before == null || after == null) continue;

                    var arcPoints = GenerateArc(center, before, after, radiusNm * 1852, clockwise, 10);

                    // Replace straight line segment with arc points
                    int insertIndex = updated.FindIndex(c => c.Index == before.Index);
                    if (insertIndex >= 0)
                    {
                        updated.InsertRange(insertIndex + 1, arcPoints);
                    }

                    var centerPoint = updated.FirstOrDefault(ll => ll.Latitude == center.Latitude && ll.Longitude == center.Longitude);

                    if(centerPoint != null)
                        updated.Remove(centerPoint);
                }
                result = updated;
            }

            return result;
        }

        /// <summary>
        /// Converts geographic coordinates from degrees, minutes, seconds, and fractional values  into a decimal degree
        /// representation.
        /// </summary>
        /// <param name="deg">The degrees component of the coordinate. Must be a valid numeric string.</param>
        /// <param name="min">The minutes component of the coordinate. Can be null or empty if not applicable.</param>
        /// <param name="sec">The seconds component of the coordinate. Can be null or empty if not applicable.</param>
        /// <param name="fraction">The fractional component of the coordinate. Represents a decimal fraction of the smallest unit  provided
        /// (seconds, minutes, or degrees). Can be null or empty if not applicable.</param>
        /// <param name="hemisphere">The hemisphere indicator. Must be "N", "S", "E", or "W". "S" and "W" result in negative values  for the
        /// decimal degree representation.</param>
        /// <returns>A double representing the coordinate in decimal degrees. Negative values indicate coordinates  in the
        /// southern or western hemispheres.</returns>
        private static double ToDecimalDegrees(string deg, string min, string sec, string fraction, string hemisphere)
        {
            double degrees = double.Parse(deg, System.Globalization.CultureInfo.InvariantCulture);
            double minutes = string.IsNullOrEmpty(min) ? 0 : double.Parse(min, System.Globalization.CultureInfo.InvariantCulture);
            double seconds = string.IsNullOrEmpty(sec) ? 0 : double.Parse(sec, System.Globalization.CultureInfo.InvariantCulture);
            double decimalPart = string.IsNullOrEmpty(fraction) ? 0 : double.Parse(fraction.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);

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

        /// <summary>
        /// Generates a list of geographic coordinates representing a circle around a specified center point.
        /// </summary>
        /// <remarks>The generated circle is an approximation based on the Earth's curvature, assuming a
        /// spherical Earth model. The accuracy of the coordinates may vary slightly depending on the radius and the
        /// number of points.</remarks>
        /// <param name="center">The center point of the circle, specified as a <see cref="LatLon"/> object.</param>
        /// <param name="radiusMeters">The radius of the circle in meters. Must be a positive value.</param>
        /// <param name="points">The number of points to generate along the circumference of the circle. Defaults to 36. Higher values result
        /// in a smoother circle.</param>
        /// <returns>A list of <see cref="LatLon"/> objects representing the geographic coordinates of the circle's
        /// circumference. The list contains <paramref name="points"/> evenly spaced coordinates.</returns>
        public static List<LatLon> GenerateCircle(LatLon center, double radiusMeters, int points = 36)
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

        public static List<LatLon> GenerateCircleNM(LatLon center, double radiusNm, int points = 36)
        {
            return GenerateCircle(center, radiusNm * 1852, points);
        }

        /// <summary>
        /// Generates coordinates along an arc of a circle between two points, centered on a given coordinate.
        /// </summary>
        public static List<LatLon> GenerateArc(LatLon center, LatLon start, LatLon end, double radiusMeters, bool clockwise, int degreesPerPoint = 10)
        {
            //return new List<LatLon>() { start, center, end };
            var results = new List<LatLon>();
            double earthRadius = 6371000.0;

            // Convert to radians
            double latC = center.Latitude * Math.PI / 180.0;
            double lonC = center.Longitude * Math.PI / 180.0;
            double latS = start.Latitude * Math.PI / 180.0;
            double lonS = start.Longitude * Math.PI / 180.0;
            double latE = end.Latitude * Math.PI / 180.0;
            double lonE = end.Longitude * Math.PI / 180.0;

            double degPP = degreesPerPoint * Math.PI / 180.0;

            // Compute initial and final bearings from center  θ=atan2(y−yc​,x−xc​)
            double startBearing = Math.Atan2(Math.Sin(lonS - lonC) * Math.Cos(latS),
                                            Math.Cos(latC) * Math.Sin(latS) - Math.Sin(latC) * Math.Cos(latS) * Math.Cos(lonS - lonC));
            //double startBearing = Math.Atan2(lonS - lonC, latS - latC);
            double endBearing = Math.Atan2(Math.Sin(lonE - lonC) * Math.Cos(latE),
                                           Math.Cos(latC) * Math.Sin(latE) - Math.Sin(latC) * Math.Cos(latE) * Math.Cos(lonE - lonC));
            //double endBearing = Math.Atan2(latE - lonC, lonE - latC);

            if (clockwise && endBearing < startBearing) endBearing += 2 * Math.PI;
            if (!clockwise && startBearing < endBearing) startBearing += 2 * Math.PI;

            /*startBearing -= Math.PI / 2 - ((Math.PI / 180) * 5);
            endBearing -= Math.PI / 2 - ((Math.PI / 180) * 5);*/

            double points = (endBearing - startBearing) / degPP;

            for (int i = 1; i < points; i++)
            {
                double bearing = startBearing + degPP * i * (clockwise ? 1 : -1);
                double newLat = Math.Asin(Math.Sin(latC) * Math.Cos(radiusMeters / earthRadius) +
                                          Math.Cos(latC) * Math.Sin(radiusMeters / earthRadius) * Math.Cos(bearing));
                double newLon = lonC + Math.Atan2(Math.Sin(bearing) * Math.Sin(radiusMeters / earthRadius) * Math.Cos(latC),
                                                  Math.Cos(radiusMeters / earthRadius) - Math.Sin(latC) * Math.Sin(newLat));

                results.Add(new LatLon
                {
                    Latitude = newLat * 180.0 / Math.PI,
                    Longitude = newLon * 180.0 / Math.PI
                });
            }

            return results;
        }


        /// <summary>
        /// Extracts a radius value in meters from a given NOTAM text string.
        /// </summary>
        /// <remarks>Supported units are: <list type="bullet"> <item><description><c>NM</c>: Nautical
        /// miles, converted to meters.</description></item> <item><description><c>KM</c>: Kilometers, converted to
        /// meters.</description></item> <item><description><c>M</c>: Meters, returned as-is.</description></item>
        /// </list> The method performs a case-insensitive match for the unit and supports both dot and comma as decimal
        /// separators.</remarks>
        /// <param name="input">The input string containing a radius value and its unit. The expected format is a numeric value  followed by
        /// a unit (e.g., "10 KM", "5.5 NM", or "1000 M").</param>
        /// <returns>The radius value in meters, or <see langword="null"/> if the input string does not contain a valid  radius
        /// value or unit.</returns>
        public static double? ExtractRadiusInMeters(string input)
        {
            var radiusMatch = Regex.Match(input, @"\b(\d+(?:[.,]\d+)?)\s*(NM|KM|M)\b", RegexOptions.IgnoreCase);
            if (!radiusMatch.Success)
                return null;

            double value = double.Parse(radiusMatch.Groups[1].Value.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
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

        /// <summary>
        /// Converts a list of latitude and longitude coordinates into a list of map-compatible points.
        /// </summary>
        /// <remarks>This method transforms geographic coordinates into a format suitable for mapping
        /// applications. Each <see cref="LatLon"/> in the input list is converted to a <see cref="PointLatLng"/>
        /// object.</remarks>
        /// <param name="input">A list of <see cref="LatLon"/> objects representing latitude and longitude coordinates.</param>
        /// <returns>A list of <see cref="PointLatLng"/> objects, where each point corresponds to the latitude and longitude of
        /// the input coordinates.</returns>
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

        /// <summary>
        /// Extracts the latitude and longitude coordinates from the "Q)" section of a NOTAM text.
        /// </summary>
        /// <remarks>This method parses the "Q)" section of the NOTAM text to extract the coordinates. The
        /// "Q)" section is expected to follow a specific format, and the coordinates are typically located in the
        /// seventh part of the section. If the format is invalid or no coordinates are found, the method returns <see
        /// langword="null"/>.</remarks>
        /// <param name="notamText">The NOTAM text containing the "Q)" section. This parameter must not be null or empty.</param>
        /// <returns>A <see cref="Tuple{T1, T2}"/> containing the latitude and longitude as <see cref="double"/> values. Returns
        /// <see langword="null"/> if the "Q)" section is missing, improperly formatted, or does not contain valid
        /// coordinates.</returns>
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

        /// <summary>
        /// Attempts to extract the NOTAM zone name from the provided NOTAM text about a zone.
        /// </summary>
        /// <remarks>This method searches for the zone name within the NOTAM text by identifying the
        /// section enclosed in parentheses following the "E)" marker. If the required markers or parentheses are not
        /// present, the method returns <see langword="null"/>.</remarks>
        /// <param name="NotamText">The text of the NOTAM from which the zone name will be extracted. Cannot be null or empty.</param>
        /// <returns>The extracted zone name as a string, or <see langword="null"/> if the zone name cannot be found.</returns>
        public static string GetNotamZoneName(string NotamText)
        {
            if (string.IsNullOrEmpty(NotamText)) return null;

            int startIndex = NotamText.IndexOf("E)");

            if(startIndex == -1) return null;

            startIndex = NotamText.IndexOf('(', startIndex);

            if (startIndex == -1) return null;

            int endIndex = NotamText.IndexOf(')', startIndex);

            if (endIndex == -1) return null;

            string zoneName = NotamText.Substring(startIndex + 1, endIndex - startIndex - 1).Trim().Replace("\r", "").Replace("\n", " ");

            return zoneName;
        }

        public static (string, string) GetNotamVerticalLimits(Notams nos, string NotamID)
        {
            if (string.IsNullOrEmpty(NotamID) || !nos.CurrentNotams.ContainsKey(NotamID))
            {
                return (null, null); // Not found
            }
            else return GetNotamVerticalLimits(nos.CurrentNotams[NotamID]);
        }

        public static (string, string) GetNotamVerticalLimits(string NotamText)
        {
            if (string.IsNullOrEmpty(NotamText)) return (null, null);

            NotamText = NotamText.Replace("\r\n", "\n");

            int lowerLimitStartIndex = NotamText.IndexOf("F) ") + 3;
            int lowerLimitEndIndex = NotamText.IndexOf("G) ", lowerLimitStartIndex);

            int upperLimitStartIndex = lowerLimitEndIndex + 3;
            int upperLimitEndIndex = NotamText.IndexOf("\n", upperLimitStartIndex);

            if(upperLimitEndIndex == -1)
            {
                upperLimitEndIndex = NotamText.Length;
            }

            if(
                lowerLimitStartIndex == -1 || 
                upperLimitStartIndex == -1 || 
                lowerLimitEndIndex == -1
                )
            {
                return (null, null);
            }

            string lowerLimit = NotamText.Substring(lowerLimitStartIndex, lowerLimitEndIndex - lowerLimitStartIndex).Trim();
            string upperLimit = NotamText.Substring(upperLimitStartIndex, upperLimitEndIndex - upperLimitStartIndex).Trim();

            return (lowerLimit, upperLimit);
        }

        public static ZoneInNotam TryFindZone(Notams nos, string NotamID)
        {
            if (
                nos == null ||
                nos.CurrentNotams == null ||
                string.IsNullOrEmpty(NotamID) || 
                !nos.CurrentNotams.ContainsKey(NotamID))
            {
                return null; // Not found
            }

            return TryFindZone(nos.CurrentNotams[NotamID]);
        }

        public static ZoneInNotam TryFindZone(string notamText, bool raw = false)
        {
            int index = raw ? 0 : notamText.IndexOf("E)");
            if (index == -1) return null; // No "E)" found

            string searchText = notamText.Substring(index).Trim().ToLower();

            int diff = notamText.Length - searchText.Length;

            var zones = MapManager.LoadPolys();

            if (zones == null) return null;

            foreach (var item in zones.AllZoneNames)
            {
                int foundIndex;

                var search = new List<string>() { 
                    item.Value.Item2.Trim().ToLower(), // as is                    
                };

                if(item.Value.Item2.Contains('(')) // ako ima zagrade, onda trazimo i samo ono pre zagrada
                    search.Add(item.Value.Item2.Substring(0, item.Value.Item2.IndexOf('(')).Trim().ToLower()); // do prve zagrade)

                foreach (var searchItem in search)
                {
                    if((foundIndex = searchText.IndexOf(searchItem)) != -1)
                    {
                        return new ZoneInNotam
                        (
                            item.Key, // Zone.ID
                            item.Value.Item2, // Zone.Name
                            notamText.Substring(foundIndex + diff, searchItem.Length), // ConvertedString
                            foundIndex + diff, // Index
                            zones.AllZones[item.Key].Points.Select(p => {
                                if (p.Count != 2)
                                    return new PointLatLng(0, 0); // Invalid point, return default
                                else
                                    return new PointLatLng(p[0], p[1]);
                                }).ToList(), // ZonePoints
                            zones.AllZones[item.Key].Color // Color
                        );
                    }
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Represents a geographical coordinate with latitude and longitude values.
    /// </summary>
    /// <remarks>The <see cref="LatLon"/> class provides properties to store latitude and longitude values, 
    /// as well as additional derived information such as a converted string representation and its length.</remarks>
    internal class LatLon
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ConvertedString { get; set; }
        public int ConvertedStringLength => ConvertedString?.Length ?? -1;
        public int Index { get; set; }
        public override string ToString() => $"{Latitude}, {Longitude}";

        // jaaaki cedo ↓... nisam ni znao da moze ovakav tip da bude return iz metode... c# postao python
        private static (int deg, int min, double sec) ToDms(double decimalDegrees)
        {
            double abs = Math.Abs(decimalDegrees);

            int deg = (int)abs;
            double minFull = (abs - deg) * 60;
            int min = (int)minFull;
            double sec = (minFull - min) * 60;

            // --- Handle rounding overflow ---
            if (sec >= 59.995)
            {
                sec = 0;
                min++;
            }
            if (min >= 60)
            {
                min = 0;
                deg++;
            }

            return (deg, min, sec);
        }

        public string ToFullString()
        {
            var (latDeg, latMin, latSec) = ToDms(Latitude);
            var (lonDeg, lonMin, lonSec) = ToDms(Longitude);

            return string.Format(
                "{0}{1:00}°{2:00}′{3:00.##}″ {4}{5:000}°{6:00}′{7:00.##}″",
                Latitude >= 0 ? "N" : "S", latDeg, latMin, latSec,
                Longitude >= 0 ? "E" : "W", lonDeg, lonMin, lonSec
            );
        }
    }

    internal class ZoneInNotam
    {
        public string ZoneID { get; private set; }
        public string ZoneName { get; private set; }
        public string ConvertedString { get; private set; }
        public int ConvertedStringLength => ConvertedString?.Length ?? -1;
        public int Index { get; private set; }
        public List<PointLatLng> ZonePoints { get; private set; }
        public List<int> Color { get; private set; }

        public ZoneInNotam(string zoneID, string zoneName, string convertedString, int index, List<PointLatLng> zonePoints, List<int> color)
        {
            ZoneID = zoneID;
            ZoneName = zoneName;
            ConvertedString = convertedString;
            Index = index;
            ZonePoints = zonePoints;
            Color = color;
        }
    }
}
