using Demo.Shared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Demo.Data
{
    public class ApiClient : IHttpClient
    {
        public ApiClient(HttpClient client)
        {
            _httpClient = client;
            _endpoints = new HashSet<string>();

            // Default is 2 minutes: https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.dnsrefreshtimeout(v=vs.110).aspx
            ServicePointManager.DnsRefreshTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            // Increases the concurrent outbound connections
            ServicePointManager.DefaultConnectionLimit = 1024;
        }

        /// <summary>
        /// Makes a rest api request
        /// </summary>
        /// <param name="address">URI address</param>
        /// <param name="optional">Optional request parameters (method, bearer token, etc)</param>
        /// <returns>Response content in string format</returns>
        public async Task<string> RequestAsync(string address, IDictionary<string, string> optional)
        {
            HttpResponseMessage response = await MakeRequest(address, optional);
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        /// <summary>
        /// Makes a rest api request
        /// </summary>
        /// <param name="address">URI address</param>
        /// <param name="optional">Optional request parameters (method, bearer token, etc)</param>
        /// <returns>Response content in provided type</returns>
        public async Task<T> RequestAsync<T>(string address, IDictionary<string, string> optional,
            bool handleTypes = false) where T : class, new()
        {
            HttpResponseMessage response = await MakeRequest(address, optional);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (handleTypes)
                return JsonConvert.DeserializeObject<T>(responseContent,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            else
                return JsonConvert.DeserializeObject<T>(responseContent);
        }

        private async Task<HttpResponseMessage> MakeRequest(string address, IDictionary<string, string> optional)
        {
            AddConnectionLeaseTimeout(address);

            // todo: refactor this to use an enum with possible optional parameters instead of strings
            var method = optional.ContainsKey("method") ? optional["method"] : "get";
            var req = new HttpRequestMessage(new HttpMethod(method), address);

            if (optional.ContainsKey("bearer"))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", optional["bearer"]);
            if (optional.ContainsKey("content"))
                req.Content = new StringContent(optional["content"]);

            var response = await _httpClient.SendAsync(req);

            if (response.IsSuccessStatusCode)
                return response;
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(responseContent);
            }
        }

        private void AddConnectionLeaseTimeout(string endpoint)
        {
            lock (_endpoints)
            {
                if (_endpoints.Contains(endpoint))
                    return;

                ServicePointManager.FindServicePoint(new Uri(endpoint)).ConnectionLeaseTimeout
                    = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

                _endpoints.Add(endpoint);
            }
        }

        private readonly HttpClient _httpClient;
        private readonly HashSet<string> _endpoints;
    }
}
