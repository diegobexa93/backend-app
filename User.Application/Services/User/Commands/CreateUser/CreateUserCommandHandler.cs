using AutoMapper;
using BaseShare.Common.Exceptions;
using BaseShare.Common.Results;
using BaseShare.Common.Utility;
using MediatR;
using Microsoft.Extensions.Logging;
using User.Application.Errors;
using User.Domain.Contracts.Infrastructure;
using User.Domain.Entities;

namespace User.Application.Services.User.Commands.CreateUser
{
    public class CreateUserCommandHandler(IUserRepository _userRepository,
                                            IMapper _mapper,
                                            ILogger<CreateUserCommandHandler> _logger) : IRequestHandler<CreateUserCommand, Result<long>>
    {
        public async Task<Result<long>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var errors = ValidadeUser(request);

            if (errors.Any())
            {
                return Result.Failure<long>(new ValidationErrors(errors));
            }

            var userDb = await _userRepository.GetByEmailAsync(request.PersonEmail);

            if (userDb is not null)
                return Result.Failure<long>(UserError.UserExists);

            string passwordCrypto = BCrypt.Net.BCrypt.HashPassword(request.UserPassword, 12);

            var entity = _mapper.Map<UserObj>(request);
            entity.SetHashPassword(passwordCrypto);

            var result = await _userRepository.CreateAsync(entity, cancellationToken);

            return result.Id;
        }

        private static Error[] ValidadeUser(CreateUserCommand request)
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

            if (string.IsNullOrWhiteSpace(request.UserPassword))
                errors.Add(UserError.UserPasswordEmpty);
          
            return [.. errors];
        }
    }
}
