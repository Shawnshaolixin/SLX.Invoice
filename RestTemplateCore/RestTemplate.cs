using Consul;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestTemplateCore
{
    /// <summary>
    /// 会自动到Consul 中解析服务的Rest客户端，能把http://ProductService/api/Product/ 这样的虚拟地址
    /// 按照客户端负载均衡算法解析为 http://192.168.1.10:8080/api/Product 这样的真实地址
    /// 
    /// </summary>
    public class RestTemplate
    {
        public string ConsulServerUrl { get; set; } = "http://127.0.0.1:8500";
        private HttpClient httpClient;
        public RestTemplate(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        /// <summary>
        /// 获取服务的第一个实现地址
        /// </summary>
        /// <param name="serivceName"></param>
        /// <returns></returns>
        private async Task<String> ResolveRootUrlAsync(string serviceName)
        {
            using (var consulClient = new ConsulClient(c => c.Address = new Uri(ConsulServerUrl)))
            {
                var services = (await consulClient.Agent.Services()).Response.Values
                    .Where(s => s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase));

                if (!services.Any())
                {
                    throw new ArgumentException($"找不到服务{serviceName}的任何实例");
                }
                else
                {
                    var service = services.ElementAt(Environment.TickCount % services.Count());
                    return $"{service.Address}:{service.Port}";
                    ;
                }
            }
        }
        /// <summary>
        ///   把 http://apiservice1/api/values 转换为 http://192.168.1.1:5000/api/values 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> ResolveUrlAsync(string url)
        {
            Uri uri = new Uri(url);
            string serviceName = uri.Host;//apiservice1
            string realRootUrl = await ResolveRootUrlAsync(serviceName);
            return $"{uri.Scheme}://{realRootUrl}{uri.PathAndQuery}";
        }

        public async Task<RestResponseWithBody<T>> GetForEntityAsync<T>(string url, HttpRequestHeaders requestHeaders = null)
        {
            using (HttpRequestMessage requestMsg = new HttpRequestMessage())
            {
                if (requestHeaders != null)
                {
                    foreach (var item in requestHeaders)
                    {
                        requestMsg.Headers.Add(item.Key, item.Value);
                    }
                }
                requestMsg.Method = System.Net.Http.HttpMethod.Get;
                requestMsg.RequestUri = new Uri(await ResolveUrlAsync(url));
                RestResponseWithBody<T> respEntity = await SendForEntityAsync<T>(requestMsg);
                return respEntity;
            }
        }
        public async Task<RestResponseWithBody<T>> SendForEntityAsync<T>(HttpRequestMessage requestMessage)
        {
            var result = await httpClient.SendAsync(requestMessage);
            RestResponseWithBody<T> respEntity = new RestResponseWithBody<T>();
            respEntity.StatusCode = result.StatusCode;
            respEntity.Headers = result.Headers;
            string bodyStr = await result.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(bodyStr))

            {
                respEntity.Body = JsonConvert.DeserializeObject<T>(bodyStr);
            }
            return respEntity;

        }
    }
}
