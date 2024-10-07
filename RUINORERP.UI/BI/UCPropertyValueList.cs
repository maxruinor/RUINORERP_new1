using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;


namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("属性值管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCPropertyValueList : BaseForm.BaseListGeneric<tb_ProdPropertyValue>
    {
        public UCPropertyValueList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCPropertyValueEdit);
            Krypton.Toolkit.KryptonButton button检查数据 = new Krypton.Toolkit.KryptonButton();
            button检查数据.Text = "提取重复数据";
            button检查数据.ToolTipValues.Description = "提取重复数据，有一行会保留，没有显示出来。";
            button检查数据.ToolTipValues.EnableToolTips = true;
            button检查数据.ToolTipValues.Heading = "提示";
            button检查数据.Click += button检查数据_Click;
            base.frm.flowLayoutPanelButtonsArea.Controls.Add(button检查数据);
        }

        private void button检查数据_Click(object sender, EventArgs e)
        {
            ListDataSoure.DataSource = GetDuplicatesList();
            dataGridView1.DataSource = ListDataSoure;
        }

    }
}
