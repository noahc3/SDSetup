using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Octokit;
using SDSetupBackend.Data;
using SDSetupCommon;
using SDSetupCommon.Data;
using SDSetupCommon.Data.Account;
using SDSetupCommon.Data.PackageModels;
using SDSetupCommon.Data.ServiceModels;

namespace SDSetupBackend.Controllers.v2 {
    [ApiController]
    [Route("api/v2/service")]
    public class ServiceController : ControllerBase {

        private readonly ILogger<GeneralController> _logger;

        public ServiceController(ILogger<GeneralController> logger) {
            _logger = logger;
        }

        [HttpGet("githubreleaseartifacts/{type}/{author}/{repo}")]
        public async Task<IActionResult> GithubReleaseArtifact([FromRoute] string type, [FromRoute] string author, [FromRoute] string repo) {
            SDSetupUser user = await AuthorizationUtilities.CheckRequestMinAuthorization(Request, SDSetupRole.Developer);
            if (user == null) return new StatusCodeResult(401); //unauthorized

            GitHubClient client = await user.GetGithubClient();
            if (client == null) return new StatusCodeResult(401);

            List<Release> releases;

            try {
                releases = (await client.Repository.Release.GetAll(author, repo)).ToList();
            } catch (Exception e) {
                //likely the repo doesnt exist, so return 404
                return new StatusCodeResult(404);
            }

            Release release;

            releases.OrderByDescending(x => x.CreatedAt.ToUnixTimeSeconds());

            if (type == "prerelease") release = releases.FirstOrDefault();
            else release = releases.FirstOrDefault(x => !x.Prerelease);

            //there are no releases, or a stable release was requested but only prereleases exist
            if (release == default) return new StatusCodeResult(404);

            GitReleaseArtifacts artifacts = new GitReleaseArtifacts() {
                Tag = release.TagName,
                Artifacts = new Dictionary<string, int>()
            };

            foreach (ReleaseAsset k in release.Assets) {
                artifacts.Artifacts.Add(k.Name, k.Id);
            }

            return new ObjectResult(JsonConvert.SerializeObject(artifacts));
        }

        [HttpGet("githubartifactdownload/{author}/{repo}/{artifactId}")]
        public async Task<IActionResult> GithubArtifactDownload([FromRoute] string author, [FromRoute] string repo, [FromRoute] int artifactId) {
            SDSetupUser user = await AuthorizationUtilities.CheckRequestMinAuthorization(Request, SDSetupRole.Developer);
            if (user == null) return new StatusCodeResult(401); //unauthorized

            GitHubClient client = await user.GetGithubClient();
            if (client == null) return new StatusCodeResult(401);

            ReleaseAsset asset;

            try {
                asset = await client.Repository.Release.GetAsset(author, repo, artifactId);
            } catch (Exception e) {
                //likely the repo doesnt exist, so return 404
                return new StatusCodeResult(404);
            }

            using (HttpClient http = new HttpClient()) {
                return new FileStreamResult(await http.GetStreamAsync(asset.BrowserDownloadUrl), new MediaTypeHeaderValue("application/octet-stream"));
            }
        }
    }
}
