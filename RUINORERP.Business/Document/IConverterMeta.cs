using RUINORERP.Business.Document;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// Non-generic meta information for converters
    /// Exposes conversion type and menu text without relying on reflection.
    /// </summary>
    public interface IConverterMeta
    {
        /// <summary>
        /// 转换类型（单据生成型 / 动作操作型）
        /// </summary>
        DocumentConversionType ConversionType { get; }

        /// <summary>
        /// 菜单项显示文本
        /// </summary>
        string MenuItemText { get; }
    }
}
