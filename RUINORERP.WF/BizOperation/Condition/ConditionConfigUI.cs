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
using System.Xml.Linq;

namespace RUINORERP.WF.BizOperation.Condition
{
    public partial class ConditionConfigUI : UCNodePropEditBase
    {
        ConditionConfig config = new ConditionConfig();
        public ConditionConfigUI()
        {
            InitializeComponent();
            NodePropName = "条件配置";
        }

        private void ConditionConfigUI_Load(object sender, EventArgs e)
        {
            if (SetNodeValue != null && SetNodeValue is ConditionConfig)
            {
                config = (ConditionConfig)SetNodeValue;
                SetNodeValue = config;
                //DataBindHelper.BindData4Cmb<ConditionConfig>(config, t => t.conditionType, txtName, BindType4TextBox.Text, false);
                //txtID.Text = step.Id;
                //DataBindHelper.BindData4TextBox<WorkFlowConfigData>(step, t => t.Id, txtID, BindType4TextBox.Text, false);
                //DataBindHelper.BindData4TextBox<WorkFlowConfigData>(step, t => t.Name, txtName, BindType4TextBox.Text, false);
                ////  DataBindHelper.BindData4TextBox<WFStartRoolNode>(step, t => t.SelectNextStep, txtDesc, BindType4TextBox.Text, false);

            }
        }

        private void cmbConditionType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
