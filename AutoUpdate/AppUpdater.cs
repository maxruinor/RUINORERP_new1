using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace AutoUpdate
{
    /// <summary>
    /// 应用程序更新器
    /// 负责处理应用程序的更新检查、下载和安装
    /// </summary>
    public class AppUpdater : IDisposable
    {
        // 添加SkipVersionManager实例
        private SkipVersionManager skipVersionManager;
        public SkipVersionManager SkipVersionManager
        {
            get { return skipVersionManager ?? (skipVersionManager = new SkipVersionManager()); }
        }

        private string _appId = "";
        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }
        #region 成员变量定义
        private string _updaterUrl;
        private bool disposed = false;
        private Component component = new Component();

        public string UpdaterUrl
        {
            set { _updaterUrl = value; }
            get { return this._updaterUrl; }
        }
        #endregion

        /// <summary>
        /// AppUpdater构造函数
        /// </summary>
        public AppUpdater()
        {
            // 初始化SkipVersionManager
            skipVersionManager = new SkipVersionManager();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    component?.Dispose();
                }
                disposed = true;
            }
        }




        public string NewVersion { get; set; }



        /// <summary>
        /// 检查更新文件
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList">更新文件清单</param>
        /// <returns></returns>
        /// <summary>
        /// 检查是否存在更新且未被跳过
        /// </summary>
        /// <param name="serverXmlFile">服务器XML文件路径</param>
        /// <param name="localXmlFile">本地XML文件路径</param>
        /// <param name="updateFileList">返回的更新文件列表</param>
        /// <param name="appId">应用程序ID</param>
        /// <returns>如果有更新且未被跳过，返回更新文件数量；否则返回0</returns>
        public int CheckForUpdate(string serverXmlFile, string localXmlFile, out Hashtable updateFileList, string appId = "")
        {
            if (!string.IsNullOrEmpty(appId))
            {
                this.AppId = appId;
            }

            // 调用原始的检查逻辑

            updateFileList = new Hashtable();
            if (!File.Exists(localXmlFile) || !File.Exists(serverXmlFile))
            {
                return -1;
            }

            XmlFiles serverXmlFiles = new XmlFiles(serverXmlFile);
            XmlFiles localXmlFiles = new XmlFiles(localXmlFile);

            XmlNodeList newNodeList = serverXmlFiles.GetNodeList("AutoUpdater/Files");
            XmlNodeList oldNodeList = localXmlFiles.GetNodeList("AutoUpdater/Files");

            // 获取版本号
            this.NewVersion = serverXmlFiles.GetNodeValue("AutoUpdater/Application/Version").ToString();

            // 检查版本是否被跳过（支持强制更新）
            bool forceUpdate = IsCommandLineArgumentPresent("--force");
            if (!forceUpdate && !string.IsNullOrEmpty(appId) && skipVersionManager.IsVersionSkipped(this.NewVersion, appId))
            {
                updateFileList = new Hashtable();
                return 0;
            }

            int k = 0;
            for (int i = 0; i < newNodeList.Count; i++)
            {
                //声明数组保存信息，这里只使用前两个维度，实际可以扩展更多
                string[] fileList = new string[3];

                string newFileName = newNodeList.Item(i).Attributes["Name"].Value.Trim();
                string newVer = newNodeList.Item(i).Attributes["Ver"].Value.Trim();

                ArrayList oldFileAl = new ArrayList();
                for (int j = 0; j < oldNodeList.Count; j++)
                {
                    string oldFileName = oldNodeList.Item(j).Attributes["Name"].Value.Trim();
                    string oldVer = oldNodeList.Item(j).Attributes["Ver"].Value.Trim();

                    oldFileAl.Add(oldFileName);
                    oldFileAl.Add(oldVer);

                }
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }//如果存在，则比较版本号
                else if (pos > -1 && CompareVersion(oldFileAl[pos + 1].ToString(), newVer) < 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }

            }
            return k;
        }




        /// <summary>
        /// 计算文件的MD5哈希值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件的MD5哈希值，如果计算失败则返回空字符串</returns>
        public static string CalculateFileHash(string filePath)
        {
            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        byte[] hashBytes = md5.ComputeHash(stream);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < hashBytes.Length; i++)
                        {
                            sb.Append(hashBytes[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 比较两个版本号
        /// CompareVersion("1.0.0.2","1.0.0.11")=-1
        /// 版本号按数字逐段比较
        /// </summary>
        /// <param name="Old_Version">旧版本号</param>
        /// <param name="New_Version">新版本号</param>
        /// <returns>0表示相等，-1表示需要更新，1表示不需要更新</returns>
        /// <summary>
        /// 检查版本是否被跳过
        /// </summary>
        /// <param name="version">要检查的版本号</param>
        /// <param name="appId">应用程序ID</param>
        /// <returns>如果版本被跳过返回true，否则返回false</returns>
        public bool IsVersionSkipped(string version, string appId)
        {
            return skipVersionManager.IsVersionSkipped(version, appId);
        }

        /// <summary>
        /// 记录跳过的版本
        /// </summary>
        /// <param name="version">要跳过的版本号</param>
        /// <param name="appId">应用程序ID</param>
        public void SkipVersion(string version, string appId)
        {
            skipVersionManager.SkipVersion(version, appId);
        }

        /// <summary>
        /// 检查命令行参数是否存在
        /// </summary>
        /// <param name="argName">参数名称</param>
        /// <returns>如果参数存在则返回true，否则返回false</returns>
        private bool IsCommandLineArgumentPresent(string argName)
        {
            string[] args = Environment.GetCommandLineArgs();
            return args.Any(arg => arg.Equals(argName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 处理压缩更新包
        /// </summary>
        /// <param name="packagePath">压缩包路径</param>
        /// <returns>处理是否成功</returns>
        public bool ProcessCompressedUpdate(string packagePath)
        {
            Debug.WriteLine("警告: ProcessCompressedUpdate 已废弃");
            return false;
        }

        /// <summary>
        /// 检查服务器是否有压缩更新包
        /// </summary>
        /// <returns>如果有压缩更新包返回true，否则返回false</returns>
        public bool CheckForCompressedUpdate()
        {
            return false;
        }

        /// <summary>
        /// 检查是否可以回滚到之前的版本
        /// </summary>
        /// <returns>如果可以回滚返回true，否则返回false</returns>
        public bool CanRollback()
        {
            try
            {
                string backupDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup");
                if (!Directory.Exists(backupDir))
                {
                    return false;
                }

                string[] backupDirs = Directory.GetDirectories(backupDir);
                return backupDirs.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 比较两个版本号
        /// 使用.NET内置的Version类进行版本比较
        /// </summary>
        /// <param name="oldVersion">旧版本号</param>
        /// <param name="newVersion">新版本号</param>
        /// <returns>0表示相等，-1表示需要更新，1表示不需要更新</returns>
        public int CompareVersion(string oldVersion, string newVersion)
        {
            try
            {
                var v1 = new Version(oldVersion);
                var v2 = new Version(newVersion);
                return v1.CompareTo(v2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"版本比较出错: {ex.Message}");
                return 0;
            }
        }



        /// <summary>
        /// 下载自动更新文件到临时目录
        /// </summary>
        public void DownAutoUpdateFile(string downpath)
        {
            if (!System.IO.Directory.Exists(downpath))
                System.IO.Directory.CreateDirectory(downpath);

            //下载从服务器返回的版本信息xml文件
            string serverXmlFile = System.IO.Path.Combine(downpath, "AutoUpdaterList.xml");

            try
            {
                HttpWebRequest request = WebRequest.Create(UpdaterUrl) as HttpWebRequest;
                request.Method = "GET";
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";
                request.KeepAlive = false;
                request.Timeout = 36000;
                
                using (WebResponse response = request.GetResponse())
                using (Stream inStream = response.GetResponseStream())
                using (Stream outStream = File.Create(serverXmlFile))
                {
                    // 【优化】增大缓冲区到 64KB，提升下载速度
                    byte[] buffer = new byte[65536];
                    int bytesRead;
                    
                    while ((bytesRead = inStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            catch (WebException e)
            {
                System.Diagnostics.Debug.WriteLine("下载失败，请联系系统管理员获取更多信息：" + e);
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine("下载失败，请联系系统管理员获取更多信息：" + e);
            }
        }





    }
}
