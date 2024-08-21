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
    public partial class ListConditionEditorDialog : Form
    {
        private List<WFCondition> _conditions;

        public List<WFCondition> Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }

        public ListConditionEditorDialog(List<WFCondition> conditions)
        {
            InitializeComponent();

            _conditions = conditions;
            UpdateControls();
        }

     
        private void UpdateControls()
        {
            // 根据列表条件的值更新对话框的控件
        }

        private void OnOKButtonClick(object sender, EventArgs e)
        {
            // 保存用户编辑后的列表条件值

            DialogResult = DialogResult.OK;
        }
    }
}
