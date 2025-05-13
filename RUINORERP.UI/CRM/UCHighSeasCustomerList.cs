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

namespace RUINORERP.UI.CRM
{
    [MenuAttrAssemblyInfo("公海客户", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.客户管理)]
    [SharedIdRequired]
    public partial class UCHighSeasCustomerList : UCCRMCustomerList, ISharedIdentification
    {
        public UCHighSeasCustomerList()
        {
            InitializeComponent();
        }
        public SharedFlag sharedFlag { get; set; } = SharedFlag.Flag1;
    }
}
