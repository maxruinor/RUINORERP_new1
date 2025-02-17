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

    [MenuAttrAssemblyInfo("箱规管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.包装资料)]
    public partial class UCBoxRulesList : BaseForm.BaseListGeneric<tb_BoxRules>
    {
        public UCBoxRulesList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCBoxRulesEdit);

        }

        


    }
}
