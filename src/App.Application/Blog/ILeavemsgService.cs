using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Framwork.Result;

namespace App.Application.Blog
{
    public interface ILeavemsgService : IAppService<LeavemsgInfo>
    {
        /// <summary>
        /// 留言评论列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PageOutputDto<List<CommentDto>> MsgList(LeavemsgQueryInputDto dto);

        /// <summary>
        /// 回复分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PageOutputDto<List<ReplyDto>> ReplyList(LeavemsgQueryInputDto dto);

        /// <summary>
        /// 留言评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult> Comment(CommentInputDto dto);

        /// <summary>
        /// 回复留言/评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult> Reply(ReplyInputDto dto);
    }
}