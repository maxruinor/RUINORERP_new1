using System;
using System.Text;
using MessagePack;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;

namespace TestSerialization
{
    /// <summary>
    /// 核心测试类 - 包含所有关键测试
    /// </summary>
    public static class CoreTests
    {
        /// <summary>
        /// 运行所有核心测试
        /// </summary>
        public static void RunAllTests()
        {
            Console.WriteLine("=== 开始核心测试 ===");
            
            TestSimpleEntitySerialization();
        TestByteArraySerialization();
        TestFixedPacketModelSerialization();
        TestVerifiedPacketModelSerialization();
        TestPacketModelTestSerialization();
        TestPacketModelDirectSerialization();
        TestClearSensitiveDataImpact();
            
            Console.WriteLine("\n=== 核心测试完成 ===");
        }

    /// <summary>
    /// 测试完全修复Key编号的VerifiedPacketModel
    /// </summary>
    public static void TestVerifiedPacketModelSerialization()
    {
        Console.WriteLine("--- 1.6 验证版PacketModel序列化测试 ---");
        
        // 创建测试数据
        var originalData = "验证版PacketModel测试数据12345";
        var verifiedPacket = new VerifiedPacketModel
        {
            CommandId = new CommandId { Name = "TEST", Category = CommandCategory.System, OperationCode = 0x01 },
            Status = PacketStatus.Created,
            CommandData = Encoding.UTF8.GetBytes(originalData),
            PacketId = Guid.NewGuid().ToString(),
            Size = originalData.Length,
            Checksum = "test_checksum",
            IsEncrypted = false,
            IsCompressed = false,
            Direction = PacketDirection.Request,
            Version = "2.0",
            MessageType = MessageType.Request,
            CreatedTimeUtc = DateTime.UtcNow,
            TimestampUtc = DateTime.UtcNow
        };
        
        Console.WriteLine($"原始CommandData: {originalData}");
        Console.WriteLine($"CommandData长度: {verifiedPacket.CommandData.Length} 字节");
        
        try
        {
            // 使用简单序列化器测试
            Console.WriteLine("使用SimpleMessagePackSerializer测试:");
            var serialized = SimpleMessagePackSerializer.Serialize(verifiedPacket);
            Console.WriteLine($"序列化后长度: {serialized.Length} 字节");
            
            var deserialized = SimpleMessagePackSerializer.Deserialize<VerifiedPacketModel>(serialized);
            var deserializedText = Encoding.UTF8.GetString(deserialized.CommandData);
            
            Console.WriteLine($"反序列化后CommandData: {deserializedText}");
            Console.WriteLine($"CommandData长度: {deserialized.CommandData.Length} 字节");
            Console.WriteLine($"数据完整性: {(deserializedText == originalData ? "✅ 通过" : "❌ 失败")}");
            Console.WriteLine($"PacketId完整性: {(deserialized.PacketId == verifiedPacket.PacketId ? "✅ 通过" : "❌ 失败")}");
            Console.WriteLine($"CommandId完整性: {(deserialized.CommandId == verifiedPacket.CommandId ? "✅ 通过" : "❌ 失败")}");
            Console.WriteLine();
            
            // 使用标准MessagePackSerializer对比测试
            Console.WriteLine("使用标准MessagePackSerializer对比:");
            var standardSerialized = MessagePackSerializer.Serialize(verifiedPacket);
            var standardDeserialized = MessagePackSerializer.Deserialize<VerifiedPacketModel>(standardSerialized);
            var standardText = Encoding.UTF8.GetString(standardDeserialized.CommandData);
            
            Console.WriteLine($"标准反序列化后CommandData: {standardText}");
            Console.WriteLine($"数据完整性: {(standardText == originalData ? "✅ 通过" : "❌ 失败")}");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 验证版PacketModel测试失败: {ex.Message}");
            Console.WriteLine($"详细错误: {ex}");
        }
        
        Console.WriteLine();
    }

