using System.Net;
using Azure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Server.IntegrationTests;

public class ControllerTests
{
    [Fact]
    public async void Test1()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.GetAsync("/Users/0");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}