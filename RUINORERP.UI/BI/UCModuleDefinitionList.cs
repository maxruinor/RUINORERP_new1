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
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("模块定义", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统参数)]
    public partial class UCModuleDefinitionList : BaseForm.BaseListGeneric<tb_ModuleDefinition>
    {
        public UCModuleDefinitionList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCModuleDefinitionEdit);
        }
    }
}
