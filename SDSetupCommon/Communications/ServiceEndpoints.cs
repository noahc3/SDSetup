using GitLabApiClient;
using GitLabApiClient.Models.Users.Responses;
using Octokit;
using SDSetupCommon.Data;
using SDSetupCommon.Data.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class ServiceEndpoints {
        public static string DownloadGithubArtifactEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/service/githubartifactdownload/{0}/{1}/{2}"); } } // {0}: author | {1}: repo name | {2}: artifact id
        public static string GithubReleaseArtifactsEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/service/githubreleaseartifacts/{0}/{1}/{2}"); } } // {0}: prerelease, stable | {1}: author | {2}: repo name


        public static async Task<GitReleaseArtifacts> GetGithubReleaseArtifacts(bool includePrereleases, string author, string repoName) {
            if (!Overrides.DirectGitHub) {
                return await CommsUtilities.GetJsonAsync<GitReleaseArtifacts>(String.Format(GithubReleaseArtifactsEndpoint, includePrereleases ? "prerelease" : "stable", author, repoName));
            } else {
                GitHubClient client = Overrides.GitHubClient;
                List<Release> releases;
                Release release;
                GitReleaseArtifacts artifacts;

                releases = (await client.Repository.Release.GetAll(author, repoName)).ToList();

                releases.OrderByDescending(x => x.CreatedAt.ToUnixTimeSeconds());

                if (includePrereleases) release = releases.FirstOrDefault();
                else release = releases.FirstOrDefault(x => !x.Prerelease);

                //there are no releases, or a stable release was requested but only prereleases exist
                if (release == default) throw new Exception("GetGithubReleaseArtifacts Override: No releases found with specified options.");

                artifacts = new GitReleaseArtifacts() {
                    Tag = release.TagName,
                    Artifacts = new Dictionary<string, int>()
                };

                foreach (ReleaseAsset k in release.Assets) {
                    artifacts.Artifacts.Add(k.Name, k.Id);
                }

                return artifacts;
            }
        }

        //TODO: Finish GitLab release integration
        //public static async Task<GitReleaseArtifacts> GetGitlabReleaseArtifacts(bool includePrereleases, string author, string repoName) {
        //    if (!Overrides.DirectGitLab) {
        //        //return await CommsUtilities.GetJsonAsync<GitReleaseArtifacts>(String.Format(GithubReleaseArtifactsEndpoint, includePrereleases ? "prerelease" : "stable", author, repoName));
        //    } else {
        //        GitLabClient client = Overrides.GitLabClient;
        //        GitReleaseArtifacts artifacts;
        //
        //        var project = await client.Projects.GetAsync($"{author}%2F{repoName}");
        //        var releases = await client.Releases.GetAsync(project.Id);
        //
        //        var release = releases.OrderByDescending(x => new DateTimeOffset(x.CreatedAt.Value).ToUnixTimeSeconds()).FirstOrDefault();
        //
        //
        //        //there are no releases, or a stable release was requested but only prereleases exist
        //        if (release == default) throw new Exception("GetGitlabReleaseArtifacts Override: No releases found with specified options.");
        //
        //        artifacts = new GitReleaseArtifacts() {
        //            Tag = release.TagName,
        //            Artifacts = new Dictionary<string, int>()
        //        };
        //
        //        foreach (var k in release.Assets.Links) {
        //            artifacts.Artifacts.Add(k.Name, k.Id);
        //        }
        //
        //        return artifacts;
        //    }
        //}

        public static async Task<MemoryStream> DownloadGithubArtifact(string author, string repoName, string artifactId) {
            MemoryStream ms = new MemoryStream();
            if (!Overrides.DirectGitHub) {
                Stream stream = await CommsUtilities.GetStreamAsync(String.Format(DownloadGithubArtifactEndpoint, author, repoName, artifactId));
                await stream.CopyToAsync(ms);
            } else {
                GitHubClient client = Overrides.GitHubClient;
                ReleaseAsset asset;

                asset = await client.Repository.Release.GetAsset(author, repoName, Convert.ToInt32(artifactId));

                using (HttpClient http = new HttpClient()) {
                    Stream stream = await http.GetStreamAsync(asset.BrowserDownloadUrl);
                    await stream.CopyToAsync(ms);
                }
            }
            return ms;
        }
    }
}
