using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Threading;
using AutoUpdateTools;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO.Compression;
/*------------------------------------------------------------
* 
* 
*  by Liu Hua-shan,2007-07-13
* 
*  sh_liuhuashan@163.com
* 
* 
* ------------------------------------------------------------*/

namespace AULWriter
{
    public partial class frmAULWriter : Form
    {

        #region [������ڹ��캯��]

        public frmAULWriter()
        {
            InitializeComponent();
        }

        #endregion [������ڹ��캯��]



        #region [ѡ���ļ�����·��]

        private void btnSearDes_Click(object sender, EventArgs e)
        {
            this.sfdDest.ShowDialog(this);
        }

        private void sfdSrcPath_FileOk(object sender, CancelEventArgs e)
        {
            this.txtAutoUpdateXmlSavePath.Text = this.sfdDest.FileName.Substring(0, this.sfdDest.FileName.LastIndexOf(@"\")) + @"\AutoUpdaterList.xml";
        }

        #endregion [ѡ���ļ�����·��]

        #region [ѡ���ų��ļ�]

        private void btnSearExpt_Click(object sender, EventArgs e)
        {
            this.ofdExpt.ShowDialog(this);
        }

        private void ofdExpt_FileOk(object sender, CancelEventArgs e)
        {
            foreach (string _filePath in this.ofdExpt.FileNames)
            {
                this.txtExpt.Text += @_filePath.ToString() + "\r\n";
            }
        }

        #endregion [ѡ���ų��ļ�]

        #region [ѡ��������]

        private void btnSrc_Click(object sender, EventArgs e)
        {
            this.ofdSrc.ShowDialog(this);
        }

        private void ofdDest_FileOk(object sender, CancelEventArgs e)
        {
            this.txtMainExePath.Text = this.ofdSrc.FileName;
        }

        #endregion [ѡ��������]

        #region [���������]

        private void frmAULWriter_Load(object sender, EventArgs e)
        {
            //�������ļ���ȡ����
            readConfigFromFile();
        }

        //�������ļ���ȡ����
        private void readConfigFromFile()
        {
            try
            {
                DataConfig dataConfig = new DataConfig();
                string path = configFilePath;
                if (File.Exists(path))
                {
                    dataConfig = SerializeXmlHelper.DeserializeXml<DataConfig>(path);
                    txtUrl.Text = dataConfig.UpdateHttpAddress;
                    txtCompareSource.Text = dataConfig.CompareSource;
                    txtBaseDir.Text = dataConfig.BaseDir;
                    txtAutoUpdateXmlSavePath.Text = dataConfig.SavePath;
                    txtMainExePath.Text = dataConfig.EntryPoint;
                    StringBuilder sbexfiles = new StringBuilder();
                    string[] exfiles = dataConfig.ExcludeFiles.Split(new string[] { "\n" }, StringSplitOptions.None);
                    foreach (var item in exfiles)
                    {
                        sbexfiles.Append(item).Append("\r\n");
                    }
                    txtExpt.Text = sbexfiles.ToString();

                    StringBuilder sbupfiles = new StringBuilder();
                    string[] upfiles = dataConfig.UpdatedFiles.Split(new string[] { "\n" }, StringSplitOptions.None);
                    foreach (var item in upfiles)
                    {
                        sbupfiles.Append(item).Append("\r\n");
                    }
                    txtUpdatedFiles.Text = sbupfiles.ToString();
                    chkCustomVer.Checked = dataConfig.CustomVer;

                    txtVerNo.Text = dataConfig.CustomVersion;
                }
            }
            catch (Exception ex)
            {
                richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":�������ļ���ȡ����ʧ��:" + ex.Message);
                richTxtLog.AppendText("\r\n");
            }

        }
        #endregion [���������]

        #region [�����ļ�]

        private void btnProduce_Click(object sender, EventArgs e)
        {
            UpdateXmlFile(txtAutoUpdateXmlSavePath.Text, txtCompareSource.Text, txtBaseDir.Text, false, chk�ļ��Ƚ�.Checked);
            tabControl1.SelectedTab = tbpLastXml;
            //���Ƶ���������ȥ������
            //WriterAUList(chk��ϣֵ�Ƚ�.Checked, false);

            try
            {
                saveConfigToFile();
                MessageBox.Show("���ñ���ɹ�");
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ñ���ʧ��:" + ex.Message);
            }
            return;
            //�������߳�

            /*
            Thread thrdProduce = new Thread(new ThreadStart(WriterAUList));

            if (this.btnProduce.Text == "����(&G)")
            {

                #region [����������]

                if (!File.Exists(this.txtSrc.Text))
                {
                    MessageBox.Show(this, "��ѡ������ڳ���!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnSrc_Click(sender, e);
                }

                #region [�������Զ�������ַ]

                if (this.txtUrl.Text.Trim().Length == 0)
                {
                    MessageBox.Show(this, "�������Զ�������ַ!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtUrl.Focus();

                    return;
                }


                #endregion [�������Զ�������ַ]

                if (this.txtDest.Text.Trim() == string.Empty)
                {
                    MessageBox.Show(this, "��ѡ��AutoUpdaterList����λ��!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnSearDes_Click(sender, e);
                }

                #endregion [����������]

                #region [���߳�д�ļ�]

                thrdProduce.IsBackground = true;
                thrdProduce.Start();

                #endregion [���߳�д�ļ�]

                this.btnProduce.Text = "ֹͣ(&S)";
            }
            else
            {
                Application.DoEvents();
                if (MessageBox.Show(this, "�Ƿ�ֹͣ�ļ����ɸ����ļ�?", "AutoUpdater", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //thrdProduce.Interrupt();
                    //thrdProduce.Abort();
                    if (thrdProduce.IsAlive)
                    {
                        thrdProduce.Abort();
                        thrdProduce.Join();
                    }
                    this.btnProduce.Text = "����(&G)";
                }
            }
            */


        }

        #region [дAutoUpdaterList]

        /// <summary>
        /// �Ƿ���й�ϣֵ�Ƚϣ�����ǣ����ϣֵ��һ�������Ӱ汾�š����򲻱�
        /// </summary>
        /// <param name="HashValueComparison"></param>
        void WriterAUList_old(bool HashValueComparison = false)
        {
            #region [дAutoUpdaterlist]

            string strEntryPoint = this.txtMainExePath.Text.Trim().Substring(this.txtMainExePath.Text.Trim().LastIndexOf(@"\") + 1, this.txtMainExePath.Text.Trim().Length - this.txtMainExePath.Text.Trim().LastIndexOf(@"\") - 1);
            string strFilePath = this.txtAutoUpdateXmlSavePath.Text.Trim();

            FileStream fs = new FileStream(strFilePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            sw.Write("<?xml version=\"1.0\" encoding=\"gb2312\" ?>");
            sw.Write("\r\n<AutoUpdater>\r\n");

            #region[description]

            sw.Write("\t<Description>");
            sw.Write(strEntryPoint.Substring(0, strEntryPoint.LastIndexOf(".")) + " autoUpdate");
            sw.Write("</Description>\r\n");

            #endregion[description]

            #region [Updater]

            sw.Write("\t<Updater>\r\n");

            sw.Write("\t\t<Url>");
            sw.Write(this.txtUrl.Text.Trim());
            sw.Write("</Url>\r\n");

            sw.Write("\t\t<LastUpdateTime>");
            sw.Write(DateTime.Now.ToString("yyyy-MM-dd"));
            sw.Write("</LastUpdateTime>\r\n");

            sw.Write("\t</Updater>\r\n");

            #endregion [Updater]

            #region [application]

            sw.Write("\t<Application applicationId = \"" + strEntryPoint.Substring(0, strEntryPoint.LastIndexOf(".")) + "\">\r\n");

            sw.Write("\t\t<EntryPoint>");
            sw.Write(strEntryPoint);
            sw.Write("</EntryPoint>\r\n");

            sw.Write("\t\t<Location>");
            sw.Write(".");
            sw.Write("</Location>\r\n");

            FileVersionInfo _lcObjFVI = FileVersionInfo.GetVersionInfo(this.txtMainExePath.Text);

            sw.Write("\t\t<Version>");
            sw.Write(string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart));
            sw.Write("</Version>\r\n");


            sw.Write("\t</Application>\r\n");


            #endregion [application]

            #region [Files]

            sw.Write("\t<Files>\r\n");

            StringCollection strColl = GetAllFiles(this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")));

            //���ָ����Ҫ���µ��ļ�,���������
            if (txtUpdatedFiles.Text.Trim().Length > 0)
            {
                string[] files = txtUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string strVerNo = string.Empty;
                strVerNo = txtVerNo.Text;
                int seed = int.Parse(txtVerNo.Text.Split('.')[3]);
                seed++;
                strVerNo = strVerNo.TrimEnd(txtVerNo.Text.Split('.')[3].ToCharArray()) + seed.ToString();

                this.prbProd.Visible = true;
                this.prbProd.Minimum = 0;
                this.prbProd.Maximum = files.Length;

                for (int i = 0; i < files.Length; i++)
                {
                    if (!CheckExist(files[i].Trim()))
                    {
                        //��ʱͳһ�ˡ����ָ���汾��
                        if (chkCustomVer.Checked)
                        {
                            txtVerNo.Text = strVerNo;
                            string rootDir = this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")) + @"\";
                            //MessageBox.Show( @files[i].Replace(@rootDir,""));
                            sw.Write("\t\t<File Ver=\""
                                + strVerNo
                                + "\" Name= \"" + files[i].Replace(@rootDir, "")
                                + "\" />\r\n");
                        }
                        else
                        {
                            FileVersionInfo m_lcObjFVI = FileVersionInfo.GetVersionInfo(files[i].ToString());
                            string rootDir = this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")) + @"\";
                            //MessageBox.Show( @files[i].Replace(@rootDir,""));
                            sw.Write("\t\t<File Ver=\""
                                + string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart)
                                + "\" Name= \"" + files[i].Replace(@rootDir, "")
                                + "\" />\r\n");
                        }

                    }

                    prbProd.Value = i;
                }

            }
            else
            {
                #region Ĭ��
                this.prbProd.Visible = true;
                this.prbProd.Minimum = 0;
                this.prbProd.Maximum = strColl.Count;

                for (int i = 0; i < strColl.Count; i++)
                {
                    if (!CheckExist(strColl[i].Trim()))
                    {

                        FileVersionInfo m_lcObjFVI = FileVersionInfo.GetVersionInfo(strColl[i].ToString());

                        string rootDir = this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")) + @"\";

                        //MessageBox.Show( @strColl[i].Replace(@rootDir,""));

                        sw.Write("\t\t<File Ver=\""
                            + string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart)
                            + "\" Name= \"" + @strColl[i].Replace(@rootDir, "")
                            + "\" />\r\n");
                    }

                    prbProd.Value = i;
                }
                #endregion
            }


            #endregion [Files]

            sw.Write("\t</Files>\r\n");

            sw.Write("</AutoUpdater>");
            sw.Close();
            fs.Close();

            tbpLastXml.Text = "���ɳɹ�:" + this.txtAutoUpdateXmlSavePath.Text.Trim();

            #region [Notification]

            MessageBox.Show(this, "�Զ������ļ����ɳɹ�:" + this.txtAutoUpdateXmlSavePath.Text.Trim(), "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.prbProd.Value = 0;
            this.prbProd.Visible = false;

            #endregion [Notification]

            #endregion [дAutoUpdaterlist]
        }


        #region �����Ծ�̬����
        /// <summary>
        /// �����ϣֵ
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string CalculateFileHash(string filePath)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        /// <summary>
        /// Դ�ļ��������µġ�Ŀ����Ҫ���µġ� 
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FindDifferentFileByHash(string sourceFolder, string targetFolder, string fileName)
        {
            // ���������ļ�����ָ���ļ�������·��
            string sourceFilePath = Path.Combine(sourceFolder, fileName);
            string targetFilePath = Path.Combine(targetFolder, fileName);

            //������µ��У�Ŀ��û����Ҫ�����ļ�����ʱҪ���ƹ�ȥ��
            if (File.Exists(sourceFilePath) && !File.Exists(targetFilePath))
            {
                return fileName;
            }

            // ��������ļ������Ƿ����ָ�����ļ�  ��������������µ�û�С�����ҪҪ�����б��С�
            if (!File.Exists(sourceFilePath) || !File.Exists(targetFilePath))
            {
                // �����һ�ļ����в����ڸ��ļ����򷵻ش��ڵ��ļ�·��
                return File.Exists(sourceFilePath) ? sourceFilePath : targetFilePath;
            }

            // ���������ļ��Ĺ�ϣֵ
            string sourceHash = CalculateFileHash(sourceFilePath);
            string targetHash = CalculateFileHash(targetFilePath);

            // �ȽϹ�ϣֵ
            if (sourceHash != targetHash)
            {
                // �����ϣֵ����ͬ���򷵻��ļ���
                return fileName;
            }

            // �����ϣֵ��ͬ���򷵻�null
            return null;
        }

        /// <summary>
        /// �������·��
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetRelativePath(string fromPath, string toPath)
        {
            // ������·��ת��Ϊ����·��
            fromPath = Path.GetFullPath(fromPath);
            toPath = Path.GetFullPath(toPath);

            // ��·��ת��Ϊ�淶������ʽ����ȷ���Ƚ�ʱ������·����Сд��б�ܷ���ͬ������
            fromPath = fromPath.Replace("\\", "/").ToLower();
            toPath = toPath.Replace("\\", "/").ToLower();

            // �ҵ���ͬ�ĸ�·��
            int length = Math.Min(fromPath.Length, toPath.Length);
            int index = 0;
            while (index < length && fromPath[index] == toPath[index])
            {
                index++;
            }
            // ������Ҫ���˵���Ŀ¼�Ĵ��������̷���ͬ�������
            int backSteps = 0;
            for (int i = index; i < fromPath.Length; i++)
            {
                if (fromPath[i] == '/')
                {
                    backSteps++;
                }
            }

            // �������˵���Ŀ¼��·��
            string relativePath = "";
            for (int i = 0; i < backSteps; i++)
            {
                relativePath += "../";
            }

            // �ӹ�ͬ��·��֮��Ĳ��ֿ�ʼ�������·��
            relativePath += toPath.Substring(index);

            // �滻б�ܷ��򲢷��ؽ��
            return relativePath.Replace("/", "\\");
        }





        #endregion


        /// <summary>
        /// �Ƿ���й�ϣֵ�Ƚϣ�����ǣ����ϣֵ��һ�������Ӱ汾�š����򲻱�
        /// </summary>
        /// <param name="HashValueComparison"></param>
        void WriterAUList(bool HashValueComparison = false, bool Preview = false)
        {

            List<string> DiffList = new List<string>();
            if (HashValueComparison)
            {
                txtDiff.Clear();
                List<string> files = txtUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (string fullPath in files)
                {

                    string sourceFolder = this.txtCompareSource.Text.Trim();
                    string targetFolder = this.txtBaseDir.Text.Trim();

                    string relativePath = GetRelativePath(targetFolder, fullPath);
                    //ȥ��ǰ���ַ�\\
                    relativePath = relativePath.TrimStart('\\');
                    string result = FindDifferentFileByHash(sourceFolder, targetFolder, relativePath);
                    if (!string.IsNullOrEmpty(result))
                    {
                        //���첻ͬ�ģ����ص���һ���ļ�������Ŀ��Ϊ��׼����һ��·����
                        //System.IO.Path.Combine(sourceFolder, result)
                        txtDiff.AppendText(result);
                        DiffList.Add(result);
                        txtDiff.AppendText("\r\n");
                    }
                }

            }


            #region [дAutoUpdaterlist]

            string strEntryPoint = this.txtMainExePath.Text.Trim().Substring(this.txtMainExePath.Text.Trim().LastIndexOf(@"\") + 1, this.txtMainExePath.Text.Trim().Length - this.txtMainExePath.Text.Trim().LastIndexOf(@"\") - 1);
            string strFilePath = this.txtAutoUpdateXmlSavePath.Text.Trim();
            //Append  Ĭ�Ͼͻ�����
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"gb2312\" ?>");
            sb.Append("\r\n<AutoUpdater>\r\n");

            #region[description]
            sb.Append("<Description>");
            sb.Append(strEntryPoint.Substring(0, strEntryPoint.LastIndexOf(".")) + " autoUpdate");
            sb.Append("</Description>\r\n");
            #endregion[description]

            #region [Updater]
            sb.Append("\t<Updater>\r\n");
            sb.Append("\t\t<Url>");
            sb.Append(this.txtUrl.Text.Trim());
            sb.Append("</Url>\r\n");
            sb.Append("\t\t<LastUpdateTime>");
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd"));
            sb.Append("</LastUpdateTime>\r\n");
            sb.Append("\t</Updater>\r\n");
            #endregion [Updater]

            #region [application]
            sb.Append("\t<Application applicationId = \"" + strEntryPoint.Substring(0, strEntryPoint.LastIndexOf(".")) + "\">\r\n");
            sb.Append("\t\t<EntryPoint>");
            sb.Append(strEntryPoint);
            sb.Append("</EntryPoint>\r\n");
            sb.Append("\t\t<Location>");
            sb.Append(".");
            sb.Append("</Location>\r\n");

            FileVersionInfo _lcObjFVI = FileVersionInfo.GetVersionInfo(this.txtMainExePath.Text);
            sb.Append("\t\t<Version>");
            sb.Append(string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart));
            sb.Append("</Version>\r\n");
            sb.Append("\t</Application>\r\n");
            #endregion [application]

            #region [Files]
            sb.Append("\t<Files>\r\n");

            StringCollection strColl = GetAllFiles(this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")));

            //���ָ����Ҫ���µ��ļ�,���������
            if (txtUpdatedFiles.Text.Trim().Length > 0)
            {
                string[] files = txtUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string strVerNo = string.Empty;
                strVerNo = txtVerNo.Text;
                int seed = int.Parse(txtVerNo.Text.Split('.')[3]);
                seed++;
                strVerNo = strVerNo.TrimEnd(txtVerNo.Text.Split('.')[3].ToCharArray()) + seed.ToString();

                this.prbProd.Visible = true;
                this.prbProd.Minimum = 0;
                this.prbProd.Maximum = files.Length;

                for (int i = 0; i < files.Length; i++)
                {
                    if (!CheckExist(files[i].Trim()))
                    {
                        //��ʱͳһ�ˡ����ָ���汾��
                        if (chkCustomVer.Checked)
                        {
                            sb.Append("\t\t<File Ver=\""
                                + strVerNo
                                + "\" Name= \"" + files[i].Replace(this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")) + @"\", "")
                                + "\" />\r\n");
                        }
                        else
                        {
                            FileVersionInfo m_lcObjFVI = FileVersionInfo.GetVersionInfo(files[i].ToString());
                            sb.Append("\t\t<File Ver=\""
                                + string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart)
                                + "\" Name= \"" + files[i].Replace(this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")) + @"\", "")
                                + "\" />\r\n");
                        }
                    }

                    prbProd.Value = i;
                }
            }
            else
            {
                #region Ĭ��
                this.prbProd.Visible = true;
                this.prbProd.Minimum = 0;
                this.prbProd.Maximum = strColl.Count;

                for (int i = 0; i < strColl.Count; i++)
                {
                    if (!CheckExist(strColl[i].Trim()))
                    {
                        FileVersionInfo m_lcObjFVI = FileVersionInfo.GetVersionInfo(strColl[i].ToString());
                        string rootDir = this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")) + @"\";
                        sb.Append("\t\t<File Ver=\""
                            + string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart)
                            + "\" Name= \"" + strColl[i].Replace(rootDir, "")
                            + "\" />\r\n");
                    }

                    prbProd.Value = i;
                }
                #endregion
            }

            sb.Append("\t</Files>\r\n");
            sb.Append("</AutoUpdater>");

            #endregion [Files]
            if (Preview)
            {
                txtLastXml.Text = sb.ToString();
            }
            else
            {
                // ��StringBuilder������д���ļ�
                File.WriteAllText(strFilePath, sb.ToString(), System.Text.Encoding.GetEncoding("gb2312"));
                MessageBox.Show(this, "�Զ������ļ����ɳɹ�:" + this.txtAutoUpdateXmlSavePath.Text.Trim(), "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.prbProd.Value = 0;
                this.prbProd.Visible = false;
            }


            #endregion [дAutoUpdaterlist]
        }
        #endregion [дAutoUpdaterList]

        #region [������Ŀ¼]

        private StringCollection GetAllFiles(string rootdir)
        {
            StringCollection result = new StringCollection();
            GetAllFiles(rootdir, result);
            return result;
        }

        private void GetAllFiles(string parentDir, StringCollection result)
        {
            string[] dir = Directory.GetDirectories(parentDir);
            for (int i = 0; i < dir.Length; i++)
                GetAllFiles(dir[i], result);
            string[] file = Directory.GetFiles(parentDir);
            for (int i = 0; i < file.Length; i++)
                result.Add(file[i]);
        }

        #endregion [������Ŀ¼]

        #region [�ų�����Ҫ���ļ�]

        private bool CheckExist(string filePath)
        {
            bool isExist = false;

            foreach (string strCheck in this.txtExpt.Text.Split(';'))
            {
                if (filePath.Trim() == strCheck.Trim())
                {
                    isExist = true;

                    break;
                }
            }

            return isExist;
        }


        #endregion [�ų�����Ҫ���ļ�]

        #endregion [�����ļ�]

        private void button_save_config_Click(object sender, EventArgs e)
        {
            //�������õ��ļ�
            try
            {
                saveConfigToFile();
                richTxtLog.AppendText("���ñ���ɹ�");
                //MessageBox.Show("���ñ���ɹ�");
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ñ���ʧ��:" + ex.Message);
            }
        }

        private string configFilePath = Application.StartupPath + "\\config\\config.xml";

        private void saveConfigToFile()
        {
            try
            {
                DataConfig dataConfig = new DataConfig();
                dataConfig.UpdateHttpAddress = txtUrl.Text.Trim();
                dataConfig.SavePath = txtAutoUpdateXmlSavePath.Text;
                dataConfig.EntryPoint = txtMainExePath.Text;
                dataConfig.ExcludeFiles = txtExpt.Text;
                dataConfig.CustomVer = chkCustomVer.Checked;
                dataConfig.CustomVersion = txtVerNo.Text;
                dataConfig.UpdatedFiles = txtUpdatedFiles.Text;
                dataConfig.CompareSource = txtCompareSource.Text;
                dataConfig.BaseDir = txtBaseDir.Text;
                string path = configFilePath;
                SerializeXmlHelper.SerializeXml(dataConfig, path);
            }
            catch (Exception ex)
            {
                richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":�������õ��ļ�ʧ��:" + ex.Message);
                richTxtLog.AppendText("\r\n");
            }
        }

        private void txtExpt_DragDrop(object sender, DragEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            //�ڴ˴���ȡ�ļ������ļ��е�·��
            Array array = (Array)e.Data.GetData(DataFormats.FileDrop);
            foreach (var item in array)
            {
                sb.Append(item).Append("\r\n");
            }
            txtExpt.Text = sb.ToString();

            //            txtExpt.Text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        }

        private void txtExpt_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link; //�޸�����Ϸ�ʱ����ʽ��
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            base.OnDragEnter(e);
        }


        private void txtSrc_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))//�ļ��ϷŲ�����
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);//����Ϸ��ļ���·����
                string filePath = filePaths[0];//ȡ�õ�һ���ļ���·����
                txtMainExePath.Text = filePath; //��TextBox����ʾ��һ���ļ�·����
            }
            // base.OnDragDrop(e);

        }

