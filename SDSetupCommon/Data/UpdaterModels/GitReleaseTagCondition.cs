using SDSetupCommon.Communications;
using SDSetupCommon.Data.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class GitReleaseTagCondition : UpdaterCondition {

        public GitReleaseSource gitReleaseSource = GitReleaseSource.GitHub;
        public GitReleaseType gitReleaseType = GitReleaseType.Stable;
        public string repoAuthor = "";
        public string repoName = "";

        public async Task<bool> Assert(string previousTag) {
            //TODO: GitLab support
            GitReleaseArtifacts artifacts = null;
            if (gitReleaseSource == GitReleaseSource.GitHub) {
                artifacts = await ServiceEndpoints.GetGithubReleaseArtifacts(gitReleaseType == GitReleaseType.Prerelease, repoAuthor, repoName);
            } else {
                throw new PlatformNotSupportedException("GitLab is not supported as a tag source for GitReleaseTagCondition:Assert()");
            }
            return previousTag != artifacts?.Tag;
        }
    }
}
