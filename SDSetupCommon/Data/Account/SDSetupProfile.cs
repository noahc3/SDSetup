using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.Account {
    public class SDSetupProfile {
        public string userid { get; set; }
        public SDSetupRole userRole { get; set; }
        public bool hasLinkedGithub { get; set; }
        public bool hasLinkedGitlab { get; set; }
        public GithubProfile githubProfile { get; set; }
        public GitlabProfile gitlabProfile { get; set; }
        public ILinkedProfile primaryProfile { get; set; }
        public LinkedService primaryService { get; set; }
    }
}
