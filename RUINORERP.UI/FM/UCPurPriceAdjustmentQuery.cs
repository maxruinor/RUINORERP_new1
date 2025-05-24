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
    [MenuAttrAssemblyInfo("采购价格调整单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购价格调整单)]
    [SharedIdRequired]
    public partial class UCPurPriceAdjustmentQuery : UCPriceAdjustmentQuery, ISharedIdentification
    {
        public UCPurPriceAdjustmentQuery()
        {
            InitializeComponent(); 
            base.PaymentType = Global.EnumExt.ReceivePaymentType.付款;
        }

        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag2;

    }
}
