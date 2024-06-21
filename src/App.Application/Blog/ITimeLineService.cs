using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Framwork.Result;

namespace App.Application.Blog
{
    public interface ITimeLineService : IAppService<TimeLine>
    {
        /// <summary>
        /// 添加/编辑时间轴
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult> Save(TimeLineInputDto dto);
    }
}