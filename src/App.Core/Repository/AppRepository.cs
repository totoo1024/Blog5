using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Pager;
using SqlSugar;
using App.Core.Extensions;

namespace App.Core.Repository
{
    /// <summary>
    /// 仓储通用操作接口
    /// </summary>
    /// <typeparam name="TEntity">entity（对应数据库表）</typeparam>
    public class AppRepository<TEntity> : IAppRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        public ISqlSugarClient Db { get; }

        /// <summary>
        /// 查询
        /// </summary>
        public ISugarQueryable<TEntity> SugarQueryable => Db.QueryFilter<TEntity>();

        public AppRepository(ISqlSugarClient sqlSugarClient)
        {
            Db = sqlSugarClient;
        }

        #region 基础

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IInsertable<TEntity> Insertable(TEntity entity)
        {
            return Db.Insertable(entity);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IInsertable<TEntity> Insertable(TEntity[] entities)
        {
            return Db.Insertable(entities);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IInsertable<TEntity> Insertable(List<TEntity> entities)
        {
            return Db.Insertable(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IUpdateable<TEntity> Updateable(TEntity entity)
        {
            return Db.Updateable(entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IUpdateable<TEntity> Updateable(TEntity[] entities)
        {
            return Db.Updateable<TEntity>(entities);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IUpdateable<TEntity> Updateable(List<TEntity> entities)
        {
            return Db.Updateable(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public IDeleteable<TEntity> Deleteable()
        {
            return Db.Deleteable<TEntity>();
        }
        #endregion

        #region 添加

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
        /// 插入数据（适用于id自动增长）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回主键ID</returns>
        public int InsertScalarRemoveCache(TEntity entity)
        {
            return Db.Insertable(entity).RemoveDataCache().ExecuteReturnIdentity();
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
        /// 插入数据（适用于id自动增长）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> InsertScalarRemoveCacheAsync(TEntity entity)
        {
            return Db.Insertable(entity).RemoveDataCache().ExecuteReturnIdentityAsync();
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
        /// 插入数据并清除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public bool InsertRemoveCache(TEntity entity)
        {
            return Db.Insertable(entity).RemoveDataCache().ExecuteCommand() > 0;
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
        /// 插入数据（异步）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回受影响行数</returns>
        public Task<int> InsertRemoveCacheAsync(TEntity entity)
        {
            return Db.Insertable(entity).RemoveDataCache().ExecuteCommandAsync();
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
        /// 批量添加并删除缓存
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns></returns>
        public bool InsertRemoveCache(List<TEntity> entities)
        {
            return Db.Insertable(entities).RemoveDataCache().ExecuteCommand() > 0;
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
        /// 批量添加（异步）并删除缓存
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>受影响的行数</returns>
        public Task<int> InsertRemoveCacheAsync(List<TEntity> entities)
        {
            return Db.Insertable(entities).RemoveDataCache().ExecuteCommandAsync();
        }

        #endregion

        #region 修改

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
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        public bool UpdateRemoveCache(TEntity entity, bool isNoUpdateNull = false)
        {
            return Db.Updateable(entity).IgnoreColumns(isNoUpdateNull).RemoveDataCache().ExecuteCommandHasChange();
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
        /// 通过主键修改（包含是否需要将null值字段提交到数据库）异步，并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="isNoUpdateNull">是否排除NULL值字段更新</param>
        /// <returns></returns>
        public Task<bool> UpdateRemoveCacheAsync(TEntity entity, bool isNoUpdateNull = false)
        {
            return Db.Updateable(entity).IgnoreColumns(isNoUpdateNull).RemoveDataCache().ExecuteCommandHasChangeAsync();
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
        /// 通过主键修改（更新实体部分字段）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        public bool UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Db.Updateable(entity).IgnoreColumns(ignoreColumns).RemoveDataCache().ExecuteCommandHasChange();
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
        /// 通过主键修改（更新实体部分字段）异步,并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ignoreColumns">忽略字段（不更新字段）</param>
        /// <returns></returns>
        public Task<bool> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Db.Updateable(entity).IgnoreColumns(ignoreColumns).RemoveDataCache().ExecuteCommandHasChangeAsync();
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
        /// 通过条件更新(不更新忽略字段),并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        public bool UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Db.Updateable(entity).Where(expression).IgnoreColumns(ignoreColumns).RemoveDataCache().ExecuteCommandHasChange();
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
        /// 通过条件更新(不更新忽略字段)异步,并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">条件</param>
        /// <param name="ignoreColumns">忽略更新的字段</param>
        /// <returns></returns>
        public Task<bool> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> ignoreColumns)
        {
            return Db.Updateable(entity).Where(expression).IgnoreColumns(ignoreColumns).RemoveDataCache().ExecuteCommandHasChangeAsync();
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
        /// 通过条件修改，并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>是否成功</returns>
        public bool UpdateRemoveCache(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return Db.Updateable(entity).Where(expression).RemoveDataCache().ExecuteCommandHasChange();
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
        /// 通过条件修改（异步）并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>是否成功</returns>
        public Task<bool> UpdateRemoveCacheAsync(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return Db.Updateable(entity).Where(expression).RemoveDataCache().ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 修改（指定字段）
        /// </summary>
        /// <param name="expression">需要修改的字段</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns>是否修改成功</returns>
        public bool Update(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return Db.Updateable<TEntity>().SetColumns(expression).Where(condition).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 修改（指定字段）,并删除缓存
        /// </summary>
        /// <param name="expression">需要修改的字段</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns>是否修改成功</returns>
        public bool UpdateRemoveCache(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return Db.Updateable<TEntity>().SetColumns(expression).Where(condition).RemoveDataCache().ExecuteCommandHasChange();
        }

        /// <summary>
        /// 修改（指定字段）异步
        /// </summary>
        /// <param name="expression">需要修改的字段</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns>是否修改成功</returns>
        public Task<bool> UpdateAsync(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return Db.Updateable<TEntity>().SetColumns(expression).Where(condition).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 修改（指定字段）异步,并删除缓存
        /// </summary>
        /// <param name="expression">需要修改的字段</param>
        /// <param name="condition">Lambda表达式条件</param>
        /// <returns>是否修改成功</returns>
        public Task<bool> UpdateRemoveCacheAsync(Expression<Func<TEntity, TEntity>> expression, Expression<Func<TEntity, bool>> condition)
        {
            return Db.Updateable<TEntity>().SetColumns(expression).Where(condition).RemoveDataCache().ExecuteCommandHasChangeAsync();
        }

        #endregion

        #region 删除

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
        /// 删除并删除缓存
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteRemoveCache(dynamic keyValue)
        {
            return Db.Deleteable<TEntity>(keyValue).RemoveDataCache().ExecuteCommandHasChange();
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
        /// 删除(异步)并删除缓存
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> DeleteRemoveCacheAsync(dynamic keyValue)
        {
            return Db.Deleteable<TEntity>(keyValue).RemoveDataCache().ExecuteCommandHasChangeAsync();
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
        /// 删除并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteRemoveCache(TEntity entity)
        {
            return Db.Deleteable(entity).RemoveDataCache().ExecuteCommandHasChange();
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
        /// 删除(异步)并删除缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> DeleteRemoveCacheAsync(TEntity entity)
        {
            return Db.Deleteable(entity).RemoveDataCache().ExecuteCommandHasChangeAsync();
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
        /// 删除并删除缓存
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteRemoveCache(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Deleteable<TEntity>().Where(expression).RemoveDataCache().ExecuteCommandHasChange();
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

        /// <summary>
        /// 删除（异步）并删除缓存
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> DeleteRemoveCacheAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Deleteable<TEntity>().Where(expression).RemoveDataCache().ExecuteCommandHasChangeAsync();
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
        /// 批量删除并删除缓存
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteRemoveCache(List<dynamic> keys)
        {
            return Db.Deleteable<TEntity>(keys).RemoveDataCache().ExecuteCommandHasChange();
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

        // <summary>
        /// 批量删除（异步）并删除缓存
        /// </summary>
        /// <param name="keys">主键集合</param>
        /// <returns>是否删除成功</returns>
        public Task<bool> DeleteRemoveCacheAsync(List<dynamic> keys)
        {
            return Db.Deleteable<TEntity>(keys).RemoveDataCache().ExecuteCommandHasChangeAsync();
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>返回实体</returns>
        public TEntity FindEntity(object keyValue)
        {
            return SugarQueryable.InSingle(keyValue);
        }


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>返回实体</returns>
        public Task<TEntity> FindEntityAsync(object keyValue)
        {
            return SugarQueryable.InSingleAsync(keyValue);
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>返回实体</returns>
        public TEntity FindEntity(Expression<Func<TEntity, bool>> expression)
        {
            return SugarQueryable.WhereIF(expression != null, expression).First();
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns>返回实体</returns>
        public Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> expression)
        {
            return SugarQueryable.WhereIF(expression != null, expression).FirstAsync();
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>集合</returns>
        public List<TEntity> GetList()
        {
            return SugarQueryable.ToList();
        }

        /// <summary>
        /// 获取所有集合(缓存)
        /// </summary>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        public List<TEntity> GetListCache(int s = int.MaxValue)
        {
            return SugarQueryable.WithCache(s).ToList();
        }

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>集合</returns>
        public Task<List<TEntity>> GetListAsync()
        {
            return SugarQueryable.ToListAsync();
        }

        /// <summary>
        /// 获取所有集合(缓存)
        /// </summary>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        public Task<List<TEntity>> GetListCacheAsync(int s = int.MaxValue)
        {
            return SugarQueryable.WithCache(s).ToListAsync();
        }

        /// <summary>
        /// 检查信息总条数
        /// </summary>
        /// <param name="expression">条件（可为空）</param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                return SugarQueryable.Count();
            }
            return SugarQueryable.Count(expression);
        }

        /// <summary>
        /// 检查信息总条数
        /// </summary>
        /// <param name="expression">条件（可为空）</param>
        /// <returns></returns>
        public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                return SugarQueryable.CountAsync();
            }
            return SugarQueryable.CountAsync(expression);
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式（可为空）</param>
        /// <returns>集合</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> expression)
        {
            return SugarQueryable.WhereIF(expression != null, expression).ToList();
        }

        /// <summary>
        /// 根据条件获取集合并加入缓存
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns></returns>
        public List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, int s = int.MaxValue)
        {
            return SugarQueryable.WhereIF(expression != null, expression).WithCache(s).ToList();
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">条件（可为空）</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return SugarQueryable.WhereIF(expression != null, expression).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">条件</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, int s = int.MaxValue)
        {
            return SugarQueryable.WhereIF(expression != null, expression).WithCache(s).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">条件（可为空）</param>
        /// <param name="orderby">排序（可为空）</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <returns>集合</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc)
        {
            return SugarQueryable.WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).ToList();
        }

        /// <summary>
        /// 根据条件获取集合(缓存)
        /// </summary>
        /// <param name="expression">条件（可为空）</param>
        /// <param name="orderby">排序（为空）</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        public List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int s = int.MaxValue)
        {
            return SugarQueryable.WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).WithCache(s).ToList();
        }

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc)
        {
            return SugarQueryable.WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取集合(缓存)异步
        /// </summary>
        /// <param name="expression">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int s = int.MaxValue)
        {
            return SugarQueryable.WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).WithCache(s).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取指定条数集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <returns>集合</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top)
        {
            return SugarQueryable.WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).Take(top).ToList();
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
        public List<TEntity> GetListCache(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top, int s = int.MaxValue)
        {
            return SugarQueryable.WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).Take(top).WithCache(s).ToList();
        }

        /// <summary>
        /// 根据条件获取指定条数集合
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top)
        {
            return SugarQueryable.WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).Take(top).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取指定条数集合异步(缓存)
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <param name="orderby">排序</param>
        /// <param name="isDesc">是否降序排列</param>
        /// <param name="top">前N条数据</param>
        /// <param name="s">缓存时间：秒</param>
        /// <returns>集合</returns>
        public Task<List<TEntity>> GetListCacheAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int top, int s = int.MaxValue)
        {
            return SugarQueryable.WhereIF(expression != null, expression).OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc).Take(top).WithCache(s).ToListAsync();
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
        public IPagedList<TEntity> GetListByPage(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderby, bool isDesc, int pageIndex, int pageSize)
        {
            return SugarQueryable
                .WhereIF(expression != null, expression)
                .OrderByIF(orderby != null, orderby, isDesc ? OrderByType.Desc : OrderByType.Asc)
                .ToPage(pageIndex, pageSize);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>集合|总条数</returns>
        public IPagedList<TEntity> GetListByPage(Expression<Func<TEntity, bool>> expression, Dictionary<Expression<Func<TEntity, object>>, OrderByType> orderby, int pageIndex, int pageSize)
        {
            var query = SugarQueryable.WhereIF(expression != null, expression);
            foreach (var item in orderby)
            {
                query.OrderBy(item.Key, item.Value);
            }
            return query.ToPage(pageIndex, pageSize);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="conditionals">查询条件</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        public IPagedList<TEntity> GetListByPage(List<IConditionalModel> conditionals, string orderFileds, int pageIndex, int pageSize)
        {
            var query = SugarQueryable;
            if (conditionals != null)
                query.Where(conditionals);

            query.OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds);
            return query.ToPage(pageIndex, pageSize);
        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return SugarQueryable.Any(expression);
        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return SugarQueryable.AnyAsync(expression);
        }

        #endregion
    }
}