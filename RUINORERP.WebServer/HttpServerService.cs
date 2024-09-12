using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WebServer
{
    internal class HttpServerService
    {
        private static bool isExcute = true;
        private static HttpListener listener = new HttpListener();
        public static void Start()
        {
            //单独开启一个线程执行监听消息
            System.Threading.ThreadPool.QueueUserWorkItem(w => Excute());
        }

        private static void Excute()
        {
            if (HttpListener.IsSupported)
            {
                if (!listener.IsListening)
                {
                    //添加需要监听的url
                    listener.Prefixes.Add("http://127.0.0.1:8888/");
                    //开始监听端口，接收客户端请求
                    listener.Start();
                }
                while (isExcute)
                {
                    try
                    {
                        //阻塞主函数至接收到一个客户端请求为止  等待请求
                        HttpListenerContext context = listener.GetContext();
                        //解析请求
                        HttpListenerRequest request = context.Request;
                        //构造响应
                        HttpListenerResponse response = context.Response;
                        string httpMethod = request.HttpMethod?.ToLower();
                        string rawUrl = request.RawUrl;
                        var Url = request.Url;
                        if (httpMethod == "get")
                        {
                            //获取查询参数
                            var queryString = request.QueryString;
                            //TODO 其他操作
                        }
                        else if (httpMethod == "post")
                        {
                            // TODO 处理请求体数据 
                            var reader = new StreamReader(request.InputStream);
                            var questBody = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(rawUrl))
                            {
                                //TODO 反序列化RequestBody，调用其他业务
                            }
                        }

                        var responseString = string.Empty;
                        responseString = JsonConvert.SerializeObject(new { code = 1, msg = "发送成功"+System.DateTime.Now.ToString() });
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        //对客户端输出相应信息.
                        response.ContentLength64 = buffer.Length;
                        //发送响应
                        using (System.IO.Stream output = response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }
                    catch (Exception exceotion)
                    {
                        string str = exceotion.Message;
                    }
                }
            }
            else
            {
                // TODO  系统不支持HttpListener
            }
        }

        public static void Stop()
        {
            isExcute = false;
            if (listener.IsListening)
                listener.Stop();
        }
    }
}
