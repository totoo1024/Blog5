using System;
using App.Framwork.DependencyInjection;
using SqlSugar;

namespace App.Core.Config
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 连接超时时间（单位：秒）
        /// </summary>
        public int CommandTimeOut { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// 是否自动关闭数据库连接
        /// </summary>
        public bool IsAutoClose { get; set; }
    }
}