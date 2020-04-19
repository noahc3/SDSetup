using SDSetupCommon.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SDSetupCommon.Communications;

namespace SDSetupWorkbench.Data {
    public class Globals {
        public static bool Authenticated;
        public static SDSetupProfile UserProfile;

        public static async Task GlobalInit() {
            Authenticated = await TryGetProfile();
            Console.WriteLine(Authenticated);
        }

        public static async Task<bool> TryGetProfile() {
            UserProfile = await AccountEndpoints.Profile();
            if (UserProfile != default(SDSetupProfile)) Console.WriteLine(UserProfile.userid);
            return UserProfile != default(SDSetupProfile);
        }
    }
}
