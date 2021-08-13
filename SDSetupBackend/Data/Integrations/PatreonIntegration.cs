using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SDSetupBackend.Data.Integrations {
    public class PatreonIntegration {
        public string Url = "";
        public int FundingCurrent = 0;
        public int FundingGoal = 1;
        public static PatreonIntegration GetPatreonData(string accessToken, string campaignId) {
            PatreonIntegration result = null;
            string query = $"?include=goals&fields[campaign]=url&fields[goal]=amount_cents,completed_percentage".Replace("[", "%5B").Replace("]", "%5D");
            string url = $"https://www.patreon.com/api/oauth2/v2/campaigns/{campaignId}{query}";
            string body;


            HttpWebRequest req = HttpWebRequest.CreateHttp(url);
            req.Headers["Authorization"] = $"Bearer {accessToken}";

            try {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                if (resp.StatusCode == HttpStatusCode.OK) {
                    using (var streamReader = new StreamReader(resp.GetResponseStream())) {
                        body = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(body);
                        result = new PatreonIntegration() {
                            Url = (string)obj.SelectToken("data.attributes.url"),
                            FundingCurrent = (int)obj.SelectToken("included[0].attributes.completed_percentage"),
                            FundingGoal = (int)obj.SelectToken("included[0].attributes.amount_cents")
                        };

                        result.FundingCurrent = (result.FundingCurrent * result.FundingGoal) / 100;
                    }
                }
            } catch (WebException e) {
                using (var streamReader = new StreamReader(e.Response.GetResponseStream())) {
                    string err = streamReader.ReadToEnd();
                    Program.logger.LogError("Failed to retrieve data from Patreon API: " + err);
                }
                
            }

            return result;
        }
    }
}
