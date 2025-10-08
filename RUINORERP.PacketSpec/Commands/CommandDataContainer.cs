using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using RUINORERP.PacketSpec.Serialization;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 创建智能数据容器
    /// </summary>
    [MessagePackObject]
    public sealed class CommandDataContainer<T> where T : class
    {

        private static readonly ConcurrentDictionary<string, byte[]> _serializationCache = new();

        private T _objectData;
        private byte[] _binaryData;
        private bool _isObjectDirty = true;
        private bool _isBinaryDirty = true;

        [Key(0)]
        public T ObjectData
        {
            get
            {
                if (_objectData == null && _binaryData != null && _isObjectDirty)
                {
                    _objectData = DeserializeFromBinary(_binaryData);
                    _isObjectDirty = false;
                }
                return _objectData;
            }
            set
            {
                _objectData = value;
                _isBinaryDirty = true;
                _isObjectDirty = false;
            }
        }

        [Key(1)]
        public byte[] BinaryData
        {
            get
            {
                if ((_binaryData == null || _isBinaryDirty) && _objectData != null)
                {
                    _binaryData = SerializeToBinary(_objectData);
                    _isBinaryDirty = false;
                }
                return _binaryData;
            }
            set
            {
                _binaryData = value;
                _isObjectDirty = true;
                _isBinaryDirty = false;
            }
        }

        private byte[] SerializeToBinary(T obj)
        {
            if (obj == null) return Array.Empty<byte>();

            // 生成缓存键
            var json = JsonConvert.SerializeObject(obj);
            using var sha256 = SHA256.Create();
            var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(json)));
            var cacheKey = $"{typeof(T).FullName}:{hash}";

            // 使用缓存或创建新条目，使用配置的MessagePack选项
            return _serializationCache.GetOrAdd(cacheKey, _ =>
            {
                return MessagePackSerializer.Serialize(obj, UnifiedSerializationService.MessagePackOptions);
            });
        }

        private T DeserializeFromBinary(byte[] data)
        {
            return MessagePackSerializer.Deserialize<T>(data, UnifiedSerializationService.MessagePackOptions);
        }
    }
}
