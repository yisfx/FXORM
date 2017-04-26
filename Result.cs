using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace FX.ORM
{
    public class Result
    {
        static MySqlConnection connect()
        {
                return new MySqlConnection("Server=127.0.0.1;Database=test;Uid=root;Pwd=wangsai");
        }

        public static int Add<T>(T t)
        {
            GetQuery queryEntity = new GetQuery();
            String query= queryEntity.GetInsertQuery<T>(t);
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

        public static int Delete<T>(T t)
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

        public static int Modetify<T>(T t)
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

        public IEnumerable<T> Select<T>(String where,params KeyValuePair<String,object>[] paras)
        {

            PropertyInfo[] Fields = typeof(T).GetProperties();
            GetQuery queryEntity = new GetQuery();
            String query = queryEntity.GetSelectQuery<T>(where);
            using (MySqlConnection conn = connect())
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    if (paras != null && paras.Count() > 0)
                    {
                        var ps = paras.Select(d => new MySqlParameter("@" + d.Key, d.Value)).ToArray();
                        comm.Parameters.AddRange(ps);
                    }
                    using (DbDataReader reader = comm.ExecuteReader())
                    {
                        T model = default(T);
                        while (reader.Read())
                        {

                            model =(T)Activator.CreateInstance(typeof(T));
                            foreach (PropertyInfo Field in Fields)
                            {
                                if (reader[Field.Name] == DBNull.Value)
                                {
                                    Field.SetValue(model, null);
                                }
                                else
                                    Field.SetValue(model,reader[Field.Name]);
                            }
                            yield return model;
                        }
                    }
                }
            }
        }
    }
}
