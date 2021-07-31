using Octokit;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class Overrides {
        public static bool DirectGitHub = false;
        public static bool DirectGitLab = false;
        public static GitHubClient GitHubClient;
        

        public static bool UseDirectGitHub(string clientId, string clientSecret) {
            GitHubClient = new GitHubClient(new ProductHeaderValue("SDSetupCommon"));
            try {
                Credentials credentials = new Credentials(clientId, clientSecret);
                GitHubClient.Credentials = credentials;
                DirectGitHub = true;
            } catch {
                throw new AuthenticationException("Failed to authenticate with GitHub.");
            }

            return true;
        }
    }
}
