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

    [MenuAttrAssemblyInfo("提醒规则管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统参数)]
    public partial class UCReminderRuleList : BaseForm.BaseListGeneric<tb_ReminderRule>
    {
        public UCReminderRuleList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCReminderRuleEdit);

        }

        


    }
}
