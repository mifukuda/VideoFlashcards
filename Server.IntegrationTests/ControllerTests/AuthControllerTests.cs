using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Server.Dtos;
using Server.Helpers;
using Server.Models;
using Xunit.Abstractions;

namespace Server.IntegrationTests
{
    public class AuthControllerTests
    {
        private readonly HttpClient _testClient;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly TestHelper _testHelper;
        public AuthControllerTests(ITestOutputHelper testOutputHelper)
        {
            var factory = new WebApplicationFactory<Program>();
            _testClient = factory.CreateClient();
            _testOutputHelper = testOutputHelper;
            _testHelper = new TestHelper();
        }

        [Fact]
        public async void Register_SetCookieHeader() {
            // Call POST Users/Registration
            UserForRegistrationDto testUser = new UserForRegistrationDto()
            {
                Email = "testRegistration@email.com",
                Password = "password",
                PasswordConfirm = "password",
                FirstName = "Test",
                LastName = "User"
            };
            var postData = new StringContent(JsonConvert.SerializeObject(testUser), Encoding.UTF8, "application/json");
            var response = await _testClient.PostAsync("/Auth/Register", postData);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Check to see if JWT token returned
            string? cookies = response.Headers.GetValues("Set-Cookie").FirstOrDefault()?.ToString();
            Assert.True(cookies?.Contains("token="));

            // Extract JWT token string
            // string? jwtToken = cookies?.Split(";")[0][6..];
            // _testOutputHelper.WriteLine(cookies?.Split(";")[0][6..]);
            // _testOutputHelper.WriteLine("Bearer " + jwtToken);

            // Log in
            UserForLoginDto loginUser = new UserForLoginDto()
            {
                Email = "testRegistration@email.com",
                Password = "password"
            };
            var loginData = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");
            var loginResponse = await _testClient.PostAsync("Auth/Login", loginData);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            // Make sure JWT is valid by calling endpoint that requires Authorization
            var refreshResponse = await _testClient.GetAsync("/Auth/RefreshToken");
            Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
            
            // Returned user
            User returnedTestUser = _testHelper.GetResponseContent<User>(response);

            _testOutputHelper.WriteLine(returnedTestUser.UserId.ToString());

            // Delete user
            var deleteResponse = await _testClient.DeleteAsync("/Users/" + returnedTestUser?.UserId);
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}