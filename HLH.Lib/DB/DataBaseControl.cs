using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HLH.Lib.DB
{
    /// <summary> 
    /// 数据库操作控制类 
    /// <para>
    /// 
    /* 
     ///调用事例： 
  
        还原数据库 
     private void button0_Click(object sender, EventArgs e) 
     { 
     DataBaseControl DBC = new DataBaseControl(); 
     DBC.ConnectionString = "Data Source=(local);User id=sa;Password=123456; Initial Catalog=master"; 
     DBC.DataBaseName = "MyDatabase"; 
     DBC.DataBaseOfBackupName = @"back.bak"; 
     DBC.DataBaseOfBackupPath = @"D:\Program Files\Microsoft SQL Server\MSSQL\Data\"; 
     DBC.ReplaceDataBase(); 
     } 
  
        附加数据库 
     private void button1_Click_1(object sender, EventArgs e) 
     { 
     DataBaseControl DBC = new DataBaseControl(); 
     DBC.ConnectionString = "Data Source=(local);User id=sa;Password=123456; Initial Catalog=master"; 
     DBC.DataBaseName = "MyDatabase"; 
     DBC.DataBase_MDF = @"D:\Program Files\Microsoft SQL Server\MSSQL\Data\MyDatabase_Data.MDF"; 
     DBC.DataBase_LDF = @"D:\Program Files\Microsoft SQL Server\MSSQL\Data\MyDatabase_Log.LDF"; 
     DBC.AddDataBase(); 
     } 
  
        备份数据库 
     private void button2_Click(object sender, EventArgs e) 
     { 
     DataBaseControl DBC = new DataBaseControl(); 
     DBC.ConnectionString = "Data Source=(local);User id=sa;Password=123456; Initial Catalog=master"; 
     DBC.DataBaseName = "MyDatabase"; 
     DBC.DataBaseOfBackupName = @"back.bak"; 
     DBC.DataBaseOfBackupPath = @"D:\Program Files\Microsoft SQL Server\MSSQL\Data\"; 
     DBC.BackupDataBase(); 
     } 
  
        分离数据库 
     private void button3_Click(object sender, EventArgs e) 
     { 
     DataBaseControl DBC = new DataBaseControl(); 
     DBC.ConnectionString = "Data Source=(local);User id=sa;Password=123456; Initial Catalog=master"; 
     DBC.DataBaseName = "MyDatabase"; 
     DBC.DeleteDataBase(); 
     } 
  
     */

    //在C#中运用SQLDMO备份和恢复Microsoft SQL Server数据库 
    //SQLDMO(SQL Distributed Management Objects，SQL分布式管理对象)封装了Microsoft SQL Server数据库中的对象。SQLDMO是Microsoft SQL Server中企业管理器所使用的应用程序接口，所以它可以执行很多功能，其中当然也包括对数据库的备份和恢复。

    //SQLDMO由Microsoft SQL Server自带的SQLDMO.dll提供，由于SQLDMO.dll是一个COM对象，所以大家在用之前必须在.NET项目中添加对它的引用。

    //备份SQL SERVER数据库：
    //public static void DBBack()
    //  {
    //   SQLDMO.Backup backup = new SQLDMO.BackupClass();
    //   SQLDMO.SQLServer server = new SQLDMO.SQLServerClass();
    //   try
    //   {
    //    server.LoginSecure = false;
    //    server.Connect("localhost","sa","sa");
    //    backup.Action = SQLDMO.SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
    //    backup.Database = "northwind";
    //    backup.Files = @"d:\Back\northwind.bak";
    //    backup.BackupSetName = "northwind";
    //    backup.BackupSetDescription = "Northwind数据库备份";
    //    backup.Initialize = true;
    //    backup.SQLBackup(server);
    //   }
    //   catch(Exception x)
    //   {
    //    throw x;
    //   }
    //   finally
    //   {
    //    server.DisConnect();
    //   }
    //  }

    //恢复SQL SERVER数据库：
    //public static void DBRestore()
    //  { 
    //   SQLDMO.Restore restore = new SQLDMO.RestoreClass();
    //   SQLDMO.SQLServer server = new SQLDMO.SQLServerClass();
    //   try
    //   {
    //    server.LoginSecure = false;
    //    server.Connect("localhost","sa","sa");
    //    restore.Action = SQLDMO.SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
    //    restore.Database = "northwind";
    //    restore.Files = @"D:\Back\Northwind.bak";
    //    restore.FileNumber = 1;
    //    restore.ReplaceDatabase = true;
    //    restore.SQLRestore(server);
    //   }
    //   catch(Exception x)
    //   {
    //    throw x;
    //   }
    //   finally
    //   {
    //    server.DisConnect();
    //   }
    //  }

    /// </para>
    /// </summary> 
    public class DataBaseControl
    {
        /// <summary>
        /// 要杀死的数据库名称
        /// </summary>
        public string KilledDataBaseName;

        /// <summary> 
        /// 数据库连接字符串 
        /// </summary> 
        public string ConnectionString;

        /// <summary> 
        /// SQL操作语句/存储过程 
        /// </summary> 
        public string StrSQL;

        /// <summary> 
        /// 实例化一个数据库连接对象 
        /// </summary> 
        private SqlConnection Conn;

        /// <summary> 
        /// 实例化一个新的数据库操作对象Comm 
        /// </summary> 
        private SqlCommand Comm;

        /// <summary> 
        /// 要操作的数据库名称 
        /// </summary> 
        public string DataBaseName;

        /// <summary> 
        /// 数据库文件完整地址 
        /// </summary> 
        public string DataBase_MDF;

        /// <summary> 
        /// 数据库日志文件完整地址 
        /// </summary> 
        public string DataBase_LDF;

        /// <summary> 
        /// 备份文件名 
        /// </summary> 
        public string DataBaseOfBackupName;

        /// <summary> 
        /// 备份文件路径 
        /// </summary> 
        public string DataBaseOfBackupPath;

        /// <summary> 
        /// 执行创建/修改数据库和表的操作 
        /// </summary> 
        public void DataBaseAndTableControl()
        {
            try
            {
                Conn = new SqlConnection(ConnectionString);
                Conn.Open();

                Comm = new SqlCommand();
                Comm.Connection = Conn;
                Comm.CommandText = StrSQL;
                Comm.CommandType = CommandType.Text;
                Comm.ExecuteNonQuery();

                MessageBox.Show("数据库操作成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary> 
        /// 附加数据库 
        /// </summary> 
        public void AddDataBase()
        {
            try
            {
                Conn = new SqlConnection(ConnectionString);
                Conn.Open();

                Comm = new SqlCommand();
                Comm.Connection = Conn;
                Comm.CommandText = "sp_attach_db";

                Comm.Parameters.Add(new SqlParameter(@"dbname", SqlDbType.NVarChar));
                Comm.Parameters[@"dbname"].Value = DataBaseName;
                Comm.Parameters.Add(new SqlParameter(@"filename1", SqlDbType.NVarChar));
                Comm.Parameters[@"filename1"].Value = DataBase_MDF;
                Comm.Parameters.Add(new SqlParameter(@"filename2", SqlDbType.NVarChar));
                Comm.Parameters[@"filename2"].Value = DataBase_LDF;
                Comm.CommandTimeout = 180; //三分钟
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.ExecuteNonQuery();

                MessageBox.Show("附加数据库成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary> 
        /// 分离数据库 
        /// </summary> 
        public void DeleteDataBase()
        {
            try
            {
                Conn = new SqlConnection(ConnectionString);
                Conn.Open();

                Comm = new SqlCommand();
                Comm.Connection = Conn;
                Comm.CommandText = @"sp_detach_db";

                Comm.Parameters.Add(new SqlParameter(@"dbname", SqlDbType.NVarChar));
                Comm.Parameters[@"dbname"].Value = DataBaseName;

                Comm.CommandType = CommandType.StoredProcedure;
                Comm.ExecuteNonQuery();

                MessageBox.Show("分离数据库成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary> 
        /// 备份数据库 
        /// </summary> 
        public void BackupDataBase()
        {
            try
            {
                Conn = new SqlConnection(ConnectionString);
                Conn.Open();

                Comm = new SqlCommand();
                Comm.Connection = Conn;
                Comm.CommandText = "use master;backup database @dbname to disk = @backupname;";

                Comm.Parameters.Add(new SqlParameter(@"dbname", SqlDbType.NVarChar));
                Comm.Parameters[@"dbname"].Value = DataBaseName;
                Comm.Parameters.Add(new SqlParameter(@"backupname", SqlDbType.NVarChar));
                Comm.Parameters[@"backupname"].Value = @DataBaseOfBackupPath + @DataBaseOfBackupName;

                Comm.CommandType = CommandType.Text;
                Comm.CommandTimeout = 300; //五分钟  规格秒
                Comm.ExecuteNonQuery();

                MessageBox.Show("备份数据库成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Conn.Close();
            }
        }


        /// <summary> 
        /// 备份数据库 
        /// </summary> 
        public void BackupDataBase(bool IsShowMessage)
        {
            try
            {
                Conn = new SqlConnection(ConnectionString);
                Conn.Open();

                Comm = new SqlCommand();
                Comm.Connection = Conn;
                Comm.CommandText = "use master;backup database @dbname to disk = @backupname;";

                Comm.Parameters.Add(new SqlParameter(@"dbname", SqlDbType.NVarChar));
                Comm.Parameters[@"dbname"].Value = DataBaseName;
                Comm.Parameters.Add(new SqlParameter(@"backupname", SqlDbType.NVarChar));
                Comm.Parameters[@"backupname"].Value = @DataBaseOfBackupPath + @DataBaseOfBackupName;

                Comm.CommandType = CommandType.Text;
                Comm.CommandTimeout = 300; //五分钟  规格秒
                Comm.ExecuteNonQuery();
                if (IsShowMessage)
                {
                    MessageBox.Show("备份数据库成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Conn.Close();
            }
        }


        /// <summary> 
        /// 还原数据库 
        /// </summary> 
        public void ReplaceDataBase()
        {
            try
            {
                string BackupFile = @DataBaseOfBackupPath + @DataBaseOfBackupName;
                Conn = new SqlConnection(ConnectionString);
                Conn.Open();

                Comm = new SqlCommand();
                Comm.Connection = Conn;
                Comm.CommandText = "use master;restore database @DataBaseName From disk = @BackupFile with replace;";

                Comm.Parameters.Add(new SqlParameter(@"DataBaseName", SqlDbType.NVarChar));
                Comm.Parameters[@"DataBaseName"].Value = DataBaseName;
                Comm.Parameters.Add(new SqlParameter(@"BackupFile", SqlDbType.NVarChar));
                Comm.Parameters[@"BackupFile"].Value = BackupFile;
                Comm.CommandTimeout = 180; //三分钟
                Comm.CommandType = CommandType.Text;
                Comm.ExecuteNonQuery();

                MessageBox.Show("还原数据库成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Conn.Close();
            }
        }


        /// <summary>
        /// 杀死指定数据库
        /// </summary>
        public int KillDataBase(string ConnectionString)
        {
            int rs = 0;
            try
            {
                string strSQL = "select spid from master..sysprocesses where dbid=db_id('" + KilledDataBaseName + "')";
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                SqlDataAdapter Da = new SqlDataAdapter(strSQL, conn);
                DataSet ds = new DataSet();
                Da.Fill(ds, "tablename");

                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandType = CommandType.Text;
                Cmd.Connection = conn;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Cmd.CommandText = "kill " + dr[0].ToString(); //强行关闭用户进程
                    Cmd.ExecuteNonQuery();
                }

                Cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {

            }
            return rs;
        }


        ///// <summary>
        ///// SQLDMO数据库备份
        ///// </summary>
        //public static void SQLDMODbBackup()
        //{
        //    SQLDMO.Backup oBackup = new SQLDMO.BackupClass();
        //    SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
        //    try
        //    {
        //        oSQLServer.LoginSecure = false;
        //        oSQLServer.Connect("localhost", "sa", "1234");
        //        oBackup.Action = SQLDMO.SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
        //        oBackup.Database = "Northwind";
        //        oBackup.Files = @"d:\Northwind.bak";
        //        oBackup.BackupSetName = "Northwind";
        //        oBackup.BackupSetDescription = "数据库备份";
        //        oBackup.Initialize = true;
        //        oBackup.SQLBackup(oSQLServer);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        oSQLServer.DisConnect();
        //    }
        //}

        // /// <summary>
        // /// 数据库恢复
        // /// </summary>
        //public static void SQLDMODbRestore()
        //{
        //    SQLDMO.Restore restore = new SQLDMO.RestoreClass();
        //    SQLDMO.SQLServer sqlServer = new SQLDMO.SQLServerClass();
        //    try
        //    {
        //        sqlServer.LoginSecure = false;
        //        sqlServer.Connect(Environment.MachineName, "sa", "zzb");
        //        string strSQL = "select spid from master..sysprocesses where dbid=db_id('" + ConfigurationManager.AppSettings["DBName"] + "')";
        //        SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        //        SqlDataAdapter Da = new SqlDataAdapter(strSQL, conn);
        //        DataTable spidTable = new DataTable();
        //        Da.Fill(spidTable);
        //        for (int iRow = 0; iRow <= spidTable.Rows.Count - 1; iRow++)
        //        {
        //            sqlServer.KillProcess(int.Parse(spidTable.Rows[iRow][0].ToString()));
        //        }
        //        restore.Action = SQLDMO.SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
        //        restore.Database = ConfigurationManager.AppSettings["DBName"];
        //        restore.Files = Server.MapPath(".." + ConfigurationManager.AppSettings["BackPath"]) + backPath.Text.Trim().ToString();
        //        restore.FileNumber = 1;
        //        restore.ReplaceDatabase = true;
        //        restore.SQLRestore(sqlServer);
        //        MessageBox.Show("还原数据库成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    finally
        //    {
        //        sqlServer.DisConnect();
        //    }
        //}






    }
}
