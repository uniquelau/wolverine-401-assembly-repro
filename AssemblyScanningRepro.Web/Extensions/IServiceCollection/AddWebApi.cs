using Microsoft.AspNetCore.Identity;
using WolverineTest.Web.Data;

namespace WolverineTest.Web.Extensions // NOTE: namespace same for all extension methods (does not match folders)
{
    public static partial class IServiceCollectionExtensions
    {
        public static void AddWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<DbUser, IdentityRole>()
                .AddEntityFrameworkStores<WolverineTestContext>()
                .AddDefaultTokenProviders();

            services.AddWebAuthentication(configuration);
            services.AddAuthorization();
		}
    }
}