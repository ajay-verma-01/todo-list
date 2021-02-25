using Xunit;
using Microsoft.EntityFrameworkCore;
using TodoListService.Models;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoListService.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TodoListService.Controllers.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly DbContextOptions<TodoListDBContext> _dbContextOptions;
        private readonly TodoListDBContext _dbContext;
        private readonly AuthenticationController _controller;

        //Database path and deafault username to mock Identity Claims are harcoded for now, we can move it to configuration file.
        private readonly string _connectionString = "Data Source = D:\\invw\\demo\\todo-list\\todo-list-service\\TodoListService.Tests\\App_Data\\TodoListDB.Test.db";
        private readonly string _defaultUser = "ajay.verma";

        public AuthenticationControllerTests()
        {
            //Setup the db context
            _dbContextOptions = new DbContextOptionsBuilder<TodoListDBContext>()
                .UseSqlite(_connectionString)
                .Options;
            _dbContext = new TodoListDBContext(_dbContextOptions);

            //Mock logger object
            var mock = new Mock<ILogger<AuthenticationController>>();
            ILogger<AuthenticationController> logger = mock.Object;
            logger = Mock.Of<ILogger<AuthenticationController>>();

            //Mock the AppSettings key for Jwt Token
            IOptions<AppSettings> appSettings = Options.Create(new AppSettings());
            appSettings.Value.Jwt = new Jwt();
            appSettings.Value.Jwt.key = "mockKey-B81F4C42-AE24-4C36-BF73-BA821CFDE7FF";

            _controller = new AuthenticationController(_dbContext, appSettings, logger);

            //Mock claim identity and passing default user from settings
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, _defaultUser),
            }, "mock"));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
        }

        [Fact()]//Post method to generate token success test case
        public void PostGenerateTokenSuccessTest()
        {
            var result = _controller.Post(new User { userId = "ajay.verma", password = "pwd" });
            var value = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsAssignableFrom<UserDto>(value.Value);
            Assert.Equal("ajay.verma", item.userId);
            Assert.NotNull(item.token);
        }

        [Fact()]//Post method to test UnAuthorized case
        public void PostUnAuthorizedTest()
        {
            var result = _controller.Post(new User { userId = "ajay.verma", password = "invalidpassword" });
            var value = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, value.StatusCode);
        }

        [Fact()]//Sample get test (get user from Claims, it is mocked)
        public void GetUserByTokenTest()
        {
            var result = _controller.Get();

            var value = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<string>(value.Value);
            Assert.Equal("ajay.verma", items);

        }

       
    }
}