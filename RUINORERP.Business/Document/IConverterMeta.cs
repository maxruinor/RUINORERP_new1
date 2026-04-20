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
    }
}
