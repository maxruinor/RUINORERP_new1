using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace HLH.Lib.DB
{
    /// <summary>
    /// 数据访问抽象基础类
    /// Copyright (C) 2004-2008 LiTianPing
    /// All rights reserved
    /// </summary>
    public abstract class MSSQLHelper
    {

        /// <summary>
        /// 数据库连接字符串(web.config来配置)
        /// </summary>
        public static string SH_CMSConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
        //public static string SH_CMSConnectionString = LTP.Common.DEncrypt.DESEncrypt.Decrypt(LTP.Common.ConfigHelper.GetConfigString("SH_CMSConnectionString"));

        /// <summary>
        /// 项目配置文件中指定连接字串。节点为"SqlConnectionString"
        /// </summary>
        public MSSQLHelper()
        {

        }

        /// <summary>
        /// 外部指定连接字串
        /// </summary>
        /// <param name="connstr"></param>
        public static void SetConnectionStr(string connstr)
        {
            if (string.IsNullOrEmpty(SH_CMSConnectionString))
            {
                SH_CMSConnectionString = connstr;
            }
        }

        #region 公用方法

        public static int GetMaxID(string FieldName, string TableName)
        {
            // string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            string strsql = "select isnull(max(cast (" + FieldName + " as int )),0)+1 from " + TableName;

            object obj = MSSQLHelper.GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }


        public static bool Exists(string strSql)
        {
            object obj = MSSQLHelper.GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = MSSQLHelper.GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    cmd.CommandTimeout = 80000;
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        connection.Close();
                        logInsert(SQLString, ex);
                        return 0;
                    }
                }
            }
        }



        /// <summary>
        /// 执行SQL语句，设置命令的执行等待时间
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        connection.Close();
                        logInsert(SQLString, ex);
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(SH_CMSConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    tx.Rollback();
                    logInsert(ex);
                    throw new Exception("事务执行出错");
                }


            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    logInsert(SQLString, ex);
                    return 0;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static object ExecuteSqlGet(string SQLString, string content)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    logInsert(SQLString, ex);
                    return null;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    logInsert(strSQL, ex);
                    return 0;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        connection.Close();
                        logInsert(SQLString, ex);
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// 执行查询语句，返回SqlDataReader(使用该方法切记要手工关闭SqlDataReader和连接)
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            SqlConnection connection = new SqlConnection(SH_CMSConnectionString);
            SqlCommand cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                logInsert(strSQL, ex);
                throw ex;
            }
            //finally //不能在此关闭，否则，返回的对象将无法使用
            //{
            //	cmd.Dispose();
            //	connection.Close();
            //}	


        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="TableName">DataSet中的源表名</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, string TableName)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = 80000;
                    command.Fill(ds, TableName);
                }
                catch (Exception ex)
                {
                    logInsert(SQLString, ex);
                }

                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="TableName">DataSet中的源表名</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {

                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = 80000;
                    command.Fill(ds, "TableName");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    logInsert(SQLString, ex);
                    throw ex;
                }

                return ds;
            }
        }


        /// <summary>
        /// 执行查询语句，返回DataSet,设置命令的执行等待时间
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times"></param>
        /// <param name="TableName">DataSet中的源表名</param>
        /// <returns></returns>
        public static DataSet Query(string SQLString, int Times, string TableName)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, TableName);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    logInsert(SQLString, ex);
                }

                return ds;
            }
        }



        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        logInsert(SQLString, ex);
                        return 0;
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。 需要验证
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(SH_CMSConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        logInsert(ex);
                        throw ex;
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        logInsert(SQLString, ex);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader (使用该方法切记要手工关闭SqlDataReader和连接)
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(SH_CMSConnectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                logInsert(SQLString, ex);
                throw ex;
            }
            //finally //不能在此关闭，否则，返回的对象将无法使用
            //{
            //	cmd.Dispose();
            //	connection.Close();
            //}	

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, string TableName, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, TableName);
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        logInsert(SQLString, ex);
                    }
                    return ds;
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "TableName");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        logInsert(SQLString, ex);
                        throw ex;
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程  (使用该方法切记要手工关闭SqlDataReader和连接)
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(SH_CMSConnectionString);
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            //Connection.Close(); 不能在此关闭，否则，返回的对象将无法使用            
            return returnReader;

        }

        /// <summary>
        /// 执行存储过程  (使用该方法切记要手工关闭SqlDataReader和连接)
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static DataSet RunProcedure(string storedProcName)
        {
            SqlConnection connection = new SqlConnection(SH_CMSConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteReader();


            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds, "TableName");
                    command.Parameters.Clear();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    logInsert(storedProcName, ex);
                }
                finally
                {
                    connection.Close();
                }
                return ds;
            }



        }










        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {

                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = 1800; //设置超时为30分钟
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = Times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }


        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }







        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion




        /// <summary>
        /// 设置SqlCommand的参数
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdText">Sql语句</param>        
        private static void SqlPrepareCommand(SqlCommand cmd, CommandType cmdType, string cmdText)
        {


            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                try
                {
                    connection.Open();

                    if (cmd != null)
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = cmdText;
                        cmd.CommandType = cmdType;
                    }
                }

                catch (SqlException ex)
                {
                    connection.Close();
                    logInsert(cmdText, ex);
                }
            }
        }



        /// <summary>
        /// 把一个DataSet更新入数据库（可以修改，添加，删除）
        /// 但是，这个表的设计中必须要有主键
        /// </summary>
        /// <param name="cmdText">select语句，和要更新的表对应</param>
        /// <param name="ds">要更新入数据库的DataSet</param>
        /// <param name="TableName">DataSet中的源表名</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteUpdate(string cmdText, DataSet ds, string TableName)
        {
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                connection.Open();
                SqlTransaction myTrans;
                myTrans = connection.BeginTransaction();
                SqlDataAdapter adp = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(cmdText, connection, myTrans);
                try
                {
                    //SqlPrepareCommand(cmd, CommandType.Text, cmdText);
                    adp.SelectCommand = cmd;
                    SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                    builder.QuotePrefix = "[";
                    builder.QuoteSuffix = "]";
                    int i = adp.Update(ds, TableName);
                    myTrans.Commit();
                    ds.AcceptChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    myTrans.Rollback();
                    logInsert(cmdText, ex);
                    return 0;
                }
                finally
                {

                    cmd.Dispose();
                    adp.Dispose();
                }
            }
        }




        /// <summary>
        ///  更新单个表操作,使用了事务,最后接受了改变
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public static bool ExecuteOneTableTrans(string TableName, DataSet ds)
        {
            bool rs = false;
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                connection.Open();
                SqlTransaction myTrans;
                myTrans = connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand("select * from " + TableName + " where  1=2", connection, myTrans);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                adp.UpdateBatchSize = 100;
                SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                builder.QuotePrefix = "[";
                builder.QuoteSuffix = "]";



                DataTable dtAdded = new DataTable();            //存放表 新增 的数据
                DataTable dstMidified = new DataTable();          //存储表  编辑 的行记录
                DataTable dtDeleted = new DataTable();         //存储表 删除 的行记录

                dtAdded = ds.Tables[TableName].GetChanges(DataRowState.Added);   //取得表中新增的行记录集
                dstMidified = ds.Tables[TableName].GetChanges(DataRowState.Modified);  //取得表中编辑的行记录集
                dtDeleted = ds.Tables[TableName].GetChanges(DataRowState.Deleted);  //取得表中删除的行记录集          

                try
                {

                    //先删除
                    if (dtDeleted != null)
                    {
                        adp.Update(dtDeleted);
                    }


                    //新增
                    if (dtAdded != null)
                    {
                        adp.Update(dtAdded);
                    }
                    //lock (this)            //处理并发情况(分布式情况)
                    //{

                    //    myAdapter.Update(ds, strTblName);

                    //}

                    //再修改

                    if (dstMidified != null)
                    {
                        adp.Update(dstMidified);
                    }
                    myTrans.Commit();
                    rs = true;
                    ds.AcceptChanges();
                }
                catch (Exception ex)
                {
                    myTrans.Rollback();
                    logInsert(TableName, ex);
                }
                finally
                {

                    ds.Dispose();
                    cmd.Dispose();
                    adp.Dispose();
                    myTrans.Dispose();
                    GC.Collect();

                }
                return rs;
            }
        }


        /// <summary>
        ///  更新单个表操作,使用了事务,更新后。DataSet没有接受改变
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public static int ExecuteBatchOneTableTransNoAcceptChanges(string TableName, DataSet ds)
        {
            int rs = 0;
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                connection.Open();
                SqlTransaction myTrans;
                myTrans = connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand("select * from " + TableName + " where  1=2", connection, myTrans);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                adp.UpdateBatchSize = 100;
                SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                builder.QuotePrefix = "[";
                builder.QuoteSuffix = "]";

                DataTable dtAdded = new DataTable();            //存放表 新增 的数据
                DataTable dstMidified = new DataTable();          //存储表  编辑 的行记录
                DataTable dtDeleted = new DataTable();         //存储表 删除 的行记录

                dtAdded = ds.Tables[TableName].GetChanges(DataRowState.Added);   //取得表中新增的行记录集
                dstMidified = ds.Tables[TableName].GetChanges(DataRowState.Modified);  //取得表中编辑的行记录集
                dtDeleted = ds.Tables[TableName].GetChanges(DataRowState.Deleted);  //取得表中删除的行记录集          

                try
                {

                    //先删除
                    if (dtDeleted != null)
                    {
                        rs = adp.Update(dtDeleted);
                    }


                    //新增
                    if (dtAdded != null)
                    {
                        rs = adp.Update(dtAdded);
                    }


                    //再修改
                    if (dstMidified != null)
                    {
                        rs = adp.Update(dstMidified);
                    }
                    myTrans.Commit();
                }
                catch (Exception ex)
                {
                    myTrans.Rollback();

                    //if (TableName == "tb_VipInfo")//如果为会员记记录一下
                    //{
                    //    InserVipinforTwo(dtAdded);
                    //}

                    logInsert(ex, TableName + " 插入重复的值失败 " + dtAdded.Rows.Count.ToString() + "行 " + dtAdded.Rows[0][0].ToString());

                }
                finally
                {

                    ds.Dispose();
                    cmd.Dispose();
                    adp.Dispose();
                    myTrans.Dispose();
                    GC.Collect();

                }
                return rs;
            }
        }









        /// <summary>
        /// 保存主从表 完成后没有AcceptChanges
        /// </summary>
        /// <param name="MaincmdText"></param>
        /// <param name="DetailcmdText"></param>
        /// <param name="dsMain"></param>
        /// <param name="dsDetail"></param>
        /// <returns></returns>
        public static int ExecuteSaveMainDetailTrans(string MaintableName, string DetailtableName, DataSet ds)
        {
            int rs = 0;
            using (SqlConnection connection = new SqlConnection(SH_CMSConnectionString))
            {
                connection.Open();

                SqlTransaction myTrans;
                myTrans = connection.BeginTransaction();

                SqlDataAdapter adp = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("select * from " + MaintableName + " where  1=2", connection, myTrans);
                adp.SelectCommand = cmd;
                adp.UpdateBatchSize = 100;// 批次更新的大小.记录条数
                SqlDataAdapter adpDetail = new SqlDataAdapter();
                SqlCommand cmdDetail = new SqlCommand("select * from " + DetailtableName + " where  1=2", connection, myTrans);
                adpDetail.SelectCommand = cmdDetail;
                adpDetail.UpdateBatchSize = 100;// 批次更新的大小.记录条数
                SqlCommandBuilder builder = new SqlCommandBuilder();
                builder.QuotePrefix = "[";
                builder.QuoteSuffix = "]";
                builder.DataAdapter = adp;


                SqlCommandBuilder builderDetail = new SqlCommandBuilder();
                builderDetail.QuotePrefix = "[";
                builderDetail.QuoteSuffix = "]";
                builderDetail.DataAdapter = adpDetail;


                try
                {
                    DataTable dsMainAdded = new DataTable();            //存放主表 新增 的数据
                    DataTable dsMainMidified = new DataTable();          //存储主表  编辑 的行记录
                    DataTable dsMainDeleted = new DataTable();         //存储主表 删除 的行记录

                    DataTable dsDetailAdded = new DataTable();            //存放明细表 新增 的数据
                    DataTable dsDetailMidified = new DataTable();          //存储明细表  编辑 的行记录
                    DataTable dsDetailDeleted = new DataTable();         //存储明细表 删除 的行记录

                    dsMainAdded = ds.Tables[MaintableName].GetChanges(DataRowState.Added);   //取得主表中新增的行记录集
                    dsMainMidified = ds.Tables[MaintableName].GetChanges(DataRowState.Modified);  //取得主表中编辑的行记录集
                    dsMainDeleted = ds.Tables[MaintableName].GetChanges(DataRowState.Deleted);  //取得主表中删除的行记录集          

                    dsDetailAdded = ds.Tables[DetailtableName].GetChanges(DataRowState.Added);   //取得明细表中新增的行记录集
                    dsDetailMidified = ds.Tables[DetailtableName].GetChanges(DataRowState.Modified);  //取得明细表中编辑的行记录集
                    dsDetailDeleted = ds.Tables[DetailtableName].GetChanges(DataRowState.Deleted);  //取得明细表中删除的行记录集          
                    adp.FillSchema(ds, SchemaType.Mapped);
                    adpDetail.FillSchema(ds, SchemaType.Mapped);

                    builder.GetInsertCommand();
                    builder.GetUpdateCommand();
                    builder.GetDeleteCommand();


                    builderDetail.GetInsertCommand();
                    builderDetail.GetUpdateCommand();
                    builderDetail.GetDeleteCommand();


                    //再删除
                    if (dsDetailDeleted != null)
                    {
                        adpDetail.Update(dsDetailDeleted);
                    }

                    if (dsMainDeleted != null)
                    {
                        rs = adp.Update(dsMainDeleted);
                    }

                    //dsMainAdded
                    if (dsMainAdded != null)
                    {
                        rs = adp.Update(dsMainAdded);
                    }

                    if (dsDetailAdded != null)
                    {
                        adpDetail.Update(dsDetailAdded);
                    }

                    //再修改
                    if (dsMainMidified != null)
                    {

                        rs = adp.Update(dsMainMidified);

                    }


                    if (dsDetailMidified != null)
                    {
                        adpDetail.Update(dsDetailMidified);
                    }

                    myTrans.Commit();

                }
                catch (Exception ex)
                {
                    myTrans.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    adp.Dispose();
                    myTrans.Dispose();

                }
                return rs;
            }
        }


        #region 测试
        public bool insertProduct(string SELECTText, string INSERTText)
        {

            using (SqlConnection sqlConn = new SqlConnection(SH_CMSConnectionString))
            {
                sqlConn.Open();

                // Start Transaction
                SqlTransaction myTrans = sqlConn.BeginTransaction();

                SqlDataAdapter sqlda = new SqlDataAdapter();
                sqlda.SelectCommand = new SqlCommand(SELECTText, sqlConn, myTrans);
                sqlda.InsertCommand = new SqlCommand(INSERTText, sqlConn, myTrans);

                try
                {
                    sqlda.InsertCommand.ExecuteNonQuery();
                    myTrans.Commit();
                }
                catch (Exception e)
                {

                    myTrans.Rollback();

                    logInsert(e);


                    return false;
                }
                finally
                {
                    //if (sqlConn.State != ConnectionState.Closed)
                    //{
                    //    sqlConn.Close();
                    //}
                }

                return true;
            }

        }

        #endregion


        /// <summary>
        /// 执行脚本文件带GO
        /// </summary>
        /// <param name="SH_CMSConnectionString"></param>
        /// <param name="txtSQL"></param>
        public static void ExecuteSqlScript(string SH_CMSConnectionString, string txtSQL)
        {
            string[] SqlLine;
            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            SqlLine = regex.Split(txtSQL);

            using (SqlConnection sqlCon = new SqlConnection(SH_CMSConnectionString))
            {
                if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();
                sqlCon.Open();

                SqlCommand cmd = sqlCon.CreateCommand();
                cmd.Connection = sqlCon;

                foreach (string line in SqlLine)
                {
                    if (line.Length > 0)
                    {
                        cmd.CommandText = line;
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            //rollback test       
                            logInsert(ex);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存sql错误日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="SQLString"></param>
        private static void logInsert(string SQLString, Exception ex)
        {
            Helper.log4netHelper.error("MSSQLheler:" + SQLString, ex);
        }


        /// <summary>
        /// 保存sql错误日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="SQLString"></param>
        private static void logInsert(Exception ex, string SQLString)
        {
            Helper.log4netHelper.error("MSSQLheler:" + SQLString, ex);
        }


        /// <summary>
        /// 保存sql错误日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="SQLString"></param>
        private static void logInsert(Exception ex)
        {
            Helper.log4netHelper.error("MSSQLheler", ex);
        }

    }

}
