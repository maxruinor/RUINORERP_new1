using System;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Commands;
using MessagePack;

namespace RUINORERP.Test
{
    /// <summary>
    /// 测试CommandData序列化问题
    /// </summary>
    public class TestCommandDataIssue
    {
        public static void TestSerialization()
        {
            Console.WriteLine("=== 测试CommandData序列化问题 ===");
            
            // 1. 创建一个测试命令
            var testCommand = new TestCommand
            {
                CommandIdentifier = new CommandId(CommandCategory.System, 100),
                Request = new TestRequest { Id = 123, Name = "测试数据" },
                CreatedTimeUtc = DateTime.UtcNow
            };
            
            Console.WriteLine($"原始命令数据: Id={testCommand.Request.Id}, Name={testCommand.Request.Name}");
            
            // 2. 序列化命令到PacketModel
            var packet = PacketModel.Create(testCommand);
            Console.WriteLine($"序列化后PacketModel的CommandData长度: {packet.CommandData?.Length ?? 0}");
            
            // 3. 反序列化PacketModel
            var deserializedPacket = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(
                UnifiedSerializationService.SerializeWithMessagePack(packet));
            Console.WriteLine($"反序列化后PacketModel的CommandData长度: {deserializedPacket.CommandData?.Length ?? 0}");
            
            // 4. 尝试从反序列化的PacketModel提取请求数据
            try
            {
                var extractedRequest = deserializedPacket.ExtractRequest<TestRequest>();
                Console.WriteLine($"提取的请求数据: Id={extractedRequest?.Id}, Name={extractedRequest?.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"提取请求数据失败: {ex.Message}");
            }
            
            // 5. 直接测试CommandData的反序列化
            if (packet.CommandData != null && packet.CommandData.Length > 0)
            {
                try
                {
                    var directDeserialized = UnifiedSerializationService.DeserializeWithMessagePack<TestCommand>(packet.CommandData);
                    Console.WriteLine($"直接反序列化CommandData: Id={directDeserialized?.Request?.Id}, Name={directDeserialized?.Request?.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"直接反序列化CommandData失败: {ex.Message}");
                }
            }
        }
    }
    
    [MessagePackObject]
    public class TestCommand : BaseCommand
    {
        [Key(10)]
        public TestRequest Request { get; set; }
        
        public override object GetSerializableData()
        {
            return Request;
        }
    }
    
    [MessagePackObject]
    public class TestRequest
    {
        [Key(0)]
        public int Id { get; set; }
        
        [Key(1)]
        public string Name { get; set; }
    }
}