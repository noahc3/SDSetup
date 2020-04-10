using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDSetupBackendRewrite.Data {
    public class SDSetupUserStore : IUserStore<SDSetupUser> {
        public Task<IdentityResult> CreateAsync(SDSetupUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(SDSetupUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
        }

        public Task<SDSetupUser> FindByIdAsync(string userId, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<SDSetupUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(SDSetupUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(SDSetupUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(SDSetupUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(SDSetupUser user, string normalizedName, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(SDSetupUser user, string userName, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(SDSetupUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
