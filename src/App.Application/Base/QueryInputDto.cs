using System.Collections.Generic;
using SqlSugar;
using System;
using System.Linq;

namespace App.Application
{
    /// <summary>
    /// 分页查询实体
    /// </summary>
    public class PageQueryInputDto : PageInputDto
    {
        /// <summary>
        /// 排序字段加asc/desc
        /// </summary>
        public string order { get; set; }

        private List<Condition> _conditions;

        /// <summary>
        /// 条件（接收客户端输入的条件参数）
        /// </summary>
        public List<Condition> conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                if (value.Any())
                {
                    var list = conditions.Select(x => new ConditionalModel
                    {
                        FieldName = x.name,
                        FieldValue = x.value,
                        ConditionalType = GetOperator(x.op)
                    }).ToList();
                    ConditionalModels.AddRange(list);
                }
            }
        }

        /// <summary>
        /// 条件（供底层条件查询）
        /// </summary>
        public List<IConditionalModel> ConditionalModels { get; private set; } = new List<IConditionalModel>();

        /// <summary>
        /// 获取操作符
        /// </summary>
        /// <param name="op">操作符简写</param>
        /// <returns></returns>
        private ConditionalType GetOperator(string op)
        {
            return op.ToLower() switch
            {
                "eq" => ConditionalType.Equal,
                "gt" => ConditionalType.GreaterThan,
                "ge" => ConditionalType.GreaterThanOrEqual,//大于或等于
                "in" => ConditionalType.In,//包括
                "lt" => ConditionalType.LessThan,//小于
                "le" => ConditionalType.LessThanOrEqual,//小于或等于
                "cn" => ConditionalType.Like,//包含
                "sw" => ConditionalType.LikeLeft,//以...开始
                "ew" => ConditionalType.LikeRight,//以什么结束
                "ne" => ConditionalType.NoEqual,//不等于
                "nc" => ConditionalType.NoLike,//不包含
                "ni" => ConditionalType.NotIn,//不包括
                _ => throw new ArgumentException("参数不匹配")
            };
        }
    }

    public class Condition
    {
        /// <summary>
        /// 查询字段名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 查询比较符号
        /// </summary>
        public string op { get; set; }

    }
}