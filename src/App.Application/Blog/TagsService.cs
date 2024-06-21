using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Core.Repository;
using App.Framwork.Generate;
using App.Framwork.Result;
using Mapster;
using SqlSugar;

namespace App.Application.Blog
{
    public class TagsService : AppService<TagsInfo>, ITagsService
    {
        public TagsService(IAppRepository<TagsInfo> repository) : base(repository)
        {

        }

        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="dto">标签信息</param>
        /// <returns></returns>
        public async Task<UnifyResult> Save(TagsInputDto dto)
        {
            if (await AnyAsync(c => c.TagName == dto.TagName && c.Id != dto.Id))
            {
                return "标签已存在，请勿重复添加";
            }

            var tags = dto.Adapt<TagsInfo>();
            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                tags.Id = SnowflakeId.NextStringId();
                return await InsertAsync(tags);
            }
            else
            {
                return await UpdateAsync(tags, i => new { i.CreatorTime, i.DeleteMark });
            }
        }

        /// <summary>
        /// 查询各个标签文章数量
        /// </summary>
        /// <returns></returns>
        public async Task<List<TagsCountDto>> TagsCount()
        {
            return await AsQueryable().Where(tag => tag.EnabledMark)
                .OrderBy(o => o.SortCode)
                .Select(tag => new TagsCountDto
                {
                    TagId = tag.Id,
                    TagName = tag.TagName,
                    Color = tag.BGColor,
                    Total = SqlFunc.Subqueryable<ArticleTags>().Where(at => SqlFunc.Subqueryable<ArticleInfo>()
                        .Where(c => tag.Id == at.TagsId && c.Id == at.ArticleId && c.DeleteMark == false && c.Visible).Any()
                    ).Select(at => SqlFunc.AggregateCount(at.ArticleId))
                }).ToListAsync();
        }
    }
}