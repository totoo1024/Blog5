using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Core.Entities.User;
using App.Core.Repository;
using App.Framwork.Generate;
using App.Framwork.Net;
using App.Framwork.Result;
using SqlSugar;

namespace App.Application.Blog
{
    public class LeavemsgService : AppService<LeavemsgInfo>, ILeavemsgService
    {
        public LeavemsgService(IAppRepository<LeavemsgInfo> repository) : base(repository)
        {
        }

        /// <summary>
        /// 留言评论列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PageOutputDto<List<CommentDto>> MsgList(LeavemsgQueryInputDto dto)
        {
            int total = 0;
            List<CommentDto> comments = Repository.Db.Queryable<LeavemsgInfo, QQUserinfo>((msg, user1) => msg.FromUId == user1.Id)
                .Where(msg => msg.DeleteMark == false && SqlFunc.IsNullOrEmpty(msg.RootId))
                .WhereIF(SqlFunc.IsNullOrEmpty(dto.Aid), msg => SqlFunc.IsNullOrEmpty(msg.ArticleId))
                .WhereIF(!SqlFunc.IsNullOrEmpty(dto.Aid), msg => msg.ArticleId == dto.Aid)
                .OrderBy(msg => msg.CreatorTime, OrderByType.Desc)
                .Select<CommentDto>().Mapper((it, cache) =>
                {
                    int count = 0;
                    it.Reply = Repository.Db.Queryable<LeavemsgInfo, QQUserinfo, QQUserinfo>((m, u1, u2) =>
                            new object[] { JoinType.Left, m.FromUId == u1.Id, JoinType.Left, m.ToUId == u2.Id }
                        ).Where(m => m.RootId == it.MsgId)
                        .OrderBy(m => m.ParentId, OrderByType.Asc)
                        .OrderBy(m => m.Id, OrderByType.Asc)
                        .OrderBy(m => m.CreatorTime, OrderByType.Asc)
                        .Select((m, u1, u2) => new ReplyDto()
                        {
                            MsgId = m.Id,
                            FromUId = m.FromUId,
                            FromNikeName = u1.NikeName,
                            FromImage = u1.Image40,
                            ToUId = m.ToUId,
                            ToNikeName = u2.NikeName,
                            ToImage = u2.Image40,
                            Content = m.Content,
                            Adscription = m.Adscription,
                            IsMaster = u1.IsMaster,
                            CreatorTime = m.CreatorTime
                        })
                        .ToPageList(1, dto.ChildSize, ref count);
                    it.page = (int)Math.Ceiling(count * 1d / dto.ChildSize);
                })
                .ToPageList(dto.Page, dto.Limit, ref total);
            return (comments, (int)Math.Ceiling(total * 1d / dto.Limit));
        }

        /// <summary>
        /// 回复分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PageOutputDto<List<ReplyDto>> ReplyList(LeavemsgQueryInputDto dto)
        {
            int count = 0;
            List<ReplyDto> reply = Repository.Db.Queryable<LeavemsgInfo, QQUserinfo, QQUserinfo>((m, u1, u2) =>
                    new object[] { JoinType.Left, m.FromUId == u1.Id, JoinType.Left, m.ToUId == u2.Id }
                ).Where(m => m.RootId == dto.RootId && m.DeleteMark == false)
                .WhereIF(SqlFunc.IsNullOrEmpty(dto.Aid), m => SqlFunc.IsNullOrEmpty(m.ArticleId))
                .WhereIF(!SqlFunc.IsNullOrEmpty(dto.Aid), m => m.ArticleId == dto.Aid)
                .OrderBy(m => m.ParentId, OrderByType.Asc)
                .OrderBy(m => m.Id, OrderByType.Asc)
                .OrderBy(m => m.CreatorTime, OrderByType.Asc)
                .Select((m, u1, u2) => new ReplyDto()
                {
                    MsgId = m.Id,
                    FromUId = m.FromUId,
                    FromNikeName = u1.NikeName,
                    FromImage = u1.Image40,
                    ToUId = m.ToUId,
                    ToNikeName = u2.NikeName,
                    ToImage = u2.Image40,
                    Content = m.Content,
                    Adscription = m.Adscription,
                    IsMaster = u1.IsMaster,
                    CreatorTime = m.CreatorTime
                })
                .ToPageList(dto.Page, dto.Limit, ref count);
            int page = (int)Math.Ceiling(count * 1d / dto.Limit);

            return (reply, page);
        }

        /// <summary>
        /// 留言评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UnifyResult> Comment(CommentInputDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.ArticleId))
            {
                if (await Repository.AnyAsync(c => c.ArticleId == dto.ArticleId))
                {
                    return "评论的文章不存在";
                }
            }
            var ipInfo = Net.GetIpAddressInfo();
            LeavemsgInfo leavemsg = new LeavemsgInfo
            {
                FromUId = dto.UserId,
                Content = dto.Content,
                IP = Net.GetClientIp(),
                Adscription = await ipInfo,
                Id = SnowflakeId.NextStringId()
            };
            leavemsg.ParentId = leavemsg.Id;
            leavemsg.ArticleId = dto.ArticleId;
            await InsertAsync(leavemsg);
            return true;
        }

        /// <summary>
        /// 回复留言/评论
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UnifyResult> Reply(ReplyInputDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ParentId) || dto.RootId == dto.ParentId)
            {
                if (!await AnyAsync(c => c.Id == dto.RootId))
                {
                    return "请选择回复内容";
                }
            }
            else
            {
                if (!await Repository.AnyAsync(c => c.Id == dto.RootId || c.Id == dto.ParentId))
                {
                    return "请选择回复内容";
                }
            }
            if (!string.IsNullOrWhiteSpace(dto.ArticleId))
            {
                if (!await AnyAsync(c => c.ArticleId == dto.ArticleId))
                {
                    return "评论的文章不存在";
                }
            }
            var ipInfo = Net.GetIpAddressInfo();
            LeavemsgInfo leavemsg = new LeavemsgInfo
            {
                FromUId = dto.UserId,
                ToUId = dto.FromId,
                RootId = dto.RootId,
                ParentId = dto.ParentId ?? dto.RootId,
                Content = dto.Content,
                IP = Net.GetClientIp(),
                Id = SnowflakeId.NextStringId(),
                Adscription = await ipInfo,
                ArticleId = dto.ArticleId
            };
            await InsertAsync(leavemsg);
            return true;
        }
    }
}