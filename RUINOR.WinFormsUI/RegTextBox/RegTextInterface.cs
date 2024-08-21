 using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace RUINOR.WinFormsUI.RegTextBox
{
    /// <summary>
    /// 为自定义验证事件提供参数
    /// </summary>
    [Description("为自定义验证事件提供参数"), Browsable(false)]
    public class CustomerEventArgs : EventArgs
    {
        /// <summary>
        /// 是否通过验证
        /// </summary>
        [Description("是否通过验证"), DefaultValue(true)]
        public bool Validated { get; set; }

        /// <summary>
        /// 获取或设置被验证的值
        /// </summary>
        [Description("获取或设置被验证的值")]
        public string Value { get; set; }

        /// <summary>
        /// 获取或设置错误信息
        /// </summary>
        [Description("获取或设置错误信息")]
        public string ErrorMessage { get; set; }
    }

    public delegate void CustomerValidatedHandler(object sender, CustomerEventArgs e);

    public interface ITzhtecControl
    {
        ButtonBase Button { get; set; }

        string RegexExpression { get; set; }

        bool AllowEmpty { get; set; }

        bool RemoveSpace { get; set; }

        string EmptyMessage { get; set; }

        string ErrorMessage { get; set; }

        void SelectAll();

        event CustomerValidatedHandler CustomerValidated;
    }
}