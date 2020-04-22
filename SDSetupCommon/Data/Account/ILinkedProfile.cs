using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.Account {
    public class ILinkedProfile {
        public LinkedService service { get; }
        public string userId { get; }
        public string username { get; }
        public string email { get; }
        public string bio { get; }
        public string avatarUrl { get; }
        public bool isPrimary { get; }

        public ILinkedProfile(LinkedService service, string userId, string username, string email, string bio, string avatarUrl, bool isPrimary) {
            this.service = service;
            this.userId = userId;
            this.username = username;
            this.email = email;
            this.bio = bio;
            this.avatarUrl = avatarUrl;
            this.isPrimary = isPrimary;
        }
    }
}
