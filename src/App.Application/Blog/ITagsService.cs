using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Framwork.Result;

namespace App.Application.Blog
{
    public interface ITagsService : IAppService<TagsInfo>
    {
        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="tags">标签信息</param>
        /// <returns></returns>
        Task<UnifyResult> Save(TagsInputDto dto);

        /// <summary>
        /// 查询各个标签文章数量
        /// </summary>
        /// <returns></returns>
        Task<List<TagsCountDto>> TagsCount();
    }
}