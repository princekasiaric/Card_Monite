using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Response;
using CardMon.Core.Interfaces.Services;
using CardMon.Infrastructure.Utilities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardMon.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ClientsController : RootController
    {
        private readonly IDataSecurityService _dataSecurityService;
        private readonly IClientService _clientService;

        public ClientsController(
            IDataSecurityService dataSecurityService,
            IClientService clientService)
        {
            _dataSecurityService = Guard.Against.Null(dataSecurityService, nameof(dataSecurityService));
            _clientService = Guard.Against.Null(clientService, nameof(clientService));
        }

        [ProducesResponseType(typeof(ServiceResponse<AuthResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [ServiceFilter(typeof(ApiKeyAuthAttribute))]
        [HttpPost("AccessToken")]
        public async Task<IActionResult> AuthenticateClient([FromBody]AuthRequest request)
        {
            return ResolveActionResult(await _dataSecurityService.AuthenticateClient(request));
        }

        [ProducesResponseType(typeof(ServiceResponse<ClientResponse>), StatusCodes.Status201Created)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [HttpPost("ClientCredentials")]
        public async Task<IActionResult> GenerateCredentials([FromBody]CredentialRequest request)
        {
            return ResolveActionResult(await _clientService.GenerateApiKeyAsync(request));
        }

        [Authorize]
        [ProducesResponseType(typeof(ServiceResponse<ClientResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ApiKeyAuthAttribute))]
        [HttpGet("ResetApiKey")]
        public async Task<IActionResult> RegenerateApiKey()
        {
            return ResolveActionResult(await _clientService.RegenerateApiKeyAsync());
        }

        [Authorize]
        [ProducesResponseType(typeof(ServiceResponse<CredentialResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(RequestValidationAttribute))]
        [ServiceFilter(typeof(ApiKeyAuthAttribute))]
        [HttpPost("ResetCredentials")]
        public async Task<IActionResult> ResetCredentials([FromBody]ResetRequest request)
        {
            return ResolveActionResult(await Task.Run(() => _clientService.ResetCredentials(request)));
        }
    }
}
