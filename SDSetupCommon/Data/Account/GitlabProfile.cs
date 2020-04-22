using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.Account {
    public class GitlabProfile : ILinkedProfile {
        public GitlabProfile(string userId, string username, string email, string bio, string avatarUrl, bool isPrimary)
            : base(LinkedService.GitLab, userId, username, email, bio, avatarUrl, isPrimary) {
        }
    }
}
