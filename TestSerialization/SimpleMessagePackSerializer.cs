using System;
using MessagePack;
using MessagePack.Resolvers;

namespace TestSerialization
{
    /// <summary>
    /// 简化版MessagePack序列化工具类
    /// 用于测试和验证序列化配置问题
    /// </summary>
    public static class SimpleMessagePackSerializer
    {
        private static readonly MessagePackSerializerOptions _options;
        
        /// <summary>
        /// 获取序列化选项
        /// </summary>
        public static MessagePackSerializerOptions Options => _options;
        
        /// <summary>
        /// 静态构造函数，初始化配置
        /// </summary>
        static SimpleMessagePackSerializer()
        {
            // 使用标准解析器，支持[Key]特性
            var resolver = CompositeResolver.Create(
                StandardResolver.Instance,
                ContractlessStandardResolver.Instance
            );
            
            _options = MessagePackSerializerOptions.Standard
                .WithResolver(resolver)
                .WithCompression(MessagePackCompression.Lz4Block);
        }
        
        /// <summary>
        /// 序列化对象为字节数组
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] Serialize<T>(T obj)
        {
            try
            {
                return MessagePackSerializer.Serialize(obj, _options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"序列化失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 反序列化字节数组为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default(T);
                
            try
            {
                return MessagePackSerializer.Deserialize<T>(data, _options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"反序列化失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 尝试反序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">字节数组</param>
        /// <param name="result">结果对象</param>
        /// <returns>是否成功</returns>
        public static bool TryDeserialize<T>(byte[] data, out T result)
        {
            try
            {
                result = Deserialize<T>(data);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }
    }
}