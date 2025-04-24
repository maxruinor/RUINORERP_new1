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
    [MenuAttrAssemblyInfo("公司收付款账号", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCFMAccountList : BaseForm.BaseListGeneric<tb_FM_Account>
    {
        public UCFMAccountList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCFMAccountEdit);

            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
           
            System.Linq.Expressions.Expression<Func<tb_FM_Account, int?>> exprCheckMode;
            exprCheckMode = (p) => p.Account_type;
            base.ColNameDataDictionary.TryAdd(exprCheckMode.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(AccountType)));
            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.Account_type, typeof(AccountType));


        }

     



    }
}
