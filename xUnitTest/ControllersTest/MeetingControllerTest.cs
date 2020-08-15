using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TrainingTests;

namespace xUnitTest.ControllersTest
{
    public class MeetingControllerTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public MeetingControllerTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
    }
}
