using SDSetupCommon.Data;
using SDSetupCommon.Data.Account;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class AccountEndpoints {
        public static string ExternalLoginEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/account/externallogin"); } }
        public static string LoginProvidersEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/account/loginproviders"); } }
        public static string ExternalRegistrationDetailsEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/account/externalregistrationdetails"); } }
        public static string ProfileEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/account/profile"); } }
        public static string LogoutEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/account/logout"); } }
        public static async Task<List<SignInProviderViewModel>> LoginProviders() {
            return await CommsUtilities.GetJsonAsync<List<SignInProviderViewModel>>(LoginProvidersEndpoint);
        }

        public static async Task<ExternalRegistrationConfirmationModel> ExternalRegistrationDetails() {
            return await CommsUtilities.GetJsonAsync<ExternalRegistrationConfirmationModel>(ExternalRegistrationDetailsEndpoint);
        }

        public static async Task<SDSetupProfile> Profile() {
            return await CommsUtilities.GetJsonAsync<SDSetupProfile>(ProfileEndpoint);
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
