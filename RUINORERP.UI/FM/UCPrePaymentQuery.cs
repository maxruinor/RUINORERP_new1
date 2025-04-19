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
    [MenuAttrAssemblyInfo("预付款查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.付款管理, BizType.预付款单)]
    [BillBusinessTypeRequired]
    public partial class UCPrePaymentQuery : UCPreReceivedPaymentQuery, IFMBillBusinessType
    {
        public UCPrePaymentQuery()
        {
            InitializeComponent(); 
            base.PaymentType = Global.EnumExt.ReceivePaymentType.付款;
        }
    }
}
