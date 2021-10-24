using Ardalis.GuardClauses;
using CardMon.Core.Interfaces.Repositories;
using CardMon.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMon.Infrastructure.Utilities
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appsettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = Guard.Against.Null(next, nameof(next));
            _appsettings = Guard.Against.Null(appSettings.Value, nameof(appSettings.Value));
        }

        public async Task Invoke(HttpContext context, IRepositoryManager repositoryManager)
        {
            var token = context.Request.Headers[HeaderNames.Authorization]
                .FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
                AttachClientToContext(context, repositoryManager, token);
            await _next(context);
        }

        private void AttachClientToContext(HttpContext context, IRepositoryManager repositoryManager, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(_appsettings.AuthConfig.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Convert.ToInt32(jwtToken.Claims.First(x => x.Type == "sub").Value);
            context.Items["User"] = repositoryManager.ClientRepository
                .GetClientByIdAsync(userId, trackChanges: false).GetAwaiter().GetResult();
        }
    }
}
