using AutoMapper;
using BaseShare.Common.Exceptions;
using BaseShare.Common.Results;
using BaseShare.Common.Utility;
using MediatR;
using Microsoft.Extensions.Logging;
using User.Application.Errors;
using User.Domain.Contracts.Infrastructure;

namespace User.Application.Services.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(IUserRepository _userRepository,
                                            IMapper _mapper,
                                            ILogger<UpdateUserCommand> _logger)
        : IRequestHandler<UpdateUserCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var errors = ValidadeUser(request);

            if (errors.Any())
            {
                return Result.Failure<bool>(new ValidationErrors(errors));
            }

            var userDb = await _userRepository.GetByHashAsync(request.HashUser);
            if (userDb is null)
            {
                _logger.LogError($"User profile with hash {request.HashUser} does not exist");
                return Result.Failure<bool>(UserError.UserNotFound);
            }

            _mapper.Map(request, userDb);

            if (!string.IsNullOrEmpty(request.UserPassword))
            {
                string passwordCrypto = BCrypt.Net.BCrypt.HashPassword(request.UserPassword, 12);
                userDb.SetHashPassword(passwordCrypto);
            }

            await _userRepository.UpdateAsync(userDb);

            return true;
        }

        private static Error[] ValidadeUser(UpdateUserCommand request)
        {
            List<Error> errors = [];

            if (string.IsNullOrWhiteSpace(request.PersonName))
                errors.Add(UserError.PersonNameEmpty);

            if (string.IsNullOrWhiteSpace(request.PersonEmail))
                errors.Add(UserError.PersonEmailEmpty);
            else
            {
                if (!EmailValidator.IsValidEmail(request.PersonEmail))
                    errors.Add(UserError.PersonEmailInvalid);
            }

            if (string.IsNullOrWhiteSpace(request.HashUser.ToString()))
                errors.Add(UserError.HashEmpty);

            return [.. errors];
        }
    }
}
