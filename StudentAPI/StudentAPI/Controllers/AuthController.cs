using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentAPI.DataAccess;
using StudentAPI.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IStudentService _service;
        public IConfiguration _config { get; set; }

        public AuthController(IConfiguration config, IStudentService service)
        {
            _service = service;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if(!_service.LoginAs(request.Login, request.Password))
            {
                return Forbid();
            }

            var tokens = GenerateToken(request.Login);

            return Ok(new
            {
                token = tokens.Item1,
                refreshToken = tokens.Item2
            });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(RefreshRequest request)
        {
            var token = _service.GetRefreshToken(request.Login);
            if(request.Token != token)
            {
                return Forbid();
            }
            var tokens = GenerateToken(request.Login);
            return Ok(new
            {
                token = tokens.Item1,
                refreshToken = tokens.Item2
            });
        }

        private Tuple<string, string> GenerateToken(string login)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Role, "employee")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var jwt = new JwtSecurityToken(issuer: "apkastudenty", audience: "studenty", claims: claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: creds);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var rToken = Guid.NewGuid().ToString();
            _service.SaveRefreshToken(login, rToken);
            return Tuple.Create(token, rToken);
        }
    }
}
