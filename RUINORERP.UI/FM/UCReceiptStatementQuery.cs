using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.Common;
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
    [MenuAttrAssemblyInfo("收款对账单查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.对账管理, BizType.收款对账单)]
    [SharedIdRequired]
    public partial class UCReceiptStatementQuery : UCFMStatementQuery, ISharedIdentification
    {
        public UCReceiptStatementQuery()
        {
            InitializeComponent(); 
            base.PaymentType = Global.EnumExt.ReceivePaymentType.收款;
        }

        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag1;

    }
}
