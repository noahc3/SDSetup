using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.Account {
    public class ILinkedProfile {
        public string serviceName { get; }
        public string userId { get; }
        public string username { get; }
        public string email { get; }
        public string bio { get; }
        public string avatarUrl { get; }

        public ILinkedProfile(string serviceName, string userId, string username, string email, string bio, string avatarUrl) {
            this.serviceName = serviceName;
            this.userId = userId;
            this.username = username;
            this.email = email;
            this.bio = bio;
            this.avatarUrl = avatarUrl;
        }
    }
}
