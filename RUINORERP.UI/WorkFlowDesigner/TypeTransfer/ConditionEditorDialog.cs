using RUINORERP.UI.WorkFlowDesigner.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.WorkFlowDesigner.TypeTransfer
{
    public partial class ConditionEditorDialog : Form
    {

        private WFCondition _condition;

        public WFCondition Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        public ConditionEditorDialog(WFCondition condition)
        {
            InitializeComponent();

            _condition = condition;
            UpdateControls();
        }

        private void UpdateControls()
        {
            // 根据条件的值更新对话框的控件
            // 例如，将条件的名称和 ID 显示在相应的文本框中
        }

        private void OnOKButtonClick(object sender, EventArgs e)
        {
            // 保存用户编辑后的条件值
            // 例如，将文本框中的值赋给条件的名称和 ID
            Condition.Name = txtName.Text;
            DialogResult = DialogResult.OK;
        }

        private void ConditionEditorDialog_Load(object sender, EventArgs e)
        {
            txtName.Text = Condition.Name;
        }
    }
}
