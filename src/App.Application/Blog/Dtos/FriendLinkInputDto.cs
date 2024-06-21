using System;

namespace App.Application.Blog.Dtos
{
    [Serializable]
    public class FriendLinkInputDto : EntityDto<string>
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 网站链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 网站logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public sbyte SortCode { get; set; }
    }
}