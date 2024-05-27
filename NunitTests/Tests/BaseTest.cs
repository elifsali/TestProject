using Newtonsoft.Json;
using NunitTests.Core.Models;
using NunitTests.Core.Utils;
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
            var responseFromHelper = await _handler.GET("users");
            //Assert the response
            Assert.AreEqual(System.Net.HttpStatusCode.OK, responseFromHelper.StatusCode);
        }

        [Test]
        public async Task CreateUser()
        {
            var userName = new Guid().ToString();
            // call CreateUserRequestBody()
            var requestBody = _handler.CreateUserRequestBody(userName);

            // call POST method
            var response = await _handler.POST("users", requestBody);
            Assert.AreEqual("Created", response.StatusCode.ToString());

            //Asserts
            // Id should not be null or empty
            // Name should be equal to the name of the user
            var responseBody = await response.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserResponse>(responseBody);
            Assert.IsNotNull(userResponse.Id);
            // Name should be equal to the name of the user
            Assert.AreEqual(userName, userResponse.Name);

        }

        [Test]
        public async Task GetSpecificUser()
        {
            //POST - to create a new user
            //GET - users/{user.ID}
            //Assert the response
        }

        [Test]
        public async Task UpdateUser()
        {
            //POST - to create a new user
            // HttpClientHelper.CreateUserRequestBody()
            // PUT - users/{user.ID}
            //Assert the response
        }

        [Test]
        public async Task DeleteUser()
        {
            //POST - to create a new user
            //DELETE - users/{user.ID}
            //Assert the response
        }
    }
}