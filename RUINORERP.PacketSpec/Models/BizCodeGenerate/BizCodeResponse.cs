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
        /// 命令类型
        /// 标识生成的编码类型（单据编号、基础信息编号、产品编码、SKU编码）
        /// </summary>
        public string CommandType { get; set; }


        /// <summary>
        /// 规则配置列表
        /// 用于返回所有规则配置信息
        /// </summary>
        public List<tb_sys_BillNoRule> RuleConfigs { get; set; }
    }
}
