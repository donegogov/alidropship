using System;
using AliexpressOpenPlatformAPI.Data;
using AliexpressOpenPlatformAPI.Helpers;
using AliexpressOpenPlatformAPI.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AliexpressOpenPlatformAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IDropShippingApiService, DropShippingApiService>();
            services.AddDbContext<DataContext>(options =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                string connStr = "";

                // Depending on if in development or production, use either Heroku-provided
                // connection string, or development connection string from env var.
                if (env == "Development")
                {
                    // Use connection string from file.
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                else
                {
                    // Use connection string provided at runtime by Heroku.
                    connStr = Environment.GetEnvironmentVariable("MYSQLCONNSTR_mysql_nopaliexpressdropshipping_connectionstring");
                }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from Heroku, use it to set up your DbContext.
                options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
            });

            return services;
        }
    }
}
//"Server = 'alidropshipdb.mysql.database.azure.com'; UserID = 'alidropshipdb'; Password = 'e4t#BRv5b:ntDzj'; Database = 'alidropshipdb'; SslMode = Required; SslCa = 'C:\\home\\site\\wwwroot\\DigiCertGlobalRootG2.crt.pem';"