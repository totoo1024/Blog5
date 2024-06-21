using System;
using System.Linq;
using System.Threading.Tasks;
using App.Application.Blog;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace App.Hosting.Controllers
{
    public class ArticleController : WebController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagsService _tagsService;
        public ArticleController(IArticleService articleService,
            ICategoryService categoryService,
            ITagsService tagsService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _tagsService = tagsService;
        }

        /// <summary>
        /// 文章专栏页
        /// </summary>
        /// <param name="cid">栏目id</param>
        /// <param name="tid">标签id</param>
        /// <returns></returns>
        public IActionResult List(string cid, string tid)
        {
            string name = string.Empty;
            if (!string.IsNullOrWhiteSpace(cid))
            {
                name = _categoryService.FindEntity(c => c.EnabledMark && c.Id == cid)?.CategoryName;
            }
            if (!string.IsNullOrWhiteSpace(tid))
            {
                name = _tagsService.FindEntity(c => c.EnabledMark && c.Id == tid)?.TagName;
            }
            ViewBag.CategoryName = name;
            return View();
        }

        /// <summary>
        /// 首页文章
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="limit">每页显示数量</param>
        /// <returns></returns>
        public IActionResult Page(ArticleQueryInputDto dto)
        {
            dto.Type = 1;
            dto.Keywords = dto.Id = string.Empty;
            dto.Sort = "IsTop desc,PublishDate desc";
            var data = _articleService.ArticleList(dto);
            if (data.count > 0)
            {
                var no = data.count * 1d / dto.Limit;
                data.count = (int)Math.Ceiling(no);
            }
            return Json(data, "yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 文章专栏列表
        /// </summary>
        /// <param name="key">关键词</param>
        /// <param name="tid">标签id</param>
        /// <param name="cid">栏目id</param>
        /// <param name="page">当前页</param>
        /// <param name="limit">每页显示的条数</param>
        /// <returns></returns>
        public IActionResult Views(string key, string tid, string cid, int page = 1, int limit = 10)
        {
            int type = 1;
            string id = "";
            id = tid ?? id;
            id = cid ?? id;
            if (!string.IsNullOrWhiteSpace(tid))
            {
                type = 2;
            }

            var dto = new ArticleQueryInputDto
            {
                Keywords = key,
                Id = id,
                Type = type,
                Limit = limit,
                Page = page,
                Sort = "IsTop desc,PublishDate desc"
            };
            var data = _articleService.ArticleList(dto);
            if (data.count > 0)
            {
                var no = data.count * 1d / limit;
                data.count = (int)Math.Ceiling(no);
            }
            return Json(data, "yyyy-MM-dd");
        }

        /// <summary>
        /// 文章详情
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(string id)
        {
            ArticleInfo article = await _articleService.FindEntityAsync(c => c.Visible && c.Id == id);
            if (article != null)
            {
                await _articleService.UpdateAsync(f => new ArticleInfo() { ReadTimes = f.ReadTimes + 1 }, c => c.Id == id);
                article.ReadTimes += 1;
            }
            return View(article);
        }

        /// <summary>
        /// 热门文章以及分类
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Module()
        {
            var hot = await _articleService.Hot(6);
            var category = await _categoryService.GetRootCategories();
            return Json(new { hot, category });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DetailModule()
        {
            var hot = await _articleService.Hot(6);
            var category = await _categoryService.GetRootCategories();
            var random = await _articleService.Random();
            return Json(new { hot, category, random });
        }
    }
}