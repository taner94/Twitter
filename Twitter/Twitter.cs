using Nancy.Json;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
    public class Twitter
    {
        private const string ApiBaseUrl = "https://api.twitter.com";
        public string OAuthConsumerSecret { get; set; }
        public string OAuthConsumerKey { get; set; }
        public async Task<string> GetTwittersAsync(string userName, int count, string accessToken = null)
        {
            if (accessToken == "")
            {
                accessToken = await GetAccessToken();
            }
            userName = "****";// postların cekilecegi hesap verilmeli
            var response = GetUserTimelineJson(accessToken, userName);
            dynamic jsonResponse = JsonConvert.DeserializeObject(response);
            return response;
        }
        public string GetUserTimelineJson(string bearerToken, string screenName, int count = 10, bool excludeReplies = false, bool includeRTs = true)
        {
            var webrequest = CreateRequest(
                "/1.1/statuses/user_timeline.json"
                + "?screen_name=" + screenName + "&count=" + count + "&exclude_replies="
                + excludeReplies.ToString().ToLower() + "&include_rts=" + includeRTs.ToString().ToLower());
            webrequest.Headers.Add("Authorization", "Bearer " + bearerToken);
            webrequest.Method = WebRequestMethods.Http.Get;

            return ReadResponse(webrequest);
        }
        private static WebRequest CreateRequest(string url)
        {
            var webrequest = WebRequest.Create(ApiBaseUrl + url);
            ((HttpWebRequest)webrequest).UserAgent = "timacheson.com";
            return webrequest;
        }
        private static string ReadResponse(WebRequest webrequest)
        {
            using (var responseStream = webrequest.GetResponse().GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var responseReader = new StreamReader(responseStream))
                    {
                        return responseReader.ReadToEnd();
                    }
                }
            }
            return null;
        }
        public async Task<string> GetAccessToken()
        {
            OAuthConsumerKey = "********";// bu kısımlar twitter devoloper hesabından alınmalı

            OAuthConsumerSecret = "*****";
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            return item["access_token"];
        }
    }
}
