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
using TransInstruction;
using System.Threading;
using RUINORERP.Global;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("开票资料管理", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.发票管理)]
    public partial class UCBillingInformationList : BaseForm.BaseListGeneric<tb_BillingInformation>
    {
        public UCBillingInformationList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCBillingInformationEdit);
        }

    }
}
