using System.Threading.Tasks;
using App.Application.User.Dtos;

namespace App.Application.User
{
    public interface IQQAuthorize
    {
        /// <summary>
        /// 获取授权登录地址
        /// </summary>
        /// <param name="url">授权成功后返回的地址</param>
        /// <returns></returns>
        string AuthorizeLoginUrl(string url);

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code">回调code</param>
        /// <param name="state">用户验证是否合法请求</param>
        /// <returns></returns>
        Task<QQAccessToken> GetAccessToken(string code, string state);

        /// <summary>
        /// 获取授权用户的openid
        /// </summary>
        /// <param name="accesstoken">accesstoken令牌</param>
        /// <returns></returns>
        Task<string> GetOpenId(string accesstoken);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accesstoken">accesstoken令牌</param>
        /// <param name="openid">用户的openid</param>
        /// <returns></returns>
        Task<UserInfo> GetUserInfo(string accesstoken, string openid);
    }
}