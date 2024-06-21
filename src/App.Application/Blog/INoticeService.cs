using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Framwork.Result;

namespace App.Application.Blog
{
    public interface INoticeService : IAppService<Noticeinfo>
    {
        /// <summary>
        /// 添加/编辑通知
        /// </summary>
        /// <param name="dto">通知信息</param>
        /// <returns></returns>
        Task<UnifyResult> Save(NoticeInputDto dto);
    }
}