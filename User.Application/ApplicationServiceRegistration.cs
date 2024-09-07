using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using User.Application.Contracts;
using User.Application.Services;
using User.Domain.Security;

namespace User.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = configuration["JwtOptions:Issuer"]!;
                options.Audience = configuration["JwtOptions:Audience"]!;
            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();


            return services;
        }
    }
}
