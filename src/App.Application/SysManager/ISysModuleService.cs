using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.SysManager.Dtos;
using App.Core.Entities.SysManager;
using App.Framwork.Result;

namespace App.Application.SysManager
{
    public interface ISysModuleService : IAppService<SysModule>
    {
        /// <summary>
        /// 新增/修改菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult> Save(SysModuleInputDto dto);

        /// <summary>
        /// 菜单按钮树
        /// </summary>
        /// <returns></returns>
        Task<List<TreeOutputDto>> Tree();
    }
}