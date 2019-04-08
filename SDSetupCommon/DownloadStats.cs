using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SDSetupBackend
{
    public class DownloadStats {
        public DateTime CurrentDateTime;
        public string CurrentDateTimeString;
        public long AllTimeBundles = 0;
        public Dictionary<string, Dictionary<string, int>> GranularStats; //<[DateTime.tostring], <[pkg name], downloads>>, last 30 days of data
        public Dictionary<string, long> AllTimeStats; //<[pkg name], alltime downloads>

        const int _hoursToStore = 720;

        public void VerifyStatisticsIntegrity(List<string> packages) {
            CurrentDateTime = GetSanitizedDateTime();
            CurrentDateTimeString = CurrentDateTime.ToString();

            if (GranularStats == null) GranularStats = new Dictionary<string, Dictionary<string, int>>();
            if (AllTimeStats == null) AllTimeStats = new Dictionary<string, long>(); 

            foreach(string k in packages) {
                if (!AllTimeStats.ContainsKey(k)) {
                    AllTimeStats[k] = 0;
                }
            }

            for (int i = 0; i > -1 * _hoursToStore; i--) {
                DateTime point = CurrentDateTime.AddHours(i);
                string sPoint = point.ToString();
                if (!GranularStats.ContainsKey(sPoint)) {
                    GranularStats[sPoint] = new Dictionary<string, int>();
                }
                foreach (string k in packages) {
                    if (!GranularStats[sPoint].ContainsKey(k)) {
                        GranularStats[sPoint][k] = -1 * i + 1;
                    }
                }
            }

            if (GranularStats.Keys.Count > _hoursToStore) {
                for (int i = _hoursToStore; i > -1 * GranularStats.Keys.Count; i--) {
                    DateTime point = CurrentDateTime.AddHours(i);
                    string sPoint = point.ToString();
                    if (!GranularStats.ContainsKey(sPoint)) {
                        GranularStats.Remove(sPoint);
                    }
                }
            }
        }

        private DateTime GetSanitizedDateTime() {
            DateTime now = DateTime.UtcNow;
            DateTime point = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, 0);
            return point;
        }

        public static DownloadStats FromDataBinary(string raw) {
            string[] binary = raw.Split('\n');

            DownloadStats stats = new DownloadStats();
            stats.CurrentDateTime = stats.GetSanitizedDateTime();
            stats.CurrentDateTimeString = stats.CurrentDateTime.ToString();

            stats.AllTimeBundles = Convert.ToInt64(binary[0]);

            stats.GranularStats = new Dictionary<string, Dictionary<string, int>>();
            stats.AllTimeStats = new Dictionary<string, long>();

            foreach (string k in binary[1].Split('.')) {
                stats.GranularStats[k] = new Dictionary<string, int>();
            }
            
            for (int n = 2; n < binary.Length - 1; n++) {
                string k = binary[n];
                string[] splits = k.Split('|');
                stats.AllTimeStats[splits[0]] = Convert.ToInt64(splits[1]);

                string[] values = splits[2].Split('.');

                int i = 0;

                foreach (string time in binary[1].Split('.')) {
                    stats.GranularStats[time][splits[0]] = Convert.ToInt32(values[i]);
                    i++;
                }
            }

            return stats;
        }

        public string ToDataBinary(List<string> packages) {
            string binary = "";
            
            binary = binary.NewLine(AllTimeBundles);

            string info = "";
            foreach (string k in GranularStats.Keys) {
                info += k + ".";
            }
            binary = binary.NewLine(info.Remove(info.Length - 1));

            Dictionary<string, string> infos = new Dictionary<string, string>();

            foreach(string k in packages) {
                infos[k] = "";
            }

            for (int i = 0; i > -1 * _hoursToStore; i--) {
                DateTime point = CurrentDateTime.AddHours(i);
                string sPoint = point.ToString();
                foreach (string k in packages) {
                    infos[k] += GranularStats[sPoint][k] + ".";
                }
            }

            foreach (string k in packages) {
                infos[k] = k + "|" + AllTimeStats[k] + "|" + infos[k];
                binary = binary.NewLine(infos[k]);
            }

            return binary;
        }
    }
}