    /// <summary>
    /// 测试修复Key编号连续性的PacketModel
    /// </summary>
    public static void TestFixedPacketModelSerialization()
    {
        Console.WriteLine("--- 1.5 修复版PacketModel序列化测试 ---");
        
        // 创建测试数据
        var originalData = "修复版PacketModel测试数据12345";
        var fixedPacket = new FixedPacketModel
        {
            CommandId = "TEST001",
            Status = "Active",
            CommandData = Encoding.UTF8.GetBytes(originalData),
            PacketId = Guid.NewGuid().ToString(),
            Size = originalData.Length,
            Checksum = "test_checksum",
            IsEncrypted = false,
            IsCompressed = false,
            Direction = "Request",
            Version = "2.0",
            MessageType = "Request"
        };
        
        Console.WriteLine($"原始CommandData: {originalData}");
        Console.WriteLine($"CommandData长度: {fixedPacket.CommandData.Length} 字节");
        
        try
        {
            // 使用简单序列化器测试
            Console.WriteLine("使用SimpleMessagePackSerializer测试:");
            var serialized = SimpleMessagePackSerializer.Serialize(fixedPacket);
            Console.WriteLine($"序列化后长度: {serialized.Length} 字节");
            
            var deserialized = SimpleMessagePackSerializer.Deserialize<FixedPacketModel>(serialized);
            var deserializedText = Encoding.UTF8.GetString(deserialized.CommandData);
            
            Console.WriteLine($"反序列化后CommandData: {deserializedText}");
            Console.WriteLine($"CommandData长度: {deserialized.CommandData.Length} 字节");
            Console.WriteLine($"数据完整性: {(deserializedText == originalData ? "✅ 通过" : "❌ 失败")}");
            Console.WriteLine();
            
            // 使用标准MessagePackSerializer对比测试
            Console.WriteLine("使用标准MessagePackSerializer对比:");
            var standardSerialized = MessagePackSerializer.Serialize(fixedPacket);
            var standardDeserialized = MessagePackSerializer.Deserialize<FixedPacketModel>(standardSerialized);
            var standardText = Encoding.UTF8.GetString(standardDeserialized.CommandData);
            
            Console.WriteLine($"标准反序列化后CommandData: {standardText}");
            Console.WriteLine($"数据完整性: {(standardText == originalData ? "✅ 通过" : "❌ 失败")}");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 修复版PacketModel测试失败: {ex.Message}");
            Console.WriteLine($"详细错误: {ex}");
        }
        
        Console.WriteLine();
    }

    /// <summary>
    /// 测试使用连续Key编号的PacketModelTest
    /// </summary>
    public static void TestPacketModelTestSerialization()
    {
        Console.WriteLine("--- 1.7 测试版PacketModel序列化测试 ---");
        
        // 创建测试数据
        var originalData = "测试版PacketModel测试数据12345";
        var testPacket = PacketModelTest.CreateTestInstance(originalData);
        
        Console.WriteLine($"原始CommandData: {originalData}");
        Console.WriteLine($"CommandData长度: {testPacket.CommandData.Length} 字节");
        
        try
        {
            // 使用简单序列化器测试
            Console.WriteLine("使用SimpleMessagePackSerializer测试:");
            var serialized = SimpleMessagePackSerializer.Serialize(testPacket);
            Console.WriteLine($"序列化后长度: {serialized.Length} 字节");
            
            var deserialized = SimpleMessagePackSerializer.Deserialize<PacketModelTest>(serialized);
            var deserializedText = Encoding.UTF8.GetString(deserialized.CommandData);
            
            Console.WriteLine($"反序列化后CommandData: {deserializedText}");
            Console.WriteLine($"CommandData长度: {deserialized.CommandData.Length} 字节");
            Console.WriteLine($"数据完整性: {(deserializedText == originalData ? "✅ 通过" : "❌ 失败")}");
            Console.WriteLine($"PacketId完整性: {(deserialized.PacketId == testPacket.PacketId ? "✅ 通过" : "❌ 失败")}");
            Console.WriteLine($"CommandId完整性: {(deserialized.CommandId == testPacket.CommandId ? "✅ 通过" : "❌ 失败")}");
            Console.WriteLine();
            
            // 使用标准MessagePackSerializer对比测试
            Console.WriteLine("使用标准MessagePackSerializer对比:");
            var standardSerialized = MessagePackSerializer.Serialize(testPacket);
            var standardDeserialized = MessagePackSerializer.Deserialize<PacketModelTest>(standardSerialized);
            var standardText = Encoding.UTF8.GetString(standardDeserialized.CommandData);
            
            Console.WriteLine($"标准反序列化后CommandData: {standardText}");
            Console.WriteLine($"数据完整性: {(standardText == originalData ? "✅ 通过" : "❌ 失败")}");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 测试版PacketModel测试失败: {ex.Message}");
            Console.WriteLine($"详细错误: {ex}");
        }
        
        Console.WriteLine();
    }

