using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.IO.Compression;
using AutoUpdate;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.Remoting.Contexts;

namespace AutoUpdate
{
    /// <summary>
    /// Form1 ��ժҪ˵����
    /// </summary>
    public class FrmUpdate : System.Windows.Forms.Form
    {
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader chFileName;
        private System.Windows.Forms.ColumnHeader chVersion;
        private System.Windows.Forms.ColumnHeader chProgress;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvUpdateList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lbState;
        private System.Windows.Forms.ProgressBar pbDownFile;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnFinish;
        private Button btnskipCurrentVersion;

        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;


        public FrmUpdate()
        {
            //
            // Windows ���������֧���������
            //
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            //
            // TODO: �� InitializeComponent ���ú�����κι��캯������
            //
        }

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows ������������ɵĴ���
        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpdate));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnskipCurrentVersion = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvUpdateList = new System.Windows.Forms.ListView();
            this.chFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pbDownFile = new System.Windows.Forms.ProgressBar();
            this.lbState = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnFinish = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(96, 240);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnskipCurrentVersion);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.lvUpdateList);
            this.panel1.Controls.Add(this.pbDownFile);
            this.panel1.Controls.Add(this.lbState);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(120, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 240);
            this.panel1.TabIndex = 2;
            // 
            // btnskipCurrentVersion
            // 
            this.btnskipCurrentVersion.Location = new System.Drawing.Point(168, 8);
            this.btnskipCurrentVersion.Name = "btnskipCurrentVersion";
            this.btnskipCurrentVersion.Size = new System.Drawing.Size(88, 23);
            this.btnskipCurrentVersion.TabIndex = 10;
            this.btnskipCurrentVersion.Text = "������ǰ�汾";
            this.btnskipCurrentVersion.UseVisualStyleBackColor = true;
            this.btnskipCurrentVersion.Click += new System.EventHandler(this.btnskipCurrentVersion_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "����Ϊ�����ļ��б�";
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 2);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // lvUpdateList
            // 
            this.lvUpdateList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFileName,
            this.chVersion,
            this.chProgress});
            this.lvUpdateList.HideSelection = false;
            this.lvUpdateList.Location = new System.Drawing.Point(3, 48);
            this.lvUpdateList.Name = "lvUpdateList";
            this.lvUpdateList.Size = new System.Drawing.Size(272, 120);
            this.lvUpdateList.TabIndex = 6;
            this.lvUpdateList.UseCompatibleStateImageBehavior = false;
            this.lvUpdateList.View = System.Windows.Forms.View.Details;
            // 
            // chFileName
            // 
            this.chFileName.Text = "�����";
            this.chFileName.Width = 123;
            // 
            // chVersion
            // 
            this.chVersion.Text = "�汾��";
            this.chVersion.Width = 98;
            // 
            // chProgress
            // 
            this.chProgress.Text = "����";
            this.chProgress.Width = 47;
            // 
            // pbDownFile
            // 
            this.pbDownFile.Location = new System.Drawing.Point(3, 200);
            this.pbDownFile.Name = "pbDownFile";
            this.pbDownFile.Size = new System.Drawing.Size(274, 17);
            this.pbDownFile.TabIndex = 5;
            // 
            // lbState
            // 
            this.lbState.Location = new System.Drawing.Point(3, 176);
            this.lbState.Name = "lbState";
            this.lbState.Size = new System.Drawing.Size(240, 16);
            this.lbState.TabIndex = 4;
            this.lbState.Text = "�������һ������ʼ�����ļ�";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 8);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(224, 264);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 24);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "��һ��(&N)>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "ȡ��(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Location = new System.Drawing.Point(13, 292);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(387, 303);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(144, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "MAXRUINOR���";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "��ӭ�Ժ������ע���ǵĲ�Ʒ��";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 48);
            this.label2.TabIndex = 10;
            this.label2.Text = "     ����������,�����������ڼ䱻�ر�,���\"���\"�Զ����³�����Զ���������ϵͳ��";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(16, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "��лʹ����������";
            // 
            // groupBox3
            // 
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 301);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(387, 2);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox2";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.linkLabel1);
            this.groupBox4.Location = new System.Drawing.Point(0, 32);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(280, 8);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(16, -10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(98, 18);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.maxruinor.com";
            this.linkLabel1.Visible = false;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(136, 264);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(80, 24);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "���(&F)";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // FrmUpdate
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(410, 291);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnFinish);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "�Զ����� 2.0.0.9";
            this.Load += new System.EventHandler(this.FrmUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        #region ��������
        public bool Debug { get; set; }
        #endregion

        private string updateUrl = string.Empty;
        private string tempUpdatePath = string.Empty;
        XmlFiles updaterXmlFiles = null;
        private int availableUpdate = 0;
        bool isRun = false;


        /// <summary>
        /// �����ļ���ָ��������������
        /// </summary>
        string mainAppExe = "";

        public string currentexeName = Assembly.GetExecutingAssembly().ManifestModule.ToString();

        ///��������ļ��б�key:Ϊ��ǰִ���ļ���Ŀ¼���ļ�����valueΪ���µĶ�Ӧ�汾��Ŀ¼���ļ���
        private List<KeyValuePair<string, string>> filesList = new List<KeyValuePair<string, string>>();

        private List<string> versionDirList = new List<string>();

        string localXmlFile = Application.StartupPath + "\\AutoUpdaterList.xml";
        string serverXmlFile = string.Empty;

        // �����ļ�·��
        private string filePath = "UpdateLog.txt";
        private string debugfilePath = "UpdateLog.log";
        /// <summary>
        /// ���������������ʱ�����ڵ�ǰĿ¼������һ���ı��ļ�����д��ֵ
        /// ������һ��ʱд��ֵ��������
        /// ����ȡ��ʱд��ֵ��ȡ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUpdate_Load(object sender, System.EventArgs e)
        {
            // �ڵ�ǰĿ¼�´�������ļ�
            File.WriteAllText(filePath, "׼������");

            btnskipCurrentVersion.Visible = SkipCurrentVersion;

            //�ж��Ƿ����Լ������Լ��ľ��ļ�
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + currentexeName + ".delete"))
            {
                System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + currentexeName + ".delete");
            }

            panel2.Visible = false;
            btnFinish.Visible = false;
            linkLabel1.Visible = false;
            try
            {
                //�ӱ��ض�ȡ���������ļ���Ϣ
                updaterXmlFiles = new XmlFiles(localXmlFile);
                string debug = updaterXmlFiles.GetNodeValue("//Debug");
                if (debug == "1")
                {
                    Debug = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�����ļ�����!", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message + ex.StackTrace);
                MessageBox.Show(localXmlFile);
                // HLH.Lib.Helper.log4netHelper.info(localXmlFile);
                this.Close();
                return;
            }
            //��ȡ��������ַ
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");

            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = updateUrl + "/AutoUpdaterList.xml";

            //�����������,���ظ��������ļ�
            try
            {
                //��ǰĿ¼����UpdaterData
                string updateDataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdaterData");

                if (!System.IO.Directory.Exists(updateDataPath))
                {
                    System.IO.Directory.CreateDirectory(updateDataPath);
                }

                tempUpdatePath = updateDataPath + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
                //tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
                appUpdater.DownAutoUpdateFile(tempUpdatePath);
            }
            catch
            {
                // MessageBox.Show("�����������ʧ��,������ʱ!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;

            }

            //��ȡ�����ļ��б�
            //Hashtable htUpdateFile = new Hashtable();
            serverXmlFile = Path.Combine(tempUpdatePath, "AutoUpdaterList.xml");
            if (!File.Exists(serverXmlFile))
            {
                return;
            }

            //availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);

            //�Ƚ������ļ����ò��졣���ظ����ļ�
            availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);
            NewVersion = appUpdater.NewVersion;
            //�ҵ��˲�����ļ�������ʾ��UI�У��ȴ��û������һ��
            if (availableUpdate > 0)
            {
                List<string> contents = new List<string>();
                for (int i = 0; i < htUpdateFile.Count; i++)
                {
                    string[] fileArray = (string[])htUpdateFile[i];
                    lvUpdateList.Items.Add(new ListViewItem(fileArray));
                    // �ڵ�ǰĿ¼�´�������ļ�
                    string content = string.Join(",", fileArray);
                    contents.Add(content);
                }

                //AppendAllLines(contents);

            }
            else
            {
                this.Visible = false;
                StartEntryPointExe(NewVersion);
                this.Close();
            }
            //else
            //    btnNext.Enabled = false;
        }






        private void btnCancel_Click(object sender, System.EventArgs e)
        {

            File.WriteAllText(filePath, "ȡ������");
            this.Close();
            Application.ExitThread();
            Application.Exit();
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            if (availableUpdate > 0)
            {

                File.WriteAllText(filePath, "������");

                btnNext.Enabled = false;
                try
                {
                    //����һ�ݾɵ�

                    XDocument doc = XDocument.Load(localXmlFile);
                    doc.Save(Application.StartupPath + "\\AutoUpdaterList_back.xml");

                    Thread threadDown = new Thread(new ThreadStart(DownUpdateFile));
                    threadDown.IsBackground = true;
                    threadDown.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Exit();
                }

            }
            else
            {
                MessageBox.Show("û�п��õĸ���!", "�Զ�����", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (string.IsNullOrEmpty(mainAppExe))
                {
                    mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
                }
                //return;
                if (System.IO.File.Exists(mainAppExe))
                {
                    Process.Start(mainAppExe);
                }
                else
                {
                    MessageBox.Show("ϵͳ�Ҳ���ָ���ļ���·����" + mainAppExe, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
                this.Dispose();
            }
        }

        /// <summary>
        /// ���ظ����ļ� ���ұ��浽����ʱ�ļ�����
        /// ��ʱ�ļ��У��԰汾��Ϊ����������by watson 2024-08-28
        /// </summary>
        private void DownUpdateFile()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //���ظ���ǰ ɱ�����������
                mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
                Process[] allProcess = Process.GetProcesses();
                foreach (Process p in allProcess)
                {
                    // MessageBox.Show(p.ProcessName.ToLower());
                    if (p.ProcessName.ToLower() + ".exe" == mainAppExe.ToLower())
                    {
                        for (int i = 0; i < p.Threads.Count; i++)
                            p.Threads[i].Dispose();
                        p.Kill();
                        Thread.Sleep(500);
                        isRun = true;
                        //break;
                    }
                }

                List<string> contents = new List<string>();

                WebClient wcClient = new WebClient();
                for (int i = 0; i < this.lvUpdateList.Items.Count; i++)
                {
                    string UpdateFile = lvUpdateList.Items[i].Text.Trim();
                    //ȡ�汾�š����浱Ŀ¼��
                    string VerNo = string.Empty;
                    ListViewItem listViewItem = lvUpdateList.Items[i];
                    if (listViewItem.SubItems.Count > 1)
                    {
                        VerNo = listViewItem.SubItems[1].Text.Trim();
                    }
                    try
                    {
                        string updateFileUrl = updateUrl + lvUpdateList.Items[i].Text.Trim();
                        long fileLength = 0;
                        string content = System.DateTime.Now.ToString() + "׼������" + updateFileUrl;
                        WebRequest webReq = WebRequest.Create(updateFileUrl);
                        WebResponse webRes = webReq.GetResponse();
                        fileLength = webRes.ContentLength;
                        content += "fileLength:" + fileLength;
                        lbState.Text = "�������ظ����ļ�,���Ժ�...";
                        pbDownFile.Value = 0;
                        if ((int)fileLength < 0)
                        {
                            MessageBox.Show("������" + updateFileUrl);
                            continue;
                        }
                        pbDownFile.Maximum = (int)fileLength;
                        Stream srm = webRes.GetResponseStream();
                        StreamReader srmReader = new StreamReader(srm);
                        byte[] bufferbyte = new byte[fileLength];
                        int allByte = (int)bufferbyte.Length;
                        int startByte = 0;
                        while (fileLength > 0)
                        {
                            try
                            {
                                Application.DoEvents();
                                int downByte = srm.Read(bufferbyte, startByte, allByte);
                                if (downByte == 0) { break; };
                                startByte += downByte;
                                allByte -= downByte;
                                pbDownFile.Value += downByte;

                                float part = (float)startByte / 1024;
                                float total = (float)bufferbyte.Length / 1024;
                                int percent = Convert.ToInt32((part / total) * 100);

                                this.lvUpdateList.Items[i].SubItems[2].Text = percent.ToString() + "%";
                            }
                            catch (Exception exStr)
                            {

                                MessageBox.Show($"���ظ����ļ�ʱʧ�ܣ�\r\n{UpdateFile}" + exStr.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        string tempPath = tempUpdatePath + "\\" + VerNo + "\\" + UpdateFile;

                        filesList.Add(new KeyValuePair<string, string>(AppDomain.CurrentDomain.BaseDirectory + UpdateFile, tempPath));

                        //��Ӷ�Ӧ�İ汾Ŀ¼
                        if (!versionDirList.Contains(VerNo))
                        {
                            versionDirList.Add(VerNo);
                        }

                        CreateDirtory(tempPath);
                        FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write);
                        fs.Write(bufferbyte, 0, bufferbyte.Length);
                        srm.Close();
                        srmReader.Close();
                        fs.Close();
                        content += " ״̬:�������";
                        contents.Add(System.DateTime.Now.ToString() + " " + content);
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show($"���ظ����ļ�ʧ�ܣ�\r\n{UpdateFile}" + ex.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }

                AppendAllLines(contents);

            }
            catch (Exception exx)
            {
                MessageBox.Show("�����ļ��б�����ʧ�ܣ�" + exx.Message.ToString() + "\r\n" + exx.StackTrace, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            InvalidateControl();
            this.Cursor = Cursors.Default;
        }
        //����Ŀ¼
        private void CreateDirtory(string path)
        {
            if (!File.Exists(path))
            {
                string[] dirArray = path.Split('\\');
                string temp = string.Empty;
                for (int i = 0; i < dirArray.Length - 1; i++)
                {
                    temp += dirArray[i].Trim() + "\\";
                    if (!Directory.Exists(temp))
                        Directory.CreateDirectory(temp);
                }
            }
        }

        /// <summary>
        /// ׷���ı�����
        /// </summary>
        /// <param name="content"></param>
        public void AppendAllText(string content)
        {
            if (Debug)
            {
                File.AppendAllText(debugfilePath, System.DateTime.Now.ToString() + content);
            }
        }

        public void AppendAllLines(List<string> contents)
        {
            if (Debug)
            {
                File.AppendAllLines(debugfilePath, contents);
            }
        }

        /// <summary>
        /// �����ļ�  ���汾��������ļ� ȫ��
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="objPath"></param>
        public void CopyFile(string sourcePath, string objPath)
        {
            List<string> contents = new List<string>();

            //			char[] split = @"\".ToCharArray();
            if (!Directory.Exists(objPath))
            {
                Directory.CreateDirectory(objPath);
            }
            string[] files = Directory.GetFiles(sourcePath);
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    #region �����ļ�
                    //ǰ�洦�����Լ������Լ�����ʱ������Լ��򲻴���
                    if (files[i] == sourcePath + @"\" + currentexeName)
                    {
                        //MessageBox.Show("�������Լ�");
                        contents.Add(System.DateTime.Now.ToString() + "�������Լ�:" + files[i]);
                        continue;
                    }
                    //http://sevenzipsharp.codeplex.com/
                    //���Ϊѹ���ļ������ѹ������ֱ�Ӹ���
                    string fileName = System.IO.Path.GetFileName(files[i]);
                    if (System.IO.Path.GetExtension(fileName).ToLower() == ".zip")
                    {
                        ///���Ĭ�Ϸ���������ʱ��������ܸ���
                        //System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.Path.Combine(sourcePath, fileName), objPath); //��ѹ
                        string zipPathWithName = System.IO.Path.Combine(sourcePath, fileName);
                        //MessageBox.Show("zipPathWithName:" + zipPathWithName);
                        //MessageBox.Show("objPath:" + objPath);
                        using (ZipArchive archive = ZipFile.OpenRead(zipPathWithName))
                        {
                            archive.ExtractToDirectory(objPath, true);
                        }
                    }
                    else if (System.IO.Path.GetExtension(fileName).ToLower() == ".rar")
                    {
                        //using (SevenZipExtractor tmp = new SevenZipExtractor(System.IO.Path.Combine(sourcePath, fileName)))
                        //{
                        //    for (int f = 0; f < tmp.ArchiveFileData.Count; f++)
                        //    {
                        //        tmp.ExtractFiles(objPath, tmp.ArchiveFileData[f].Index);
                        //    }
                        //}

                        RARToFileEmail(objPath, System.IO.Path.Combine(sourcePath, fileName));

                        //using (SevenZipExtractor tmp = new SevenZipExtractor(System.IO.Path.Combine(sourcePath, fileName)))
                        //{
                        //    for (int f = 0; f < tmp.ArchiveFileData.Count; f++)
                        //    {
                        //        tmp.ExtractFiles(objPath, tmp.ArchiveFileData[f].Index);
                        //    }
                        //}


                    }
                    else
                    {
                        File.Copy(files[i], System.IO.Path.Combine(objPath, fileName), true);
                        contents.Add(System.DateTime.Now.ToString() + "�����ļ��ɹ�:" + files[i]);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            string[] dirs = Directory.GetDirectories(sourcePath);
            for (int i = 0; i < dirs.Length; i++)
            {
                string[] childdir = dirs[i].Split('\\');
                CopyFile(dirs[i], objPath + @"\" + childdir[childdir.Length - 1]);
                contents.Add(System.DateTime.Now.ToString() + "����Ŀ¼�ɹ�:" + dirs[i]);
            }
            AppendAllLines(contents);
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        /// <summary>
        /// ����������µİ汾����
        /// </summary>
        public int MaxVersionCount = 3;

        public int mainResult = 0;
        //�����ɸ��Ƹ����ļ���Ӧ�ó���Ŀ¼
        private void btnFinish_Click(object sender, System.EventArgs e)
        {
            try
            {
                File.WriteAllText(filePath, "�������");
                //������ɺ�copy�ļ� ,�����ص���ʱ�ļ����е����µ��ļ����Ƶ�Ӧ�ó���Ŀ¼����Ч������
                //CopyFile(tempUpdatePath, Directory.GetCurrentDirectory());
                //System.IO.Directory.Delete(tempUpdatePath, true);

                for (int i = 0; i < versionDirList.Count; i++)
                {
                    CopyFile((tempUpdatePath + versionDirList[i]), Directory.GetCurrentDirectory());
                }
                AppendAllText("�������");

                try
                {
                    #region Ϊ��ʵ�ְ汾�ع�ֻ����5���汾
                    List<string> versions = new List<string>();
                    // ��ȡ�������ļ��е�·��
                    string[] subDirectories = Directory.GetDirectories(tempUpdatePath);
                    foreach (var subdir in subDirectories)
                    {
                        string verDir = Path.GetFileName(subdir);
                        versions.Add(verDir);
                    }

                    if (versions.Count > 0)
                    {
                        //ɾ����ɵİ汾
                        // �԰汾�Ž�������
                        versions.Sort();
                        int deleteCount = versions.Count - MaxVersionCount;
                        // �Ƴ���С�� �������µ�5��
                        versions = versions.Take(deleteCount).ToList();

                        // ��������Ƴ�����İ汾�ź���б�
                        foreach (var version in versions)
                        {
                            System.IO.Directory.Delete(tempUpdatePath + version, true);
                        }
                    }
                    #endregion
                }
                catch (Exception exx)
                {
                    AppendAllText(" ɾ������" + exx.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }



            //��������������ļ����Լ�autoUpdate���������� �Զ����³�������Ҹ��£������ǰ���Ҫ�ų�
            //���������д������Ҹ��¡�ʵ����A->B    B->A
            string autoupdatePath = string.Empty;
            var autoupdate = filesList.Where(c => c.Key.Contains(currentexeName)).FirstOrDefault();
            if (autoupdate.Key != null)
            {
                autoupdatePath = autoupdate.Key + "|" + autoupdate.Value;
                string filename = Assembly.GetExecutingAssembly().Location;
                if (System.IO.File.Exists(filename + ".delete"))
                {
                    System.IO.File.Delete(filename + ".delete");
                }
                File.Move(filename, filename + ".delete");
                File.Copy(autoupdate.Value, filename);
                //ProcessStartInfo p = new ProcessStartInfo();
                //p.FileName = "AutoUpdateSelf.exe";
                //p.Arguments = autoupdatePath;
                //p.WindowStyle = ProcessWindowStyle.Hidden;
                //Process.Start(p);
            }


            /*
            if (System.IO.File.Exists(System.IO.Path.Combine(tempUpdatePath, currentexeName)))
            {
                string filename = Assembly.GetExecutingAssembly().Location;
                if (System.IO.File.Exists(filename + ".delete"))
                {
                    System.IO.File.Delete(filename + ".delete");
                }
                File.Move(filename, filename + ".delete");                
                File.Copy(System.IO.Path.Combine(tempUpdatePath, currentexeName), filename);              
                // Application.Restart();
            }
            */

            //ȫ��������ɺ������ļ�ҲҪ���¹���
            File.Copy(serverXmlFile, localXmlFile, true);

            StartEntryPointExe(NewVersion);
            mainResult = 0;
            this.Close();
            this.Dispose();
        }

        //���»��ƴ��岿�ֿؼ�����
        private void InvalidateControl()
        {
            panel2.Location = panel1.Location;
            panel2.Size = panel1.Size;
            panel1.Visible = false;
            panel2.Visible = true;

            btnNext.Visible = false;
            btnCancel.Visible = false;
            btnFinish.Location = btnCancel.Location;
            btnFinish.Visible = true;
            linkLabel1.Visible = true;
        }


        /// ��ѹ�ļ�(��������) RARѹ������  ���ؽ�ѹ�������ļ�����
        /// </summary>
        /// <param name="destPath">��ѹ��Ŀ¼</param>
        /// <param name="rarfilePath">ѹ���ļ�·��</param>
        public static int RARToFileEmail(string destPath, string rarfilePath)
        {
            try
            {
                //��ϳ���Ҫshell��������ʽ
                string shellArguments = string.Format("x -o+ \"{0}\" \"{1}\\\"",
                    rarfilePath, destPath);

                //��Process����
                using (Process unrar = new Process())
                {
                    unrar.StartInfo.FileName = "winrar.exe";
                    unrar.StartInfo.Arguments = shellArguments;
                    //����rar����Ĵ���
                    unrar.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    unrar.Start();
                    //�ȴ���ѹ���
                    unrar.WaitForExit();
                    unrar.Close();
                }
                //ͳ�ƽ�ѹ���Ŀ¼���ļ���
                //string str=string.Format("��ѹ��ɣ�����ѹ����{0}��Ŀ¼��{1}���ļ�",
                //    di.GetDirectories().Length, di.GetFiles().Length);
                //return str;
            }
            catch (Exception ex)
            {
                return 0;
            }
            DirectoryInfo di = new DirectoryInfo(destPath);
            int dirfileCount = 0;
            foreach (System.IO.DirectoryInfo dir in di.GetDirectories())
            {
                dirfileCount++;
            }
            foreach (System.IO.FileInfo item in di.GetFiles())
            {
                dirfileCount++;
            }
            return dirfileCount;
        }


        //�ж���Ӧ�ó����Ƿ���������
        private bool IsMainAppRun()
        {
            string mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
            bool isRun = true;
            Process[] allProcess = Process.GetProcesses();
            foreach (Process p in allProcess)
            {
                if (p.ProcessName.ToLower() + ".exe" == mainAppExe.ToLower())
                {
                    p.Kill();
                    isRun = false;
                    break;
                }
                try
                {
                    if (p.ProcessName.ToLower() + ".exe" == "��ҵ���ֻ�����ERP.exe".ToLower())
                    {
                        p.Kill();
                        isRun = false;
                        break;
                    }
                }
                catch (Exception)
                {

                }

            }
            return isRun;
        }


        #region �ⲿ����

        /// <summary>
        /// �ֹ����ε��ã�����ᵯ��
        /// </summary>
        /// <returns></returns>
        public bool CheckHasUpdates()
        {
            //�������Լ�load�¼���

            string localXmlFile = Application.StartupPath + "\\AutoUpdaterList.xml";
            string serverXmlFile = string.Empty;


            try
            {
                //�ӱ��ض�ȡ���������ļ���Ϣ
                updaterXmlFiles = new XmlFiles(localXmlFile);
            }
            catch (Exception ex)
            {
                throw new Exception("�����ļ�����" + ex.ToString());
            }
            //��ȡ��������ַ
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");

            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = updateUrl + "/AutoUpdaterList.xml";

            //�����������,���ظ��������ļ�
            try
            {
                //��ǰĿ¼����UpdaterData
                string updateDataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdaterData");

                if (!System.IO.Directory.Exists(updateDataPath))
                {
                    System.IO.Directory.CreateDirectory(updateDataPath);
                }

                //

                tempUpdatePath = updateDataPath + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
                //tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
                appUpdater.DownAutoUpdateFile(tempUpdatePath);
            }
            catch (Exception ex)
            {

                throw new Exception("�����������ʧ��,������ʱ" + ex.ToString());

            }

            //��ȡ�����ļ��б�
            // Hashtable htUpdateFile = new Hashtable();

            serverXmlFile = tempUpdatePath + "\\AutoUpdaterList.xml";
            if (!File.Exists(serverXmlFile))
            {
                return false;
            }

            availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);
            if (string.IsNullOrEmpty(mainAppExe))
            {
                mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
            }
            if (availableUpdate > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// �Զ���ε��ã�����������ֻ������ʾ
        /// </summary>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool CheckHasUpdates(out string errormsg)
        {
            errormsg = string.Empty;
            //�������Լ�load�¼���

            string localXmlFile = Application.StartupPath + "\\AutoUpdaterList.xml";
            string serverXmlFile = string.Empty;


            try
            {
                //�ӱ��ض�ȡ���������ļ���Ϣ
                updaterXmlFiles = new XmlFiles(localXmlFile);
            }
            catch (Exception ex)
            {

                throw new Exception("�����ļ�����" + ex.ToString());
            }
            //��ȡ��������ַ
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");

            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = updateUrl + "/AutoUpdaterList.xml";

            //�����������,���ظ��������ļ�
            try
            {
                //��ǰĿ¼����UpdaterData
                string updateDataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdaterData");

                if (!System.IO.Directory.Exists(updateDataPath))
                {
                    System.IO.Directory.CreateDirectory(updateDataPath);
                }

                //

                tempUpdatePath = updateDataPath + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
                //tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
                appUpdater.DownAutoUpdateFile(tempUpdatePath);
            }
            catch (Exception ex)
            {

                errormsg = "�����������ʧ��,������ʱ" + ex.ToString();
                return false;
            }

            //��ȡ�����ļ��б�
            // Hashtable htUpdateFile = new Hashtable();

            serverXmlFile = tempUpdatePath + "\\AutoUpdaterList.xml";
            if (!File.Exists(serverXmlFile))
            {
                return false;
            }

            availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);
            if (string.IsNullOrEmpty(mainAppExe))
            {
                mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
            }
            if (availableUpdate > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// �����ļ��б�
        /// </summary>
        Hashtable htUpdateFile = new Hashtable();

        public void UpdateAndDownLoadFile()
        {
            try
            {


                mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
                Process[] allProcess = Process.GetProcesses();
                foreach (Process p in allProcess)
                {

                    if (p.ProcessName.ToLower() + ".exe" == mainAppExe.ToLower())
                    {
                        for (int i = 0; i < p.Threads.Count; i++)
                            p.Threads[i].Dispose();
                        p.Kill();
                        isRun = true;
                        //break;
                    }
                }
                WebClient wcClient = new WebClient();

                //��ȡ�����ļ��б�



                foreach (DictionaryEntry var in htUpdateFile)
                {
                    string[] file = (string[])var.Value;
                    string UpdateFile = file[0].Trim();
                    string updateFileUrl = updateUrl + file[0].Trim();
                    long fileLength = 0;

                    WebRequest webReq = WebRequest.Create(updateFileUrl);
                    WebResponse webRes = webReq.GetResponse();
                    fileLength = webRes.ContentLength;

                    pbDownFile.Value = 0;
                    pbDownFile.Maximum = (int)fileLength;

                    try
                    {
                        Stream srm = webRes.GetResponseStream();
                        StreamReader srmReader = new StreamReader(srm);
                        byte[] bufferbyte = new byte[fileLength];
                        int allByte = (int)bufferbyte.Length;
                        int startByte = 0;
                        while (fileLength > 0)
                        {
                            Application.DoEvents();
                            int downByte = srm.Read(bufferbyte, startByte, allByte);
                            if (downByte == 0) { break; };
                            startByte += downByte;
                            allByte -= downByte;
                            pbDownFile.Value += downByte;

                            float part = (float)startByte / 1024;
                            float total = (float)bufferbyte.Length / 1024;
                            int percent = Convert.ToInt32((part / total) * 100);

                        }

                        string tempPath = tempUpdatePath + UpdateFile;
                        CreateDirtory(tempPath);
                        FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write);
                        fs.Write(bufferbyte, 0, bufferbyte.Length);
                        srm.Close();
                        srmReader.Close();
                        fs.Close();


                    }
                    catch (WebException ex)
                    {
                        throw ex;
                        //  MessageBox.Show("�����ļ�����ʧ�ܣ�" + ex.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception exx)
            {
                throw exx;
                // MessageBox.Show("�����ļ�����ʧ�ܣ�" + exx.Message.ToString(), "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            InvalidateControl();
            this.Cursor = Cursors.Default;

        }

        /// <summary>
        /// �޽�����µĵ��÷���������û��ʹ�á���ΪҪ�Ż�
        /// </summary>
        public void ApplyApp()
        {
            //��������������ļ����Լ�����������
            if (System.IO.File.Exists(System.IO.Path.Combine(tempUpdatePath, currentexeName)))
            {
                string filename = Assembly.GetExecutingAssembly().Location;
                //MessageBox.Show(filename);
                File.Move(filename, filename + ".delete");                    // 1
                File.Copy(System.IO.Path.Combine(tempUpdatePath, currentexeName), filename);                // 2
                // Application.Restart();
            }

            try
            {
                //������ɺ�copy�ļ�
                // CopyFile(tempUpdatePath, Directory.GetCurrentDirectory());

                CopyFile(tempUpdatePath, AppDomain.CurrentDomain.BaseDirectory);
                System.IO.Directory.Delete(tempUpdatePath, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Ϊ�����޽������ ��ʱ��û����ɡ��޽����ǲ�����Ҫ��ʾ���ȵ����õĳ������أ�
        /// </summary>
        public void StartEntryPointExe(params string[] args)
        {

            if (string.IsNullOrEmpty(mainAppExe))
            {
                mainAppExe = updaterXmlFiles.GetNodeValue("//EntryPoint");
            }
            IsMainAppRun();
            //return;
            if (System.IO.File.Exists(mainAppExe))
            {
                //Process.Start(mainAppExe);
                // Ҫ���ݸ�����Ĳ���
                string arguments = tempUpdatePath;
                // ������ת��Ϊ��"|"�ָ����ַ���
                arguments = String.Join("|", args);
                ProcessStartInfo startInfo = new ProcessStartInfo(mainAppExe, arguments);
                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show("ϵͳ�Ҳ���ָ���ļ���·����" + mainAppExe, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //MessageBox.Show(mainAppExe);

            mainResult = 0;
        }
        #endregion


        private bool skipCurrentVersion = false;

        /// <summary>
        /// �Ƿ�������ǰ�汾��������²����ɹ�������Ϊfalse,��ǿ�ƣ��������͸���ʱʧЧ
        /// </summary>
        public bool SkipCurrentVersion
        {
            get { return skipCurrentVersion; }
            set { skipCurrentVersion = value; }
        }


        private string NewVersion = string.Empty;

        private void btnskipCurrentVersion_Click(object sender, EventArgs e)
        {
            File.WriteAllText(filePath, "������ǰ�汾");
            mainResult = -9;
            SkipCurrentVersion = true;
            this.Close();
        }



    }
}
