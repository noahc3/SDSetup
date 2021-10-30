using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class DeleteFileSystemItemsTask : UpdaterTask {
        public FileSystemItemFilterComponent filter { get; set; } = new FileSystemItemFilterComponent();

        public async Task Apply(string root) {
            List<FileSystemInfo> matches = await filter.Match(root);

            foreach (FileSystemInfo i in matches) {
                if (i.IsFile()) i.Delete();
                else Directory.Delete(i.FullName, true);
            }
        }
    }
}
