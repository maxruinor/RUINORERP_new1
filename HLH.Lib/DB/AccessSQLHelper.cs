using System;
//读取配置文件
using System.Configuration;
//access数据库连接
using System.Data;
using System.Data.OleDb;

namespace HLH.Lib.DB
{
    public class AccessSQLHelper
    {
        //数据库连接
        protected OleDbConnection DbConn;
        protected OleDbCommand DbComm;

        //数据库连接字符串
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
            {//最好为空方法
                _dbPath = value;
            }
        }

        //连接数据库
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
                throw new Exception("数据库连接出错！");
            }
        }

        //关闭数据库
        protected void CloseAccConn()
        {
            try
            {
                if ((DbConn.State == ConnectionState.Open) || (DbConn.State == ConnectionState.Broken))
                {//数据库连接是打开状态
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

        ////////////////////////////////////////////////下面是数据库操作部分///////////////////////////////////////
        /// <summary>
        /// 执行没有返回值的SQL
        /// </summary>
        /// <param name="tempSql"></param>
        public void ExcuteNonSql(string tempSql)
        {
            try
            {
                OpenAccConn();
                DbComm.CommandType = CommandType.Text;//设置命令类型为文本
                DbComm.CommandText = tempSql;
                DbComm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw new Exception("数据库操作失败！");
            }
            finally
            {
                CloseAccConn();
            }
        }

        /// <summary>
        /// 执行返回影响行数的SQL
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
        /// 执行返回DATASET的SQL
        /// </summary>
        /// <param name="tempSql"></param>
        /// <returns></returns>
        public DataSet ExecuteDSSql(string tempSql)
        {
            OleDbDataAdapter tempAccDA = new OleDbDataAdapter();//实例化一个数据缓存适配器
            DataSet tempAccDS = new DataSet();//实例化一个数据缓存器
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
        /// 执行返回DATATABLE的SQL
        /// </summary>
        /// <param name="tempSql"></param>
        /// <returns></returns>
        public DataTable ExecuteDTSql(string tempSql)
        {
            OleDbDataAdapter tempAccDA = new OleDbDataAdapter();//实例化一个数据缓存适配器
            DataTable tempAccDT = new DataTable();//实例化一个数据缓存器
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
