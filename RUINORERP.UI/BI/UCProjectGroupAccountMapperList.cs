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
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("账号配置依项目组", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCProjectGroupAccountMapperList : BaseForm.BaseListGeneric<tb_ProjectGroupAccountMapper>
    {
        public UCProjectGroupAccountMapperList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCProjectGroupAccountMapperEdit);

            

        }

     



    }
}
