using BaseShare.Common.Repositories;
using User.Domain.Contracts.Infrastructure;
using User.Domain.Entities;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Repositories
{
    public class UserRepository(UserContext userContext)
        : RepositoryBase<UserObj>(userContext), IUserRepository
    {

    }
}
