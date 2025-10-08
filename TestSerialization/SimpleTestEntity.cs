using MessagePack;

namespace TestSerialization
{
    /// <summary>
    /// 简单测试实体类，用于验证byte[]序列化
    /// </summary>
    [MessagePackObject]
    public class SimpleTestEntity
    {
        /// <summary>
        /// ID属性
        /// </summary>
        [Key(0)]
        public int Id { get; set; }
        
        /// <summary>
        /// 名称属性
        /// </summary>
        [Key(1)]
        public string Name { get; set; }
        
        /// <summary>
        /// 二进制数据属性
        /// </summary>
        [Key(2)]
        public byte[] BinaryData { get; set; }
        
        /// <summary>
        /// 描述属性
        /// </summary>
        [Key(3)]
        public string Description { get; set; }
    }
}