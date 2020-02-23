/* Copyright (c) 2019 noahc3
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
        public static Task<int> AddFile(IJSRuntime jsRuntime, string url, string path, string filename) {
            return Promises.ExecuteAsync<int>(jsRuntime, "interop_addFile", new string[] { url, path, filename });
        }

        public static Task<string> AwaitableGenerateZip(IJSRuntime jsRuntime, string url) {
            return Promises.ExecuteAsync<string>(jsRuntime, "interop_generateZip", url);
        }

        public static ValueTask<string> ScrollToTop(IJSRuntime jsRuntime) {
            return jsRuntime.InvokeAsync<string>("scrollToTop");
        }

        public static ValueTask<int> GetScroll(IJSRuntime jsRuntime) {
            return jsRuntime.InvokeAsync<int>("getScrollPos");
        }

        public static ValueTask<string> SetScroll(IJSRuntime jsRuntime, int pos) {
            return jsRuntime.InvokeAsync<string>("setScrollPos", pos);
        }

        public static async Task<string> GetFileData(IJSRuntime jsRuntime, string fileInputRef) {
            return (await jsRuntime.InvokeAsync<StringHolder>("getFileData", fileInputRef)).Content;
        }

        private class StringHolder {
            public string Content { get; set; }
        }
    }
}
