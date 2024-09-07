using BaseShare.Common.Results;
using MediatR;

namespace User.Application.Services.User.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid hashId) : IRequest<Result<bool>>;