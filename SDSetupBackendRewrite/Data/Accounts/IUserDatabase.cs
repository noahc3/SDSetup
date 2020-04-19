using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackendRewrite.Data.Accounts {
    public interface IUserDatabase {

        Task<string> GetSDSetupIdByGithubId(string githubId);
        Task<bool> UserExistsWithGithubId(string githubId);

        Task<string> GetSDSetupIdByGitlabId(string gitlabId);
        Task<bool> UserExistsWithGitlabId(string gitlabId);

        Task<SDSetupUser> GetSDSetupUserById(string sdsetupId);
        Task<SDSetupUser> GetSDSetupUserBySessionToken(string token);
        Task<bool> UserExistsWithSDSetupId(string sdsetupId);

        
        Task<bool> RegisterUserFromGithub(SDSetupUser user);
        Task<bool> RegisterUserFromGitlab(SDSetupUser user);



        
    }
}
