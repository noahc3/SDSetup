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
using SDSetupCommon;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Octokit;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Authentication;

namespace SDSetupBackendRewrite.Controllers {
    [ApiController]
    [Route("api/v2/account")]
    public class AccountController : ControllerBase {

        private readonly IUrlHelper _urlHelper;
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IUrlHelper urlHelper, ILogger<AccountController> logger, IHttpClientFactory clientFactory) {
            _urlHelper = urlHelper;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet("githublogin"), AllowAnonymous]
        public async Task<IActionResult> GithubLogin() {
            return Redirect(
                "https://github.com/login/oauth/authorize?" +
                "client_id=" + Program.ActiveConfig.GithubClientId +
                "&redirect_uri=" + "http://files.sdsetup.com/api/v2/account/githublogincallback" +
                "&state=" + SDSetupCommon.Utilities.CreateCryptographicallySecureGuid().ToCleanString()
            );
        }

        [HttpGet("gitlablogin"), AllowAnonymous]
        public async Task<IActionResult> GitlabLogin() {
            return Redirect(
                "https://gitlab.com/oauth/authorize?" +
                "client_id=" + Program.ActiveConfig.GitlabClientId +
                "&redirect_uri=" + "http://files.sdsetup.com/api/v2/account/gitlablogincallback" +
                "&state=" + SDSetupCommon.Utilities.CreateCryptographicallySecureGuid().ToCleanString() +
                "&response_type=code" +
                "&scope=read_user"
            );
        }

        [HttpGet("githublogincallback"), AllowAnonymous]
        public async Task<IActionResult> GithubLoginCallback([FromQuery] string code, [FromQuery] string state) {

            SDSetupUser user = new SDSetupUser();
            await user.AuthenticateGithub(code, state);

            //check if the account is already linked to a user
            string id = await Program.Users.GetSDSetupIdByGithubId(user.GetGithubUserId());
            if (id.NullOrWhiteSpace()) {
                //if not, check if there is a session token in the request
                if (!Request.Cookies["session"].NullOrWhiteSpace()) {
                    //try to link the new login with the current user identified by the session token
                    if (!(await Program.Users.LinkUserFromGithub(Request.Cookies["session"], user))) {
                        //if that fails (because the session token is invalid or the user already has a linked github account)
                        //try to register a new user
                        if (!(await Program.Users.RegisterUserFromGithub(user))) return new StatusCodeResult(401);
                        user = await Program.Users.GetSDSetupUserById(user.SDSetupUserId);
                    } else {
                        //if that succeeds set to the existing user
                        user = await Program.Users.GetSDSetupUserBySessionToken(Request.Cookies["session"]);
                    }
                } else {
                    if (!(await Program.Users.RegisterUserFromGithub(user))) return new StatusCodeResult(401);
                    user = await Program.Users.GetSDSetupUserById(user.SDSetupUserId);
                }

            } else {
                SDSetupUser existingUser = await Program.Users.GetSDSetupUserById(id);
                await existingUser.UpdateGithubAuthentication(user);
                user = existingUser;
            }

            string sessionToken = await user.CreateSessionToken();

            Response.Cookies.Append("session", sessionToken);

            return new RedirectResult(Program.ActiveConfig.ManagerFrontendUrl, false);
        }

        [HttpGet("gitlablogincallback"), AllowAnonymous]
        public async Task<IActionResult> GitlabLoginCallback([FromQuery] string code, [FromQuery] string state) {

            SDSetupUser user = new SDSetupUser();
            await user.AuthenticateGitlab(code, state);

            //check if the account is already linked to a user
            string id = await Program.Users.GetSDSetupIdByGitlabId(user.GetGitlabUserId());
            if (id.NullOrWhiteSpace()) {
                //if not, check if there is a session token in the request
                if (!Request.Cookies["session"].NullOrWhiteSpace()) {
                    //try to link the new login with the current user identified by the session token
                    if (!(await Program.Users.LinkUserFromGitlab(Request.Cookies["session"], user))) {
                        //if that fails (because the session token is invalid or the user already has a linked gitlab account)
                        //try to register a new user
                        if (!(await Program.Users.RegisterUserFromGitlab(user))) return new StatusCodeResult(401);
                        user = await Program.Users.GetSDSetupUserById(user.SDSetupUserId);
                    } else {
                        //if that succeeds set to the existing user
                        user = await Program.Users.GetSDSetupUserBySessionToken(Request.Cookies["session"]);
                    }
                } else {
                    if (!(await Program.Users.RegisterUserFromGitlab(user))) return new StatusCodeResult(401);
                    user = await Program.Users.GetSDSetupUserById(user.SDSetupUserId);
                }
            } else {
                SDSetupUser existingUser = await Program.Users.GetSDSetupUserById(id);
                await existingUser.UpdateGitlabAuthentication(user);
                user = existingUser;
            }

            string sessionToken = await user.CreateSessionToken();

            Response.Cookies.Append("session", sessionToken);

            return new RedirectResult(Program.ActiveConfig.ManagerFrontendUrl, false);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout() {
            string sessionToken = Request.Cookies["session"];
            if (String.IsNullOrWhiteSpace(sessionToken)) return new StatusCodeResult(401);

            SDSetupUser user = await Program.Users.GetSDSetupUserBySessionToken(sessionToken);
            if (user != default(SDSetupUser)) await user.RevokeSessionToken(sessionToken);
            Response.Cookies.Delete("session");
            return new RedirectResult(Program.ActiveConfig.ManagerFrontendUrl, false);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile() {
            string sessionToken = Request.Cookies["session"];
            if (String.IsNullOrWhiteSpace(sessionToken)) 
                return new StatusCodeResult(401);

            SDSetupUser user = await Program.Users.GetSDSetupUserBySessionToken(sessionToken);
            if (user == default(SDSetupUser)) 
                return new StatusCodeResult(401);
            return new ObjectResult(await user.GetProfile());

        }

        [HttpPost("confirmregistration"), AllowAnonymous]
        public async Task<IActionResult> ConfirmRegistration(ExternalRegistrationConfirmationModel model) {
            //// Get the information about the user from the external login provider
            //var info = await _signInManager.GetExternalLoginInfoAsync();
            //if (info == null) {
            //    throw new ApplicationException("Error loading external login information during confirmation.");
            //}
            //var user = new SDSetupUser { UserName = model.Username, Email = model.Email, EmailConfirmed = true };
            //var result = await _userManager.CreateAsync(user);
            //if (result.Succeeded) {
            //    result = await _userManager.AddLoginAsync(user, info);
            //    if (result.Succeeded) {
            //        await _signInManager.SignInAsync(user, isPersistent: false);
            //        return Ok();
            //    }
            //}
            //return BadRequest();
            return StatusCode(StatusCodes.Status501NotImplemented);

        }
    }
}
