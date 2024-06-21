using System;
using System.Threading.Tasks;
using App.Core.Entities.User;
using App.Core.Repository;
using App.Framwork.Generate;
using App.Framwork.Result;

namespace App.Application.User
{
    public class QQUserService : AppService<QQUserinfo>, IQQUserService
    {
        private readonly IQQAuthorize _qQAuthorize;

        public QQUserService(IAppRepository<QQUserinfo> repository,
            IQQAuthorize qQAuthorize) : base(repository)
        {
            _qQAuthorize = qQAuthorize;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="code">回调返回的code</param>
        /// <param name="state">自定义验证状态码</param>
        /// <returns></returns>
        public async Task<QQUserinfo> Login(string code, string state)
        {
            //token需要缓存起来，有效期30天
            var token = await _qQAuthorize.GetAccessToken(code, state);
            string openid = await _qQAuthorize.GetOpenId(token.access_token);
            QQUserinfo userInfo = await FindEntityAsync(c => c.OpenId == openid);
            if (userInfo == null)
            {
                var user = await _qQAuthorize.GetUserInfo(token.access_token, openid);
                userInfo = new QQUserinfo()
                {
                    Id = SnowflakeId.NextStringId(),
                    NikeName = user.nickname,
                    Gender = user.gender,
                    Birthday = user.year,
                    OpenId = openid,
                    Province = user.province,
                    Image40 = user.figureurl_qq_1,
                    Image100 = user.figureurl_qq_2,
                    LastLoginTime = DateTime.Now,
                    AccessToken = token.access_token,
                    RefreshToken = token.refresh_token,
                    ExpireDate = DateTime.Now.AddSeconds(token.expires_in)
                };
                await InsertAsync(userInfo);
            }
            else
            {
                userInfo.LastLoginTime = DateTime.Now;
                userInfo.AccessToken = token.access_token;
                userInfo.RefreshToken = token.refresh_token;
                userInfo.ExpireDate = DateTime.Now.AddSeconds(token.expires_in);
                await UpdateAsync(userInfo, i => new { i.CreatorTime, i.IsMaster, i.OpenId });
            }
            return userInfo;
        }

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="curl">登录成功后返回的地址</param>
        /// <returns>授权地址</returns>
        public UnifyResult<string> Authorize(string curl)
        {
            return _qQAuthorize.AuthorizeLoginUrl(curl);
        }
    }
}