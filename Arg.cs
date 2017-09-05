using System;
using System.Collections.Generic;
using System.Text;

namespace FX.ORM
{
    public class Arg
    {

        /// <summary>
        /// 参数
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="option">查询类型</param>
        /// <param name="value">值</param>
        public Arg(String field,SqlOption option,Object value)
        {
            this.Option = option;
            this.Stantard = value;
            this.Field = field;
        }

        /// <summary>
        /// 条件字段
        /// </summary>
        public String Field { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public SqlOption Option { set; get; }
        /// <summary>
        /// 范围
        /// </summary>
        public Object Stantard { set; get; }

    }

    /// <summary>
    /// sql 条件
    /// </summary>
    public enum SqlOption
    {
        /// <summary>
        /// 大于
        /// </summary>
        Gt=0,
        /// <summary>
        /// 不大于
        /// </summary>
        Ngt=1,
        /// <summary>
        /// 小于
        /// </summary>
        Lt=2,
        /// <summary>
        /// 不小于
        /// </summary>
        Nlt=3,
        /// <summary>
        /// 等于
        /// </summary>
        Eq=4,
        /// <summary>
        /// 不等于
        /// </summary>
        Neq=5,
        /// <summary>
        /// 包含
        /// </summary>
        Content=6,
        /// <summary>
        /// 
        /// </summary>
        StartWith=7,
        /// <summary>
        /// 
        /// </summary>
        EndWith=8

    } 
}
