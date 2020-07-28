using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using TrainingTests;
using TrainingTests.Controllers;
using TrainingTests.Models;
using TrainingTests.Repositories;
using TrainingTests.ViewModels;
using Xunit;

namespace xUnitTest.ControllersTest
{
    public class TestingTestControllerTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public TestingTestControllerTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        // https://stackru.com/questions/39263149/nastrojka-fajlov-cookie-dlya-httpclient-aspnet-core-testserver
        // C# TestServer get and set cookie

        [Fact]
        public async void CheckTest()
        {
            var anotherAnswer = "Answer";

            var getAll = await _client.GetAsync("/api/TestingTest/GetAllTest");
            var tests = JArray.Parse(await getAll.Content.ReadAsStringAsync());

            foreach(var test in tests)
            {
                var getTest = await _client.PostAsJsonAsync("/api/TestingTest/GetTest", (string)test["id"]);
                var questions = JArray.Parse(await getTest.Content.ReadAsStringAsync());

                bool st = false;

                foreach(var question in questions)
                {
                    /**
                     * {
                        "id": "quest2",
                        "themaId": "thema-1",
                        "thema": null,
                        "name": "Question 2",
                        "description": "Текст ВОПРОСА 1",
                        "answers": [
                          "Ответ 1",
                          "Ответ 2",
                          "Ответ 3",
                          "Ответ 4",
                          "Ответ 5"
                        ],
                        "rightAnswers": null,
                        "appraisal": 5.0,
                        "countAnswers": 5,
                        "time": 0,
                        "allAnswers": false,
                        "typeAnswer": 2
                      }
                     */
                    var jA = new JArray();
                    if (st)
                    {
                        jA.Add(anotherAnswer);
                    }
                    else
                    {
                        jA.Add("Ответ 1");
                    }
                    st = !st;
                    question["rightAnswers"] = jA;
                    question["time"] = 1500;
                }
                var getResultTest = await _client.PostAsJsonAsync("/api/TestingTest/Result", questions);
                var result = JObject.Parse(await getResultTest.Content.ReadAsStringAsync());

                Assert.NotNull(result.Value<string>("testId"));
                Assert.NotEmpty(result.Value<string>("testId"));
            }

            //var testsView = Assert.IsType<ActionResult<List<TestView>>>(testsP);
            //var testsVw = Assert.IsType<OkObjectResult>(testsView.Result);
            //var tests = Assert.IsAssignableFrom<List<TestView>>(testsVw.Value);
            //controller.Response = new Mock<HttpResponse>().Object;
            //var q = controller.Response.Cookies;
            //controller.Response.Cookies.Append("test", new Mock<string>().Object);

            //var cookies = new HttpCookieCollection();
            //cookies.Add(new HttpCookie("usercookie"));
            //controller.Response.Cookies.Returns(cookies);

            //controller.Response.Cookies.Append("test", new Mock<string>().Object);
            //controller.Response.Cookies.Append("dateStart", new Mock<string>().Object);

         }

        [Fact]
        public async void GetAllTests()
        {

        }

        [Fact]
        public void CheckTestPassword()
        {

        }
    }
}
