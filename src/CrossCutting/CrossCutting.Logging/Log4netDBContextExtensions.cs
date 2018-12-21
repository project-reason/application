using CrossCutting.Logging.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Logging
{
    public static class Log4netDBContextExtensions
    {
        public static void AddLog4netDBContext(this IServiceCollection services, string connstring)
        {
            services.AddDbContext<Log4netDBContext>(options => options.UseSqlite(connstring));
        }

        public static void AddLog4netDBContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<Log4netDBContext>(options => options.UseSqlServer(Configuration["Logging:ConnectionString"]));
        }
    }
}
