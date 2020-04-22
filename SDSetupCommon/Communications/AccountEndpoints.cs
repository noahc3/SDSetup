using SDSetupCommon.Data;
using SDSetupCommon.Data.Account;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class AccountEndpoints {
        public static readonly string ExternalLoginEndpoint = "/api/v2/account/externallogin";
        public static readonly string LoginProvidersEndpoint = "/api/v2/account/loginproviders";
        public static readonly string ExternalRegistrationDetailsEndpoint = "/api/v2/account/externalregistrationdetails";
        public static readonly string ProfileEndpoint = "/api/v2/account/profile";
        public static async Task<List<SignInProviderViewModel>> LoginProviders() {
            return await CommsUtilities.GetAsync<List<SignInProviderViewModel>>(CommsUtilities.FullApiEndpoint(LoginProvidersEndpoint));
        }

        public static async Task<ExternalRegistrationConfirmationModel> ExternalRegistrationDetails() {
            return await CommsUtilities.GetAsync<ExternalRegistrationConfirmationModel>(CommsUtilities.FullApiEndpoint(ExternalRegistrationDetailsEndpoint));
        }

        public static async Task<SDSetupProfile> Profile() {
            return await CommsUtilities.GetAsync<SDSetupProfile>(CommsUtilities.FullApiEndpoint(ProfileEndpoint));
        }

        public static string GetLoginEndpoint(LinkedService service) {
            switch (service) {
                case LinkedService.GitHub:
                    return CommsUtilities.FullApiEndpoint("/api/v2/account/githublogin");
                case LinkedService.GitLab:
                    return CommsUtilities.FullApiEndpoint("/api/v2/account/gitlablogin");
                default:
                    return "";
            }
        }
    }
}
