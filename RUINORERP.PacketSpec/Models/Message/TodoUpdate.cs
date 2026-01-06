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
        /// 实体对象引用
        /// </summary>
        public object entity { get; set; } = new object();

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
        /// 创建BillStatusUpdateData实例的工厂方法
        /// </summary>
        /// <param name="updateType">更新类型</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="billId">单据ID</param>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="businessStatusValue">业务状态值</param>
        /// <returns>创建的BillStatusUpdateData实例</returns>
        public static TodoUpdate Create(
            TodoUpdateType updateType,
            BizType bizType,
            long billId,
            string bilno,
            object entity,
            Type statusType,
            object businessStatusValue
        )
        {
            return new TodoUpdate
            {
                UpdateType = updateType,
                BusinessType = bizType,
                BillId = billId,
                BillNo= bilno,
                entity = entity,
                BizStatusType = statusType,
                BizStatusValue = businessStatusValue,
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
        public static TodoUpdate CreateFromUpdate(
            TodoUpdate update,
            object entity ,
            Type statusType,
            object businessStatusValue
        )
        {
            return new TodoUpdate
            {
                UpdateType = update.UpdateType,
                BusinessType = update.BusinessType,
                BillId = update.BillId,
                BillNo=update.BillNo,
                entity = entity ?? new BaseEntity(),
                BizStatusType = statusType,
                BizStatusValue = businessStatusValue,
                AdditionalData = new Dictionary<string, object>(update.AdditionalData),
                ConditionValues = new Dictionary<string, object>()
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
