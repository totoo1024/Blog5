﻿using System;
using SqlSugar;

namespace App.Core.Entities.Logs
{
    /// <summary>
    /// 系统异常日志
    /// </summary>
    [Serializable]
    public class SysExceptionLog : Entity<string>
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 内部信息
        /// </summary>
        public string InnerException { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// 服务器
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// 客户端
        /// </summary>
        public string ClientHost { get; set; }

        /// <summary>
        /// 运行环境
        /// </summary>
        public string Runtime { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 浏览器代理
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateaAcountId { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatorTime { get; set; }
    }
}
