using Ardalis.GuardClauses;
using CardMon.Core.DTOs.Response;
using CardMon.Core.Interfaces.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CardMon.Core.Helpers
{
    public class ResponseResult : IResponseResult
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResponseResult(IHttpContextAccessor httpContextAccessor) 
            => _httpContextAccessor = Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));

        public BaseResponse Failure(
            ResponseCodes responseCode, 
            int statusCode = StatusCodes.Status400BadRequest, 
            string responseDescription = null, 
            IList<string> reasons = null)
        {
            return new BaseResponse
                (
                   false,
                   new ErrorResponse
                   (
                       _httpContextAccessor.HttpContext.Request.Method,
                       _httpContextAccessor.HttpContext.Request.Path,
                       responseCode.GetCode(),
                       string.IsNullOrEmpty(responseDescription)?responseCode.GetDescription():responseDescription,
                       reasons??new List<string>()
                    ),
                   statusCode
                );
        }

        public ServiceResponse<T> Success<T>(
            T payload = default, 
            int statusCode = StatusCodes.Status200OK) where T : class
        {
            return new ServiceResponse<T>(true, statusCode, payload);
        }

        public ServiceResponse<T> Success<T>(
            bool isSuccess,
            T payload = default,
            int statusCode = StatusCodes.Status200OK) where T : class
        {
            return new ServiceResponse<T>(isSuccess, statusCode, payload);
        }

        public ServiceResponse<dynamic> Success(int statusCode = StatusCodes.Status200OK)
        {
            return new ServiceResponse<dynamic>(true, statusCode, default);
        }
    }
}
