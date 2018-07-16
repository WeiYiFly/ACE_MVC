using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.RollCall.Common
{
    public class SQLHelper
    {
        private static readonly string connString = ConfigurationManager.AppSettings["124"];
        public static DataTable GetTable(string sql, CommandType type, params SqlParameter[] pars)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlDataAdapter apter = new SqlDataAdapter(sql, conn))
                {
                    apter.SelectCommand.CommandType = type;
                    //if (pars != null)
                    //{
                    //    apter.SelectCommand.Parameters.AddRange(pars);
                    //}
                    DataTable da = new DataTable();
                    apter.Fill(da);
                    return da;
                }
            }
        }

        public static int ExecuteNonquery(string sql, CommandType type, params SqlParameter[] pars)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = type;
                    if (pars != null)
                    {
                        cmd.Parameters.AddRange(pars);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }

        }
        public static object ExecuteScalare(string sql, CommandType type, params SqlParameter[] pars)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = type;
                    if (pars != null)
                    {
                        cmd.Parameters.AddRange(pars);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}