        private void txtSrc_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link; //�޸�����Ϸ�ʱ����ʽ��
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            base.OnDragEnter(e);
        }

        private void txtUpdatedFiles_DragDrop(object sender, DragEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            //�ڴ˴���ȡ�ļ������ļ��е�·��
            Array array = (Array)e.Data.GetData(DataFormats.FileDrop);

            //����
            Array.Sort(array);

            foreach (string path in array)
            {
                if (File.Exists(path))
                {
                    sb.Append(path).Append("\r\n");
                }
                else if (Directory.Exists(path))
                {
                    StringCollection sc = GetAllFiles(path);
                    foreach (var item in sc)
                    {
                        sb.Append(item).Append("\r\n");
                    }
                }
                else
                    Console.WriteLine("��Ч��·��");


            }
            if (chkAppend.Checked)
            {
                txtUpdatedFiles.Text += sb.ToString();
            }
            else
            {
                txtUpdatedFiles.Text = sb.ToString();
            }

        }

        private void txtUpdatedFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link; //�޸�����Ϸ�ʱ����ʽ��
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            base.OnDragEnter(e);
        }

        private void chkCustomVer_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmAULWriter_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveConfigToFile();
        }

        #region ��ȡԭ����xml�����ļ�

        //private string ReadoldXml()
        //{
        //    string content = string.Empty;
        //    string filePath = txtAutoUpdateXmlSavePath.Text;

        //    try
        //    {
        //        // ��ȡ�ļ�����
        //        content = File.ReadAllText(filePath);

        //        // ����ļ�����
        //        //Console.WriteLine(content);
        //    }
        //    catch (Exception ex)
        //    {
        //        // ������ܵ��쳣�������ļ������ڻ���������
        //        Console.WriteLine("Error reading file: " + ex.Message);
        //    }
        //    return content;
        //}

        List<string> DiffList = new List<string>();

        public void UpdateXmlFile(string xmlFilePath, string sourceFolder, string targetFolder, bool Preview = true, bool FileComparison = true)
        {
            XDocument doc = XDocument.Load(xmlFilePath);
            txtDiff.Clear();

            foreach (XElement fileElement in doc.Descendants("File"))
            {
                if (FileComparison)
                {
                    string fileName = fileElement.Attribute("Name").Value;
                    string sourceFilePath = Path.Combine(sourceFolder, fileName);
                    string targetFilePath = Path.Combine(targetFolder, fileName);

                    bool isSourceExist = File.Exists(sourceFilePath);
                    bool isTargetExist = File.Exists(targetFilePath);
                    long sourceSize = isSourceExist ? new FileInfo(sourceFilePath).Length : 0;
                    long targetSize = isTargetExist ? new FileInfo(targetFilePath).Length : 0;
                    DateTime sourceLastWriteTime = isSourceExist ? File.GetLastWriteTimeUtc(sourceFilePath) : DateTime.MinValue;
                    DateTime targetLastWriteTime = isTargetExist ? File.GetLastWriteTimeUtc(targetFilePath) : DateTime.MinValue;

                    if (sourceLastWriteTime != targetLastWriteTime || sourceSize != targetSize)
                    {
                        // ����ļ���С���޸�ʱ�䲻ͬ��ֱ�Ӹ��°汾��
                        string version = fileElement.Attribute("Ver").Value;
                        IncrementVersion(ref version);
                        fileElement.SetAttributeValue("Ver", version);
                        DiffList.Add(fileName);
                        txtDiff.AppendText(fileName + "\r\n");
                    }
                    else if (isSourceExist && isTargetExist)
                    {
                        // ������ù�ϣֵ�Ƚ�
                        string sourceHash = CalculateFileHash(sourceFilePath);
                        string targetHash = CalculateFileHash(targetFilePath);

                        if (sourceHash != targetHash)
                        {
                            // �����ϣֵ��ͬ������°汾��
                            string version = fileElement.Attribute("Ver").Value;
                            IncrementVersion(ref version);
                            fileElement.SetAttributeValue("Ver", version);
                            DiffList.Add(fileName);
                            txtDiff.AppendText(fileName + "\r\n");
                        }
                    }
                    else if (isSourceExist && sourceSize < 1048576) // �ļ�С��1MB
                    {
                        // ����ļ�С��1MB���������ֽڱȽ�
                        bool isContentSame = CompareFilesByChunk(sourceFilePath, targetFilePath);
                        if (!isContentSame)
                        {
                            string version = fileElement.Attribute("Ver").Value;
                            IncrementVersion(ref version);
                            fileElement.SetAttributeValue("Ver", version);
                            DiffList.Add(fileName);
                            txtDiff.AppendText(fileName + "\r\n");
                        }
                    }
                }
            }

            if (Preview)
            {
                txtLastXml.Text = doc.ToString();
                richTxtLog.AppendText("Ԥ��-�������µ�XML�ļ��ɹ���\r\n");
            }
            else
            {
                doc.Save(xmlFilePath);
                richTxtLog.AppendText("����-�������µ�XML�ļ��ɹ���\r\n");
            }
        }


        /// <summary>
        /// ���ֽڱȽ�
        /// </summary>
        /// <param name="filePath1"></param>
        /// <param name="filePath2"></param>
        /// <returns></returns>
        private bool CompareFilesByChunk(string filePath1, string filePath2)
        {
            using (var fs1 = File.OpenRead(filePath1))
            using (var fs2 = File.OpenRead(filePath2))
            {
                int bufferSize = 1024 * 1024; // 1MB buffer size
                byte[] buffer1 = new byte[bufferSize];
                byte[] buffer2 = new byte[bufferSize];

                while (true)
                {
                    int bytesRead1 = fs1.Read(buffer1, 0, buffer1.Length);
                    int bytesRead2 = fs2.Read(buffer2, 0, buffer2.Length);

                    if (bytesRead1 != bytesRead2 || !buffer1.SequenceEqual(buffer2))
                        return false;

                    if (bytesRead1 == 0) break;
                }

                return true;
            }
        }

        private void IncrementVersion(ref string version)
        {
            var parts = version.Split('.');
            parts[3] = (int.Parse(parts[3]) + 1).ToString();
            version = string.Join(".", parts);
        }



        #endregion

        private void btnPreview_Click(object sender, EventArgs e)
        {
            //��ʾ���켰���ɺ��xml
            UpdateXmlFile(txtAutoUpdateXmlSavePath.Text, txtCompareSource.Text, txtBaseDir.Text, true, chk�ļ��Ƚ�.Checked);
            tabControl1.SelectedTab = tbpLastXml;
        }

        private void btnCompareSource_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogCompareSource.ShowDialog() == DialogResult.OK)
            {
                txtCompareSource.Text = folderBrowserDialogCompareSource.SelectedPath;
            }
        }

        private void btnBaseDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogCompareSource.ShowDialog() == DialogResult.OK)
            {
                txtBaseDir.Text = folderBrowserDialogCompareSource.SelectedPath;
            }
        }

        private void chk��ϣֵ�Ƚ�_CheckedChanged(object sender, EventArgs e)
        {
            //btnrelease.Visible = chk��ϣֵ�Ƚ�.Checked;
        }


        #region [���Ʒ���]



        /// <summary>
        /// �����ļ�  ���汾��������ļ� ȫ��
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="objPath"></param>
        public void CopyFile(string sourceDir, string targetDir, string tartgetfileName)
        {
            string sourcePath = System.IO.Path.Combine(sourceDir, tartgetfileName);
            string targetPath = System.IO.Path.Combine(targetDir, tartgetfileName);
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
            try
            {
                File.Copy(sourcePath, targetPath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //��������ļ� ����I����ʱĿ¼���ٵ�����������������ʱ�汾��ֻ�Բ���ĸ���  �������õ��ļ���
        //�ȸ��� ������xml

        #endregion [���Ʒ���]
        private void btnrelease_Click(object sender, EventArgs e)
        {
            foreach (var item in DiffList)
            {
                CopyFile(txtCompareSource.Text, txtBaseDir.Text, item);
            }

            richTxtLog.AppendText($"���浽Ŀ��-{txtBaseDir.Text}�ɹ���");
            richTxtLog.AppendText("\r\n");

            string lastTargetPath = Path.GetDirectoryName(txtAutoUpdateXmlSavePath.Text.Trim());

            foreach (var item in DiffList)
            {
                CopyFile(txtCompareSource.Text, lastTargetPath, item);
            }
            richTxtLog.AppendText($"������Ŀ��-{lastTargetPath}�ɹ���");
            richTxtLog.AppendText("\r\n");
        }
    }
}