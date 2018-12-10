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

        public static Task<string> AwaitableGenerateZip(string url) {
            return Promises.ExecuteAsync<string>("interop_generateZip", url);
        }

        public static Task<string> ScrollToTop() {
            return JSRuntime.Current.InvokeAsync<string>("scrollToTop");
        }

        public static Task<int> GetScroll() {
            return JSRuntime.Current.InvokeAsync<int>("getScrollPos");
        }

        public static Task<string> SetScroll(int pos) {
            return JSRuntime.Current.InvokeAsync<string>("setScrollPos", pos);
        }

        public static async Task<string> GetFileData(string fileInputRef) {
            return (await JSRuntime.Current.InvokeAsync<StringHolder>("getFileData", fileInputRef)).Content;
        }

        private class StringHolder {
            public string Content { get; set; }
        }
    }
}
