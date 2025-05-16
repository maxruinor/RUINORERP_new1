using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.Common;
using RUINORERP.UI.FM.FMBase;
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

    [MenuAttrAssemblyInfo("其他费用收入查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用管理, BizType.其他费用收入)]
    [SharedIdRequired]
    public partial class UCOtherIncomeVoucherQuery : UCOtherExpenseQuery, ISharedIdentification
    {
        public UCOtherIncomeVoucherQuery()
        {
            InitializeComponent();
        }

        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag1;
    }
}
