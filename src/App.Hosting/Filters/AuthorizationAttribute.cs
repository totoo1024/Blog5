using System;
using System.Linq;
using App.Application.SysManager;
using App.Application.User.Dtos;
using App.Framwork;
using App.Framwork.Result;
using App.Hosting.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace App.Hosting
{
    /// <summary>
    /// 检验用户登录和权限验证过滤器（过滤器只能用于类和方法）有AllowAnonymousAttribute标记忽略验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var auth = context.HttpContext.User;
            if (!auth.Identity.IsAuthenticated && !context.Filters.Any(filter => filter is IAllowAnonymousFilter))
            {
                if (context.HttpContext.Request.Method.ToLower() == "post" || context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                {
                    UnifyResult result = new UnifyResult("登录已过期", ResultCode.Unauthorized);
                    context.Result = new JsonResult(result);
                }
                else
                {
                    context.HttpContext.Response.WriteAsync("<script type=\"text/javascript\">top.location.href = '/Main/Login/Index'</script>");
                }
            }
            else
            {
                //是否忽略权限验证
                bool ignore = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(false).Any(f => f is AllowAccessAttribute);
                if (!ignore)
                {
                    var currentUser = context.HttpContext.GetAuthUser();
                    var sysPermissionLogic = Framwork.Storage.GetScopeService<ISysPermissionService>();
                    var dic = context.ActionDescriptor.RouteValues;
                    //区域
                    string area = dic["area"];
                    //控制器
                    string controller = dic["controller"];
                    //action名
                    string action = dic["action"];
                    if (area != null)
                    {
                        area = $"/{area}";
                    }
                    //当前请求地址的虚拟路径
                    string currUrl = $"{area}/{controller}/{action}";
                    bool isAuthorize = sysPermissionLogic.CheckPermission(currentUser.RoleId, currUrl);
                    if (!isAuthorize)
                    {
                        //没有权限
                        if (context.HttpContext.Request.Method.ToLower() == "post" || context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                        {
                            UnifyResult result = new UnifyResult("无权访问", ResultCode.Forbidden);
                            context.Result = new JsonResult(result);
                        }
                        else
                        {
                            //跳转到相关页面
                            context.HttpContext.Response.WriteAsync("<script type=\"text/javascript\">top.location.href = '/Main/Login/Index'</script>");
                        }
                    }
                }
            }
        }
    }
}