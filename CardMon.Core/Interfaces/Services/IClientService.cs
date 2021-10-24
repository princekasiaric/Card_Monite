using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using System.Threading.Tasks;

namespace CardMon.Core.Interfaces.Services
{
    public interface IClientService
    {
        Task<BaseResponse> GenerateApiKeyAsync(CredentialRequest request);
        Task<BaseResponse> RegenerateApiKeyAsync();
        BaseResponse ResetCredentials(ResetRequest request);
    }
}
