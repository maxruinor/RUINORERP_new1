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
    [MenuAttrAssemblyInfo("溢余确认单查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用管理, BizType.溢余确认单)]
    [SharedIdRequired]
    public partial class UCProfitConfirmQuery : UCProfitLossQuery, ISharedIdentification
    {
        public UCProfitConfirmQuery()
        {
            InitializeComponent();
            base.profitLossDirect = Global.EnumExt.ProfitLossDirection.Surplus;
        }

        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag1;

    }
}
