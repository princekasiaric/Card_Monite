using CardMon.Core.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardMon.API.Controllers
{
    public class RootController : ControllerBase
    {
        protected ActionResult ResolveActionResult<TRespnse>(
            TRespnse response, 
            string actionName = null) where TRespnse : BaseResponse
        {
            if (response.StatusCode == StatusCodes.Status200OK)
                return Ok(response);
            else if (response.StatusCode == StatusCodes.Status400BadRequest)
                return BadRequest(response);
            else if (response.StatusCode == StatusCodes.Status401Unauthorized)
                return Unauthorized(response);
            else if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response);
            else if (response.StatusCode == StatusCodes.Status204NoContent)
                return Ok(response);
            else if (response.StatusCode == StatusCodes.Status201Created)
                return string.IsNullOrEmpty(actionName)?StatusCode(StatusCodes.Status201Created, response)
                    :CreatedAtAction(actionName, response);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
}
