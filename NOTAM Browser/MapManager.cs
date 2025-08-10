using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Diagnostics.Eventing.Reader;



#if DEBUG
using System.Diagnostics;
#endif

namespace NOTAM_Browser
{

    internal static class MapManager
    {
        private static string FILENAME = "polys.json";
        private static string fullFileName { get { return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), FILENAME); } }

        #region "Old versions"
        /// <summary>
        /// Checks if the save file is a file from a previous version, in which case asks the user if they want to automatically update the file and set all the polygons in it in the "Custom" category.
        /// </summary>
        private static void CheckForV1Json()
        {
            try
            {
                string json = File.ReadAllText(fullFileName);

                Group v1Json = JsonConvert.DeserializeObject<Group>(json);

                DialogResult convert = MessageBox.Show("Fajl gde su sačuvane zone je od stare verzije. Da li biste želeli da se fajl automatski konvertuje u novu verziju?", "Stara verzija", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (convert == DialogResult.Yes)
                {
                    PolyData pd = new PolyData();
                    pd._groups = new Dictionary<string, JToken>();
                    JToken jt = JToken.FromObject(v1Json);
                    pd._groups.Add("Custom", jt);

                    string jsonText = JsonConvert.SerializeObject(pd);

                    File.WriteAllText(fullFileName, jsonText);

                    // TODO fix this thank
                    Application.Restart();
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"MapManager: Error while checking for V1 JSON file: {ex}");
#endif
            }
        }
        #endregion

        /// <summary>
        /// Reads the file and returns the deserialized json
        /// </summary>
        /// <returns>The deserialized json as a PolyData object</returns>
        public static PolyData LoadPolys()
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
                PolyData data = JsonConvert.DeserializeObject<PolyData>(json);
                return data;
            }
            catch (JsonException ex)
            {
#if DEBUG
                Debug.WriteLine($"MapManager: Error deserializing zones from {FILENAME}: {ex.ToString()}");
#endif
                CheckForV1Json();
            }
            catch(Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"MapManager: General error deserializing zones from {FILENAME}: {ex.ToString()}");
