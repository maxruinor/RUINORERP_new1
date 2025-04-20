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

            ///下面的代码通过GridViewDisplayHelper这个类已经优化成 自动引用实体中的 特殊 关联 统一处理好了。下面代码可能不会执行

            #region 数据源来源单位和目标单位的字段不等于原始单位表中的主键字段，在显示时通过主键无法找到对应显示的名称

            base.SetForeignkeyPointsList<tb_Currency, tb_CurrencyExchangeRate>(s => s.Currency_ID, t => t.BaseCurrencyID);
            base.SetForeignkeyPointsList<tb_Currency, tb_CurrencyExchangeRate>(s => s.Currency_ID, t => t.TargetCurrencyID);

            ColDisplayTypes.Add(typeof(tb_Currency));

            #endregion
        }
    }
}
