using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class FileSystemItemFilterComponent {
        public FileSystemItemType matchFileSystemItemType { get; set; } = FileSystemItemType.Any;
        public string filterDirectory { get; set; } = "";
        public MatchType matchType { get; set; } = MatchType.MatchRegex;
        public string matchParameter { get; set; } = "";

        public async Task<List<FileSystemInfo>> Match(string root) {
            List<FileSystemInfo> matches = new List<FileSystemInfo>();
            DirectoryInfo curItem = new DirectoryInfo(Path.Join(root, filterDirectory));

            if (!curItem.Exists) return matches;

            foreach(FileSystemInfo i in curItem.GetFileSystemInfos()) {
                if (Regex.IsMatch(i.Name, matchParameter) && (matchFileSystemItemType == FileSystemItemType.Any || i.IsFile() == (matchFileSystemItemType == FileSystemItemType.File))) {
                    matches.Add(i);
                }
            }

            return matches;
        }
    }
}
