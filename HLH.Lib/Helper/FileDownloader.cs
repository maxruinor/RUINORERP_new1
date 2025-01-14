/* ----------------------------------------------------------------------
 * 
 *  Author: Shailen Sukul BSC, MCSD.Net, Mcsd, Mcad
 *  Date: 9/April/2006
 *  Copyright Ashlen Consulting Services P/L
 * ----------------------------------------------------------------------*/
using System;
using System.IO;
using System.Net;
namespace HLH.Lib.Helper
{
    /// <summary>
    /// FileDownloader encapsulates the file download operation.
    /// </summary>
    public class FileDownloader
    {
        #region  Fields
        private string _DocumentUrl = string.Empty;

        /// <summary>
        /// 下载文件的地址
        /// </summary>
        public string DocumentUrl
        {
            get { return _DocumentUrl; }
            set { _DocumentUrl = value; }
        }
        private string _DirectoryPath = string.Empty;
        public bool _IsDownloading = false;
        public bool _IsDownloadSuccessful = false;
        public bool _IsStarted = false;

        #endregion

        #region Delegates
        public delegate void _delDownloadStarting(FileDownloader thread);
        public delegate void _delDownloadCompleted(FileDownloader thread, bool isSuccess, bool isSaveToDb);

        #endregion

        #region Events
        public event _delDownloadStarting DownloadStarting;
        public event _delDownloadCompleted DownloadCompleted;

        #endregion


        public delegate void _debugTrackerHandler(Exception ex, string msg);
        public event _debugTrackerHandler _debugTrackerEvent;

        #region Properties

        private string proxyUri = string.Empty;

        public string ProxyUri
        {
            get { return proxyUri; }
            set { proxyUri = value; }
        }

        private string absolutefilename;

        /// <summary>
        /// 相对于本地的 绝对地址 如F://test/a.jpg
        /// </summary>
        public string Absolutefilename
        {
            get { return absolutefilename; }
            set { absolutefilename = value; }
        }

        private string filename = string.Empty;

        /// <summary>
        /// 保存文件时的文件名，如果不设置则为下载地址中取名字
        /// </summary>
        public string FileName
        {
            get
            {
                if (_DocumentUrl.Equals(string.Empty))
                {
                    throw new ArgumentException("Please supply a document url.");
                }
                if (filename.Equals(string.Empty))
                {
                    int loc = _DocumentUrl.LastIndexOf("/") + 1;
                    int len = _DocumentUrl.Length - loc;
                    filename = _DocumentUrl.Substring(loc, len);
                }
                return filename;
            }
            set
            {
                filename = value;
            }
        }

        #endregion


        private object dataPara;

        /// <summary>
        /// 用于传参数
        /// </summary>
        public object DataPara
        {
            get { return dataPara; }
            set { dataPara = value; }
        }

        private string contentID = string.Empty;

        /// <summary>
        /// 内容ID，传递参数用
        /// </summary>
        public string ContentID
        {
            get { return contentID; }
            set { contentID = value; }
        }


        #region Methods

        #region ctor
        public FileDownloader(string documentUrl, string directory)
        {
            _DocumentUrl = documentUrl;
            _DirectoryPath = directory;
        }
        #endregion

        /// <summary>
        /// Starts the download of the attached url into the given directory.
        /// </summary>
        /// <param name="isSaveToDb">是否需要保存数据到数据库 因为是线程传参数 所以需要用ojbect转bool</param>
        public void StartDownload(object isSaveToDb)
        {
            if (_DocumentUrl.Equals(string.Empty))
            {
                throw new ArgumentException("Please supply a document url.");
            }
            if (Absolutefilename.Trim().Length == 0 && _DirectoryPath.Equals(string.Empty))
            {
                throw new ArgumentException("Please supply a directory.");
            }
            _IsStarted = true;
            /* raise the download starting event. */
            if (DownloadStarting != null)
            {
                DownloadStarting(this);
            }


            _IsDownloading = true;
            _IsDownloadSuccessful = false;
            Stream stream = null;
            FileStream fstream = null;

            try
            {
                string destFileName = string.Empty;

                if (Absolutefilename.ToString().Trim().Length == 0)
                {
                    destFileName = _DirectoryPath + "\\" + FileName;
                }
                else
                {
                    destFileName = Absolutefilename;
                }
                destFileName = destFileName.Replace("/", " ").Replace("%20", " ");


                //判断如果目录不存在则创建
                System.IO.FileInfo fi = new FileInfo(destFileName);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }


                ///如果图片不存在，则下载
                if (File.Exists(destFileName) == false)
                {
                    IWebProxy proxy = null;
                    if (ProxyUri != null && ProxyUri != string.Empty)
                    {
                        proxy = new WebProxy(ProxyUri, true);
                        proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    }
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_DocumentUrl);
                    //  if (proxy != null )

                    if (proxy != null && !_DocumentUrl.ToLower().StartsWith("https"))
                    {
                        request.Proxy = proxy;
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    stream = response.GetResponseStream();

                    byte[] inBuffer = ReadFully(stream, 32768);

                    fstream = new FileStream(destFileName, FileMode.OpenOrCreate, FileAccess.Write);
                    fstream.Write(inBuffer, 0, inBuffer.Length);


                    fstream.Close();
                    stream.Close();
                }
                _IsDownloadSuccessful = true;
                _IsDownloading = false;

                /* raise a download completed event.引发下载完成事件 */
                if (DownloadCompleted != null)
                {
                    DownloadCompleted(this, _IsDownloadSuccessful, bool.Parse(isSaveToDb.ToString()));
                }

            }
            catch (Exception ef)
            {
                _IsDownloadSuccessful = false;
                if (DownloadCompleted != null)
                {
                    DownloadCompleted(this, _IsDownloadSuccessful, bool.Parse(isSaveToDb.ToString()));
                }
                if (_debugTrackerEvent != null)
                {
                    _debugTrackerEvent(ef, "文件下载失败。" + _DocumentUrl + "---" + ef.Message + "==" + ef.StackTrace);
                }
            }
            finally
            {
                if (fstream != null)
                {
                    fstream.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public byte[] ReadFully(Stream stream, int initialLength)
        {
            /* If we've been passed an unhelpful initial length, just
             use 32K. */
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                /* If we've reached the end of our buffer, check to see if there's
                 any more information */
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    /* End of stream? If so, we're done */
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    /* Nope. Resize the buffer, put in the byte we've just
                     read, and continue */
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            /* Buffer is now too big. Shrink it. */
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
        #endregion
    }





}
