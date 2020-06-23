using SDSetupCommon.Data;
using SDSetupCommon.Data.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class ServiceEndpoints {
        public static string DownloadGithubArtifactEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/service/githubartifactdownload/{0}/{1}/{2}"); } } // {0}: author | {1}: repo name | {2}: artifact id
        public static string GithubReleaseArtifactsEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/service/githubreleaseartifacts/{0}/{1}/{2}"); } } // {0}: prerelease, stable | {1}: author | {2}: repo name
        public static async Task<GitReleaseArtifacts> GetGithubReleaseArtifacts(bool includePrereleases, string author, string repoName) {
            return await CommsUtilities.GetJsonAsync<GitReleaseArtifacts>(String.Format(GithubReleaseArtifactsEndpoint, includePrereleases ? "prerelease" : "stable", author, repoName));
        }
        public static async Task<MemoryStream> DownloadGithubArtifact(string author, string repoName, string artifactId) {
            Stream stream = await CommsUtilities.GetStreamAsync(String.Format(DownloadGithubArtifactEndpoint, author, repoName, artifactId));
            MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms;
        }
    }
}
