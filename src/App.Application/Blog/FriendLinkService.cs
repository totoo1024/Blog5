using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Core.Repository;
using App.Framwork.Generate;
using App.Framwork.Result;
using Mapster;

namespace App.Application.Blog
{
    public class FriendLinkService : AppService<FriendLink>, IFriendLinkService
    {
        public FriendLinkService(IAppRepository<FriendLink> repository) : base(repository)
        {
        }

        /// <summary>
        /// 新增/编辑友情链接
        /// </summary>
        /// <param name="link">友情链接信息</param>
        /// <returns></returns>
        public async Task<UnifyResult> Save(FriendLinkInputDto link)
        {
            var fl = link.Adapt<FriendLink>();
            if (string.IsNullOrWhiteSpace(link.Id))
            {
                fl.Id = SnowflakeId.NextStringId();
                return await InsertRemoveCacheAsync(fl);
            }
            else
            {
                return await UpdateRemoveCacheAsync(fl, f => new { f.DeleteMark, f.CreatorTime });
            }
        }
    }
}