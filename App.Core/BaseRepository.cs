﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Core
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected SqlSugarClient Db { get; } = AppDbContext.Db;

        /// <summary>
        /// 插入数据（适用于id自动增长）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回主键ID</returns>
        public int InsertScalar(TEntity entity)
        {
            return Db.Insertable(entity).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 插入数据（适用于id自动增长）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> InsertScalarAsync(TEntity entity)
        {
            return Db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回是否插入成功</returns>
        public bool Insert(TEntity entity)
        {
            return Db.Insertable(entity).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入数据（异步）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回受影响行数</returns>
        public Task<int> InsertAsync(TEntity entity)
        {
            return Db.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public bool Insert(List<TEntity> entities)
        {
            return Db.Insertable(entities).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 批量添加（异步）
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>受影响的行数</returns>
        public Task<int> InsertAsync(List<TEntity> entities)
        {
            return Db.Insertable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        public bool Update(TEntity entity, bool isNoUpdateNull = false)
        {
            return Db.Updateable(entity).IgnoreColumns(isNoUpdateNull).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(TEntity entity, bool isNoUpdateNull = false)
        {
            return Db.Updateable(entity).IgnoreColumns(isNoUpdateNull).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 通过主键修改（更新实体部分字段）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        public bool Update(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Db.Updateable(entity).IgnoreColumns(ignoreColumns).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 通过主键修改（更新实体部分字段）异步
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Db.Updateable(entity).IgnoreColumns(ignoreColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 通过条件更新(不更新忽略字段)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        public bool Update(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Db.Updateable(entity).Where(expression).IgnoreColumns(ignoreColumns).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 通过条件更新(不更新忽略字段)异步
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Db.Updateable(entity).Where(expression).IgnoreColumns(ignoreColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 通过条件修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>是否成功</returns>
        public bool Update(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return Db.Updateable(entity).Where(expression).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 通过条件修改（异步）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>是否成功</returns>
        public Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return Db.Updateable(entity).Where(expression).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 修改（指定字段）
        /// </summary>
        /// <param name="expression">需要修改的字段</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns>是否修改成功</returns>
        public bool Update(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return Db.Updateable<TEntity>().UpdateColumns(expression).Where(condition).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 修改（指定字段）异步
        /// </summary>
        /// <param name="expression">需要修改的字段</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns>是否修改成功</returns>
        public Task<bool> UpdateAsync(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return Db.Updateable<TEntity>().UpdateColumns(expression).Where(condition).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(dynamic keyValue)
        {
            return Db.Deleteable<TEntity>(keyValue).ExecuteCommandHasChange();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> DeleteAsync(dynamic keyValue)
        {
            return Db.Deleteable<TEntity>(keyValue).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(TEntity entity)
        {
            return Db.Deleteable(entity).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> DeleteAsync(TEntity entity)
        {
            return Db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Deleteable<TEntity>().Where(expression).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Deleteable<TEntity>().Where(expression).ExecuteCommandHasChangeAsync();
        }

        // <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(List<dynamic> keys)
        {
            return Db.Deleteable<TEntity>(keys).ExecuteCommandHasChange();
        }

        // <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> DeleteAsync(List<dynamic> keys)
        {
            return Db.Deleteable<TEntity>(keys).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>返回实体</returns>
        public TEntity FindEntity(object keyValue)
        {
            return Db.Queryable<TEntity>().InSingle(keyValue);
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>返回实体</returns>
        public TEntity FindEntity(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Queryable<TEntity>().WhereIF(expression != null, expression).First();
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>返回实体</returns>
        public Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Queryable<TEntity>().WhereIF(expression != null, expression).FirstAsync();
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>集合</returns>
        public List<TEntity> Queryable()
        {
            return Db.Queryable<TEntity>().ToList();
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>集合</returns>
        public Task<List<TEntity>> QueryableAsync()
        {
            return Db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 检查信息总条数
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int QueryableCount(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                return Db.Queryable<TEntity>().Count();
            }
            return Db.Queryable<TEntity>().Count(expression);
        }

        /// <summary>
        /// 检查信息总条数
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public Task<int> QueryableCountAsync(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                return Db.Queryable<TEntity>().CountAsync();
            }
            return Db.Queryable<TEntity>().CountAsync(expression);
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>集合</returns>
        public List<TEntity> Queryable(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Queryable<TEntity>().WhereIF(expression != null, expression).ToList();
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> QueryableAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Queryable<TEntity>().WhereIF(expression != null, expression).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <returns>集合</returns>
        public List<TEntity> Queryable(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc)
        {
            return Db.Queryable<TEntity>().WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).ToList();
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> QueryableAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc)
        {
            return Db.Queryable<TEntity>().WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取指定条数集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <returns>集合</returns>
        public List<TEntity> Queryable(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top)
        {
            return Db.Queryable<TEntity>().WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).Take(top).ToList();
        }

        /// <summary>
        /// 根据条件获取指定条数集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> QueryableAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top)
        {
            return Db.Queryable<TEntity>().WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).Take(top).ToListAsync();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>集合|总条数</returns>
        public Tuple<List<TEntity>, int> QueryableByPage(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int pageIndex, int pageSize)
        {
            var total = 0;
            return Tuple.Create(Db.Queryable<TEntity>().WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).ToPageList(pageIndex, pageSize, ref total), total);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>集合|总条数</returns>
        public Tuple<List<TEntity>, int> QueryableByPage(Expression<Func<TEntity, bool>> expression, Dictionary<Expression<Func<TEntity, object>>, OrderByType> orderby, int pageIndex, int pageSize)
        {
            var total = 0;
            var query = Db.Queryable<TEntity>().WhereIF(expression != null, expression);
            foreach (var item in orderby)
            {
                query.OrderBy(item.Key, item.Value);
            }
            return Tuple.Create(query.ToPageList(pageIndex, pageSize, ref total), total);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="conditionals">查询条件</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        public Tuple<List<TEntity>, int> QueryableByPage(List<IConditionalModel> conditionals, string orderFileds, int pageIndex, int pageSize)
        {
            var total = 0;
            var query = Db.Queryable<TEntity>();
            if (conditionals != null)
                query.Where(conditionals);

            query.OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds);
            return Tuple.Create(query.ToPageList(pageIndex, pageSize, ref total), total);
        }
    }
}
