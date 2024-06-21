using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Framwork.DataValidation.Filters
{
    /// <summary>
    /// 数据验证过滤器
    /// </summary>
    public sealed class DataValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (!actionDescriptor.MethodInfo.IsDefined(typeof(IgnoreValidationAttribute), true) && !context.ModelState.IsValid)
            {
                //ajax请求
                if (context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                {
                    //响应验证错误
                    //var errors = context.ModelState.Values.Where(x => x.Errors.Any()).Select(x => x.Errors.FirstOrDefault()?.ErrorMessage);
                    //context.Result = new JsonResult(new UnifyResult(errors.FirstOrDefault(), ResultCode.ValidError));
                }
                else
                {
                    //同步请求
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}