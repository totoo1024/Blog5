using System.Linq;
using App.Framwork.DependencyInjection.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Framwork.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 自动注入（和AutofacModule二选一即可）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoDependencyInjection(this IServiceCollection services)
        {
            //获取所有程序集
            var assemblys = Storage.Assemblys;

            //获取继承需要依赖注入的类型
            var types = assemblys.SelectMany(x => x.GetTypes()).Where(x => (typeof(IScopedDependency).IsAssignableFrom(x)
                                                                           || typeof(ISingletonDependency).IsAssignableFrom(x)
                                                                           || typeof(ITransientDependency).IsAssignableFrom(x))
                                                                           && x.IsClass && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
            foreach (var type in types)
            {
                if (typeof(ITransientDependency).IsAssignableFrom(type))
                {
                    services.TryAddTransient(type);
                    var interfaceType = type.GetInterfaces().FirstOrDefault(x => typeof(ITransientDependency) != x);
                    if (interfaceType != null)
                    {
                        services.TryAddTransient(interfaceType, type);
                    }
                    continue;
                }
                if (typeof(IScopedDependency).IsAssignableFrom(type))
                {
                    services.TryAddScoped(type);
                    var interfaceTypes = type.GetInterfaces().Where(x => typeof(IScopedDependency) != x);
                    foreach (var iType in interfaceTypes)
                    {
                        services.TryAddScoped(iType, type);
                    }
                    continue;
                }
                if (typeof(ISingletonDependency).IsAssignableFrom(type))
                {
                    services.TryAddSingleton(type);
                    var interfaceType = type.GetInterfaces().FirstOrDefault(x => typeof(ISingletonDependency) != x);
                    if (interfaceType != null)
                    {
                        services.TryAddSingleton(interfaceType, type);
                    }
                }
            }

            return services;
        }

        /// <summary>
        /// 注入配置(支持热更新)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var types = Storage.Assemblys.SelectMany(x => x.GetTypes().Where(t => typeof(IConfig).IsAssignableFrom(t) && t.IsClass && !t.IsInterface && !t.IsAbstract && !t.IsGenericType));

            //通过反射获取泛型方法Configure<TOptions>(IConfiguration);
            var method = typeof(OptionsConfigurationServiceCollectionExtensions).GetMethod(nameof(OptionsConfigurationServiceCollectionExtensions.Configure), new[] { typeof(IServiceCollection), typeof(IConfiguration) });
            foreach (var item in types)
            {
                var attr = item.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(SectionAttribute)) as SectionAttribute;
                IConfigurationSection section = configuration.GetSection(attr?.Section ?? item.Name);
                method?.MakeGenericMethod(item).Invoke(null, new object[] { services, section });
            }
            return services;
        }
    }
}