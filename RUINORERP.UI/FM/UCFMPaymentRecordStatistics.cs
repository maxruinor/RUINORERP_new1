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

    [MenuAttrAssemblyInfo("付款统计", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.付款管理, BizType.付款统计)]
    public partial class UCFMPaymentRecordStatistics : UCPaymentRecordStatistics, ISharedIdentification
    {
        public UCFMPaymentRecordStatistics()
        {
            InitializeComponent();
            base.PaymentType = Global.EnumExt.ReceivePaymentType.付款;
        }
        //要和对应的查询一样的值,但是收入和支出要不同
        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag2;
    }
}
