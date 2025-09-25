using SuperSocket.ProtoBase;
using RUINORERP.PacketSpec.Protocol;
using SuperSocket.ProtoBase;
using System;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 业务数据包信息 - SuperSocket数据包载体
    /// 
    /// 🔄 数据包处理流程：
    /// 1. SuperSocket接收原始数据
    /// 2. BizPipelineFilter解析包头
    /// 3. 创建BizPackageInfo实例
    /// 4. 填充数据包信息
    /// 5. 传递给业务处理器
    /// 
    /// 📋 核心职责：
    /// - 封装业务数据包信息
    /// - 提供数据包基本属性
    /// - 数据包验证功能
    /// - 原始数据缓存
    /// - 数据包大小计算
    /// 
    /// 🔗 与架构集成：
    /// - 由 BizPipelineFilter 创建和填充
    /// - 传递给 SuperSocketClient 处理
    /// - 包含完整的原始数据用于后续处理
    /// - 提供数据包验证和统计信息
    /// 
    /// 📊 数据包结构：
    /// - Key: 数据包标识
    /// - Flag: 数据包标志
    /// - Body: 业务数据内容
    /// - OriginalData: 完整原始数据
    /// - IsValid: 数据包有效性
    /// - TotalSize: 数据包总大小
    /// </summary>
    public class BizPackageInfo : IPackageInfo<string>
    {
        /// <summary>
        /// 包标识
        /// 用于在SuperSocket框架中标识数据包类型
        /// </summary>
        public string Key { get; set; }
        public PacketModel Packet { get; set; }
    }
}