using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.User.Dtos;
using App.Core.Config;
using App.Framwork;
using App.Framwork.DependencyInjection;
using App.Framwork.Generate;
using App.Framwork.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace App.Application.User
{
    /// <summary>
    /// QQ授权
    /// </summary>
    public class QQAuthorize : IQQAuthorize, IScopedDependency
    {
        private const string KeyPrefix = "lib";
        private readonly QQAuthConfig _config;

        public QQAuthorize(IOptionsMonitor<QQAuthConfig> config)
        {
            _config = config.CurrentValue;
        }

        /// <summary>
        /// 获取授权登录地址
        /// </summary>
        /// <param name="url">授权成功后返回的地址</param>
        /// <returns></returns>
        public string AuthorizeLoginUrl(string url)
        {
            string state = SnowflakeId.NextStringId();
            Storage.Current.HttpContext.Session.SetString($"{KeyPrefix}{state}", url);
            return $"https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={_config.AppId}&redirect_uri={_config.CallbackUrl}&state={state}";
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code">回调code</param>
        /// <param name="state">用户验证是否合法请求</param>
        /// <returns></returns>
        public async Task<QQAccessToken> GetAccessToken(string code, string state)
        {
            if (string.IsNullOrWhiteSpace(Storage.Current.HttpContext.Session.GetString("lib" + state)))
            {
                throw new Exception("缺少参数");
            }
            string url = $"https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={_config.AppId}&client_secret={_config.AppKey}&code={code}&redirect_uri={System.Net.WebUtility.UrlEncode(_config.CallbackUrl)}";

            string result = await Net.GetAsync(url);
            QQAccessToken token;
            if (result.Contains("callback"))
            {
                int s = result.IndexOf("(");
                int e = result.IndexOf(")");
                result = result.Substring(s + 1, e - s - 1);
                token = JsonConvert.DeserializeObject<QQAccessToken>(result);
            }
            else
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string[] arr = result.Split('&');
                foreach (string item in arr)
                {
                    string[] temp = item.Split('=');
                    dic[temp[0]] = temp[1];
                }
                token = new QQAccessToken()
                {
                    access_token = dic["access_token"],
                    expires_in = Convert.ToInt32(dic["expires_in"]),
                    refresh_token = dic["refresh_token"]
                };
            }
            if (!string.IsNullOrWhiteSpace(token.error))
            {
                throw new Exception(token.error_description);
            }
            return token;
        }

        /// <summary>
        /// 获取授权用户的openid
        /// </summary>
        /// <param name="accesstoken">accesstoken令牌</param>
        /// <returns></returns>
        public async Task<string> GetOpenId(string accesstoken)
        {
            string url = $"https://graph.qq.com/oauth2.0/me?access_token={accesstoken}";
            string result = await Net.GetAsync(url);
            if (result.Contains("callback"))
            {
                int s = result.IndexOf("(");
                int e = result.IndexOf(")");
                result = result.Substring(s + 1, e - s - 1);
            }
            return JsonConvert.DeserializeObject<OpenIdInfo>(result)?.openid;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accesstoken">accesstoken令牌</param>
        /// <param name="openid">用户的openid</param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserInfo(string accesstoken, string openid)
        {
            string url = $"https://graph.qq.com/user/get_user_info?access_token={accesstoken}&oauth_consumer_key={_config.AppId}&openid={openid}";
            string result = await Net.GetAsync(url);
            return JsonConvert.DeserializeObject<UserInfo>(result);
        }
    }
}