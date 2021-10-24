using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using System.Threading.Tasks;

namespace CardMon.Core.Interfaces.Services
{
    public interface IDataSecurityService
    {
        string EncryptPayload(string payload, string key, string iv);
        string DecryptPayload(string payload, string key, string iv);
        Task<BaseResponse> AuthenticateClient(AuthRequest request);
    }
}
