using Data_Validation_Tool.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Validation_Tool.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly AppSettings _appSettings;
        public IConfiguration Configuration { get; }

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            //_appSettings = appSettings.Value;
            Configuration = configuration;
        }

        public async Task Invoke(HttpContext context, prd_phyndContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachAccountToContext(context, dataContext, token);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, prd_phyndContext dataContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Secret"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);
                var userName = jwtToken.Claims.First(x => x.Type == "UserName").Value.ToString();

                // attach account to context on successful jwt validation
                //context.Items["Account"] = await dataContext.DataanalysisUsers.FindAsync(accountId);

                context.Items["LogedInUserId"] = accountId;
                context.Items["LogedInUser"] = userName;
                
            }
            catch
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }
}