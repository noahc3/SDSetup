using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SDSetupCommon.Data.UpdaterModels {
    public class MoveFileSystemItemsTask : UpdaterTask {
        public FileSystemItemFilterComponent filter { get; set; } = new FileSystemItemFilterComponent();
        public string targetDirectory { get; set; } = "";
        public bool createTargetDirectoryIfMissing { get; set; } = true;

        public async Task Apply(string root) {
            List<FileSystemInfo> matches = await filter.Match(root);

            if (matches.Count == 0) return;

            DirectoryInfo target = new DirectoryInfo(Path.Join(root, targetDirectory));
            if (!target.Exists) {
                if (createTargetDirectoryIfMissing) target.Create();
                else return;
            }

            foreach (FileSystemInfo i in matches) {
                if (i.IsFile()) File.Move(i.FullName, Path.Join(target.FullName, i.Name));
                else Directory.Move(i.FullName, Path.Join(target.FullName, i.Name));
            }
        }
    }
}
