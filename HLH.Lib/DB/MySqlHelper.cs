using System;
using System.Data;
using System.Data.Odbc;
//using Microsoft.Data.Odbc;


namespace MainHead.Common.Data
{
    /// <summary>
    /// MySql ��ODBC �������ݿ⡣
    /// </summary>
    public class MySqlHelper
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public MySqlHelper()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }


        /// <summary>
        /// ִ��SQL���(�޷���ֵ)
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="commandText">ִ�е�SQL���</param>
        /// <returns>�ɹ�����true�����򷵻�false</returns>
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
        /// ִ��SQL���(�з���ֵ)
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="commandText">ִ�е�SQL���</param>
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
