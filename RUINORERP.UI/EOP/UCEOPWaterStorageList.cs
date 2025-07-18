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
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Windows.Documents;
using RUINORERP.Business.Security;
using SqlSugar;

namespace RUINORERP.UI.EOP
{

    [MenuAttrAssemblyInfo("蓄水管理", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.蓄水管理)]
    public partial class UCEOPWaterStorageList : BaseForm.BaseListGeneric<tb_EOP_WaterStorage>
    {
        public UCEOPWaterStorageList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCEOPWaterStorageEdit);
        }

        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_EOP_WaterStorage).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_EOP_WaterStorage>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
              .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
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
