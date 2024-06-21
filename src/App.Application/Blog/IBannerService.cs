using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Framwork.Result;

namespace App.Application.Blog
{
    public interface IBannerService : IAppService<BannerInfo>
    {
        /// <summary>
        /// 添加、编辑banner
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult> Save(BannerInputDto dto);
    }
}