using System;
using System.Threading.Tasks;

namespace RUINORERP.Model.UI
{
    /// <summary>
    /// 金额输入选择器接口
    /// 用于业务层调用UI层的金额输入窗体,避免循环依赖
    /// </summary>
    public interface IAmountInputSelector
    {
        /// <summary>
        /// 输入的金额值
        /// </summary>
        decimal InputAmount { get; }

        /// <summary>
        /// 窗体标题
        /// </summary>
        string SelectorTitle { get; set; }

        /// <summary>
        /// 提示文本
        /// </summary>
        string PromptText { get; set; }

        /// <summary>
        /// 最小金额限制
        /// </summary>
        decimal MinAmount { get; set; }

        /// <summary>
        /// 最大金额限制(0表示无限制)
        /// </summary>
        decimal MaxAmount { get; set; }

        /// <summary>
        /// 是否允许超额(超过建议金额)
        /// </summary>
        bool AllowExcess { get; set; }

        /// <summary>
        /// 建议金额(用于提示用户)
        /// </summary>
        decimal? SuggestedAmount { get; set; }

        /// <summary>
        /// 显示输入窗体
        /// </summary>
        /// <returns>用户是否确认输入</returns>
        bool ShowDialog();
    }

    /// <summary>
    /// 金额输入选择器工厂接口
    /// </summary>
    public interface IAmountInputSelectorFactory
    {
        /// <summary>
        /// 创建金额输入选择器
        /// </summary>
        /// <returns>金额输入选择器实例</returns>
        IAmountInputSelector CreateSelector();
    }
}
