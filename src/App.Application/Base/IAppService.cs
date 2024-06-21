using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Pager;
using App.Framwork.Result;
using SqlSugar;

namespace App.Application
{
    public interface IAppService<TEntity>
    {
        ISugarQueryable<TEntity> AsQueryable();

        /// <summary>
        /// 插入数据（适用于id自动增长）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回主键ID</returns>
        UnifyResult InsertScalar(TEntity entity);

        /// <summary>
        /// 插入数据（适用于id自动增长）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回主键ID</returns>
        UnifyResult InsertScalarRemoveCache(TEntity entity);

        /// <summary>
        /// 插入数据（适用于id自动增长）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<UnifyResult> InsertScalarAsync(TEntity entity);

        /// <summary>
        /// 插入数据（适用于id自动增长）并删除缓存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<UnifyResult> InsertScalarRemoveCacheAsync(TEntity entity);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回是否插入成功</returns>
        UnifyResult Insert(TEntity entity);

        /// <summary>
        /// 插入数据并清除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回是否插入成功</returns>
        UnifyResult InsertRemoveCache(TEntity entity);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回受影响行数</returns>
        Task<UnifyResult> InsertAsync(TEntity entity);

        /// <summary>
        /// 插入数据并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回受影响行数</returns>
        Task<UnifyResult> InsertRemoveCacheAsync(TEntity entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        UnifyResult Insert(List<TEntity> entities);

        /// <summary>
        /// 批量添加并删除缓存
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        UnifyResult InsertRemoveCache(List<TEntity> entities);

        /// <summary>
        /// 批量添加（异步）
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>受影响的行数</returns>
        Task<UnifyResult> InsertAsync(List<TEntity> entities);

        /// <summary>
        /// 批量添加（异步）并删除缓存
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>受影响的行数</returns>
        Task<UnifyResult> InsertRemoveCacheAsync(List<TEntity> entities);

        /// <summary>
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        UnifyResult Update(TEntity entity, bool isNoUpdateNull = false);

        /// <summary>
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        UnifyResult UpdateRemoveCache(TEntity entity, bool isNoUpdateNull = false);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateAsync(TEntity entity, bool isNoUpdateNull = false);

        /// <summary>
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）异步，并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateRemoveCacheAsync(TEntity entity, bool isNoUpdateNull = false);

        /// <summary>
        /// 修改（更新实体部分字段）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        UnifyResult Update(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 修改（更新实体部分字段）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        UnifyResult UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 修改（更新实体部分字段）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 修改（更新实体部分字段）异步，并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 通过条件更新(不更新忽略字段)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        UnifyResult Update(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 通过条件更新(不更新忽略字段),并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        UnifyResult UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 通过条件更新(不更新忽略字段)异步
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 通过条件更新(不更新忽略字段)异步,并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns></returns>
        UnifyResult Update(TEntity entity, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 通过条件修改，并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        UnifyResult UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 修改指定字段
        /// </summary>
        /// <param name="expression">Lambda表达式对象</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns></returns>
        UnifyResult Update(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 修改指定字段,并删除缓存
        /// </summary>
        /// <param name="expression">条件对象</param>
        /// <param name="condition">条件条件</param>
        /// <returns></returns>
        UnifyResult UpdateRemoveCache(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 修改指定字段（异步）
        /// </summary>
        /// <param name="expression">Lambda表达式对象</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateAsync(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 修改指定字段,并删除缓存
        /// </summary>
        /// <param name="expression">条件对象</param>
        /// <param name="condition">条件条件</param>
        /// <returns></returns>
        Task<UnifyResult> UpdateRemoveCacheAsync(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        UnifyResult Delete(dynamic keyValue);

        /// <summary>
        /// 删除并删除缓存
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        UnifyResult DeleteRemoveCache(dynamic keyValue);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        Task<UnifyResult> DeleteAsync(dynamic keyValue);

        /// <summary>
        /// 删除(异步)并删除缓存
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        Task<UnifyResult> DeleteRemoveCacheAsync(dynamic keyValue);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        UnifyResult Delete(TEntity entity);

        // <summary>
        /// 删除并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否删除成功</returns>
        UnifyResult DeleteRemoveCache(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        Task<UnifyResult> DeleteAsync(TEntity entity);

        // <summary>
        /// 删除并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否删除成功</returns>
        Task<UnifyResult> DeleteRemoveCacheAsync(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        UnifyResult Delete(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 删除并删除缓存
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        UnifyResult DeleteRemoveCache(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        Task<UnifyResult> DeleteAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 删除（异步）并删除缓存
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        Task<UnifyResult> DeleteRemoveCacheAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns>是否删除成功</returns>
        UnifyResult Delete(List<dynamic> keys);

        /// <summary>
        /// 批量删除并删除缓存
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns></returns>
        UnifyResult DeleteRemoveCache(List<dynamic> keys);

        // <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns></returns>
        Task<UnifyResult> DeleteAsync(List<dynamic> keys);

        /// <summary>
        /// 批量删除（异步）并删除缓存
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns></returns>
        Task<UnifyResult> DeleteRemoveCacheAsync(List<dynamic> keys);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>返回实体</returns>
        TEntity FindEntity(object keyValue);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>返回实体</returns>
        Task<TEntity> FindEntityAsync(object keyValue);

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>返回实体</returns>
        TEntity FindEntity(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>返回实体</returns>
        Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>集合</returns>
        List<TEntity> GetList();

        /// <summary>
        /// 获取所有集合(缓存)
        /// </summary>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        List<TEntity> GetListCache(int s = int.MaxValue);

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>集合</returns>
        Task<List<TEntity>> GetListAsync();

        /// <summary>
        /// 获取所有集合(缓存)
        /// </summary>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        Task<List<TEntity>> GetListCacheAsync(int s = int.MaxValue);

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>集合</returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件获取集合并加入缓存
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, int s = int.MaxValue);

        /// <summary>
        /// 检查信息总条数
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 检查信息总条数
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>集合</returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件获取集合(缓存)
        /// </summary>
        /// <param name="expression">条件Lambda表达式</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, int s = int.MaxValue);

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <returns>集合</returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc);

        /// <summary>
        /// 根据条件获取集合(缓存)
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int s = int.MaxValue);

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <returns>集合</returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc);

        /// <summary>
        /// 根据条件获取集合(缓存)异步
        /// </summary>
        /// <param name="expression">条件Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int s = int.MaxValue);

        /// <summary>
        /// 根据条件获取指定条数集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <returns>集合</returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top);

        /// <summary>
        /// 根据条件获取指定条数集合(缓存)
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top, int s = int.MaxValue);

        /// <summary>
        /// 根据条件获取指定条数集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <returns>集合</returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top);

        /// <summary>
        /// 根据条件获取指定条数集合(缓存)异步
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top, int s = int.MaxValue);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>集合|总条数</returns>
        IPagedList<TEntity> GetListByPage(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int pageIndex, int pageSize);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression">条件</param>
        /// <param name="orderby">多字段排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>集合|总条数</returns>
        IPagedList<TEntity> GetListByPage(Expression<Func<TEntity, bool>> expression, Dictionary<Expression<Func<TEntity, object>>, OrderByType> orderby, int pageIndex, int pageSize);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="conditionals">查询条件</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        IPagedList<TEntity> GetListByPage(List<IConditionalModel> conditionals, string orderFileds, int pageIndex, int pageSize);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="model">分页查询实体</param>
        /// <returns></returns>
        PageOutputDto<List<TEntity>> GetListByPage(PageQueryInputDto model);

        /// <summary>
        /// 是否存在指定数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Any(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 是否存在指定数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取指定类型的集合
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <returns></returns>
        Task<List<TResult>> GetListAsync<TResult>();

        /// <summary>
        /// 获取指定类型的集合
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        Task<List<TResult>> GetListCacheAsync<TResult>(int s = int.MaxValue);

        /// <summary>
        /// 获取指定类型的集合
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        Task<List<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取指定类型的集合
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">条件</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        Task<List<TResult>> GetListCacheAsync<TResult>(Expression<Func<TEntity, bool>> expression, int s = int.MaxValue);
    }
}