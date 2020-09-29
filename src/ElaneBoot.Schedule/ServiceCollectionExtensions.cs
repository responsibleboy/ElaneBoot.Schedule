using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ElaneBoot.Schedule.Services;
using WangSql;
using ElaneBoot.Schedule.Models;
using ElaneBoot.Schedule.Filters;

namespace ElaneBoot.Schedule
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiSchedule(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection("Database").Get<List<DbSettings>>();

            //data
            if (dbSettings != null && dbSettings.Any())
            {
                foreach (var item in dbSettings)
                {
                    DbProviderManager.Set
                       (
                       item.Name,
                       item.ConnectionString,
                       item.ConnectionType,
                       item.UseParameterPrefixInSql,
                       item.UseParameterPrefixInParameter,
                       item.ParameterPrefix,
                       item.UseQuotationInSql,
                       item.Debug
                       );
                }

                //注入SqlMapper
                services.AddScoped<ISqlMapper, SqlMapper>();
            }
            else
            {
                DbProviderManager.Set
                                   (
                                   "SQLite",
                                   "Data Source=schedule.db;",
                                   "System.Data.SQLite.SQLiteConnection,System.Data.SQLite",
                                   true,
                                   true,
                                   "@",
                                   false,
                                   false
                                   );

                //注入SqlMapper
                services.AddScoped<ISqlMapper, SqlMapper>();
            }

            services.AddTransient<ScheduleService>();

            //mvc
            services.AddMvc(options =>
            {
                options.Filters.Add<ApiActionFilter>();
            })
            //枚举序列化字符串
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            ;

            return services;
        }

        public static IApplicationBuilder UseApiSchedule(this IApplicationBuilder app, IConfiguration configuration)
        {
            //静态资源
            app.UseStaticFiles();

            var dllPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ElaneBoot.Schedule.dll");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new ManifestEmbeddedFileProvider(Assembly.LoadFrom(dllPath), "wwwroot")
            });

            ScheduleManager.Start();

            //mvc
            app.UseMvc();

            return app;
        }
    }
}