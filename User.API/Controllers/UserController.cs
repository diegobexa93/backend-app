using BaseShare.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.API.Helper;
using User.Application.Services.User.Commands.CreateUser;
using User.Application.Services.User.Commands.DeleteUser;
using User.Application.Services.User.Commands.UpdateUser;

namespace User.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize]
    public class UserController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IResult> CreateUser([FromBody] CreateUserCommand command)
        {

            var result = await _mediator.Send(command);
            return result.Match(Results.Ok, ApiResults.Problem);
        }

        [HttpPut]
        public async Task<IResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Match(Results.Ok, ApiResults.Problem);
        }

        [HttpDelete("{hashId}")]
        public async Task<IResult> DeleteUser(Guid hashId)
        {
            var result = await _mediator.Send(new DeleteUserCommand(hashId));
            return result.Match(Results.Ok, ApiResults.Problem);
        }
    }
}