#endif
                CheckForV1Json();
            }

            return null;
        }

        /// <summary>
        /// Saves the provided raw polygon data to a file in JSON format.
        /// </summary>
        /// <remarks>This method serializes the <paramref name="polyData"/> object into a JSON string and
        /// writes it to a file. The file path is determined by the internal configuration of the application. If an
        /// error occurs during serialization or file writing, the exception is caught and logged in debug
        /// mode.</remarks>
        /// <param name="polyData">The polygon data to be serialized and saved. Cannot be null.</param>
        public static void SaveRawData(PolyData polyData)
        {
            try
            {
                string jsonText = JsonConvert.SerializeObject(polyData);

                File.WriteAllText(fullFileName, jsonText);
            }
            catch(Exception ex)
            {
# if DEBUG
                Debug.WriteLine($"MapManager: Failed to write raw poly data to file: {ex}");
#endif
            }
        }

        /// <summary>
        /// Adds the specified polygon data to the existing data file, merging new groups and polygons with the existing
        /// ones. If the file does not exist, it creates a new file with the provided data.
        /// </summary>
        /// <remarks>This method reads the existing polygon data from a file, updates it with the provided
        /// data,  and writes the merged result back to the file. If the file is missing or the existing data  cannot be
        /// deserialized, a new file is created with the provided data. <para> Groups and polygons in the provided
        /// <paramref name="data"/> are added to the existing data.  If a group or polygon already exists, it is updated
        /// with the new values; otherwise, it is added. </para></remarks>
        /// <param name="data">The polygon data to add. Must not be <see langword="null"/> and must contain at least one group.</param>
        public static void AddPolys(PolyData data)
        {
            if (data == null || data.Groups.Count == 0) return;
            string json;
            PolyData existingData = new PolyData();

            if (File.Exists(fullFileName))
            {
                try
                {
                    json = File.ReadAllText(fullFileName);
                    existingData = JsonConvert.DeserializeObject<PolyData>(json);

                    if (existingData == null) existingData = new PolyData();
                }
                catch (JsonException ex)
                {
#if DEBUG
                    Debug.WriteLine($"MapManager: Error deserializing existing zones from {FILENAME}: {ex.ToString()}");
#endif
                }
            }

            foreach (var group in data.Groups)
            {
                if (group.Value == null)
                {
                    existingData.Groups.Remove(group.Key); //this just returns false if the key doesn't exist, no need to check if it exists
                    continue;
                }

                if (!existingData.Groups.ContainsKey(group.Key))
                    existingData.Groups.Add(group.Key, new Group());

                foreach (var zone in group.Value.Polygons)
                {
                    if (existingData.Groups[group.Key].Polygons.ContainsKey(zone.Key))
                    {
                        existingData.Groups[group.Key].Polygons[zone.Key] = zone.Value;
#if DEBUG
                        Debug.WriteLine($"MapManager: Added new zone {zone.Key} in group {group.Key} in {FILENAME}.");
#endif
                    }
                    else
                    { 
                        existingData.Groups[group.Key].Polygons.Add(zone.Key, zone.Value);
#if DEBUG
                        Debug.WriteLine($"MapManager: Updated existing zone {zone.Key} in group {group.Key} in {FILENAME}.");
#endif
                    }
                }
            }

            json = JsonConvert.SerializeObject(existingData);

            try
            {
                File.WriteAllText(fullFileName, json);
#if DEBUG
                Debug.WriteLine($"MapManager: Successfully wrote polygons to {FILENAME}.");
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

    public class PolyData
    {
        public PolyData()
        {
            Groups = new Dictionary<string, Group>();
        }

        [JsonExtensionData]
        public Dictionary<string, JToken> _groups { get; set; }

        [JsonIgnore]
        public Dictionary<string, Group> Groups { get; set; }

        /// <summary>
        /// Prepares the data for serialization by ensuring the internal dictionary is initialized and populated with
        /// the current group data.
        /// </summary>
        /// <remarks>This method is invoked automatically during the serialization process to ensure the
        /// internal state is properly prepared. It initializes the internal dictionary if it is null and clears its
        /// contents before repopulating it with the current group data.</remarks>
        /// <param name="context">The streaming context that provides additional information about the serialization operation.</param>
        [OnSerializing]
        private void PrepareData(StreamingContext context)
        {
            _groups = new Dictionary<string, JToken>();

            foreach (var g in Groups)
            {
                if (g.Value == null) continue;
                _groups.Add(g.Key, JToken.FromObject(g.Value));
            }
        }

        /// <summary>
        /// Gets a dictionary containing all zones, where each key is a unique identifier  composed of the group name
        /// and polygon name, and each value is the corresponding zone data.
        /// </summary>
        /// <remarks>This property aggregates all zones from the available groups and their polygons into
        /// a single  dictionary for convenient access. The keys are constructed by combining the group name and 
        /// polygon name, ensuring uniqueness.</remarks>
        [JsonIgnore]
        public Dictionary<string, ZoneData> AllZones { 
            get
            {
                var output = new Dictionary<string, ZoneData>();

                foreach (var g in Groups)
                {
                    foreach (var p in g.Value.Polygons)
                    {
                        output.Add($"{g.Key}_{p.Key}", p.Value);
                    }
                }

                return output;
            } 
        }
        
        /// <summary>
        /// Invoked after the object has been deserialized to perform additional initialization.
        /// </summary>
        /// <remarks>This method ensures that the deserialized data is properly initialized by converting
        /// serialized group objects into their corresponding <see cref="Group"/> instances, assigning names, and
        /// populating the <see cref="Groups"/> collection. It also calls <see cref="Group.SetUIDs"/> to finalize the
        /// initialization of each group.</remarks>
        /// <param name="context">The streaming context that provides contextual information about the source or destination of the
        /// serialization.</param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_groups == null) return;

            foreach (var kvp in _groups)
            {
                if (kvp.Key == null || kvp.Value == null) continue;
                Group g = kvp.Value.ToObject<Group>();

                Groups.Add(kvp.Key, g);

                if (g == null) continue;

                g.Name = kvp.Key;


                g.SetUIDs();
            }
        }

        public ZoneData GetPolyFromId(string id)
        {
            if(!AllZones.ContainsKey(id)) return null;

            return AllZones[id];
        }
    }

    public class Group
    {
        public Group()
        {
            Polygons = new Dictionary<string, ZoneData>();
        }

        [JsonProperty("default_color")]
        public Color DefaultColor { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> _polygons { get; set; }

        [JsonIgnore]
        public Dictionary<string, ZoneData> Polygons { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [OnSerializing]
        public void ConvertData(StreamingContext context)
        {
            _polygons = new Dictionary<string, JToken>();

            foreach (var p in Polygons)
            {
                if (p.Value == null) continue;
                _polygons.Add(p.Key, JToken.FromObject(p.Value));
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_polygons == null) return;

            foreach (var kvp in _polygons)
            {
                if (kvp.Key == null || kvp.Value == null) continue;

                var g = kvp.Value.ToObject<ZoneData>();

                Polygons.Add(kvp.Key, g);

                if (g == null) continue;

                g.Name = kvp.Key;
                g.UID = $"{Name}_{g.Name}";

            }
        }
        public void SetUIDs()
        {
            foreach (var p in Polygons)
            {
                if(p.Value == null) continue;
                p.Value.UID = $"{Name}_{p.Value.Name}";
            }
        }
    }

    public class ZoneData
    {
        [JsonIgnore]
        public string Name { get; set; }
        [JsonIgnore]
        public string UID { get; set; }

        [JsonProperty("color")]
        public List<int> Color { get; set; }

        [JsonProperty("points")]
        public List<List<double>> Points { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }
    }
}

