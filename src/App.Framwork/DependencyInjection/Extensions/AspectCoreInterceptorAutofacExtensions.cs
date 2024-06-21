using System;
using System.Linq;
using System.Reflection;
using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using Autofac;
using EasyCaching.Core.Configurations;
using EasyCaching.Core.Interceptor;
using EasyCaching.Interceptor.AspectCore;
using Microsoft.Extensions.Options;

namespace App.Framwork.DependencyInjection.Extensions
{
    /// <summary>
    /// 基于AspectCore的动态代理接入Autofac
    /// 参考地址：https://github.com/yrinleung/EasyCaching.Extensions/blob/master/src/EasyCaching.Interceptor.AspectCore.Autofac/AspectCoreInterceptorAutofacExtensions.cs
    /// github源码地址：https://github.com/yrinleung/EasyCaching.Extensions
    /// </summary>
    public static class AspectCoreInterceptorAutofacExtensions
    {
        /// <summary>
        /// Add the AspectCore interceptor.
        /// </summary>
        public static void AddAspectCoreInterceptor(this ContainerBuilder builder, Action<EasyCachingInterceptorOptions> action)
        {
            builder.RegisterType<DefaultEasyCachingKeyGenerator>().As<IEasyCachingKeyGenerator>();

            builder.RegisterType<EasyCachingInterceptor>();

            var config = new EasyCachingInterceptorOptions();

            action(config);

            var options = Options.Create(config);

            builder.Register(x => options);

            builder.RegisterDynamicProxy(configure =>
            {
                bool all(MethodInfo x) => x.CustomAttributes.Any(data => typeof(EasyCachingInterceptorAttribute).GetTypeInfo().IsAssignableFrom(data.AttributeType));

                configure.Interceptors.AddTyped<EasyCachingInterceptor>(all);
            });
        }
    }
}