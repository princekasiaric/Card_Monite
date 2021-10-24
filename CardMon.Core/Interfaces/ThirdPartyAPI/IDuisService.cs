using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using System.Threading.Tasks;

namespace CardMon.Core.Interfaces.ThirdPartyAPI
{
    public interface IDuisService
    {
        Task<BaseResponse> Link(EncryptedRequest request);
        Task<BaseResponse> Unlink(EncryptedRequest request);
        Task<BaseResponse> Resync(EncryptedRequest request);
        Task<BaseResponse> ResetPin(EncryptedRequest request);
    }
}
