using Newtonsoft.Json;
using System;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Moviemo.IntegrationTests.Extensions
{
    public static class SerializerExtensions
    {

        public static async Task<TOut> ReadAsJsonAsync<TOut> (this HttpContent content)
        {
            return JsonConvert.DeserializeObject<TOut>(await content.ReadAsStringAsync());
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<TModel> (this HttpClient client, string requestUrl, TModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await client.PostAsync(requestUrl, stringContent);
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync<TModel> (this HttpClient client, string requestUrl, TModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PutAsync(requestUrl, stringContent);
        }
    }
}
