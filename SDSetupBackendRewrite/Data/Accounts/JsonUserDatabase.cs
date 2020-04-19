using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackendRewrite.Data.Accounts {
    /// <summary>
    /// Implementation of IUserDatabase that writes all data to a specified json file. 
    /// Designed only for development environments and should not be used in production for many reasons!
    /// </summary>
    public class JsonUserDatabase : IUserDatabase {

        List<SDSetupUser> users = new List<SDSetupUser>();

        public async Task<string> GetSDSetupIdByGithubId(string githubId) {
            return await Task.Run(() => {
                SDSetupUser user = users.FirstOrDefault(x => githubId == x.GetGithubUserId());
                if (user == default(SDSetupUser)) return "";
                else return user.GetSDSetupUserId(); 
            });
        }

        public async Task<string> GetSDSetupIdByGitlabId(string gitlabId) {
            return await Task.Run(() => {
                SDSetupUser user = users.FirstOrDefault(x => gitlabId == x.GetGitlabUserId());
                if (user == default(SDSetupUser)) return "";
                else return user.GetSDSetupUserId();
            });
        }

        public async Task<SDSetupUser> GetSDSetupUserById(string sdsetupId) {
            return await Task.Run(() => {
                SDSetupUser user = users.FirstOrDefault(x => sdsetupId == x.GetSDSetupUserId());
                return user;
            });
        }

        public async Task<SDSetupUser> GetSDSetupUserBySessionToken(string token) {
            return await Task.Run(() => {
                SDSetupUser user = users.FirstOrDefault(x => x.ValidSessionToken(token));
                return user;
            });
        }

        /// <summary>
        /// Registers an SDSetup user from an SDSetupUser object authenticated with GitHub.
        /// </summary>
        /// <param name="user">The SDSetupUser object authenticated with GitHub.</param>
        /// <returns>Returns Task<string>: a blank string if the GitHub ID is already linked to an SDSetup user, otherwise returns the new SDSetup user ID.</returns>
        public async Task<bool> RegisterUserFromGithub(SDSetupUser user) {
            if (await UserExistsWithGithubId(user.GetGithubUserId()))
                return false;

            if (await user.IsAuthenticatedWithGithub()) {
                users.Add(user);
                return true;
            }

            return false;
        }

        public async Task<bool> RegisterUserFromGitlab(SDSetupUser user) {
            if (await UserExistsWithGitlabId(user.GetGitlabUserId()))
                return false;

            if (await user.IsAuthenticatedWithGitlab()) {
                users.Add(user);
                return true;
            }

            return false;
        }

        public async Task<bool> UserExistsWithGithubId(string githubId) {
            return !String.IsNullOrWhiteSpace(await GetSDSetupIdByGithubId(githubId));
        }

        public async Task<bool> UserExistsWithGitlabId(string gitlabId) {
            return !String.IsNullOrWhiteSpace(await GetSDSetupIdByGitlabId(gitlabId));
        }

        public Task<bool> UserExistsWithSDSetupId(string sdsetupId) {
            throw new NotImplementedException();
        }
    }
}
