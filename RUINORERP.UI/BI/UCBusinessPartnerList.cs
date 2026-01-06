using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.Common;
using RUINORERP.UI.FM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.BI
{
    [SharedIdRequired]
    [MenuAttrAssemblyInfo("所有往来单位", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.供销资料)]
    public partial class UCBusinessPartnerList : UCCustomerVendorList, ISharedIdentification
    {
        public UCBusinessPartnerList()
        {
            InitializeComponent();
        }
        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag4;
    }
}
