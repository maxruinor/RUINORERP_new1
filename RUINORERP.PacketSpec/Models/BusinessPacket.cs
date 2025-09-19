using RUINORERP.PacketSpec.Models.Core;

using System;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 业务数据包类
    /// 包含业务数据和统一数据包的包装
    /// </summary>
    public class BusinessPacket : BaseModel
    {
        /// <summary>
        /// 统一数据包
        /// </summary>
        public PacketModel UnifiedPacket { get; set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        public object BusinessData { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 验证包有效性
        /// </summary>
        public override bool IsValid()
        {
            return base.IsValid() &&
                   UnifiedPacket != null &&
                   UnifiedPacket.IsValid();
        }

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public override void ClearSensitiveData()
        {
            base.ClearSensitiveData();

            if (UnifiedPacket != null)
            {
                UnifiedPacket.ClearSensitiveData();
            }

            BusinessData = null;
        }

        /// <summary>
        /// 从统一数据包创建业务数据包
        /// </summary>
        /// <param name="unifiedPacket">统一数据包</param>
        /// <returns>业务数据包</returns>
        public static BusinessPacket FromUnifiedPacket(PacketModel unifiedPacket)
        {
            if (unifiedPacket == null)
                return null;

            return new BusinessPacket
            {
                UnifiedPacket = unifiedPacket,
                BusinessType = Convert.ToString(unifiedPacket.Command.FullCode), // 使用Convert.ToString确保正确转换
                CreatedTime = unifiedPacket.CreatedTime,
                LastUpdatedTime = unifiedPacket.LastUpdatedTime,
                Flag = unifiedPacket.Flag,
                Body = unifiedPacket.Body,
                SessionId = unifiedPacket.SessionId
            };
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        public override string ToString()
        {
            return $"BusinessPacket[Type:{BusinessType}, UnifiedPacket:{UnifiedPacket?.ToString() ?? "null"}]";
        }
    }
}
