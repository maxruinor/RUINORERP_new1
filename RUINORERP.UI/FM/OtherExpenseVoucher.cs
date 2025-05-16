using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.Common;
using RUINORERP.UI.FM.FMBase;
using RUINORERP.UI.PSI.SAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("其他费用支出", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用管理, BizType.其他费用支出)]
    [SharedIdRequired]
    public partial class OtherExpenseVoucher : UCOtherExpense, ISharedIdentification
    {
 
        public OtherExpenseVoucher()
        {
            InitializeComponent();
        }
        //要和对应的查询一样的值,但是收入和支出要不同
        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag2;
    }
}