    /// <summary>
    /// 测试简单实体类的byte[]序列化
    /// </summary>
    public static void TestSimpleEntitySerialization()
    {
        Console.WriteLine("--- 0. 简单实体类byte[]序列化测试 ---");
        
        // 创建测试数据
        var originalData = "简单实体类测试数据12345";
        var testEntity = new SimpleTestEntity
        {
            Id = 1001,
            Name = "测试实体",
            Description = "这是一个测试实体",
            BinaryData = Encoding.UTF8.GetBytes(originalData)
        };
        
        Console.WriteLine($"原始BinaryData: {originalData}");
        Console.WriteLine($"BinaryData长度: {testEntity.BinaryData.Length} 字节");
        
        try
        {
            // 使用简单序列化器测试
            Console.WriteLine("使用SimpleMessagePackSerializer测试:");
            var serialized = SimpleMessagePackSerializer.Serialize(testEntity);
            Console.WriteLine($"序列化后长度: {serialized.Length} 字节");
            
            var deserialized = SimpleMessagePackSerializer.Deserialize<SimpleTestEntity>(serialized);
            var deserializedText = Encoding.UTF8.GetString(deserialized.BinaryData);
            
            Console.WriteLine($"反序列化后BinaryData: {deserializedText}");
            Console.WriteLine($"BinaryData长度: {deserialized.BinaryData.Length} 字节");
            Console.WriteLine($"数据完整性: {(deserializedText == originalData ? "✅ 通过" : "❌ 失败")}");
            Console.WriteLine();
            
            // 使用标准MessagePackSerializer对比测试
            Console.WriteLine("使用标准MessagePackSerializer对比:");
            var standardSerialized = MessagePackSerializer.Serialize(testEntity);
            var standardDeserialized = MessagePackSerializer.Deserialize<SimpleTestEntity>(standardSerialized);
            var standardText = Encoding.UTF8.GetString(standardDeserialized.BinaryData);
            
            Console.WriteLine($"标准反序列化后BinaryData: {standardText}");
            Console.WriteLine($"数据完整性: {(standardText == originalData ? "✅ 通过" : "❌ 失败")}");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 简单实体类测试失败: {ex.Message}");
        }
        
        Console.WriteLine();
    }

