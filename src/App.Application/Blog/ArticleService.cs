using System;
using System.Collections.Generic;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Core.Repository;
using App.Framwork.Result;
using Mapster;
using System.Linq;
using System.Threading.Tasks;
using App.Framwork.Generate;
using SqlSugar;
using App.Core.Data.Interceptor;

namespace App.Application.Blog
{
    public class ArticleService : AppService<ArticleInfo>, IArticleService
    {
        private readonly IArticleCategoryService _articleCategoryService;
        private readonly IArticleTagsService _articleTagsService;
        private readonly ITagsService _tagsService;
        private readonly ILeavemsgService _leavemsgService;

        public ArticleService(IAppRepository<ArticleInfo> repository,
            IArticleCategoryService articleCategoryService,
            IArticleTagsService articleTagsService,
            ITagsService tagsService,
            ILeavemsgService leavemsgService) : base(repository)
        {
            _articleCategoryService = articleCategoryService;
            _articleTagsService = articleTagsService;
            _tagsService = tagsService;
            _leavemsgService = leavemsgService;
        }

        /// <summary>
        /// 添加、修改文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Transaction]
        public async Task<UnifyResult> Save(ArticleInputDto dto)
        {
            bool isAdded = string.IsNullOrEmpty(dto.Id);
            ArticleInfo article = dto.Adapt<ArticleInfo>();
            if (isAdded)
            {
                article.Id = SnowflakeId.NextStringId();
                await InsertAsync(article);
            }
            else
            {
                await UpdateAsync(article, x => new { x.CreatorTime, x.Visible, x.ReadTimes });
                await _articleCategoryService.DeleteAsync(x => x.ArticleId == article.Id);
                await _articleTagsService.DeleteAsync(x => x.ArticleId == article.Id);
            }
            List<ArticleCategory> articleCategories = dto.Categories.Select(x => new ArticleCategory { Id = SnowflakeId.NextStringId(), ArticleId = article.Id, CategoryId = x }).ToList();
            List<ArticleTags> articleTags = dto.Tags.Select(x => new ArticleTags { Id = SnowflakeId.NextStringId(), ArticleId = article.Id, TagsId = x }).ToList();
            await _articleCategoryService.InsertAsync(articleCategories);
            await _articleTagsService.InsertAsync(articleTags);
            return true;
        }

        /// <summary>
        /// 文章列表分页
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PageOutputDto<List<ArticleDto>> ArticleList(ArticleQueryInputDto dto)
        {
            int total = 0;
            var query = Repository.SugarQueryable.Where(a => a.Visible);
            if (!string.IsNullOrEmpty(dto.Keywords))
            {
                query.Where(a => a.Title.Contains(dto.Keywords)
                || SqlFunc.Subqueryable<TagsInfo>().Where(t => t.TagName.ToLower().Contains(dto.Keywords.ToLower())).Any()
                || SqlFunc.Subqueryable<CategoryInfo>().Where(ci => ci.CategoryName.ToLower().Contains(dto.Keywords.ToLower())).Any()
                );

            }
            if (!string.IsNullOrWhiteSpace(dto.Id))
            {
                if (dto.Type == 1)
                {
                    query.Where(a => SqlFunc.Subqueryable<ArticleCategory>().Where(ac => ac.ArticleId == a.Id && ac.CategoryId == dto.Id).Any());
                }
                else
                {
                    query.Where(a => SqlFunc.Subqueryable<ArticleTags>().Where(at => at.ArticleId == a.Id && at.TagsId == dto.Id).Any());
                }
            }
            if (!string.IsNullOrWhiteSpace(dto.Sort))
            {
                query.OrderBy(dto.Sort);
            }

            Dictionary<string, List<string>> keys = new Dictionary<string, List<string>>();
            List<string> tagIds = new List<string>();
            List<ArticleDto> ad = query.Select<ArticleDto>().Mapper((it, cache) =>
            {
                var tids = cache.Get(list =>
                {
                    var ids = list.Select(i => i.Id).ToList();

                    var tags = _articleTagsService.AsQueryable().Where(c => ids.Contains(c.ArticleId)).ToList();
                    var dic = tags.GroupBy(x => x.ArticleId)
                        .ToDictionary(x => x.Key, x => x.ToList().Select(b => b.TagsId).ToList());
                    foreach (var item in dic)
                    {
                        keys.Add(item.Key, item.Value);
                    }
                    return tags.Select(f => f.TagsId).ToList();
                });
                tagIds.AddRange(tids);

            }).ToPageList(dto.Page, dto.Limit, ref total);
            List<string> articleIds = keys.Select(x => x.Key).ToList();
            List<TagDto> tagDtos = _tagsService.AsQueryable()
                .Where(c => c.EnabledMark && tagIds.Contains(c.Id)).Select<TagDto>()
                .ToList();

            var msgCount = _leavemsgService.AsQueryable()
                .Where(msg => SqlFunc.IsNullOrEmpty(msg.ToUId) && articleIds.Contains(msg.ArticleId))
                .GroupBy(msg => msg.ArticleId).Select(msg => new { Id = msg.ArticleId, Total = SqlFunc.AggregateCount(msg.ArticleId) })
                .ToList();
            foreach (var item in ad)
            {
                if (keys.ContainsKey(item.Id))
                {
                    item.Tags = tagDtos.Where(x => keys[item.Id].Contains(x.Id)).ToList();
                }

                item.MsgTimes = msgCount.FirstOrDefault(x => x.Id == item.Id)?.Total ?? 0;
            }

            return (ad, total);
        }

        /// <summary>
        /// 热门文章
        /// </summary>
        /// <param name="topN">前N条</param>
        /// <returns></returns>
        public async Task<List<ArticleHotDto>> Hot(int topN)
        {
            return await AsQueryable().Where(x => x.Visible)
                 .Select<ArticleHotDto>()
                 .OrderBy(x => x.Id, OrderByType.Desc)
                 .Take(topN).ToListAsync();
        }

        /// <summary>
        /// 随机文章10条
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> Random()
        {
            return (await GetListAsync(c => c.Visible, o => SqlFunc.GetRandom(), false, 10)).Select(s => new { ArticleId = s.Id, Title = s.Title });
        }
    }
}