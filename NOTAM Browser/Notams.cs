using NOTAM_Browser.Properties;
using System;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOTAM_Browser
{
    public class Notams
    {
        // private vars
        private const string FILENAME = "notams.json";
        private readonly string NOTAMQUERY = $"{Settings.Default.urlPre}{{0}}{Settings.Default.urlAft}";
        private readonly string NOTAM_DECODE_PRE = Settings.Default.notamPre;
        private readonly string NOTAM_DECODE_AFT = Settings.Default.notamAft;

        //private properties
        private static string fullFileName { get { return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), FILENAME); } }

        //public delegates
        public delegate void NotamAcknowledgedHandler(string NotamID);
        public delegate void NotamUnacknowledgedHandler(string NotamID);

        //public properties
        public Dictionary<string, string> AcknowledgedNotams { get; private set; }
        public Dictionary<string, string> CurrentNotams { get; private set; }
        public DateTime CurrentNotamsTime { get; private set; }
        public string LastSearch { get; private set; }

        //public events
        public event NotamAcknowledgedHandler NotamAcknowledged;
        public event NotamUnacknowledgedHandler NotamUnacknowledged;

        public Notams()
        {
            NotamsData nd;

            if (File.Exists(fullFileName))
            {
                nd = JsonSerializer.Deserialize<NotamsData>(File.ReadAllText(fullFileName));
            }
            else
                nd = new NotamsData();

            if (nd != null && nd.LatestNotamsTime != null)
                this.CurrentNotamsTime = nd.LatestNotamsTime;

            this.AcknowledgedNotams = nd.AcknowledgedNotams ?? new Dictionary<string, string>();
            this.CurrentNotams = nd.LatestNotams ?? new Dictionary<string, string>();
            this.LastSearch = nd.LastSearch ?? "";
        }

        public Dictionary<string, string> GetNotamsForDate(DateTime Date)
        {
            Regex rStartTime = new Regex(@"B\) [0-9]* ");
            Regex rEndTime = new Regex(@"C\) [0-9]* ");
            Match m;

            Date = Date.Date;

            string[] notamValidityStr = { "", "" };

            DateTime[] notamValidity = new DateTime[2];

            Dictionary<string, string> filteredNotams = new Dictionary<string, string>();

            foreach (var notam in CurrentNotams)
            {
                m = rStartTime.Match(notam.Value);

                if (m.Success)
                {
                    notamValidityStr[0] = m.Value.Substring(2).Trim();
                }

                m = rEndTime.Match(notam.Value);

                if (m.Success)
                {
                    notamValidityStr[1] = m.Value.Substring(2).Trim();
                }

                if (notamValidityStr[1] == "PERM") notamValidityStr[1] = "9901010000"; //If the notam is PERM, set the end date at 2099


                for (int i = 0; i < 2; i++)
                {
                    int[] parsed = new int[5];
                    /*
                     * 0 - Year
                     * 1 - Month
                     * 2 - Date
                     * 3 - Hour
                     * 4 - Minute
                     */

                    bool success = true;

                    for (int j = 0; j < 5; j++) //parsing data, each 2 chars long 
                    {
                        success &= int.TryParse(notamValidityStr[i].Substring(j * 2, 2), out parsed[j]);
                    }

                    if (!success)
                    {
                        notamValidity[0] = new DateTime(1, 1, 1);
                        notamValidity[1] = new DateTime(9999, 1, 1);
                        break;
                    }

                    notamValidity[i] = new DateTime(parsed[0] + 2000, parsed[1], parsed[2], parsed[3], parsed[4], 0);
                }

                if (Date >= notamValidity[0] && Date <= notamValidity[1])
                    filteredNotams.Add(notam.Key, notam.Value);
            }

            return filteredNotams;
        }


        public async Task GetFromInternet(string Designators)
        {
            try
            {
                string URL = String.Format(NOTAMQUERY, Designators);

                string httpResponse;

                using (HttpClient client = new HttpClient())
                {
                    httpResponse = await client.GetStringAsync(URL).ConfigureAwait(false);
                }

                this.CurrentNotams.Clear();

                int index;
                string notamText;
                string notamId = "";

                Regex r = new Regex(@"A\) [A-Z]{4}");

                Match m;

                while ((index = httpResponse.IndexOf(NOTAM_DECODE_PRE)) != -1)
                {
                    httpResponse = httpResponse.Substring(index + 5);

                    index = httpResponse.IndexOf(NOTAM_DECODE_AFT);

                    notamText = httpResponse.Substring(0, index);

                    m = r.Match(notamText);

                    if (m.Success)
                    {
                        notamId = m.Value.Substring(3);
                    }

                    notamId += notamText.Substring(0, notamText.IndexOf(' '));

                    if (!CurrentNotams.ContainsKey(notamId))
                        CurrentNotams.Add(notamId, notamText);
                }

                CurrentNotamsTime = DateTime.Now;
                LastSearch = Designators;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show($"Greška u toku dobijanja NOTAM-a: {ex.ToString()}");
#else
                MessageBox.Show($"Greška u toku dobijanja NOTAM-a: {ex.Message}");
#endif
            }
        }

        public bool AcknowledgeNotam(string NotamID)
        {
            if (!CurrentNotams.ContainsKey(NotamID)) return false;

            if (AcknowledgedNotams.ContainsKey(NotamID)) return true;

            AcknowledgedNotams.Add(NotamID, CurrentNotams[NotamID]);

            // fire the event
            NotamAcknowledged?.Invoke(NotamID);

            return true;
        }

        public bool UnacknowledgeNotam(string NotamID)
        {
            if (!AcknowledgedNotams.ContainsKey(NotamID)) return true;

            AcknowledgedNotams.Remove(NotamID);
#if DEBUG
            Debug.WriteLine($"Notams: fired unacknowledged event for {NotamID}!");
#endif
            // fire the event
            NotamUnacknowledged?.Invoke(NotamID);

            return true;
        }

        public bool SaveToFile()
        {
            NotamsData nd = new NotamsData();
            nd.AcknowledgedNotams = this.AcknowledgedNotams;
            nd.LatestNotams = this.CurrentNotams;
            nd.LatestNotamsTime = this.CurrentNotamsTime;
            nd.LastSearch = this.LastSearch;

            //string fileText = JsonConvert.SerializeObject(nd);
            string fileText = JsonSerializer.Serialize<NotamsData>(nd);
            try
            {
                File.WriteAllText(fullFileName, fileText);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show($"Neuspešno čuvanje fajla:\n{ex.ToString()}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                MessageBox.Show($"Neuspešno čuvanje fajla: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
                return false;
            }

            return true;
        }

        public class NotamsData
        {
            [JsonPropertyName("acknowledged_notams")]
            public Dictionary<string, string> AcknowledgedNotams { get; set; }

            [JsonPropertyName("latest_notams")]
            public Dictionary<string, string> LatestNotams { get; set; }

            [JsonPropertyName("latest_notams_time")]
            public DateTime LatestNotamsTime { get; set; }
            [JsonPropertyName("last_search")]
            public string LastSearch { get; set; }
        }
    }


}
