using BaseShare.Common.Results;
using MediatR;

namespace User.Application.Services.User.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    string PersonName,
    string PersonEmail,
    string? UserPassword,
    Guid HashUser) : IRequest<Result<bool>>;

