/* Copyright (c) 2018 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using System.Configuration;
using System.Collections.Specialized;

namespace SDSetupManifestGenerator {
    public static class Git {
        private static readonly bool AllowUnsafeGithubAuth = true;
        public static GitHubClient git = new GitHubClient(new ProductHeaderValue("noahc3.SDSetupManifestGenerator", "alpha1"));
        public static int Auth(string username, string password) {
            try {
                Credentials auth = new Credentials(username, password);
                git.Credentials = auth;
                MiscellaneousRateLimit rates = git.Miscellaneous.GetRateLimits().Result;
                if (rates.Rate.Limit > 60) {
                    if (AllowUnsafeGithubAuth) {
                        App.Default.GitUsername = username;
                        App.Default.GitPassword = password;
                        App.Default.Save();
                    }
                    return rates.Rate.Remaining;
                } else return -1;
            } catch (Exception) {
                return -1;
            }
        }

        //NOTE: This application doesn't implement the OAuth flow due to readability of secrets, therefore this is basically useless.
        public static int AuthCached() {
            string token = App.Default.GitToken;
            if (!String.IsNullOrEmpty(token)) {
                try {
                    Credentials auth = new Credentials(token);
                    git.Credentials = auth;
                    MiscellaneousRateLimit rates = git.Miscellaneous.GetRateLimits().Result;
                    if (rates.Rate.Limit > 60) return rates.Rate.Remaining;
                    else return -1;
                } catch (Exception) {
                    return -1;
                }
            } else return -1;
        }

        public static int AuthCachedUnsafe() {
            if (AllowUnsafeGithubAuth) {
                string username = App.Default.GitUsername;
                string password = App.Default.GitPassword;
                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) return -2;

                return Auth(username, password);
            } else {
                App.Default.GitUsername = "";
                App.Default.GitPassword = "";
                App.Default.Save();
                return -3;
            }
        }

        public static string[] GetLatestReleaseAssets(string url) {
            string[] _ = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string user = _[2];
            string repo = _[3];
            ReleaseAsset[] assets = git.Repository.Release.GetAll(user, repo).Result[0].Assets.ToArray();
            List<string> urls = new List<string>();
            foreach(ReleaseAsset k in assets) {
                urls.Add(k.BrowserDownloadUrl);
            }
            return urls.ToArray();
        }
    }
}
