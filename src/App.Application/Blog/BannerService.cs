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
    public class BannerService : AppService<BannerInfo>, IBannerService
    {
        public BannerService(IAppRepository<BannerInfo> repository) : base(repository)
        {
        }

        /// <summary>
        /// 添加、编辑banner
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UnifyResult> Save(BannerInputDto dto)
        {
            var banner = dto.Adapt<BannerInfo>();
            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                banner.Id = SnowflakeId.NextStringId();
                return await InsertAsync(banner);
            }

            return await UpdateAsync(banner, x => new { x.CreatorTime, x.DeleteMark });
        }
    }
}