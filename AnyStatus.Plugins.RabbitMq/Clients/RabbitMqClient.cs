using AnyStatus.Plugins.RabbitMq.Nodes.Contracts;
using AnyStatus.Plugins.RabbitMq.Policies.Contracts;
using AnyStatus.Plugins.RabbitMq.QueueStats.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AnyStatus.Plugins.RabbitMq.Clients
{
    public class RabbitMqClient
    {
        private readonly string _baseAddress;
        private readonly string _userName;
        private readonly string _password;
        private static readonly HttpClient _httpClient = new HttpClient();

        public RabbitMqClient(string baseAddress, string userName, string password)
        {
            _baseAddress = baseAddress;
            _userName = userName;
            _password = password;
        }

        public Task<IEnumerable<Policy>> GetPoliciesAsync(string policiesPath, string vhostName)
        {
            var virtualHost = HttpUtility.UrlEncode(vhostName, Encoding.UTF8);

            return GetAsync<IEnumerable<Policy>>(policiesPath + "/" + virtualHost);
        }

        public Task<IEnumerable<QueueStatistics>> GetQueueStatisticsAsync(string queuesPath, string vhostName)
        {
            var virtualHost = HttpUtility.UrlEncode(vhostName, Encoding.UTF8);

            return GetAsync<IEnumerable<QueueStatistics>>(queuesPath + "/" + virtualHost);
        }

        public Task<IEnumerable<NodeInfo>> GetNodeInfosAsync(string nodesPath)
        {
            return GetAsync<IEnumerable<NodeInfo>>(nodesPath);
        }

        public Task<NodeInfo> GetNodeInfoAsync(string nodePath, string nodeName)
        {
            return GetAsync<NodeInfo>(nodePath + "/" + nodeName);
        }

        private async Task<T> GetAsync<T>(string path)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(_baseAddress + path));
            httpRequest.Headers.Authorization = GetAuthorization(_userName, _password);

            using (var response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                var str = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(str);
            }
        }

        private static AuthenticationHeaderValue GetAuthorization(string userName, string password)
        {
            return new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}")));
        }
    }
}