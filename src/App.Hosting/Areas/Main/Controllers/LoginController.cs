using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.User;
using App.Application.User.Dtos;
using App.Core.Config;
using App.Core.Entities.Logs;
using App.Framwork.Generate;
using App.Framwork.Generate.Geetest;
using App.Framwork.Result;
using App.Hosting.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace App.Hosting.Areas.Main.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [Area("Main")]
    public class LoginController : Controller
    {
        private readonly ISysAccountService _sysAccountService;
        private readonly IGenerateValidate _generateValidate;
        private readonly SysConfig _sysConfig;

        public LoginController(ISysAccountService sysAccountService,
            IGenerateValidate generateValidate,
            IOptionsMonitor<SysConfig> optionsMonitor)
        {
            _sysAccountService = sysAccountService;
            _generateValidate = generateValidate;
            _sysConfig = optionsMonitor.CurrentValue;
        }
        // GET
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Main/Home/Index");
            }
            return View(_sysConfig.UseGeetest);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileContentResult ValidateCode()
        {
            VerifyCode code = new VerifyCode();
            code.SetHeight = 36;
            code.SetForeNoisePointCount = 4;
            var byteImg = code.GetVerifyCodeImage();
            HttpContext.Session.SetString("VerifyCode", code.SetVerifyCodeText);
            return File(byteImg, @"image/jpeg");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto">用户名/密码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Login(LoginInputDto dto, string code)
        {
            UnifyResult<string> result = new UnifyResult<string>();
            if (_sysConfig.UseGeetest)
            {
                if (!_generateValidate.Validate())
                {
                    result.StatusCode = ResultCode.ValidError;
                    result.Message = "请重新验证";
                    return Json(result);
                }
            }
            else
            {
                string verifyCode = HttpContext.Session.GetString("VerifyCode");
                if (verifyCode == null)
                {
                    result.StatusCode = ResultCode.ValidError;
                    result.Message = "验证码已过期";
                    return Json(result);
                }

                if (code.ToLower() != verifyCode.ToString().ToLower())
                {
                    result.StatusCode = ResultCode.ValidError;
                    result.Message = "验证码有误";
                    return Json(result);
                }
                //清除验证码
                HttpContext.Session.Remove("VerifyCode");
            }
            var data = await _sysAccountService.Login(dto);
            AuthUser auth = data.Data;
            if (auth != null)
            {
                await HttpContext.Login(auth);
                result.Data = "/Main/Home/Index";
            }
            result.StatusCode = data.StatusCode;
            result.Message = data.Message;
            return Json(result);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public new async Task<JsonResult> SignOut()
        {
            AuthUser user = HttpContext.GetAuthUser();
            await HttpContext.SignOut();
            return Json(new UnifyResult("注销成功"));
        }

        [HttpGet]
        public IActionResult Register()
        {
            var json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(_generateValidate.Generate());
            return Json(json);
        }
    }
}