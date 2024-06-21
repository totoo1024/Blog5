using System;
using System.Threading.Tasks;
using App.Framwork.Log;
using App.Framwork.Result;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ILogger = App.Framwork.Log.ILogger;

namespace App.Hosting.Middleware
{
    /// <summary>
    /// 异常处理中间件
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error("系统异常", ex);
                if (context.Request.Method.ToLower() == "post" || context.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                {
                    await ExceptionHandlerAsync(context, ex);
                }
                else
                {
                    context.Response.Redirect("/Error.html");
                }
            }
        }

        /// <summary>
        /// 异常处理，返回JSON
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task ExceptionHandlerAsync(HttpContext context, UnifyResult result)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}