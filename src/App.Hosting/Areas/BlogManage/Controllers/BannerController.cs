using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using App.Application;
using App.Application.Blog;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class BannerController : AdminController
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        [HttpPost]
        public IActionResult Index(PageQueryInputDto query)
        {
            return Json(_bannerService.GetListByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增编辑轮播图")]

        public async Task<IActionResult> Form(BannerInputDto banner)
        {
            return Json(await _bannerService.Save(banner));
        }

        [AllowAccess]
        [Description("轮播图详情")]
        public async Task<IActionResult> Detail(string key)
        {
            return Json(await _bannerService.FindEntityAsync(key));
        }

        /// <summary>
        /// 上传banner图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAccess]
        public IActionResult UploadImg([FromServices] IHostEnvironment env)
        {
            string path;
            try
            {
                var file = Request.Form.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string fileExtension = Path.GetExtension(fileName);
                if (".jpg.png.gif.jpeg".Contains(fileExtension))
                {
                    string savePath = env.ContentRootPath + "/Uploads/Resource";
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
                    path = "/Uploads/Resource/" + name;
                }
                else
                {
                    return Error("上传图片格式必须为jpg|png|gif|jpeg");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success(path);
        }

        [HttpPost]
        [Description("删除轮播图")]
        public IActionResult Delete(string key)
        {
            return Json(_bannerService.UpdateRemoveCache(b => new BannerInfo() { DeleteMark = true }, c => c.Id == key));
        }
    }
}