/* Copyright (c) 2018 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
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

        public static Task<int> AwaitableDownloadZip() {
            return Promises.ExecuteAsync<int>("interop_downloadZip");
        }
    }
}
