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
using RUINORERP.Common.CustomAttribute;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo( "产品类型", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCProductTypeList : BaseListGeneric<tb_ProductType>
    {

        //[CustPropertyAutowiredAttribute]
       // private IDataPortal<RUINORERP.Business.UseCsla.tb_LocationTypeList> portal { get; set; }

        public UCProductTypeList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCProductTypeEdit);
           
        }

    }
}
