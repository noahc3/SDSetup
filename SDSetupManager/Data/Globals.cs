using SDSetupCommon.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SDSetupCommon.Communications;

namespace SDSetupManager.Data {
    public class Globals {
        public static bool Authenticated;
        public static SDSetupProfile UserProfile;

        public static async Task GlobalInit() {
            Authenticated = await TryGetProfile();
        }

        public static async Task<bool> TryGetProfile() {
            UserProfile = await AccountEndpoints.Profile();
            return UserProfile != default(SDSetupProfile);
        }
    }
}
