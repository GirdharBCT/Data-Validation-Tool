using Data_Validation_Tool.Interface;
using Data_Validation_Tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data_Validation_Tool.Controllers
{
    [Route("authenticate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuth _auth;

        public AuthController(IConfiguration config, IAuth auth)
        {
            _config = config;
            _auth = auth;
        }
        [HttpPost]
        public async Task<IActionResult> Login(DataanalysisUser user)
        {
            if (user != null && user.UserName != null && user.Password != null)
            {
                var login = await _auth.LoginUser(user.UserName, user.Password);
                if (login != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id", login.Id.ToString()),
                        new Claim("UserName",user.UserName)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _config["Jwt:Issuer"],
                        _config["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }

                else
                {
                    return BadRequest("Invalid Credentials");
                }

            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }

    }
}
