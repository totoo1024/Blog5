using System.Threading.Tasks;
using App.Application.SysManager.Dtos;
using App.Core.Entities.SysManager;
using App.Framwork.Result;

namespace App.Application.SysManager
{
    public interface ISysButtonService : IAppService<SysButton>
    {
        /// <summary>
        /// 新增/修改按钮
        /// </summary>
        /// <param name="dto">按钮实体</param>
        /// <returns></returns>
        Task<UnifyResult> Save(SysButtonInputDto dto);
    }
}