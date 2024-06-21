using System;

namespace App.Framwork.Log
{
    public interface ILogger
    {
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        void Info(string message, Exception exception = null);

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        void Warn(string message, Exception exception = null);

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        void Debug(string message, Exception exception = null);

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        void Error(string message, Exception exception = null);
    }
}