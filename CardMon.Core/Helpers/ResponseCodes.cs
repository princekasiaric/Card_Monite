using System;

namespace CardMon.Core.Helpers
{
    public enum ResponseCodes
    {
        [ResponseCodeDescriber("ERR01", "Service is currently unavailable at this time. Please try again leter.")]
        GeneralError = 1,

        [ResponseCodeDescriber("ERR02", "User name is invalid or does not exist.")]
        InvalidUserName = 2,

        [ResponseCodeDescriber("ERR03", "Invalid api key.")]
        InvalidApiKey = 3,

        [ResponseCodeDescriber("ERR04", "Request validation failed.")]
        RequestValidationFailure = 4,

        [ResponseCodeDescriber("ERR05", "User name has been taken. Please try again.")]
        ClientAlreadyExist = 5,

        [ResponseCodeDescriber("ERR06", "Unable to decrypt request payload.")]
        RequestPayloadDecrytionFailure = 6,

        [ResponseCodeDescriber("ERR07", "Invalid IV credential.")]
        InvalidCredential = 7,

        [ResponseCodeDescriber("ERR08", "Unable to map request.")]
        MappingFailed
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    sealed class ResponseCodeDescriberAttribute : Attribute
    {
        public string Code { get; }
        public string Description { get; }

        public ResponseCodeDescriberAttribute(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }

    public static class ResponseCodeExtension
    {
        public static string GetCode(this ResponseCodes responseCodes)
        {
            var type = typeof(ResponseCodes);
            var property = type.GetField(responseCodes.ToString());
            var attribute = (ResponseCodeDescriberAttribute[])property.GetCustomAttributes(typeof(ResponseCodeDescriberAttribute), false);
            return attribute[0].Code;
        }

        public static string GetDescription(this ResponseCodes responseCodes)
        {
            var type = typeof(ResponseCodes);
            var property = type.GetField(responseCodes.ToString());
            var attribute = (ResponseCodeDescriberAttribute[])property.GetCustomAttributes(typeof(ResponseCodeDescriberAttribute), false);
            return attribute[0].Description;
        }
    }
}
