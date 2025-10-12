using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 客户端缓存变更事件参数
    /// </summary>
    public class ClientCacheChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public CacheOperation Operation { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        public Type ValueType { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 是否同步到服务器
        /// </summary>
        public bool SyncToServer { get; set; }
    }
}
