using RUINORERP.Common.Helper;
using RUINORERP.WF.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.WF.BizOperation.Steps
{
    public partial class DecisionStepUI : UCNodePropEditBase
    {
        public DecisionStepUI()
        {
            InitializeComponent();
            NodePropName = "步骤配置";
        }
        ApprovalStep step = new ApprovalStep();
        private void ApprovalStepUI_Load(object sender, EventArgs e)
        {
            if (SetNodeValue != null && SetNodeValue is BizOperation.Steps.ApprovalStep)
            {
                step = (ApprovalStep)SetNodeValue;
                SetNodeValue = step;
                txtID.Text = step.Id;
                DataBindHelper.BindData4TextBox<WorkFlowContextData>(step, t => t.Id, txtID, BindDataType.Text, false);
                DataBindHelper.BindData4TextBox<WorkFlowContextData>(step, t => t.Name, txtName, BindDataType.Text, false);
              //  DataBindHelper.BindData4TextBox<WFStartRoolNode>(step, t => t.SelectNextStep, txtDesc, BindType4TextBox.Text, false);

            }
        }
    }
}
