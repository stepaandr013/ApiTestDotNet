using Newtonsoft.Json;
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
        public static readonly string URL = "https://reqres.in";

        [Test]
        public async Task GetUsersTest()
        {
            
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage responseMessage = (await httpClient.GetAsync(URL + "/api/users?page=2"))
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

        [Test]
        public async Task UserRegistrationTest()
        {
            var requesrObjectUser = new UserDataRegistration("eve.holt@reqres.in", "pistol");

            HttpClient httpClient = new HttpClient();
            string url = "/api/register";

            string json = JsonConvert.SerializeObject(requesrObjectUser);

            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }

    }
}
