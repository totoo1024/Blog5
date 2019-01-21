﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.IServices;
using App.Entities;
using App.Entities.Dtos;
using System.ComponentModel;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using App.Common.Utils;
using AppSoft.Filter;

namespace AppSoft.Areas.BlogManage.Controllers
{
    [Area("BlogManage")]
    public class ArticleController : BaseControler
    {
        private readonly IArticleInfoLogic _articleInfoLogic;
        private readonly IArticleCategoryLogic _articleCategoryLogic;
        private readonly IArticleTagsLogic _articleTagsLogic;
        //private readonly IArticleTagsLogic _articleTagsLogic;
        public ArticleController(IArticleInfoLogic articleInfoLogic, IArticleCategoryLogic articleCategoryLogic, IArticleTagsLogic articleTagsLogic)
        {
            _articleInfoLogic = articleInfoLogic;
            _articleCategoryLogic = articleCategoryLogic;
            _articleTagsLogic = articleTagsLogic;
        }

        [HttpPost]
        [Description("文章列表")]
        public IActionResult Index(QueryDto query)
        {
            return Json(_articleInfoLogic.QueryableByPage(query), "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("新增/编辑文章")]
        public IActionResult Form(ArticleInfo article)
        {
            return Json(_articleInfoLogic.Save(article), "yyyy-MM-dd HH:mm:ss");
        }

        [AllowAccessFilter]
        [Description("查看文章详情")]
        public IActionResult Detail(string key)
        {
            var article = _articleInfoLogic.FindEntity(c => c.ArticleId == key);
            article.Categories = _articleCategoryLogic.Queryable(c => c.ArticleId == key).Select(k => k.CategoryId).ToList();
            article.Tags = _articleTagsLogic.Queryable(c => c.ArticleId == key).Select(k => k.TagsId).ToList();
            return Json(article, "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        [Description("设置是否置顶")]
        public IActionResult Top(string id, bool status)
        {
            return Json(_articleInfoLogic.Update(a => new ArticleInfo { IsTop = status }, c => c.ArticleId == id));
        }

        [HttpPost]
        [Description("设置是否显示")]
        public IActionResult Show(string id, bool status)
        {
            return Json(_articleInfoLogic.Update(a => new ArticleInfo { Visible = status }, c => c.ArticleId == id));
        }

        [HttpPost]
        [AllowAccessFilter]
        [Description("文章上传缩略图")]
        public IActionResult Thumbnail([FromServices]IHostingEnvironment env)
        {
            OperateResult<string> result = new OperateResult<string>();
            try
            {
                //文件保存目录路径
                string savePath = "/Uploads/Thumbnail/";
                string dirPath = env.WebRootPath + savePath;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                var file = Request.Form.Files[0];

                string fileName = Path.GetFileName(file.FileName);
                string fileExtension = Path.GetExtension(fileName);
                if (file.Length / 1024 > 500)
                {
                    result.Message = "上传文件大小超过限制";
                }
                else if (".jpg.png.gif".Contains(fileExtension))
                {
                    string name = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;
                    string imgPath = dirPath + name;
                    using (FileStream fs = System.IO.File.Create(imgPath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    result.Data = savePath + name;
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    result.Message = "上传图片格式必须为jpg|png|gif";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }

        [HttpPost]
        [AllowAccessFilter]
        [Description("富文本编辑器上传资源")]
        public IActionResult upload([FromServices]IHostingEnvironment env)
        {
            try
            {

                //最大文件大小
                int maxSize = 2048;
                //文件保存目录路径
                string savePath = "/Uploads/attached/";
                //文件保存目录URL
                string saveUrl = "/Uploads/attached/";
                //定义允许上传的文件扩展名
                Hashtable extTable = new Hashtable();
                extTable.Add("image", "gif,jpg,jpeg,png,bmp");
                extTable.Add("flash", "swf,flv");
                extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
                extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");
                var imgFile = Request.Form.Files["imgFile"];
                if (imgFile == null)
                {
                    return Json(new { error = 1, message = "请选择文件" });
                }
                string dirPath = env.WebRootPath + savePath;
                if (!Directory.Exists(dirPath))
                {
                    return Json(new { error = 1, message = "上传目录不存在" });
                }
                string dirName = HttpContext.Request.Query["dir"].FirstOrDefault();

                if (string.IsNullOrEmpty(dirName))
                {
                    dirName = "image";
                }
                if (!extTable.ContainsKey(dirName))
                {
                    return Json(new { error = 1, message = "目录名不正确" });
                }
                string fileName = imgFile.FileName;
                string fileExt = Path.GetExtension(fileName).ToLower();

                if (imgFile.Length / 1024 > maxSize)
                {
                    return Json(new { error = 1, message = "上传文件大小超过限制" });
                }

                if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(((string)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    return Json(new { error = 1, message = "上传文件扩展名是不允许的扩展名。\n只允许" + ((string)extTable[dirName]) + "格式" });
                }
                //创建文件夹
                dirPath += dirName + "/";
                saveUrl += dirName + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                dirPath += ymd + "/";
                saveUrl += ymd + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string newFileName = SnowflakeUtil.NextStringId() + fileExt;
                string filePath = dirPath + newFileName;
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    imgFile.CopyTo(fs);
                    fs.Flush();
                }

                string fileUrl = saveUrl + newFileName;
                return Json(new { error = 0, url = fileUrl });
            }
            catch (Exception ex)
            {
                return Json(new { error = 1, message = ex.Message });
            }
        }
        [AllowAccessFilter]
        [Description("富文本编辑器上传文件配置")]
        public IActionResult config([FromServices]IHostingEnvironment env, string dir, string path, string order)
        {
            string currentPath = "";
            string currentUrl = "";
            string currentDirPath = "";
            string moveupDirPath = "";
            //图片扩展名
            string fileTypes = "gif,jpg,jpeg,png,bmp";
            string dirPath = env.WebRootPath + "/Uploads/attached/";
            string rootUrl = "/Uploads/attached/";
            if (!string.IsNullOrEmpty(dir))
            {
                if (Array.IndexOf("image,flash,media,file".Split(','), dir) == -1)
                {
                    return Content("Invalid Directory name.");
                }
                dirPath += dir + "/";
                rootUrl += dir + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            path = string.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                return Content("无权访问");
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                return Content("参数无效");
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                return Content("文件夹不存在.");
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dirInfo.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dirInfo.Name;
                hash["datetime"] = dirInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            return Json(result);
        }


        #region 对象比较
        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
        #endregion
    }
}