using Crawler;
using HLH.Lib.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonProcess.StringProcess
{

    [Serializable]
    public class DownloadFileInfo
    {
        private int _id = 0;
        private bool _downloadSuccess = false;
        private string _oldName = string.Empty;
        private string _url = string.Empty;
        private string _localPath = string.Empty;
        private object _otherpara;
        public int Id { get => _id; set => _id = value; }

        public bool DownloadSuccess { get => _downloadSuccess; set => _downloadSuccess = value; }
        public string OldName { get => _oldName; set => _oldName = value; }
        public string Url { get => _url; set => _url = value; }
        public string LocalPath { get => _localPath; set => _localPath = value; }
        public object Otherpara { get => _otherpara; set => _otherpara = value; }
    }

    [Serializable]
    public class UCDownloadFilePara : UCBasePara
    {
        public bool _DownloadTypeisHtmlPage = true;
        /// <summary>
        /// 下载类型如果是网页页面为真
        /// </summary>
        public bool DownloadTypeisHtmlPage
        {
            get { return _DownloadTypeisHtmlPage; }
            set { _DownloadTypeisHtmlPage = value; }
        }
        private List<DownloadFileInfo> _fileList = new List<DownloadFileInfo>();

        /// <summary>
        /// 如果下载文件时，成功下载的文件列表
        /// /// </summary>
        public List<DownloadFileInfo> SuccessFileList { get => _fileList; set => _fileList = value; }


        private string _MultipleFileDelimiter = "#||#";
        /// <summary>
        /// 多个文件时的分隔符
        /// </summary>
        public string MultipleFileDelimiter
        {
            get { return _MultipleFileDelimiter; }
            set { _MultipleFileDelimiter = value; }
        }


        public bool Is缓存HTML内容 { get => is缓存HTML内容; set => is缓存HTML内容 = value; }

        private bool is缓存HTML内容 = false;

        public string _HTML缓存内容 = string.Empty;

        public string HTML缓存内容
        {
            get { return _HTML缓存内容; }
            set { _HTML缓存内容 = value; }
        }

        #region 文件下载相关属性

        private bool is将相对地址补全为绝对地址 = false;

        public bool Is将相对地址补全为绝对地址
        {
            get { return is将相对地址补全为绝对地址; }
            set { is将相对地址补全为绝对地址 = value; }
        }
        private bool is下载图片 = false;

        public bool Is下载图片
        {
            get { return is下载图片; }
            set { is下载图片 = value; }
        }
        private bool is探测文件地址并下载 = false;

        public bool Is探测文件地址并下载
        {
            get { return is探测文件地址并下载; }
            set { is探测文件地址并下载 = value; }
        }
        private bool is探测文件地址但不下载 = false;

        public bool Is探测文件地址但不下载
        {
            get { return is探测文件地址但不下载; }
            set { is探测文件地址但不下载 = value; }
        }

        private string fileSaveformat = string.Empty;

        public string FileSaveformat
        {
            get { return fileSaveformat; }
            set { fileSaveformat = value; }
        }

        /// <summary>
        /// 图片在内容表中的字段 保存是否用绝对路径 如果是注意DB长度
        /// </summary>
        public bool IsAbsolutePath { get; set; }

        public string ImgSaveDirectory { get; set; }

        #endregion
        public UCDownloadFilePara()
        {

        }

        /// <summary>
        /// 外部下载
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate string DownloadHandler(object Para);


        [Browsable(true), Description("外部下载")]
        public event DownloadHandler DownloadEvent;


        public override string ProcessDo(string StrIn)
        {
            string rs = StrIn;
            if (ImgSaveDirectory.Trim().Length == 0 && !DownloadTypeisHtmlPage)
            {
                OnDebugTacker("保存目录不能为空！");
                //throw new Exception("保存目录不能为空！");
            }
            else
            {
                if (is缓存HTML内容)
                {
                    rs = HTML缓存内容;
                }
                else
                {
                    // 优先外部事件的方法  暂时没有应用场景  后面优化
                    if (DownloadEvent != null)
                    {
                        rs = DownloadEvent(StrIn);
                    }
                    else
                    {

                        if (DownloadTypeisHtmlPage)
                        {
                            rs = DownloadHtml(StrIn);
                        }
                        else
                        {
                            //要提醒调用 方法 如果多个图片的分割符是，
                            rs = DownLoadfile(StrIn);
                        }
                    }

                }
                //下载
            }


            return rs;
        }



        private string DownloadHtml(string url)
        {
            //判断是否是url格式 ，否则报错
            string htmlcode = string.Empty;
            WebPageState state = new WebPageState(new Uri(url));
            //state.PagePara = ptp;
            WebPageProcessor processor = new WebPageProcessor();
            //如果缓存结果，则不需要采集，也不需要分析
            processor.ContentHandler += new WebPageContentDelegate(HandleContent);
            processor.Process(state);
            if ((state.StatusCode == "OK" && state.ProcessSuccessfull))
            {
                /////给后面测试用
                //ptp.TestContentUrl = url;
                //ptp.TestContent = state.Content;
                htmlcode = state.Content;
            }
            return htmlcode;
        }

        private void HandleContent(WebPageState state)
        {
            // throw new NotImplementedException();
        }


        /// <summary>
        /// 文件下载成功的话，得到本地路径,号分割
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        private string DownLoadfile(string urls)
        {
            string rs = string.Empty;
            List<DownloadFileInfo> imgs = new List<DownloadFileInfo>();
            #region 下载产品图片到本地，再按下面的处理
            string[] ImgUrls;
            if (!urls.Contains("#||#"))
            {
                ImgUrls = urls.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                ImgUrls = urls.Split(new char[] { '#', '|', '|', '#' }, StringSplitOptions.RemoveEmptyEntries);
            }



            foreach (string url in ImgUrls)
            {
                string tempUrl = url;
                tempUrl = tempUrl.Trim().TrimStart('\"');
                tempUrl = tempUrl.TrimEnd('\"');
                System.IO.FileInfo info = new FileInfo(tempUrl.Substring(9));
                DownloadFileInfo img = new DownloadFileInfo();
                img.Url = tempUrl;
                img.OldName = info.Name;
                img.LocalPath = System.IO.Path.Combine(ImgSaveDirectory, info.Name);
                imgs.Add(img);
            }
            //使用下载器下载
            FileDownloader download = null;
            foreach (DownloadFileInfo kv in imgs)
            {
                bool oners = true;
                //key 为url,value 为path
                if (kv.Url.Length > 5 && this.Is下载图片)
                {
                    download = new FileDownloader(kv.Url.ToString(), this.ImgSaveDirectory);
                    download.DownloadStarting += new FileDownloader._delDownloadStarting(download_DownloadStarting);
                    download.DownloadCompleted += new FileDownloader._delDownloadCompleted(download_DownloadCompleted);
                    download.DataPara = kv;
                    //download.ProxyUri = kv.Key.ToString();
                    //filename = kv.Value;
                    download.Absolutefilename = kv.LocalPath;
                    download._debugTrackerEvent += Download__debugTrackerEvent;
                    try
                    {
                        download.ProxyUri = kv.Url.ToString();
                        Thread t = new Thread(new ParameterizedThreadStart(download.StartDownload));
                        t.Start(true);
                    }
                    catch (Exception ex)
                    {
                        OnDebugTacker(this, ex);
                        /* If the download fails for some reason, flag and error. */
                        oners = false;
                    }

                }
            }
            #endregion
            SuccessFileList = imgs;
            rs = urls;
            return rs;
        }

        private void Download__debugTrackerEvent(Exception ex, string msg)
        {
            OnDebugTacker(this, ex);
        }

        //下载图片等
        /// <summary>
        /// Gets triggered when the download has started.
        /// </summary>
        /// <param name="thread"></param>
        void download_DownloadStarting(FileDownloader thread)
        {
            PrintDebugInfo("开始下载：" + thread.FileName);
            // SetStatus("Downloading", thread);
            // frmMain.InstancePicker.PrintInfoLog("开始下载：" + thread.FileName, Color.Brown);
        }

        /// <summary>
        /// 文件下载完成事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void DownloadCompleted(object Parameters);


        [Browsable(true), Description("文件下载完成事件")]
        public event DownloadCompleted DownloadCompletedEvent;



        /// <summary>
        /// Gets trigerred when the download has completed.
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="isSuccess"></param>
        void download_DownloadCompleted(FileDownloader thread, bool isSuccess, bool isSaveToDb)
        {
            PrintDebugInfo("下载完成：" + thread.FileName);
            DownloadFileInfo file = thread.DataPara as DownloadFileInfo;
            file.DownloadSuccess = true;
            if (isSaveToDb)
            {

            }
            //如果下载完成一个 则通知一次
            if (DownloadCompletedEvent != null)
            {
                DownloadCompletedEvent(thread.Absolutefilename);
            }
        }

        public string FilesToString()
        {
            string rs = string.Empty;
            foreach (DownloadFileInfo file in this.SuccessFileList)
            {
                rs += file.Url + "#||#";
                // string[] tempimgs = df.FieldValue.Split(new string[] { "#||#" }, StringSplitOptions.RemoveEmptyEntries);
                // imgs += fileNameForDownload + "#||#";
            }

            rs = rs.TrimEnd(new char[] { '#', '|', '|', '#' });
            return rs;
        }


    }
}
