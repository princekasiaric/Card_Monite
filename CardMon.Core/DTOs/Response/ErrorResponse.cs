using System.Collections.Generic;

namespace CardMon.Core.DTOs.Response
{
    public class ErrorResponse
    {
        public string HttpMethod { get; set; }
        public string RequestPath { get; set; }
        public string ErrorCode { get; set; }
        public string Description { get; set; }
        public IList<string> Reasons { get; set; }

        public ErrorResponse(
            string httpMethod, 
            string requestPath, 
            string errorCode, 
            string description, 
            IList<string> reasons = default)
        {
            HttpMethod = httpMethod;
            RequestPath = requestPath;
            ErrorCode = errorCode;
            Description = description;
            Reasons = reasons;
        }
    }
}
