using BaseShare.Common.Results;
using MediatR;
using User.Application.Services.User.Queries.ViewModel;

namespace User.Application.Services.User.Queries.GetList;

public sealed record GetListQuery() : IRequest<Result<List<UserViewModel>>>;
