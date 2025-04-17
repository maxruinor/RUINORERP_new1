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

    [MenuAttrAssemblyInfo("币别汇率换算", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCCurrencyExchangeRateList : BaseForm.BaseListGeneric<tb_CurrencyExchangeRate>
    {
        public UCCurrencyExchangeRateList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCurrencyExchangeRateEdit);
        }
    }
}
