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


        #region [�ر�д�������]

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        #endregion [�ر�д�������]

        #region [ѡ���ļ�����·��]

        private void btnSearDes_Click(object sender, EventArgs e)
        {
            this.sfdDest.ShowDialog(this);
        }

        private void sfdSrcPath_FileOk(object sender, CancelEventArgs e)
        {
            this.txtDest.Text = this.sfdDest.FileName.Substring(0, this.sfdDest.FileName.LastIndexOf(@"\")) + @"\AutoUpdaterList.xml";
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
            this.txtSrc.Text = this.ofdSrc.FileName;
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
                    txtDest.Text = dataConfig.SavePath;
                    txtSrc.Text = dataConfig.EntryPoint;
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
            WriterAUList(true);
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
        void WriterAUList(bool HashValueComparison = false)
        {
            #region [дAutoUpdaterlist]

            string strEntryPoint = this.txtSrc.Text.Trim().Substring(this.txtSrc.Text.Trim().LastIndexOf(@"\") + 1, this.txtSrc.Text.Trim().Length - this.txtSrc.Text.Trim().LastIndexOf(@"\") - 1);
            string strFilePath = this.txtDest.Text.Trim();

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
            sw.Write(DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd"));
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

            FileVersionInfo _lcObjFVI = FileVersionInfo.GetVersionInfo(this.txtSrc.Text);

            sw.Write("\t\t<Version>");
            sw.Write(string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart));
            sw.Write("</Version>\r\n");


            sw.Write("\t</Application>\r\n");


            #endregion [application]

            #region [Files]

            sw.Write("\t<Files>\r\n");

            StringCollection strColl = GetAllFiles(this.txtSrc.Text.Substring(0, this.txtSrc.Text.LastIndexOf(@"\")));

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
                            string rootDir = this.txtSrc.Text.Substring(0, this.txtSrc.Text.LastIndexOf(@"\")) + @"\";
                            //MessageBox.Show( @files[i].Replace(@rootDir,""));
                            sw.Write("\t\t<File Ver=\""
                                + strVerNo
                                + "\" Name= \"" + files[i].Replace(@rootDir, "")
                                + "\" />\r\n");
                        }
                        else
                        {
                            FileVersionInfo m_lcObjFVI = FileVersionInfo.GetVersionInfo(files[i].ToString());
                            string rootDir = this.txtSrc.Text.Substring(0, this.txtSrc.Text.LastIndexOf(@"\")) + @"\";
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

                        string rootDir = this.txtSrc.Text.Substring(0, this.txtSrc.Text.LastIndexOf(@"\")) + @"\";

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


            #region [Notification]

            MessageBox.Show(this, "�Զ������ļ����ɳɹ�:" + this.txtDest.Text.Trim(), "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.prbProd.Value = 0;
            this.prbProd.Visible = false;

            #endregion [Notification]

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
                MessageBox.Show("���ñ���ɹ�");
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
                dataConfig.SavePath = txtDest.Text;
                dataConfig.EntryPoint = txtSrc.Text;
                dataConfig.ExcludeFiles = txtExpt.Text;
                dataConfig.CustomVer = chkCustomVer.Checked;
                dataConfig.CustomVersion = txtVerNo.Text;
                dataConfig.UpdatedFiles = txtUpdatedFiles.Text;
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
                txtSrc.Text = filePath; //��TextBox����ʾ��һ���ļ�·����
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
    }
}