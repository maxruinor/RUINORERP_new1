using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using TransInstruction;
using System.Threading;
using RUINORERP.Global;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.EOP
{

    [MenuAttrAssemblyInfo("蓄水管理", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.蓄水管理)]
    public partial class UCEOPWaterStorageRegisterList : BaseForm.BaseListGeneric<tb_EOP_WaterStorageRegister>
    {
        public UCEOPWaterStorageRegisterList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCEOPWaterStorageRegisterEdit);
        }

        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_EOP_WaterStorageRegister).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildInvisibleCols()
        {
            base.InvisibleColsExp.Add(c => c.WSR_ID);
        }

        public override void BuildSummaryCols()
        {
            SummaryCols.Add(c => c.TotalAmount);
            SummaryCols.Add(c => c.PlatformFeeAmount);
        }

    }
}
