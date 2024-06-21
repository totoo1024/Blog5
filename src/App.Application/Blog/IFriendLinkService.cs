using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Framwork.Result;

namespace App.Application.Blog
{
    public interface IFriendLinkService : IAppService<FriendLink>
    {
        /// <summary>
        /// 新增/编辑友情链接
        /// </summary>
        /// <param name="link">友情链接信息</param>
        /// <returns></returns>
        Task<UnifyResult> Save(FriendLinkInputDto link);
    }
}