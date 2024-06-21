using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Framwork.Result;
using EasyCaching.Core.Interceptor;

namespace App.Application.Blog
{
    public interface IArticleService : IAppService<ArticleInfo>
    {
        /// <summary>
        /// 添加、修改文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult> Save(ArticleInputDto dto);

        /// <summary>
        /// 文章列表分页
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        //[EasyCachingAble(Expiration = 1000)]
        PageOutputDto<List<ArticleDto>> ArticleList(ArticleQueryInputDto dto);

        /// <summary>
        /// 热门文章
        /// </summary>
        /// <param name="topN">前N条</param>
        /// <returns></returns>
        Task<List<ArticleHotDto>> Hot(int topN);

        /// <summary>
        /// 随机文章10条
        /// </summary>
        /// <returns></returns>
        Task<dynamic> Random();
    }
}