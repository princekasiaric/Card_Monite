using System;

namespace CardMon.Core.DTOs.Response
{
    public class AuthResponse
    {
        public DateTime ValidTo { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public AuthResponse(DateTime validTo, string username, string accessToken)
        {
            ValidTo = validTo;
            Username = username;
            AccessToken = accessToken;
        }
    }
}
