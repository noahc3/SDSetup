using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.ServiceModels {
    public class GitReleaseArtifacts {
        public string Tag;
        public Dictionary<string, int> Artifacts;
    }
}
