using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RUINORERP.Model.Context;
using RUINORERP.Model;
using RUINORERP.WebServerConsole.Comm;
using SimpleHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.VisualBasic;

namespace RUINORERP.WebServerConsole
{
    internal class WebServer
    {
        string webDir = Path.Combine(Directory.GetCurrentDirectory(), "Site");
        int MaxFileSize = 1024 * 1024 * 10; //10MB
        private List<tb_SysGlobalDynamicConfig> _sysGlobalDynamicConfigs;
        private readonly ConfigManager _configManager;
        private readonly ApplicationContext _appContext;
        private readonly ILoggerService _logger; // 假设引入了日志库
        private readonly IAuthenticationService _authService;
        private readonly IAuthorizationService _authzService;

        public WebServer(ApplicationContext appContext, ConfigManager configManager,
            ILoggerService logger, IAuthenticationService authService, IAuthorizationService authzService)
        {
            _appContext = appContext;
            _configManager = configManager;
            _logger = logger;
            _authService = authService;
            _authzService = authzService;
        }


        public async void RunWebServer()
        {
            //------------------- define routes -------------------
            #region
            /*
   Route.Before = (rq, rp) => { Console.WriteLine($"Requested: {rq.Url.PathAndQuery}"); return false; };

   //home page
   Route.Add("/", (rq, rp, args) =>
   {
       rp.AsFile(rq, Path.Combine(webDir, "Index.html"));
   });

   //1) URL parsing demo
   Route.Add("/{action}/{paramA}-{paramB}", (rq, rp, args) =>
   {
       var txt = Templating.RenderFile(Path.Combine(webDir, "UrlParsingResponse.thtml"), args); //populate template
       rp.AsText(txt);
   });

   //2) serve file (and video streaming)
   Route.Add((rq, args) =>
   {
       args["file"] = Path.Combine(webDir, rq.Url.LocalPath.TrimStart('/'));
       return Path.HasExtension(args["file"]);
   },
   (rq, rp, args) => rp.AsFile(rq, args["file"]));

   //3) form parsing demo
   Route.Add("/upload/", (rq, rp, args) =>
   {
       var files = rq.ParseBody(args);

       //这里写死的。实际可能要配置性的。
       string ServerImagesDir = Path.Combine(webDir, "ERPImages");

       foreach (var f in files.Values)
           f.Save(Path.Combine(ServerImagesDir, f.FileName), true);


       var txtRp = "Form fields: " + String.Join(";  ", args.Select(x => $"'{x.Key}: {x.Value}'")) + "\n" +
                   "Files:       " + String.Join(";  ", files.Select(x => $"'{x.Key}: {x.Value.FileName}, {x.Value.ContentType}'"));

       rp.AsText($"<pre>{txtRp}</pre>");
   },
   "POST");

   //4) handle exception demo
   Route.Add("/handleException/", (rq, rp, args) =>
   {
       //to override/stylize default error response define a custom error function: Route.Error = (rq, rp, ex) => { };
       throw new NotImplementedException("My not implemented exception.");
   });

   //1) api demo
   Route.Add("/api/v1/users", (rq, rp, args) =>
   {
       var txt = Templating.RenderFile(Path.Combine(webDir, "UrlParsingResponse.thtml"), args); //populate template
       rp.AsText(GetUsers(rq));
   });

   //4) 删除图片
   Route.Add("/deleteImages/", (rq, rp, args) =>
   {
       var serverDir = _configManager.GetValue("ServerImageDirectory");
       //to override/stylize default error response define a custom error function: Route.Error = (rq, rp, ex) => { };
       if (System.IO.File.Exists(System.IO.Path.Combine(webDir + serverDir) + "/" + args["image"]))
       {

       }
   });
   */
            #endregion

            try
            {
                _sysGlobalDynamicConfigs = _appContext.Db.Queryable<tb_SysGlobalDynamicConfig>().ToList();

                ConfigureRoutes();
                //------------------- start server -------------------           
                var port = 8081;
                Console.WriteLine("Running HTTP server on: " + port);
                _logger.LogInformation("Running HTTP server on: " + port);
                string msg = string.Empty;
                CancellationTokenSource cts = new CancellationTokenSource();
                var ts = HttpServer.ListenAsync(port, cts.Token, Route.OnHttpRequestAsync, msg, useHttps: false);
                if (!string.IsNullOrEmpty(msg))
                {
                    _logger.LogInformation("Running HTTP server on: " + port);
                    Console.WriteLine(msg);
                }


                AppExit.WaitFor(cts, ts);



            }

            catch (Exception exx)
            {
                _logger.LogError("错误exx", exx);
            }
        }

