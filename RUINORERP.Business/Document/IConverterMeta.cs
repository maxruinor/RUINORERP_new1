using RUINORERP.Business.Document;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// Non-generic meta information for converters
    /// Exposes conversion type and display name without relying on reflection.
    /// </summary>
    public interface IConverterMeta
    {
        /// <summary>
        /// 转换类型（单据生成型 / 动作操作型）
        /// </summary>
        DocumentConversionType ConversionType { get; }
        
        /// <summary>
        /// 转换操作的显示名称
        /// 如果子类重写了 DisplayName,返回子类的值;否则返回基类的智能默认值
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 转换唯一标识符
        /// 用于区分同一源/目标类型下的不同转换逻辑（如：正常转单 vs 退款）
        /// 建议格式：{Action} (例如: "Normal", "Refund", "Offset")
        /// </summary>
        string ConversionIdentifier { get; }
    }
}
