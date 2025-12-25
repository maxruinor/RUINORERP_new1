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
        // 常量定义
        private const int BufferSize = 1024 * 1024; // 1MB缓冲区大小
        private static readonly string[] TextSeparators = { "\r\n" };

        // 颜色配置
        private readonly Color SameColor = Color.FromArgb(240, 240, 240);
        private readonly Color DiffColor = Color.FromArgb(255, 220, 220);
        private readonly Color AddedColor = Color.FromArgb(220, 255, 220);
        private readonly Color RemovedColor = Color.FromArgb(255, 200, 200);
        private readonly Color HeaderColor = Color.FromArgb(230, 230, 255);
        // 后台工作组件
        private readonly BackgroundWorker bgWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        // 配置文件路径
        private readonly string configFilePath = Path.Combine(
            Application.StartupPath, "config", "config.xml");


        #region [基本入口构造函数]

        public frmAULWriter()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            SetupRichTextBoxes();
            SetupSafeScrollSync();
        }
        #region 初始化方法
        private void InitializeBackgroundWorker()
        {
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
        }

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

        #endregion
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



        public void CompareXmlDocuments(XDocument oldDoc, XDocument newDoc)
        {
            // 获取所有文件元素
            var oldFiles = oldDoc.Descendants("File").ToList();
            var newFiles = newDoc.Descendants("File").ToList();

            // 创建文件列表用于比较
            var oldFileEntries = oldFiles.Select(f => $"{f.Attribute("Name")?.Value}|{f.Attribute("Ver")?.Value}").ToArray();
            var newFileEntries = newFiles.Select(f => $"{f.Attribute("Name")?.Value}|{f.Attribute("Ver")?.Value}").ToArray();

            // 配置差异引擎
            var diff = new AdvancedDiff(oldFileEntries, newFileEntries);
            diff.SetLineMatchFunction((a, b) => a.Split('|')[0] == b.Split('|')[0]);

            // 自定义版本变更的内联差异
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

            // 计算差异
            var results = diff.ComputeDiff();

            // 在RichTextBox控件中显示
            DisplayDifferencesInRichTextBox(rtbOld, results, false);
            DisplayDifferencesInRichTextBox(rtbNew, results, true);
            
            // 在txtDiff中显示目录对比结果摘要
            DisplayDirectoryDiffSummary(oldDoc, newDoc);
        }
        
        /// <summary>
        /// 显示目录对比结果摘要
        /// </summary>
        private void DisplayDirectoryDiffSummary(XDocument oldDoc, XDocument newDoc)
        {
            // 获取所有文件列表
            var oldFiles = oldDoc.Descendants("File").Select(f => f.Attribute("Name").Value).ToHashSet();
            var newFiles = newDoc.Descendants("File").Select(f => f.Attribute("Name").Value).ToHashSet();
            
            // 计算差异
            var addedFiles = newFiles.Except(oldFiles).ToList();
            var deletedFiles = oldFiles.Except(newFiles).ToList();
            var commonFiles = oldFiles.Intersect(newFiles).ToList();
            
            // 计算修改的文件
            var modifiedFiles = new List<string>();
            foreach (var file in commonFiles)
            {
                var oldVer = oldDoc.Descendants("File")
                    .First(f => f.Attribute("Name").Value == file)
                    .Attribute("Ver").Value;
                var newVer = newDoc.Descendants("File")
                    .First(f => f.Attribute("Name").Value == file)
                    .Attribute("Ver").Value;
                
                if (oldVer != newVer)
                {
                    modifiedFiles.Add(file);
                }
            }
            
            // 清空现有内容
            txtDiff.Clear();
            
            // 显示差异摘要
            txtDiff.SelectionFont = new Font(txtDiff.Font, FontStyle.Bold);
            txtDiff.AppendText($"目录对比结果摘要\r\n");
            txtDiff.AppendText($"==================================\r\n\r\n");
            
            // 显示新增文件
            txtDiff.SelectionFont = new Font(txtDiff.Font, FontStyle.Bold);
            txtDiff.SelectionColor = Color.Green;
            txtDiff.AppendText($"新增文件 ({addedFiles.Count} 个):\r\n");
            txtDiff.SelectionFont = new Font(txtDiff.Font, FontStyle.Regular);
            foreach (var file in addedFiles)
            {
                txtDiff.AppendText($"  + {file}\r\n");
            }
            txtDiff.AppendText($"\r\n");
            
            // 显示修改文件
            txtDiff.SelectionFont = new Font(txtDiff.Font, FontStyle.Bold);
            txtDiff.SelectionColor = Color.Blue;
            txtDiff.AppendText($"修改文件 ({modifiedFiles.Count} 个):\r\n");
            txtDiff.SelectionFont = new Font(txtDiff.Font, FontStyle.Regular);
            foreach (var file in modifiedFiles)
            {
                var oldVer = oldDoc.Descendants("File")
                    .First(f => f.Attribute("Name").Value == file)
                    .Attribute("Ver").Value;
                var newVer = newDoc.Descendants("File")
                    .First(f => f.Attribute("Name").Value == file)
                    .Attribute("Ver").Value;
                txtDiff.AppendText($"  * {file} ({oldVer} → {newVer})\r\n");
            }
            txtDiff.AppendText($"\r\n");
            
            // 显示删除文件
            txtDiff.SelectionFont = new Font(txtDiff.Font, FontStyle.Bold);
            txtDiff.SelectionColor = Color.Red;
            txtDiff.AppendText($"删除文件 ({deletedFiles.Count} 个):\r\n");
            txtDiff.SelectionFont = new Font(txtDiff.Font, FontStyle.Regular);
            foreach (var file in deletedFiles)
            {
                txtDiff.AppendText($"  - {file}\r\n");
            }
            txtDiff.AppendText($"\r\n");
            
            // 恢复默认字体和颜色
            txtDiff.SelectionFont = new Font(txtDiff.Font, FontStyle.Regular);
            txtDiff.SelectionColor = txtDiff.ForeColor;
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
            this.txtTargetDirectory.TextChanged += new System.EventHandler(this.txtTargetDirectory_TextChanged);
        }



        /// <summary>
        /// 从配置文件读取配置
        /// </summary>
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
                    
                    // 加载排除后缀名配置
                    StringBuilder sbExcludeExt = new StringBuilder();
                    if (!string.IsNullOrEmpty(dataConfig.ExcludeExtensions))
                    {
                        string[] extFiles = dataConfig.ExcludeExtensions.Split(new string[] { "\n" }, StringSplitOptions.None);
                        foreach (var item in extFiles)
                        {
                            sbExcludeExt.Append(item).Append("\r\n");
                        }
                    }
                    txtExcludeExtensions.Text = sbExcludeExt.ToString();

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
        // 在窗体类中定义以下成员变量

        private UpdateXmlParameters workerParams;
        private void btnProduce_Click(object sender, EventArgs e)
        {
            // 禁用按钮防止重复点击
            btnGenerateNewlist.Enabled = false;
            prbProd.Value = 0;
            txtDiff.Clear();
            DiffFileList.Clear();
            // 准备参数
            workerParams = new UpdateXmlParameters
            {
                TargetXmlFilePath = txtAutoUpdateXmlSavePath.Text,
                SourceFolder = txtCompareSource.Text,
                TargetFolder = txtTargetDirectory.Text,
                FileComparison = chk文件比较.Checked,
                UseBaseVersion = chkUseBaseVersion.Checked,
                BaseExeVersion = txtBaseExeVersion.Text,
                PreVerUpdatedFiles = txtPreVerUpdatedFiles.Text.Split(new[] { "\r\n" },
                                  StringSplitOptions.RemoveEmptyEntries).ToList(),
                ExcludeUnnecessaryFiles = ExcludeUnnecessaryFiles,
                UpdateUrl = txtUrl.Text.Trim() // 将URL作为参数传递
            };

            // 开始后台工作
            bgWorker.RunWorkerAsync(workerParams);
            //UpdateXmlFile(txtAutoUpdateXmlSavePath.Text, txtCompareSource.Text, txtTargetDirectory.Text, chk文件比较.Checked);
            //tabControl1.SelectedTab = tbpLastXml;
            //btnDiff.Enabled = true;
            //return;
            LoadOldCurrentList(txtAutoUpdateXmlSavePath.Text);
        }

        #region 核心业务逻辑

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var parameters = (UpdateXmlParameters)e.Argument;
            var diffList = new List<string>();

            try
            {
                AppendLog("开始生成更新配置文件...");
                
                var doc = XDocument.Load(parameters.TargetXmlFilePath);
                
                // 更新Url节点值，使用参数中的UpdateUrl
                var urlElement = doc.Descendants("Updater").Elements("Url").FirstOrDefault();
                if (urlElement != null)
                {
                    urlElement.Value = parameters.UpdateUrl;
                    AppendLog($"更新URL地址为: {parameters.UpdateUrl}");
                }
                
                UpdateVersionInformation(doc, parameters);

                // 执行完整的目录对比
                AppendLog("开始执行目录对比...");
                DirectoryDiffResult diffResult = CompareDirectories(parameters.SourceFolder, doc, parameters.ExcludeUnnecessaryFiles);
                
                // 记录对比结果
                AppendLog($"目录对比完成：新增 {diffResult.NewFiles.Count} 个文件，修改 {diffResult.ModifiedFiles.Count} 个文件，删除 {diffResult.DeletedFiles.Count} 个文件");
                
                // 处理修改的文件
                var fileElements = doc.Descendants("File").ToList();
                worker.ReportProgress(0, new ProgressData { TotalFiles = fileElements.Count + diffResult.NewFiles.Count });
                
                if (diffResult.ModifiedFiles.Count > 0)
                {
                    AppendLog($"开始处理 {diffResult.ModifiedFiles.Count} 个修改文件...");
                    ProcessFiles(worker, parameters, fileElements, diffList, diffResult.ModifiedFiles);
                }
                
                // 处理新增文件（包括自动扫描的和手动指定的）
                List<string> allNewFiles = new List<string>();
                allNewFiles.AddRange(diffResult.NewFiles);
                allNewFiles.AddRange(parameters.PreVerUpdatedFiles);
                allNewFiles = allNewFiles.Distinct().ToList();
                
                if (allNewFiles.Count > 0)
                {
                    AppendLog($"开始处理 {allNewFiles.Count} 个新增文件...");
                    ProcessNewFiles(doc, allNewFiles, diffList, parameters);
                }
                
                // 处理删除的文件（从配置文件中移除）
                if (diffResult.DeletedFiles.Count > 0)
                {
                    AppendLog($"开始处理 {diffResult.DeletedFiles.Count} 个删除文件...");
                    ProcessDeletedFiles(doc, diffResult.DeletedFiles, diffList);
                }
                
                // 确保DiffList包含所有差异文件
                // 合并所有差异文件到diffList
                diffList.Clear();
                diffList.AddRange(diffResult.ModifiedFiles);
                diffList.AddRange(diffResult.NewFiles);
                diffList.AddRange(diffResult.DeletedFiles);
                
                // 计算没有变化的文件数量
                int totalFiles = diffResult.ModifiedFiles.Count + diffResult.NewFiles.Count + diffResult.DeletedFiles.Count + diffResult.UnchangedFiles.Count;
                
                AppendLog($"差异文件统计：共 {diffList.Count} 个差异文件");
                AppendLog($"其中：修改 {diffResult.ModifiedFiles.Count} 个，新增 {diffResult.NewFiles.Count} 个，删除 {diffResult.DeletedFiles.Count} 个，无变化 {diffResult.UnchangedFiles.Count} 个");
                AppendLog($"总文件数：{totalFiles} 个");

            AppendLog("更新配置文件生成完成");
            e.Result = new UpdateXmlResult { Document = doc, DiffList = diffList };
            }
            catch (Exception ex)
            {
                string errorMsg = $"生成更新配置文件失败: {ex.Message}";
                AppendLog(errorMsg);
                Debug.WriteLine(errorMsg);
                e.Result = ex;
            }
        }
        
        /// <summary>
        /// 处理文件更新（兼容原有方法）
        /// </summary>
        private void ProcessFiles(BackgroundWorker worker, UpdateXmlParameters parameters, 
            List<XElement> fileElements, List<string> diffList)
        {
            // 处理所有文件
            for (int i = 0; i < fileElements.Count; i++)
            {
                if (worker.CancellationPending) return;

                var fileElement = fileElements[i];
                var fileName = fileElement.Attribute("Name").Value;

                //排除指定文件
                if (ShouldExcludeFile(fileName, parameters)) continue;

                string diffFileName = string.Empty;
                if (IsFileModified(parameters, fileName))
                {
                    UpdateFileVersion(fileElement);
                    diffFileName = fileName;
                    diffList.Add(fileName);
                }

                ReportProgress(worker, i + 1, fileElements.Count, diffFileName);
            }
        }
        
        /// <summary>
        /// 处理修改的文件，仅处理指定列表中的文件
        /// </summary>
        private void ProcessFiles(BackgroundWorker worker, UpdateXmlParameters parameters, 
            List<XElement> fileElements, List<string> diffList, List<string> modifiedFiles)
        {
            // 仅处理修改的文件
            for (int i = 0; i < fileElements.Count; i++)
            {
                if (worker.CancellationPending) return;

                var fileElement = fileElements[i];
                var fileName = fileElement.Attribute("Name").Value;

                // 仅处理修改的文件
                if (!modifiedFiles.Contains(fileName)) continue;
                
                //排除指定文件
                if (ShouldExcludeFile(fileName, parameters)) continue;

                string diffFileName = string.Empty;
                if (IsFileModified(parameters, fileName))
                {
                    UpdateFileVersion(fileElement);
                    diffFileName = fileName;
                    diffList.Add(fileName);
                }

                ReportProgress(worker, i + 1, fileElements.Count, diffFileName);
            }
        }
        
        /// <summary>
        /// 处理删除的文件，从配置文件中移除
        /// </summary>
        private void ProcessDeletedFiles(XDocument doc, List<string> deletedFiles, List<string> diffList)
        {
            if (deletedFiles == null || deletedFiles.Count == 0)
            {
                AppendLog("没有需要删除的文件");
                return;
            }
            
            int deletedCount = 0;
            foreach (var fileName in deletedFiles)
            {
                // 从配置文件中移除删除的文件
                var fileElement = doc.Descendants("File")
                    .FirstOrDefault(f => f.Attribute("Name").Value.Equals(fileName, StringComparison.OrdinalIgnoreCase));
                
                if (fileElement != null)
                {
                    fileElement.Remove();
                    diffList.Add(fileName);
                    deletedCount++;
                    AppendLog($"从配置文件中移除删除的文件: {fileName}");
                }
            }
            
            AppendLog($"共处理 {deletedFiles.Count} 个需要删除的文件，成功移除 {deletedCount} 个");
        }
        
        /// <summary>
        /// 扫描目录中的所有文件，返回相对路径列表
        /// </summary>
        /// <param name="sourceDir">源目录路径</param>
        /// <param name="excludePredicate">排除文件的谓词函数</param>
        /// <returns>相对路径列表</returns>
        private List<string> ScanDirectoryForNewFiles(string sourceDir, Func<string, bool> excludePredicate)
        {
            List<string> files = new List<string>();
            int totalFiles = 0;
            int excludedFiles = 0;
            
            try
            {
                if (Directory.Exists(sourceDir))
                {
                    AppendLog($"开始扫描目录: {sourceDir}");
                    
                    // 递归扫描目录中的所有文件
                    // 使用*.*可能会排除某些没有扩展名的文件，改为使用*来包含所有文件
                    string[] allFiles = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
                    totalFiles = allFiles.Length;
                    AppendLog($"共扫描到 {totalFiles} 个文件");
                    
                    foreach (string filePath in allFiles)
                    {
                        // 计算相对路径
                        string relativePath = filePath.Substring(sourceDir.Length);
                        if (relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                        {
                            relativePath = relativePath.Substring(1);
                        }
                        
                        // 获取文件扩展名
                        string extension = Path.GetExtension(filePath).ToLower();
                        
                        // 检查是否需要排除（包括.zip文件）
                        if (excludePredicate(relativePath))
                        {
                            excludedFiles++;
                            AppendLog($"文件 {relativePath} 被排除：符合排除条件");
                            continue;
                        }
                        
                        // 额外检查：排除隐藏目录中的文件
                        if (IsInHiddenDirectory(sourceDir, relativePath))
                        {
                            excludedFiles++;
                            AppendLog($"文件 {relativePath} 被排除：位于隐藏目录中");
                            continue;
                        }
                        
                        // 额外检查：排除超大文件
                        if (IsOverSizeLimit(sourceDir, relativePath, 100))
                        {
                            excludedFiles++;
                            AppendLog($"文件 {relativePath} 被排除：超过100MB大小限制");
                            continue;
                        }
                        
                        // 文件通过所有检查，添加到列表中
                        files.Add(relativePath);
                    }
                    
                    AppendLog($"目录扫描完成：共 {totalFiles} 个文件，排除 {excludedFiles} 个，保留 {files.Count} 个");
                }
                else
                {
                    AppendLog($"错误：源目录 {sourceDir} 不存在");
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"扫描目录失败: {ex.Message}";
                AppendLog(errorMsg);
                Debug.WriteLine(errorMsg);
            }
            
            return files;
        }
        private void UpdateVersionInformation(XDocument doc, UpdateXmlParameters parameters)
        {
            if (!parameters.UseBaseVersion) return;

            var versionElement = doc.Descendants("Version").FirstOrDefault();
            versionElement?.SetValue(parameters.BaseExeVersion);
        }

        // 扩展的排除条件检查
        private bool ShouldExcludeFile(string fileName, UpdateXmlParameters parameters)
        {
            // 基础排除规则
            if (parameters.ExcludeUnnecessaryFiles?.Invoke(fileName) == true)
            {
                AppendLog($"文件 {fileName} 被排除：符合基础排除规则");
                return true;
            }

            // 从配置中读取排除后缀名
            var excludeExtensions = txtExcludeExtensions.Text
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ext => ext.Trim().ToLower())
                .Where(ext => !string.IsNullOrEmpty(ext))
                .ToArray();

            // 检查文件扩展名
            var extension = Path.GetExtension(fileName).ToLower();
            if (excludeExtensions.Contains(extension))
            {
                AppendLog($"文件 {fileName} 被排除：扩展名 {extension} 在排除列表中");
                return true;
            }

            // 检查文件名是否以下划线开头
            if (fileName.StartsWith("_"))
            {
                AppendLog($"文件 {fileName} 被排除：文件名以下划线开头");
                return true;
            }

            return false;
        }

        private bool IsInHiddenDirectory(string sourceDir, string relativePath)
        {
            try
            {
                // 构建完整路径
                string fullPath = Path.Combine(sourceDir, relativePath);
                string dir = Path.GetDirectoryName(fullPath);
                
                // 处理根目录文件的情况，GetDirectoryName会返回空字符串
                if (string.IsNullOrEmpty(dir))
                {
                    return false; // 根目录不是隐藏目录
                }
                
                return (new DirectoryInfo(dir).Attributes & FileAttributes.Hidden)
                       == FileAttributes.Hidden;
            }
            catch
            {
                return false;
            }
        }

        private bool IsOverSizeLimit(string sourceDir, string relativePath, int maxMB)
        {
            try
            {
                // 检查文件路径是否有效
                if (string.IsNullOrEmpty(relativePath))
                {
                    return false;
                }
                
                // 构建完整路径
                string fullPath = Path.Combine(sourceDir, relativePath);
                
                // 检查文件是否存在
                if (!File.Exists(fullPath))
                {
                    return false;
                }
                
                var fileInfo = new FileInfo(fullPath);
                return fileInfo.Length > maxMB * 1024L * 1024L;
            }
            catch
            {
                return false;
            }
        }

        private bool IsSystemFile(string sourceDir, string relativePath)
        {
            try
            {
                // 检查文件路径是否有效
                if (string.IsNullOrEmpty(relativePath))
                {
                    return false;
                }
                
                // 构建完整路径
                string fullPath = Path.Combine(sourceDir, relativePath);
                
                // 检查文件是否存在
                if (!File.Exists(fullPath))
                {
                    return false;
                }
                
                var attr = File.GetAttributes(fullPath);
                return (attr & FileAttributes.System) == FileAttributes.System;
            }
            catch
            {
                return false;
            }
        }

        private bool IsTemporaryFile(string fileName)
        {
            // 检查文件路径是否有效
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            
            return Path.GetExtension(fileName).Equals(".tmp", StringComparison.OrdinalIgnoreCase) ||
                   fileName.Contains("~$");
        }
        //private bool IsFileModified(UpdateXmlParameters parameters, string fileName)
        //{
        //    var sourcePath = Path.Combine(parameters.SourceFolder, fileName);
        //    var targetPath = Path.Combine(parameters.TargetFolder, fileName);

        //    if (!File.Exists(sourcePath) || !File.Exists(targetPath))
        //        return true;

        //    return CompareFiles(sourcePath, targetPath);
        //}

        private bool IsFileModified(UpdateXmlParameters parameters, string fileName)
        {
            var sourcePath = Path.Combine(parameters.SourceFolder, fileName);
            var targetPath = Path.Combine(parameters.TargetFolder, fileName);

            // 如果源文件不存在，认为文件未修改（返回false）
            if (!File.Exists(sourcePath))
                return false;

            // 如果目标文件不存在但源文件存在，认为文件已修改
            if (!File.Exists(targetPath))
                return true;

            // 两个文件都存在时，进行内容比较
            return CompareFiles(sourcePath, targetPath);
        }

        private bool CompareFiles(string sourcePath, string targetPath)
        {
            var sourceInfo = new FileInfo(sourcePath);
            var targetInfo = new FileInfo(targetPath);

            // 快速比较：文件大小和修改时间
            if (sourceInfo.Length != targetInfo.Length ||
                sourceInfo.LastWriteTimeUtc != targetInfo.LastWriteTimeUtc)
            {
                return true;
            }

            // 精确比较：哈希值校验
            return CalculateFileHash(sourcePath) != CalculateFileHash(targetPath);
        }


        //新添加的文件 从配置的基础版本号开始加到集合中
        private void ProcessNewFiles(XDocument doc,
                            List<string> newFiles,
                            List<string> diffList,
                            UpdateXmlParameters parameters)
        {
            try
            {
                AppendLog($"开始处理新文件，共 {newFiles.Count} 个待处理文件");
                
                // 获取现有文件名的哈希集合（不区分大小写）
                var existingFiles = doc.Descendants("File")
                    .Select(f => f.Attribute("Name").Value)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                // 使用LINQ过滤需要处理的新文件
                var validNewFiles = newFiles
                    .Where(f => !existingFiles.Contains(f) &&
                               !ShouldExcludeFile(f, parameters))
                    .ToList();
                
                AppendLog($"过滤后有效新文件数量: {validNewFiles.Count}");

                // 获取基础版本号
                // 注意：新增文件默认版本应该是1.0.0.0，exe入口文件的特殊设置仅适用于主程序
                string baseVersion = "1.0.0.0";
                AppendLog($"新增文件使用默认基础版本号: {baseVersion}");
                // 保持exe入口文件的特殊版本设置，但仅适用于主程序，不适用于新增文件
                // 新增文件统一使用1.0.0.0作为初始版本号

                int addedCount = 0;
                foreach (var newFile in validNewFiles)
                {
                    // 添加文件节点到XML，使用配置的基础版本号
                    var newElement = new XElement("File",
                        new XAttribute("Ver", baseVersion),
                        new XAttribute("Name", newFile));

                    doc.Descendants("Files").First().Add(newElement);
                    diffList.Add(newFile);
                    addedCount++;
                    AppendLog($"添加新文件到配置: {newFile}，版本: {baseVersion}");
                }
                
                AppendLog($"新文件处理完成，共成功添加 {addedCount} 个新文件");
            }
            catch (Exception ex)
            {
                // 记录异常日志
                string errorMsg = $"处理新文件时出错: {ex.Message}";
                AppendLog(errorMsg);
                Debug.WriteLine(errorMsg);
                throw;
            }
        }

        private void AddNewFileElement(XDocument doc, string fileName)
        {
            var newElement = new XElement("File",
                new XAttribute("Ver", "1.0.0.0"),
                new XAttribute("Name", fileName));

            doc.Descendants("Files").First().Add(newElement);
        }
        #endregion

        #region 辅助方法

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

            //using (var sha256 = SHA256.Create())
            //using (var stream = File.OpenRead(filePath))
            //{
            //    return BitConverter.ToString(sha256.ComputeHash(stream))
            //        .Replace("-", "").ToLowerInvariant();
            //}


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

        // 辅助方法保持原样
        private void UpdateFileVersion(XElement fileElement)
        {
            //var version = fileElement.Attribute("Ver").Value;
            //var parts = version.Split('.');
            //parts[3] = (int.Parse(parts[3]) + 1).ToString();
            //fileElement.SetAttributeValue("Ver", string.Join(".", parts));

            var version = fileElement.Attribute("Ver").Value;
            IncrementVersion(ref version);
            fileElement.SetAttributeValue("Ver", version);
        }



        private void ReportProgress(BackgroundWorker worker, int processed, int total, string fileName)
        {
            var progress = (int)((double)processed / total * 100);
            worker.ReportProgress(progress, new ProgressData
            {
                Processed = processed,
                DiffItems = new List<string> { fileName }
            });
        }
        #endregion

        #region UI事件处理
        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is ProgressData data)
            {
                UpdateProgressBar(data);
                UpdateDiffList(data);
            }


        }

        private void UpdateProgressBar(ProgressData data)
        {    // 更新进度条
            //if (data.TotalFiles > 0)
            //    prbProd.Maximum = data.TotalFiles;

            //prbProd.Value = Math.Min(data.Processed, prbProd.Maximum);
            // 初始化进度条
            if (data.TotalFiles > 0 && prbProd.Maximum != data.TotalFiles)
            {
                prbProd.Maximum = data.TotalFiles;
            }

            // 更新进度值
            if (data.Processed > 0)
            {
                prbProd.Value = Math.Min(data.Processed, prbProd.Maximum);
            }



        }

        private void UpdateLastUpdateTime(XDocument doc)
        {
            var lastUpdateElement = doc.Descendants("LastUpdateTime").FirstOrDefault();
            if (lastUpdateElement != null)
            {
                //lastUpdateElement.Value = DateTime.Now.ToString("yyyy-MM-dd");
                lastUpdateElement.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


        // 实时更新差异列表
        private void UpdateDiffList(ProgressData data)
        {
            if (data.DiffItems == null) return;

            foreach (var item in data.DiffItems)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    DiffFileList.Add(item);
                    txtDiff.AppendText($"{item}\r\n");
                }
            }
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnGenerateNewlist.Enabled = true;

            if (e.Error != null)
            {
                ShowErrorMessage($"操作失败: {e.Error.Message}");
            }
            else if (e.Cancelled)
            {
                ShowInformationMessage("操作已取消");
            }
            else
            {
                HandleSuccessfulCompletion(e.Result as UpdateXmlResult);
            }

            AppendLog("检测到差异文件个数为: " + DiffFileList.Count);
        }


        private void HandleSuccessfulCompletion(UpdateXmlResult result)
        {

            XDocument document = result?.Document;
            if (document == null)
            {
                MessageBox.Show("配置文件读取失败，请检查更新服务器是否能正常访问。");
                return;
            }

            // 更新DiffFileList，确保差异文件能在UI中显示
            DiffFileList.Clear();
            if (result.DiffList != null)
            {
                DiffFileList.AddRange(result.DiffList);
            }

            // 在保存文档前调用
            UpdateLastUpdateTime(document);

            txtLastXml.Text = document.ToString();
            //txtLastXml.Text = result?.Document.ToString();
            rtbNew.Text = txtLastXml.Text;
            
            // 更新差异文件列表显示
            txtDiff.Clear();
            foreach (var item in DiffFileList)
            {
                txtDiff.AppendText($"{item}\r\n");
            }
            
            tabControl1.SelectedTab = tbpLastXml;
            btnDiff.Enabled = true;
            AppendLog("XML文件生成成功！");
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AppendLog($"错误: {message}");
        }

        private void ShowInformationMessage(string message)
        {
            MessageBox.Show(message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppendLog(message);
        }

        private void AppendLog(string message)
        {
            // 检查是否需要跨线程调用
            if (richTxtLog.InvokeRequired)
            {
                // 使用Invoke在主线程中执行
                richTxtLog.Invoke(new Action<string>(AppendLog), message);
                return;
            }
            
            // 直接在主线程中执行
            richTxtLog.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}\r\n");
        }
        #endregion

        #region 文件操作
        private void SafeFileCopy(string source, string target)
        {
            try
            {
                var dir = Path.GetDirectoryName(target);
                Directory.CreateDirectory(dir);
                File.Copy(source, target, true);
            }
            catch (Exception ex)
            {
                AppendLog($"文件复制失败: {ex.Message}");
            }
        }
        #endregion








        /// <summary>
        /// 版本号递增策略枚举
        /// </summary>
        private enum VersionIncrementStrategy
        {
            /// <summary>递增修订号（默认）</summary>
            Revision = 0,
            /// <summary>递增构建号</summary>
            Build = 1,
            /// <summary>递增次版本号</summary>
            Minor = 2,
            /// <summary>递增主版本号</summary>
            Major = 3
        }

        /// <summary>
        /// 版本号加1，支持不同的递增策略
        /// </summary>
        /// <param name="version">版本号字符串</param>
        /// <param name="strategy">版本号递增策略，默认为递增修订号</param>
        private void IncrementVersion(ref string version, VersionIncrementStrategy strategy = VersionIncrementStrategy.Revision)
        {
            var parts = version.Split('.').Select(int.Parse).ToArray();
            
            // 根据策略递增相应的版本号部分
            switch (strategy)
            {
                case VersionIncrementStrategy.Major:
                    parts[0]++;
                    parts[1] = 0;
                    parts[2] = 0;
                    parts[3] = 0;
                    break;
                case VersionIncrementStrategy.Minor:
                    parts[1]++;
                    parts[2] = 0;
                    parts[3] = 0;
                    break;
                case VersionIncrementStrategy.Build:
                    parts[2]++;
                    parts[3] = 0;
                    break;
                case VersionIncrementStrategy.Revision:
                default:
                    parts[3]++;
                    break;
            }
            
            version = string.Join(".", parts);
        }
        
        /// <summary>
        /// 目录差异结果类，用于存储目录对比的结果
        /// </summary>
        private class DirectoryDiffResult
        {
            /// <summary>
            /// 新增文件列表（源目录有，配置文件没有）
            /// </summary>
            public List<string> NewFiles { get; set; } = new List<string>();
            
            /// <summary>
            /// 修改文件列表（源目录和配置文件都有，但内容不同）
            /// </summary>
            public List<string> ModifiedFiles { get; set; } = new List<string>();
            
            /// <summary>
            /// 删除文件列表（配置文件有，源目录没有）
            /// </summary>
            public List<string> DeletedFiles { get; set; } = new List<string>();
            
            /// <summary>
            /// 无变化文件列表（源目录和配置文件都有，且内容相同）
            /// </summary>
            public List<string> UnchangedFiles { get; set; } = new List<string>();
        }
        
        /// <summary>
        /// 比较源目录和配置文件，生成完整的差异报告
        /// </summary>
        /// <param name="sourceDir">源目录路径</param>
        /// <param name="configDoc">配置文件</param>
        /// <param name="excludePredicate">排除文件的谓词函数</param>
        /// <returns>目录差异结果</returns>
        private DirectoryDiffResult CompareDirectories(string sourceDir, XDocument configDoc, Func<string, bool> excludePredicate)
        {
            DirectoryDiffResult result = new DirectoryDiffResult();
            
            try
            {
                // 扫描源目录中的所有文件
                List<string> allFilesInSource = ScanDirectoryForNewFiles(sourceDir, excludePredicate);
                
                // 获取配置文件中的文件列表
                var configFiles = configDoc.Descendants("File")
                    .Select(f => f.Attribute("Name").Value)
                    .ToList();
                
                // 找出新增文件（源目录有，配置文件没有）
                result.NewFiles = allFilesInSource.Except(configFiles).ToList();
                
                // 找出删除文件（配置文件有，源目录没有）
                result.DeletedFiles = configFiles.Except(allFilesInSource).ToList();
                
                // 找出修改文件和无变化文件（源目录和配置文件都有）
                var commonFiles = allFilesInSource.Intersect(configFiles).ToList();
                foreach (var file in commonFiles)
                {
                    // 检查文件是否被修改
                    UpdateXmlParameters tempParams = new UpdateXmlParameters
                    {
                        SourceFolder = sourceDir,
                        TargetFolder = sourceDir, // 使用源目录作为目标目录进行比较
                        FileComparison = true,
                        ExcludeUnnecessaryFiles = excludePredicate
                    };
                    
                    if (IsFileModified(tempParams, file))
                    {
                        result.ModifiedFiles.Add(file);
                    }
                    else
                    {
                        result.UnchangedFiles.Add(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"比较目录失败: {ex.Message}");
            }
            
            return result;
        }

        #region [写AutoUpdaterList]






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
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
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
            try
            {
                // 获取排除文件列表
                string[] files = txtExpt.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                
                // 检查文件是否在排除列表中
                foreach (string strCheck in files)
                {
                    // 移除空白行
                    string excludeFile = strCheck.Trim();
                    if (string.IsNullOrEmpty(excludeFile))
                        continue;
                    
                    // 支持两种匹配方式：完全匹配和相对路径匹配
                    if (filePath.Trim().Equals(excludeFile, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    
                    // 检查文件名是否匹配（不考虑路径）
                    string fileName = Path.GetFileName(filePath);
                    if (fileName.Equals(excludeFile, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                
                // 检查文件扩展名是否在排除列表中
                var extension = Path.GetExtension(filePath).ToLower();
                if (!string.IsNullOrEmpty(extension))
                {
                    // 从配置中读取排除后缀名
                    var excludeExtensions = txtExcludeExtensions.Text
                        .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(ext => ext.Trim().ToLower())
                        .Where(ext => !string.IsNullOrEmpty(ext))
                        .ToArray();
                    
                    // 检查文件扩展名是否在排除列表中
                    if (excludeExtensions.Contains(extension))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"排除文件检查失败: {ex.Message}");
            }
            
            return false;
        }


        #endregion [排除不需要的文件]

        #endregion [生成文件]

        private void button_save_config_Click(object sender, EventArgs e)
        {
            //保存配置到文件
            try
            {
                saveConfigToFile();
                richTxtLog.AppendText("配置保存成功：" + configFilePath);
                //MessageBox.Show("配置保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置保存失败:" + ex.Message);
            }
        }



        /// <summary>
        /// 保存配置到文件
        /// </summary>
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
                dataConfig.ExcludeExtensions = txtExcludeExtensions.Text;
                string path = configFilePath;
                SerializeXmlHelper.SerializeXml(dataConfig, path);
            }
            catch (Exception ex)
            {
                richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":保存配置到文件失败:" + ex.Message);
                richTxtLog.AppendText("\r\n");
            }
        }

        /// <summary>
        /// 保存配置模板
        /// </summary>
        private void saveConfigTemplate()
        {
            try
            {
                // 显示保存文件对话框
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "配置模板文件 (*.template)|*.template|所有文件 (*.*)|*.*";
                    saveFileDialog.Title = "保存配置模板";
                    saveFileDialog.InitialDirectory = Path.GetDirectoryName(configFilePath);
                    saveFileDialog.FileName = "UpdateConfig.template";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 创建配置对象
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

                        // 序列化并保存
                        SerializeXmlHelper.SerializeXml(dataConfig, saveFileDialog.FileName);
                        richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":配置模板保存成功:" + saveFileDialog.FileName);
                        richTxtLog.AppendText("\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":保存配置模板失败:" + ex.Message);
                richTxtLog.AppendText("\r\n");
            }
        }

        /// <summary>
        /// 加载配置模板
        /// </summary>
        private void loadConfigTemplate()
        {
            try
            {
                // 显示打开文件对话框
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "配置模板文件 (*.template)|*.template|所有文件 (*.*)|*.*";
                    openFileDialog.Title = "加载配置模板";
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(configFilePath);

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 反序列化配置
                        DataConfig dataConfig = SerializeXmlHelper.DeserializeXml<DataConfig>(openFileDialog.FileName);
                        if (dataConfig != null)
                        {
                            // 加载配置到UI
                            txtUrl.Text = dataConfig.UpdateHttpAddress;
                            txtAutoUpdateXmlSavePath.Text = dataConfig.SavePath;
                            txtMainExePath.Text = dataConfig.EntryPoint;
                            txtExpt.Text = dataConfig.ExcludeFiles;
                            txtPreVerUpdatedFiles.Text = dataConfig.UpdatedFiles;
                            txtCompareSource.Text = dataConfig.CompareSource;
                            txtTargetDirectory.Text = dataConfig.BaseDir;
                            txtBaseExeVersion.Text = dataConfig.BaseExeVersion;
                            chkUseBaseVersion.Checked = dataConfig.UseBaseExeVersion;

                            richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":配置模板加载成功:" + openFileDialog.FileName);
                            richTxtLog.AppendText("\r\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":加载配置模板失败:" + ex.Message);
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
                    System.Diagnostics.Debug.WriteLine("无效的路径");
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
        //        //System.Diagnostics.Debug.WriteLine(content);
        //    }
        //    catch (Exception ex)
        //    {
        //        // 处理可能的异常，例如文件不存在或网络问题
        //        System.Diagnostics.Debug.WriteLine("Error reading file: " + ex.Message);
        //    }
        //    return content;
        //}

        List<string> DiffFileList = new List<string>();

        #region 差异文件管理

        /// <summary>
        /// 差异文件数据模型
        /// </summary>
        private class DiffFileItem
        {
            /// <summary>
            /// 是否选中
            /// </summary>
            public bool IsSelected { get; set; }
            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 文件路径
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// 文件大小（KB）
            /// </summary>
            public string FileSize { get; set; }
            /// <summary>
            /// 最后修改时间
            /// </summary>
            public string LastModified { get; set; }
            /// <summary>
            /// 文件状态
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 完整文件路径
            /// </summary>
            public string FullFilePath { get; set; }
        }

        /// <summary>
        /// 差异文件列表
        /// </summary>
        private List<DiffFileItem> diffFileItems = new List<DiffFileItem>();
        /// <summary>
        /// 过滤后的差异文件列表
        /// </summary>
        private List<DiffFileItem> filteredDiffFileItems = new List<DiffFileItem>();
        

        

        /// <summary>
        /// 当前搜索关键词
        /// </summary>
        private string currentSearchKeyword = string.Empty;

        /// <summary>
        /// 初始化差异文件列表
        /// </summary>
        private void InitializeDiffFileList()
        {
            diffFileItems.Clear();
            
            // 从DiffFileList中加载差异文件
            foreach (var filePath in DiffFileList)
            {
                string fullPath = Path.Combine(txtCompareSource.Text, filePath);
                FileInfo fileInfo = null;
                string status = "新增";
                
                try
                {
                    if (File.Exists(fullPath))
                    {
                        fileInfo = new FileInfo(fullPath);
                    }
                    
                    // 检查文件是新增还是修改
                    // 检查文件是否在目标目录中存在
                    string targetPath = Path.Combine(txtTargetDirectory.Text, filePath);
                    if (File.Exists(targetPath))
                    {
                        status = "修改";
                    }
                    
                    diffFileItems.Add(new DiffFileItem
                    {
                        IsSelected = true, // 默认选中
                        FileName = Path.GetFileName(filePath),
                        FilePath = filePath,
                        FileSize = fileInfo != null ? (fileInfo.Length / 1024.0).ToString("0.00") + " KB" : "N/A",
                        LastModified = fileInfo != null ? fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") : "N/A",
                        Status = status,
                        FullFilePath = fullPath
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"读取文件信息失败: {filePath} - {ex.Message}");
                    diffFileItems.Add(new DiffFileItem
                    {
                        IsSelected = true,
                        FileName = Path.GetFileName(filePath),
                        FilePath = filePath,
                        FileSize = "N/A",
                        LastModified = "N/A",
                        Status = status,
                        FullFilePath = fullPath
                    });
                }
            }
            
            // 保存初始选择状态
            SaveDiffFileSelection();
            
            // 应用过滤和分页
            ApplyFilterAndPagination();
        }

        /// <summary>
        /// 应用过滤（移除了分页功能）
        /// </summary>
        private void ApplyFilterAndPagination()
        {
            // 应用过滤
            if (!string.IsNullOrEmpty(currentSearchKeyword))
            {
                filteredDiffFileItems = diffFileItems.Where(item => 
                    item.FileName.IndexOf(currentSearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    item.FilePath.IndexOf(currentSearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            else
            {
                filteredDiffFileItems = diffFileItems.ToList();
            }
            
            // 绑定数据到DataGridView
            BindDiffFileData();
        }

        /// <summary>
        /// 绑定差异文件数据到DataGridView
        /// </summary>
        private void BindDiffFileData()
        {
            // 直接绑定所有过滤后的数据，不再分页
            // 绑定数据 - 即使没有数据，也要绑定一个空列表，确保列标题显示
            dgvDiffFiles.DataSource = new BindingList<DiffFileItem>(filteredDiffFileItems);
        }

 

        /// <summary>
        /// 保存差异文件选择状态
        /// </summary>
        private void SaveDiffFileSelection()
        {
            // 这里可以实现选择状态的本地存储，例如使用临时文件或内存缓存
            // 目前使用内存缓存
        }

        /// <summary>
        /// 加载差异文件选择状态
        /// </summary>
        private void LoadDiffFileSelection()
        {
            // 这里可以实现选择状态的加载，例如从临时文件或内存缓存
            // 目前使用内存缓存
        }

        /// <summary>
        /// 全选差异文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (var item in diffFileItems)
            {
                item.IsSelected = true;
            }
            
            SaveDiffFileSelection();
            ApplyFilterAndPagination();
        }

        /// <summary>
        /// 取消全选差异文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (var item in diffFileItems)
            {
                item.IsSelected = false;
            }
            
            SaveDiffFileSelection();
            ApplyFilterAndPagination();
        }

        /// <summary>
        /// 应用选择的差异文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApplySelection_Click(object sender, EventArgs e)
        {
            // 获取选中的差异文件
            var selectedFiles = diffFileItems.Where(item => item.IsSelected).Select(item => item.FilePath).ToList();
            
            if (selectedFiles.Count == 0)
            {
                MessageBox.Show("请至少选择一个差异文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            // 确认操作
            var result = MessageBox.Show($"确定要将选中的 {selectedFiles.Count} 个差异文件添加到配置文件中吗？", 
                "确认操作", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    // 更新DiffFileList为选中的文件
                    DiffFileList.Clear();
                    DiffFileList.AddRange(selectedFiles);
                    
                    // 重新生成配置文件
                    // 这里可以调用现有的生成逻辑
                    
                    MessageBox.Show("差异文件选择已应用", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"应用选择失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 搜索差异文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            currentSearchKeyword = txtSearch.Text.Trim();
            ApplyFilterAndPagination();
        }
 

        /// <summary>
        /// 差异文件选择状态变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDiffFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colSelect.Index && e.RowIndex >= 0)
            {
                // 切换选择状态
                DataGridViewRow row = dgvDiffFiles.Rows[e.RowIndex];
                DiffFileItem item = (DiffFileItem)row.DataBoundItem;
                item.IsSelected = !item.IsSelected;
                
                // 更新UI
                dgvDiffFiles.Refresh();
                
                // 保存选择状态
                SaveDiffFileSelection();
            }
        }

        /// <summary>
        /// Tab页切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当切换到差异文件列表Tab页时，确保显示最新的差异文件
            if (tabControl1.SelectedTab == tbpDiffList)
            {
                // 更新差异文件列表显示
                txtDiff.Clear();
                foreach (var item in DiffFileList)
                {
                    txtDiff.AppendText($"{item}\r\n");
                }
            }
            // 当切换到差异文件管理Tab页时，初始化差异文件列表
            else if (tabControl1.SelectedTab == tabPageDiffFile)
            {
                InitializeDiffFileList();
            }
        }

        #endregion

        /// <summary>
        /// 这个方法被AI重写了。这里是正确的。AI的要检查验证
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
            
            // 更新Url节点值，使其与UI上的txtUrl同步
            var urlElement = doc.Descendants("Updater").Elements("Url").FirstOrDefault();
            if (urlElement != null)
            {
                urlElement.Value = txtUrl.Text.Trim();
            }

            if (chkUseBaseVersion.Checked)
            {
                // 查找 <Version> 元素
                var versionElement = doc.Descendants("Version").FirstOrDefault();
                if (versionElement != null)
                {
                    versionElement.Value = txtBaseExeVersion.Text; // 替换为您想要设置的新值
                }
            }
            //来自NAS的发布目标中现有的文件中的清单文件数量
            string[] files = txtPreVerUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //排序
            Array.Sort(files);
            List<string> list = files.ToList();
            var XElementFiles = doc.Descendants("File");
            foreach (XElement fileElement in XElementFiles)
            {
                string fileName = fileElement.Attribute("Name").Value;
                if (list.Count > 0 && !list.Contains(fileName))
                {
                    // 如果文件不在新文件列表中，删除该文件
                    // fileElement.Remove();
                    //这个情况暂时是版本号不变。也就是不更新。
                }
                //如果文件在排除列表中也移除
                if (ExcludeUnnecessaryFiles(fileName))
                {
                    //fileElement.Remove();
                    continue;
                    //这个情况暂时是版本号不变。也就是不更新。  移除会导致xml文件结构破坏
                    //如果是第一次则是不参与生成

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
                        DiffFileList.Add(fileName);
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
                            DiffFileList.Add(fileName);
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
                            DiffFileList.Add(fileName);
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
                DiffFileList.Add(item);
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
        public bool CopyFile(string sourceDir, string targetDir, string tartgetfileName)
        {
            bool copySuccess = false;

            string sourcePath = System.IO.Path.Combine(sourceDir, tartgetfileName);
            string targetPath = System.IO.Path.Combine(targetDir, tartgetfileName);
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
            try
            {
                //存在才复制过去
                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, targetPath, true);
                    copySuccess = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return copySuccess;
        }
        
        /// <summary>
        /// 复制配置文件中的所有文件到目标目录
        /// </summary>
        /// <param name="configDoc">配置文件文档</param>
        /// <param name="sourceDir">源目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <returns>成功复制的文件数量</returns>
        private int CopyAllConfigFiles(XDocument configDoc, string sourceDir, string targetDir)
        {
            int copySuccessCount = 0;
            
            try
            {
                // 获取配置文件中的所有文件清单
                var allFilesInConfig = configDoc.Descendants("File")
                    .Select(f => f.Attribute("Name").Value)
                    .ToList();
                
                if (allFilesInConfig.Count == 0)
                {
                    AppendLog("配置文件中没有文件清单");
                    return 0;
                }
                
                AppendLog($"开始复制配置文件中的所有文件，共 {allFilesInConfig.Count} 个文件");
                
                // 创建目标目录（如果不存在）
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                    AppendLog($"创建目标目录：{targetDir}");
                }
                
                // 逐个复制文件
                foreach (var fileName in allFilesInConfig)
                {
                    string sourcePath = Path.Combine(sourceDir, fileName);
                    string targetPath = Path.Combine(targetDir, fileName);
                    
                    // 确保目标文件所在的目录存在
                    string targetFileDir = Path.GetDirectoryName(targetPath);
                    if (!Directory.Exists(targetFileDir))
                    {
                        Directory.CreateDirectory(targetFileDir);
                    }
                    
                    // 复制文件
                    try
                    {
                        if (File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, targetPath, true);
                            copySuccessCount++;
                            AppendLog($"成功复制文件：{fileName}");
                        }
                        else
                        {
                            AppendLog($"警告：源文件不存在，跳过复制：{sourcePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"复制文件失败：{fileName}，错误：{ex.Message}");
                    }
                }
                
                AppendLog($"文件复制完成，成功复制 {copySuccessCount} 个文件，失败 {allFilesInConfig.Count - copySuccessCount} 个文件");
            }
            catch (Exception ex)
            {
                AppendLog($"复制配置文件中的文件失败，错误：{ex.Message}");
            }
            
            return copySuccessCount;
        }
        
        //将差异的文件 复制I盘临时目录，再到服务器。并且生成时版本号只对差异的更新  保存配置到文件中
        //先复制 再生成xml

        #endregion [复制发布]


        private void btnrelease_Click(object sender, EventArgs e)
        {
            if (DiffFileList.Count == 0)
            {
                MessageBox.Show("没有需要发布的文件。请先生成差异文件。");
                return;
            }

            int CopySuccessed = 0;
            XDocument newDoc = null;
            if (!chkTest.Checked)
            {
                // 1. 解析最新的配置文件
                newDoc = XDocument.Parse(txtLastXml.Text);
                
                // 2. 复制所有配置文件中的文件到目标目录
                AppendLog("开始发布流程...");
                AppendLog($"从 {txtCompareSource.Text} 复制到 {txtTargetDirectory.Text}");
                
                // 3. 使用新方法复制所有配置文件中的文件
                CopySuccessed = CopyAllConfigFiles(newDoc, txtCompareSource.Text, txtTargetDirectory.Text);
            }
            
            if (chkTest.Checked)
            {
                richTxtLog.AppendText($"测试模式----保存到目标-{txtTargetDirectory.Text}  成功{CopySuccessed}个。");
                richTxtLog.AppendText("\r\n");
                return;

            }
            else
            {
                richTxtLog.AppendText($"保存到目标-{txtTargetDirectory.Text}  成功{CopySuccessed}个。");
                richTxtLog.AppendText("\r\n");
            }

            // 将StringBuilder的内容写入文件
            var tempPath = Path.Combine(Path.GetTempPath(), this.txtAutoUpdateXmlSavePath.Text.Trim());

            // 创建带有UTF-8编码的XML写入设置
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.Indent = true; // 保持缩进格式

            // 确保newDoc不为null
            if (newDoc == null)
            {
                newDoc = XDocument.Parse(txtLastXml.Text);
            }

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

            richTxtLog.AppendText($"发布到目标-{txtTargetDirectory.Text}成功。");
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
                //XDocument oldDoc = XDocument.Load(txtAutoUpdateXmlSavePath.Text);
                //XDocument newDoc = XDocument.Parse(txtLastXml.Text);

                XDocument oldDoc = XDocument.Parse(rtbOld.Text);
                XDocument newDoc = XDocument.Parse(rtbNew.Text);

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
            readConfigFromFile();
        }


        /// <summary>
        /// 加载旧的更新列表
        /// </summary>
        /// <param name="xmlFilePath"></param>
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

            rtbOld.Text = doc.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bgWorker.CancelAsync();
        }

        private void txtTargetDirectory_TextChanged(object sender, EventArgs e)
        {
            string mainProgramFileName = "企业数字化集成ERP.exe";
            string listFileName = "AutoUpdaterList.xml";

            FileInfo exeFileInfo = new FileInfo(txtMainExePath.Text);
            if (exeFileInfo != null && exeFileInfo.DirectoryName != null)
            {
                mainProgramFileName = exeFileInfo.Name;
            }
            FileInfo listFileInfo = new FileInfo(txtAutoUpdateXmlSavePath.Text);
            if (listFileInfo != null && listFileInfo.DirectoryName != null)
            {
                listFileName = listFileInfo.Name;
            }

            txtMainExePath.Text = System.IO.Path.Combine(txtTargetDirectory.Text, mainProgramFileName);
            txtAutoUpdateXmlSavePath.Text = System.IO.Path.Combine(txtTargetDirectory.Text, listFileName);

        }
    }


    #region 辅助类
    public class UpdateXmlResult
    {
        public XDocument Document { get; set; }
        public List<string> DiffList { get; set; }
    }


    public class UpdateXmlParameters
    {
        public string TargetXmlFilePath { get; set; }
        public string SourceFolder { get; set; }
        public string TargetFolder { get; set; }
        public bool FileComparison { get; set; }
        public bool UseBaseVersion { get; set; }
        public string BaseExeVersion { get; set; }
        public List<string> PreVerUpdatedFiles { get; set; }
        public Func<string, bool> ExcludeUnnecessaryFiles { get; set; }
        public string UpdateUrl { get; set; }
    }

    public class ProgressData
    {
        public int TotalFiles { get; set; }
        public int Processed { get; set; }
        public List<string> DiffItems { get; set; }
    }
    #endregion


}
