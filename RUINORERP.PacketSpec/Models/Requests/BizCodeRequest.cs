using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.Model;

namespace RUINORERP.PacketSpec.Models.Requests
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
        public string BaseInfoType { get; set; }

        /// <summary>
        /// 参数常量 - 用于传递编码生成过程中需要的常量参数
        /// </summary>
        public string ParaConst { get; set; }

        /// <summary>
        /// 业务编码参数 - 扩展参数，可用于传递复杂的编码生成规则或配置
        /// </summary>
        public BizCodeParameter BizCodePara { get; set; }

        /// <summary>
        /// 产品相关参数 - 用于产品编码生成的特定参数
        /// </summary>
        public ProductCodeParameter ProductParameter { get; set; }

        /// <summary>
        /// SKU相关参数 - 用于SKU编码生成的特定参数
        /// </summary>
        public SKUCodeParameter SKUParameter { get; set; }

        /// <summary>
        /// 条码相关参数 - 用于条码生成的特定参数
        /// </summary>
        public BarCodeParameter BarCodeParameter { get; set; }
    }

    /// <summary>
    /// 产品编码参数类
    /// 用于传递产品编码生成所需的特定参数
    /// </summary>
    public class ProductCodeParameter
    {
        /// <summary>
        /// 产品类别ID - 用于根据产品类别生成编码
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 自定义前缀 - 产品编码的自定义前缀部分
        /// </summary>
        public string CustomPrefix { get; set; }

        /// <summary>
        /// 是否包含日期 - 是否在产品编码中包含日期部分
        /// </summary>
        public bool IncludeDate { get; set; } = false;
    }

    /// <summary>
    /// SKU编码参数类
    /// 用于传递SKU编码生成所需的特定参数
    /// </summary>
    public class SKUCodeParameter
    {
        /// <summary>
        /// 产品ID - 关联的产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 产品编码 - 关联的产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 属性组合 - SKU的属性组合信息，如颜色、尺寸等
        /// </summary>
        public string Attributes { get; set; }

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
