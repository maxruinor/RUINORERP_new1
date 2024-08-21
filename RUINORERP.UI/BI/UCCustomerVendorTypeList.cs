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

    [MenuAttrAssemblyInfo( "往来单位类型", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.供销资料)]
    public partial class UCCustomerVendorTypeList : BaseForm.BaseListGeneric<tb_CustomerVendorType>
    {
        public UCCustomerVendorTypeList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCustomerVendorTypeEdit);
           
        }

     





    }
}
