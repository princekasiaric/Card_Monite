using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using CardMon.Core.Interfaces.ThirdPartyAPI;
using CardMon.Infrastructure.Utilities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardMon.API.Controllers
{
    [Authorize]
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class ExoSkeletonController : RootController
    {
        private readonly IExoSkeletonService _exoSkeletonService;
        public ExoSkeletonController(
            IExoSkeletonService exoSkeletonService)
        {
            _exoSkeletonService = Guard.Against.Null(exoSkeletonService, nameof(exoSkeletonService));
        }

        //[ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        //[ServiceFilter(typeof(RequestValidationAttribute))]
        //[HttpGet("Account")]
        //public async Task<IActionResult> Account([FromBody]EncryptedRequest request)
        //{
        //    return ResolveActionResult(await _exoSkeletonService.Account(request));
        //}

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpGet("User")]
        public async Task<IActionResult> GetUser()
        {
            return ResolveActionResult(await _exoSkeletonService.User());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpGet("Validate")]
        public async Task<IActionResult> Validate()
        {
            return ResolveActionResult(await _exoSkeletonService.Validate());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.UpdateUser(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpPost("ActivateUser")]
        public async Task<IActionResult> ActivateUser()
        {
            return ResolveActionResult(await _exoSkeletonService.ActivateUser());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpPost("UnlockUser")]
        public async Task<IActionResult> UnlockUser()
        {
            return ResolveActionResult(await _exoSkeletonService.UnlockUser());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpPost("LockUser")]
        public async Task<IActionResult> LockUser()
        {
            return ResolveActionResult(await _exoSkeletonService.LockUser());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPut("AddAccount")]
        public async Task<IActionResult> AddUserAccount([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.AddUserAccount(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteUserAccount([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.DeleteUserAccount(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpPost("ResetSecret")]
        public async Task<IActionResult> ResetSecret()
        {
            return ResolveActionResult(await _exoSkeletonService.ResetSecret());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpGet("UserLimits")]
        public async Task<IActionResult> Limits()
        {
            return ResolveActionResult(await _exoSkeletonService.Limits());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("UserLimit")]
        public async Task<IActionResult> Limit([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.Limit(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpPost("ResetUserPassword")]
        public async Task<IActionResult> ResetUserPassword()
        {
            return ResolveActionResult(await _exoSkeletonService.ResetUserPassword());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("AssignToken")]
        public async Task<IActionResult> AssignToken([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.AssignToken(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("UnAssignToken")]
        public async Task<IActionResult> UnAssignToken([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.UnAssignToken(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("ResyncToken")]
        public async Task<IActionResult> ResyncToken([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.ResyncToken(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("ExemptCardAuth")]
        public async Task<IActionResult> ExemptCardAuth([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.ExemptCardAuth(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [ServiceFilter(typeof(ONBUserIdAttribute))]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("Transactions")]
        public async Task<IActionResult> Transactions([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.Transactions(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ONBUserNameAttribute))]
        [HttpGet("Audit")]
        public async Task<IActionResult> Audit()
        {
            return ResolveActionResult(await _exoSkeletonService.Audit());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [HttpGet("Profiles")]
        public async Task<IActionResult> Profiles()
        {
            return ResolveActionResult(await _exoSkeletonService.Profiles());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [HttpGet("Roles")]
        public async Task<IActionResult> Roles()
        {
            return ResolveActionResult(await _exoSkeletonService.Roles());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("SearchOrganization")]
        public async Task<IActionResult> SearchOrganization([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.SearchOrganization(request));
        }

        //[ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        //[ServiceFilter(typeof(RequestValidationAttribute))]
        //[HttpGet("Corporate")]
        //public async Task<IActionResult> Corporate([FromBody]EncryptedRequest request)
        //{
        //    return ResolveActionResult(await _exoSkeletonService.Corporate(request));
        //}

        //[ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        //[ServiceFilter(typeof(RequestValidationAttribute))]
        //[HttpGet("Config")]
        //public async Task<IActionResult> Config([FromBody]EncryptedRequest request)
        //{
        //    return ResolveActionResult(await _exoSkeletonService.Config(request));
        //}

        //[ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        //[ServiceFilter(typeof(RequestValidationAttribute))]
        //[HttpGet("Cache")]
        //public async Task<IActionResult> Cache([FromBody]EncryptedRequest request)
        //{
        //    return ResolveActionResult(await _exoSkeletonService.Cache(request));
        //}

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [HttpGet("CacheList")]
        public async Task<IActionResult> CacheList()
        {
            return ResolveActionResult(await _exoSkeletonService.CacheList());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [HttpGet("GroupList")]
        public async Task<IActionResult> GroupList()
        {
            return ResolveActionResult(await _exoSkeletonService.GroupList());
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPut("CreateOrganization")]
        public async Task<IActionResult> CreateOrganization([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.CreateOrganization(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPut("CreateCorporateUser")]
        public async Task<IActionResult> CreateCorporateUser([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.CreateCorporateUser(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPut("CreateRetailUser")]
        public async Task<IActionResult> CreateRetailUser([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _exoSkeletonService.CreateRetailUser(request));
        }
    }
}
