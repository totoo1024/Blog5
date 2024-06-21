using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Application.User.Dtos;
using App.Core.Entities.User;
using App.Core.Repository;
using App.Framwork.Result;
using App.Framwork.Encryption;
using App.Framwork.Generate;
using Mapster;

namespace App.Application.User
{
    public class SysAccountService : AppService<SysAccount>, ISysAccountService
    {
        private readonly ISysUserService _sysUserService;

        public SysAccountService(IAppRepository<SysAccount> repository, ISysUserService sysUserService) : base(repository)
        {
            _sysUserService = sysUserService;
        }

        /// <summary>
        /// 新增/编辑系统用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UnifyResult> Save(AccountDetailsDto dto)
        {
            var account = dto.Adapt<SysAccount>();
            var user = dto.Adapt<SysUser>();
            bool exs = await Repository.AnyAsync(a => a.UserName == account.UserName && a.Id != dto.AccountId);
            if (exs)
            {
                return "用户名已存在";
            }
            //添加
            if (string.IsNullOrWhiteSpace(dto.AccountId))
            {
                account.Id = SnowflakeId.NextStringId();
                user.AccountId = account.Id;
                user.Id = SnowflakeId.NextStringId();
                account.Password = Md5Encrypt.Encrypt(account.Id + account.Password);
                var result = await InsertAsync(account);
                await _sysUserService.InsertAsync(user);
                return result;
            }
            else
            {
                //编辑
                var result = await UpdateAsync(a => new SysAccount() { UserName = account.UserName, RoleId = account.RoleId }, c => c.Id == dto.AccountId);
                if (result.StatusCode == ResultCode.Success)
                {
                    result = await _sysUserService.UpdateAsync(user, c => c.AccountId == dto.AccountId, ig => new { ig.Id, ig.AccountId });
                }
                return result;
            }
        }

        /// <summary>
        /// 重置系统用户密码
        /// </summary>
        /// <param name="dto">重置用户密码信息</param>
        /// <returns></returns>
        public async Task<UnifyResult> ResetPassword(ResetPasswordInputDto dto)
        {
            string password = Md5Encrypt.Encrypt($"{dto.AccountId}{dto.Password}");
            return await UpdateAsync(a => new SysAccount { Password = password }, c => c.Id == dto.AccountId);
        }

        /// <summary>
        /// 变更密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UnifyResult> ChangePassword(ChangePasswordInputDto dto)
        {
            SysAccount account = await FindEntityAsync(c => c.Id == dto.AccountId);
            if (account == null)
            {
                return "用户不存在";
            }
            string pwd = Md5Encrypt.Encrypt($"{account.Id}{dto.Original}");
            if (account.Password != pwd)
            {
                return "输入的原始密码错误";
            }

            pwd = Md5Encrypt.Encrypt($"{dto.AccountId}{dto.Password}");
            return await UpdateAsync(a => new SysAccount() { Password = pwd }, c => c.Id == dto.AccountId);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UnifyResult<AuthUser>> Login(LoginInputDto dto)
        {
            var account = await GetAccountDetails(a => a.UserName == dto.UserName);
            if (account == null)
            {
                return ("用户名或密码错误", false);
            }
            if (account.Password != Md5Encrypt.Encrypt(account.AccountId + dto.Password))
            {
                return ("用户名或密码错误", false);

            }
            if (!account.EnabledMark)
            {
                return ("登录账户已被禁用", false);
            }

            var authUser = account.Adapt<AuthUser>();
            authUser.LoginId = SnowflakeId.NextStringId();
            authUser.HeadIcon ??= "/images/default.png";
            return authUser;
        }

        /// <summary>
        /// 分页查询用户详细信息
        /// </summary>
        /// <param name="dto">分页查询实体</param>
        /// <returns></returns>
        public PageOutputDto<List<AccountDetailsDto>> AccountPagging(PageQueryInputDto dto)
        {
            var query = Repository.Db.Queryable<SysAccount, SysUser>((a, u) => a.Id == u.AccountId);
            if (dto.ConditionalModels.Any())
            {
                query.Where(dto.ConditionalModels);
            }

            int total = 0;
            var list = query.Select<AccountDetailsDto>().ToPageList(dto.Page, dto.Limit, ref total);
            return (list, total);
        }

        /// <summary>
        /// 获取系统账户详细信息
        /// </summary>
        /// <param name="accountId">账户id</param>
        /// <returns></returns>
        public async Task<AccountDetailsDto> AccountDetail(string accountId)
        {
            return await GetAccountDetails(a => a.Id == accountId);
        }


        private async Task<AccountDetailsDto> GetAccountDetails(Expression<Func<SysAccount, bool>> expression)
        {
            var query = Repository.Db.Queryable<SysAccount, SysUser>((a, u) => a.Id == u.AccountId);
            return await query.Where(expression).Select<AccountDetailsDto>().FirstAsync();
        }
    }
}