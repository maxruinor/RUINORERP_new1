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
    /// updater ��ժҪ˵����
    /// </summary>
    public class AppUpdater : IDisposable
    {
        #region ��Ա���ֶ�����
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
        /// AppUpdater���캯��
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
        /// �������ļ�  ��ʱ
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
        /// �������ļ�
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList">Ҫ���µļ���</param>
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
                //����ඨ����һ��ά�ȣ�����ֻ�õ�������ά�ȣ���ʵ���Բ��ö����
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
                //����ɵ���û�С������
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }//������У���Ƚϰ汾��
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
         ���������汾�� version1 �� version2 ������Ƚ����ǡ�

�汾����һ�������޶�����ɣ����޶�����һ�� '.' ���ӡ�ÿ���޶����� ��λ���� ��ɣ����ܰ��� ǰ���� ��ÿ���汾�����ٰ���һ���ַ����޶��Ŵ����ұ�ţ��±�� 0 ��ʼ������ߵ��޶����±�Ϊ 0 ����һ���޶����±�Ϊ 1 ���Դ����ơ����磬2.5.33 �� 0.1 ������Ч�İ汾�š�

�Ƚϰ汾��ʱ���밴�����ҵ�˳�����αȽ����ǵ��޶��š��Ƚ��޶���ʱ��ֻ��Ƚ� �����κ�ǰ����������ֵ ��Ҳ����˵���޶��� 1 ���޶��� 001 ��� ������汾��û��ָ��ĳ���±괦���޶��ţ�����޶�����Ϊ 0 �����磬�汾 1.0 С�ڰ汾 1.1 ����Ϊ�����±�Ϊ 0 ���޶�����ͬ�����±�Ϊ 1 ���޶��ŷֱ�Ϊ 0 �� 1 ��0 < 1 ��

���ع������£�

��� version1 > version2 ���� 1��
��� version1 < version2 ���� -1��
����֮�ⷵ�� 0��

ʾ�� 1
���룺version1 = "1.01", version2 = "1.001"
�����0
���ͣ�����ǰ���㣬"01" �� "001" ����ʾ��ͬ������ "1"

ʾ�� 2
���룺version1 = "1.0", version2 = "1.0.0"
�����0
���ͣ�version1 û��ָ���±�Ϊ 2 ���޶��ţ�����Ϊ "0"

ʾ�� 3
���룺version1 = "0.1", version2 = "1.1"
�����-1
���ͣ�version1 ���±�Ϊ 0 ���޶����� "0"��version2 ���±�Ϊ 0 ���޶����� "1" ��0 < 1������ version1 < version2

��ʾ
1 <= version1.length, version2.length <= 500
version1 �� version2 ���������ֺ� '.'
version1 �� version2 ���� ��Ч�汾��
version1 �� version2 �������޶��Ŷ����Դ洢�� 32 λ���� ��

˼·
(˫ָ��)O(n+m)

�Ƚ������汾�Ŵ�С���汾�����޶�����ɣ��м�ʹ��'.'�ָ���Խ�����ַ���ǰ�ߣ��޶��ŵ����ȼ�Խ�󡣵�v1 > v2ʱ���� 1����v1 < v2ʱ���� -1�����ʱ���� 0��

����
��������ʾ��v1= 1.02.3, v2 = 1.02.2��ǰ�����޶��Ŷ���ȣ�v1�ĵ������޶��Ŵ���v2�ĵ������޶��ţ����v1 > v2������1������������˫ָ���������

����ʹ������ָ��i��j�ֱ�ָ�������ַ����Ŀ�ͷ��Ȼ����������������С����'.'ʱͣ����������ÿ��С����'.'�ָ������޶��Ž��������ֽ��бȽϣ�Խ����ǰ�ߣ��޶��ŵ����ȼ�Խ�󡣸����޶��Ŵ�С��ϵ��������Ӧ����ֵ��
��������������������������������
��Ȩ����������ΪCSDN������Dreamt�᡹��ԭ�����£���ѭCC 4.0 BY-SA��ȨЭ�飬ת���븽��ԭ�ĳ������Ӽ���������
ԭ�����ӣ�https://blog.csdn.net/u011645307/article/details/131117996
         */

        /// <summary>
        /// �Ƚ������汾��https://blog.csdn.net/u011645307/article/details/131117996
        /// CompareVersion("1.0.0.2","1.0.0.11")=-1
        /// </summary>
        /// <param name="Old_Version"></param>
        /// <param name="New_Version"></param>
        /// <returns>0���䣬-1Ҫ���£�1Ϊ��Ҫ����</returns>
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
        /// �������ļ�   2020-11-2ע���ˡ�
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
        /// ���ط����������ø����ļ��ĵ���ʱĿ¼
        /// </summary>
        /// <returns></returns>
        public void DownAutoUpdateFile(string downpath)
        {
            if (!System.IO.Directory.Exists(downpath))
                System.IO.Directory.CreateDirectory(downpath);

            //���ر���ɰ汾��xml�ļ�
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
                request.Timeout = 36000;//��ʱʱ��
                // ���շ��ص�ҳ��
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
                System.Console.WriteLine("����ʧ��,����ϵϵͳ����Ա��������Ϣ��" + e);
                // MessageBox.Show("����ʧ��,����ϵϵͳ����Ա��������Ϣ��" + e, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (IOException e)
            {
                System.Console.WriteLine("����ʧ��,����ϵϵͳ����Ա��������Ϣ��" + e);
                //  MessageBox.Show("����ʧ��,����ϵϵͳ����Ա��������Ϣ��" + e, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