        /// <summary>
        /// 测试字节数组序列化 - 验证MessagePack对byte[]的处理
        /// </summary>
        public static void TestByteArraySerialization()
        {
            Console.WriteLine("\n--- 1. 字节数组序列化测试 ---");
            
            try
            {
                // 测试数据
                var testData = Encoding.UTF8.GetBytes("测试数据12345");
                Console.WriteLine($"原始数据: {Encoding.UTF8.GetString(testData)}");
                Console.WriteLine($"数据长度: {testData.Length} 字节");

                // MessagePack序列化
                var serialized = MessagePackSerializer.Serialize(testData);
                var deserialized = MessagePackSerializer.Deserialize<byte[]>(serialized);

                Console.WriteLine($"序列化后长度: {serialized.Length} 字节");
                Console.WriteLine($"反序列化后: {Encoding.UTF8.GetString(deserialized)}");
                Console.WriteLine($"数据完整性: {(BytesEqual(testData, deserialized) ? "✅ 通过" : "❌ 失败")}");

                // 测试null值
                byte[] nullData = null;
                var nullSerialized = MessagePackSerializer.Serialize(nullData);
                var nullDeserialized = MessagePackSerializer.Deserialize<byte[]>(nullSerialized);
                Console.WriteLine($"null值处理: {(nullDeserialized == null ? "✅ 正确" : "❌ 错误")}");

                // 测试空数组
                var emptyData = new byte[0];
                var emptySerialized = MessagePackSerializer.Serialize(emptyData);
                var emptyDeserialized = MessagePackSerializer.Deserialize<byte[]>(emptySerialized);
                Console.WriteLine($"空数组处理: {(emptyDeserialized != null && emptyDeserialized.Length == 0 ? "✅ 正确" : "❌ 错误")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"字节数组测试失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 测试PacketModel直接序列化 - 排除网络影响
        /// </summary>
        public static void TestPacketModelDirectSerialization()
        {
            Console.WriteLine("\n--- 2. PacketModel直接序列化测试 ---");
            
            try
            {
                // 创建测试数据
                var testData = Encoding.UTF8.GetBytes("PacketModel测试数据");
                Console.WriteLine($"测试数据: {Encoding.UTF8.GetString(testData)}");
                Console.WriteLine($"数据长度: {testData.Length} 字节");

                // 创建PacketModel
                var packet = new PacketModel
                {
                    CommandData = testData,
                    CommandId = new CommandId { Name = "TEST", Category = CommandCategory.System, OperationCode = 0x01 },
                    Status = PacketStatus.Created,
                    PacketId = Guid.NewGuid().ToString(),
                    Size = testData.Length
                };

                Console.WriteLine($"创建后PacketModel:");
                Console.WriteLine($"  CommandData长度: {packet.CommandData?.Length ?? 0}");
                Console.WriteLine($"  CommandData内容: {(packet.CommandData != null ? Encoding.UTF8.GetString(packet.CommandData) : "null")}");

                // 使用UnifiedSerializationService序列化
                var serialized = UnifiedSerializationService.SerializeWithMessagePack(packet);
                var deserialized = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(serialized);

                Console.WriteLine($"反序列化后PacketModel:");
                Console.WriteLine($"  CommandData长度: {deserialized.CommandData?.Length ?? 0}");
                Console.WriteLine($"  CommandData内容: {(deserialized.CommandData != null ? Encoding.UTF8.GetString(deserialized.CommandData) : "null")}");

                // 数据完整性验证
                if (deserialized.CommandData == null)
                {
                    Console.WriteLine("❌ CommandData为null - 数据丢失！");
                }
                else if (!BytesEqual(testData, deserialized.CommandData))
                {
                    Console.WriteLine("❌ CommandData内容不匹配 - 数据损坏！");
                }
                else
                {
                    Console.WriteLine("✅ CommandData完整性验证通过");
                }

                // 使用标准MessagePackSerializer对比测试
                Console.WriteLine("\n  --- 使用标准MessagePackSerializer对比测试 ---");
                var mpSerialized = MessagePackSerializer.Serialize(packet);
                var mpDeserialized = MessagePackSerializer.Deserialize<PacketModel>(mpSerialized);
                
                Console.WriteLine($"标准MessagePack反序列化:");
                Console.WriteLine($"  CommandData长度: {mpDeserialized.CommandData?.Length ?? 0}");
                Console.WriteLine($"  CommandData内容: {(mpDeserialized.CommandData != null ? Encoding.UTF8.GetString(mpDeserialized.CommandData) : "null")}");

                if (mpDeserialized.CommandData == null)
                {
                    Console.WriteLine("❌ 标准MessagePackSerializer: CommandData也为null");
                }
                else if (BytesEqual(testData, mpDeserialized.CommandData))
                {
                    Console.WriteLine("✅ 标准MessagePackSerializer: CommandData完整性验证通过");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PacketModel测试失败: {ex.Message}");
                Console.WriteLine($"异常详情: {ex}");
            }
        }

        /// <summary>
        /// 测试ClearSensitiveData对CommandData的影响
        /// </summary>
        public static void TestClearSensitiveDataImpact()
        {
            Console.WriteLine("\n--- 3. ClearSensitiveData影响测试 ---");
            
            try
            {
                var testData = Encoding.UTF8.GetBytes("ClearSensitiveData测试");
                var packet = new PacketModel
                {
                    CommandData = testData,
                    Status = PacketStatus.Created
                };

                Console.WriteLine($"调用前CommandData长度: {packet.CommandData?.Length ?? 0}");
                Console.WriteLine($"调用前CommandData内容: {(packet.CommandData != null ? Encoding.UTF8.GetString(packet.CommandData) : "null")}");

                // 调用ClearSensitiveData
                packet.ClearSensitiveData();

                Console.WriteLine($"调用后CommandData长度: {packet.CommandData?.Length ?? 0}");
                Console.WriteLine($"调用后CommandData内容: {(packet.CommandData != null ? Encoding.UTF8.GetString(packet.CommandData) : "null")}");

                if (packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    Console.WriteLine("✅ ClearSensitiveData未清理CommandData（符合预期，清理逻辑已注释）");
                }
                else
                {
                    Console.WriteLine("❌ ClearSensitiveData清理了CommandData");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClearSensitiveData测试失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 字节数组比较工具方法
        /// </summary>
        private static bool BytesEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
    }
}