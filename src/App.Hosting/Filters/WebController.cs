using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Hosting
{
    public abstract class WebController : Controller
    {
        /// <summary>
        /// 创建 JsonResult 对象，该对象使用指定 JSON 请求行为将指定对象序列化为 JavaScript 对象表示法 (JSON) 格式。
        /// </summary>
        /// <param name="data">要序列化的 JavaScript 对象图</param>
        /// <param name="format">时间格式</param>
        /// <returns></returns>
        public JsonResult Json(object data, string format)
        {
            var setting = new JsonSerializerSettings();
            setting.DateFormatString = format;
            return base.Json(data, setting);
        }
    }
}