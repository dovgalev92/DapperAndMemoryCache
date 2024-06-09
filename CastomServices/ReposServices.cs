using DapperMemoryCache.ImplementationRepos;
using DapperMemoryCache.IRepos;

namespace DapperMemoryCache.CastomServices
{
    public static class ReposServices
    {
        public static IServiceCollection AddReposService(this IServiceCollection services)
        {
            services.AddScoped<IProgrammerRepos, ProgrammerRepos>();
            return services;
        }
    }
}
