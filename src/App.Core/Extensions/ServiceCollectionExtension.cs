using System.Collections.Generic;
using App.Core.Config;
using App.Core.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace App.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 添加数据库连接
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarConnection(this IServiceCollection services, IConfiguration configuration, string section = "DbConfig")
        {
            var configs = configuration.GetSection(section).Get<List<ConnectionConfig>>();
            foreach (var item in configs)
            {
                item.ConfigureExternalServices = new ConfigureExternalServices
                {
                    //配置ORM缓存
                    DataInfoCacheService = new SqlSugarCache()
                };
            }

            //注入SqlSugarClient
            services.AddScoped<ISqlSugarClient>(x =>
            {
                return new SqlSugarClient(configs);
            });
            //注入泛型仓储
            services.AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>));
            return services;
        }
    }
}