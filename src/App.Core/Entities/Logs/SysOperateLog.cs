﻿using System;

namespace App.Core.Entities.Logs
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    [Serializable]
    public class SysOperateLog : Entity<string>
    {
        /// <summary>
        /// 客户端
        /// </summary>
        public string ClientHost { get; set; }

        /// <summary>
        /// 服务端IP地址
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// 请求内容大小
        /// </summary>
        public long RequestContentLength { get; set; }

        /// <summary>
        /// 请求类型 get or post
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// 当前请求Url信息
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 上次请求Url信息
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 浏览器代理信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Action执行时间(秒)
        /// </summary>
        public double ActionExecutionTime { get; set; }

        /// <summary>
        /// 页面展示时间(秒)
        /// </summary>
        public double ResultExecutionTime { get; set; }

        /// <summary>
        /// 响应状态
        /// </summary>
        public string ResponseStatus { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public string CreateAccountId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatorTime { get; set; }
    }
}
