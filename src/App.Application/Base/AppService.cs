using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Pager;
using App.Core.Repository;
using App.Framwork.Result;
using SqlSugar;
namespace App.Application
{
    public class AppService<TEntity> : IAppService<TEntity> where TEntity : class, new()
    {
        protected readonly IAppRepository<TEntity> Repository;

        public ISugarQueryable<TEntity> AsQueryable() => Repository.SugarQueryable;
        public AppService(IAppRepository<TEntity> repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// 插入数据（适用于id自动增长）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回主键ID</returns>
        public virtual UnifyResult InsertScalar(TEntity entity)
        {
            return Repository.InsertScalar(entity) > 0;
        }

        /// <summary>
        /// 插入数据（适用于id自动增长）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回主键ID</returns>
        public virtual UnifyResult InsertScalarRemoveCache(TEntity entity)
        {
            return Repository.InsertScalarRemoveCache(entity) > 0;
        }

        /// <summary>
        /// 插入数据（适用于id自动增长）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> InsertScalarAsync(TEntity entity)
        {
            return await Repository.InsertScalarAsync(entity) > 0;
        }

        /// <summary>
        /// 插入数据（适用于id自动增长）并删除缓存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> InsertScalarRemoveCacheAsync(TEntity entity)
        {
            return await Repository.InsertScalarRemoveCacheAsync(entity) > 0;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回是否插入成功</returns>
        public virtual UnifyResult Insert(TEntity entity)
        {
            return Repository.Insert(entity);
        }

        /// <summary>
        /// 插入数据并清除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回是否插入成功</returns>
        public virtual UnifyResult InsertRemoveCache(TEntity entity)
        {
            return Repository.InsertRemoveCache(entity);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回受影响行数</returns>
        public virtual async Task<UnifyResult> InsertAsync(TEntity entity)
        {
            return await Repository.InsertAsync(entity) > 0;
        }

        /// <summary>
        /// 插入数据并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回受影响行数</returns>
        public virtual async Task<UnifyResult> InsertRemoveCacheAsync(TEntity entity)
        {
            return await Repository.InsertRemoveCacheAsync(entity) > 0;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public virtual UnifyResult Insert(List<TEntity> entities)
        {
            return Repository.Insert(entities);
        }

        /// <summary>
        /// 批量添加并删除缓存
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public virtual UnifyResult InsertRemoveCache(List<TEntity> entities)
        {
            return Repository.InsertRemoveCache(entities);
        }

        /// <summary>
        /// 批量添加（异步）
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>受影响的行数</returns>
        public virtual async Task<UnifyResult> InsertAsync(List<TEntity> entities)
        {
            return await Repository.InsertAsync(entities) > 0;
        }

        /// <summary>
        /// 批量添加（异步）并删除缓存
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>受影响的行数</returns>
        public virtual async Task<UnifyResult> InsertRemoveCacheAsync(List<TEntity> entities)
        {
            return await Repository.InsertRemoveCacheAsync(entities) > 0;
        }

        /// <summary>
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        public virtual UnifyResult Update(TEntity entity, bool isNoUpdateNull = false)
        {
            return Repository.Update(entity, isNoUpdateNull);
        }

        /// <summary>
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        public virtual UnifyResult UpdateRemoveCache(TEntity entity, bool isNoUpdateNull = false)
        {
            return Repository.UpdateRemoveCache(entity, isNoUpdateNull);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateAsync(TEntity entity, bool isNoUpdateNull = false)
        {
            return await Repository.UpdateAsync(entity, isNoUpdateNull);
        }

        /// <summary>
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）异步，并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateRemoveCacheAsync(TEntity entity, bool isNoUpdateNull = false)
        {
            return await Repository.UpdateRemoveCacheAsync(entity, isNoUpdateNull);
        }

        /// <summary>
        /// 修改（更新实体部分字段）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        public virtual UnifyResult Update(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Repository.Update(entity, ignoreColumns);
        }

        /// <summary>
        /// 修改（更新实体部分字段）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        public virtual UnifyResult UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Repository.UpdateRemoveCache(entity, ignoreColumns);
        }

        /// <summary>
        /// 修改（更新实体部分字段）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return await Repository.UpdateAsync(entity, ignoreColumns);
        }

        /// <summary>
        /// 修改（更新实体部分字段）异步，并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return await Repository.UpdateRemoveCacheAsync(entity, ignoreColumns);
        }

        /// <summary>
        /// 通过条件更新(不更新忽略字段)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        public virtual UnifyResult Update(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Repository.Update(entity, expression, ignoreColumns);
        }

        public virtual UnifyResult UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Repository.UpdateRemoveCache(entity, expression, ignoreColumns);
        }

