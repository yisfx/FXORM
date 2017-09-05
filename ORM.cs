using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FX.ORM
{
    public partial class ORM<T> where T:class
    {
        MySqlConnection connect()
        {
            return new MySqlConnection(constr);
        }
        String constr { set; get; }

        DbConnection con { get; set; }

        List<MySqlParameter> Paras { get; set; }

        StringBuilder QueryStirng { set; get; }


        /// <summary>
        /// 构造函数，初始化查询
        /// </summary>
        public ORM(String constr)
        {
            QueryStirng = new StringBuilder();
            Paras = new List<MySqlParameter>();

            this.constr = constr;

            QueryStirng.Append("select ");
            String TableName = typeof(T).ToString().Split('.').LastOrDefault();
            PropertyInfo[] Fields = typeof(T).GetProperties();
            foreach (PropertyInfo filed in Fields)
            {
                QueryStirng.Append(" " + filed.Name + ",");
            }
            QueryStirng.Append(",");
            QueryStirng.Append(" from " + TableName + " where 1=1 ");
            String temp=QueryStirng.ToString().Replace(",,", " ");
            QueryStirng = new StringBuilder();
            QueryStirng.Append(temp);
        }

        #region 大于
        /// <summary>
        /// 大于；
        /// 生成条件 feild>standard
        /// </summary>
        /// <param name="list"></param>
        /// <param name="feild"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> Gt(String feild, object standard)
        {
            String keyflag = feild + Paras.Count;
            QueryStirng.Append(" and " + feild + ">@" + keyflag + " ");
            Paras.Add(new MySqlParameter(keyflag, standard));
            return this;
        }
        #endregion

        #region 不大于
        /// <summary>
        /// 不大于
        /// 小于或等于
        /// </summary>
        /// <param name="field"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> Ngt(String feild, Object standard)
        {
            String keyflag = feild + Paras.Count;
            QueryStirng.Append(" and " + feild + "<=@" + keyflag + " ");
            Paras.Add(new MySqlParameter(keyflag, standard));
            return this;
        }
        #endregion

        #region 小于
        /// <summary>
        /// 小于
        /// 生成条件 feild< standard
        /// </summary>
        /// <param name="list"></param>
        /// <param name="feild"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> Lt(String feild, object standard)
        {
            String keyflag = feild + Paras.Count;
            QueryStirng.Append(" and " + feild + "<@" + keyflag + " ");
            Paras.Add(new MySqlParameter(keyflag, standard));
            return this;
        }
        #endregion

        #region 不小于 
        /// <summary>
        /// 不小于
        /// 大于或等于
        /// </summary>
        /// <param name="feild"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> Nlt(String feild, Object standard)
        {
            String keyflag = feild + Paras.Count;
            QueryStirng.Append(" and " + feild + ">=@" + keyflag + " ");
            Paras.Add(new MySqlParameter(keyflag, standard));
            return this;
        }
        #endregion

        #region 等于
        /// <summary>
        /// 等于
        /// 生成条件 feild = standard
        /// </summary>
        /// <param name="list"></param>
        /// <param name="feild"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> Eq(String feild, object standard)
        {
            String keyflag = feild + Paras.Count;
            QueryStirng.Append(" and " + feild + "=@" + keyflag + " ");
            Paras.Add(new MySqlParameter(keyflag, standard));
            return this;
        }
        #endregion

        #region 不等于
        /// <summary>
        /// 不等于
        /// 生成条件 feild != standard
        /// 此方法引起全表扫描
        /// </summary>
        /// <param name="list"></param>
        /// <param name="feild"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> Neq(String feild, object standard)
        {
            String keyflag = feild + Paras.Count;
            QueryStirng.Append(" and " + feild + "<>@" + keyflag + " ");
            Paras.Add(new MySqlParameter(keyflag, standard));
            return this;
        }
        #endregion

        #region 以standard开始
        /// <summary>
        /// 以standard开始
        /// 生成 like standard% sql 
        /// </summary>
        /// <param name="feild"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> StartWith(String feild, Object standard)
        {
            QueryStirng.Append(" and " + feild + "like '"+ standard + "%' ");
            return this;
        }
        #endregion

        #region 包含standard
        /// <summary>
        /// 包含standard
        /// 生成 like %standard%
        /// </summary>
        /// <param name="feild"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> Content(String feild, Object standard)
        {
            QueryStirng.Append(" and " + feild + "like '%" + standard + "%' ");
            return this;
        }
        #endregion

        #region 以standard结束
        /// <summary>
        /// 以standard结束
        /// 生成 like %standard
        /// </summary>
        /// <param name="feild"></param>
        /// <param name="standard"></param>
        /// <returns></returns>
        public ORM<T> EndWith(String feild, Object standard)
        {
            QueryStirng.Append(" and " + feild + "like '%" + standard + "' ");
            return this;
        }
        #endregion

        #region 添加条件
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ORM<T> ConditionAttach(params ORM.Arg[] args)
        {
            if (args == null || args.Count() < 1)
                return this;
            foreach (ORM.Arg arg in args)
            {
                switch (arg.Option)
                {
                    case SqlOption.Content:
                        Content(arg.Field, arg.Stantard);
                        break;
                    case SqlOption.EndWith:
                        EndWith(arg.Field, arg.Stantard);
                        break;
                    case SqlOption.Eq:
                        Eq(arg.Field, arg.Stantard);
                        break;
                    case SqlOption.Gt:
                        Gt(arg.Field, arg.Stantard);
                        break;
                    case SqlOption.Lt:
                        Lt(arg.Field, arg.Stantard);
                        break;
                    case SqlOption.Neq:
                        Neq(arg.Field, arg.Stantard);
                        break;
                    case SqlOption.Ngt:
                        Ngt(arg.Field, arg.Stantard);
                        break;
                    case SqlOption.Nlt:
                        Nlt(arg.Field, arg.Stantard);
                        break;
                    case SqlOption.StartWith:
                        StartWith(arg.Field, arg.Stantard);
                        break;
                }
            }

            return this;
        }
        #endregion

        #region select
        /// <summary>
        /// select
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Select()
        {
            PropertyInfo[] Fields = typeof(T).GetProperties();

            using (MySqlConnection conn = connect())
            {
                using (MySqlCommand comm = new MySqlCommand(QueryStirng.ToString(), conn))
                {
                    conn.Open();
                    if (Paras != null && Paras.Count() > 0)
                    {
                        comm.Parameters.AddRange(Paras.ToArray());
                    }
                    using (DbDataReader reader = comm.ExecuteReader())
                    {
                        T model = default(T);
                        while (reader.Read())
                        {
                            model = (T)Activator.CreateInstance(typeof(T));
                            foreach (PropertyInfo Field in Fields)
                            {
                                if (reader[Field.Name] == DBNull.Value)
                                {
                                    Field.SetValue(model, null, null);
                                }
                                else
                                {
                                    var type =Type.GetType(Field.PropertyType.FullName);
                                    var value = Helper.ConvertHelper.Convert(reader[Field.Name], type);
                                    Field.SetValue(model, value, null);
                                }
                            }
                            yield return model;
                        }
                    }
                }
            }
        }
        #endregion

        #region add
        /// <summary>
        /// add
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Add(T t)
        {
            GetQuery queryEntity = new GetQuery();
            String query = queryEntity.GetInsertQuery<T>(t);
            using (MySqlConnection conn = connect())
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    comm.Parameters.AddRange(queryEntity.paramaters.ToArray());
                    return comm.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region delete
        /// <summary>
        /// delete 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Delete(T t)
        {
            GetQuery queryEntity = new GetQuery();
            String query = queryEntity.GetDeleteQuery<T>(t);
            using (MySqlConnection conn = connect())
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    comm.Parameters.AddRange(queryEntity.paramaters.ToArray());
                    return comm.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Modetify
        /// <summary>
        /// Modetify
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Modetify(T t)
        {
            GetQuery queryEntity = new GetQuery();
            String query = queryEntity.GetModetifyQuery<T>(t);
            using (MySqlConnection conn = connect())
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    comm.Parameters.AddRange(queryEntity.paramaters.ToArray());
                    return comm.ExecuteNonQuery();
                }
            }
        }
        #endregion

    }
}
