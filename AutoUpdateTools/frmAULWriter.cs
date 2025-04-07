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
using System.Xml;
using AutoUpdateTools.XmlCompare;
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
        private readonly Color SameColor = Color.FromArgb(240, 240, 240);
        private readonly Color DiffColor = Color.FromArgb(255, 220, 220);
        private readonly Color AddedColor = Color.FromArgb(220, 255, 220);
        private readonly Color RemovedColor = Color.FromArgb(255, 200, 200);
        private readonly Color HeaderColor = Color.FromArgb(230, 230, 255);

        #region [基本入口构造函数]

        public frmAULWriter()
        {
            InitializeComponent();
            SetupRichTextBoxes();
            SetupSafeScrollSync();
        }

        private SafeScrollSynchronizer _syncOldToNew;
        private SafeScrollSynchronizer _syncNewToOld;
        private void SetupSafeScrollSync()
        {
            // 关键：使用单向同步，并交换主从关系
            _syncOldToNew = new SafeScrollSynchronizer(rtbOld, rtbNew);
            _syncNewToOld = new SafeScrollSynchronizer(rtbNew, rtbOld);
        }



        #endregion [基本入口构造函数]

        #region

        private void SetupRichTextBoxes()
        {
            // 设置左右两个RichTextBox的样式
            foreach (var rtb in new[] { rtbOld, rtbNew })
            {
                rtb.Font = new Font("Consolas", 10);
                rtb.WordWrap = false;
                rtb.ScrollBars = RichTextBoxScrollBars.Both;
                rtb.DetectUrls = false;
            }

        }

        public void CompareXmlDocuments(XDocument oldDoc, XDocument newDoc)
        {
            // Get all File elements from both documents
            var oldFiles = oldDoc.Descendants("File").ToList();
            var newFiles = newDoc.Descendants("File").ToList();

            // Create lists of file entries for comparison
            var oldFileEntries = oldFiles.Select(f => $"{f.Attribute("Name")?.Value}|{f.Attribute("Ver")?.Value}").ToArray();
            var newFileEntries = newFiles.Select(f => $"{f.Attribute("Name")?.Value}|{f.Attribute("Ver")?.Value}").ToArray();

            // Configure the diff engine
            var diff = new AdvancedDiff(oldFileEntries, newFileEntries);
            diff.SetLineMatchFunction((a, b) => a.Split('|')[0] == b.Split('|')[0]);

            // Custom inline diff for version changes
            diff.SetInlineDiffFunction((left, right) =>
            {
                var leftParts = left.Split('|');
                var rightParts = right.Split('|');

                if (leftParts[0] == rightParts[0] && leftParts[1] != rightParts[1])
                {
                    return new[]
                    {
                new DiffSegment { Text = leftParts[0], IsModified = false },
                new DiffSegment { Text = "|" + leftParts[1], IsModified = true, RightText = "|" + rightParts[1] }
            };
                }
                return new[] { new DiffSegment { Text = left, IsModified = true, RightText = right } };
            });

            // Compute the differences
            var results = diff.ComputeDiff();

            // Display in RichTextBox controls
            DisplayDifferencesInRichTextBox(rtbOld, results, false);
            DisplayDifferencesInRichTextBox(rtbNew, results, true);
        }

        #region 比较2025-04-06
        private void DisplayDifferencesInRichTextBox(RichTextBox rtb, List<DiffBlock> results, bool showNewVersion)
        {
            rtb.Clear();
            rtb.Font = new Font("Consolas", 10); // Monospaced font for alignment

            foreach (var result in results)
            {
                switch (result.Type)
                {
                    case DiffType.Unchanged:
                        AppendWithColor(rtb, string.Join(Environment.NewLine, result.LeftLines), Color.Black);
                        break;

                    case DiffType.Modified:
                        if (showNewVersion)
                        {
                            if (result.InlineDiffs != null)
                            {
                                foreach (var line in result.RightLines.Zip(result.InlineDiffs, (text, diffs) => new { text, diffs }))
                                {
                                    AppendInlineDiff(rtb, line.diffs, true);
                                    rtb.AppendText(Environment.NewLine);
                                }
                            }
                            else
                            {
                                AppendWithColor(rtb, string.Join(Environment.NewLine, result.RightLines), Color.Green);
                            }
                        }
                        else
                        {
                            if (result.InlineDiffs != null)
                            {
                                foreach (var line in result.LeftLines.Zip(result.InlineDiffs, (text, diffs) => new { text, diffs }))
                                {
                                    AppendInlineDiff(rtb, line.diffs, false);
                                    rtb.AppendText(Environment.NewLine);
                                }
                            }
                            else
                            {
                                AppendWithColor(rtb, string.Join(Environment.NewLine, result.LeftLines), Color.Red);
                            }
                        }
                        break;

                    case DiffType.Added when showNewVersion:
                        AppendWithColor(rtb, string.Join(Environment.NewLine, result.RightLines), Color.Green);
                        break;

                    case DiffType.Removed when !showNewVersion:
                        AppendWithColor(rtb, string.Join(Environment.NewLine, result.LeftLines), Color.Red, true);
                        break;
                }

                rtb.AppendText(Environment.NewLine);
            }
        }

        private void AppendInlineDiff(RichTextBox rtb, IEnumerable<DiffSegment> segments, bool showRight)
        {
            foreach (var segment in segments)
            {
                if (segment.IsModified)
                {
                    var text = showRight && !string.IsNullOrEmpty(segment.RightText) ?
                        segment.RightText : segment.Text;
                    AppendWithColor(rtb, text, Color.Blue);
                }
                else
                {
                    AppendWithColor(rtb, segment.Text, Color.Black);
                }
            }
        }

        private void AppendWithColor(RichTextBox rtb, string text, Color color, bool strikethrough = false)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionColor = color;
            if (strikethrough)
            {
                rtb.SelectionFont = new Font(rtb.Font, FontStyle.Strikeout);
            }

            rtb.AppendText(text);

            rtb.SelectionColor = rtb.ForeColor;
            rtb.SelectionFont = rtb.Font;
        }

        #endregion












        #endregion



        #region [选择文件保存路径]

        private void btnSearDes_Click(object sender, EventArgs e)
        {
            this.sfdDest.ShowDialog(this);
        }

        private void sfdSrcPath_FileOk(object sender, CancelEventArgs e)
        {
            this.txtAutoUpdateXmlSavePath.Text = this.sfdDest.FileName.Substring(0, this.sfdDest.FileName.LastIndexOf(@"\")) + @"\AutoUpdaterList.xml";
        }

        #endregion [选择文件保存路径]

        #region [选择排除文件]

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

        #endregion [选择排除文件]

        #region [选择主程序]

        private void btnSrc_Click(object sender, EventArgs e)
        {
            this.ofdSrc.ShowDialog(this);
        }

        private void ofdDest_FileOk(object sender, CancelEventArgs e)
        {
            this.txtMainExePath.Text = this.ofdSrc.FileName;
        }

        #endregion [选择主程序]

        #region [主窗体加载]

        private void frmAULWriter_Load(object sender, EventArgs e)
        {
            //从配置文件读取配置
            readConfigFromFile();
            btnDiff.Enabled = false;
        }

        //从配置文件读取配置
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
                    txtTargetDirectory.Text = dataConfig.BaseDir;
                    chkUseBaseVersion.Checked = dataConfig.UseBaseExeVersion;
                    txtBaseExeVersion.Text = dataConfig.BaseExeVersion;
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
                    txtPreVerUpdatedFiles.Text = sbupfiles.ToString();

                }
            }
            catch (Exception ex)
            {
                richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":从配置文件读取配置失败:" + ex.Message);
                richTxtLog.AppendText("\r\n");
            }

        }
        #endregion [主窗体加载]

        #region [生成文件]

        private void btnProduce_Click(object sender, EventArgs e)
        {
            UpdateXmlFile(txtAutoUpdateXmlSavePath.Text, txtCompareSource.Text, txtTargetDirectory.Text, chk文件比较.Checked);
            tabControl1.SelectedTab = tbpLastXml;
            btnDiff.Enabled = true;
            return;
            //建立新线程
            /*
            Thread thrdProduce = new Thread(new ThreadStart(WriterAUList));

            if (this.btnProduce.Text == "生成(&G)")
            {

                #region [检测基本条件]

                if (!File.Exists(this.txtSrc.Text))
                {
                    MessageBox.Show(this, "请选择主入口程序!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnSrc_Click(sender, e);
                }

                #region [请输入自动更新网址]

                if (this.txtUrl.Text.Trim().Length == 0)
                {
                    MessageBox.Show(this, "请输入自动更新网址!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtUrl.Focus();

                    return;
                }


                #endregion [请输入自动更新网址]

                if (this.txtDest.Text.Trim() == string.Empty)
                {
                    MessageBox.Show(this, "请选择AutoUpdaterList保存位置!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnSearDes_Click(sender, e);
                }

                #endregion [检测基本条件]

                #region [新线程写文件]

                thrdProduce.IsBackground = true;
                thrdProduce.Start();

                #endregion [新线程写文件]

                this.btnProduce.Text = "停止(&S)";
            }
            else
            {
                Application.DoEvents();
                if (MessageBox.Show(this, "是否停止文件生成更新文件?", "AutoUpdater", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //thrdProduce.Interrupt();
                    //thrdProduce.Abort();
                    if (thrdProduce.IsAlive)
                    {
                        thrdProduce.Abort();
                        thrdProduce.Join();
                    }
                    this.btnProduce.Text = "生成(&G)";
                }
            }
            */


        }

        #region [写AutoUpdaterList]




        #region 工具性静态方法
        /// <summary>
        /// 计算哈希值
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
        /// 源文件夹是最新的。目标是要更新的。 
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FindDifferentFileByHash(string sourceFolder, string targetFolder, string fileName)
        {
            // 构建两个文件夹中指定文件的完整路径
            string sourceFilePath = Path.Combine(sourceFolder, fileName);
            string targetFilePath = Path.Combine(targetFolder, fileName);

            //如果最新的有，目标没有则要返回文件名到时要复制过去。
            if (File.Exists(sourceFilePath) && !File.Exists(targetFilePath))
            {
                return fileName;
            }

            // 检查两个文件夹中是否存在指定的文件  这种情况就是最新的没有。则不需要要更新列表中。
            if (!File.Exists(sourceFilePath) || !File.Exists(targetFilePath))
            {
                // 如果任一文件夹中不存在该文件，则返回存在的文件路径
                return File.Exists(sourceFilePath) ? sourceFilePath : targetFilePath;
            }

            // 计算两个文件的哈希值
            string sourceHash = CalculateFileHash(sourceFilePath);
            string targetHash = CalculateFileHash(targetFilePath);

            // 比较哈希值
            if (sourceHash != targetHash)
            {
                // 如果哈希值不相同，则返回文件名
                return fileName;
            }

            // 如果哈希值相同，则返回null
            return null;
        }

        /// <summary>
        /// 到得相对路径
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetRelativePath(string fromPath, string toPath)
        {
            // 将两个路径转换为绝对路径
            fromPath = Path.GetFullPath(fromPath);
            toPath = Path.GetFullPath(toPath);

            // 将路径转换为规范化的形式，以确保比较时不会因路径大小写或斜杠方向不同而出错
            fromPath = fromPath.Replace("\\", "/").ToLower();
            toPath = toPath.Replace("\\", "/").ToLower();

            // 找到共同的根路径
            int length = Math.Min(fromPath.Length, toPath.Length);
            int index = 0;
            while (index < length && fromPath[index] == toPath[index])
            {
                index++;
            }
            // 计算需要回退到根目录的次数（即盘符不同的情况）
            int backSteps = 0;
            for (int i = index; i < fromPath.Length; i++)
            {
                if (fromPath[i] == '/')
                {
                    backSteps++;
                }
            }

            // 构建回退到根目录的路径
            string relativePath = "";
            for (int i = 0; i < backSteps; i++)
            {
                relativePath += "../";
            }

            // 从共同根路径之后的部分开始构建相对路径
            relativePath += toPath.Substring(index);

            // 替换斜杠方向并返回结果
            return relativePath.Replace("/", "\\");
        }





        #endregion

        /*
        /// <summary>
        /// 是否进行哈希值比较，如果是，则哈希值不一样才增加版本号。否则不变
        /// </summary>
        /// <param name="HashValueComparison"></param>
        void WriterAUList()
        {

            #region [写AutoUpdaterlist]

            string strEntryPoint = this.txtMainExePath.Text.Trim().Substring(this.txtMainExePath.Text.Trim().LastIndexOf(@"\") + 1, this.txtMainExePath.Text.Trim().Length - this.txtMainExePath.Text.Trim().LastIndexOf(@"\") - 1);
            string strFilePath = this.txtAutoUpdateXmlSavePath.Text.Trim();
            //Append  默认就换行了
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
            sb.Append("\t\t<Version>");

            FileVersionInfo _lcObjFVI = FileVersionInfo.GetVersionInfo(this.txtMainExePath.Text);
            if (chkUseBaseVersion.Checked)
            {
                if (this.txtBaseExeVersion.Text.TrimEnd().Length == 0)
                {
                    MessageBox.Show("选择指定版本，但是没有输入版本。");
                    return;
                }
                sb.Append(this.txtBaseExeVersion.Text);
            }
            else
            {
                sb.Append(string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart));
            }
            sb.Append("</Version>\r\n");
            sb.Append("\t</Application>\r\n");
            #endregion [application]

            #region [Files]
            sb.Append("\t<Files>\r\n");

            StringCollection strCollection = GetAllFiles(this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")));
            List<string> strColl = strCollection.Cast<string>().ToList();
            strColl.Sort();

            //如果指定了要更新的文件,优先这个。 意思是更新的列表，要在这个清单中
            if (txtPreVerUpdatedFiles.Text.Trim().Length > 0)
            {
                string[] files = txtPreVerUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                //排序
                Array.Sort(files);

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
                    if (!ExcludeUnnecessaryFiles(files[i].Trim()))
                    {
                        //暂时统一了。如果指定版本号
                        if (chk更新文件版本号.Checked)
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
                #region 默认
                this.prbProd.Visible = true;
                this.prbProd.Minimum = 0;
                this.prbProd.Maximum = strColl.Count;

                for (int i = 0; i < strColl.Count; i++)
                {
                    string rootDir = this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")) + @"\";
                    if (chkUseBaseVersion.Checked)
                    {
                        if (this.txtBaseExeVersion.Text.TrimEnd().Length == 0)
                        {
                            MessageBox.Show("选择指定版本，但是没有输入版本。");
                            return;
                        }

                        sb.Append("\t\t<File Ver=\""
                                + this.txtBaseExeVersion.Text
                                + "\" Name= \"" + strColl[i].Replace(rootDir, "")
                                + "\" />\r\n");
                    }
                    else
                    {
                        if (!ExcludeUnnecessaryFiles(strColl[i].Trim()))
                        {
                            FileVersionInfo m_lcObjFVI = FileVersionInfo.GetVersionInfo(strColl[i].ToString());

                            sb.Append("\t\t<File Ver=\""
                                + string.Format("{0}.{1}.{2}.{3}", _lcObjFVI.FileMajorPart, _lcObjFVI.FileMinorPart, _lcObjFVI.FileBuildPart, _lcObjFVI.FilePrivatePart)
                                + "\" Name= \"" + strColl[i].Replace(rootDir, "")
                                + "\" />\r\n");
                        }
                    }

                    prbProd.Value = i;
                }
                #endregion
            }

            sb.Append("\t</Files>\r\n");
            sb.Append("</AutoUpdater>");

            #endregion [Files]

            txtLastXml.Text = sb.ToString();


            this.prbProd.Value = 0;
            this.prbProd.Visible = false;

            #endregion [写AutoUpdaterlist]
        }
        */

        #endregion [写AutoUpdaterList]

        #region [遍历子目录]

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

        #endregion [遍历子目录]

        #region [排除不需要的文件]

        /// <summary>
        /// 排除不需要的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>为真则排除</returns>
        private bool ExcludeUnnecessaryFiles(string filePath)
        {
            bool isExist = false;
            string fileName = filePath.Replace(txtTargetDirectory.Text, "");
            fileName = fileName.TrimStart('\\');
            string[] files = txtExpt.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (files.Contains(fileName))
            {
                return true;
            }
            else
            {
                return false;
            }
            foreach (string strCheck in files)
            {
                if (filePath.Trim() == strCheck.Trim())
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }


        #endregion [排除不需要的文件]

        #endregion [生成文件]

        private void button_save_config_Click(object sender, EventArgs e)
        {
            //保存配置到文件
            try
            {
                saveConfigToFile();
                richTxtLog.AppendText("配置保存成功");
                //MessageBox.Show("配置保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置保存失败:" + ex.Message);
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
                dataConfig.UpdatedFiles = txtPreVerUpdatedFiles.Text;
                dataConfig.CompareSource = txtCompareSource.Text;
                dataConfig.BaseDir = txtTargetDirectory.Text;
                dataConfig.BaseExeVersion = txtBaseExeVersion.Text;
                dataConfig.UseBaseExeVersion = chkUseBaseVersion.Checked;
                string path = configFilePath;
                SerializeXmlHelper.SerializeXml(dataConfig, path);
            }
            catch (Exception ex)
            {
                richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":保存配置到文件失败:" + ex.Message);
                richTxtLog.AppendText("\r\n");
            }
        }

        private void txtExpt_DragDrop(object sender, DragEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            //在此处获取文件或者文件夹的路径
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
            e.Effect = DragDropEffects.Link; //修改鼠标拖放时的样式。
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
            if (e.Data.GetDataPresent(DataFormats.FileDrop))//文件拖放操作。
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);//获得拖放文件的路径。
                string filePath = filePaths[0];//取得第一个文件的路径。
                txtMainExePath.Text = filePath; //在TextBox中显示第一个文件路径。
            }
            // base.OnDragDrop(e);

        }

        private void txtSrc_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link; //修改鼠标拖放时的样式。
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
            //在此处获取文件或者文件夹的路径
            Array array = (Array)e.Data.GetData(DataFormats.FileDrop);

            //已经存在的文件 这里用这个来判断不要重复
            string[] files = txtPreVerUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //排序
            Array.Sort(array);
            List<string> addFiles = new List<string>();
            foreach (string path in array)
            {
                //这个新拖进来的文件 是全路径，要去前面与来源相同的目录去掉
                if (path.IndexOf(txtCompareSource.Text) == 0)
                {
                    addFiles.Add(path.Substring(txtCompareSource.Text.Length).TrimStart('\\'));
                }
            }

            //排序
            addFiles.Sort();
            foreach (string path in addFiles)
            {

                if (files.Contains(path))
                {
                    richTxtLog.AppendText($"{path}这个路径已存在将会忽略。\r\n");
                    //已经存在
                    continue;
                }
                if (File.Exists(System.IO.Path.Combine(txtCompareSource.Text, path)) || File.Exists(path))
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
                {
                    richTxtLog.AppendText($"{path}无效的路径。拖入文件失败！\r\n");
                    Console.WriteLine("无效的路径");
                }



            }
            if (chkAppend.Checked)
            {
                txtPreVerUpdatedFiles.Text += sb.ToString();
            }
            else
            {
                txtPreVerUpdatedFiles.Text = sb.ToString();
            }

        }

        private void txtUpdatedFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link; //修改鼠标拖放时的样式。
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

        #region 读取原来的xml配置文件

        //private string ReadoldXml()
        //{
        //    string content = string.Empty;
        //    string filePath = txtAutoUpdateXmlSavePath.Text;

        //    try
        //    {
        //        // 读取文件内容
        //        content = File.ReadAllText(filePath);

        //        // 输出文件内容
        //        //Console.WriteLine(content);
        //    }
        //    catch (Exception ex)
        //    {
        //        // 处理可能的异常，例如文件不存在或网络问题
        //        Console.WriteLine("Error reading file: " + ex.Message);
        //    }
        //    return content;
        //}

        List<string> DiffList = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetXmlFilePath"></param>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolder"></param>
        /// <param name="FileComparison"></param>
        public void UpdateXmlFile(string targetXmlFilePath, string sourceFolder, string targetFolder,
            bool FileComparison = true)
        {
            XDocument doc = XDocument.Load(targetXmlFilePath);
            txtDiff.Clear();

            if (chkUseBaseVersion.Checked)
            {
                // 查找 <Version> 元素
                var versionElement = doc.Descendants("Version").FirstOrDefault();
                if (versionElement != null)
                {
                    versionElement.Value = txtBaseExeVersion.Text; // 替换为您想要设置的新值
                }
            }

            string[] files = txtPreVerUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //排序
            Array.Sort(files);
            List<string> list = files.ToList();
            foreach (XElement fileElement in doc.Descendants("File"))
            {
                string fileName = fileElement.Attribute("Name").Value;
                if (list.Count > 0 && !list.Contains(fileName))
                {
                    // 如果文件不在新文件列表中，删除该文件
                   // fileElement.Remove();
                   //这个情况暂时是版本号不变。也就是不更新。
                }
                //如果文件在排除列表中也移除
                if(ExcludeUnnecessaryFiles(fileName))
                {
                    //fileElement.Remove();
                    //这个情况暂时是版本号不变。也就是不更新。  移除会导致xml文件结构破坏
                }

                if (FileComparison)
                {
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
                        // 如果文件大小或修改时间不同，直接更新版本号
                        string version = fileElement.Attribute("Ver").Value;
                        IncrementVersion(ref version);
                        fileElement.SetAttributeValue("Ver", version);
                        DiffList.Add(fileName);
                        txtDiff.AppendText(fileName + "\r\n");
                    }
                    else if (isSourceExist && isTargetExist)
                    {
                        // 如果启用哈希值比较
                        string sourceHash = CalculateFileHash(sourceFilePath);
                        string targetHash = CalculateFileHash(targetFilePath);

                        if (sourceHash != targetHash)
                        {
                            // 如果哈希值不同，则更新版本号
                            string version = fileElement.Attribute("Ver").Value;
                            IncrementVersion(ref version);
                            fileElement.SetAttributeValue("Ver", version);
                            DiffList.Add(fileName);
                            txtDiff.AppendText(fileName + "\r\n");
                        }
                    }
                    else if (isSourceExist && sourceSize < 1048576) // 文件小于1MB
                    {
                        // 如果文件小于1MB，进行逐字节比较
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
                else
                {
                    //不用比较了。直接升级一个版本。
                    string version = fileElement.Attribute("Ver").Value;
                    IncrementVersion(ref version);
                    fileElement.SetAttributeValue("Ver", version);

                }
                //这里不比较文件时，差异只在于新增加的项目 以【将要生成的清单】与旧的AutoUpdaterList 中的文件列表相比较
                //旧的存在的。不管什么方式。增加了就去掉目标清单。留下来的就是新增加的
                list.Remove(fileName);
            }

            foreach (var item in list)
            {
                //如果文件在排除列表中,忽略。不添加
                if (ExcludeUnnecessaryFiles(item))
                {
                    continue;
                }
                //添加新的文件到这个xml的File节点下面
                XElement newFileElement = new XElement("File");
                newFileElement.SetAttributeValue("Ver", "1.0.0.0");
                newFileElement.SetAttributeValue("Name", item);
                var FilesElement = doc.Descendants("Files").FirstOrDefault();
                FilesElement.Add(newFileElement);
                DiffList.Add(item);
                txtDiff.AppendText(item + "\r\n");
            }


            txtLastXml.Text = doc.ToString();
            richTxtLog.AppendText("生成最新的XML文件结果成功。请继续发布才是真正完成\r\n");

            //}
            //else
            //{
            //    doc.Save(xmlFilePath);
            //    richTxtLog.AppendText("保存-生成最新的XML文件成功。\r\n");
            //}
        }


        /// <summary>
        /// 逐字节比较
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

        /// <summary>
        /// 版本号加1
        /// </summary>
        /// <param name="version"></param>
        private void IncrementVersion(ref string version)
        {
            var parts = version.Split('.');
            parts[3] = (int.Parse(parts[3]) + 1).ToString();
            version = string.Join(".", parts);
        }

        #endregion


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
                txtTargetDirectory.Text = folderBrowserDialogCompareSource.SelectedPath;
            }
        }

        private void chk哈希值比较_CheckedChanged(object sender, EventArgs e)
        {
            //加载前一个AutoUpdaterList.xml中的文件列表
        }


        #region [复制发布]



        /// <summary>
        /// 复制文件  将版本号下面的文件 全部
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
        //将差异的文件 复制I盘临时目录，再到服务器。并且生成时版本号只对差异的更新  保存配置到文件中
        //先复制 再生成xml

        #endregion [复制发布]


        private void btnrelease_Click(object sender, EventArgs e)
        {
            foreach (var item in DiffList)
            {
                CopyFile(txtCompareSource.Text, txtTargetDirectory.Text, item);
            }

            richTxtLog.AppendText($"保存到目标-{txtTargetDirectory.Text}  成功{DiffList.Count}个。");
            richTxtLog.AppendText("\r\n");

            // 将StringBuilder的内容写入文件
            //File.WriteAllText(this.txtAutoUpdateXmlSavePath.Text.Trim(), txtLastXml.Text.ToString(), System.Text.Encoding.GetEncoding("gb2312"));
            XDocument newDoc = XDocument.Parse(txtLastXml.Text);
            var tempPath = Path.Combine(Path.GetTempPath(), this.txtAutoUpdateXmlSavePath.Text.Trim());

            // 创建带有gb2312编码的XML写入设置
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.GetEncoding("gb2312");
            settings.Indent = true; // 保持缩进格式

            // 使用XmlWriter保存文档
            using (XmlWriter writer = XmlWriter.Create(tempPath, settings))
            {
                newDoc.Save(writer);
            }

            MessageBox.Show(this, "自动更新文件生成成功:" + this.txtAutoUpdateXmlSavePath.Text.Trim(), "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {
                saveConfigToFile();
                //MessageBox.Show("配置保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置保存失败:" + ex.Message);
            }

            richTxtLog.AppendText($"发布到目标-{txtTargetDirectory}成功。");
            richTxtLog.AppendText("\r\n");
        }




        #region 新旧版本比较

        private void CompareUpdateXml(string OldConfigPath)
        {
            if (string.IsNullOrWhiteSpace(OldConfigPath))
            {
                MessageBox.Show("请先选择两个要比较的配置文件");
                return;
            }

            try
            {
                rtbOld.Clear();
                rtbNew.Clear();

                XDocument oldDoc = XDocument.Load(OldConfigPath);

                XDocument newDoc = XDocument.Parse(txtLastXml.Text);

                // 比较XML文档
                var comparer = new EnhancedXmlDiff();
                var diffBlocks = comparer.CompareXmlFiles(oldDoc, newDoc);
                // 显示差异
                var diffViewer = new XmlDiffViewer();
                // 显示差异
                //diffViewer.DisplayDifferences(diffBlocks);
                try
                {

                    var comparer1 = new XmlComparer();
                    var diffBlocks1 = comparer1.CompareXml(oldDoc, newDoc);

                    diffViewer.DisplayDifferences(diffBlocks1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"比较失败：{ex.Message}");
                }


                // 在窗体中显示
                var form = new Form();
                form.Text = "XML差异比较工具";
                form.Size = new Size(1200, 800);
                form.Controls.Add(diffViewer);
                diffViewer.Dock = DockStyle.Fill;
                form.Show();

                ShowXmlComparison(oldDoc, newDoc);


                CompareXmlDocuments(oldDoc, newDoc);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"比较配置文件时出错: {ex.Message}");
            }
        }

        public void ShowXmlComparison(XDocument oldDoc, XDocument newDoc)
        {
            var diffEngine = new EnhancedXmlDiff();
            List<DiffBlock> diffBlocks = diffEngine.CompareXmlFiles(oldDoc, newDoc);

            var viewer = new XmlDiffViewer();
            viewer.Dock = DockStyle.Fill;
            viewer.DisplayDifferences(diffBlocks);

            var form = new Form();
            form.Text = "XML Comparison - Like Beyond Compare";
            form.Size = new Size(1200, 800);
            form.Controls.Add(viewer);
            form.Show();
        }


        // 改进的内联差异检测算法
        private List<DiffSegment> ComputeEnhancedInlineDiff(string left, string right)
        {
            var segments = new List<DiffSegment>();

            if (left == right)
            {
                segments.Add(new DiffSegment { Text = left, IsModified = false });
                return segments;
            }

            // 使用更精细的差异算法（如基于词或标记的差异）
            var diff = new Differ();
            var changes = diff.DiffText(left, right);

            foreach (var change in changes)
            {
                segments.Add(new DiffSegment
                {
                    Text = change.LeftText,
                    RightText = change.RightText,
                    IsModified = change.Type != DiffType.Unchanged,
                    DiffType = change.Type,
                    StartPosition = change.LeftStart,
                    Length = change.LeftLength
                });
            }

            return segments;
        }
        #endregion

        private void btnDiff_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument oldDoc = XDocument.Load(txtAutoUpdateXmlSavePath.Text);
                XDocument newDoc = XDocument.Parse(txtLastXml.Text);

                var oldPath = SaveTempXml("old.xml", oldDoc);
                var newPath = SaveTempXml("new.xml", newDoc);

                Process.Start(@"D:\Program Files (x86)\Beyond Compare 3\BCompare.exe",
                    $"\"{oldPath}\" \"{newPath}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开Beyond Compare失败：{ex.Message}");
            }
            //return;
            //比较
            //CompareUpdateXml(txtAutoUpdateXmlSavePath.Text);
        }
        private string SaveTempXml(string fileName, XDocument doc)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), fileName);
            doc.Save(tempPath);
            return tempPath;
        }
        private void btnLoadCurrentVer_Click(object sender, EventArgs e)
        {
            LoadOldCurrentList(txtAutoUpdateXmlSavePath.Text);
        }


        private void LoadOldCurrentList(string xmlFilePath)
        {
            txtPreVerUpdatedFiles.Text = "";
            txtPreVerUpdatedFiles.Clear();

            if (!System.IO.File.Exists(xmlFilePath))
            {
                return;
            }
            XDocument doc = XDocument.Load(xmlFilePath);


            List<string> oldfiles = new List<string>();

            foreach (XElement fileElement in doc.Descendants("File"))
            {

                string fileName = fileElement.Attribute("Name").Value;
                oldfiles.Add(fileName);
            }

            oldfiles.Sort();

            foreach (string filename in oldfiles)
            {
                txtPreVerUpdatedFiles.AppendText(filename + "\r\n");
            }


        }


    }

}
