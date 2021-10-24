using Ardalis.GuardClauses;
using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using CardMon.Core.Helpers;
using CardMon.Core.Interfaces.Helpers;
using CardMon.Core.Interfaces.Services;
using CardMon.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CardMon.Core.Services
{
    public class DataSecurityService : IDataSecurityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IResponseResult _responseResult;
        private readonly AppSettings _appsettings;

        public DataSecurityService(
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> appSettings,
            IResponseResult responseResult)
        {
            _httpContextAccessor = Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));
            _responseResult = Guard.Against.Null(responseResult, nameof(responseResult));
            _appsettings = Guard.Against.Null(appSettings.Value, nameof(appSettings.Value));
        }

        public async Task<BaseResponse> AuthenticateClient(AuthRequest request)
        {
            var expires = DateTime.Now.AddMinutes(1440);
            var issuedAt = DateTime.Now;

            if (!(_httpContextAccessor.HttpContext.Items["apiKey"] is Client client))
                return _responseResult.Failure(ResponseCodes.InvalidUserName, StatusCodes.Status401Unauthorized);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, client.UserName),
                new Claim(JwtRegisteredClaimNames.Exp, expires.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _appsettings.AuthConfig.ValidIssuer),
                new Claim(JwtRegisteredClaimNames.Aud, _appsettings.AuthConfig.ValidAudience),
                new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToString())
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() => GenerateAccessToken(tokenHandler, claims, issuedAt, expires));
            return _responseResult.Success(new AuthResponse(token.ValidTo, request.UserName, tokenHandler.WriteToken(token)));
        }

        private SecurityToken GenerateAccessToken(
            JwtSecurityTokenHandler tokenHandler,
            IList<Claim> claims,
            DateTime issuedAt,
            DateTime expires)
        {
            var key = Encoding.UTF8.GetBytes(_appsettings.AuthConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _appsettings.AuthConfig.ValidAudience,
                IssuedAt = issuedAt,
                Expires = expires,
                Issuer = _appsettings.AuthConfig.ValidIssuer,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public string DecryptPayload(string payload, string key, string iv)
        {
            byte[] cipher = StringToByteArray(payload);
            byte[] bytes = cipher;
            byte[] byteBuffer = new byte[bytes.Length];
            using var ms = new MemoryStream();
            using var aes = new RijndaelManaged();
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.Close();
            byte[] decryptedBytes = ms.ToArray();
            var str = Encoding.UTF8.GetString(decryptedBytes);
            return str;
        }

        private byte[] StringToByteArray(string payload)
        {
            return Enumerable.Range(0, payload.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(payload.Substring(x, 2), 16))
                .ToArray();
        }

        public string EncryptPayload(string payload, string key, string ivd)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(payload);
            using var ms = new MemoryStream();
            using var aes = new RijndaelManaged();
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(ivd);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.Close();
            var encryptedBytes = ms.ToArray();
            var prestr = ByteArrayToString(encryptedBytes);
            return prestr;
        }

        static string ByteArrayToString(byte[] encryptedPayload)
        {
            StringBuilder hex = new StringBuilder(encryptedPayload.Length * 2);
            foreach (var encryptedByte in encryptedPayload)
                hex.AppendFormat("{0:x2}", encryptedByte);
            return hex.ToString();
        }
    }
}
