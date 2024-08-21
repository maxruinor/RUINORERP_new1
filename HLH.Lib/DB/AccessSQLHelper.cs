using System;
//��ȡ�����ļ�
using System.Configuration;
//access���ݿ�����
using System.Data;
using System.Data.OleDb;

namespace HLH.Lib.DB
{
    public class AccessSQLHelper
    {
        //���ݿ�����
        protected OleDbConnection DbConn;
        protected OleDbCommand DbComm;

        //���ݿ������ַ���
        private string _dbPath;
        public string DbPath
        {
            get
            {
                if ((_dbPath != null) && (_dbPath.Length != 0))
                {
                    return _dbPath;
                }
                else
                {
                    _dbPath = ConfigurationManager.AppSettings["PoliceDataPath"].ToString();
                    return _dbPath;
                }
            }
            set
            {//���Ϊ�շ���
                _dbPath = value;
            }
        }

        //�������ݿ�
        protected void OpenAccConn()
        {
            try
            {
                DbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DbPath);
                DbComm = new OleDbCommand();
                DbComm.Connection = DbConn;
                DbConn.Open();
            }
            catch (OleDbException oleEx)
            {
                throw new Exception(oleEx.Message);
            }
            catch (Exception)
            {
                throw new Exception("���ݿ����ӳ���");
            }
        }

        //�ر����ݿ�
        protected void CloseAccConn()
        {
            try
            {
                if ((DbConn.State == ConnectionState.Open) || (DbConn.State == ConnectionState.Broken))
                {//���ݿ������Ǵ�״̬
                    DbConn.Close();
                }
                DbConn.Dispose();
                DbComm.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ////////////////////////////////////////////////���������ݿ��������///////////////////////////////////////
        /// <summary>
        /// ִ��û�з���ֵ��SQL
        /// </summary>
        /// <param name="tempSql"></param>
        public void ExcuteNonSql(string tempSql)
        {
            try
            {
                OpenAccConn();
                DbComm.CommandType = CommandType.Text;//������������Ϊ�ı�
                DbComm.CommandText = tempSql;
                DbComm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw new Exception("���ݿ����ʧ�ܣ�");
            }
            finally
            {
                CloseAccConn();
            }
        }

        /// <summary>
        /// ִ�з���Ӱ��������SQL
        /// </summary>
        /// <param name="tempSql"></param>
        /// <returns></returns>
        public int ExecuteIntSql(string tempSql)
        {
            try
            {
                OpenAccConn();
                DbComm.CommandType = CommandType.Text;
                DbComm.CommandText = tempSql;
                return DbComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseAccConn();
            }
        }

        /// <summary>
        /// ִ�з���DATASET��SQL
        /// </summary>
        /// <param name="tempSql"></param>
        /// <returns></returns>
        public DataSet ExecuteDSSql(string tempSql)
        {
            OleDbDataAdapter tempAccDA = new OleDbDataAdapter();//ʵ����һ�����ݻ���������
            DataSet tempAccDS = new DataSet();//ʵ����һ�����ݻ�����
            try
            {
                OpenAccConn();
                DbComm.CommandType = CommandType.Text;
                DbComm.CommandText = tempSql;
                tempAccDA.SelectCommand = DbComm;
                tempAccDA.Fill(tempAccDS);
                return tempAccDS;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseAccConn();
            }
        }

        /// <summary>
        /// ִ�з���DATATABLE��SQL
        /// </summary>
        /// <param name="tempSql"></param>
        /// <returns></returns>
        public DataTable ExecuteDTSql(string tempSql)
        {
            OleDbDataAdapter tempAccDA = new OleDbDataAdapter();//ʵ����һ�����ݻ���������
            DataTable tempAccDT = new DataTable();//ʵ����һ�����ݻ�����
            try
            {
                OpenAccConn();
                DbComm.CommandType = CommandType.Text;
                DbComm.CommandText = tempSql;
                tempAccDA.SelectCommand = DbComm;
                tempAccDA.Fill(tempAccDT);
                return tempAccDT;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseAccConn();
            }
        }
    }
}
