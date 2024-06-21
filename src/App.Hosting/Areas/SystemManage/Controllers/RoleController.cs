using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using App.Application;
using App.Application.SysManager;
using App.Application.SysManager.Dtos;
using App.Core.Entities.SysManager;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class RoleController : AdminController
    {
        private readonly ISysRoleService _sysRoleService;
        public RoleController(ISysRoleService sysRoleService)
        {
            _sysRoleService = sysRoleService;
        }

        [HttpPost]
        [Description("角色数据列表")]
        public ActionResult Index(PageQueryInputDto query)
        {
            return Json(_sysRoleService.GetListByPage(query));
        }

        [HttpPost]
        [Description("新增/修改角色")]
        public async Task<ActionResult> Form(SysRoleInputDto sysRole)
        {
            sysRole.CreatorAccountId = CurrentUser.AccountId;
            return Json(await _sysRoleService.Save(sysRole));
        }

        [HttpGet, AllowAccess]
        [Description("获取角色详情")]
        public ActionResult GetForm(string key)
        {
            return Json(_sysRoleService.FindEntity(o => o.Id == key));
        }

        [HttpPost]
        [Description("启用/禁用角色")]
        public async Task<ActionResult> Enable(string id, bool status)
        {
            return Json(await _sysRoleService.UpdateAsync(m => new SysRole() { EnabledMark = status }, c => c.Id == id));
        }

        [HttpPost]
        [Description("删除角色")]
        public async Task<JsonResult> Delete(string key)
        {
            return Json(await _sysRoleService.UpdateAsync(m => new SysRole() { DeleteMark = true }, c => c.Id == key));
        }

        [HttpGet, AllowAccess]
        [Description("获取角色下拉框")]
        public async Task<ActionResult> Select()
        {
            var list = (await _sysRoleService.GetListAsync(m => m.EnabledMark && m.DeleteMark == false)).Select(s => new { text = s.FullName, value = s.Id });
            return Json(list);
        }
    }
}