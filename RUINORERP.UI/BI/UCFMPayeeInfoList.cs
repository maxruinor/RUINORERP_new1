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
using RUINORERP.Business.Processor;
using RUINORERP.Global.EnumExt;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("收款账号管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCFMPayeeInfoList : BaseForm.BaseListGeneric<tb_FM_PayeeInfo>
    {
        public UCFMPayeeInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCFMPayeeInfoEdit);
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给

            System.Linq.Expressions.Expression<Func<tb_FM_PayeeInfo, int?>> exprCheckMode;
            exprCheckMode = (p) => p.Account_type;
            base.ColNameDataDictionary.TryAdd(exprCheckMode.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(AccountType)));

        }


        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            //清空过滤条件，因为这个基本数据 需要显示出来 
            QueryConditionFilter.FilterLimitExpressions.Clear();
           
        }

    }
}
