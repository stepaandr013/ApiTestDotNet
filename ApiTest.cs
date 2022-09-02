using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestsDotNet
{
    public class ApiTest
    {
        [Test]
        public async Task GetUsers()
        {
            
            HttpClient httpClient = new HttpClient();
            string url = "https://reqres.in/api/users?page=2";

            HttpResponseMessage responseMessage = (await httpClient.GetAsync(url))
                .EnsureSuccessStatusCode();

            string responseBody = await responseMessage.Content.ReadAsStringAsync();
            JObject keyValuePairs = JObject.Parse(responseBody);

            JToken jToken = keyValuePairs["data"];

            List<UserPojo> users = jToken.ToObject<List<UserPojo>>();

            Assert.IsTrue(responseMessage.IsSuccessStatusCode);

            for(int i = 0; i < users.Count; i++)
            {
                Assert.IsTrue(users[i].email.ToLower().Contains(users[i].first_name.ToLower()));
                Assert.IsTrue(users[i].email.ToLower().Contains(users[i].last_name.ToLower()));
                Assert.IsTrue(users[i].avatar.Contains(users[i].id.ToString()));
            }


        }
    }
}
