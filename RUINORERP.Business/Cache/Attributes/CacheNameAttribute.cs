using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache.Attributes
{

    /// <summary>
    /// 缓存名称特性
    /// 用于标记业务实体对应的缓存名称，实现实体与缓存的关联
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CacheNameAttribute : Attribute
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 缓存过期时间（可选，不设置则使用默认策略）
        /// </summary>
        public int ExpirationMinutes { get; set; } = -1;

        /// <summary>
        /// 缓存存储类型（可选，不设置则使用默认策略）
        /// </summary>
        public CacheStorageType StorageType { get; set; } = CacheStorageType.Memory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">缓存名称（通常对应表名或业务模块名）</param>
        public CacheNameAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("缓存名称不能为空", nameof(name));

            Name = name;
        }
    }
}


