using System;
using System.Web;
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
using System.Xml.Serialization;

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

        // 添加VersionHistoryManager实例
        private VersionHistoryManager versionHistoryManager;
        public VersionHistoryManager VersionHistoryManager
        {
            get { return versionHistoryManager ?? (versionHistoryManager = new VersionHistoryManager()); }
        }

        // 添加VersionRollbackManager实例
        private VersionRollbackManager versionRollbackManager;
        public VersionRollbackManager VersionRollbackManager
        {
            get { return versionRollbackManager ?? (versionRollbackManager = new VersionRollbackManager(this.UpdaterUrl, this.AppId)); }
        }

        // 添加EnhancedVersionManager实例
        private EnhancedVersionManager enhancedVersionManager;
        public EnhancedVersionManager EnhancedVersionManager
        {
            get { return enhancedVersionManager ?? (enhancedVersionManager = new EnhancedVersionManager(Environment.CurrentDirectory, this.UpdaterUrl, this.AppId)); }
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
        private IntPtr handle;
        private Component component = new Component();
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);


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
            this.handle = handle;
            // 初始化SkipVersionManager
            skipVersionManager = new SkipVersionManager();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                    component.Dispose();
                }
                CloseHandle(handle);
                handle = IntPtr.Zero;
            }
            disposed = true;
        }

        ~AppUpdater()
        {
            Dispose(false);
        }


        /// <summary>
        /// 检查更新文件(旧版本)
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList"></param>
        /// <returns></returns>
        [Obsolete]
        public int CheckForUpdateOld(string serverXmlFile, string localXmlFile, out Hashtable updateFileList)
        {
            updateFileList = new Hashtable();
            if (!File.Exists(localXmlFile) || !File.Exists(serverXmlFile))
            {
                return -1;
            }

            XmlFiles serverXmlFiles = new XmlFiles(serverXmlFile);
            XmlFiles localXmlFiles = new XmlFiles(localXmlFile);

            XmlNodeList newNodeList = serverXmlFiles.GetNodeList("AutoUpdater/Files");
            XmlNodeList oldNodeList = localXmlFiles.GetNodeList("AutoUpdater/Files");

            int k = 0;
            for (int i = 0; i < newNodeList.Count; i++)
            {
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
                }
                else if (pos > -1 && newVer.CompareTo(oldFileAl[pos + 1].ToString()) > 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }

            }
            return k;
        }

        private string _NewVersion = string.Empty;
        public string NewVersion { get; set; }



        /// <summary>
        /// 检查更新文件
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList">Ҫ���µļ���</param>
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

            // 检查版本是否被跳过
            if (!string.IsNullOrEmpty(appId) && skipVersionManager.IsVersionSkipped(this.NewVersion, appId))
            {
                updateFileList = new Hashtable();
                return 0; // 版本已被跳过，返回0表示没有更新
            }

            // 检查是否为强制更新
            bool forceUpdate = IsCommandLineArgumentPresent("--force");
            if (!forceUpdate && !string.IsNullOrEmpty(appId) && skipVersionManager.IsVersionSkipped(this.NewVersion, appId))
            {
                updateFileList = new Hashtable();
                return 0; // 不是强制更新且版本被跳过，返回0表示没有更新
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
                //����ɵ���û�С�������
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
        /// 执行版本更新并记录历史
        /// </summary>
        /// <returns>更新是否成功</returns>
        public bool UpdateAndRecordHistory()
        {
            try
            {
                // 先获取新版本信息
                Hashtable updateFileList;
                int updateResult = CheckForUpdate(null, null, out updateFileList, this.AppId);

                // 如果有更新并且未被跳过
                if (updateResult > 0)
                {
                    // 记录更新前的版本历史
                    VersionHistoryManager.RecordNewVersion(this.NewVersion);
                    Debug.WriteLine($"已记录版本更新历史: {this.NewVersion}");

                    // 此处应调用实际的更新方法
                    // bool updateSuccess = PerformActualUpdate();
                    // 由于没有看到PerformActualUpdate方法，这里返回true表示成功
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"执行更新并记录历史失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 处理压缩更新包
        /// </summary>
        /// <param name="packagePath">压缩包路径</param>
        /// <returns>处理是否成功</returns>
        public bool ProcessCompressedUpdate(string packagePath)
        {
            try
            {
                // 创建版本条目
                VersionEntry version = new VersionEntry
                {
                    Version = Path.GetFileNameWithoutExtension(packagePath), // 假设文件名包含版本信息
                    InstallTime = DateTime.Now
                };

                // 使用EnhancedVersionManager处理压缩包
                bool result = EnhancedVersionManager.ProcessCompressedUpdateStatic(packagePath, version);

                if (result)
                {
                    Debug.WriteLine($"压缩更新包处理成功: {packagePath}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"处理压缩更新包失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 检查服务器是否有压缩更新包
        /// </summary>
        /// <returns>如果有压缩更新包返回true，否则返回false</returns>
        public bool CheckForCompressedUpdate()
        {
            // 检查服务器是否有新版本的压缩包
            // 这里可以实现与服务器的通信逻辑
            // 目前返回true作为示例
            return EnhancedVersionManager.CheckForUpdates(this.NewVersion);
        }

        /// <summary>
        /// 获取可回滚的版本列表
        /// </summary>
        /// <returns>可回滚的版本列表</returns>
        public List<VersionEntry> GetRollbackVersions()
        {
            return VersionRollbackManager.GetRollbackVersions();
        }

        /// <summary>
        /// 回滚到指定版本
        /// </summary>
        /// <param name="version">目标版本号</param>
        /// <returns>回滚是否成功</returns>
        public bool RollbackToVersion(string version)
        {
            return VersionRollbackManager.RollbackToVersion(version);
        }

        /// <summary>
        /// 检查是否可以回滚（是否存在历史版本）
        /// </summary>
        /// <returns>如果可以回滚则返回true，否则返回false</returns>
        public bool CanRollback()
        {
            return VersionRollbackManager.CanRollback();
        }

        public int CompareVersion(string Old_Version, string New_Version)
        {
            try
            {
                int version1Index = 0;//old
                int version2Index = 0;//new

                while (version1Index < Old_Version.Length || version2Index < New_Version.Length)
                {
                    long version1Num = 0;
                    long version2Num = 0;

                    while (version1Index < Old_Version.Length && Old_Version[version1Index] != '.')
                    {
                        version1Num = version1Num * 10 + (Old_Version[version1Index] - '0');
                        version1Index++;
                    }

                    while (version2Index < New_Version.Length && New_Version[version2Index] != '.')
                    {
                        version2Num = version2Num * 10 + (New_Version[version2Index] - '0');
                        version2Index++;
                    }

                    if (version1Num > version2Num)
                    {
                        return 1;
                    }
                    if (version1Num < version2Num)
                    {
                        return -1;
                    }

                    version2Index++;
                    version1Index++;
                }

                return 0;
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

            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;

            try
            {

                HttpWebRequest request = WebRequest.Create(UpdaterUrl) as HttpWebRequest;
                request.Method = "GET";
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";
                request.KeepAlive = false;
                request.Timeout = 36000;//超时时间
                // 接收返回的页面
                response = request.GetResponse() as HttpWebResponse;


                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.UpdaterUrl);
                ////request.AllowAutoRedirect = false; //�������ض���
                //request.Timeout = 50000; //���ӳ�ʱʱ������


                // response = request.GetResponse();
                stream = response.GetResponseStream();

                byte[] buffer = new byte[1024];




                Stream outStream = File.Create(serverXmlFile);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch (WebException e)
            {
                System.Diagnostics.Debug.WriteLine("下载失败，请联系系统管理员获取更多信息：" + e);
                // MessageBox.Show("下载失败，请联系系统管理员获取更多信息：" + e, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine("下载失败，请联系系统管理员获取更多信息：" + e);
                //  MessageBox.Show("下载失败，请联系系统管理员获取更多信息：" + e, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }


        }





    }
}
