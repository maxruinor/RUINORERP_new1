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
    [MenuAttrAssemblyInfo("采购对账", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.对账管理, BizType.付款对账单)]
    [SharedIdRequired]
    public partial class UCStatementCreatorByPurchaser : UCStatementCreator, ISharedIdentification
    {
        public UCStatementCreatorByPurchaser()
        {
            InitializeComponent(); 
            base.PaymentType = Global.EnumExt.ReceivePaymentType.付款;
        }
        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag2;
    }
}
