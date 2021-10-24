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
    public class DuisController : RootController
    {
        private readonly IDuisService _duisService;

        public DuisController(
            IDuisService duisService)
        {
            _duisService = Guard.Against.Null(duisService, nameof(duisService));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("Link")]
        public async Task<IActionResult> Link([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _duisService.Link(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("Unlink")]
        public async Task<IActionResult> Unlink([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _duisService.Unlink(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("Resync")]
        public async Task<IActionResult> Resync([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _duisService.Resync(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<EncryptedResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("ResetPin")]
        public async Task<IActionResult> ResetPin([FromBody]EncryptedRequest request)
        {
            return ResolveActionResult(await _duisService.ResetPin(request));
        }
    }
}
