using NOTAM_Browser.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOTAM_Browser.Helpers
{
    internal static class SettingsManager
    {
        public const int SEARCH_HISTORY_COUNT = 10;

        #region "Search History"
        public static List<string> GetSearchHistory()
        {
            string searchHistory = Settings.Default.searchHistory;

            if (string.IsNullOrEmpty(searchHistory))
            {
                return new List<string>();
            }

            string[] searches = searchHistory.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            return searches.ToList();
        }

        public static List<string> AddToSearchHistory(string search)
        {
            var history = GetSearchHistory();

            history.Remove(search);
            history.Insert(0, search);

            if(history.Count > SEARCH_HISTORY_COUNT)
            {
                history.RemoveRange(SEARCH_HISTORY_COUNT, history.Count - SEARCH_HISTORY_COUNT);
            }

            string[] searches = history.ToArray();

            string settingsRaw = string.Join(";", searches);

            Settings.Default.searchHistory = settingsRaw;
            Settings.Default.Save();

            return history;
        }
        #endregion
    }
}
