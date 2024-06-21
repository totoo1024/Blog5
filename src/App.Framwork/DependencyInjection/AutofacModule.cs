using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace App.Framwork.DependencyInjection
{
    /// <summary>
    /// Autofac自动注入
    /// </summary>
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //获取继承了指定接口的类型
            var types = Storage.Assemblys.SelectMany(x => x.GetTypes())
                .Where(x => (typeof(IScopedDependency).IsAssignableFrom(x)
                                       || typeof(ISingletonDependency).IsAssignableFrom(x)
                                       || typeof(ITransientDependency).IsAssignableFrom(x))
                                      && x.IsClass && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);

            #region 注入瞬时

            var dependencyType = typeof(ITransientDependency);
            var arrDependencyType = types.Where(t => dependencyType.IsAssignableFrom(t) && t != dependencyType).ToArray();

            builder.RegisterTypes(arrDependencyType)
                .AsSelf()//注入自身
                .AsImplementedInterfaces()
                .InstancePerDependency(); //每次使用都创建一个新实例   瞬时的  默认的
                                          //.PropertiesAutowired(); // 开启属性注入

            #endregion

            #region 注入范围

            var scopeDependencyType = typeof(IScopedDependency);
            var arrScopeDependencyType = types.Where(t => scopeDependencyType.IsAssignableFrom(t) && t != scopeDependencyType).ToArray();

            builder.RegisterTypes(arrScopeDependencyType)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); //每次请求是唯一实
            //.PropertiesAutowired(); // 开启属性注入

            #endregion

            #region 注入单例

            var singletonDependencyType = typeof(ISingletonDependency);
            var arrSingletonDependencyType = types.Where(t => singletonDependencyType.IsAssignableFrom(t) && t != singletonDependencyType).ToArray();

            builder.RegisterTypes(arrSingletonDependencyType)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
            //.PropertiesAutowired(); // 开启属性注入

            #endregion

            #region 注册controller实现类

            var controller = typeof(ControllerBase);
            var arrControllerType = types.Where(t => controller.IsAssignableFrom(t) && t != controller).ToArray();
            builder.RegisterTypes(arrControllerType).PropertiesAutowired();

            #endregion
        }
    }
}