        /// <summary>
        /// 通过条件更新(不更新忽略字段)异步
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return await Repository.UpdateAsync(entity, expression, ignoreColumns);
        }

        /// <summary>
        /// 通过条件更新(不更新忽略字段)异步,并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return await Repository.UpdateRemoveCacheAsync(entity, expression, ignoreColumns);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns></returns>
        public virtual UnifyResult Update(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return Repository.Update(entity, expression);
        }

        /// <summary>
        /// 通过条件修改，并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual UnifyResult UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return Repository.UpdateRemoveCache(entity, expression);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.UpdateAsync(entity, expression);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.UpdateRemoveCacheAsync(entity);
        }

        /// <summary>
        /// 修改指定字段
        /// </summary>
        /// <param name="expression">Lambda表达式对象</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns></returns>
        public virtual UnifyResult Update(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return Repository.Update(expression, condition);
        }

        /// <summary>
        /// 修改指定字段,并删除缓存
        /// </summary>
        /// <param name="expression">条件对象</param>
        /// <param name="condition">条件条件</param>
        /// <returns></returns>
        public virtual UnifyResult UpdateRemoveCache(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return Repository.UpdateRemoveCache(expression, condition);
        }

        /// <summary>
        /// 修改指定字段（异步）
        /// </summary>
        /// <param name="expression">Lambda表达式对象</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateAsync(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return await Repository.UpdateAsync(expression, condition);
        }

        /// <summary>
        /// 修改指定字段,并删除缓存
        /// </summary>
        /// <param name="expression">条件对象</param>
        /// <param name="condition">条件条件</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> UpdateRemoveCacheAsync(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return await Repository.UpdateRemoveCacheAsync(expression, condition);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public virtual UnifyResult Delete(dynamic keyValue)
        {
            return Repository.Delete(keyValue);
        }

        /// <summary>
        /// 删除并删除缓存
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public virtual UnifyResult DeleteRemoveCache(dynamic keyValue)
        {
            return Repository.DeleteRemoveCache(keyValue);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> DeleteAsync(dynamic keyValue)
        {
            return await Repository.DeleteAsync(keyValue);
        }

        /// <summary>
        /// 删除(异步)并删除缓存
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> DeleteRemoveCacheAsync(dynamic keyValue)
        {
            return await Repository.DeleteRemoveCacheAsync(keyValue);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public virtual UnifyResult Delete(TEntity entity)
        {
            return Repository.Delete(entity);
        }

        /// <summary>
        /// 删除并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否删除成功</returns>
        public virtual UnifyResult DeleteRemoveCache(TEntity entity)
        {
            return Repository.DeleteRemoveCache(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> DeleteAsync(TEntity entity)
        {
            return await Repository.DeleteAsync(entity);
        }

        /// <summary>
        /// 删除并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否删除成功</returns>
        public virtual async Task<UnifyResult> DeleteRemoveCacheAsync(TEntity entity)
        {
            return await Repository.DeleteRemoveCacheAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual UnifyResult Delete(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.Delete(expression);
        }

        /// <summary>
        /// 删除并删除缓存
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual UnifyResult DeleteRemoveCache(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.DeleteRemoveCache(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.DeleteAsync(expression);
        }

        /// <summary>
        /// 删除（异步）并删除缓存
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> DeleteRemoveCacheAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.DeleteRemoveCacheAsync(expression);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns>是否删除成功</returns>
        public virtual UnifyResult Delete(List<dynamic> keys)
        {
            return Repository.Delete(keys);
        }

        /// <summary>
        /// 批量删除并删除缓存
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns></returns>
        public virtual UnifyResult DeleteRemoveCache(List<dynamic> keys)
        {
            return Repository.DeleteRemoveCache(keys);
        }

        /// 批量删除
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> DeleteAsync(List<dynamic> keys)
        {
            return await Repository.DeleteAsync(keys);
        }

        /// <summary>
        /// 批量删除（异步）并删除缓存
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns></returns>
        public virtual async Task<UnifyResult> DeleteRemoveCacheAsync(List<dynamic> keys)
        {
            return await Repository.DeleteRemoveCacheAsync(keys);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>返回实体</returns>
        public virtual TEntity FindEntity(object keyValue)
        {
            return Repository.FindEntity(keyValue);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>返回实体</returns>
        public virtual Task<TEntity> FindEntityAsync(object keyValue)
        {
            return Repository.FindEntityAsync(keyValue);
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>返回实体</returns>
        public virtual TEntity FindEntity(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.FindEntity(expression);
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>返回实体</returns>
        public virtual async Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.FindEntityAsync(expression);
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>集合</returns>
        public virtual List<TEntity> GetList()
        {
            return Repository.GetList();
        }

        /// <summary>
        /// 获取所有集合(缓存)
        /// </summary>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        public virtual List<TEntity> GetListCache(int s = int.MaxValue)
        {
            return Repository.GetListCache();
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>集合</returns>
        public virtual async Task<List<TEntity>> GetListAsync()
        {
            return await Repository.GetListAsync();
        }

        /// <summary>
        /// 获取所有集合(缓存)
        /// </summary>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListCacheAsync(int s = int.MaxValue)
        {
            return await Repository.GetListCacheAsync(s);
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>集合</returns>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.GetList(expression);
        }

        /// <summary>
        /// 根据条件获取集合并加入缓存
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        public virtual List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, int s = int.MaxValue)
        {
            return Repository.GetListCache(expression, s);
        }

        /// <summary>
        /// 检查信息总条数
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.Count(expression);
        }

        /// <summary>
        /// 检查信息总条数
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.CountAsync(expression);
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>集合</returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.GetListAsync(expression);
        }

        /// <summary>
        /// 根据条件获取集合(缓存)
        /// </summary>
        /// <param name="expression">条件Lambda表达式</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        public virtual async Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, int s = int.MaxValue)
        {
            return await Repository.GetListCacheAsync(expression, s);
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <returns>集合</returns>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc)
        {
            return Repository.GetList(expression, orderby, isDesc);
        }

        public virtual List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc, int s = int.MaxValue)
        {
            return Repository.GetListCache(expression, orderby, isDesc, s);
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <returns>集合</returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc)
        {
            return await Repository.GetListAsync(expression, orderby, isDesc);
        }

        /// <summary>
        /// 根据条件获取集合(缓存)异步
        /// </summary>
        /// <param name="expression">条件Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        public virtual async Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc, int s = int.MaxValue)
        {
            return await Repository.GetListCacheAsync(expression, orderby, isDesc, s);
        }

        /// <summary>
        /// 根据条件获取指定条数集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <returns>集合</returns>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc, int top)
        {
            return Repository.GetList(expression, orderby, isDesc);
        }

        /// <summary>
        /// 根据条件获取指定条数集合(缓存)
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        public virtual List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc, int top, int s = int.MaxValue)
        {
            return Repository.GetListCache(expression, orderby, isDesc, top, s);
        }

        /// <summary>
        /// 根据条件获取指定条数集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <returns>集合</returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc, int top)
        {
            return await Repository.GetListAsync(expression, orderby, isDesc, top);
        }

        /// <summary>
        /// 根据条件获取指定条数集合(缓存)异步
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        public virtual async Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc, int top, int s = Int32.MaxValue)
        {

            return await Repository.GetListCacheAsync(expression, orderby, isDesc, top, s);
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
        public virtual IPagedList<TEntity> GetListByPage(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> @orderby, bool isDesc, int pageIndex, int pageSize)
        {
            return Repository.GetListByPage(expression, orderby, isDesc, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression">条件</param>
        /// <param name="orderby">多字段排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>集合|总条数</returns>
        public virtual IPagedList<TEntity> GetListByPage(Expression<Func<TEntity, bool>> expression, Dictionary<Expression<Func<TEntity, object>>, OrderByType> @orderby, int pageIndex, int pageSize)
        {
            return Repository.GetListByPage(expression, orderby, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="conditionals">查询条件</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        public virtual IPagedList<TEntity> GetListByPage(List<IConditionalModel> conditionals, string orderFileds, int pageIndex, int pageSize)
        {
            return Repository.GetListByPage(conditionals, orderFileds, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="model">分页查询实体</param>
        /// <returns></returns>
        public virtual PageOutputDto<List<TEntity>> GetListByPage(PageQueryInputDto model)
        {
            var list = Repository.GetListByPage(model.ConditionalModels, model.order, model.Page, model.Limit);
            return new PageOutputDto<List<TEntity>>
            {
                count = list.Total,
                data = list.ToList()
            };
        }

        /// <summary>
        /// 是否存在指定数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.Any(expression);
        }

        /// <summary>
        /// 是否存在指定数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.AnyAsync(expression);
        }

        /// <summary>
        /// 获取指定类型的集合
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <returns></returns>
        public virtual async Task<List<TResult>> GetListAsync<TResult>()
        {
            return await Repository.SugarQueryable.Select<TResult>().ToListAsync();
        }

        /// <summary>
        /// 获取指定类型的集合
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        public virtual async Task<List<TResult>> GetListCacheAsync<TResult>(int s = int.MaxValue)
        {
            return await Repository.SugarQueryable.Select<TResult>().WithCache(s).ToListAsync();
        }

        /// <summary>
        /// 获取指定类型的集合
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public virtual async Task<List<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, bool>> expression)
        {
            return await Repository.SugarQueryable.WhereIF(expression != null, expression).Select<TResult>().ToListAsync();
        }

        /// <summary>
        /// 获取指定类型的集合
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">条件</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        public virtual async Task<List<TResult>> GetListCacheAsync<TResult>(Expression<Func<TEntity, bool>> expression, int s = int.MaxValue)
        {
            return await Repository.SugarQueryable.WhereIF(expression != null, expression).Select<TResult>().WithCache(s).ToListAsync();
        }
    }
}