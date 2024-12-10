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
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Processor;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.EnumExt.CRM;

namespace RUINORERP.UI.CRM
{

    [MenuAttrAssemblyInfo("客户线索", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.客户管理)]
    public partial class UCLeadsList : BaseForm.BaseListGeneric<tb_CRM_Leads>
    {
        public UCLeadsList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCLeadsEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_Leads, int>> expLeadsStatus;
            expLeadsStatus = (p) => p.LeadsStatus;
            base.ColNameDataDictionary.TryAdd(expLeadsStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(LeadsStatus)));

            tb_CRMConfig CRMConfig = MainForm.Instance.AppContext.Db.Queryable<tb_CRMConfig>().First();
            if (CRMConfig != null)
            {
                if (!CRMConfig.CS_UseLeadsFunction)
                {
                    MessageBox.Show("请联系管理员开通线索功能.");
                    this.Exit(this);
                }
            }
        }
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Leads).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_Leads>()
                               .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了销售只看到自己的客户,采 
            .ToExpression();    //拥有权控制
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

    }
}
