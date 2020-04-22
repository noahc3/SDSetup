using SDSetupCommon.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.Account {
    public class GithubProfile : ILinkedProfile {

        public GithubProfile(string userId, string username, string email, string bio, string avatarUrl, bool isPrimary) 
            : base(LinkedService.GitHub, userId, username, email, bio, avatarUrl, isPrimary) {
        }
    }
}
