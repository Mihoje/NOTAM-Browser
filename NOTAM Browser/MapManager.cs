using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;
using System;

#if DEBUG
using System.Diagnostics;
#endif

namespace NOTAM_Browser
{

    internal static class MapManager
    {
        private static string FILENAME = "polys.json";
        private static string fullFileName { get { return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), FILENAME); } }

        public static Zones LoadPolys()
        {
            if (!File.Exists(fullFileName))
            {
#if DEBUG
                Debug.WriteLine($"MapManager: File {FILENAME} not found.");
#endif
                return null;
            }

            try
            {
                string json = File.ReadAllText(fullFileName);
                Zones zones = JsonConvert.DeserializeObject<Zones>(json);
                return zones;
            }
            catch (JsonException ex)
            {
#if DEBUG
                Debug.WriteLine($"MapManager: Error deserializing zones from {FILENAME}: {ex.ToString()}");
#endif
            }

            return null;
        }

        public static void AddPolys(Zones zones)
        {
            if (zones == null || zones.ZoneData.Count == 0) return;
            string json;
            Zones existingZones = new Zones();

            if (File.Exists(fullFileName))
            {
                try
                {
                    json = File.ReadAllText(fullFileName);
                    existingZones = JsonConvert.DeserializeObject<Zones>(json);

                    if (existingZones == null) existingZones = new Zones();
                }
                catch (JsonException ex)
                {
#if DEBUG
                    Debug.WriteLine($"MapManager: Error deserializing existing zones from {FILENAME}: {ex.ToString()}");
#endif
                }
            }

            foreach (var zone in zones.ZoneData)
            {
                if (!existingZones.ZoneData.ContainsKey(zone.Key))
                {
                    existingZones.ZoneData.Add(zone.Key, zone.Value);
#if DEBUG
                    Debug.WriteLine($"MapManager: Added new zone {zone.Key} in {FILENAME}.");
#endif
                }
                else
                {
                    existingZones.ZoneData[zone.Key] = zone.Value;
#if DEBUG
                    Debug.WriteLine($"MapManager: Updated existing zone {zone.Key} in {FILENAME}.");
#endif
                }
            }

            json = JsonConvert.SerializeObject(zones.ZoneData);

            try
            {
                File.WriteAllText(fullFileName, json);
#if DEBUG
                Debug.WriteLine($"MapManager: Successfully wrote zones to {FILENAME}.");
#endif
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"MapManager: Error writing to {FILENAME}: {ex.ToString()}");
#endif
            }
        }
    }

    public class Zones
    {
        public Zones()
        {
            ZoneData = new Dictionary<string, ZoneData>();
        }

        [JsonExtensionData]
        public Dictionary<string, JToken> _zoneData { get; set; }

        public Dictionary<string, ZoneData> ZoneData { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_zoneData == null) return;

            foreach (var kvp in _zoneData)
            {
                ZoneData.Add(kvp.Key, kvp.Value.ToObject<ZoneData>());
            }
        }
    }

    public class ZoneData
    {
        [JsonProperty("color")]
        public List<int> Color { get; set; }

        [JsonProperty("points")]
        public List<List<double>> Points { get; set; }
    }
}

