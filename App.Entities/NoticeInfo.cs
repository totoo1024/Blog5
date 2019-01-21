﻿using SqlSugar;
using System;

namespace App.Entities
{
    /// <summary>
    /// 首页通知轮播展示
    /// </summary>
    [Serializable]
    public class Noticeinfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string NoticeId { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 跳转方式
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public sbyte? SortCode { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteMark { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatorTime { get; set; } = DateTime.Now;
    }
}
