using System;
using System.Web;
using System.IO;
using System.Net;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace AutoUpdate
{
    /// <summary>
    /// updater 的摘要说明。
    /// </summary>
    public class AppUpdater : IDisposable
    {
        #region 成员与字段属性
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
        /// 检查更新文件  过时
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



        /// <summary>
        /// 检查更新文件
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList">要更新的集合</param>
        /// <returns></returns>
        public int CheckForUpdate(string serverXmlFile, string localXmlFile, out Hashtable updateFileList)
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
                //这里多定义了一个维度，下面只用到了两个维度，其实可以不用定义的
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
                //如果旧的中没有。就添加
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }//如果都有，则比较版本号
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




        /*
         给你两个版本号 version1 和 version2 ，请你比较它们。

版本号由一个或多个修订号组成，各修订号由一个 '.' 连接。每个修订号由 多位数字 组成，可能包含 前导零 。每个版本号至少包含一个字符。修订号从左到右编号，下标从 0 开始，最左边的修订号下标为 0 ，下一个修订号下标为 1 ，以此类推。例如，2.5.33 和 0.1 都是有效的版本号。

比较版本号时，请按从左到右的顺序依次比较它们的修订号。比较修订号时，只需比较 忽略任何前导零后的整数值 。也就是说，修订号 1 和修订号 001 相等 。如果版本号没有指定某个下标处的修订号，则该修订号视为 0 。例如，版本 1.0 小于版本 1.1 ，因为它们下标为 0 的修订号相同，而下标为 1 的修订号分别为 0 和 1 ，0 < 1 。

返回规则如下：

如果 version1 > version2 返回 1，
如果 version1 < version2 返回 -1，
除此之外返回 0。

示例 1
输入：version1 = "1.01", version2 = "1.001"
输出：0
解释：忽略前导零，"01" 和 "001" 都表示相同的整数 "1"

示例 2
输入：version1 = "1.0", version2 = "1.0.0"
输出：0
解释：version1 没有指定下标为 2 的修订号，即视为 "0"

示例 3
输入：version1 = "0.1", version2 = "1.1"
输出：-1
解释：version1 中下标为 0 的修订号是 "0"，version2 中下标为 0 的修订号是 "1" 。0 < 1，所以 version1 < version2

提示
1 <= version1.length, version2.length <= 500
version1 和 version2 仅包含数字和 '.'
version1 和 version2 都是 有效版本号
version1 和 version2 的所有修订号都可以存储在 32 位整数 中

思路
(双指针)O(n+m)

比较两个版本号大小，版本号由修订号组成，中间使用'.'分隔，越靠近字符串前边，修订号的优先级越大。当v1 > v2时返回 1，当v1 < v2时返回 -1，相等时返回 0。

样例
如样例所示，v1= 1.02.3, v2 = 1.02.2，前两个修订号都相等，v1的第三个修订号大于v2的第三个修订号，因此v1 > v2，返回1。下面来讲解双指针的做法。

我们使用两个指针i和j分别指向两个字符串的开头，然后向后遍历，当遇到小数点'.'时停下来，并将每个小数点'.'分隔开的修订号解析成数字进行比较，越靠近前边，修订号的优先级越大。根据修订号大小关系，返回相应的数值。
————————————————
版权声明：本文为CSDN博主「Dreamt灬」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
原文链接：https://blog.csdn.net/u011645307/article/details/131117996
         */

        /// <summary>
        /// 比较两个版本号https://blog.csdn.net/u011645307/article/details/131117996
        /// CompareVersion("1.0.0.2","1.0.0.11")=-1
        /// </summary>
        /// <param name="Old_Version"></param>
        /// <param name="New_Version"></param>
        /// <returns>0不变，-1要更新，1为不要更新</returns>
        public int CompareVersion(string Old_Version, string New_Version)
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


        /*
        
        /// <summary>
        /// 检查更新文件   2020-11-2注释了。
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList"></param>
        /// <returns></returns>
        public int CheckForUpdate()
        {
            string localXmlFile = Application.StartupPath + "\\AutoUpdaterList.xml";
            if (!File.Exists(localXmlFile))
            {
                return -1;
            }

            XmlFiles updaterXmlFiles = new XmlFiles(localXmlFile);


            string tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
            this.UpdaterUrl = updaterXmlFiles.GetNodeValue("//Url") + "/AutoUpdaterList.xml";
            this.DownAutoUpdateFile(tempUpdatePath);

            string serverXmlFile = tempUpdatePath + "\\AutoUpdaterList.xml";
            if (!File.Exists(serverXmlFile))
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
                    k++;
                }
                else if (pos > -1 && newVer.CompareTo(oldFileAl[pos + 1].ToString()) > 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    k++;
                }

            }
            return k;
        }
        */

        /// <summary>
        /// 下载服务器的配置更新文件的到临时目录
        /// </summary>
        /// <returns></returns>
        public void DownAutoUpdateFile(string downpath)
        {
            if (!System.IO.Directory.Exists(downpath))
                System.IO.Directory.CreateDirectory(downpath);

            //本地保存旧版本的xml文件
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
                ////request.AllowAutoRedirect = false; //不允许重定向
                //request.Timeout = 50000; //连接超时时间设置


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
                System.Console.WriteLine("下载失败,请联系系统管理员。错误消息：" + e);
                // MessageBox.Show("下载失败,请联系系统管理员。错误消息：" + e, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (IOException e)
            {
                System.Console.WriteLine("下载失败,请联系系统管理员。错误消息：" + e);
                //  MessageBox.Show("下载失败,请联系系统管理员。错误消息：" + e, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }

            //WebRequest req = WebRequest.Create(this.UpdaterUrl);
            //WebResponse res = req.GetResponse();
            //if (res.ContentLength > 0)
            //{

            //    WebClient wClient = new WebClient();
            //    wClient.DownloadFile(this.UpdaterUrl, serverXmlFile);
            //    wClient.Dispose();
            //}


            //return tempPath;
        }





    }
}
