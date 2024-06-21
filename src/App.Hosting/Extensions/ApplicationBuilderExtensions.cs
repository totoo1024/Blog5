using App.Hosting.Middleware;
using Microsoft.AspNetCore.Builder;

namespace App.Hosting.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseExceptionHandle(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}