using System.Threading.Tasks;
using App.Core.Entities.User;
using App.Framwork.Result;

namespace App.Application.User
{
    public interface IQQUserService : IAppService<QQUserinfo>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="code">回调返回的code</param>
        /// <param name="state">自定义验证状态码</param>
        /// <returns></returns>
        Task<QQUserinfo> Login(string code, string state);

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="curl">登录成功后返回的地址</param>
        /// <returns>授权地址</returns>
        UnifyResult<string> Authorize(string curl);
    }
}