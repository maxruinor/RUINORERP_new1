﻿using Npgsql;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HLH.Lib.DB
{

    /*
     * 注意：
1.查询语句中表名、字段名需用中括号[ ] 封起来，在操作类中会统一替换为双引号。
2.命令语句中的参数名格式为：@或？+ 参数名，在操作类中为统一替换为冒号。
3.对于二进制流、日期等特殊类型需采用2中的参数形式
4.经多次测试postgreSQL数据库只直接支持（冒号+参数名）的参数方式

     IDBHelper dbHelper = new PostgreHelper();
string connectionString = "User ID=postgres;Password=admin;Server=192.168.0.226;Port=5432;Database=MonitorDB;";
string sql = "insert into [RawLog]([ProjectID],[File],[Note]) values('项目编号',@file,'备注')";

string file = this.textBox1.Text;
if (!File.Exists(file)) return;

//获取文件二进制流
System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open);
BinaryReader br = new BinaryReader(fs);
byte[] b = br.ReadBytes((int)fs.Length);
br.Close();fs.Close();

int r = dbHelper.ExecuteNonQuery(connectionString, CommandType.Text,sql,new Npgsql.NpgsqlParameter("@file",raw_log.File));

     
     */


    /// <summary>
    /// 数据库操作基类(for PostgreSQL)
    /// </summary>
    public class PostgreHelper : IDBHelper
    {
        /// <summary>
        /// 得到数据条数
        /// </summary>
        public int GetCount(string connectionString, string tblName, string condition)
        {
            StringBuilder sql = new StringBuilder("select count(*) from " + tblName);
            if (!string.IsNullOrEmpty(condition))
                sql.Append(" where " + condition);

            object count = ExecuteScalar(connectionString, CommandType.Text, sql.ToString(), null);
            return int.Parse(count.ToString());
        }

        /// <summary>
        /// 执行查询，返回DataSet
        /// </summary>
        public DataSet ExecuteQuery(string connectionString, CommandType cmdType, string cmdText,
          params DbParameter[] cmdParms)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                        return ds;
                    }
                }
            }
        }


        /// <summary>
        /// 执行查询，返回DataSet
        /// </summary>
        public DataSet ExecuteQuery(string connectionString, CommandType cmdType, string cmdText)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                        return ds;
                    }
                }
            }
        }


        /// <summary>
        /// 在事务中执行查询，返回DataSet
        /// </summary>
        public DataSet ExecuteQuery(DbTransaction trans, CommandType cmdType, string cmdText,
          params DbParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "ds");
            cmd.Parameters.Clear();
            return ds;
        }

        /// <summary>
        /// 执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText,
          params DbParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 在事务中执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        public int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText,
          params DbParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行查询，返回DataReader
        /// </summary>
        public DbDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText,
          params DbParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                NpgsqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 在事务中执行查询，返回DataReader
        /// </summary>
        public DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText,
          params DbParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            NpgsqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText,
          params DbParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 在事务中执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        public object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText,
          params DbParameter[] cmdParms)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 生成要执行的命令
        /// </summary>
        /// <remarks>参数的格式：冒号+参数名</remarks>
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType,
          string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText.Replace("@", ":").Replace("?", ":").Replace("[", "\"").Replace("]", "\"");

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (NpgsqlParameter parm in cmdParms)
                {
                    parm.ParameterName = parm.ParameterName.Replace("@", ":").Replace("?", ":");

                    cmd.Parameters.Add(parm);
                }
            }
        }
    }


}
