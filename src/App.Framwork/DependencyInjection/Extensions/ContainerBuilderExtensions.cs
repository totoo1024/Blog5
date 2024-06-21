using System;
using System.Reflection;
using Autofac;

namespace App.Framwork.DependencyInjection.Extensions
{
    /// <summary>
    /// Autofac容器扩展
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Autofac自动注入Service层应用
        /// </summary>
        /// <param name="builder">容器</param>
        /// <param name="serviceNameEndsWith">实例名统一已指定名字结尾</param>
        public static void AutoRegisterService(this ContainerBuilder builder, string serviceNameEndsWith)
        {
            //自动注入service
            var assemblysServices = Storage.Assemblys.ToArray();
            builder.RegisterAssemblyTypes(assemblysServices)
                .Where(x => x.Name.EndsWith(serviceNameEndsWith, StringComparison.OrdinalIgnoreCase))
                .AsSelf()//注入自身
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}