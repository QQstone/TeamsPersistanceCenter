using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using TeamsPersistanceCenter.Models.Contexts;

namespace TeamsPersistanceCenter.Api.Infrastructure.DbContext
{
    /// <summary>
    /// Configure DB Contexts
    /// </summary>
    public static class ConfigureDbContext
    {
        static IConfiguration Configuration;

        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;
            services.AddDbContext<TeamsPersistanceContext>(ConfigureGlobalDbContextOptions); 
        }

        private static void ConfigureGlobalDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder options)
        {
            var connectionString = Configuration.GetConnectionString("SegmentationResultsConnection");
            Debug.Assert(!string.IsNullOrEmpty(connectionString));
            var cb = new SqlConnectionStringBuilder(connectionString);
            if(!cb.IntegratedSecurity && string.IsNullOrEmpty(cb.Password))
            {
                var password = Configuration["SegmentationResultsPassword"];
                cb.Password = password;
            }
            options.UseSqlServer(cb.ToString(), sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName);
            });
        }
    }
}
