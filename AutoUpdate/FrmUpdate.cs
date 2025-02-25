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
    /// Form1 的摘要说明。
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
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;


        public FrmUpdate()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
        }

        /// <summary>
        /// 清理所有正在使用的资源。
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

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
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
            this.btnskipCurrentVersion.Text = "跳过当前版本";
            this.btnskipCurrentVersion.UseVisualStyleBackColor = true;
            this.btnskipCurrentVersion.Click += new System.EventHandler(this.btnskipCurrentVersion_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "以下为更新文件列表";
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
            this.chFileName.Text = "组件名";
            this.chFileName.Width = 123;
            // 
            // chVersion
            // 
            this.chVersion.Text = "版本号";
            this.chVersion.Width = 98;
            // 
            // chProgress
            // 
            this.chProgress.Text = "进度";
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
            this.lbState.Text = "点击“下一步”开始更新文件";
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
            this.btnNext.Text = "下一步(&N)>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消(&C)";
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
            this.label4.Text = "MAXRUINOR软件";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "欢迎以后继续关注我们的产品。";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 48);
            this.label2.TabIndex = 10;
            this.label2.Text = "     程序更新完成,如果程序更新期间被关闭,点击\"完成\"自动更新程序会自动重新启动系统。";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(16, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "感谢使用在线升级";
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
            this.btnFinish.Text = "完成(&F)";
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
            this.Text = "自动更新 2.0.0.9";
            this.Load += new System.EventHandler(this.FrmUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        #region 定义属性
        public bool Debug { get; set; }
        #endregion

        private string updateUrl = string.Empty;
        private string tempUpdatePath = string.Empty;
        XmlFiles updaterXmlFiles = null;
        private int availableUpdate = 0;
        bool isRun = false;


        /// <summary>
        /// 更新文件中指定的启动程序名
        /// </summary>
        string mainAppExe = "";

        public string currentexeName = Assembly.GetExecutingAssembly().ManifestModule.ToString();

        ///保存更新文件列表，key:为当前执行文件的目录及文件名，value为更新的对应版本的目录及文件名
        private List<KeyValuePair<string, string>> filesList = new List<KeyValuePair<string, string>>();

        private List<string> versionDirList = new List<string>();

        string localXmlFile = Application.StartupPath + "\\AutoUpdaterList.xml";
        string serverXmlFile = string.Empty;

        // 定义文件路径
        private string filePath = "UpdateLog.txt";
        private string debugfilePath = "UpdateLog.log";
        /// <summary>
        /// 启动加载这个窗体时。会在当前目录下生成一个文本文件里面写入值
        /// 当点下一步时写入值“升级”
        /// 当点取消时写入值“取消升级”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUpdate_Load(object sender, System.EventArgs e)
        {
            // 在当前目录下创建或打开文件
            File.WriteAllText(filePath, "准备升级");

            btnskipCurrentVersion.Visible = SkipCurrentVersion;

            //判断是否有自己更新自己的旧文件
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + currentexeName + ".delete"))
            {
                System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + currentexeName + ".delete");
            }

            panel2.Visible = false;
            btnFinish.Visible = false;
            linkLabel1.Visible = false;
            try
            {
                //从本地读取更新配置文件信息
                updaterXmlFiles = new XmlFiles(localXmlFile);
                string debug = updaterXmlFiles.GetNodeValue("//Debug");
                if (debug == "1")
                {
                    Debug = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置文件出错!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message + ex.StackTrace);
                MessageBox.Show(localXmlFile);
                // HLH.Lib.Helper.log4netHelper.info(localXmlFile);
                this.Close();
                return;
            }
            //获取服务器地址
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");

            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = updateUrl + "/AutoUpdaterList.xml";

            //与服务器连接,下载更新配置文件
            try
            {
                //当前目录建立UpdaterData
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
                // MessageBox.Show("与服务器连接失败,操作超时!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;

            }

            //获取更新文件列表
            //Hashtable htUpdateFile = new Hashtable();
            serverXmlFile = Path.Combine(tempUpdatePath, "AutoUpdaterList.xml");
            if (!File.Exists(serverXmlFile))
            {
                return;
            }

            //availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);

            //比较两个文件配置差异。下载更新文件
            availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);
            NewVersion = appUpdater.NewVersion;
            //找到了差异的文件集合显示的UI中，等待用户点击下一步
            if (availableUpdate > 0)
            {
                List<string> contents = new List<string>();
                for (int i = 0; i < htUpdateFile.Count; i++)
                {
                    string[] fileArray = (string[])htUpdateFile[i];
                    lvUpdateList.Items.Add(new ListViewItem(fileArray));
                    // 在当前目录下创建或打开文件
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

            File.WriteAllText(filePath, "取消升级");
            this.Close();
            Application.ExitThread();
            Application.Exit();
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            if (availableUpdate > 0)
            {

                File.WriteAllText(filePath, "升级中");

                btnNext.Enabled = false;
                try
                {
                    //保存一份旧的

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
                MessageBox.Show("没有可用的更新!", "自动更新", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    MessageBox.Show("系统找不到指定文件的路径：" + mainAppExe, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
                this.Dispose();
            }
        }

        /// <summary>
        /// 下载更新文件 并且保存到了临时文件夹中
        /// 临时文件夹：以版本号为区别命名了by watson 2024-08-28
        /// </summary>
        private void DownUpdateFile()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //下载更新前 杀掉主程序进程
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
                    //取版本号。后面当目录用
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
                        string content = System.DateTime.Now.ToString() + "准备下载" + updateFileUrl;
                        WebRequest webReq = WebRequest.Create(updateFileUrl);
                        WebResponse webRes = webReq.GetResponse();
                        fileLength = webRes.ContentLength;
                        content += "fileLength:" + fileLength;
                        lbState.Text = "正在下载更新文件,请稍后...";
                        pbDownFile.Value = 0;
                        if ((int)fileLength < 0)
                        {
                            MessageBox.Show("跳过：" + updateFileUrl);
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

                                MessageBox.Show($"下载更新文件时失败！\r\n{UpdateFile}" + exStr.Message.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        string tempPath = tempUpdatePath + "\\" + VerNo + "\\" + UpdateFile;

                        filesList.Add(new KeyValuePair<string, string>(AppDomain.CurrentDomain.BaseDirectory + UpdateFile, tempPath));

                        //添加对应的版本目录
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
                        content += " 状态:下载完成";
                        contents.Add(System.DateTime.Now.ToString() + " " + content);
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show($"下载更新文件失败！\r\n{UpdateFile}" + ex.Message.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }

                AppendAllLines(contents);

            }
            catch (Exception exx)
            {
                MessageBox.Show("更新文件列表下载失败！" + exx.Message.ToString() + "\r\n" + exx.StackTrace, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            InvalidateControl();
            this.Cursor = Cursors.Default;
        }
        //创建目录
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
        /// 追加文本内容
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
        /// 复制文件  将版本号下面的文件 全部
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
                    #region 复制文件
                    //前面处理了自己更新自己，这时如果是自己则不处理
                    if (files[i] == sourcePath + @"\" + currentexeName)
                    {
                        //MessageBox.Show("不复制自己");
                        contents.Add(System.DateTime.Now.ToString() + "不复制自己:" + files[i]);
                        continue;
                    }
                    //http://sevenzipsharp.codeplex.com/
                    //如果为压缩文件，则解压，否则直接复制
                    string fileName = System.IO.Path.GetFileName(files[i]);
                    if (System.IO.Path.GetExtension(fileName).ToLower() == ".zip")
                    {
                        ///这个默认方法。存在时会出错。不能覆盖
                        //System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.Path.Combine(sourcePath, fileName), objPath); //解压
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
                        contents.Add(System.DateTime.Now.ToString() + "复制文件成功:" + files[i]);
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
                contents.Add(System.DateTime.Now.ToString() + "复制目录成功:" + dirs[i]);
            }
            AppendAllLines(contents);
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        /// <summary>
        /// 保留最多最新的版本数量
        /// </summary>
        public int MaxVersionCount = 3;

        public int mainResult = 0;
        //点击完成复制更新文件到应用程序目录
        private void btnFinish_Click(object sender, System.EventArgs e)
        {
            try
            {
                File.WriteAllText(filePath, "升级完成");
                //下载完成后，copy文件 ,将下载到临时文件夹中的最新的文件复制到应用程序目录中生效再启动
                //CopyFile(tempUpdatePath, Directory.GetCurrentDirectory());
                //System.IO.Directory.Delete(tempUpdatePath, true);

                for (int i = 0; i < versionDirList.Count; i++)
                {
                    CopyFile((tempUpdatePath + versionDirList[i]), Directory.GetCurrentDirectory());
                }
                AppendAllText("复制完成");

                try
                {
                    #region 为了实现版本回滚只保留5个版本
                    List<string> versions = new List<string>();
                    // 获取所有子文件夹的路径
                    string[] subDirectories = Directory.GetDirectories(tempUpdatePath);
                    foreach (var subdir in subDirectories)
                    {
                        string verDir = Path.GetFileName(subdir);
                        versions.Add(verDir);
                    }

                    if (versions.Count > 0)
                    {
                        //删除最旧的版本
                        // 对版本号进行排序
                        versions.Sort();
                        int deleteCount = versions.Count - MaxVersionCount;
                        // 移除最小的 保留最新的5个
                        versions = versions.Take(deleteCount).ToList();

                        // 输出排序并移除多余的版本号后的列表
                        foreach (var version in versions)
                        {
                            System.IO.Directory.Delete(tempUpdatePath + version, true);
                        }
                    }
                    #endregion
                }
                catch (Exception exx)
                {
                    AppendAllText(" 删除出错" + exx.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }



            //如果下载下来的文件有自己autoUpdate，则重命名 自动更新程序的自我更新，最后处理。前面的要排除
            //在主程序中处理自我更新。实际是A->B    B->A
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

            //全部更新完成后。配置文件也要更新过来
            File.Copy(serverXmlFile, localXmlFile, true);

            StartEntryPointExe(NewVersion);
            mainResult = 0;
            this.Close();
            this.Dispose();
        }

        //重新绘制窗体部分控件属性
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


        /// 解压文件(不带密码) RAR压缩程序  返回解压出来的文件数量
        /// </summary>
        /// <param name="destPath">解压至目录</param>
        /// <param name="rarfilePath">压缩文件路径</param>
        public static int RARToFileEmail(string destPath, string rarfilePath)
        {
            try
            {
                //组合出需要shell的完整格式
                string shellArguments = string.Format("x -o+ \"{0}\" \"{1}\\\"",
                    rarfilePath, destPath);

                //用Process调用
                using (Process unrar = new Process())
                {
                    unrar.StartInfo.FileName = "winrar.exe";
                    unrar.StartInfo.Arguments = shellArguments;
                    //隐藏rar本身的窗口
                    unrar.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    unrar.Start();
                    //等待解压完成
                    unrar.WaitForExit();
                    unrar.Close();
                }
                //统计解压后的目录和文件数
                //string str=string.Format("解压完成，共解压出：{0}个目录，{1}个文件",
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


        //判断主应用程序是否正在运行
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
                    if (p.ProcessName.ToLower() + ".exe" == "企业数字化集成ERP.exe".ToLower())
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


        #region 外部调用

        /// <summary>
        /// 手工单次调用，出错会弹出
        /// </summary>
        /// <returns></returns>
        public bool CheckHasUpdates()
        {
            //代理来自己load事件中

            string localXmlFile = Application.StartupPath + "\\AutoUpdaterList.xml";
            string serverXmlFile = string.Empty;


            try
            {
                //从本地读取更新配置文件信息
                updaterXmlFiles = new XmlFiles(localXmlFile);
            }
            catch (Exception ex)
            {
                throw new Exception("配置文件出错" + ex.ToString());
            }
            //获取服务器地址
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");

            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = updateUrl + "/AutoUpdaterList.xml";

            //与服务器连接,下载更新配置文件
            try
            {
                //当前目录建立UpdaterData
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

                throw new Exception("与服务器连接失败,操作超时" + ex.ToString());

            }

            //获取更新文件列表
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
        /// 自动多次调用，出错不弹出，只返回提示
        /// </summary>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool CheckHasUpdates(out string errormsg)
        {
            errormsg = string.Empty;
            //代理来自己load事件中

            string localXmlFile = Application.StartupPath + "\\AutoUpdaterList.xml";
            string serverXmlFile = string.Empty;


            try
            {
                //从本地读取更新配置文件信息
                updaterXmlFiles = new XmlFiles(localXmlFile);
            }
            catch (Exception ex)
            {

                throw new Exception("配置文件出错" + ex.ToString());
            }
            //获取服务器地址
            updateUrl = updaterXmlFiles.GetNodeValue("//Url");

            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = updateUrl + "/AutoUpdaterList.xml";

            //与服务器连接,下载更新配置文件
            try
            {
                //当前目录建立UpdaterData
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

                errormsg = "与服务器连接失败,操作超时" + ex.ToString();
                return false;
            }

            //获取更新文件列表
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
        /// 下载文件列表
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

                //获取更新文件列表



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
                        //  MessageBox.Show("更新文件下载失败！" + ex.Message.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception exx)
            {
                throw exx;
                // MessageBox.Show("更新文件下载失败！" + exx.Message.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            InvalidateControl();
            this.Cursor = Cursors.Default;

        }

        /// <summary>
        /// 无介面更新的调用方法。但是没有使用。因为要优化
        /// </summary>
        public void ApplyApp()
        {
            //如果下载下来的文件有自己，则重命名
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
                //下载完成后，copy文件
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
        /// 为了想无介面更新 暂时还没有完成。无介面是不是需要显示进度到调用的程序中呢？
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
                // 要传递给程序的参数
                string arguments = tempUpdatePath;
                // 将数组转换为以"|"分隔的字符串
                arguments = String.Join("|", args);
                ProcessStartInfo startInfo = new ProcessStartInfo(mainAppExe, arguments);
                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show("系统找不到指定文件的路径：" + mainAppExe, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //MessageBox.Show(mainAppExe);

            mainResult = 0;
        }
        #endregion


        private bool skipCurrentVersion = false;

        /// <summary>
        /// 是否跳过当前版本，如果更新操作成功后。设置为false,当强制，主动推送更新时失效
        /// </summary>
        public bool SkipCurrentVersion
        {
            get { return skipCurrentVersion; }
            set { skipCurrentVersion = value; }
        }


        private string NewVersion = string.Empty;

        private void btnskipCurrentVersion_Click(object sender, EventArgs e)
        {
            File.WriteAllText(filePath, "跳过当前版本");
            mainResult = -9;
            SkipCurrentVersion = true;
            this.Close();
        }



    }
}
