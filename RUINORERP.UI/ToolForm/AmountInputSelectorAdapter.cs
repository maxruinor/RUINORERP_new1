using RUINORERP.Model.UI;
using System;

namespace RUINORERP.UI.ToolForm
{
    /// <summary>
    /// 金额输入选择器适配器
    /// 将 frmAmountInput 适配为 IAmountInputSelector 接口
    /// </summary>
    public class AmountInputSelectorAdapter : IAmountInputSelector
    {
        private readonly frmAmountInput _selector;

        public AmountInputSelectorAdapter()
        {
            _selector = new frmAmountInput();
        }

        /// <summary>
        /// 输入的金额值
        /// </summary>
        public decimal InputAmount
        {
            get => _selector.InputAmount;
        }

        /// <summary>
        /// 窗体标题
        /// </summary>
        public string SelectorTitle
        {
            get => _selector.SelectorTitle;
            set => _selector.SelectorTitle = value;
        }

        /// <summary>
        /// 提示文本
        /// </summary>
        public string PromptText
        {
            get => _selector.PromptText;
            set => _selector.PromptText = value;
        }

        /// <summary>
        /// 最小金额限制
        /// </summary>
        public decimal MinAmount
        {
            get => _selector.MinAmount;
            set => _selector.MinAmount = value;
        }

        /// <summary>
        /// 最大金额限制(0表示无限制)
        /// </summary>
        public decimal MaxAmount
        {
            get => _selector.MaxAmount;
            set => _selector.MaxAmount = value;
        }

        /// <summary>
        /// 是否允许超额(超过建议金额)
        /// </summary>
        public bool AllowExcess
        {
            get => _selector.AllowExcess;
            set => _selector.AllowExcess = value;
        }

        /// <summary>
        /// 建议金额(用于提示用户)
        /// </summary>
        public decimal? SuggestedAmount
        {
            get => _selector.SuggestedAmount;
            set => _selector.SuggestedAmount = value;
        }

        /// <summary>
        /// 显示输入窗体
        /// </summary>
        /// <returns>用户是否确认输入</returns>
        public bool ShowDialog()
        {
            return _selector.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }
    }

    /// <summary>
    /// 金额输入选择器工厂
    /// </summary>
    public class AmountInputSelectorFactory : IAmountInputSelectorFactory
    {
        /// <summary>
        /// 创建金额输入选择器
        /// </summary>
        public IAmountInputSelector CreateSelector()
        {
            return new AmountInputSelectorAdapter();
        }
    }
}
