using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using App.Framwork.DependencyInjection;
using App.Framwork.Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace App.Framwork.Generate.Geetest
{
    /// <summary>
    /// 极验验证SDK
    /// </summary>

    public class GeetestValidate : IGenerateValidate, IScopedDependency
    {
        private readonly GeetestConfig _geetestConfig;

        #region 常量

        private const string JSON_FORMAT = "1";

        public const string VERSION = "csharp-aspnetcoremvc:3.1.0";

        /// <summary>
        /// 本地随机生成32位字符串
        /// </summary>
        private const string Characters = "0123456789abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 极验验证API服务状态Session Key
        /// </summary>
        private const string GeetestServerStatusSessionKey = "gt_server_status";

        /// <summary>
        /// 当前用户
        /// </summary>
        private const string CurrentUniqueUserKey = "gt_unique_user";


        /// <summary>
        /// 极验二次验证表单传参字段 chllenge
        /// </summary>
        private const string Challenge = "geetest_challenge";

        /// <summary>
        /// 极验二次验证表单传参字段 validate
        /// </summary>
        private const string Verify = "geetest_validate";

        /// <summary>
        /// 极验二次验证表单传参字段 seccode
        /// </summary>
        private const string SECCODE = "geetest_seccode";

        #endregion
        public GeetestValidate(IOptionsMonitor<GeetestConfig> geetestConfig)
        {
            _geetestConfig = geetestConfig.CurrentValue;
        }

        /// <summary>
        /// 必传参数
        /// digestmod 此版本sdk可支持md5、sha256、hmac-sha256，md5之外的算法需特殊配置的账号，联系极验客服
        /// 自定义参数,可选择添加
        /// user_id user_id作为客户端用户的唯一标识，确定用户的唯一性；作用于提供进阶数据分析服务，可在register和validate接口传入，不传入也不影响验证服务的使用；若担心用户信息风险，可作预处理(如哈希处理)再提供到极验
        /// client_type 客户端类型，web：电脑上的浏览器；h5：手机上的浏览器，包括移动应用内完全内置的web_view；native：通过原生sdk植入app应用的方式；unknown：未知
        /// ip_address 客户端请求sdk服务器的ip地址
        /// </summary>
        public string Generate()
        {
            var paramDict = new Dictionary<string, string>();
            if (!paramDict.ContainsKey("digestmod"))
            {
                paramDict["digestmod"] = "md5";
            }
            string userId = Guid.NewGuid().ToString();
            paramDict["user_id"] = userId;
            paramDict.Add("gt", _geetestConfig.AppId);
            paramDict.Add("json_format", JSON_FORMAT);
            paramDict.Add("sdk", VERSION);
            string result = string.Empty;
            try
            {
                string body = HttpGet(_geetestConfig.RegisterUrl, paramDict);
                var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);

                result = dic["challenge"];
            }
            catch (Exception e)
            {
            }
            string encode = string.Empty, success = "0";
            //为空或者值为0代表失败
            if (string.IsNullOrWhiteSpace(result) || "0".Equals(result))
            {
                encode = GetRandomNumber();
            }
            else
            {
                switch (paramDict["digestmod"])
                {
                    case "md5":
                        encode = Md5Encrypt.Encrypt(result + _geetestConfig.AppKey);
                        break;
                    case "sha256":
                        encode = Sha256Encrypt.Encrypt(result + _geetestConfig.AppKey);
                        break;
                    case "hmac-sha256":
                        encode = Sha256Encrypt.HmacEncrypt(result, _geetestConfig.AppKey);
                        break;
                    default:
                        encode = Md5Encrypt.Encrypt(result + _geetestConfig.AppKey);
                        break;
                }
                success = "1";
            }
            var json = new { success = success, gt = _geetestConfig.AppId, challenge = encode, new_captcha = true };

            // 将结果状态写到session中，此处register接口存入session，后续validate接口会取出使用
            if (success == "1")
            {
                Storage.Current.HttpContext.Session.SetString(GeetestServerStatusSessionKey, success);
                Storage.Current.HttpContext.Session.SetString(CurrentUniqueUserKey, paramDict["user_id"]);
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            var context = Storage.Current.HttpContext;
            string status = context.Session.GetString(GeetestServerStatusSessionKey);
            string userId = context.Session.GetString(CurrentUniqueUserKey);
            string result = "fail", msg = string.Empty;
            if (status is null)
            {
                return false;
            }
            if (status == "1")
            {
                string challenge = context.Request.Form[Challenge];
                string validate = context.Request.Form[Verify];
                string seccode = context.Request.Form[SECCODE];
                if (string.IsNullOrWhiteSpace(challenge) || string.IsNullOrWhiteSpace(validate) || string.IsNullOrWhiteSpace(seccode))
                {
                    return false;
                }
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("seccode", seccode);
                dic.Add("json_format", JSON_FORMAT);
                dic.Add("challenge", challenge);
                dic.Add("sdk", VERSION);
                dic.Add("captchaid", _geetestConfig.AppId);
                dic["user_id"] = userId;
                string code = string.Empty;
                try
                {
                    string body = HttpPost(_geetestConfig.ValidateUrl, dic);
                    Dictionary<string, string> resDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
                    code = resDict["seccode"]; ;
                }
                catch (Exception e)
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(code) || "false".Equals(code))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送GET请求，获取服务器返回结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramDict"></param>
        /// <returns></returns>
        private string HttpGet(string url, IDictionary<string, string> paramDict)
        {
            Stream resStream = null;
            try
            {
                StringBuilder paramStr = new StringBuilder();
                foreach (KeyValuePair<string, string> item in paramDict)
                {
                    if (!(string.IsNullOrWhiteSpace(item.Key) || string.IsNullOrWhiteSpace(item.Value)))
                    {
                        paramStr.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(item.Key, Encoding.UTF8), HttpUtility.UrlEncode(item.Value, Encoding.UTF8));
                    }

                }
                url = url + "?" + paramStr.ToString().Substring(1);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.ReadWriteTimeout = 5000;
                req.Timeout = 5000;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                resStream = res.GetResponseStream();
                StreamReader reader = new StreamReader(resStream, Encoding.GetEncoding("utf-8"));
                return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (resStream != null)
                {
                    resStream.Close();
                }
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramDict"></param>
        /// <returns></returns>
        private string HttpPost(string url, IDictionary<string, string> paramDict)
        {
            Stream reqStream = null;
            Stream resStream = null;
            try
            {
                StringBuilder paramStr = new StringBuilder();
                foreach (KeyValuePair<string, string> item in paramDict)
                {
                    if (!(string.IsNullOrWhiteSpace(item.Key) || string.IsNullOrWhiteSpace(item.Value)))
                    {
                        paramStr.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(item.Key, Encoding.UTF8), HttpUtility.UrlEncode(item.Value, Encoding.UTF8));
                    }

                }
                byte[] bytes = Encoding.UTF8.GetBytes(paramStr.ToString().Substring(1));
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ReadWriteTimeout = 5000;
                req.Timeout = 5000;
                reqStream = req.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                resStream = res.GetResponseStream();
                StreamReader reader = new StreamReader(resStream, Encoding.GetEncoding("utf-8"));
                return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reqStream != null)
                {
                    reqStream.Close();
                }
                if (resStream != null)
                {
                    resStream.Close();
                }
            }
        }

        /// <summary>
        /// 获取32位随机字符
        /// </summary>
        /// <returns></returns>
        private string GetRandomNumber()
        {
            StringBuilder randomStr = new StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < 32; i++)
            {
                randomStr.Append(Characters[rd.Next(Characters.Length)]);
            }
            return randomStr.ToString();
        }
    }
}