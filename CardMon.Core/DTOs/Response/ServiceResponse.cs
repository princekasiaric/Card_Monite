namespace CardMon.Core.DTOs.Response
{
    public class ServiceResponse<T> : BaseResponse where T : class
    {
        public T Payload { get; set; }

        public ServiceResponse(bool isSuccess, int statusCode, T payload) : base(isSuccess, null, statusCode)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Payload = payload;
        }
    }
}
