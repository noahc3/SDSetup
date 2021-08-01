using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SDSetupCommon.Data.UpdaterModels;

namespace SDSetupCommon.Data.PackageModels {
    public class Package {
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Platform { get; set; } = "";
        public string Section { get; set; } = "";
        public string Category { get; set; } = "";
        public string Subcategory { get; set; } = "";
        public string Authors { get; set; } = "";
        public long Downloads { get; set; } = 0;
        public VersionInfo VersionInfo { get; set; }
        public string Source { get; set; } = "";
        public string License { get; set; } = "";
        public int Priority { get; set; } = 0;
        public bool EnabledByDefault { get; set; } = false;
        public bool Visible { get; set; } = true;
        public bool ShowsInCredits { get; set; } = true;
        public string Description { get; set; } = "";
        public List<string> When { get; set; } = new List<string>();
        public int WhenMode { get; set; } = 0; //0: all | 1: any
        public Warning Warning { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();
        public List<string> DeleteOnUpdate { get; set; } = new List<string>();

        public bool AutoUpdates { get; set; } = false;

        public List<UpdaterTask> AutoUpdateTasks { get; set; } = new List<UpdaterTask>();
        public List<UpdaterTrigger> AutoUpdateTriggers { get; set; } = new List<UpdaterTrigger>();
        public List<UpdaterCondition> AutoUpdateConditions { get; set; } = new List<UpdaterCondition>();
        public VersionSource AutoUpdateVersionSource { get; set; }

        public Package() {

        }

        //TODO: Implement Package::Validate()
        public bool Validate() {
            return true;
        }

        public Package Copy() {
            string copy = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Package>(copy, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
