using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackendRewrite.Data.Accounts {
    public class GitlabTokenResponse {
        public string access_token;
        public string token_type;
        public string expires_in;
        public string refresh_token;
    }
}
