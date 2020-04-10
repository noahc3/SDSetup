using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDSetupCommon.Data;
using SDSetupBackendRewrite.Providers;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SDSetupBackendRewrite.Data;
using SDSetupCommon.Data;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authentication;

namespace SDSetupBackendRewrite.Controllers {
    [ApiController]
    [Route("api/v2/account")]
    public class AccountController : ControllerBase {

        private readonly IUrlHelper _urlHelper;
        private readonly UserManager<SDSetupUser> _userManager;
        private readonly SignInManager<SDSetupUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(UserManager<SDSetupUser> userManager, SignInManager<SDSetupUser> signInManager, IUrlHelper urlHelper, ILogger<AccountController> logger) {
            _userManager = userManager;
            _signInManager = signInManager;
            _urlHelper = urlHelper;
            _logger = logger;
        }

        [HttpGet("loginproviders"), AllowAnonymous]
        public async Task<IActionResult> LoginProviders() {
            return Ok((await _signInManager.GetExternalAuthenticationSchemesAsync()).Select(s => new SignInProviderViewModel() { 
                Name = s.Name,
                DisplayName = s.DisplayName
            }));
        }

        [HttpPost("externallogin"), AllowAnonymous]
        public async Task<IActionResult> ExternalLogin([FromForm] string provider) {
            string redirectUrl = _urlHelper.Action("ExternalLoginCallback", "AccountController");
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("externallogincallback"), AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null) {
            Console.WriteLine("0");
            //Get the login info from the extrernal provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) {
                Console.WriteLine("1");
                return null;
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result == null || result.IsNotAllowed) {
                Console.WriteLine("2");
                return Redirect(Program.ActiveConfig.ManagerFrontendUrl + "/login");
            }

            if (result.Succeeded) {
                Console.WriteLine("3");
                return Redirect(Program.ActiveConfig.ManagerFrontendUrl);
            }
            Console.WriteLine("4");
            // If the user does not have an account, then ask the user to create an account.
            return Redirect(Program.ActiveConfig.ManagerFrontendUrl + "/account/confirmregistration");
        }

        [HttpPost("confirmregistration"), AllowAnonymous]
        public async Task<IActionResult> ConfirmRegistration(ExternalRegistrationConfirmationModel model) {
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) {
                throw new ApplicationException("Error loading external login information during confirmation.");
            }
            var user = new SDSetupUser { UserName = model.Username, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded) {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}
