using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace App.Framwork.DependencyInjection.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用静态全局容器(autofac)
        /// </summary>
        /// <param name="app"></param>
        public static void UseStaticContainer(this IApplicationBuilder app)
        {
            Storage.Container = app.ApplicationServices.CreateScope().ServiceProvider.GetAutofacRoot();
        }
    }
}