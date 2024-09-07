using BaseShare.Common.Repositories;
using User.Domain.Entities;

namespace User.Domain.Contracts.Infrastructure
{
    public interface IUserRepository : IRepositoryBase<UserObj>
    {
        Task<UserObj?> GetByEmailAsync(string email, bool disableTracking = true);
        Task<UserObj?> GetByHashAsync(Guid hash, bool disableTracking = true);
    }
}
