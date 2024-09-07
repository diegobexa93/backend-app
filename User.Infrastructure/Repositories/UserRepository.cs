using BaseShare.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using User.Domain.Contracts.Infrastructure;
using User.Domain.Entities;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Repositories
{
    public class UserRepository(UserContext userContext)
        : RepositoryBase<UserObj>(userContext), IUserRepository
    {
        public async Task<UserObj?> GetByEmailAsync(string email, bool disableTracking = true)
        {
            IQueryable<UserObj> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Include(x => x.Person)
                         .Where(x => x.Person!.Email == email);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<UserObj?> GetByHashAsync(Guid hash, bool disableTracking = true)
        {
            IQueryable<UserObj> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Include(x => x.Person)
                         .Where(x => x.GuidId == hash);

            return await query.FirstOrDefaultAsync();
        }
    }
}