        private void ConfigureRoutes()
        {
            Route.Before = (rq, rp) => { Console.WriteLine($"Requested: {rq.Url.PathAndQuery}"); return false; };
            Route.Error = HandleError;
            Route.Add("/", HandleHomePage, "GET");
            Route.Add("/{ERPImages}/", HandleViewImagesFiles, "GET");
            //改为？号分割
            Route.Add("/{action}/{paramA}?{paramB}", HandleUrlParsingDemo, "GET");///ERPImages/01J8Q1Y1A3SNCD545HFGGSSTQG-a71b4c9a927824741768f76a89624f32.jpg
            Route.Add("/upload/", HandleFormParsingUpload, "POST");
            Route.Add("/login", HandleLogin, "POST");
            Route.Add("/api/v1/users", HandleApiDemo, "GET");
            Route.Add("/delete", HandleDelete, "DELETE"); // 删除路由，仅处理DELETE请求
            Route.Add("/deleteImages", HandleDeleteImages, "DELETE");
            Route.Add("/favicon.ico", (rq, rp, args) =>
            {
                rp.AsFile(rq, Path.Combine(webDir, "favicon.ico"));
            });
            // 4) 处理异常演示
            Route.Add("/handleException/", (rq, rp, args) =>
            {
                throw new NotImplementedException("My not implemented exception.");
            });


            // 6) 删除图片
            //Route.Add("/deleteImages/", (rq, rp, args) =>
            //{
            //    var serverDir = _configManager.GetValue("ServerImageDirectory");
            //    string imagePath = Path.Combine(webDir, serverDir, args["image"]);
            //    if (File.Exists(imagePath))
            //    {
            //        File.Delete(imagePath);
            //        rp.AsText("Image deleted successfully.");
            //    }
            //    else
            //    {
            //        rp.AsText("Image not found.", HttpStatusCode.NotFound.GetDescription());
            //    }
            //});
            // 7) 未找到的路由
            Route.Add("", (rq, rp, args) =>
            {
                rp.AsText("404 Not Found", HttpStatusCode.NotFound.GetDescription());
            });

            // 8) 内部服务器错误
            Route.Error = (rq, rp, ex) =>
            {
                rp.AsText($"500 Internal Server Error: {ex.Message}", HttpStatusCode.InternalServerError.GetDescription());
            };

            // 2) 服务静态文件 放到最后 包含上面有没有没有处理的情况
            Route.Add((rq, args) =>
            {
                string filePath = Path.Combine(webDir, rq.Url.LocalPath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    args["file"] = filePath;
                    return true;
                }
                return false;
            }, (rq, rp, args) => rp.AsFile(rq, args["file"]));

        }
        // 封装异常处理
        private void HandleError(HttpListenerRequest rq, HttpListenerResponse rp, Exception ex)
        {
            _logger.LogError($"Error processing request: {ex.Message}", ex);
            rp.AsText($"500 Internal Server Error: {ex.Message}", HttpStatusCode.InternalServerError.GetDescription());
        }





