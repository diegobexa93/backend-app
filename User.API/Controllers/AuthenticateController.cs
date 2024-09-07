using BaseShare.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using User.API.Helper;
using User.Application.Contracts;
using User.Application.Dtos;

namespace User.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class AuthenticateController(IAuthenticationService _authenticationService) : ControllerBase
    {
        [HttpPost]
        public async Task<IResult> Login([FromBody] AuthenticationRequestDto request)
        {
            var result = await _authenticationService.Authenticate(request);

            return result.Match(Results.Ok, ApiResults.Problem);

        }
    }
}
