using SDSetupCommon.Data;
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
        public static async Task<List<SignInProviderViewModel>> LoginProviders() {
            return await CommsUtilities.GetAsync<List<SignInProviderViewModel>>(CommsUtilities.FullApiEndpoint(LoginProvidersEndpoint));
        }

        public static async Task<ExternalRegistrationConfirmationModel> ExternalRegistrationDetails() {
            return await CommsUtilities.GetAsync<ExternalRegistrationConfirmationModel>(CommsUtilities.FullApiEndpoint(ExternalRegistrationDetailsEndpoint));
        }
    }
}
