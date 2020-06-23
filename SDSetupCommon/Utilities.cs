using SDSetupCommon.Data.UpdaterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SDSetupCommon {
    public class Utilities {
        public static Guid CreateCryptographicallySecureGuid() {
            using (var provider = RandomNumberGenerator.Create()) {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                return new Guid(bytes);
            }
        }

        public static Guid CreateGuid() {
            return Guid.NewGuid();
        }

        public static FileSystemItem BuildRecursiveFileSystemItems(string path) {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileSystemItem entry = new FileSystemItem() { 
                name = dir.Name, 
                fullPath = dir.FullName, 
                fileSystemItemType = FileSystemItemType.Folder, 
                childItems = new List<FileSystemItem>() 
            };

            foreach (FileInfo f in dir.GetFiles()) {
                entry.childItems.Add(new FileSystemItem { 
                    name = f.Name,
                    fullPath = f.FullName,
                    fileSystemItemType = FileSystemItemType.File,
                    childItems = null
                });
            }

            foreach(DirectoryInfo d in dir.GetDirectories()) {
                entry.childItems.Add(BuildRecursiveFileSystemItems(d.FullName));
            }

            return entry;
        }

        public static void RebaseFileSystemItems(FileSystemItem item, string newRoot = "", FileSystemItem parent = null) {
            item.fullPath = $"{newRoot}/{item.name}".AsPath();
            item.parent = parent;
            if (item.childItems != null) {
                foreach (FileSystemItem i in item.childItems) {
                    RebaseFileSystemItems(i, item.fullPath, item);
                }
            }
        }
    }
}
