namespace CardMon.Core.DTOs.Response
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public ErrorResponse Error { get; set; }
        public int StatusCode { get; set; }

        public BaseResponse(bool isSuccess, ErrorResponse error, int statusCode)
        {
            IsSuccess = isSuccess;
            Error = error;
            StatusCode = statusCode;
        }
    }
}
