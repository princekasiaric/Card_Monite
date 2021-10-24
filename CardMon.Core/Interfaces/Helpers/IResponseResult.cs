using CardMon.Core.DTOs.Response;
using CardMon.Core.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CardMon.Core.Interfaces.Helpers
{
    public interface IResponseResult
    {
        BaseResponse Failure(
            ResponseCodes responseCode, 
            int statusCode = StatusCodes.Status400BadRequest, 
            string responseDescription = default, 
            IList<string> reasons = default);

        ServiceResponse<T> Success<T>(
            T payload = default, 
            int status = StatusCodes.Status200OK) where T : class;

        public ServiceResponse<T> Success<T>(
            bool isSuccess,
            T payload = default,
            int statusCode = StatusCodes.Status200OK) where T : class;

        ServiceResponse<dynamic> Success(int statusCode = StatusCodes.Status200OK);
    }
}
