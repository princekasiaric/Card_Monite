using Ardalis.GuardClauses;
using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using CardMon.Core.DTOs.ThirdPartAPIDto.Request;
using CardMon.Core.DTOs.ThirdPartAPIDto.Response;
using CardMon.Core.Helpers;
using CardMon.Core.Interfaces.Helpers;
using CardMon.Core.Interfaces.Repositories;
using CardMon.Core.Interfaces.Services;
using CardMon.Core.Interfaces.ThirdPartyAPI;
using CardMon.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CardMon.Infrastructure.ThirdPartyAPI
{
    public class DuisService : IDuisService
    {
        private readonly IDataSecurityService _dataSecurityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IResponseResult _responseResult;
        private readonly AppSettings _appsettings;
        private readonly ILoggerManager _logger;
        private HttpClient _httpClient;

        public DuisService(
            IDataSecurityService dataSecurityService,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IRepositoryManager repositoryManager,
            IOptions<AppSettings> appSettings,
            IResponseResult responseResult,
            ILoggerManager logger)
        {
            _dataSecurityService = Guard.Against.Null(dataSecurityService, nameof(dataSecurityService));
            _httpContextAccessor = Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));
            _httpClientFactory = Guard.Against.Null(httpClientFactory, nameof(httpClientFactory));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _responseResult = Guard.Against.Null(responseResult, nameof(responseResult));
            _appsettings = Guard.Against.Null(appSettings.Value, nameof(appSettings.Value));
            _logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task<BaseResponse> Link(EncryptedRequest request)
        {
            return await MakeApiCallAndLogResponseAsync(request.EncryptedPayload, _appsettings.Endpoints.Link, ispinReset: false);
        }

        public async Task<BaseResponse> Resync(EncryptedRequest request)
        {
            return await MakeApiCallAndLogResponseAsync(request.EncryptedPayload, _appsettings.Endpoints.Resync, ispinReset: false);
        }

        public async Task<BaseResponse> Unlink(EncryptedRequest request)
        {
            return await MakeApiCallAndLogResponseAsync(request.EncryptedPayload, _appsettings.Endpoints.Unlink, ispinReset: false);
        }

        public async Task<BaseResponse> ResetPin(EncryptedRequest request)
        {
            return await MakeApiCallAndLogResponseAsync(request.EncryptedPayload, _appsettings.Endpoints.PinReset, ispinReset: true);
        }

        private async Task<BaseResponse> MakeApiCallAndLogResponseAsync(string encryptedPayload, string endpoint, bool ispinReset)
        {
            HttpResponseMessage response;
            string responseBody;
            if (!(_httpContextAccessor.HttpContext.Items["apiKey"] is Client client))
                return _responseResult.Failure(ResponseCodes.InvalidUserName, StatusCodes.Status401Unauthorized);
            _logger.LogInfo($"{client.UserName} encrypted request: {encryptedPayload}");
            var decryptedPayload = _dataSecurityService.DecryptPayload(encryptedPayload, client.ApiKey, client.IV);
            decryptedPayload = decryptedPayload.Trim();
            if (!decryptedPayload.StartsWith("{") && decryptedPayload.EndsWith("}") || !decryptedPayload.StartsWith("[") && decryptedPayload.EndsWith("]"))
                return _responseResult.Failure(ResponseCodes.InvalidCredential);
            _httpClient = _httpClientFactory.CreateClient(_appsettings.Service_Name);
            if (ispinReset)
            {
                var requestDto = JsonSerializer.Deserialize<PinResetRequestDto>(decryptedPayload);
                _httpClient.BaseAddress = new Uri(_appsettings.PinReset_RootUrl);
                var req = new HttpRequestMessage(HttpMethod.Post, endpoint + "?phoneNumber=" + requestDto.phoneNumber); 
                response = await _httpClient.SendAsync(req);
                responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult(); 
            }
            else
            {
                _httpClient.BaseAddress = new Uri(_appsettings.INB_RootUrl);
                var content = new StringContent(decryptedPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
                response = await _httpClient.PostAsync(endpoint, content);
                responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            string encryptedResponse = _dataSecurityService.EncryptPayload(responseBody, client.ApiKey, client.IV);
            // Log api service response
            var apiServiceLog = new APIsServiceLog();
            apiServiceLog.ClientId = client.Id;
            if (!string.IsNullOrEmpty(decryptedPayload))
                apiServiceLog.Request = decryptedPayload;
            if (!string.IsNullOrEmpty(responseBody))
                apiServiceLog.Response = responseBody;
            apiServiceLog.Method = _httpContextAccessor.HttpContext.Request.Method;
            apiServiceLog.Path = _httpContextAccessor.HttpContext.Request.Path;
            _repositoryManager.APIsServiceLogRepository.Create(apiServiceLog);
            _repositoryManager.SaveChangesAsync().GetAwaiter().GetResult();
            var res = new EncryptedResponse();
            if (ispinReset)
            {
                if (response.IsSuccessStatusCode)
                {
                    res.EncryptedPayload = encryptedResponse;
                    return _responseResult.Success(res, (int)response.StatusCode);
                }
                res.EncryptedPayload = encryptedResponse;
                _logger.LogInfo($"CardMon encrypted response: {encryptedResponse}");
                return _responseResult.Success(isSuccess: false, res, (int)response.StatusCode);
            }
            var responseDto = JsonSerializer.Deserialize<DuisResourceResponseDto>(responseBody);
            if (responseDto.response_code == "00")
            {
                res.EncryptedPayload = encryptedResponse;
                return _responseResult.Success(res, (int)response.StatusCode);
            }
            res.EncryptedPayload = encryptedResponse;
            _logger.LogInfo($"CardMon encrypted response: {encryptedResponse}");
            return _responseResult.Success(isSuccess: false, res, (int)response.StatusCode);
        }
    }
}
