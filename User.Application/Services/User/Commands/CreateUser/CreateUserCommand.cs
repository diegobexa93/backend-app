using BaseShare.Common.Results;
using MediatR;

namespace User.Application.Services.User.Commands.CreateUser;

public sealed record CreateUserCommand(
    string PersonName,
    string PersonEmail,
    string UserPassword) : IRequest<Result<long>>;