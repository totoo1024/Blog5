using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Application;
using App.Application.Blog;
using App.Application.Blog.Dtos;
using App.Application.User;
using App.Core.Entities.Blog;
using App.Core.Entities.User;
using App.Framwork.Result;
using App.Hosting.Extensions;
using App.Hosting.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace App.Hosting.Controllers
{
    public class HomeController : WebController
    {
        private readonly IBannerService _bannerService;
        private readonly IQQUserService _qQUserService;
        private readonly ILeavemsgService _leavemsgService;
        private readonly IArticleService _articleService;
        private readonly INoticeService _noticeService;
        private readonly ITagsService _tagsService;
        private readonly IFriendLinkService _friendLinkService;
        private readonly ITimeLineService _timeLineService;

        public HomeController(IBannerService bannerService,
            IQQUserService qQUserService,
            ILeavemsgService leavemsgService,
            IArticleService articleService,
            INoticeService noticeService,
            ITagsService tagsService,
            IFriendLinkService friendLinkService,
            ITimeLineService timeLineService)
        {
            _bannerService = bannerService;
            _qQUserService = qQUserService;
            _leavemsgService = leavemsgService;
            _articleService = articleService;
            _noticeService = noticeService;
            _tagsService = tagsService;
            _friendLinkService = friendLinkService;
            _timeLineService = timeLineService;
        }

        #region 视图
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(string code, string state)
        {
            List<BannerInfo> list = await _bannerService.GetListCacheAsync(null, o => o.SortCode, false);
            return View(list);
        }

        /// <summary>
        /// 作品展示
        /// </summary>
        /// <returns></returns>
        public IActionResult Works()
        {
            return View();
        }

        /// <summary>
        /// 时光轴
        /// </summary>
        /// <returns></returns>
        public IActionResult TimeLine()
        {
            return View();
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <returns></returns>
        public IActionResult About()
        {
            return View();
        }
        #endregion


        public async Task<IActionResult> Init()
        {
            var hot = await _articleService.Hot(6);
            var notice = await _noticeService.GetListCacheAsync(null, o => o.SortCode, false);
            var tags = await _tagsService.TagsCount();
            var link = await _friendLinkService.GetListCacheAsync(null, o => o.SortCode, false);
            return Json(new { hot, notice, tags, link });
        }

        /// <summary>
        /// 时间轴
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        public IActionResult Line(PageInputDto pageInput)
        {
            var result = _timeLineService.GetListByPage(null, x => x.PublishDate, true, pageInput.Page, pageInput.Limit);
            var data = result;
            IEnumerable<int> years = data.Select(s => s.PublishDate.Year).Distinct().OrderByDescending(o => o);
            List<TimeLineDto> times = new List<TimeLineDto>();
            foreach (int item in years)
            {
                TimeLineDto dto = new TimeLineDto();
                dto.Year = item;
                var list = data.Where(c => c.PublishDate.Year == item);
                IEnumerable<int> months = list.Select(s => s.PublishDate.Month).Distinct().OrderBy(o => o);
                Dictionary<string, IEnumerable<LineItem>> pairs = new Dictionary<string, IEnumerable<LineItem>>();
                foreach (int m in months)
                {
                    pairs[m.ToString("D2")] = list.Where(c => c.PublishDate.Month == m).Select(s => new LineItem { Content = s.Content, Time = s.PublishDate.ToString("MM月dd日 HH:mm") }).OrderBy(o => o.Time);
                }
                dto.Items = pairs;
                times.Add(dto);
            }
            PageOutputDto<List<TimeLineDto>> d = new PageOutputDto<List<TimeLineDto>>();
            d.code = 0;
            d.count = data.Pages;
            d.data = times;
            return Json(d);
        }

        /// <summary>
        /// 留言列表
        /// </summary>
        /// <returns></returns>
        public IActionResult Msg(LeavemsgQueryInputDto dto)
        {
            return Json(_leavemsgService.MsgList(dto), "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 回复分页
        /// </summary>
        /// <param name="dto">查询model</param>
        /// <returns></returns>
        public IActionResult ReplyPage(LeavemsgQueryInputDto dto)
        {
            return Json(_leavemsgService.ReplyList(dto), "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Reply(ReplyInputDto dto)
        {
            UnifyResult result = new UnifyResult();
            QQUserinfo user = HttpContext.GetSession<QQUserinfo>("QQ_User");
            if (user == null)
            {
                result.StatusCode = ResultCode.Unauthorized;
                result.Message = "未登录";
            }
            else
            {
                dto.UserId = user.Id;
                result = await _leavemsgService.Reply(dto);
            }
            return Json(result);
        }

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="dto">留言内容</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Comment(CommentInputDto dto)
        {
            UnifyResult result = new UnifyResult();
            QQUserinfo user = HttpContext.GetSession<QQUserinfo>("QQ_User");
            if (user == null)
            {
                result.StatusCode = ResultCode.Unauthorized;
                result.Message = "未登录";
            }
            else
            {
                dto.UserId = user.Id;
                result = await _leavemsgService.Comment(dto);
            }
            return Json(result);
        }

        /// <summary>
        /// 友情链接
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Link()
        {
            var link = await _friendLinkService.GetListCacheAsync(null, o => o.SortCode, false);
            return Json(link);
        }

        /// <summary>
        /// QQ授权登录
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Login(string code, string state)
        {
            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(state))
            {
                string referer = HttpContext.Request.Headers[HeaderNames.Referer].FirstOrDefault();
                if (string.IsNullOrWhiteSpace(referer))
                {
                    referer = "/home/index";
                }
                return Json(_qQUserService.Authorize(referer));
            }
            else
            {
                var user = await _qQUserService.Login(code, state);
                string url = HttpContext.Session.GetString("lib" + state);
                if (string.IsNullOrWhiteSpace(url))
                {
                    url = "/home/index";
                }
                if (user != null)
                {
                    string json = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("QQ_User", json);
                }
                HttpContext.Session.Remove("lib" + state);
                return Redirect(url);
            }

        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public new IActionResult SignOut()
        {
            HttpContext.Session.Remove("QQ_User");
            return Json(new UnifyResult("已安全退出"));
        }
    }
}