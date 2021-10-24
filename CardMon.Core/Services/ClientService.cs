using Ardalis.GuardClauses;
using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using CardMon.Core.Helpers;
using CardMon.Core.Interfaces.Helpers;
using CardMon.Core.Interfaces.Repositories;
using CardMon.Core.Interfaces.Services;
using CardMon.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CardMon.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IResponseResult _responseResult;

        public ClientService(
            IHttpContextAccessor httpContextAccessor,
            IRepositoryManager repositoryManager,
            IResponseResult responseResult)
        {
            _httpContextAccessor = Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _responseResult = Guard.Against.Null(responseResult, nameof(responseResult));
        }

        public async Task<BaseResponse> GenerateApiKeyAsync(CredentialRequest request)
        {
            if (_repositoryManager.ClientRepository.HasAny(x => x.UserName.Equals(request.UserName)))
                return _responseResult.Failure(ResponseCodes.ClientAlreadyExist);
            
            var client = new Client();
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            client.ApiKey = Convert.ToBase64String(aes.Key);
            client.IV = Convert.ToBase64String(aes.IV);
            client.UserName = request.UserName;

            _repositoryManager.ClientRepository.Create(client);
            await _repositoryManager.SaveChangesAsync();
            var createdClient = await _repositoryManager.ClientRepository
                .GetClientAsync(trackChanges: false);
            var response = new CredentialResponse();
            response.ApiKey = createdClient.ApiKey;
            response.IV = createdClient.IV;
            return _responseResult.Success(response, StatusCodes.Status201Created);
        }

        public async Task<BaseResponse> RegenerateApiKeyAsync()
        {
            if (!(_httpContextAccessor.HttpContext.Items["apiKey"] is Client client))
                return _responseResult.Failure(ResponseCodes.InvalidUserName, StatusCodes.Status401Unauthorized);
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            client.ApiKey = Convert.ToBase64String(aes.Key);
            client.LastUpdated = DateTime.Now;
            _repositoryManager.ClientRepository.Update(client);
            await _repositoryManager.SaveChangesAsync();

            var updatedClient = await _repositoryManager.ClientRepository
                .GetClientAsync(client.ApiKey, trackChanges: false);
            var response = new ClientResponse();
            response.ApiKey = updatedClient.ApiKey;
            return _responseResult.Success(response);
        }

        public BaseResponse ResetCredentials(ResetRequest request)
        {
            if (!(_httpContextAccessor.HttpContext.Items["apiKey"] is Client client))
                return _responseResult.Failure(ResponseCodes.InvalidUserName, StatusCodes.Status401Unauthorized);

            if (request.UserName != client.UserName)
                return _responseResult.Failure(ResponseCodes.InvalidUserName, StatusCodes.Status401Unauthorized);
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC; 
            aes.Padding = PaddingMode.PKCS7;
            client.ApiKey = Convert.ToBase64String(aes.Key);
            client.IV = Convert.ToBase64String(aes.IV);
            client.LastUpdated = DateTime.Now;
            _repositoryManager.ClientRepository.Update(client);
            _repositoryManager.SaveChangesAsync();

            var response = new CredentialResponse();
            response.ApiKey = client.ApiKey;
            response.IV = client.IV;
            return _responseResult.Success(response);
        }
    }
}
