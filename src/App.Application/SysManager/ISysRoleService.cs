using System.Threading.Tasks;
using App.Application.SysManager.Dtos;
using App.Core.Entities.SysManager;
using App.Framwork.Result;

namespace App.Application.SysManager
{
    public interface ISysRoleService : IAppService<SysRole>
    {
        /// <summary>
        /// 新增/修改角色
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        Task<UnifyResult> Save(SysRoleInputDto dto);
    }
}