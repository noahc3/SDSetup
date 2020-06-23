using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.UpdaterModels {
    public class FileSystemItem {
        public string name;
        public string fullPath;
        public FileSystemItemType fileSystemItemType;
        public FileSystemItem parent;
        public List<FileSystemItem> childItems = new List<FileSystemItem>();
    }
}
