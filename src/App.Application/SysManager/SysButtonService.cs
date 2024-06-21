using System.Threading.Tasks;
using App.Application.SysManager.Dtos;
using App.Core.Entities.SysManager;
using App.Core.Repository;
using App.Framwork.Generate;
using App.Framwork.Result;
using Mapster;

namespace App.Application.SysManager
{
    public class SysButtonService : AppService<SysButton>, ISysButtonService
    {
        public SysButtonService(IAppRepository<SysButton> repository) : base(repository)
        {
        }

        /// <summary>
        /// 新增/修改按钮
        /// </summary>
        /// <param name="dto">按钮实体</param>
        /// <returns></returns>
        public async Task<UnifyResult> Save(SysButtonInputDto dto)
        {
            var sysButton = dto.Adapt<SysButton>();
            if (await Repository.AnyAsync(c => c.EnCode == sysButton.EnCode && c.Id != sysButton.Id && c.SysModuleId == sysButton.SysModuleId))
            {
                return "按钮编码已存在";
            }
            if (string.IsNullOrEmpty(sysButton.Id))
            {
                sysButton.Id = SnowflakeId.NextStringId();
                return await InsertRemoveCacheAsync(sysButton);
            }
            else
            {
                return await UpdateRemoveCacheAsync(sysButton, i => new { i.SysModuleId, i.CreatorTime, i.CreatorAccountId });
            }
        }
    }
}