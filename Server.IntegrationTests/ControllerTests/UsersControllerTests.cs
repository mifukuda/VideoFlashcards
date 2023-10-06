using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Server.Helpers;
using Server.Models;
using Xunit.Abstractions;

namespace Server.IntegrationTests;

public class UserControllerTests
{
    private readonly HttpClient _testClient;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly TestHelper _testHelper;

    public UserControllerTests(ITestOutputHelper testOutputHelper)
    {
        var factory = new WebApplicationFactory<Program>();
        _testClient = factory.CreateClient();
        _testOutputHelper = testOutputHelper;
        _testHelper = new TestHelper();
    }

    [Fact]
    public async void GetUsers_GetAllUsers()
    {
        var response = await _testClient.GetAsync("/Users/0");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async void PostUsers_GetUpsertedUserAfterPost()
    {
        // Execute PUT request to /Users (create new user)
        User testUser = new User()
        {
            Email = "testUser@test.com",
            FirstName = "test",
            LastName = "user"
        };
        var putData = new StringContent(JsonConvert.SerializeObject(testUser), Encoding.UTF8, "application/json");
        var putResponse = await _testClient.PutAsync("/Users", putData);
        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

        // Parse JSON response into User
        User returnedTestUser = _testHelper.GetResponseContent<User>(putResponse);

        // Execute GET request with returned UserId
        var getResponse = await _testClient.GetAsync("/Users/" + returnedTestUser?.UserId);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        // Parse JSON response into User[]
        User[] returnedUsers = _testHelper.GetResponseContent<User[]>(getResponse);

        // Check to see if returned user is same as created user
        Assert.Equal(returnedTestUser?.UserId, returnedUsers[0].UserId);
        Assert.Equal(returnedTestUser?.FirstName, returnedUsers[0].FirstName);
        Assert.Equal(returnedTestUser?.LastName, returnedUsers[0].LastName);
        Assert.Equal(returnedTestUser?.Email, returnedUsers[0].Email);

        // Delete newly created user
        var deleteResponse = await _testClient.DeleteAsync("/Users/" + returnedTestUser?.UserId);
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

         // Check to see if user is deleted
        getResponse = await _testClient.GetAsync("/Users/" + returnedTestUser?.UserId);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        returnedUsers = _testHelper.GetResponseContent<User[]>(getResponse);
        Assert.Empty(returnedUsers);
    }

    [Fact]
    public async void PostUsers_DuplicateEmail()
    {
        // Insert user with email testUser@test.com
        User testUser = new User()
        {
            Email = "testUser@test.com",
            FirstName = "test",
            LastName = "user"
        };
        var putData = new StringContent(JsonConvert.SerializeObject(testUser), Encoding.UTF8, "application/json");
        var putResponse = await _testClient.PutAsync("/Users", putData);
        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

        // Parse JSON response into User
        User returnedTestUser = _testHelper.GetResponseContent<User>(putResponse);

        // Try to insert user with same Email
        putResponse = await _testClient.PutAsync("/Users", putData);
        Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);

        // Delete newly created user
        var deleteResponse = await _testClient.DeleteAsync("/Users/" + returnedTestUser?.UserId);
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
    }
}