        private async void HandleHomePage(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            try
            {
                bool isAuthenticated = await _authService.Authenticate(rq, rp);
                if (!isAuthenticated)
                {
                    rp.AsText("Unauthorized", HttpStatusCode.Unauthorized.GetDescription());
                    return;
                }
                // 2) 服务静态文件
                string filePath = Path.Combine(webDir, rq.Url.LocalPath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    args["file"] = filePath;
                    var ss = Path.HasExtension(args["file"]);
                    rp.AsFile(rq, args["file"]);
                }
                else
                {
                    rp.AsFile(rq, Path.Combine(webDir, "Index.html"));
                }
                // );


            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void HandleUrlParsingDemo(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            try
            {
                var txt = Templating.RenderFile(Path.Combine(webDir, "UrlParsingResponse.thtml"), args);
                rp.AsText(txt);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void HandleViewImagesFiles(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            try
            {
                string filePath = Path.Combine(webDir, rq.Url.LocalPath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    args["file"] = filePath;
                    rp.AsFile(rq, args["file"]);
                }

            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        // 安全的文件上传处理
        private async void HandleFormParsingUpload(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            try
            {
                bool isAuthenticated = await _authService.Authenticate(rq, rp);
                if (!isAuthenticated)
                {
                    rp.AsText("Unauthorized", HttpStatusCode.Unauthorized.GetDescription());
                    return;
                }

                var files = rq.ParseBody(args);
                string serverImagesDir = Path.Combine(webDir, "ERPImages");

                foreach (var f in files.Values)
                {
                    // 验证文件类型和大小
                    if (!IsValidFileType(f.FileName) || f.Value.Length > MaxFileSize)
                    {
                        rp.AsText("Invalid file type or size", HttpStatusCode.BadRequest.GetDescription());
                        return;
                    }

                    // 清理文件名以防止路径遍历攻击
                    string safeFileName = GetSafeFileName(f.FileName);
                    f.Save(Path.Combine(serverImagesDir, safeFileName), true);
                }

                //Upload successful这个是返回给客户端的文本标记上传成功。要对应！！TODO:

                var txtRp = "UploadSuccessful Form fields: " + String.Join(";  ", args.Select(x => $"'{x.Key}: {x.Value}'")) +
                            "Files:       " + String.Join(";  ", files.Select(x => $"'{x.Key}: {x.Value.FileName}, {x.Value.ContentType}'"));

                rp.AsText($"<pre>{txtRp}</pre>");
            }
            catch (Exception ex)
            {
                HandleError(rq, rp, ex);
            }
        }


        //private void HandleFormParsingUpload(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        //{
        //    try
        //    {
        //        var files = rq.ParseBody(args);
        //        string serverImagesDir = Path.Combine(webDir, "ERPImages");

        //        foreach (var f in files.Values)
        //            f.Save(Path.Combine(serverImagesDir, f.FileName), true);

        //        var txtRp = "Form fields: " + String.Join(";  ", args.Select(x => $"'{x.Key}: {x.Value}'")) +
        //                    "Files:       " + String.Join(";  ", files.Select(x => $"'{x.Key}: {x.Value.FileName}, {x.Value.ContentType}'"));

        //        rp.AsText($"<pre>{txtRp}</pre>");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //    }
        //}

        private void HandleApiDemo(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            try
            {
                var txt = Templating.RenderFile(Path.Combine(webDir, "UrlParsingResponse.thtml"), args);
                rp.AsText(GetUsers(rq));
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        // DELETE请求处理示例
        private async void HandleDelete(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            bool isAuthenticated = await _authService.Authenticate(rq, rp);
            if (!isAuthenticated)
            {
                rp.AsText("Unauthorized", HttpStatusCode.Unauthorized.GetDescription());
                return;
            }


            // 获取要删除的资源ID
            // var id = GetResourceId(rq);
            // 执行删除操作
            // DeleteResource(id);
            // rp.AsText("Resource deleted successfully."); // 响应删除成功
        }

        async void HandleLogin(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            AuthenticationService authenticationService = new AuthenticationService();
            bool result = await authenticationService.LoginAsync(rq, rp);
        }

        // 删除图片
        private async void HandleDeleteImages(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            try
            {
                bool isAuthenticated = await _authService.Authenticate(rq, rp);
                if (!isAuthenticated)
                {
                    rp.AsText("Unauthorized", HttpStatusCode.Unauthorized.GetDescription());
                    return;
                }

                var serverDir = _configManager.GetValue("ServerImageDirectory");
                string fileName = rq.Headers["fileName"].ToString();
                string imagePath = Path.Combine(webDir, serverDir, fileName);
                imagePath += ".jpg";//文件后缀
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                    rp.AsText("Image deleted successfully.");
                }
                else
                {
                    rp.AsText("Image not found.", HttpStatusCode.NotFound.GetDescription());
                }
            }
            catch (Exception ex)
            {
                HandleError(rq, rp, ex);
            }
        }

        // 404 Not Found
        private void HandleNotFound(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            rp.AsText("404 Not Found", HttpStatusCode.NotFound.GetDescription());
        }

        private static string GetUsers(HttpListenerRequest arg)
        {
            return JsonConvert.SerializeObject(new[]
            {
                new { Id = 1, Username = "peter" },
                new { Id = 1, Username = "jack" },
                new { Id = 1, Username = "john" },
            });
        }


        // 验证文件类型
        private bool IsValidFileType(string fileName)
        {
            var validExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return validExtensions.Contains(extension);
        }

        // 获取安全的文件名
        private string GetSafeFileName(string fileName)
        {
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            var safeFileName = new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());
            return safeFileName;
        }

        private void LogError(Exception ex)
        {
            _logger.LogError("", ex);
        }
    }
}
