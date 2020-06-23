using SDSetupCommon.Communications;
using SDSetupCommon.Data.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Enumeration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class GitReleaseDownloadTask : UpdaterTask {
        public GitReleaseSource gitReleaseSource = GitReleaseSource.GitHub;
        public GitReleaseType gitReleaseType = GitReleaseType.Stable;
        public string repoAuthor = "";
        public string repoName = "";
        public MatchType matchType = MatchType.MatchRegex;
        public string matchParameter = "";
        public string downloadPath = "";
        public bool createDownloadPathIfMissing = true;
        public bool extractArchives = true;

        public async Task Apply(string root) {
            DirectoryInfo target = new DirectoryInfo(Path.Join(root, downloadPath));

            //TODO: GitLab support
            Dictionary<int, string> matchedArtifacts = new Dictionary<int, string>();
            if (gitReleaseSource == GitReleaseSource.GitHub) {
                GitReleaseArtifacts artifacts = await ServiceEndpoints.GetGithubReleaseArtifacts(gitReleaseType == GitReleaseType.Prerelease, repoAuthor, repoName);
                foreach(KeyValuePair<string, int> kvp in artifacts.Artifacts) {
                    if (Regex.IsMatch(kvp.Key, matchParameter) == (matchType == MatchType.MatchRegex)) {
                        matchedArtifacts.Add(kvp.Value, kvp.Key.Split('/').Last());
                    }
                }
            } else {
                throw new PlatformNotSupportedException("GitLab is not supported as a tag source for GitReleaseDownloadTask:Apply()");
            }

            if (matchedArtifacts.Count() == 0) return;

            if (!target.Exists) target.Create();

            foreach(KeyValuePair<int, string> kvp in matchedArtifacts) {
                using (MemoryStream ms = await ServiceEndpoints.DownloadGithubArtifact(repoAuthor, repoName, kvp.Key.ToString())) {
                    if (!extractArchives || !kvp.Value.EndsWith(".zip")) {
                        File.WriteAllBytes(Path.Join(target.FullName, kvp.Value), ms.GetBuffer());
                        continue;
                    }

                    using (ZipArchive zip = new ZipArchive(ms)) {
                        zip.ExtractToDirectory(target.FullName);
                    }
                }
            }
        }
    }
}
