using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using System.Threading.Tasks;

namespace CardMon.Core.Interfaces.ThirdPartyAPI
{
    public interface IExoSkeletonService
    {
        Task<BaseResponse> User();
        Task<BaseResponse> Validate();
        Task<BaseResponse> ActivateUser();
        Task<BaseResponse> UnlockUser();
        Task<BaseResponse> LockUser();
        Task<BaseResponse> ResetSecret();
        Task<BaseResponse> Limits();
        Task<BaseResponse> Audit();
        Task<BaseResponse> Profiles();
        Task<BaseResponse> Roles();
        Task<BaseResponse> CacheList();
        Task<BaseResponse> GroupList();
        Task<BaseResponse> ResetUserPassword();
        Task<BaseResponse> Account(EncryptedRequest request);
        Task<BaseResponse> UpdateUser(EncryptedRequest request);
        Task<BaseResponse> AddUserAccount(EncryptedRequest request);
        Task<BaseResponse> DeleteUserAccount(EncryptedRequest request);
        Task<BaseResponse> Limit(EncryptedRequest request);
        Task<BaseResponse> AssignToken(EncryptedRequest request);
        Task<BaseResponse> UnAssignToken(EncryptedRequest request);
        Task<BaseResponse> ResyncToken(EncryptedRequest request);
        Task<BaseResponse> ExemptCardAuth(EncryptedRequest request);
        Task<BaseResponse> Transactions(EncryptedRequest request);
        Task<BaseResponse> SearchOrganization(EncryptedRequest request);
        Task<BaseResponse> Corporate(EncryptedRequest request);
        Task<BaseResponse> Config(EncryptedRequest request);
        Task<BaseResponse> Cache(EncryptedRequest request);
        Task<BaseResponse> CreateOrganization(EncryptedRequest request);
        Task<BaseResponse> CreateCorporateUser(EncryptedRequest request);
        Task<BaseResponse> CreateRetailUser(EncryptedRequest request);
    }
}
