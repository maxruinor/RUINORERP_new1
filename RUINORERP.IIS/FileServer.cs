using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.IIS
{
    public class FileServer
    {
        private static FileServer _instance;
        private HttpListener _listener;
        private string _rootDirectory;

        private FileServer()
        {
            _rootDirectory = @"D:\Files\"; // 指定文件根目录
        }

        public static FileServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileServer();
                }
                return _instance;
            }
        }

        public void Start()
        {
            if (_listener != null && _listener.IsListening)
            {
                frmMain.Instance.logViewer1.AddLog("");
                throw new InvalidOperationException("文件服务器已在运行。");
            }

            string url = "http://localhost:8080/";

            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add(url);
                _listener.Start();

                frmMain.Instance.logViewer1.AddLog($"File Server is running. Listening on {url}");

                while (true)
                {
                    HttpListenerContext context = _listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    string filePath = string.Empty;
                    if (request.Url != null)
                    {
                         filePath = Path.Combine(_rootDirectory, request.Url.LocalPath.TrimStart('/'));
                    }

                    if (File.Exists(filePath))
                    {
                        byte[] buffer = File.ReadAllBytes(filePath);

                        response.ContentType = GetContentType(filePath);
                        response.ContentLength64 = buffer.Length;

                        response.OutputStream.Write(buffer, 0, buffer.Length);
                        response.OutputStream.Close();
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                frmMain.Instance.logViewer1.AddLog($"Error: {ex.Message}");
            }
        }

        public void Stop()
        {
            if (_listener != null && _listener.IsListening)
            {
                _listener.Stop();
                _listener.Close();
                _listener = null;

                frmMain.Instance.logViewer1.AddLog("File Server stopped.");
            }
        }

        private string GetContentType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            switch (extension)
            {
                case ".txt":
                    return "text/plain";
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
