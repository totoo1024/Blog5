using System.Collections.Concurrent;
using System.Threading.Tasks;
using App.Core.Data;
using App.Core.Pager;
using SqlSugar;

namespace App.Core.Extensions
{
    public static class SqlSugarExtensions
    {
        private static readonly ConcurrentDictionary<string, SqlFilterItem> filters = new ConcurrentDictionary<string, SqlFilterItem>();

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">返回列表的类型</typeparam>
        /// <param name="queryable">查询对象</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        public static IPagedList<T> ToPage<T>(this ISugarQueryable<T> queryable, int pageIndex, int pageSize)
        {
            int totals = 0, pages = 0;
            var list = queryable.ToPageList(pageIndex, pageSize, ref totals, ref pages);
            var pager = new PagedList<T>()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Pages = pages,
                Total = totals
            };
            pager.AddRange(list);
            return pager;
        }

        /// <summary>
        /// 动态增加过滤器
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlSugarClient"></param>
        /// <returns></returns>
        public static ISugarQueryable<TEntity> QueryFilter<TEntity>(this ISqlSugarClient sqlSugarClient) where TEntity : class, new()
        {
            //每次先清理过滤器
            sqlSugarClient.QueryFilter.Clear();
            string key = typeof(TEntity).FullName;
            var filter = filters.GetOrAdd(key, x =>
              {
                  if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                  {
                      string name = nameof(ISoftDelete.DeleteMark);
                      var item = new SqlFilterItem
                      {
                          FilterValue = filter => new SqlFilterResult { Sql = $" {name}=0" },
                          IsJoinQuery = false
                      };
                      return item;
                  }
                  return null;
              });
            if (filter != null)
            {
                sqlSugarClient.QueryFilter.Add(filter);
            }
            return sqlSugarClient.Queryable<TEntity>();
        }
    }
}