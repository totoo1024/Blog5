using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Data;
using App.Framwork.Generate;
using App.Framwork.Net;
using App.Framwork.Result;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using App.Framwork.Encryption;
using App.Framwork.Generate.Geetest;

namespace AppUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string name = nameof(ISoftDelete.DeleteMark);
            Assert.AreEqual(name, "DeleteMark");
        }

        [TestMethod]
        public async Task GetIpAdressInfo()
        {
            var result = await Net.GetIpInfo("112.24.157.32");
            Assert.AreEqual(result, $"Ω≠À’ŒﬁŒ˝“∆∂Ø");
        }

        [TestMethod]
        public void GetResult()
        {
            UnifyResult<string> unify = ("abc", false);
            Assert.AreEqual(unify.StatusCode, ResultCode.ValidError);

            UnifyResult<string> unifyResult = "abc";
            Assert.AreEqual(unifyResult.Data, "abc");
            Assert.AreEqual(unifyResult.StatusCode, ResultCode.Success);
        }

        [TestMethod]
        public async Task GetResultAsync()
        {
            UnifyResult<string> unify = ("abc", false);
            Assert.AreEqual(unify.StatusCode, ResultCode.ValidError);

            UnifyResult<string> unifyResult = await GetString();
            Assert.AreEqual(unifyResult.Data, "abc");
            Assert.AreEqual(unifyResult.StatusCode, ResultCode.Success);
        }

        public async Task<UnifyResult<string>> GetString()
        {
            return await Task.Run(() => "abc");
        }

        [TestMethod]
        public void WokerId()
        {
            List<string> list = new List<string>();
            ConcurrentDictionary<int, string> keys = new ConcurrentDictionary<int, string>();
            //SnowflakeId snowflakeId = new SnowflakeId(1, 1);

            Parallel.For(1, 10000000, (x, y) =>
            {
                var id = SnowflakeId.NextId().ToString();
                //var id = IdWorker.NextStringId();
                keys[x] = id;
            });
            list = keys.Select(x => x.Value).ToList();
            var list1 = list.Where(x => string.IsNullOrWhiteSpace(x));
            int a = list.GroupBy(x => x).Where(c => c.Count() > 0).Count();
            Assert.AreEqual(a, list.Count);
        }

        [TestMethod]
        public void check()
        {
            string s1;
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes("123456"));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                s1 = sb.ToString();
            }

            string s2 = Md5Encrypt.Encrypt("123456");

            Assert.AreEqual(s1, s2);
        }
    }
}
