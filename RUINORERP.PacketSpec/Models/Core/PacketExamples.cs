using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands.FileTransfer;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 数据包使用示例 - 展示新体系的使用方法
    /// </summary>
    public static class PacketExamples
    {
        /// <summary>
        /// 演示基本数据包创建
        /// </summary>
        public static void DemonstrateBasicUsage()
        {
            Console.WriteLine("=== 基本数据包创建示例 ===");

            // 方法1: 直接实例化
            var packet1 = new PacketModel()
                .WithCommand(AuthenticationCommands.Login)
                .WithTextData("username=admin&password=123456");

            Console.WriteLine($"方法1: {packet1}");

            // 方法2: 使用构建器模式
            var packet2 = PacketBuilder.Create()
                .WithCommand(new CommandId(CommandCategory.System, 0xF0))
                .WithSession("session_123")
                .WithJsonData(new { TimestampUtc = DateTime.UtcNow })
                .Build();

            Console.WriteLine($"方法2: {packet2}");

            // 方法3: 使用扩展方法链
            var packet3 = new PacketModel()
                .WithCommand(new CommandId(CommandCategory.System, 0xF1))
                .WithDirection(PacketDirection.ClientToServer)
                .WithTextData("SELECT * FROM Users")
                .WithExtension("QueryType", "SQL");

            Console.WriteLine($"方法3: {packet3}");
        }

        /// <summary>
        /// 演示登录请求创建
        /// </summary>
        public static void DemonstrateLoginRequest()
        {
            Console.WriteLine("\n=== 登录请求创建示例 ===");

            // 使用构建器快捷方法
            var loginPacket = PacketBuilder.Create()
                .AsLoginRequest("admin", "password123", "1.0.0")
                .WithSession("session_abc")
                .WithRequestId("req_001")
                .Build();

            Console.WriteLine($"登录包: {loginPacket}");
            Console.WriteLine($"命令类型: {loginPacket.Command}");
            Console.WriteLine($"数据大小: {loginPacket.Size} bytes");
      

            // 提取JSON数据
            var loginData = loginPacket.GetJsonData<dynamic>();
            Console.WriteLine($"用户名: {loginData.Username}");
            Console.WriteLine($"客户端版本: {loginData.ClientVersion}");
        }

        /// <summary>
        /// 演示文件操作请求
        /// </summary>
        public static void DemonstrateFileOperations()
        {
            Console.WriteLine("\n=== 文件操作请求示例 ===");

            // 文件下载请求
            var downloadPacket = PacketBuilder.Create()
                .AsFileDownloadRequest("file_789", "documents")
                .WithTimeout(30000) // 30秒超时
                .Build();

            Console.WriteLine($"下载包: {downloadPacket}");

            // 文件上传请求
            var uploadPacket = PacketBuilder.Create()
                .AsFileUploadRequest("report.pdf", 1024000, "reports") // 1MB文件
                .WithExtension("Chunked", true)
                .Build();

            Console.WriteLine($"上传包: {uploadPacket}");
        }

        /// <summary>
        /// 演示响应处理
        /// </summary>
        public static void DemonstrateResponseHandling()
        {
            Console.WriteLine("\n=== 响应处理示例 ===");

            // 创建成功响应
            var successResponse = ResponseBase<string>.CreateSuccess("登录成功", "操作完成");
            Console.WriteLine($"成功响应: {successResponse}");

            // 创建失败响应
            var failureResponse = ResponseBase<object>.Failure("用户名或密码错误");
            Console.WriteLine($"失败响应: {failureResponse}");

            // 使用数据包转换为响应
            var dataPacket = PacketBuilder.Create()
                .WithCommand(new CommandId(CommandCategory.System, 0xF1))
                .WithJsonData(new[] { "item1", "item2", "item3" })
                .Build();

            var apiResponse = dataPacket.ToApiResponse<string[]>();
            Console.WriteLine($"API响应: {apiResponse}");
            if (apiResponse.IsSuccess)
            {
                Console.WriteLine($"响应数据: {string.Join(", ", apiResponse.Data)}");
            }
        }

        /// <summary>
        /// 演示性能优化特性
        /// </summary>
        public static void DemonstratePerformanceFeatures()
        {
            Console.WriteLine("\n=== 性能优化特性示例 ===");

            // 创建大数据包
            var largeData = new byte[1024 * 1024]; // 1MB数据
            new Random().NextBytes(largeData);

            var largePacket = new PacketModel()
                .WithDataDirect(largeData) // 直接设置数据，避免复制
                .WithCommand(FileCommands.FileDownload);

            Console.WriteLine($"大数据包大小: {largePacket.Size / 1024} KB");

            // 使用数据切片（避免复制整个数组）
            var slice = largePacket.GetDataSlice(0, 100); // 前100字节
            Console.WriteLine($"数据切片大小: {slice.Length} bytes");

            // 计算哈希值（用于快速比较）
            var hash = largePacket.ComputeHash();
            Console.WriteLine($"数据哈希: {hash.Substring(0, 16)}...");
        }

        /// <summary>
        /// 演示序列化操作
        /// </summary>
        public static void DemonstrateSerialization()
        {
            Console.WriteLine("\n=== 序列化操作示例 ===");

            var packet = PacketBuilder.Create()
                .WithCommand(new CommandId(CommandCategory.System, 0xF0))
                .WithJsonData(new { Status = "OK", TimestampUtc = DateTime.UtcNow })
                .Build();

            // 序列化为JSON
            var json = packet.ToJson(Formatting.Indented);
            Console.WriteLine("JSON序列化:");
            Console.WriteLine(json.Substring(0, Math.Min(200, json.Length)) + "...");

            // 序列化为二进制
            var binary = packet.ToBinary();
            Console.WriteLine($"二进制大小: {binary.Length} bytes");

          
        }

        /// <summary>
        /// 运行所有示例
        /// </summary>
        public static void RunAllExamples()
        {
            try
            {
                DemonstrateBasicUsage();
                DemonstrateLoginRequest();
                DemonstrateFileOperations();
                DemonstrateResponseHandling();
                DemonstratePerformanceFeatures();
                DemonstrateSerialization();

                Console.WriteLine("\n=== 所有示例执行完成 ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"示例执行错误: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 数据包工厂 - 提供常见数据包的创建方法
    /// </summary>
    public static class PacketFactory
    {
        /// <summary>
        /// 创建登录请求数据包
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientVersion">客户端版本</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>登录请求数据包</returns>
        public static PacketModel CreateLoginRequest(string username, string password, 
            string clientVersion = null, string sessionId = null)
        {
            return PacketBuilder.Create()
                .AsLoginRequest(username, password, clientVersion)
                .WithSession(sessionId)
                .Build();
        }

        /// <summary>
        /// 创建心跳请求数据包
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>心跳请求数据包</returns>
        public static PacketModel CreateHeartbeatRequest(string sessionId, string clientId = null)
        {
            return PacketBuilder.Create()
                .AsHeartbeatRequest(sessionId, clientId)
                .Build();
        }

        /// <summary>
        /// 创建成功响应数据包
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">响应数据</param>
        /// <param name="message">响应消息</param>
        /// <param name="requestId">请求ID</param>
        /// <returns>成功响应数据包</returns>
        public static PacketModel CreateSuccessResponse<T>(T data, string message = "成功", string requestId = null)
        {
            var response = ResponseBase<T>.CreateSuccess(data, message);
            if (!string.IsNullOrEmpty(requestId))
            {
                response.RequestId = requestId;
            }

            return PacketBuilder.Create()
                .WithCommand(SystemCommands.Heartbeat)
                .WithDirection(PacketDirection.ServerToClient)
                .WithJsonData(response)
                .Build();
        }

        /// <summary>
        /// 创建错误响应数据包
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="requestId">请求ID</param>
        /// <returns>错误响应数据包</returns>
        public static PacketModel CreateErrorResponse(string message, int code = 500, string requestId = null)
        {
            var response = ResponseBase<object>.Failure(message);
            if (!string.IsNullOrEmpty(requestId))
            {
                response.RequestId = requestId;
            }

            return PacketBuilder.Create()
                .WithCommand(SystemCommands.ExceptionReport)
                .WithDirection(PacketDirection.ServerToClient)
                .WithJsonData(response)
                .Build();
        }

        /// <summary>
        /// 创建查询请求数据包
        /// </summary>
        /// <typeparam name="T">查询条件类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>查询请求数据包</returns>
        public static PacketModel CreateQueryRequest<T>(T query, string sessionId = null)
        {
            return PacketBuilder.Create()
                .AsQueryRequest(query)
                .WithSession(sessionId)
                .Build();
        }
    }
}
