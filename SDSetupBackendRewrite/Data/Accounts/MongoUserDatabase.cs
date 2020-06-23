using MongoDB.Driver;
using MongoDB.Bson;
using SDSetupCommon;
using SDSetupCommon.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackendRewrite.Data.Accounts {
    public class MongoUserDatabase : IUserDatabase {

        private static MongoClient MongoClient;
        private static IMongoDatabase MongoDatabase;
        public static IMongoCollection<SDSetupUser> Users;

        public MongoUserDatabase() {
            MongoClient = new MongoClient(new MongoClientSettings() {
                UseTls = false,
                Credential = MongoCredential.CreateCredential(Program.ActiveConfig.MongoDBDatabase, Program.ActiveConfig.MongoDBUsername, Program.ActiveConfig.MongoDBPassword),
                Server = new MongoServerAddress(Program.ActiveConfig.MongoDBHostname)
            });

            MongoDatabase = MongoClient.GetDatabase(Program.ActiveConfig.MongoDBDatabase);
            Users = MongoDatabase.GetCollection<SDSetupUser>("Users");
        }

        public async Task<string> GetSDSetupIdByGithubId(string githubId) {
            return await Task.Run(() => {
                SDSetupUser user = Users.Find(Builders<SDSetupUser>.Filter.Eq(x => x.LinkedGithubId, githubId)).FirstOrDefault();
                if (user == default(SDSetupUser)) return "";
                else return user.GetSDSetupUserId();
            });
        }

        public async Task<string> GetSDSetupIdByGitlabId(string gitlabId) {
            return await Task.Run(() => {
                SDSetupUser user = Users.Find(Builders<SDSetupUser>.Filter.Eq(x => x.LinkedGitlabId, gitlabId)).FirstOrDefault();
                if (user == default(SDSetupUser)) return "";
                else return user.GetSDSetupUserId();
            });
        }

        public async Task<SDSetupUser> GetSDSetupUserById(string sdsetupId) {
            return await Task.Run(() => {
                SDSetupUser user = Users.Find(Builders<SDSetupUser>.Filter.Eq(x => x.SDSetupUserId, sdsetupId)).FirstOrDefault();
                return user;
            });
        }

        public async Task<SDSetupUser> GetSDSetupUserBySessionToken(string token) {
            return await Task.Run(() => {
                SDSetupUser user = Users.Find(Builders<SDSetupUser>.Filter.Eq(x => x.SessionToken, token)).FirstOrDefault();
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
                await Users.InsertOneAsync(user);
                await SetPrimaryService(user.GetSDSetupUserId(), LinkedService.GitHub);
                return true;
            }

            return false;
        }

        public async Task<bool> RegisterUserFromGitlab(SDSetupUser user) {
            if (await UserExistsWithGitlabId(user.GetGitlabUserId()))
                return false;

            if (await user.IsAuthenticatedWithGitlab()) {
                await Users.InsertOneAsync(user);
                await SetPrimaryService(user.GetSDSetupUserId(), LinkedService.GitLab);
                return true;
            }

            return false;
        }

        public async Task<bool> LinkUserFromGithub(string sessionToken, SDSetupUser user) {
            SDSetupUser existingUser = await GetSDSetupUserBySessionToken(sessionToken);
            if (existingUser != default(SDSetupUser)) {
                if (existingUser.GetGithubUserId().NullOrWhiteSpace()) {
                    if (await user.IsAuthenticatedWithGithub()) {
                        await existingUser.UpdateGithubAuthentication(user);
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> LinkUserFromGitlab(string sessionToken, SDSetupUser user) {
            SDSetupUser existingUser = await GetSDSetupUserBySessionToken(sessionToken);
            if (existingUser != default(SDSetupUser)) {
                if (existingUser.GetGitlabUserId().NullOrWhiteSpace()) {
                    if (await user.IsAuthenticatedWithGitlab()) {
                        await existingUser.UpdateGitlabAuthentication(user);
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> SetPrimaryService(string userId, LinkedService service) {
            SDSetupUser user = await GetSDSetupUserById(userId);
            if (user != default(SDSetupUser)) {
                await user.SetPrimaryService(service);

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

        public async Task<bool> UpdateUser(SDSetupUser user) {
            return (await Users.ReplaceOneAsync(x => x.SDSetupUserId == user.SDSetupUserId, user)).MatchedCount > 0;
        }
    }
}
