using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestsDotNet
{
    public class BaseApi
    {
        public async Task<string> postRequest(object value, string url, int statusCode)
        {
            HttpClient httpClient = new HttpClient();
            string json = JsonConvert.SerializeObject(value);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.IsTrue((int)response.StatusCode == statusCode);
            return responseMessage;
        }
    }
}
