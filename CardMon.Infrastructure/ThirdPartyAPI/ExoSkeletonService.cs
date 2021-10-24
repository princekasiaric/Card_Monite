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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CardMon.Infrastructure.ThirdPartyAPI
{
    public class ExoSkeletonService : IExoSkeletonService
    {
        private readonly IDataSecurityService _dataSecurityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IResponseResult _responseResult;
        private readonly AppSettings _appsettings;
        private readonly ILoggerManager _logger;
        private HttpClient _httpClient;
        
        public ExoSkeletonService(
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
            _httpClient = _httpClientFactory.CreateClient(_appsettings.Service_Name);
            _httpClient.BaseAddress = new Uri(_appsettings.ONB_RootUrl);
        }

        public Task<BaseResponse> Account(EncryptedRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> User()
        {
            var endpoint = _appsettings.Endpoints.User;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Get, null, new Dictionary<string, string> { { "username", username } });
        }

        public async Task<BaseResponse> Validate()
        {
            var endpoint = _appsettings.Endpoints.Validate;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Get, null, new Dictionary<string, string> { { "username", username } });
        }

        public async Task<BaseResponse> UpdateUser(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.UpdateUser;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Post, request.EncryptedPayload, new Dictionary<string, string> 
            { 
                { "username", username } 
            });
        }

        public async Task<BaseResponse> ActivateUser()
        {
            var endpoint = _appsettings.Endpoints.ActivateUser;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Post, null, new Dictionary<string, string> { { "username", username } });
        }

        public async Task<BaseResponse> UnlockUser()
        {
            var endpoint = _appsettings.Endpoints.UnlockUser;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Post, null, new Dictionary<string, string> { { "username", username } });
        }

        public async Task<BaseResponse> LockUser()
        {
            var endpoint = _appsettings.Endpoints.LockUser;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Post, null, new Dictionary<string, string> { { "username", username } });
        }

        public Task<BaseResponse> AddUserAccount(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.AddUserAccount;
            return MakeApiCallAsync(endpoint, HttpMethod.Put, request.EncryptedPayload);
        }

        public Task<BaseResponse> DeleteUserAccount(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.DeleteUserAccount;
            return MakeApiCallAsync(endpoint, HttpMethod.Delete, request.EncryptedPayload);
        }

        public async Task<BaseResponse> ResetSecret()
        {
            var endpoint = _appsettings.Endpoints.ResetSecret;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Post, null, new Dictionary<string, string> { { "username", username } });
        }

        public async Task<BaseResponse> Limits()
        {
            var endpoint = _appsettings.Endpoints.Limits;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Get, null, new Dictionary<string, string> { { "username", username } });
        }

        public async Task<BaseResponse> Limit(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.Limit;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Post, request.EncryptedPayload, new Dictionary<string, string>
            {
                { "username", username }
            });
        }

        public async Task<BaseResponse> ResetUserPassword()
        {
            var endpoint = _appsettings.Endpoints.ResetUserPassword;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Post, null, new Dictionary<string, string> { { "username", username } });
        }

        public Task<BaseResponse> AssignToken(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.AssignToken;
            return MakeApiCallAsync(endpoint, HttpMethod.Post, request.EncryptedPayload);
        }

        public Task<BaseResponse> UnAssignToken(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.UnAssignToken;
            return MakeApiCallAsync(endpoint, HttpMethod.Post, request.EncryptedPayload);
        }

        public Task<BaseResponse> ResyncToken(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.ResyncToken;
            return MakeApiCallAsync(endpoint, HttpMethod.Post, request.EncryptedPayload);
        }

        public Task<BaseResponse> ExemptCardAuth(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.ExemptCardAuth;
            return MakeApiCallAsync(endpoint, HttpMethod.Post, request.EncryptedPayload);
        }

        public async Task<BaseResponse> Transactions(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.Transactions;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            var userId = _httpContextAccessor.HttpContext.Items["userId"].ToString();
            return await SendQueryStringApiCallAsync(endpoint, request.EncryptedPayload, isTransactionsCall: true,
                isOrganizationCall: false, userId, username);
        }

        public async Task<BaseResponse> Audit()
        {
            var endpoint = _appsettings.Endpoints.Audit;
            var username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            return await MakeApiCallAsync(endpoint, HttpMethod.Get, null, new Dictionary<string, string> { { "username", username } });
        }

        public async Task<BaseResponse> Profiles()
        {
            return await MakeApiCallAsync(_appsettings.Endpoints.Profiles, HttpMethod.Get);
        }

        public async Task<BaseResponse> Roles()
        {
            return await MakeApiCallAsync(_appsettings.Endpoints.Roles, HttpMethod.Get);
        }

        public async Task<BaseResponse> SearchOrganization(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.SearchOrganization;
            return await SendQueryStringApiCallAsync(endpoint, request.EncryptedPayload, isTransactionsCall: false, 
                isOrganizationCall: true, null, null);
        }

        public Task<BaseResponse> Corporate(EncryptedRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> Config(EncryptedRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> Cache(EncryptedRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> CacheList()
        {
            return await MakeApiCallAsync(_appsettings.Endpoints.CacheList, HttpMethod.Get);
        }

        public async Task<BaseResponse> GroupList()
        {
            return await MakeApiCallAsync(_appsettings.Endpoints.GroupList, HttpMethod.Get);
        }

        public Task<BaseResponse> CreateOrganization(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.CreateOrganization;
            return MakeApiCallAsync(endpoint, HttpMethod.Put, request.EncryptedPayload);
        }

        public Task<BaseResponse> CreateCorporateUser(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.CreateCorporateUser;
            return MakeApiCallAsync(endpoint, HttpMethod.Put, request.EncryptedPayload);
        }

        public Task<BaseResponse> CreateRetailUser(EncryptedRequest request)
        {
            var endpoint = _appsettings.Endpoints.CreateRetailUser;
            return MakeApiCallAsync(endpoint, HttpMethod.Put, request.EncryptedPayload);
        }

        private async Task<BaseResponse> MakeApiCallAsync(
            string endpoint, 
            HttpMethod httpMethod,
            string encryptedPayload = null,
            IDictionary<string, string> headers = null) 
        {
            StringContent content;
            var response = new HttpResponseMessage();
            string decryptedPayload = string.Empty;

            if (!(_httpContextAccessor.HttpContext.Items["apiKey"] is Client client))
                return _responseResult.Failure(ResponseCodes.InvalidUserName, StatusCodes.Status401Unauthorized);
            if (encryptedPayload != null)
            {
                decryptedPayload = _dataSecurityService.DecryptPayload(encryptedPayload, client.ApiKey, client.IV);
                _logger.LogInfo($"{client.UserName} encrypted request: {encryptedPayload}");
            }
            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            if (httpMethod == HttpMethod.Get)
                response = await _httpClient.GetAsync(endpoint);
            if (httpMethod == HttpMethod.Post)
            {
                if (!string.IsNullOrEmpty(decryptedPayload))
                {
                    content = new StringContent(decryptedPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
                    response = await _httpClient.PostAsync(endpoint, content);
                }
                else
                {
                    var req = new HttpRequestMessage(HttpMethod.Post, endpoint);
                    response = await _httpClient.SendAsync(req);
                }
            }
            if(httpMethod == HttpMethod.Put)
            {
                content = new StringContent(decryptedPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
                response = await _httpClient.PutAsync(endpoint, content);
            }
            if (httpMethod == HttpMethod.Delete)
            {
                var req = new HttpRequestMessage(HttpMethod.Delete, endpoint);
                req.Content = new StringContent(decryptedPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
                response = await _httpClient.SendAsync(req);
            }
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var encryptedResponse = _dataSecurityService.EncryptPayload(responseBody, client.ApiKey, client.IV);
            InsertIntoAPIsServiceLogTbl(client.Id, decryptedPayload, responseBody).GetAwaiter().GetResult();
            var res = new EncryptedResponse();
            if (response.IsSuccessStatusCode)
            {
                res.EncryptedPayload = encryptedResponse;
                _logger.LogInfo($"CardMon encrypted response: {encryptedResponse}");
                return _responseResult.Success(res, (int)response.StatusCode);
            }
            res.EncryptedPayload = encryptedResponse;
            _logger.LogInfo($"CardMon encrypted response: {encryptedResponse}");
            return _responseResult.Success(isSuccess: false, res, (int)response.StatusCode);
        }

        private async Task<BaseResponse> SendQueryStringApiCallAsync(
            string endpoint,
            string encryptedPayload,
            bool isTransactionsCall,
            bool isOrganizationCall,
            string userId = null,
            string username = null)
        {
            TransactionsRequestDto dto;
            OrganizationSearchRequestDto requestDto;
            HttpRequestMessage request = new HttpRequestMessage();
            HttpResponseMessage response = new HttpResponseMessage();

            if (!(_httpContextAccessor.HttpContext.Items["apiKey"] is Client client))
                return _responseResult.Failure(ResponseCodes.InvalidUserName, StatusCodes.Status401Unauthorized);
            var decryptedPayload = _dataSecurityService.DecryptPayload(encryptedPayload, client.ApiKey, client.IV);
            _logger.LogInfo($"{client.UserName} encrypted request: {encryptedPayload}");
            if (!string.IsNullOrEmpty(userId))
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("userId", userId);
            if (!string.IsNullOrEmpty(username))
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("username", username);
            if (isOrganizationCall)
            {
                requestDto = JsonSerializer.Deserialize<OrganizationSearchRequestDto>(decryptedPayload);
                request = new HttpRequestMessage(HttpMethod.Get, endpoint + "?organization_id=" + requestDto.organization_id);
                response = await _httpClient.SendAsync(request);
            }
            if (isTransactionsCall)
            {
                dto = JsonSerializer.Deserialize<TransactionsRequestDto>(decryptedPayload);
                var startDateSplit = dto.startDate.Split('/');
                var d = startDateSplit[0];
                var m = startDateSplit[1];
                var y = startDateSplit[2];
                var endDateSplit = dto.endDate.Split('/');
                var dd = endDateSplit[0];
                var mm = endDateSplit[1];
                var yy = endDateSplit[2];
                request = new HttpRequestMessage(HttpMethod.Get, endpoint + "?startDate=" + d + "%2F" + m + "%2F" + y + "%2F" 
                    + "&endDate=" + dd + "%2F" + mm + "%2F" + yy + "%2F" + "&transactionId=" + dto.transactionId + "&accountNumber=" 
                    + dto.accountNumber + "&amount=" + dto.amount);
                response = await _httpClient.SendAsync(request);
            }
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var encryptedResponse = _dataSecurityService.EncryptPayload(responseBody, client.ApiKey, client.IV);
            InsertIntoAPIsServiceLogTbl(client.Id, decryptedPayload, responseBody).GetAwaiter().GetResult();
            var res = new EncryptedResponse();
            if (response.IsSuccessStatusCode)
            {
                res.EncryptedPayload = encryptedResponse;
                _logger.LogInfo($"CardMon encrypted response: {encryptedResponse}");
                return _responseResult.Success(res, (int)response.StatusCode);
            }
            res.EncryptedPayload = encryptedResponse;
            _logger.LogInfo($"CardMon encrypted response: {encryptedResponse}");
            return _responseResult.Success(isSuccess: false, res, (int)response.StatusCode);
        }

        private async Task InsertIntoAPIsServiceLogTbl(int Id, string decryptedPayload, string responseBody)
        {
            var apiServiceLog = new APIsServiceLog();
            apiServiceLog.ClientId = Id;
            if (!string.IsNullOrEmpty(decryptedPayload))
                apiServiceLog.Request = decryptedPayload;
            if (!string.IsNullOrEmpty(responseBody))
                apiServiceLog.Response = responseBody;
            apiServiceLog.Method = _httpContextAccessor.HttpContext.Request.Method;
            apiServiceLog.Path = _httpContextAccessor.HttpContext.Request.Path;
            _repositoryManager.APIsServiceLogRepository.Create(apiServiceLog);
            await _repositoryManager.SaveChangesAsync();
        }
    }

}

