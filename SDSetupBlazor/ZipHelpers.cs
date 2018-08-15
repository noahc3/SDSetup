using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PromiseBlazorTest;

namespace SDSetupBlazor
{
    public static class ZipHelpers
    {
        public static Task<int> AddFile(string url, string path, string filename) {
            return Promises.ExecuteAsync<int>("interop_addFile", new string[] { url, path, filename });
        }

        public static Task<int> DownloadZip() {
            return JSRuntime.Current.InvokeAsync<int>("js_interop.interop_downloadFile");
        }


    }
}
