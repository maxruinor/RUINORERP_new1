using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Message
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


        public string BillNo { get; set; }

        /// <summary>
        /// 实体对象引用 - 仅用于服务端内部处理,不传输
        /// ⚠️ 重要: 网络传输前必须设置为null,否则会导致OOM
        /// [JsonIgnore] 避免在网络传输中序列化庞大的实体对象
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public object Entity 
        { 
            get => _entity;
            set 
            { 
                // 强制Entity为null，防止内存泄漏
                if (value != null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ 警告: 尝试设置TodoUpdate.Entity为非null值，已强制设为null。这可能导致内存泄漏!");
                }
                _entity = null; 
            }
        }
        private object _entity = null;

        /// <summary>
        /// 条件匹配值集合
        /// 存储用于条件匹配的字段值
        /// </summary>
        public Dictionary<string, object> ConditionValues { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 业务状态类型
        /// </summary>
        public Type BizStatusType { get; set; }

        /// <summary>
        /// 业务状态值
        /// <summary>
        public object BizStatusValue { get; set; }

        /// <summary>
        /// 创建BillStatusUpdateData实例的工厂方法（已废弃）
        /// ⚠️ 注意: Entity参数会被忽略，始终设为null以避免内存泄漏
        /// </summary>
        [Obsolete("请使用 CreateSafe 方法")]
        public static TodoUpdate Create(
            TodoUpdateType updateType,
            BizType bizType,
            long billId,
            string billNo,
            object entity,  // 此参数会被忽略
            Type statusType,
            object businessStatusValue
        )
        {
            return new TodoUpdate
            {
                UpdateType = updateType,
                BusinessType = bizType,
                BillId = billId,
                BillNo = billNo,
                Entity = null,  // 强制为null，确保网络安全
                BizStatusType = statusType,
                BizStatusValue = businessStatusValue,
                AdditionalData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase),
                ConditionValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            };
        }

        /// <summary>
        /// 安全创建TodoUpdate实例 - 强制Entity为null，避免内存泄漏
        /// 推荐在所有网络传输场景中使用此方法
        /// </summary>
        /// <param name="updateType">更新类型</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="billId">单据ID</param>
        /// <param name="billNo">单据编号</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="businessStatusValue">业务状态值</param>
        /// <returns>创建的TodoUpdate实例（Entity强制为null）</returns>
        public static TodoUpdate CreateSafe(
            TodoUpdateType updateType,
            BizType bizType,
            long billId,
            string billNo,
            Type statusType,
            object businessStatusValue
        )
        {
            return new TodoUpdate
            {
                UpdateType = updateType,
                BusinessType = bizType,
                BillId = billId,
                BillNo = billNo,
                Entity = null,  // 强制为null，确保网络安全
                BizStatusType = statusType,
                BizStatusValue = businessStatusValue,
                AdditionalData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase),
                ConditionValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            };
        }

        /// <summary>
        /// 基于TodoUpdate创建新实例的工厂方法（已废弃）
        /// ⚠️ 注意: Entity参数会被忽略，始终设为null以避免内存泄漏
        /// </summary>
        [Obsolete("请使用 CreateSafeFromUpdate 方法")]
        public static TodoUpdate CreateFromUpdate(
            TodoUpdate update,
            object entity,  // 此参数会被忽略
            Type statusType,
            object businessStatusValue
        )
        {
            return new TodoUpdate
            {
                UpdateType = update.UpdateType,
                BusinessType = update.BusinessType,
                BillId = update.BillId,
                BillNo = update.BillNo,
                Entity = null,  // 强制为null，确保网络安全
                BizStatusType = statusType,
                BizStatusValue = businessStatusValue,
                AdditionalData = update.AdditionalData != null 
                    ? new Dictionary<string, object>(update.AdditionalData, StringComparer.OrdinalIgnoreCase) 
                    : new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase),
                ConditionValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            };
        }

        /// <summary>
        /// 安全地基于TodoUpdate创建新实例 - 强制Entity为null
        /// </summary>
        /// <param name="update">源TodoUpdate实例</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="businessStatusValue">业务状态值</param>
        /// <returns>创建的新TodoUpdate实例（Entity强制为null）</returns>
        public static TodoUpdate CreateSafeFromUpdate(
            TodoUpdate update,
            Type statusType,
            object businessStatusValue
        )
        {
            if (update == null)
                throw new ArgumentNullException(nameof(update));

            return new TodoUpdate
            {
                UpdateType = update.UpdateType,
                BusinessType = update.BusinessType,
                BillId = update.BillId,
                BillNo = update.BillNo,
                Entity = null,  // 强制为null，确保网络安全
                BizStatusType = statusType,
                BizStatusValue = businessStatusValue,
                AdditionalData = update.AdditionalData != null 
                    ? new Dictionary<string, object>(update.AdditionalData, StringComparer.OrdinalIgnoreCase) 
                    : new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase),
                ConditionValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            };
        }


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
