using BaseShare.Common.Results;
using User.Application.Dtos;

namespace User.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticationResponseDto>> Authenticate(AuthenticationRequestDto request);

    }
}
