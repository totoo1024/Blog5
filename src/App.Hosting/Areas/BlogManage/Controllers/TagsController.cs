using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using App.Application;
using App.Application.Blog;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using Microsoft.AspNetCore.Mvc;

namespace App.Hosting.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class TagsController : AdminController
    {
        private readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpPost]
        [Description("标签列表")]
        public IActionResult Index(PageQueryInputDto query)
        {
            return Json(_tagsService.GetListByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增/编辑标签")]
        public async Task<IActionResult> Form(TagsInputDto tags)
        {
            return Json(await _tagsService.Save(tags));
        }

        [AllowAccess]
        [Description("标签详情")]
        public async Task<IActionResult> Detail(string key)
        {
            return Json(await _tagsService.FindEntityAsync(c => c.Id == key));
        }

        [HttpPost]
        [Description("删除标签")]
        public async Task<IActionResult> Delete(string key)
        {
            return Json(await _tagsService.UpdateAsync(tag => new TagsInfo() { DeleteMark = true }, c => c.Id == key));
        }

        [HttpPost]
        [Description("启用禁用标签")]
        public async Task<IActionResult> Enable(string id, bool status)
        {
            return Json(await _tagsService.UpdateAsync(tag => new TagsInfo() { EnabledMark = status }, c => c.Id == id));
        }

        [AllowAccess]
        [Description("获取所有标签")]
        public async Task<IActionResult> List()
        {
            return Json((await _tagsService.GetListAsync(c => c.EnabledMark)).Select(d => new { value = d.Id, name = d.TagName, selected = "", disabled = "" }));
        }
    }
}