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
using RUINORERP.Global;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("数据权限规则", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.权限管理)]
    public partial class UCRowAuthPolicyList : BaseForm.BaseListGeneric<tb_RowAuthPolicy>
    {
        public UCRowAuthPolicyList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCRowAuthPolicyEdit);
        }
    }
}