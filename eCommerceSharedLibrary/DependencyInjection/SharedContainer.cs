
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;            // <- add Serilog reference
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.SharedLibrary.DependencyInjection
{
    public static class SharedContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services,
            IConfiguration config, string fileName) where TContext : DbContext
        {
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("eCommerceConnection"), sqlServeroptions =>
                sqlServeroptions.EnableRetryOnFailure()));

            // Configure Serilog logger (customize as needed)
            Log.Logger = new LoggerConfiguration().
                 MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.txt",
                 restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information ,
                 outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.ff zzz} [{Level:u3}] {message:lj} {Newline} {exception}",
                 rollingInterval:RollingInterval.Month)
                .CreateLogger();
            
            JWTAuthenticationScheme.AddJWTAuthentication(services, config);
            return services;
        }
    }
}
   
