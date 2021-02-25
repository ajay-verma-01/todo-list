using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoListService.Infrastructure;
using TodoListService.Infrastructure.Security;
using TodoListService.Models;

namespace TodoListService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly TodoListDBContext _dbContext;
        private ILogger<AuthenticationController> _logger;
        public AuthenticationController(TodoListDBContext orgContext, IOptions<AppSettings> appSettings, ILogger<AuthenticationController> logger)
        {
            _dbContext = orgContext;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [HttpGet()]// This is a test method
        [Authorize]
        public IActionResult Get()
        {
            var userName = User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
            return Ok(userName);
        }

        //Method verify the user and credential using database and then create token to return to the client.
        //If authetication fails then it returns Unauthorized response 401.
        public IActionResult Post([FromBody] User user)
        {
            //hardcoding username and password.
            //later we can create login page and get the user name and password from user input.
            var dbUser = _dbContext.User.SingleOrDefault(x => x.userId == user.userId);

            if (dbUser != null && dbUser.password == user.password && dbUser.isActive == 1)
            {
                var tokenGenerator = new JwtTokenGenerator();
                var userDto = new UserDto
                {
                    token = tokenGenerator.CreateJwtToken(_appSettings.Jwt.key, user.userId),
                    userId = dbUser.userId,
                    emailId = dbUser.emailId,
                    firstName = dbUser.firstName,
                    lastName = dbUser.lastName
                };
                
                return Ok(userDto);
            }
            else
            {
                return Unauthorized();
            }
        }

        //[HttpGet("{username}")]
        //public IActionResult Get(string username)
        //{
        //    var tokenGenerator = new JwtTokenGenerator();
        //    return Ok(tokenGenerator.CreateJwtToken(_appSettings.Jwt.key, username));
        //}
    }
}
