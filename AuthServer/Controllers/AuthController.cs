﻿using AuthServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.IO;
using System.Text.Json;
using API; 

namespace AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ToDoDbContext _dataContext;
        private readonly ILogger<PrivateController> _logger;

        public AuthController(IConfiguration configuration, ToDoDbContext dataContext,ILogger<PrivateController> logger)
        {
            _configuration = configuration;
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpPost("/api/login")]
public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
{
    var user = _dataContext.Users?.FirstOrDefault(u => u.username == loginModel.Name && u.password == loginModel.Password);
    if (user is not null)
    {
        var jwt = CreateJWT(user);
        await AddSession(user);
        return Ok(jwt);
    }
    return Unauthorized();
}

        [HttpPost("/api/register")]
        public async Task<IActionResult> Register([FromBody] LoginModel loginModel)
        {
            _logger.LogInformation("רגיסטר ");
            var name = loginModel.Name;
            var lastId = _dataContext.Users?.Max(u => u.id) ?? 0;
            var newUser = new Users {username = name, password = loginModel.Password};
            _dataContext.Users?.Add(newUser);
            await _dataContext.SaveChangesAsync();
            var jwt = CreateJWT(newUser);
            AddSession(newUser);
            return Ok(jwt);
        }

        private object CreateJWT(Users user)
        {
            var claims = new List<Claim>()
                {
                    new Claim("id", user.id.ToString()),
                    new Claim("name", user.username),
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JWT:Issuer"),
                audience: _configuration.GetValue<string>("JWT:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return new { Token = tokenString };
        }

        private async Task AddSession(Users user)
        {
            var session = new Session { UserId = user.id,Date=DateTime.Now};
            _dataContext.Sessions?.Add(session);
            await _dataContext.SaveChangesAsync();

        }
    }
    
}
