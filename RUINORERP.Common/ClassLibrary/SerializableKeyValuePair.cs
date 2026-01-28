using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RUINORERP.Common
{
    /// <summary>
    /// 可序列化键值对：支持XML序列化的键值对结构（双泛型版本）
    /// Key和Value都支持泛型，适用于任何键值对场景
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    [XmlRoot("KeyValue")]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        /// <summary>
        /// 键
        /// </summary>
        [XmlAttribute("Key")]
        public TKey Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [XmlAttribute("Value")]
        public TValue Value { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SerializableKeyValuePair() { }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 隐式转换：从KeyValuePair创建
        /// </summary>
        /// <param name="kvp">键值对</param>
        public static implicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> kvp)
        {
            return new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// 隐式转换：转换为KeyValuePair
        /// </summary>
        /// <param name="kvp">可序列化键值对</param>
        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> kvp)
        {
            return new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>格式化字符串</returns>
        public override string ToString()
        {
            return $"{Value} ({Key})";
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <returns>是否为空</returns>
        public bool IsEmpty()
        {
            if (Key == null && Value == null)
            {
                return true;
            }

            // 对于值类型，判断默认值
            if (typeof(TKey).IsValueType && Key.Equals(default(TKey)))
            {
                return true;
            }

            if (typeof(TValue).IsValueType && Value.Equals(default(TValue)))
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 可序列化键值对：Key固定为string的单泛型版本（最常用）
    /// 适用于Key为字符串的场景（如字段名、表名、配置键等）
    /// </summary>
    /// <typeparam name="TValue">值的类型</typeparam>
    [XmlRoot("KeyValue")]
    public class SerializableKeyValuePair<TValue> : SerializableKeyValuePair<string, TValue>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SerializableKeyValuePair() : base() { }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="key">字符串键</param>
        /// <param name="value">值</param>
        public SerializableKeyValuePair(string key, TValue value) : base(key, value) { }

        /// <summary>
        /// 隐式转换：从Tuple创建
        /// </summary>
        /// <param name="tuple">元组（key, value）</param>
        public static implicit operator SerializableKeyValuePair<TValue>((string key, TValue value) tuple)
        {
            return new SerializableKeyValuePair<TValue>(tuple.key, tuple.value);
        }

        /// <summary>
        /// 判断Key是否为空
        /// </summary>
        /// <returns>Key是否为空或空白</returns>
        public bool IsKeyEmpty()
        {
            return string.IsNullOrEmpty(Key) || string.IsNullOrWhiteSpace(Key);
        }

        /// <summary>
        /// 获取Key，如果为空则返回默认值
        /// </summary>
        /// <param name="defaultKey">默认键</param>
        /// <returns>键值</returns>
        public string GetKeyOrDefault(string defaultKey = "")
        {
            return string.IsNullOrEmpty(Key) ? defaultKey : Key;
        }

        /// <summary>
        /// 获取Value，如果为空则返回默认值
        /// </summary>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public TValue GetValueOrDefault(TValue defaultValue = default(TValue))
        {
            if (Value == null || (typeof(TValue).IsValueType && Value.Equals(default(TValue))))
            {
                return defaultValue;
            }
            return Value;
        }
    }

    /// <summary>
    /// 可序列化键值对：string-string版本（向后兼容）
    /// 适用于最常用的键值都是字符串的场景（如字段名-字段名、表名-表名等）
    /// </summary>
    [XmlRoot("KeyValue")]
    public class SerializableKeyValuePair : SerializableKeyValuePair<string>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SerializableKeyValuePair() : base() { }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public SerializableKeyValuePair(string key, string value) : base(key, value) { }

        /// <summary>
        /// 判断Value是否为空
        /// </summary>
        /// <returns>Value是否为空或空白</returns>
        public bool IsValueEmpty()
        {
            return string.IsNullOrEmpty(Value) || string.IsNullOrWhiteSpace(Value);
        }

        /// <summary>
        /// 创建空的SerializableKeyValuePair
        /// </summary>
        public static SerializableKeyValuePair Empty => new SerializableKeyValuePair();
    }
}
