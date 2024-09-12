using AutoMapper;
using BaseShare.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using User.Application.Services.User.Queries.ViewModel;
using User.Domain.Contracts.Infrastructure;
using User.Domain.Entities;

namespace User.Application.Services.User.Queries.GetList
{
    public class GetListQueryHandler(IUserRepository _userRepository,
                                     IMapper _mapper,
                                     ILogger<GetListQueryHandler> _logger) :
                                     IRequestHandler<GetListQuery, Result<List<UserViewModel>>>
    {
        public async Task<Result<List<UserViewModel>>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {

            var result = await _userRepository.GetAllAsync(new List<Expression<Func<UserObj, object>>>
            {
                u => u.Person!
            });

            return _mapper.Map<List<UserViewModel>>(result);
        }
    }
}
