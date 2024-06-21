using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Application.SysManager.Dtos;
using App.Core.Data.Interceptor;
using App.Core.Entities.SysManager;
using App.Core.Repository;
using App.Framwork.Generate;
using App.Framwork.Result;
using EasyCaching.Core;

namespace App.Application.SysManager
{
    public class SysPermissionService : AppService<SysPermission>, ISysPermissionService
    {
        private readonly ISysRoleService _sysRoleService;
        private readonly ISysModuleService _sysModuleService;
        private readonly ISysButtonService _sysButtonService;
        private readonly IEasyCachingProvider _easyCachingProvider;

        const string KeyPrefix = "Auth-";

        public SysPermissionService(IAppRepository<SysPermission> repository
            , ISysRoleService sysRoleService,
            ISysModuleService sysModuleService,
            ISysButtonService sysButtonService,
            IEasyCachingProvider easyCachingProvider) : base(repository)
        {
            _sysRoleService = sysRoleService;
            _sysModuleService = sysModuleService;
            _sysButtonService = sysButtonService;
            _easyCachingProvider = easyCachingProvider;
        }

        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="dto">角色权限分配信息</param>
        /// <returns></returns>
        [Transaction]
        public async Task<UnifyResult> Save(SysPermissionInputDto dto)
        {
            bool any = await _sysRoleService.AnyAsync(c => c.Id == dto.RoleId);
            if (!any)
            {
                return "所分配角色不存在";
            }

            var menus = await _sysModuleService.GetListAsync(m => dto.Permission.Contains(m.Id));
            var buttons = await _sysButtonService.GetListAsync(b => dto.Permission.Contains(b.Id));

            if (!menus.Any() && !buttons.Any())
            {
                return "分配的权限不能为空";
            }

            //删除现有权限重新分配权限
            await DeleteRemoveCacheAsync(c => c.AuthorizeId == dto.RoleId);

            List<SysPermission> list = new List<SysPermission>();
            if (menus.Any())
            {
                list = (from m in menus
                        select new SysPermission
                        {
                            Id = SnowflakeId.NextStringId(),
                            AuthorizeId = dto.RoleId,
                            ModuleType = 1,
                            ObjectType = 1,
                            SysModuleId = m.Id

                        }).ToList();
            }
            if (buttons.Any())
            {
                var bs = (from b in buttons
                          select new SysPermission
                          {
                              Id = SnowflakeId.NextStringId(),
                              AuthorizeId = dto.RoleId,
                              ModuleType = 2,
                              ObjectType = 1,
                              SysModuleId = b.Id

                          }).ToList();
                list.AddRange(bs);
            }
            string key = $"{KeyPrefix}{dto.RoleId}";
            //删除缓存
            await _easyCachingProvider.RemoveAsync(key);
            return await InsertRemoveCacheAsync(list);
        }

        /// <summary>
        /// 获取指定角色、部门、用户所有可访问的菜单按钮信息
        /// </summary>
        /// <param name="authorrizeId">角色ID、部门ID、用户ID</param>
        /// <returns></returns>
        public async Task<SysAllModuleDto> GetMenuAndButton(string authorrizeId)
        {
            string key = $"{KeyPrefix}{authorrizeId}";
            //获取缓存
            var data = await _easyCachingProvider.GetAsync<SysAllModuleDto>(key);
            if (!data.IsNull)
            {
                return data.Value;
            }

            var r = await _sysRoleService.GetListAsync(null);

            var author = await GetListAsync(p => authorrizeId == p.AuthorizeId);
            var mids = author.Select(c => c.SysModuleId).ToArray();
            //所有菜单
            var menuList = await _sysModuleService.GetListAsync(m => mids.Contains(m.Id) && m.EnabledMark);
            //所有按钮
            var buttonList = await _sysButtonService.GetListAsync(b => mids.Contains(b.Id));
            List<MenuSettingDto> topMenu = new List<MenuSettingDto>();
            Dictionary<string, List<MenuSettingDto>> childMenu = new Dictionary<string, List<MenuSettingDto>>();
            Dictionary<string, List<SysButton>> tool = new Dictionary<string, List<SysButton>>();
            Dictionary<string, List<SysButton>> row = new Dictionary<string, List<SysButton>>();
            foreach (var menu in menuList.Where(m => m.ParentId == "0").OrderBy(o => o.SortCode))
            {
                var child = GetChildMenuAndButton(menuList, buttonList, menu.Id, tool, row);
                topMenu.Add(new MenuSettingDto()
                {
                    id = menu.EnCode,
                    title = menu.FullName,
                    icon = menu.Icon,
                    href = menu.UrlAddress
                });
                childMenu[menu.EnCode] = child;
            }
            var result = new SysAllModuleDto { topMenus = topMenu, childMenus = childMenu, rowButtons = row, toolButtons = tool };
            //设置缓存
            if (childMenu.Any() || childMenu.Any() || row.Any() || tool.Any())
            {
                await _easyCachingProvider.SetAsync(key, result, TimeSpan.FromDays(1));
            }
            return result;
        }

        /// <summary>
        /// 检验权限
        /// </summary>
        /// <param name="authorizeId">授权id（角色ID/部门ID/用户ID）</param>
        /// <param name="url">授权地址</param>
        /// <returns></returns>
        public bool CheckPermission(string authorizeId, string url)
        {
            url = url.ToLower();
            List<SysModule> menus;
            List<SysButton> buttons;
            var permissions = GetList(p => p.AuthorizeId == authorizeId).Select(c => c.SysModuleId).ToList();
            if (permissions.Any())
            {
                menus = _sysModuleService.GetListCache(m => permissions.Contains(m.Id));
                buttons = _sysButtonService.GetListCache(b => permissions.Contains(b.Id));
                if (menus.Any(m => m.UrlAddress != null && m.UrlAddress.ToLower() == url) || buttons.Any(b => b.UrlAddress != null && b.UrlAddress.ToLower() == url))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 获取菜单和菜单下的按钮
        /// </summary>
        /// <param name="modules">所有菜单</param>
        /// <param name="buttons">所有按钮</param>
        /// <param name="parentId">上级菜单id</param>
        /// <param name="toolButton">工具栏按钮</param>
        /// <param name="rowButton">行内按钮</param>
        /// <returns></returns>
        private List<MenuSettingDto> GetChildMenuAndButton(List<SysModule> modules, List<SysButton> buttons, string parentId, Dictionary<string, List<SysButton>> toolButton, Dictionary<string, List<SysButton>> rowButton)
        {
            List<MenuSettingDto> list = new List<MenuSettingDto>();
            foreach (var item in modules.Where(m => m.ParentId == parentId).OrderBy(o => o.SortCode))
            {
                MenuSettingDto ms = new MenuSettingDto();
                ms.id = item.EnCode;
                ms.title = item.FullName;
                ms.icon = item.Icon;
                ms.href = item.UrlAddress;
                ms.children = GetChildMenuAndButton(modules, buttons, item.Id, toolButton, rowButton);
                list.Add(ms);
                var temptool = buttons.Where(b => b.SysModuleId == item.Id && b.Location == 1).OrderBy(o => o.SortCode).ToList();
                if (temptool.Any())
                {
                    toolButton.Add(item.EnCode, temptool);
                }
                var temprow = buttons.Where(b => b.SysModuleId == item.Id && b.Location == 2).OrderBy(o => o.SortCode).ToList();
                if (temprow.Any())
                {
                    rowButton.Add(item.EnCode, temprow);
                }
            }
            return list;
        }
    }
}