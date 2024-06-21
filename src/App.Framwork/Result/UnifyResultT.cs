using System;
using App.Framwork.ValueType;

namespace App.Framwork.Result
{
    /// <summary>
    /// 统一响应结果泛型
    /// </summary>
    /// <typeparam name="T">返回结果的数据类型</typeparam>
    public class UnifyResult<T> : UnifyResult
    {
        public UnifyResult()
        {

        }

        public UnifyResult(T data, ResultCode code = ResultCode.Success) : base(code)
        {
            Data = data;
        }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 正常消息并返回数据（如果返回结果为string类型可替换为UnifiedResult<dynamic>）
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator UnifyResult<T>(T data)
        {
            return new UnifyResult<T>
            {
                Data = data,
                StatusCode = ResultCode.Success,
                Message = ResultCode.Success.Description()
            };
        }

        /// <summary>
        /// 错误消息使用
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator UnifyResult<T>((string, bool) message)
        {
            return new UnifyResult<T>
            {
                Message = message.Item1,
                StatusCode = message.Item2 ? ResultCode.Success : ResultCode.ValidError
            };
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="exception"></param>
        public static implicit operator UnifyResult<T>(Exception exception)
        {
            return new UnifyResult<T>
            {
                Message = exception.Message,
                StatusCode = ResultCode.Error
            };
        }
    }
}