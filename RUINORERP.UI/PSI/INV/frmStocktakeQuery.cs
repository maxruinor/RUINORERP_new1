using ComponentFactory.Krypton.Toolkit;
using RUINORERP.Business;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Model;
using RUINORERP.Model.QueryDto;
using RUINORERP.UI.BaseForm;
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
using static RUINORERP.UI.Common.GUIUtils;

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("盘点单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.库存管理)]
    public partial class frmStocktakeQuery : frmBaseQuery
    {
        public frmStocktakeQuery()
        {
            InitializeComponent();
        }
    }
}
