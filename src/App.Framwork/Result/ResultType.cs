using System.ComponentModel;

namespace App.Framwork.Result
{
    public enum ResultCode
    {
        /// <summary>
        /// 指定参数的数据不存在
        /// </summary>
        [Description("必要参数不可为空")]
        OperaParamNull,

        /// <summary>
        /// 输入信息验证失败
        /// </summary>
        [Description("输入信息验证失败")]
        ValidError,

        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 200,

        /// <summary>
        /// 未授权
        /// </summary>
        [Description("未经授权")]
        Unauthorized = 401,

        /// <summary>
        /// 无权访问
        /// </summary>
        [Description("无权访问")]
        Forbidden = 403,

        /// <summary>
        /// 系统异常
        /// </summary>
        [Description("服务端错误")]
        Error = 500
    }
}