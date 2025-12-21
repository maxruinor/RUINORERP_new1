using RUINORERP.Global;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Common
{
    /// <summary>
    /// 任务状态更新信息
    /// </summary>
    public class TodoUpdate
    {
        /// <summary>
        /// 更新类型
        /// </summary>
        public TodoUpdateType UpdateType { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BizType BusinessType { get; set; }

        /// <summary>
        /// 单据主键ID（雪花ID）
        /// 用于存储单据主键
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 原状态
        /// </summary>
        public string OldStatus { get; set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public string NewStatus { get; set; }

        /// <summary>
        /// 更新时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 发起操作的用户ID（用于网络传输）
        /// </summary>
        public string InitiatorUserId 
        { 
            get => AdditionalData.TryGetValue("InitiatorUserId", out var value) ? value.ToString() : null;
            set => AdditionalData["InitiatorUserId"] = value;
        }

        /// <summary>
        /// 操作描述（用于网络传输）
        /// </summary>
        public string OperationDescription 
        { 
            get => AdditionalData.TryGetValue("OperationDescription", out var value) ? value.ToString() : null;
            set => AdditionalData["OperationDescription"] = value;
        }

        /// <summary>
        /// 是否来自服务器的更新
        /// </summary>
        public bool IsFromServer 
        { 
            get => AdditionalData.TryGetValue("IsFromServer", out var value) && Convert.ToBoolean(value);
            set => AdditionalData["IsFromServer"] = value;
        }
    }
}
