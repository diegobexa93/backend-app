using BaseShare.Common.Exceptions;
using BaseShare.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using User.Application.Errors;
using User.Domain.Contracts.Infrastructure;

namespace User.Application.Services.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(IUserRepository _userRepository,
                                          ILogger<DeleteUserCommandHandler> _logger)
        : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var errors = ValidadeUser(request);

            if (errors.Any())
            {
                return Result.Failure<bool>(new ValidationErrors(errors));
            }

            var userDb = await _userRepository.GetByHashAsync(request.hashId);
            if (userDb is null)
            {
                return Result.Failure<bool>(UserError.UserNotFound);
            }

            await _userRepository.DeleteAsync(userDb);

            return true;
        }

        private static Error[] ValidadeUser(DeleteUserCommand request)
        {
            List<Error> errors = [];

            if (string.IsNullOrWhiteSpace(request.hashId.ToString()))
                errors.Add(UserError.HashEmpty);


            return [.. errors];
        }
    }
}
