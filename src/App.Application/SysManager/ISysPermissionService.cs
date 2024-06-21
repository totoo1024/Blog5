using System.Threading.Tasks;
using App.Application.SysManager.Dtos;
using App.Core.Entities.SysManager;
using App.Framwork.Result;

namespace App.Application.SysManager
{
    public interface ISysPermissionService : IAppService<SysPermission>
    {
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="dto">角色权限分配信息</param>
        /// <returns></returns>
        Task<UnifyResult> Save(SysPermissionInputDto dto);

        /// <summary>
        /// 获取指定角色、部门、用户所有可访问的菜单按钮信息
        /// </summary>
        /// <param name="authorrizeId">角色ID、部门ID、用户ID</param>
        /// <returns></returns>
        Task<SysAllModuleDto> GetMenuAndButton(string authorrizeId);

        /// <summary>
        /// 检验权限
        /// </summary>
        /// <param name="authorizeId">授权id（角色ID/部门ID/用户ID）</param>
        /// <param name="url">授权地址</param>
        /// <returns></returns>
        bool CheckPermission(string authorizeId, string url);
    }
}