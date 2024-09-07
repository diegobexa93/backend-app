using BaseShare.Common.Repositories;
using User.Domain.Entities;

namespace User.Domain.Contracts.Infrastructure
{
    public interface IUserRepository : IRepositoryBase<UserObj>
    {

    }
}
