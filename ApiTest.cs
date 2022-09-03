using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestsDotNet
{
    public class ApiTest : BaseApi
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
            var requestObjectUser = new UserDataRegistration("eve.holt@reqres.in", "stepaandr123");
            var responseMessage = await postRequest(requestObjectUser, URL + "/api/register", 200);
            var responseValue = JsonConvert.DeserializeObject<UserDataRegistrationResponse>(responseMessage);

            Assert.IsNotNull(responseValue);
            Assert.IsTrue((responseValue.token is not null) && (responseValue.id.ToString() is not null));
        }

        [Test]
        public async Task UserLoginTest()
        {
            var requestObjectUser = new UserDataRegistration("eve.holt@reqres.in", "stepaandr123");
            var responseMessage = await postRequest(requestObjectUser, URL + "/api/login", 200);
            var responseValue = JsonConvert.DeserializeObject<UserDataLoginResponse>(responseMessage);
            
            Assert.IsNotNull(responseValue.token);
        }

        [Test]
        public async Task UserLoginFaultTest()
        {
            var requestObjectUser = new UserDataRegistration("eve.holt@reqres.in", "stepaandr321");
            var responseMessage = await postRequest(requestObjectUser, URL + "/api/login", 400);
            var responseValue = JsonConvert.DeserializeObject<UserDataLoginResponse>(responseMessage);

            Assert.IsNull(responseValue.token);
        }

    }
}
