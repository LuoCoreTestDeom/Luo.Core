using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Utility
{
    public class RequestHttpApiService
    {
        private readonly HttpClient _httpClient;
        public RequestHttpApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://gorest.co.in");
            string token = "changeMe";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> GetStatusCodeAsync()
        {
            var result = await _httpClient.GetAsync("/public/v1/users");
            return result.StatusCode.ToString();
        }
    }
}
