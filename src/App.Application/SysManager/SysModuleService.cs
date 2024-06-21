using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Application.SysManager.Dtos;
using App.Core.Entities.SysManager;
using App.Core.Repository;
using App.Framwork.Generate;
using App.Framwork.Result;
using Mapster;

namespace App.Application.SysManager
{
    public class SysModuleService : AppService<SysModule>, ISysModuleService
    {
        private readonly ISysButtonService _sysButtonService;

        public SysModuleService(IAppRepository<SysModule> repository,
            ISysButtonService sysButtonService) : base(repository)
        {
            _sysButtonService = sysButtonService;
        }

        /// <summary>
        /// 新增/修改菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UnifyResult> Save(SysModuleInputDto dto)
        {
            var module = dto.Adapt<SysModule>();
            if (await Repository.AnyAsync(m => m.EnCode.ToLower() == module.EnCode.ToLower() && m.Id != module.Id))
            {
                return "菜单编码已存在";
            }
            else
            {
                module.ParentId = string.IsNullOrWhiteSpace(module.ParentId) ? null : module.ParentId;
                if (string.IsNullOrWhiteSpace(module.Id))
                {
                    module.Id = SnowflakeId.NextStringId();
                    return await InsertRemoveCacheAsync(module);
                }
                else
                {
                    if (module.Id == module.ParentId)
                    {
                        return "父级菜单不能指向当前菜单";
                    }
                    if (module.ParentId != "0")
                    {
                        var list = await GetListCacheAsync(m => m.DeleteMark == false && m.EnabledMark);
                        if (GetChildMenu(list, module.Id).Any(o => o.Id == module.ParentId))
                        {
                            return "无法将父级菜单指定到子级菜单下";
                        }
                    }
                    return await UpdateRemoveCacheAsync(module, c => new { c.CreatorTime, c.CreatorAccountId, c.DeleteMark });
                }
            }
        }

        /// <summary>
        /// 菜单按钮树
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeOutputDto>> Tree()
        {
            //所有菜单
            var menuList = await GetListCacheAsync(m => m.EnabledMark && m.DeleteMark == false);
            //所有按钮
            var buttonList = await _sysButtonService.GetListCacheAsync();

            var list = GetTrees(menuList, buttonList, "0");
            return list;
        }

        /// <summary>
        /// 获取菜单按钮树
        /// </summary>
        /// <param name="modules">所有菜单</param>
        /// <param name="buttons">所有按钮</param>
        /// <param name="parentId">上级id</param>
        /// <returns></returns>
        private List<TreeOutputDto> GetTrees(List<SysModule> modules, List<SysButton> buttons, string parentId)
        {
            List<TreeOutputDto> treeModules = new List<TreeOutputDto>();
            foreach (var item in modules.Where(m => m.ParentId == parentId))
            {
                TreeOutputDto tree = new TreeOutputDto
                {
                    id = item.Id,
                    name = item.FullName,
                    pid = item.ParentId ?? "0"
                };
                treeModules.Add(tree);

                var bts = buttons.Where(b => b.SysModuleId == item.Id);//获取当前菜单下所有按钮
                if (bts.Any())
                {
                    var trees = from b in bts select new TreeOutputDto { id = b.Id, name = b.FullName, pid = b.SysModuleId };
                    treeModules.AddRange(trees);
                }
                treeModules.AddRange(GetTrees(modules, buttons, item.Id));
            }
            return treeModules;
        }

        /// <summary>
        /// 获取指定菜单下所有菜单
        /// </summary>
        /// <param name="list">所有菜单集合</param>
        /// <param name="id">指定菜单ID</param>
        /// <returns></returns>
        private List<SysModule> GetChildMenu(List<SysModule> list, string id)
        {
            List<SysModule> modules = new List<SysModule>();
            foreach (var item in list.Where(m => m.ParentId == id))
            {
                modules.Add(item);
                GetChildMenu(list, item.Id);
            }
            return modules;
        }
    }
}