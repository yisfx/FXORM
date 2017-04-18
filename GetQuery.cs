using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;
using System.Linq.Expressions;

namespace FX.ORM
{
    public class GetQuery
    {
        public List<DbParameter> paramaters { set; get; }

        StringBuilder sqlQuery = new StringBuilder();

        List<DbParameter> paras = new List<DbParameter>();

        #region insert sql query
        /// <summary>
        /// insert sql query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public String GetInsertQuery<T>(T t)
        {
            
            String TableName = t.ToString().Split('.').LastOrDefault();
            if (String.IsNullOrWhiteSpace(TableName))
                throw new Exception("数据表名错误:"+t.ToString());
            StringBuilder insertPar = new StringBuilder();
            sqlQuery.Append("insert into " + TableName + "(");
            PropertyInfo[] Fileds = t.GetType().GetProperties();
            foreach(PropertyInfo filed in Fileds)
            {
                sqlQuery.Append(filed.Name+",");
                insertPar.Append("@" + filed.Name+",");
                paras.Add(new MySqlParameter(filed.Name, filed.GetValue(t,null)));
            }
            sqlQuery.Append(") values(");
            insertPar.Append(")");
            sqlQuery.Append(insertPar.ToString());
            String query = sqlQuery.ToString().Replace(",)", ")"); ;
            paramaters = paras;
            return query;
        }
        #endregion

        #region delete sql query
        /// <summary>
        /// delete sql query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public String GetDeleteQuery<T>(T t)
        {
            String TableName = t.ToString().Split('.').LastOrDefault();
            if (String.IsNullOrWhiteSpace(TableName))
                throw new Exception("数据表名错误:" + t.ToString());
            StringBuilder insertPar = new StringBuilder();
            sqlQuery.Append("delete from " + TableName + " where ");
            PropertyInfo key = t.GetType().GetProperties().Where(d => d.Name.ToLower().Equals("id")).FirstOrDefault();
            if (key == null)
            {
                throw new Exception("未找到主键ID");
            }
            sqlQuery.Append(key.Name + "=@" + key.Name);
            paras.Add(new MySqlParameter("@" + key.Name, key.GetValue(t, null)));

            String query = sqlQuery.ToString();
            paramaters = paras;
            return query;
        }
        #endregion

        #region modetify sql query
        /// <summary>
        /// modetify sql query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public String GetModetifyQuery<T>(T t)
        {

            String TableName = t.ToString().Split('.').LastOrDefault();
            if (String.IsNullOrWhiteSpace(TableName))
                throw new Exception("数据表名错误:" + t.ToString());
            sqlQuery.Append("update " + TableName + " set ");
            PropertyInfo[] Fileds = t.GetType().GetProperties();


            foreach (PropertyInfo filed in Fileds)
            {
                if (filed.Name.ToLower().Equals("id"))
                    continue;
                sqlQuery.Append(filed.Name + "=@"+filed.Name+",");
                paras.Add(new MySqlParameter(filed.Name, filed.GetValue(t, null)));
            }
            PropertyInfo key = Fileds.Where(d => d.Name.ToLower().Equals("id")).FirstOrDefault();
            if (key == null)
                throw new Exception("未找到主键");

            sqlQuery.Append(" where " + key.Name + "=@" + key.Name);
            paras.Add(new MySqlParameter(key.Name, key.GetValue(t, null)));

            String query = sqlQuery.ToString().Replace(", where", " where");
            paramaters = paras;
            return query;
        }
        #endregion

        #region select sql query
        /// <summary>
        /// select sql query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public String GetSelectQuery<T>(String where)
        {
            sqlQuery.Append("select ");
            String TableName=typeof(T).ToString().Split('.').LastOrDefault();
            PropertyInfo[] Fields = typeof(T).GetProperties();
            foreach(PropertyInfo filed in Fields)
            {
                sqlQuery.Append(" " + filed.Name + ",");
            }
            sqlQuery.Append(" from "+TableName+" where 1=1 ");
            if (!String.IsNullOrEmpty(where))
            {
                sqlQuery.Append(" and "+where);
            }
            String result = sqlQuery.ToString().Replace(", from", " from");
            return result;
        }
        #endregion

    }
}
