using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.Model;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.BizCodeGenerate
{
    /// <summary>
    /// 业务编码生成响应类
    /// 用于返回业务编码生成的结果
    /// </summary>
    public class BizCodeResponse : ResponseBase
    {
        /// <summary>
        /// 生成的编码
        /// 包含业务单据编号、基础信息编号、产品编码或SKU编码
        /// </summary>
        public string GeneratedCode { get; set; }

        /// <summary>
        /// 是否生成成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误消息
        /// 仅在生成失败时包含错误详情
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 命令类型
        /// 标识生成的编码类型（单据编号、基础信息编号、产品编码、SKU编码）
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// 编码生成时间戳
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 规则配置列表
        /// 用于返回所有规则配置信息
        /// </summary>
        public List<tb_sys_BillNoRule> RuleConfigs { get; set; }
    }
}
