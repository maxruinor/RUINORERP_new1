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

    [MenuAttrAssemblyInfo("系统参数设置", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统参数)]
    public partial class UCSystemConfigList : BaseForm.BaseListGeneric<tb_SystemConfig>
    {
        public UCSystemConfigList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCSystemConfigEdit);
            //如果有一行数据 就无法增加，
            if (base.bindingSourceList.Count>0)
            {

            }
        }
    }
}
