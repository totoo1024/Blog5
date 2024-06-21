using System;
using System.Collections.Generic;

namespace App.Application.Blog.Dtos
{
    public class CommentDto
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 评论人ID
        /// </summary>
        public string FromUId { get; set; }

        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string NikeName { get; set; }

        /// <summary>
        /// QQ头像40*40
        /// </summary>
        public string Image40 { get; set; }

        /// <summary>
        /// 归属地
        /// </summary>
        public string Adscription { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评论日期
        /// </summary>
        public DateTime CreatorTime { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        public List<ReplyDto> Reply { get; set; }

        /// <summary>
        /// 回复页数
        /// </summary>
        public int page { get; set; }
    }

    /// <summary>
    /// 回复
    /// </summary>
    public class ReplyDto
    {
        public string MsgId { get; set; }

        /// <summary>
        /// 评论人ID
        /// </summary>
        public string FromUId { get; set; }

        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string FromNikeName { get; set; }

        /// <summary>
        /// 回复人ID
        /// </summary>
        public string ToUId { get; set; }

        /// <summary>
        /// 回复人昵称
        /// </summary>
        public string ToNikeName { get; set; }


        /// <summary>
        /// QQ头像40*40
        /// </summary>
        public string FromImage { get; set; }

        /// <summary>
        /// QQ头像40*40
        /// </summary>
        public string ToImage { get; set; }

        /// <summary>
        /// 归属地
        /// </summary>
        public string Adscription { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否是博主
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// 回复日期
        /// </summary>
        public DateTime CreatorTime { get; set; }

    }
}