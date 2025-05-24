using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
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
    [MenuAttrAssemblyInfo("销售价格调整单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售价格调整单)]
    [SharedIdRequired]
    public partial class UCSalePriceAdjustment : UCPriceAdjustment, ISharedIdentification
    {
        public UCSalePriceAdjustment()
        {
            InitializeComponent();
            base.PaymentType = Global.EnumExt.ReceivePaymentType.收款;
        }
        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag1;
    }
}
