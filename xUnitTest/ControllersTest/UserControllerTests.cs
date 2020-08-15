using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using TrainingTests;
using TrainingTests.Models;
using TrainingTests.ViewModels;
using Xunit;

namespace xUnitTest.ControllersTest
{
    public class UserControllerTests
    {// https://vk.com/dev/Like
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
            _client.DefaultRequestHeaders.Add("authorization", "Bearer " + responseJson["accessToken"]);

            var userResponse = await _client.GetAsync("/api/Users/User");

            responseString = await userResponse.Content.ReadAsStringAsync();
            User userObj = JsonSerializer.Deserialize<User>(responseString);
            userObj.Id = "qwe";
            responseJson = JObject.Parse(responseString);
            /**
             *  httpPost.setHeader("Accept", "application/json");
             *  httpPost.setHeader("Content-type", "application/json");
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

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var userUserResponse = await _client.PostAsJsonAsync("/api/Users/Update", responseJson);
            var responseUserString = await userUserResponse.Content.ReadAsStringAsync();
            var responseUserJson = JObject.Parse(responseUserString);

            Assert.NotEqual(responseJson["role"], responseUserJson["role"]);
            Assert.Equal(responseJson["firstname"], responseUserJson["firstname"]);
            Assert.Equal(responseJson["secondname"], responseUserJson["secondname"]);
            Assert.Equal(responseJson["username"], responseUserJson["username"]);
            Assert.Equal(responseJson["secondname"], responseUserJson["secondname"]);
        }

        [Fact]
        public async void CreateNewUser()
        {
            string username = "user";
            string email = "email@mail.ru";
            string password = "Paswwrodsa2";
            string firstname = "Paswwrodsa2";
            string secondname = "Paswwrodsa2";
            string group = "group";
            string departament = "departament";
            string discipline = "discipline";
            DateTime dateBirth = DateTime.Now;

            // Super
            RegistrationView registration = new RegistrationView
            {
                Username = username + "super",
                Email = email,
                Password = password,
                Firstname = firstname,
                Secondname = secondname,
                Group = group,
                Department = departament,
                Discipline = discipline,
                DateBirth = dateBirth
            };
            registration.Role = "super";

            var responseSuper = await _client.PostAsJsonAsync("/api/Users/Registration", registration);
            if(responseSuper.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                // Если пользователи уже были созданы
                return;
            }
            var responseJsonSuper = JObject.Parse(await responseSuper.Content.ReadAsStringAsync());

            Assert.Equal(email, (string)responseJsonSuper["email"]);
            Assert.Equal(firstname, (string)responseJsonSuper["firstname"]);
            Assert.Equal(secondname, (string)responseJsonSuper["secondname"]);

            //Student
            registration.Role = "student";

            var responseStudent = await _client.PostAsJsonAsync("/api/Users/Registration", registration);
            var responseJsonStudent = JObject.Parse(await responseStudent.Content.ReadAsStringAsync());

            Assert.Equal(email, (string)responseJsonStudent["email"]);
            Assert.Equal(firstname, (string)responseJsonStudent["firstname"]);
            Assert.Equal(secondname, (string)responseJsonStudent["secondname"]);

            //Update
            registration.Role = "teacher";

            var responseTeacher = await _client.PostAsJsonAsync("/api/Users/Registration", registration);
            var responseJsonTeacher = JObject.Parse(await responseTeacher.Content.ReadAsStringAsync());

            Assert.Equal(email, (string)responseJsonTeacher["email"]);
            Assert.Equal(firstname, (string)responseJsonTeacher["firstname"]);
            Assert.Equal(secondname, (string)responseJsonTeacher["secondname"]);
        }
    }
}
