using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace SDSetupCommon.Communications {
    public class CommsUtilities {
        public static string FullApiEndpoint(string endpoint) {
            return EndpointSettings.serverInformation.Hostname + endpoint;
        }

        public static async Task<Stream> GetStreamAsync(string endpoint) {
            HttpResponseMessage response = await EndpointSettings.HttpClient.GetAsync(new Uri(endpoint));
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsStreamAsync();
            } else {
                return default;
            }
        }

        public static async Task<string> GetStringAsync(string endpoint) {
            HttpResponseMessage response = await EndpointSettings.HttpClient.GetAsync(new Uri(endpoint));
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsStringAsync();
            } else {
                return default;
            }
        }

        public static async Task<ReturnType> GetJsonAsync<ReturnType>(string endpoint) {
            HttpResponseMessage response = await EndpointSettings.HttpClient.GetAsync(new Uri(endpoint));
            if (response.IsSuccessStatusCode) {
                return await Task.Run<ReturnType>(async () => {
                    return JsonConvert.DeserializeObject<ReturnType>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
                });
            } else {
                return default;
            }
        }

        public static async Task<ReturnType> PostJsonAsync<ReturnType, PostType>(string endpoint, PostType postData) {
            string json = JsonConvert.SerializeObject(postData, typeof(PostType), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await EndpointSettings.HttpClient.PostAsync(endpoint, content);
            if (response.IsSuccessStatusCode) {
                return await Task.Run<ReturnType>(async () => {
                    return JsonConvert.DeserializeObject<ReturnType>(await response.Content.ReadAsStringAsync());
                });
            } else {
                return default;
            }
        }

        public static async Task<HttpStatusCode> PostJsonAsync<PostType>(string endpoint, PostType postData) {
            string json = JsonConvert.SerializeObject(postData, typeof(PostType), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await EndpointSettings.HttpClient.PostAsync(endpoint, content);
            return response.StatusCode;
        }
    }
}
