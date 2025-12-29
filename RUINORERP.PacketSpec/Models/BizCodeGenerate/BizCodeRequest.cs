using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.Model;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.Model.ProductAttribute;

namespace RUINORERP.PacketSpec.Models.BizCodeGenerate
{
    /// <summary>
    /// 业务编码生成请求类
    /// 用于处理各类业务编码生成的请求参数
    /// </summary>
    public class BizCodeRequest : RequestBase
    {
        /// <summary>
        /// 业务类型 - 用于生成业务单据编号时指定具体的业务类型
        /// </summary>
        public BizType BizType { get; set; }

        /// <summary>
        /// 基础信息类型 - 用于生成基础信息编号时指定具体的数据表或信息类型
        /// </summary>
        public BaseInfoType BaseInfoType { get; set; }

        /// <summary>
        /// 参数常量 - 用于传递编码生成过程中需要的常量参数
        /// </summary>
        public string ParaConst { get; set; }

        /// <summary>
        /// 产品SKU相关参数 - 用于SKU编码生成的特定参数
        /// </summary>
        public ProdParameter ProductParameter { get; set; }

        /// <summary>
        /// 条码相关参数 - 用于条码生成的特定参数
        /// </summary>
        public BarCodeParameter BarCodeParameter { get; set; }

        /// <summary>
        /// 规则配置 - 用于规则配置的保存和更新
        /// </summary>
        public tb_sys_BillNoRule RuleConfig { get; set; }

        /// <summary>
        /// 规则配置ID - 用于规则配置的删除操作
        /// </summary>
        public long RuleConfigId { get; set; }

        /// <summary>
        /// 规则配置列表 - 用于获取所有规则配置
        /// </summary>
        public List<tb_sys_BillNoRule> RuleConfigs { get; set; }
    }


    /// <summary>
    /// SKU编码参数类
    /// 用于传递SKU编码生成所需的特定参数
    /// 注意：此类已被AttributeCombination替代，建议使用AttributeCombination类
    /// </summary>
    public class ProdParameter
    {

        public tb_Prod prod { get; set; }

        public tb_ProdDetail prodDetail { get; set; }

        /// <summary>
        /// 是否包含日期 - 是否在产品编码中包含日期部分
        /// </summary>
        public bool IncludeDate { get; set; } = false;

        /// <summary>
        /// 序号长度 - SKU序号的固定长度
        /// </summary>
        public int SeqLength { get; set; } = 3;
    }

 

    /// <summary>
    /// 条码参数类
    /// 用于传递条码生成所需的特定参数
    /// </summary>
    public class BarCodeParameter
    {
        /// <summary>
        /// 原始编码 - 需要生成条码的基础编码
        /// </summary>
        public string OriginalCode { get; set; }

        /// <summary>
        /// 补位码 - 当编码长度不足时用于补足的字符
        /// </summary>
        public char PaddingChar { get; set; } = '0';
    }
}
