using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using TrainingTests;
using TrainingTests.Controllers;
using TrainingTests.Models;
using TrainingTests.Repositories;
using Xunit;

namespace xUnitTest.ControllersTest
{
    public class UserControllerTests
    {
        private readonly MySqlContext mySql;
        private readonly UsersController controller;

        private readonly TestServer _server;
        private readonly HttpClient _client;

        public UserControllerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async void AuthorizationTest()
        {
            string otherName = "qwe";
            JObject jObject = new JObject();
            jObject.Add("username", "SuperUser");
            jObject.Add("password", "SuperUser");
            var response = await _client.PostAsJsonAsync("/api/Users/Authorization", jObject);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            _client.DefaultRequestHeaders.Add("authorization", "Bearer "+responseJson["accessToken"]);

            var userResponse = await _client.GetAsync("/api/Users/User");

            responseString = await userResponse.Content.ReadAsStringAsync();
            User userObj = JsonSerializer.Deserialize<User>(responseString);
            userObj.Id = "qwe";
            responseJson = JObject.Parse(responseString);
            /**
             * 
             * +responseJson	{{
                  "role": 0,
                  "id": "SuperUser",
                  "username": "SuperUser",
                  "email": "Email",
                  "firstname": "SuperUser",
                  "secondname": "SuperUser",
                  "dateBirth": "0001-01-01T00:00:00",
                  "dateRegistration": "2020-07-22T20:21:54.1044671+05:00"
                }}	Newtonsoft.Json.Linq.JObject
             */
            Assert.NotNull(responseJson["role"]);
            Assert.NotNull(responseJson["firstname"]);
            Assert.NotNull(responseJson["secondname"]);
            userObj.Email = "qwe";
            responseJson["role"] = 1;
            responseJson["firstname"] = otherName;
            responseJson["secondname"] = otherName;
            // Отправить через Form

            var userUserResponse = await _client.PostAsJsonAsync("/api/Users/Update", responseJson);
            var responseUserString = await userUserResponse.Content.ReadAsStringAsync();
            var responseUserJson = JObject.Parse(responseUserString);

            Assert.NotEqual(responseJson["role"], responseUserJson["role"]);
            Assert.Equal(responseJson["firstname"], responseUserJson["firstname"]);
            Assert.Equal(responseJson["secondname"], responseUserJson["secondname"]);
            Assert.Equal(responseJson["username"], responseUserJson["username"]);
            Assert.Equal(responseJson["secondname"], responseUserJson["secondname"]);

            // User.Identity.Name
            //controller.User.Identity
            // UserMy            
            //var resultUser = await controller.UserMy();

            //var viewResultu = Assert.IsType<ActionResult<UserView>>(resultUser);
            //var userVu = Assert.IsType<OkObjectResult>(userVu.Result);
            //var userResu = Assert.IsAssignableFrom<UserView>(userResu.Value);
        }

        [Fact]
        public void SettingsTest()
        {
            //My
            //Update

        }
    }
}
