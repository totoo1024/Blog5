using System.Threading.Tasks;
using App.Application.User;
using App.Application.User.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomeController : AdminController
    {
        private readonly ISysAccountService _sysAccountService;

        public HomeController(ISysAccountService sysAccountService)
        {
            _sysAccountService = sysAccountService;
        }

        [AllowAccess]
        public override IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 默认加载页
        /// </summary>
        /// <returns></returns>
        [AllowAccess]
        public ActionResult Default()
        {
            return View();
        }

        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <returns></returns>
        [AllowAccess]
        public IActionResult Password()
        {
            return View();
        }

        [AllowAccess]
        public IActionResult Info()
        {
            ViewBag.AccountId = CurrentUser.AccountId;
            return View();
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAccess]
        public async Task<IActionResult> Info(AccountDetailsDto account)
        {
            account.AccountId = CurrentUser.AccountId;
            account.RoleId = CurrentUser.RoleId;
            account.UserName = CurrentUser.UserName;
            return Json(await _sysAccountService.Save(account));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAccess]
        public async Task<IActionResult> ChangePwd(ChangePasswordInputDto dto)
        {
            dto.AccountId = CurrentUser.AccountId;
            return Json(await _sysAccountService.ChangePassword(dto));
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAccess]
        public async Task<IActionResult> Lock(string pwd)
        {
            var dto = new LoginInputDto
            {
                Password = pwd,
                UserName = CurrentUser.UserName
            };
            var data = await _sysAccountService.Login(dto);
            return Json(data);
        }
    }
}