using Newtonsoft.Json;
using NunitTests.Core.Models;
using NunitTests.Core.Utils;
using System.Net;
using System.Text;

namespace NunitTests.Tests
{
    [TestFixture]
    public class BaseTests
    {
        private HttpClientHelper _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new HttpClientHelper();
        }

        [Test]
        public async Task GetAllUser()
        {
            var response = await _handler.GET("users");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<UserResponse[]>(responseBody);

            Assert.IsTrue(users.Length > 0);

        }

        [Test]
        public async Task CreateUser()
        {
            var userName = new Guid().ToString();
            var requestBody = _handler.CreateUserRequestBody(userName);

            var response = await _handler.POST("users", requestBody);
            Assert.AreEqual("Created", response.StatusCode.ToString());

            
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserResponse>(responseBody);
            
            Assert.IsNotNull(createdUser.Id);
            Assert.IsNotEmpty(createdUser.Id.ToString());
            Assert.AreEqual(userName, createdUser.Name);

        }

        [Test]
        public async Task GetSpecificUser()
        {
            // Create a new user
            var userName = new Guid().ToString();
            var requestBody = _handler.CreateUserRequestBody(userName);
            var createUserResponse = await _handler.POST("users", requestBody);
            Assert.AreEqual(HttpStatusCode.Created, createUserResponse.StatusCode);

            var responseBody = await createUserResponse.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserResponse>(responseBody);

            // GET the specific user
            var getUserResponse = await _handler.GET($"users/{createdUser.Id}");
            Assert.AreEqual(HttpStatusCode.OK, getUserResponse.StatusCode);

            var getUserResponseBody = await getUserResponse.Content.ReadAsStringAsync();
            var retrievedUser = JsonConvert.DeserializeObject<UserResponse>(getUserResponseBody);

            // Asserts
            Assert.AreEqual(createdUser.Id, retrievedUser.Id);
            Assert.AreEqual(createdUser.Name, retrievedUser.Name);
        }

        [Test]
        public async Task UpdateUser()
        {
            // Create a new user
            var userName = new Guid().ToString();
            var requestBody = _handler.CreateUserRequestBody(userName);
            var createUserResponse = await _handler.POST("users", requestBody);
            Assert.AreEqual(HttpStatusCode.Created, createUserResponse.StatusCode);

            var responseBody = await createUserResponse.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserResponse>(responseBody);

            // Update the user
            createdUser.Name = "Updated Name";
            var updatedRequestBody = new StringContent(JsonConvert.SerializeObject(createdUser), Encoding.UTF8, "application/json");
            var updateUserResponse = await _handler.PUT($"users/{createdUser.Id}", updatedRequestBody);
            Assert.AreEqual(HttpStatusCode.OK, updateUserResponse.StatusCode);

            var updatedUserResponseBody = await updateUserResponse.Content.ReadAsStringAsync();
            var updatedUser = JsonConvert.DeserializeObject<UserResponse>(updatedUserResponseBody);

            // Asserts
            Assert.AreEqual(createdUser.Id, updatedUser.Id);
            Assert.AreEqual(createdUser.Name, updatedUser.Name);
        }

        [Test]
        public async Task DeleteUser()
        {
            // Create a new user
            var userName = new Guid().ToString();
            var requestBody = _handler.CreateUserRequestBody(userName);
            var createUserResponse = await _handler.POST("users", requestBody);
            Assert.AreEqual(HttpStatusCode.Created, createUserResponse.StatusCode);

            var responseBody = await createUserResponse.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserResponse>(responseBody);

            // Delete the user
            var deleteUserResponse = await _handler.DELETE($"users/{createdUser.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, deleteUserResponse.StatusCode);
        }
    }
}