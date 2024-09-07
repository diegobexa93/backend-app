using BaseShare.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.Domain.Contracts.Infrastructure;
using User.Infrastructure.Persistence;
using User.Infrastructure.Repositories;
using User.Infrastructure.Resilience;

namespace User.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwksManager()
                    .UseJwtValidation()
                    .PersistKeysToDatabaseStore<UserContext>();

            services.AddDbContext<UserContext>(options =>
             options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                        sqliteOptions =>
                        {
                            sqliteOptions.ExecutionStrategy(dependencies =>
                                new SqliteRetryingExecutionStrategy(dependencies, maxRetryCount: 5,
                                                                    maxRetryDelay: TimeSpan.FromSeconds(10)));
                        }));


            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IUserRepository, UserRepository>();


            return services;
        }
    }
}
