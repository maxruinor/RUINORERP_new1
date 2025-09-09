using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RUINORERP.Model.Context;
using RUINORERP.Model;
using RUINORERP.SimpleHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.VisualBasic;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINORERP.IIS.Comm;
using System.IO;
using System.Threading;

namespace RUINORERP.IIS
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
        private readonly ImageProcessingService _imageProcessingService;

        public WebServer(ApplicationContext appContext, ConfigManager configManager,
            ILoggerService logger, IAuthenticationService authService, IAuthorizationService authzService, 
            ImageProcessingService imageProcessingService)
        {
            _appContext = appContext;
            _configManager = configManager;
            _logger = logger;
            _authService = authService;
            _authzService = authzService;
            _imageProcessingService = imageProcessingService;
        }

        CancellationTokenSource cts = new CancellationTokenSource();
        public void RunWebServer(string ip, int port)
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
                //  port = 8082;
                Console.WriteLine("Running HTTP server on: " + port);
                _logger.LogInformation($"Running HTTP server on: {ip}:{port}");
                string msg = string.Empty;
                //ip = "192.168.0.254";
                HttpServer httpServer = new HttpServer();
                httpServer.OnShowLog += HttpServer_OnShowLog;
                var ts = httpServer.ListenAsync(ip, port, cts.Token, Route.OnHttpRequestAsync, msg, useHttps: false);
                if (!string.IsNullOrEmpty(msg))
                {
                    _logger.LogInformation($"Running HTTP server on: {ip}:{port} msg: {msg}");
                    frmMain.Instance.PrintInfoLog($"Running HTTP server on: {ip}:{port} msg: {msg}");
                }
                AppExit.WaitFor(cts, ts);
            }
            catch (Exception exx)
            {
                _logger.LogError("错误exx", exx);
            }
        }

        private void HttpServer_OnShowLog(string msg)
        {
            frmMain.Instance.PrintInfoLog($" HttpServer_OnShowLog {msg}");
        }

        private void ConfigureRoutes()
        {
            Route.Before = (rq, rp) => { Console.WriteLine($"Requested: {rq.Url.PathAndQuery}"); return false; };
            Route.Error = HandleError;
            Route.Add("/", HandleHomePage, "GET");
            
            // 专门处理图片访问，增加安全检查
            Route.Add("/ERPImages/{bizType}/{entityId}/{filename}", HandleImageAccess, "GET");
            
            Route.Add("/{action}/{paramA}?{paramB}", HandleUrlParsingDemo, "GET");
            Route.Add("/upload/", HandleFormParsingUpload, "POST");
            Route.Add("/login", HandleLogin, "POST");
            Route.Add("/api/v1/users", HandleApiDemo, "GET");
            Route.Add("/delete", HandleDelete, "DELETE");
            Route.Add("/deleteImages", HandleDeleteImages, "DELETE");
            Route.Add("/favicon.ico", (rq, rp, args) =>
            {
                rp.AsFile(rq, Path.Combine(webDir, "favicon.ico"));
            });
            
            Route.Add("/handleException/", (rq, rp, args) =>
            {
                throw new NotImplementedException("My not implemented exception.");
            });

            // 添加一个中间件来阻止访问上传目录中的非图片文件
            Route.Add((rq, args) =>
            {
                return rq.Url.LocalPath.StartsWith("/ERPImages/") && 
                       !IsValidImageRequest(rq.Url.LocalPath);
            }, (rq, rp, args) =>
            {
                rp.AsText("Forbidden", HttpStatusCode.Forbidden.GetDescription());
            });

            Route.Add("", (rq, rp, args) =>
            {
                rp.AsText("404 Not Found", HttpStatusCode.NotFound.GetDescription());
            });

            Route.Error = (rq, rp, ex) =>
            {
                rp.AsText($"500 Internal Server Error: {ex.Message}", HttpStatusCode.InternalServerError.GetDescription());
            };

            // 服务静态文件，但排除上传目录
            Route.Add((rq, args) =>
            {
                string filePath = Path.Combine(webDir, rq.Url.LocalPath.TrimStart('/'));
                if (File.Exists(filePath) && !rq.Url.LocalPath.StartsWith("/ERPImages/"))
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
            frmMain.Instance.PrintInfoLog($"Error processing request: {ex.Message}");
        }

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
                
                // 获取业务类型和实体ID参数
                string bizType = args.ContainsKey("bizType") ? args["bizType"] : "general";
                string entityId = args.ContainsKey("entityId") ? args["entityId"] : Guid.NewGuid().ToString();
                string fieldName = args.ContainsKey("fieldName") ? args["fieldName"] : "image";
                string imageHash = args.ContainsKey("imageHash") ? args["imageHash"] : "";
                
                // 创建按业务类型和实体ID组织的目录结构
                string serverImagesDir = Path.Combine(webDir, "ERPImages", bizType, entityId);
                
                // 确保目录存在
                if (!Directory.Exists(serverImagesDir))
                {
                    Directory.CreateDirectory(serverImagesDir);
                }

                var uploadedFiles = new List<string>();
                
                foreach (var f in files.Values)
                {
                    // 验证文件类型和大小
                    if (!IsValidFileType(f.FileName) || f.Value.Length > MaxFileSize)
                    {
                        rp.AsText("Invalid file type or size", HttpStatusCode.BadRequest.GetDescription());
                        return;
                    }

                    // 检查图片哈希值是否已存在
                    if (!string.IsNullOrEmpty(imageHash))
                    {
                        string existingFileName = _imageProcessingService.GetExistingImageFileName(serverImagesDir, imageHash);
                        
                        if (!string.IsNullOrEmpty(existingFileName))
                        {
                            // 返回已存在的文件名，避免重复上传
                            var response = new
                            {
                                Status = "ImageExists",
                                File = $"/ERPImages/{bizType}/{entityId}/{existingFileName}"
                            };
                            rp.AsText(JsonConvert.SerializeObject(response), "application/json");
                            return;
                        }
                    }

                    // 生成文件名（使用哈希值前8位或GUID）
                    string fileExtension = Path.GetExtension(f.FileName);
                    string uniqueFileName = !string.IsNullOrEmpty(imageHash) ? 
                        $"{bizType}_{entityId}_{imageHash.Substring(0, 8)}{fileExtension}" : 
                        $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
                    
                    string filePath = Path.Combine(serverImagesDir, uniqueFileName);
                    
                    // 保存文件
                    f.Save(filePath, true);
                    uploadedFiles.Add($"/ERPImages/{bizType}/{entityId}/{uniqueFileName}");
                }

                // 返回上传成功的文件路径列表
                var response = new
                {
                    Status = "UploadSuccessful",
                    Files = uploadedFiles,
                    FormFields = args.Where(x => !files.ContainsKey(x.Key)).ToDictionary(x => x.Key, x => x.Value)
                };

                rp.AsText(JsonConvert.SerializeObject(response), "application/json");
            }
            catch (Exception ex)
            {
                HandleError(rq, rp, ex);
            }
        }

        // 处理图片访问请求
        private async void HandleImageAccess(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            try
            {
                bool isAuthenticated = await _authService.Authenticate(rq, rp);
                if (!isAuthenticated)
                {
                    rp.AsText("Unauthorized", HttpStatusCode.Unauthorized.GetDescription());
                    return;
                }

                string bizType = args["bizType"];
                string entityId = args["entityId"];
                string filename = args["filename"];

                string imagePath = Path.Combine(webDir, "ERPImages", bizType, entityId, filename);
                
                // 验证文件是否存在且为图片
                if (File.Exists(imagePath) && IsValidImageRequest(filename))
                {
                    rp.AsFile(rq, imagePath);
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

        // 验证是否为合法的图片请求
        private bool IsValidImageRequest(string path)
        {
            // 检查文件扩展名是否为允许的图片格式
            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extension = Path.GetExtension(path).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        // 验证文件类型
        private bool IsValidFileType(string fileName)
        {
            var validExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return validExtensions.Contains(extension);
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
                else
                {
                    frmMain.Instance.PrintInfoLog("文件不存在" + filePath);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private async void HandleHomePage(HttpListenerRequest rq, HttpListenerResponse rp, Dictionary<string, string> args)
        {
            try
            {
                bool isAuthenticated = await _authService.Authenticate(rq, rp);
                if (!isAuthenticated)
                {
                    rp.AsText("Unauthorized", HttpStatusCode.Unauthorized.GetDescription());
                    frmMain.Instance.PrintInfoLog("HandleHomePage-Unauthorized");
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
            if (result)
            {
                frmMain.Instance.PrintInfoLog("登陆成功");
            }
            //else
            //{
            //   // frmMain.Instance.PrintInfoLog("登陆失败");
            //}
        }

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

                // 从请求参数获取业务类型、实体ID和文件名
                string bizType = rq.Headers["bizType"]?.ToString() ?? "general";
                string entityId = rq.Headers["entityId"]?.ToString() ?? "";
                string fileName = rq.Headers["fileName"]?.ToString() ?? "";
                
                if (string.IsNullOrEmpty(entityId) || string.IsNullOrEmpty(fileName))
                {
                    rp.AsText("Missing required parameters", HttpStatusCode.BadRequest.GetDescription());
                    return;
                }

                string entityImagePath = Path.Combine(webDir, "ERPImages", bizType, entityId);
                
                // 删除整个实体目录（包含所有尺寸的图片）
                if (Directory.Exists(entityImagePath))
                {
                    Directory.Delete(entityImagePath, true);
                    rp.AsText("Images deleted successfully.");
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

        internal void StopWebServer()
        {
            try
            {
                cts.Cancel();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cts.Dispose();
            }
        }
    }
}