using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;

namespace App.Framwork
{
    public static class Storage
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Storage()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            Configuration = builder.Build();
            Assemblys = DependencyContext.Default.CompileLibraries
                .Where(x => x.Type == "project")
                .Select(x => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(x.Name)))
                .ToList();
        }

        /// <summary>
        /// http请求上下文
        /// </summary>
        public static IHttpContextAccessor Current => GetService<IHttpContextAccessor>();

        /// <summary>
        /// 配置文件的根节点
        /// </summary>
        public static IConfigurationRoot Configuration { get; }

        /// <summary>
        /// 全局程序集
        /// </summary>
        public static List<Assembly> Assemblys { get; }


        /// <summary>
        /// Autofac依赖注入静态服务
        /// </summary>
        public static ILifetimeScope Container;

        /// <summary>
        /// 获取服务(Single)
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// 获取服务(请求生命周期内)
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetScopeService<T>() where T : class
        {
            return (T)GetService<IHttpContextAccessor>().HttpContext.RequestServices.GetService(typeof(T));
        }
    }
}