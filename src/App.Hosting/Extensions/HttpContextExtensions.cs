using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Application.User.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using App.Framwork;

namespace App.Hosting.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 设置cookie保存用户登录信息
        /// </summary>
        /// <param name="context">请求对象</param>
        /// <param name="authUser">用户信息</param>
        /// <returns></returns>
        public static Task Login(this HttpContext context, AuthUser authUser)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(authUser)));
            return context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static Task SignOut(this HttpContext context)
        {
            return context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// 获取当前后台用户登录的信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AuthUser GetAuthUser(this HttpContext context)
        {
            var auth = context.User;
            if (auth.Identity != null && auth.Identity.IsAuthenticated)
            {
                string info = auth.Claims.FirstOrDefault()?.Value;
                return JsonConvert.DeserializeObject<AuthUser>(info);
            }
            return null;
        }

        /// <summary>
        /// 获取session对应键的值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSession(this HttpContext context, string key)
        {
            return context.Session.GetString(key);
        }

        /// <summary>
        /// 获取session
        /// </summary>
        /// <typeparam name="T">存储对象类型</typeparam>
        /// <param name="context"></param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static T GetSession<T>(this HttpContext context, string key)
        {
            string v = context.GetSession(key);
            if (string.IsNullOrWhiteSpace(v))
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(v);
            }
        }
    }
}