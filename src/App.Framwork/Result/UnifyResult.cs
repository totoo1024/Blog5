using System;
using App.Framwork.ValueType;
namespace App.Framwork.Result
{
    /// <summary>
    /// 统一响应结果
    /// </summary>
    public class UnifyResult
    {
        public UnifyResult()
        {
            StatusCode = ResultCode.Success;
            Message = StatusCode.Description();
        }

        public UnifyResult(ResultCode code, object message = null)
        {
            StatusCode = code;
            Message = message ?? code.Description();
        }

        public UnifyResult(object message, ResultCode code = ResultCode.Success)
        {
            Message = message;
            StatusCode = code;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public object Message { get; set; }

        /// <summary>
        /// 状态值
        /// </summary>
        public ResultCode StatusCode { get; set; }

        /// <summary>
        /// 返回错误消息
        /// </summary>
        /// <param name="message"></param>

        public static implicit operator UnifyResult(string message)
        {
            return new UnifyResult { Message = message, StatusCode = ResultCode.ValidError };
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <param name="success"></param>
        public static implicit operator UnifyResult(bool success)
        {
            return new UnifyResult
            {
                StatusCode = ResultCode.Success
                ,
                Message = success ? ResultCode.Success.Description() : ResultCode.ValidError.Description()
            };
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="exception"></param>
        public static implicit operator UnifyResult(Exception exception)
        {
            return new UnifyResult
            {
                StatusCode = ResultCode.Error
                ,
                Message = exception.Message
            };
        }

        /// <summary>
        /// 枚举消息
        /// </summary>
        /// <param name="code"></param>
        public static implicit operator UnifyResult(ResultCode code)
        {
            return new UnifyResult
            {
                StatusCode = code
                ,
                Message = code.Description()
            };
        }
    }
}