using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
namespace HLH.Lib.DB
{

    public class SQLiteHelper : IDisposable
    {
        private SQLiteConnection _connection;
        private string _dataSource = string.Empty;
        private static SQLiteHelper _instance = new SQLiteHelper();
        private bool _isFirstUse = false;
        private Dictionary<int, SQLiteTransaction> _localTransactionCollection;
        private static object _locker = new object();
        private string _password = string.Empty;
        private static int _refCount = 0;
        private static string sqLiteDatabasePathName = string.Empty;




        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {

                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }



        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SQLiteParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            string m_strTempConnectionString = "Data Source={0};Version = 3";
            m_strTempConnectionString = string.Format(m_strTempConnectionString, SqLiteDatabasePathName);
            using (SQLiteConnection conn = new SQLiteConnection(m_strTempConnectionString))
            {

                conn.Open();
                using (SQLiteTransaction trans = conn.BeginTransaction())
                {
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SQLiteParameter[] cmdParms = (SQLiteParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
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
            string m_strTempConnectionString = "Data Source={0};Version = 3";
            m_strTempConnectionString = string.Format(m_strTempConnectionString, SqLiteDatabasePathName);

            using (SQLiteConnection conn = new SQLiteConnection(m_strTempConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
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
                catch (System.Data.SQLite.SQLiteException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }



        /// <summary>
        /// sqlite文件路径带文件名
        /// </summary>
        public static string SqLiteDatabasePathName
        {
            get { return sqLiteDatabasePathName; }
            set { sqLiteDatabasePathName = value; }
        }

        private SQLiteHelper()
        {

        }

        public void CommitTransaction()
        {
            lock (_locker)
            {
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                if (this.LocalTransactionCollection.ContainsKey(managedThreadId))
                {
                    this.LocalTransactionCollection[managedThreadId].Commit();
                    _refCount--;
                    this.LocalTransactionCollection.Remove(managedThreadId);
                    if (_refCount == 0)
                    {
                        this._connection.Close();
                    }
                }
            }
        }

        public SQLiteCommand CreateCommand(string sql, List<SQLiteParameter> parameters)
        {
            SQLiteCommand command = null;
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
            if (this.LocalTransactionCollection.ContainsKey(managedThreadId) && (this.LocalTransactionCollection[managedThreadId] != null))
            {
                command = new SQLiteCommand(sql, this._connection, this.LocalTransactionCollection[managedThreadId]);
            }
            else
            {
                command = new SQLiteCommand(sql, this._connection);
            }
            if (parameters != null)
            {
                foreach (SQLiteParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }


        /// <summary>
        /// 这个方法是将扩大了传入参数的类型 在这里再缩小处理，没必要
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public List<SQLiteParameter> DeriveParameters(string commandText, object[] paramList)
        {
            if (paramList == null)
            {
                return null;
            }
            List<SQLiteParameter> list = new List<SQLiteParameter>();
            if (paramList.Length == 0)
            {
                return list;
            }



            string input = commandText.Substring(commandText.IndexOf("@")).Replace(",", " ,").Replace(")", " )");
            string pattern = @"(@)\S*(.*?)\b";
            MatchCollection matchs = new Regex(pattern, RegexOptions.IgnoreCase).Matches(input);
            List<string> list2 = new List<string>();
            foreach (Match match in matchs)
            {
                if (!list2.Contains(match.Value))
                {
                    list2.Add(match.Value);
                }
            }
            string[] strArray = list2.ToArray();
            int index = 0;
            //Type type = null;
            System.Data.Common.DbParameter temppara = null;
            foreach (object obj2 in paramList)
            {
                if (obj2 == null)
                {
                    SQLiteParameter item = new SQLiteParameter();
                    item.DbType = DbType.Object;
                    item.ParameterName = strArray[index];
                    item.Value = DBNull.Value;
                    list.Add(item);
                }
                else
                {

                    if (obj2 is SQLiteParameter)
                    {
                        temppara = (SQLiteParameter)obj2;
                        list.Add(temppara as SQLiteParameter);
                        continue;
                    }
                    else
                    {
                        temppara = (System.Data.Common.DbParameter)obj2;
                        //type = obj2.GetType();
                    }



                    SQLiteParameter parameter2 = new SQLiteParameter();


                    switch (temppara.DbType.ToString())//(type.ToString())
                    {
                        case "System.String":
                            parameter2.DbType = DbType.String;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (string)paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Byte[]":
                            parameter2.DbType = DbType.Binary;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (byte[])paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Int64":
                            parameter2.DbType = DbType.Int64;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (long)paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Int32":
                        case "Int32":
                            parameter2.DbType = DbType.Int32;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (int)paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Boolean":
                            parameter2.DbType = DbType.Boolean;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (bool)paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.DateTime":
                            parameter2.DbType = DbType.DateTime;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = Convert.ToDateTime(paramList[index]);
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Double":
                            parameter2.DbType = DbType.Double;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = Convert.ToDouble(paramList[index]);
                            list.Add(parameter2);
                            goto Label_0408;

                        case "System.Decimal":
                            parameter2.DbType = DbType.Decimal;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = Convert.ToDecimal(paramList[index]);
                            goto Label_0408;

                        case "System.Guid":
                            parameter2.DbType = DbType.Guid;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = (Guid)paramList[index];
                            goto Label_0408;

                        case "System.Object":
                            parameter2.DbType = DbType.Object;
                            parameter2.ParameterName = strArray[index];
                            parameter2.Value = paramList[index];
                            list.Add(parameter2);
                            goto Label_0408;
                    }
                    throw new SystemException("Value is of unknown data type");
                }
            Label_0408:
                index++;
            }
            return list;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposed)
        {
            try
            {
                if (disposed)
                {
                    if (this._localTransactionCollection != null)
                    {
                        lock (_locker)
                        {
                            foreach (SQLiteTransaction transaction in this._localTransactionCollection.Values)
                            {
                                try
                                {
                                    transaction.Rollback();
                                    transaction.Dispose();
                                    continue;
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            this._localTransactionCollection.Clear();
                            this._localTransactionCollection = null;
                        }
                    }
                    if (this._connection != null)
                    {
                        this._connection.Close();
                        this._connection.Dispose();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                this._connection = null;
            }
        }

        public void EnableConnection()
        {

            if (this._connection == null)
            {
                // string connectionString = string.Format("Data Source={0};Password={1}", this._dataSource, this._password);
                this._connection = new SQLiteConnection(ConnectionString);
                // 密码已在连接字符串中设置，不需要单独设置

            }
            if (this._connection.State == ConnectionState.Closed)
            {
                this._connection.Open();

            }
        }

        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        public int ExecuteNonQuery(string sql, List<SQLiteParameter> parameters)
        {
            this.EnableConnection();
            return this.CreateCommand(sql, parameters).ExecuteNonQuery();
        }

        public SQLiteDataReader ExecuteReader(string sql, List<SQLiteParameter> parameters)
        {
            this.EnableConnection();
            return this.CreateCommand(sql, parameters).ExecuteReader();
        }
        public int ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }

        public int ExecuteScalar(string sql, List<SQLiteParameter> parameters)
        {
            this.EnableConnection();
            object obj2 = this.CreateCommand(sql, parameters).ExecuteScalar();
            if (obj2 == DBNull.Value)
                return 1;
            if (obj2 != null)
            {
                return int.Parse(obj2.ToString());
            }
            return 1;
        }

        public object GetSingle(string SQLString, List<SQLiteParameter> cmdParms)
        {
            this.EnableConnection();
            return this.CreateCommand(SQLString, cmdParms).ExecuteScalar();
        }

        public object GetSingle(string SQLString)
        {
            return GetSingle(SQLString, null);
        }


        /// <summary>
        /// 这个方法不太好  可能会出现  DataTable 已属于另一个 DataSet。这种错误  能不能？？ 。copy()
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(GetDataTable(sql));
            return ds;
        }

        public DataTable GetDataTable(string sql)
        {
            return GetDataTable(sql, null);
        }

        public DataTable GetDataTable(string sql, List<SQLiteParameter> parameters)
        {
            //this.EnableConnection();

            //DataTable dt = new DataTable();
            //SQLiteDataReader reader = this.ExecuteReader(sql, parameters);
            //dt.Load(reader);
            //reader.Close();
            //return dt;


            this.EnableConnection();

            SQLiteCommand cmd = new SQLiteCommand();
            cmd = this.CreateCommand(sql, parameters);
            //create the DataAdapter & DataSet
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            //da.FillSchema(ds, SchemaType.Source);
            da.Fill(ds);
            if (ds.Tables.Count == 0)
            {
                da.FillSchema(ds, SchemaType.Source);
            }
            cmd.Parameters.Clear();
            return ds.Tables[0];
        }

        public bool Exists(string strSql)
        {
            return Exists(strSql, null);
        }

        public bool Exists(string strSql, List<SQLiteParameter> cmdParms)
        {
            int cmdresult = ExecuteScalar(strSql, cmdParms);

            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;

            return ExecuteScalar(strsql);
        }

        ~SQLiteHelper()
        {
            this.Dispose(false);
        }

        public string ConnectionString
        {
            get
            {
                return SetConnectionString();
            }
        }

        protected string SetConnectionString()
        {
            if (string.IsNullOrEmpty(SqLiteDatabasePathName))
            {
                throw new System.Exception("请设置权限数据库的全路径名称属性!");
            }


            string m_strTempConnectionString = "Data Source={0};Version = 3";
            m_strTempConnectionString = string.Format(m_strTempConnectionString, SqLiteDatabasePathName);

            if (string.IsNullOrEmpty(m_strTempConnectionString))
                throw new System.Exception("未配置数据库连接字符串!");

            string[] m_strConnStr = m_strTempConnectionString.Split(';');
            foreach (string str in m_strConnStr)
            {
                string[] m_strs = str.Split('=');
                if (m_strs[0] == "Data Source")
                    this._dataSource = m_strs[1];
                if (m_strs[0] == "Password")
                    this._password = m_strs[1];
            }
            if (string.IsNullOrEmpty(this._dataSource))
                throw new System.Exception("未配置数据库连接字符串的Data Source的值!");

            //  this._dataSource = Path.Combine(CfgSystemDirectory, this._dataSource);
            if (!string.IsNullOrEmpty(this._password))
                return string.Format("Data Source={0};Password={1}", this._dataSource, this._password);
            else
                return string.Format("Data Source={0}", this._dataSource);

        }

        /// <summary>
        /// 服务运行的目录,程序运行的目录,Web运行的目录 
        /// </summary>
        public string CfgSystemDirectory
        {
            get
            {
                string m_CfgSystemDirectory = string.Empty;

                if (CheckWhetherIsWeb())
                    m_CfgSystemDirectory = System.Web.HttpContext.Current.Server.MapPath("~/");
                else
                {
                    string m_strLine = @"\";
                    string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    path = path.Substring(0, path.LastIndexOf(m_strLine));

                    m_CfgSystemDirectory = path;
                }
                return m_CfgSystemDirectory;
            }
        }
        /// <summary>
        /// true:Web Form ;  false:非非Web Form方式
        /// </summary>
        /// <returns></returns>
        private static bool CheckWhetherIsWeb()
        {
            bool result = false;
            AppDomain domain = AppDomain.CurrentDomain;
            try
            {
                if (domain.ShadowCopyFiles)
                    result = (System.Web.HttpContext.Current.GetType() != null);
            }
            catch (System.Exception) { }
            return result;
        }

        public bool InitializeDatabase(string currentUserSid, ref string p_strMsg)
        {
            bool flag;
            lock (_locker)
            {
                if (!this.Disposed)
                {
                    this.Dispose();
                }
                #region 以前的
                //string app = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //app = Path.Combine(app, "Fetion");
                //string path = Path.Combine(app, currentUserSid);
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //this._dataSource = Path.Combine(path, DATABASE_NAME);
                //this._password = currentUserSid;

                //this._localTransactionCollection = new Dictionary<int, SQLiteTransaction>();
                //try
                //{
                //    if (!File.Exists(this._dataSource))
                //    {
                //        SQLiteConnection.CreateFile(this._dataSource);
                //        string connectionString = string.Format("Data Source={0};Password={1}", this._dataSource, this._password);
                //        this._connection = new SQLiteConnection(connectionString);
                //        this._connection.SetPassword(this._password);
                //    }
                //    flag = true;
                //}
                //catch
                //{
                //    this.Dispose();
                //    File.Delete(this._dataSource);
                //    flag = false;
                //}
                #endregion

                this._localTransactionCollection = new Dictionary<int, SQLiteTransaction>();
                try
                {
                    if (!File.Exists(this._dataSource))
                    {
                        this._connection = new SQLiteConnection(ConnectionString);
                        SQLiteConnection.CreateFile(this._dataSource);
                        // 密码已在连接字符串中设置，不需要单独设置
                    }
                    flag = true;
                }
                catch (System.Exception ex)
                {
                    this.Dispose();
                    if (File.Exists(this._dataSource))
                        File.Delete(this._dataSource);
                    p_strMsg = ex.Message;
                    flag = false;
                }
            }
            return flag;
        }

        public void JoinTransaction()
        {
            lock (_locker)
            {
                this.EnableConnection();
                _refCount++;
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                if (!this.LocalTransactionCollection.ContainsKey(managedThreadId))
                {
                    this.LocalTransactionCollection.Add(managedThreadId, this._connection.BeginTransaction());
                }
            }
        }

        public void RollbackTransaction()
        {
            lock (_locker)
            {
                int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                if (this.LocalTransactionCollection.ContainsKey(managedThreadId))
                {
                    this.LocalTransactionCollection[managedThreadId].Rollback();
                    _refCount--;
                    this.LocalTransactionCollection.Remove(managedThreadId);
                    if (_refCount == 0)
                    {
                        this._connection.Close();
                    }
                }
            }
        }

        public bool Disposed
        {
            get
            {
                return (this._connection != null);
            }
        }

        public static SQLiteHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool IsFirstUse
        {
            get
            {
                return this._isFirstUse;
            }
        }

        private Dictionary<int, SQLiteTransaction> LocalTransactionCollection
        {
            get
            {
                lock (_locker)
                {
                    if (this._localTransactionCollection == null)
                    {
                        this._localTransactionCollection = new Dictionary<int, SQLiteTransaction>();
                    }
                    return this._localTransactionCollection;
                }
            }
        }

        public List<string> Objects
        {
            get
            {
                lock (_locker)
                {
                    List<string> list = new List<string>();
                    using (SQLiteDataReader reader = this.ExecuteReader("SELECT [Name] FROM [SQLITE_MASTER] WHERE ([type] = 'table') OR ([type] = 'view')", null))
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["name"].ToString());
                        }
                    }
                    return list;
                }
            }
        }
    }

}