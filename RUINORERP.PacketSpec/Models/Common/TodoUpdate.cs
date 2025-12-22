using RUINORERP.Global;
using RUINORERP.Model;
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

    /// <summary>
    /// 单据状态更新数据
    /// 扩展TodoUpdate，支持更丰富的状态信息和条件匹配
    /// </summary>
    public class BillStatusUpdateData : TodoUpdate
    {
        /// <summary>
        /// 实体对象引用
        /// </summary>
        public BaseEntity entity { get; set; } = new BaseEntity();

        /// <summary>
        /// 条件匹配值集合
        /// 存储用于条件匹配的字段值
        /// </summary>
        public Dictionary<string, object> ConditionValues { get; set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// 状态类型（如"string", "enum", "int"等）
        /// </summary>
        public string StatusType { get; set; }
        
        /// <summary>
        /// 业务状态值
        /// <summary>
        public object BusinessStatusValue { get; set; }

        /// <summary>
        /// 创建BillStatusUpdateData实例的工厂方法
        /// </summary>
        /// <param name="updateType">更新类型</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="billId">单据ID</param>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="businessStatusValue">业务状态值</param>
        /// <returns>创建的BillStatusUpdateData实例</returns>
        public static BillStatusUpdateData Create(
            TodoUpdateType updateType,
            BizType bizType,
            long billId,
            BaseEntity entity,
            string statusType = null,
            object businessStatusValue = null
        )
        {
            return new BillStatusUpdateData
            {
                UpdateType = updateType,
                BusinessType = bizType,
                BillId = billId,
                entity = entity,
                StatusType = statusType,
                BusinessStatusValue = businessStatusValue,
                Timestamp = DateTime.Now,
                AdditionalData = new Dictionary<string, object>(),
                ConditionValues = new Dictionary<string, object>()
            };
        }
        
        /// <summary>
        /// 基于TodoUpdate创建BillStatusUpdateData实例的工厂方法
        /// </summary>
        /// <param name="update">TodoUpdate实例</param>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="businessStatusValue">业务状态值</param>
        /// <returns>创建的BillStatusUpdateData实例</returns>
        public static BillStatusUpdateData CreateFromUpdate(
            TodoUpdate update,
            BaseEntity entity = null,
            string statusType = null,
            object businessStatusValue = null
        )
        {
            return new BillStatusUpdateData
            {
                UpdateType = update.UpdateType,
                BusinessType = update.BusinessType,
                BillId = update.BillId,
                entity = entity ?? new BaseEntity(),
                StatusType = statusType,
                BusinessStatusValue = businessStatusValue,
                OldStatus = update.OldStatus,
                NewStatus = update.NewStatus,
                Timestamp = update.Timestamp,
                AdditionalData = new Dictionary<string, object>(update.AdditionalData),
                ConditionValues = new Dictionary<string, object>()
            };
        }

        
    }
}
