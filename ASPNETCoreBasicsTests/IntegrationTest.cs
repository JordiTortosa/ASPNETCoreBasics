using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ASPNETCoreBasicsTests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {});
            _client = _factory.CreateClient();
        }
        /*
        [Fact]
        public async Task Get_HomePage_ReturnsSuccessStatusCode()
        {
            var url = "/";

            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Welcome", responseString);
        } */
        [Fact]
        public void DosMasDos()
        {
            Assert.Equal(2, 2);
        }


    }
    //public partial class Program { }
}
