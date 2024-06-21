using System.Collections.Generic;

namespace App.Core.Pager
{
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数据条数
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        int Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        int Pages { get; set; }
    }
}