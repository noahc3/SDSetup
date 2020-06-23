using Microsoft.AspNetCore.Http;
using SDSetupBackendRewrite.Data;
using SDSetupCommon.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackendRewrite {
    public class AuthorizationUtilities {

        //If the user is authenticated, returns the user. Otherwise returns null.
        public static async Task<SDSetupUser> CheckRequestAuthenticated(HttpRequest Request) {
            string sessionToken = Request.Cookies["session"];
            if (String.IsNullOrWhiteSpace(sessionToken)) return null;

            SDSetupUser user = await Program.Users.GetSDSetupUserBySessionToken(sessionToken);
            if (user == default(SDSetupUser)) return null;

            return user;
        }

        public static async Task<SDSetupUser> CheckRequestMinAuthorization(HttpRequest request, SDSetupRole minRoleInclusive) {
            SDSetupUser user = await CheckRequestAuthenticated(request);
            if (user == null) return null;
            if (user.SDSetupRole >= minRoleInclusive) return user;
            return null;
        }
    }
}
