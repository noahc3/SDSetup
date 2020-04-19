using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupUpdater {
    class Config {
        public string GithubUsername { get; set; }
        public string GithubAuthToken { get; set; }
        public string GitlabAuthToken { get; set; }
        public string WebhookEndpoint { get; set; }
        public string BackendHostname { get; set; }
        public string LibGetRepo { get; set; }
        public string KosmosScriptPath { get; set; }
        public string KosmosUpdaterBuilderSubdirectory { get; set; }
        public long KosmosRepositoryId { get; set; }
        public string KosmosMasterUrl { get; set; }
        public List<string> OldPackagesets { get; set; }
    }
}
