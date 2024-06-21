using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using App.Application;
using App.Application.User;
using App.Application.User.Dtos;
using App.Core.Entities.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.UserManage.Controllers
{
    [Area("UserManage")]
    public class UserController : AdminController
    {
        private readonly ISysAccountService _sysAccountService;
        private readonly ISysUserService _sysUserService;
        public UserController(ISysAccountService sysAccountService, ISysUserService sysUserService)
        {
            _sysAccountService = sysAccountService;
            _sysUserService = sysUserService;
        }

        [HttpPost]
        [Description("系统用户信息列表")]
        public ActionResult Index(PageQueryInputDto query)
        {
            return Json(_sysAccountService.AccountPagging(query), "yyyy-MM-dd");
        }

        [HttpPost]
        [Description("新增/编辑系统用户信息")]
        public async Task<ActionResult> Form(AccountDetailsDto account)
        {
            account.CreatorAccountId = CurrentUser.AccountId;
            return Json(await _sysAccountService.Save(account));
        }

        [HttpPost]
        [Description("启用/禁用系统用户")]
        public async Task<ActionResult> Enable(string id, bool status)
        {
            return Json(await _sysAccountService.UpdateAsync(a => new SysAccount() { EnabledMark = status }, c => c.Id == id));
        }

        [HttpPost]
        [Description("删除系统用户")]
        public async Task<ActionResult> Delete(string key)
        {
            return Json(await _sysAccountService.UpdateAsync(a => new SysAccount() { DeleteMark = true, DeleteAccountId = CurrentUser.AccountId }, c => c.Id == key));
        }

        [HttpGet]
        [Description("重置系统用户密码页面")]
        public ActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        [Description("重置系统用户密码")]
        public async Task<ActionResult> Reset(ResetPasswordInputDto dto)
        {
            return Json(await _sysAccountService.ResetPassword(dto));
        }

        [HttpGet]
        [Description("获取系统用户详细信息")]
        [AllowAccess]
        public async Task<ActionResult> GetForm(string key)
        {
            return Json(await _sysAccountService.AccountDetail(key), "yyyy-MM-dd");
        }

        [HttpPost]
        [Description("上传系统用户头像")]
        [AllowAccess]
        public ActionResult Upload([FromServices] IWebHostEnvironment env)
        {
            var file = Request.Form.Files[0];
            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName);
            string path;
            if (".jpg.png.gif.jpeg".Contains(fileExtension))
            {
                string savePath = env.WebRootPath + "/Uploads/Pictures";
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                string name = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                string imgPath = savePath + "/" + name;
                using (FileStream fs = System.IO.File.Create(imgPath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                path = "/Uploads/Pictures/" + name;
            }
            else
            {
                return Error("上传图片格式必须为jpg|png|gif|jpeg");
            }
            return Success(path);
        }
    }
}