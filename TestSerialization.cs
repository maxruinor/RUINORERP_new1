using System;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;
using MessagePack;

namespace TestSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 测试CommandData序列化问题 ===");
            
            try
            {
                // 1. 创建一个测试数据
                var testData = new TestRequest { Id = 123, Name = "测试数据" };
                Console.WriteLine($"原始数据: Id={testData.Id}, Name={testData.Name}");
                
                // 2. 序列化测试数据
                var serializedData = UnifiedSerializationService.SerializeWithMessagePack(testData);
                Console.WriteLine($"序列化后数据长度: {serializedData.Length}");
                
                // 3. 直接反序列化测试
                var deserializedData = UnifiedSerializationService.DeserializeWithMessagePack<TestRequest>(serializedData);
                Console.WriteLine($"直接反序列化: Id={deserializedData.Id}, Name={deserializedData.Name}");
                
                // 4. 测试PacketModel的序列化/反序列化
                var packet = new PacketModel
                {
                    CommandData = serializedData,
                    CommandId = new CommandId(PacketSpec.Commands.CommandCategory.System, 100),
                    PacketId = "test-123"
                };
                
                Console.WriteLine($"PacketModel CommandData长度: {packet.CommandData.Length}");
                
                // 5. 序列化整个PacketModel
                var packetBytes = UnifiedSerializationService.SerializeWithMessagePack(packet);
                Console.WriteLine($"序列化PacketModel长度: {packetBytes.Length}");
                
                // 6. 反序列化PacketModel
                var deserializedPacket = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(packetBytes);
                Console.WriteLine($"反序列化后PacketModel CommandData长度: {deserializedPacket.CommandData?.Length ?? 0}");
                
                // 7. 从反序列化的PacketModel提取数据
                if (deserializedPacket.CommandData != null && deserializedPacket.CommandData.Length > 0)
                {
                    var extractedData = deserializedPacket.ExtractRequest<TestRequest>();
                    Console.WriteLine($"提取的数据: Id={extractedData?.Id}, Name={extractedData?.Name}");
                }
                else
                {
                    Console.WriteLine("CommandData为null或空！");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
                Console.WriteLine($"堆栈: {ex.StackTrace}");
            }
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