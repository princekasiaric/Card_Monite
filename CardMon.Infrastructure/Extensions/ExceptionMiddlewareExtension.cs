using AutoMapper;
using CardMon.Core.Helpers;
using CardMon.Core.Interfaces.Helpers;
using CardMon.Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text.Json;

namespace CardMon.Infrastructure.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        private static string responseJson;
        public static void ConfigureExceptionHandler(
            this IApplicationBuilder app, 
            ILoggerManager loggerManager, 
            IResponseResult responseResult)
        {
            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature!=null)
                    {
                        loggerManager.LogError($"Something went wrong: {contextFeature.Error}");
                        if (contextFeature.Error is ValidationException validationException)
                        {
                            responseJson = WriteValidationException(validationException, context, loggerManager, responseResult);
                        }
                        else if (contextFeature.Error is CryptographicException cryptographicException)
                        {
                            responseJson = WriteCryptographicException(cryptographicException, context, loggerManager, responseResult);
                        }
                        else if (contextFeature.Error is AutoMapperMappingException mappingException)
                        {
                            responseJson = WriteAutomapperException(mappingException, context, loggerManager, responseResult);
                        }
                        else
                        {
                            responseJson = WriteGenericException(contextFeature.Error, context, loggerManager, responseResult);
                        }
                        await context.Response.WriteAsync(responseJson);
                    }
                });
            });
        }

        private static string WriteAutomapperException(
            AutoMapperMappingException mappingException, 
            HttpContext context, 
            ILoggerManager loggerManager, 
            IResponseResult responseResult)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = responseResult.Failure(ResponseCodes.MappingFailed, StatusCodes.Status500InternalServerError);
            response.Error.Reasons.Add(mappingException.Message);
            var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            loggerManager.LogError($"Automapper error: {responseJson}");
            return responseJson;
        }

        private static string WriteCryptographicException(
            CryptographicException cryptographicException, 
            HttpContext context, 
            ILoggerManager loggerManager, 
            IResponseResult responseResult)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var response = responseResult.Failure(ResponseCodes.RequestPayloadDecrytionFailure, StatusCodes.Status400BadRequest);
            response.Error.Reasons.Add(cryptographicException.Message);
            var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            loggerManager.LogError($"Payload decryption error: {responseJson}");
            return responseJson;
        }

        private static string WriteValidationException(
            ValidationException validationException, 
            HttpContext context, 
            ILoggerManager loggerManager, 
            IResponseResult responseResult)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var errorMessages = new List<string>();
            foreach (var error in validationException.Errors)
            {
                errorMessages.Add(error.ErrorMessage);
            }
            var response = responseResult.Failure(ResponseCodes.RequestValidationFailure, StatusCodes.Status400BadRequest, reasons: errorMessages);
            var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            loggerManager.LogError($"Request validation error: {responseJson}");
            return responseJson;
        }

        private static string WriteGenericException(
            Exception exception, 
            HttpContext context,
            ILoggerManager loggerManager,
            IResponseResult responseResult)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = responseResult.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            response.Error.Reasons.Add(exception.Message);
            var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            loggerManager.LogError($"System error: {responseJson}");
            return responseJson;
        }
    }
}
