using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.IntegrationModels {
    public class DonationModel {
        public string KofiUrl { get; set; } = "";
        public string PatreonUrl { get; set; } = "";
        public int PatreonFundingCurrent { get; set; } = 0;
        public int PatreonFundingGoal { get; set; } = 1;
    }
}
