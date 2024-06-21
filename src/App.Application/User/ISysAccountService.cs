using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.User.Dtos;
using App.Core.Entities.User;
using App.Framwork.Result;

namespace App.Application.User
{
    public interface ISysAccountService : IAppService<SysAccount>
    {
        /// <summary>
        /// 新增/编辑系统用户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        Task<UnifyResult> Save(AccountDetailsDto dto /*, string createUserId*/);

        /// <summary>
        /// 重置系统用户密码
        /// </summary>
        /// <param name="dto">重置用户密码信息</param>
        /// <returns></returns>
        Task<UnifyResult> ResetPassword(ResetPasswordInputDto dto);

        /// <summary>
        /// 变更密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult> ChangePassword(ChangePasswordInputDto dto);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult<AuthUser>> Login(LoginInputDto dto);

        /// <summary>
        /// 分页查询用户详细信息
        /// </summary>
        /// <param name="dto">分页查询实体</param>
        /// <returns></returns>
        PageOutputDto<List<AccountDetailsDto>> AccountPagging(PageQueryInputDto dto);

        /// <summary>
        /// 获取系统账户详细信息
        /// </summary>
        /// <param name="accountId">账户id</param>
        /// <returns></returns>
        Task<AccountDetailsDto> AccountDetail(string accountId);
    }
}