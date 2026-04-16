using RUINORERP.Model.UI;
using System;
using System.Windows.Forms;

namespace RUINORERP.UI.ToolForm
{
    /// <summary>
    /// 金额输入窗体
    /// 用于输入金额的通用对话框
    /// </summary>
    public partial class frmAmountInput : Krypton.Toolkit.KryptonForm
    {
        /// <summary>
        /// 输入的金额值
        /// </summary>
        public decimal InputAmount { get; private set; }

        /// <summary>
        /// 窗体标题
        /// </summary>
        public string SelectorTitle
        {
            get => this.Text;
            set => this.Text = value;
        }

        /// <summary>
        /// 提示文本
        /// </summary>
        public string PromptText
        {
            get => lblPrompt.Text;
            set => lblPrompt.Text = value;
        }

        /// <summary>
        /// 最小金额限制
        /// </summary>
        public decimal MinAmount { get; set; } = 0.01m;

        /// <summary>
        /// 最大金额限制(0表示无限制)
        /// </summary>
        public decimal MaxAmount { get; set; } = 0;

        /// <summary>
        /// 是否允许超额(超过建议金额)
        /// </summary>
        public bool AllowExcess { get; set; } = true;

        /// <summary>
        /// 建议金额(用于提示用户)
        /// </summary>
        public decimal? SuggestedAmount { get; set; }

        public frmAmountInput()
        {
            InitializeComponent();
            InitializeControls();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitializeControls()
        {
            // 设置默认值
            txtAmount.Text = "0.00";
            txtAmount.SelectAll();
            
            // 如果有建议金额,显示提示
            if (SuggestedAmount.HasValue && SuggestedAmount.Value > 0)
            {
                lblSuggestedAmount.Text = $"建议金额: {SuggestedAmount.Value:F2}";
                lblSuggestedAmount.Visible = true;
            }
            else
            {
                lblSuggestedAmount.Visible = false;
            }
        }

        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (!decimal.TryParse(txtAmount.Text.Trim(), out decimal amount))
            {
                MessageBox.Show("请输入有效的金额", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAmount.Focus();
                txtAmount.SelectAll();
                return;
            }

            InputAmount = amount;

            // 验证最小金额
            if (InputAmount < MinAmount)
            {
                MessageBox.Show($"金额不能小于 {MinAmount:F2}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAmount.Focus();
                txtAmount.SelectAll();
                return;
            }

            // 验证最大金额
            if (MaxAmount > 0 && InputAmount > MaxAmount)
            {
                MessageBox.Show($"金额不能超过 {MaxAmount:F2}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAmount.Focus();
                txtAmount.SelectAll();
                return;
            }

            // 检查是否超额
            if (SuggestedAmount.HasValue && SuggestedAmount.Value > 0 && 
                InputAmount > SuggestedAmount.Value && !AllowExcess)
            {
                var result = MessageBox.Show(
                    $"输入金额 {InputAmount:F2} 超过建议金额 {SuggestedAmount.Value:F2},确定要继续吗?",
                    "超额警告",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    txtAmount.Focus();
                    txtAmount.SelectAll();
                    return;
                }
            }

            // 通过验证,关闭窗体
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 金额文本框按键事件 - 只允许数字、小数点和控制键
        /// </summary>
        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许数字、小数点、退格键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // 只允许一个小数点
            if (e.KeyChar == '.' && ((TextBox)sender).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// 金额文本框获得焦点时全选
        /// </summary>
        private void txtAmount_Enter(object sender, EventArgs e)
        {
            txtAmount.SelectAll();
        }

        /// <summary>
        /// 回车键触发确认
        /// </summary>
        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk.PerformClick();
            }
        }
    }
}
