using System;
using System.Data;
using System.Data.Odbc;
//using Microsoft.Data.Odbc;


namespace MainHead.Common.Data
{
    /// <summary>
    /// MySql 用ODBC 访问数据库。
    /// </summary>
    public class MySqlHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MySqlHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 执行SQL语句(无返回值)
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行的SQL语句</param>
        /// <returns>成功返回true，否则返回false</returns>
        public static bool ExecuteNonQuery(string connectionString, string commandText)
        {
            OdbcConnection odbcConnection = new OdbcConnection(connectionString);
            odbcConnection.Open();

            try
            {
                OdbcCommand odbcCommand = new OdbcCommand("", odbcConnection);
                odbcCommand.CommandText = commandText;
                odbcCommand.ExecuteNonQuery();

                odbcConnection.Close();

                return true;
            }
            catch (Exception exce)
            {
                throw exce;
            }

        }


        /// <summary>
        /// 执行SQL语句(有返回值)
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行的SQL语句</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string connectionString, string commandText)
        {
            OdbcConnection odbcConnection = new OdbcConnection(connectionString);

            OdbcDataAdapter adapter = new OdbcDataAdapter(commandText, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            return dataSet;
        }
    }
}
