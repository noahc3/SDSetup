using SDSetupCommon.Communications;
using SDSetupCommon.Data.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class GitReleaseTagVersionSource : VersionSource {
        public GitReleaseSource gitReleaseSource = GitReleaseSource.GitHub;
        public GitReleaseType gitReleaseType = GitReleaseType.Stable;
        public string repoAuthor = "";
        public string repoName = "";
        public string regexFilter = ".*";
        public string format = "{tag}";

        public async Task<string> GetVersion() {
            GitReleaseArtifacts artifacts;
            string tag;
            Match match;

            //TODO: Gitlab support.
            if (gitReleaseSource == GitReleaseSource.GitHub) {
                artifacts = await ServiceEndpoints.GetGithubReleaseArtifacts(gitReleaseType == GitReleaseType.Prerelease, repoAuthor, repoName);
            } else {
                throw new PlatformNotSupportedException("GitLab is not supported as a tag source for GitReleaseTagVersionSource:Assert()");
            }

            tag = artifacts.Tag;
            match = Regex.Match(tag, regexFilter);

            if (!match.Success) {
                throw new Exception("GitReleaseVersionSource: Could not match regex.");
            }

            tag = match.Value;
            tag = format.Replace("{tag}", tag);

            return tag;
        }
    }
}
