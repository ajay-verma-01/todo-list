using Xunit;
using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace TodoListService.Infrastructure.Security.Tests
{
    public class JwtTokenGeneratorTests
    {
        [Fact()]//Create Jwt token by hardcoded key and username
        public void CreateJwtTokenTest()
        {
            var tokenGenerator = new JwtTokenGenerator();
            var token = tokenGenerator.CreateJwtToken("TestKey-B81F4C42-AE24-4C36-BF73-BA821CFDE7FF", "ajay.verma");
            var value = Assert.IsType<string>(token);
            Assert.NotNull(value);
        }

        [Fact()]//Create Jwt token for specific user and verify the Jwt token for that user username by validating token by given key.
        public void VerifyJwtTokenTest()
        {
            var tokenGenerator = new JwtTokenGenerator();
            var token = tokenGenerator.CreateJwtToken("TestKey-B81F4C42-AE24-4C36-BF73-BA821CFDE7FF", "ajay.verma");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("TestKey-B81F4C42-AE24-4C36-BF73-BA821CFDE7FF");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero

            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var username = jwtToken.Claims.First(x => x.Type == "unique_name").Value;
            Assert.Equal("ajay.verma", username);
        }
    }
}