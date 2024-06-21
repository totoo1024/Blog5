using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using SqlSugar;
using System;
using System.Data;
using System.Threading.Tasks;

namespace App.Core.Data.Interceptor
{
    /// <summary>
    /// 数据库事务拦截器
    /// </summary>
    public class TransactionAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="level">事务级别</param>
        public TransactionAttribute(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            Level = level;
        }

        /// <summary>
        /// 事务级别
        /// </summary>
        public IsolationLevel Level { get; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var client = context.ServiceProvider.Resolve<ISqlSugarClient>();
            //var client = context.ServiceProvider.Resolve<ISqlSugarClient>() as SqlSugarClient;//多数据库使用
            try
            {
                //单库开启事务
                client.Ado.BeginTran(Level);
                //client.BeginTran();//多数据库使用
                await next(context);

                //单库提交事务
                client.Ado.CommitTran();
                //client.CommitTran();//多数据库使用
            }
            catch (Exception ex)
            {
                //单库提交事务
                client.Ado.RollbackTran();
                //client.RollbackTran();//多数据库使用
                throw ex;
            }
        }
    }
}
