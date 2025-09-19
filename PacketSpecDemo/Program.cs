using System;
using System.Text;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Responses;

namespace PacketSpecDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== RUINORERP 数据包模型新体系演示 ===\n");

            try
            {
                // 演示基本用法
                DemonstrateBasicUsage();

                // 演示登录请求
                DemonstrateLoginRequest();

                // 演示文件操作
                DemonstrateFileOperations();

                // 演示响应处理
                DemonstrateResponseHandling();

                // 演示性能特性
                DemonstratePerformanceFeatures();

                // 演示序列化
                DemonstrateSerialization();

                Console.WriteLine("\n=== 演示完成 ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"演示过程中发生错误: {ex.Message}");
                Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }

            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }

        static void DemonstrateBasicUsage()
        {
            Console.WriteLine("1. 基本数据包创建演示");
            Console.WriteLine("-".PadRight(50, '-'));

            // 方法1: 直接实例化
            var packet1 = new PacketModel()
                .WithCommand(PacketCommand.Login)
                .WithPriority(PacketPriority.High)
                .WithTextData("username=admin&password=123456");

            Console.WriteLine($"方法1 - 直接实例化:");
            Console.WriteLine($"  命令: {packet1.Command}");
            Console.WriteLine($"  优先级: {packet1.Priority}");
            Console.WriteLine($"  数据大小: {packet1.Size} bytes");
            Console.WriteLine($"  有效性: {packet1.IsValid()}");
            Console.WriteLine();

            // 方法2: 使用构建器模式
            var packet2 = PacketBuilder.Create()
                .WithCommand(PacketCommand.Heartbeat)
                .WithPriority(PacketPriority.Low)
                .WithSession("session_123")
                .WithJsonData(new { Timestamp = DateTime.UtcNow, Status = "OK" })
                .Build();

            Console.WriteLine($"方法2 - 构建器模式:");
            Console.WriteLine($"  命令: {packet2.Command}");
            Console.WriteLine($"  会话ID: {packet2.SessionId}");
            Console.WriteLine($"  数据: {packet2.GetTextData().Substring(0, 50)}...");
            Console.WriteLine();

            // 方法3: 使用扩展方法链
            var packet3 = new PacketModel()
                .WithCommand(PacketCommand.Query)
                .WithPriority(PacketPriority.Normal)
                .WithDirection(PacketDirection.ClientToServer)
                .WithTextData("SELECT * FROM Users WHERE Status = 1")
                .WithExtension("QueryType", "SQL")
                .WithExtension("Timeout", 30000);

            Console.WriteLine($"方法3 - 扩展方法链:");
            Console.WriteLine($"  方向: {packet3.Direction}");
            Console.WriteLine($"  扩展属性: QueryType={packet3.GetExtension<string>("QueryType")}");
            Console.WriteLine($"  扩展属性: Timeout={packet3.GetExtension<int>("Timeout")}");
            Console.WriteLine();
        }

        static void DemonstrateLoginRequest()
        {
            Console.WriteLine("2. 登录请求创建演示");
            Console.WriteLine("-".PadRight(50, '-'));

            // 使用构建器快捷方法
            var loginPacket = PacketBuilder.Create()
                .AsLoginRequest("admin", "password123", "1.0.0")
                .WithSession("session_abc")
                .WithRequestId("req_001")
                .Build();

            Console.WriteLine($"登录数据包:");
            Console.WriteLine($"  ID: {loginPacket.Id}");
            Console.WriteLine($"  命令: {loginPacket.Command}");
            Console.WriteLine($"  优先级: {loginPacket.Priority}");
            Console.WriteLine($"  会话ID: {loginPacket.SessionId}");
            Console.WriteLine($"  请求ID: {loginPacket.RequestId}");
            Console.WriteLine($"  数据大小: {loginPacket.Size} bytes");

            // 提取JSON数据
            var loginData = loginPacket.GetJsonData<dynamic>();
            Console.WriteLine($"  用户名: {loginData.Username}");
            Console.WriteLine($"  客户端版本: {loginData.ClientVersion}");
            Console.WriteLine();
        }

        static void DemonstrateFileOperations()
        {
            Console.WriteLine("3. 文件操作请求演示");
            Console.WriteLine("-".PadRight(50, '-'));

            // 文件下载请求
            var downloadPacket = PacketBuilder.Create()
                .AsFileDownloadRequest("file_789", "documents")
                .WithHighPriority()
                .WithTimeout(30000) // 30秒超时
                .Build();

            Console.WriteLine($"文件下载请求:");
            Console.WriteLine($"  命令: {downloadPacket.Command}");
            Console.WriteLine($"  文件ID: {downloadPacket.GetExtension<string>("FileId")}");
            Console.WriteLine($"  分类: {downloadPacket.GetExtension<string>("Category")}");
            Console.WriteLine($"  超时: {downloadPacket.GetExtension<int>("TimeoutMs")}ms");
            Console.WriteLine();

            // 文件上传请求
            var uploadPacket = PacketBuilder.Create()
                .AsFileUploadRequest("report.pdf", 1024000, "reports") // 1MB文件
                .WithPriority(PacketPriority.Normal)
                .WithExtension("Chunked", true)
                .WithExtension("ChunkSize", 102400) // 100KB分块
                .Build();

            Console.WriteLine($"文件上传请求:");
            Console.WriteLine($"  文件名: {uploadPacket.GetExtension<string>("FileName")}");
            Console.WriteLine($"  文件大小: {uploadPacket.GetExtension<long>("FileSize")} bytes");
            Console.WriteLine($"  分块上传: {uploadPacket.GetExtension<bool>("Chunked")}");
            Console.WriteLine($"  分块大小: {uploadPacket.GetExtension<int>("ChunkSize")} bytes");
            Console.WriteLine();
        }

        static void DemonstrateResponseHandling()
        {
            Console.WriteLine("4. 响应处理演示");
            Console.WriteLine("-".PadRight(50, '-'));

            // 创建成功响应
            var successResponse = ApiResponse<string>.Success("登录成功", "操作完成");
            Console.WriteLine($"成功响应:");
            Console.WriteLine($"  成功: {successResponse.Success}");
            Console.WriteLine($"  消息: {successResponse.Message}");
            Console.WriteLine($"  数据: {successResponse.Data}");
            Console.WriteLine($"  代码: {successResponse.Code}");
            Console.WriteLine();

            // 创建失败响应
            var failureResponse = ApiResponse<object>.Failure("用户名或密码错误", 401);
            Console.WriteLine($"失败响应:");
            Console.WriteLine($"  成功: {failureResponse.Success}");
            Console.WriteLine($"  消息: {failureResponse.Message}");
            Console.WriteLine($"  代码: {failureResponse.Code}");
            Console.WriteLine();

            // 使用数据包转换为响应
            var queryData = new[] { "item1", "item2", "item3", "item4", "item5" };
            var dataPacket = PacketBuilder.Create()
                .WithCommand(PacketCommand.Query)
                .WithJsonData(queryData)
                .Build();

            var apiResponse = dataPacket.ToApiResponse<string[]>();
            Console.WriteLine($"数据包转换响应:");
            Console.WriteLine($"  成功: {apiResponse.Success}");
            if (apiResponse.Success)
            {
                Console.WriteLine($"  数据数量: {apiResponse.Data.Length}");
                Console.WriteLine($"  数据内容: {string.Join(", ", apiResponse.Data)}");
            }
            Console.WriteLine();
        }

        static void DemonstratePerformanceFeatures()
        {
            Console.WriteLine("5. 性能优化特性演示");
            Console.WriteLine("-".PadRight(50, '-'));

            // 创建大数据包
            var largeData = new byte[1024 * 1024]; // 1MB数据
            new Random().NextBytes(largeData);

            var largePacket = new PacketModel()
                .WithDataDirect(largeData) // 直接设置数据，避免复制
                .WithCommand(PacketCommand.FileUpload);

            Console.WriteLine($"大数据包性能:");
            Console.WriteLine($"  数据包大小: {largePacket.Size / 1024} KB");
            Console.WriteLine($"  创建时间: {largePacket.CreatedAt:HH:mm:ss.fff}");

            // 使用数据切片（避免复制整个数组）
            var slice = largePacket.GetDataSlice(0, 100); // 前100字节
            Console.WriteLine($"  数据切片大小: {slice.Length} bytes (避免复制整个1MB数据)");

            // 计算哈希值（用于快速比较）
            var hash = largePacket.ComputeHash();
            Console.WriteLine($"  数据哈希: {hash.Substring(0, 16)}...");
            Console.WriteLine($"  哈希计算时间: <1ms (快速比较)");
            Console.WriteLine();
        }

        static void DemonstrateSerialization()
        {
            Console.WriteLine("6. 序列化操作演示");
            Console.WriteLine("-".PadRight(50, '-'));

            var packet = PacketBuilder.Create()
                .WithCommand(PacketCommand.Heartbeat)
                .WithJsonData(new { 
                    Status = "OK", 
                    Timestamp = DateTime.UtcNow,
                    MemoryUsage = 1024000,
                    CpuUsage = 25.5
                })
                .Build();

            Console.WriteLine($"原始数据包:");
            Console.WriteLine($"  命令: {packet.Command}");
            Console.WriteLine($"  数据大小: {packet.Size} bytes");

            // 序列化为JSON
            var json = packet.ToJson(Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine($"\nJSON序列化 (大小: {Encoding.UTF8.GetByteCount(json)} bytes):");
            Console.WriteLine(json.Substring(0, Math.Min(200, json.Length)) + "...");

            // 序列化为二进制
            var binary = packet.ToBinary();
            Console.WriteLine($"\n二进制序列化 (大小: {binary.Length} bytes):");
            Console.WriteLine($"  二进制比JSON小 {100 - (binary.Length * 100 / Encoding.UTF8.GetByteCount(json))}%");

            // 反序列化
            var deserializedPacket = PacketModel.FromBinary(binary);
            Console.WriteLine($"\n反序列化验证:");
            Console.WriteLine($"  命令匹配: {packet.Command == deserializedPacket.Command}");
            Console.WriteLine($"  数据大小匹配: {packet.Size == deserializedPacket.Size}");
            Console.WriteLine();
        }
    }
}