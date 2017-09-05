using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.ORM
{

    /// <summary>
    /// 数据库结构相关的操作
    /// </summary>
    public static class DBStructureOption
    {

        /// <summary>
        /// 删除数据表
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ConStr"></param>
        /// <returns></returns>
        public static bool DropTable(String TableName,String ConStr)
        {

            String str = "DROP TABLE IF EXISTS "+TableName;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    using (MySqlCommand comm = conn.CreateCommand())
                    {
                        conn.Open();
                        comm.CommandText = str;
                        comm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
