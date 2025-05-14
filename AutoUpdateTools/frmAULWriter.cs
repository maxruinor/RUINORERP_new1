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
        // ��������
        private const int BufferSize = 1024 * 1024; // 1MB��������С
        private static readonly string[] TextSeparators = { "\r\n" };

        // ��ɫ����
        private readonly Color SameColor = Color.FromArgb(240, 240, 240);
        private readonly Color DiffColor = Color.FromArgb(255, 220, 220);
        private readonly Color AddedColor = Color.FromArgb(220, 255, 220);
        private readonly Color RemovedColor = Color.FromArgb(255, 200, 200);
        private readonly Color HeaderColor = Color.FromArgb(230, 230, 255);
        // ��̨�������
        private readonly BackgroundWorker bgWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        // �����ļ�·��
        private readonly string configFilePath = Path.Combine(
            Application.StartupPath, "config", "config.xml");


        #region [������ڹ��캯��]

        public frmAULWriter()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            SetupRichTextBoxes();
            SetupSafeScrollSync();
        }
        #region ��ʼ������
        private void InitializeBackgroundWorker()
        {
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
        }

        private void SetupRichTextBoxes()
        {
            // ������������RichTextBox����ʽ
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
            // �ؼ���ʹ�õ���ͬ�������������ӹ�ϵ
            _syncOldToNew = new SafeScrollSynchronizer(rtbOld, rtbNew);
            _syncNewToOld = new SafeScrollSynchronizer(rtbNew, rtbOld);
        }



        #endregion [������ڹ��캯��]

        #region



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

        #region �Ƚ�2025-04-06
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
            btnDiff.Enabled = false;
            this.txtTargetDirectory.TextChanged += new System.EventHandler(this.txtTargetDirectory_TextChanged);
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
                richTxtLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":�������ļ���ȡ����ʧ��:" + ex.Message);
                richTxtLog.AppendText("\r\n");
            }

        }
        #endregion [���������]

        #region [�����ļ�]
        // �ڴ������ж������³�Ա����

        private UpdateXmlParameters workerParams;
        private void btnProduce_Click(object sender, EventArgs e)
        {
            // ���ð�ť��ֹ�ظ����
            btnGenerateNewlist.Enabled = false;
            prbProd.Value = 0;
            txtDiff.Clear();

            // ׼������
            workerParams = new UpdateXmlParameters
            {
                TargetXmlFilePath = txtAutoUpdateXmlSavePath.Text,
                SourceFolder = txtCompareSource.Text,
                TargetFolder = txtTargetDirectory.Text,
                FileComparison = chk�ļ��Ƚ�.Checked,
                UseBaseVersion = chkUseBaseVersion.Checked,
                BaseExeVersion = txtBaseExeVersion.Text,
                PreVerUpdatedFiles = txtPreVerUpdatedFiles.Text.Split(new[] { "\r\n" },
                                  StringSplitOptions.RemoveEmptyEntries).ToList(),
                ExcludeUnnecessaryFiles = ExcludeUnnecessaryFiles
            };

            // ��ʼ��̨����
            bgWorker.RunWorkerAsync(workerParams);
            //UpdateXmlFile(txtAutoUpdateXmlSavePath.Text, txtCompareSource.Text, txtTargetDirectory.Text, chk�ļ��Ƚ�.Checked);
            //tabControl1.SelectedTab = tbpLastXml;
            //btnDiff.Enabled = true;
            //return;

        }

        #region ����ҵ���߼�

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var parameters = (UpdateXmlParameters)e.Argument;
            var diffList = new List<string>();

            try
            {
                var doc = XDocument.Load(parameters.TargetXmlFilePath);
                UpdateVersionInformation(doc, parameters);

                var fileElements = doc.Descendants("File").ToList();
                worker.ReportProgress(0, new ProgressData { TotalFiles = fileElements.Count });

                ProcessFiles(worker, parameters, fileElements, diffList);
                ProcessNewFiles(doc, parameters.PreVerUpdatedFiles, diffList, parameters);

                e.Result = new UpdateXmlResult { Document = doc, DiffList = diffList };
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }
        private void UpdateVersionInformation(XDocument doc, UpdateXmlParameters parameters)
        {
            if (!parameters.UseBaseVersion) return;

            var versionElement = doc.Descendants("Version").FirstOrDefault();
            versionElement?.SetValue(parameters.BaseExeVersion);
        }

        private void ProcessFiles(BackgroundWorker worker, UpdateXmlParameters parameters,
            List<XElement> fileElements, List<string> diffList)
        {
            for (int i = 0; i < fileElements.Count; i++)
            {
                if (worker.CancellationPending) return;

                var fileElement = fileElements[i];
                var fileName = fileElement.Attribute("Name").Value;

                //�ų�ָ���ļ�
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


        // ��չ���ų��������
        private bool ShouldExcludeFile(string fileName, UpdateXmlParameters parameters)
        {
            // �����ų�����
            if (parameters.ExcludeUnnecessaryFiles?.Invoke(fileName) == true)
                return true;

            // ��չ����
            var extension = Path.GetExtension(fileName).ToLower();
            var excludeExtensions = new[] { ".log", ".tmp", ".bak" };

            return
                excludeExtensions.Contains(extension) ||    // �ų��ض���չ��
                fileName.StartsWith("_") ||                 // �ų��»��߿�ͷ���ļ�
                IsInHiddenDirectory(fileName) ||            // �ų�����Ŀ¼�е��ļ�
                IsOverSizeLimit(fileName, 100);             // �ų�����100MB�Ĵ��ļ�
        }

        private bool IsInHiddenDirectory(string filePath)
        {
            try
            {
                var dir = Path.GetDirectoryName(filePath);
                return (new DirectoryInfo(dir).Attributes & FileAttributes.Hidden)
                       == FileAttributes.Hidden;
            }
            catch
            {
                return false;
            }
        }

        private bool IsOverSizeLimit(string filePath, int maxMB)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                return fileInfo.Length > maxMB * 1024L * 1024L;
            }
            catch
            {
                return false;
            }
        }

        private bool IsSystemFile(string fileName)
        {
            try
            {
                var attr = File.GetAttributes(fileName);
                return (attr & FileAttributes.System) == FileAttributes.System;
            }
            catch
            {
                return false;
            }
        }

        private bool IsTemporaryFile(string fileName)
        {
            return Path.GetExtension(fileName).Equals(".tmp", StringComparison.OrdinalIgnoreCase) ||
                   fileName.Contains("~$");
        }
        private bool IsFileModified(UpdateXmlParameters parameters, string fileName)
        {
            var sourcePath = Path.Combine(parameters.SourceFolder, fileName);
            var targetPath = Path.Combine(parameters.TargetFolder, fileName);

            if (!File.Exists(sourcePath) || !File.Exists(targetPath))
                return true;

            return CompareFiles(sourcePath, targetPath);
        }

        private bool CompareFiles(string sourcePath, string targetPath)
        {
            var sourceInfo = new FileInfo(sourcePath);
            var targetInfo = new FileInfo(targetPath);

            // ���ٱȽϣ��ļ���С���޸�ʱ��
            if (sourceInfo.Length != targetInfo.Length ||
                sourceInfo.LastWriteTimeUtc != targetInfo.LastWriteTimeUtc)
            {
                return true;
            }

            // ��ȷ�Ƚϣ���ϣֵУ��
            return CalculateFileHash(sourcePath) != CalculateFileHash(targetPath);
        }


        //����ӵ��ļ� ��1.0.0.0��ʼ�ӵ�������
        private void ProcessNewFiles(XDocument doc,
                            List<string> newFiles,
                            List<string> diffList,
                            UpdateXmlParameters parameters)
        {
            try
            {
                // ��ȡ�����ļ����Ĺ�ϣ���ϣ������ִ�Сд��
                var existingFiles = doc.Descendants("File")
                    .Select(f => f.Attribute("Name").Value)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                // ʹ��LINQ������Ҫ��������ļ�
                var validNewFiles = newFiles
                    .Where(f => !existingFiles.Contains(f) &&
                               !ShouldExcludeFile(f, parameters))
                    .ToList();

                foreach (var newFile in validNewFiles)
                {
                    // ����ļ��ڵ㵽XML
                    var newElement = new XElement("File",
                        new XAttribute("Ver", "1.0.0.0"),
                        new XAttribute("Name", newFile));

                    doc.Descendants("Files").First().Add(newElement);
                    diffList.Add(newFile);
                }
            }
            catch (Exception ex)
            {
                // ��¼�쳣��־
                Debug.WriteLine($"�������ļ�ʱ����: {ex.Message}");
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

        #region ��������

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

            //using (var sha256 = SHA256.Create())
            //using (var stream = File.OpenRead(filePath))
            //{
            //    return BitConverter.ToString(sha256.ComputeHash(stream))
            //        .Replace("-", "").ToLowerInvariant();
            //}


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

        // ������������ԭ��
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

        #region UI�¼�����
        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is ProgressData data)
            {
                UpdateProgressBar(data);
                UpdateDiffList(data);
            }


        }

        private void UpdateProgressBar(ProgressData data)
        {    // ���½�����
            //if (data.TotalFiles > 0)
            //    prbProd.Maximum = data.TotalFiles;

            //prbProd.Value = Math.Min(data.Processed, prbProd.Maximum);
            // ��ʼ��������
            if (data.TotalFiles > 0 && prbProd.Maximum != data.TotalFiles)
            {
                prbProd.Maximum = data.TotalFiles;
            }

            // ���½���ֵ
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


        // ʵʱ���²����б�
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
                ShowErrorMessage($"����ʧ��: {e.Error.Message}");
            }
            else if (e.Cancelled)
            {
                ShowInformationMessage("������ȡ��");
            }
            else
            {
                HandleSuccessfulCompletion(e.Result as UpdateXmlResult);
            }

        }


        private void HandleSuccessfulCompletion(UpdateXmlResult result)
        {

            XDocument document = result?.Document;
            // �ڱ����ĵ�ǰ����
            UpdateLastUpdateTime(document);

            txtLastXml.Text = document.ToString();
            //txtLastXml.Text = result?.Document.ToString();
            rtbNew.Text = txtLastXml.Text;
            tabControl1.SelectedTab = tbpLastXml;
            btnDiff.Enabled = true;
            AppendLog("XML�ļ����ɳɹ���");
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AppendLog($"����: {message}");
        }

        private void ShowInformationMessage(string message)
        {
            MessageBox.Show(message, "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppendLog(message);
        }

        private void AppendLog(string message)
        {
            richTxtLog.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}\r\n");
        }
        #endregion

        #region �ļ�����
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
                AppendLog($"�ļ�����ʧ��: {ex.Message}");
            }
        }
        #endregion








        /// <summary>
        /// �汾�ż�1
        /// </summary>
        /// <param name="version"></param>
        private void IncrementVersion(ref string version)
        {
            //var parts = version.Split('.').Select(int.Parse).ToArray();
            //parts[3]++;
            //version = string.Join(".", parts);

            var parts = version.Split('.');
            parts[3] = (int.Parse(parts[3]) + 1).ToString();
            version = string.Join(".", parts);
        }

        #region [дAutoUpdaterList]






        /*
        /// <summary>
        /// �Ƿ���й�ϣֵ�Ƚϣ�����ǣ����ϣֵ��һ�������Ӱ汾�š����򲻱�
        /// </summary>
        /// <param name="HashValueComparison"></param>
        void WriterAUList()
        {

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
            sb.Append("\t\t<Version>");

            FileVersionInfo _lcObjFVI = FileVersionInfo.GetVersionInfo(this.txtMainExePath.Text);
            if (chkUseBaseVersion.Checked)
            {
                if (this.txtBaseExeVersion.Text.TrimEnd().Length == 0)
                {
                    MessageBox.Show("ѡ��ָ���汾������û������汾��");
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

            //���ָ����Ҫ���µ��ļ�,��������� ��˼�Ǹ��µ��б�Ҫ������嵥��
            if (txtPreVerUpdatedFiles.Text.Trim().Length > 0)
            {
                string[] files = txtPreVerUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                //����
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
                        //��ʱͳһ�ˡ����ָ���汾��
                        if (chk�����ļ��汾��.Checked)
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
                    string rootDir = this.txtMainExePath.Text.Substring(0, this.txtMainExePath.Text.LastIndexOf(@"\")) + @"\";
                    if (chkUseBaseVersion.Checked)
                    {
                        if (this.txtBaseExeVersion.Text.TrimEnd().Length == 0)
                        {
                            MessageBox.Show("ѡ��ָ���汾������û������汾��");
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

            #endregion [дAutoUpdaterlist]
        }
        */

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

        /// <summary>
        /// �ų�����Ҫ���ļ�
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Ϊ�����ų�</returns>
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


        #endregion [�ų�����Ҫ���ļ�]

        #endregion [�����ļ�]

        private void button_save_config_Click(object sender, EventArgs e)
        {
            //�������õ��ļ�
            try
            {
                saveConfigToFile();
                richTxtLog.AppendText("���ñ���ɹ���" + configFilePath);
                //MessageBox.Show("���ñ���ɹ�");
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ñ���ʧ��:" + ex.Message);
            }
        }



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

            //�Ѿ����ڵ��ļ� ������������жϲ�Ҫ�ظ�
            string[] files = txtPreVerUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //����
            Array.Sort(array);
            List<string> addFiles = new List<string>();
            foreach (string path in array)
            {
                //������Ͻ������ļ� ��ȫ·����Ҫȥǰ������Դ��ͬ��Ŀ¼ȥ��
                if (path.IndexOf(txtCompareSource.Text) == 0)
                {
                    addFiles.Add(path.Substring(txtCompareSource.Text.Length).TrimStart('\\'));
                }
            }

            //����
            addFiles.Sort();
            foreach (string path in addFiles)
            {

                if (files.Contains(path))
                {
                    richTxtLog.AppendText($"{path}���·���Ѵ��ڽ�����ԡ�\r\n");
                    //�Ѿ�����
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
                    richTxtLog.AppendText($"{path}��Ч��·���������ļ�ʧ�ܣ�\r\n");
                    Console.WriteLine("��Ч��·��");
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

        List<string> DiffFileList = new List<string>();

        /// <summary>
        /// ���������AI��д�ˡ���������ȷ�ġ�AI��Ҫ�����֤
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
                // ���� <Version> Ԫ��
                var versionElement = doc.Descendants("Version").FirstOrDefault();
                if (versionElement != null)
                {
                    versionElement.Value = txtBaseExeVersion.Text; // �滻Ϊ����Ҫ���õ���ֵ
                }
            }
            //����NAS�ķ���Ŀ�������е��ļ��е��嵥�ļ�����
            string[] files = txtPreVerUpdatedFiles.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            //����
            Array.Sort(files);
            List<string> list = files.ToList();
            var XElementFiles = doc.Descendants("File");
            foreach (XElement fileElement in XElementFiles)
            {
                string fileName = fileElement.Attribute("Name").Value;
                if (list.Count > 0 && !list.Contains(fileName))
                {
                    // ����ļ��������ļ��б��У�ɾ�����ļ�
                    // fileElement.Remove();
                    //��������ʱ�ǰ汾�Ų��䡣Ҳ���ǲ����¡�
                }
                //����ļ����ų��б���Ҳ�Ƴ�
                if (ExcludeUnnecessaryFiles(fileName))
                {
                    //fileElement.Remove();
                    continue;
                    //��������ʱ�ǰ汾�Ų��䡣Ҳ���ǲ����¡�  �Ƴ��ᵼ��xml�ļ��ṹ�ƻ�
                    //����ǵ�һ�����ǲ���������

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
                        // ����ļ���С���޸�ʱ�䲻ͬ��ֱ�Ӹ��°汾��
                        string version = fileElement.Attribute("Ver").Value;
                        IncrementVersion(ref version);
                        fileElement.SetAttributeValue("Ver", version);
                        DiffFileList.Add(fileName);
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
                            DiffFileList.Add(fileName);
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
                            DiffFileList.Add(fileName);
                            txtDiff.AppendText(fileName + "\r\n");
                        }
                    }

                }
                else
                {
                    //���ñȽ��ˡ�ֱ������һ���汾��
                    string version = fileElement.Attribute("Ver").Value;
                    IncrementVersion(ref version);
                    fileElement.SetAttributeValue("Ver", version);

                }
                //���ﲻ�Ƚ��ļ�ʱ������ֻ���������ӵ���Ŀ �ԡ���Ҫ���ɵ��嵥����ɵ�AutoUpdaterList �е��ļ��б���Ƚ�
                //�ɵĴ��ڵġ�����ʲô��ʽ�������˾�ȥ��Ŀ���嵥���������ľ��������ӵ�
                list.Remove(fileName);
            }

            foreach (var item in list)
            {
                //����ļ����ų��б���,���ԡ������
                if (ExcludeUnnecessaryFiles(item))
                {
                    continue;
                }
                //����µ��ļ������xml��File�ڵ�����
                XElement newFileElement = new XElement("File");
                newFileElement.SetAttributeValue("Ver", "1.0.0.0");
                newFileElement.SetAttributeValue("Name", item);
                var FilesElement = doc.Descendants("Files").FirstOrDefault();
                FilesElement.Add(newFileElement);
                DiffFileList.Add(item);
                txtDiff.AppendText(item + "\r\n");
            }


            txtLastXml.Text = doc.ToString();
            richTxtLog.AppendText("�������µ�XML�ļ�����ɹ�����������������������\r\n");

            //}
            //else
            //{
            //    doc.Save(xmlFilePath);
            //    richTxtLog.AppendText("����-�������µ�XML�ļ��ɹ���\r\n");
            //}
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

        private void chk��ϣֵ�Ƚ�_CheckedChanged(object sender, EventArgs e)
        {
            //����ǰһ��AutoUpdaterList.xml�е��ļ��б�
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
            if (DiffFileList.Count == 0)
            {
                MessageBox.Show("û����Ҫ�������ļ����������ɲ����ļ���");
                return;
            }
            foreach (var item in DiffFileList)
            {
                if (!chkTest.Checked)
                {
                    CopyFile(txtCompareSource.Text, txtTargetDirectory.Text, item);
                }

            }

            richTxtLog.AppendText($"���浽Ŀ��-{txtTargetDirectory.Text}  �ɹ�{DiffFileList.Count}����");
            richTxtLog.AppendText("\r\n");

            if (chkTest.Checked)
            {
                return;
            }

            // ��StringBuilder������д���ļ�
            //File.WriteAllText(this.txtAutoUpdateXmlSavePath.Text.Trim(), txtLastXml.Text.ToString(), System.Text.Encoding.GetEncoding("gb2312"));
            XDocument newDoc = XDocument.Parse(txtLastXml.Text);
            var tempPath = Path.Combine(Path.GetTempPath(), this.txtAutoUpdateXmlSavePath.Text.Trim());

            // ��������gb2312�����XMLд������
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.GetEncoding("gb2312");
            settings.Indent = true; // ����������ʽ

            // ʹ��XmlWriter�����ĵ�
            using (XmlWriter writer = XmlWriter.Create(tempPath, settings))
            {
                newDoc.Save(writer);
            }

            MessageBox.Show(this, "�Զ������ļ����ɳɹ�:" + this.txtAutoUpdateXmlSavePath.Text.Trim(), "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {
                saveConfigToFile();
                //MessageBox.Show("���ñ���ɹ�");
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ñ���ʧ��:" + ex.Message);
            }

            richTxtLog.AppendText($"������Ŀ��-{txtTargetDirectory}�ɹ���");
            richTxtLog.AppendText("\r\n");
        }




        #region �¾ɰ汾�Ƚ�

        private void CompareUpdateXml(string OldConfigPath)
        {
            if (string.IsNullOrWhiteSpace(OldConfigPath))
            {
                MessageBox.Show("����ѡ������Ҫ�Ƚϵ������ļ�");
                return;
            }

            try
            {
                rtbOld.Clear();
                rtbNew.Clear();

                XDocument oldDoc = XDocument.Load(OldConfigPath);

                XDocument newDoc = XDocument.Parse(txtLastXml.Text);

                // �Ƚ�XML�ĵ�
                var comparer = new EnhancedXmlDiff();
                var diffBlocks = comparer.CompareXmlFiles(oldDoc, newDoc);
                // ��ʾ����
                var diffViewer = new XmlDiffViewer();
                // ��ʾ����
                //diffViewer.DisplayDifferences(diffBlocks);
                try
                {

                    var comparer1 = new XmlComparer();
                    var diffBlocks1 = comparer1.CompareXml(oldDoc, newDoc);

                    diffViewer.DisplayDifferences(diffBlocks1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"�Ƚ�ʧ�ܣ�{ex.Message}");
                }


                // �ڴ�������ʾ
                var form = new Form();
                form.Text = "XML����ȽϹ���";
                form.Size = new Size(1200, 800);
                form.Controls.Add(diffViewer);
                diffViewer.Dock = DockStyle.Fill;
                form.Show();

                ShowXmlComparison(oldDoc, newDoc);


                CompareXmlDocuments(oldDoc, newDoc);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"�Ƚ������ļ�ʱ����: {ex.Message}");
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


        // �Ľ��������������㷨
        private List<DiffSegment> ComputeEnhancedInlineDiff(string left, string right)
        {
            var segments = new List<DiffSegment>();

            if (left == right)
            {
                segments.Add(new DiffSegment { Text = left, IsModified = false });
                return segments;
            }

            // ʹ�ø���ϸ�Ĳ����㷨������ڴʻ��ǵĲ��죩
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
                MessageBox.Show($"��Beyond Compareʧ�ܣ�{ex.Message}");
            }
            //return;
            //�Ƚ�
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
            string mainProgramFileName = "��ҵ���ֻ�����ERP.exe";
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


    #region ������
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
    }

    public class ProgressData
    {
        public int TotalFiles { get; set; }
        public int Processed { get; set; }
        public List<string> DiffItems { get; set; }
    }
    #endregion


}
