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
using RUINORERP.Global;

namespace RUINORERP.UI.CRM
{

    [MenuAttrAssemblyInfo("跟进计划", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.跟进管理)]
    public partial class UCCRMFollowUpPlansList : BaseForm.BaseListGeneric<tb_CRM_FollowUpPlans>
    {
        public UCCRMFollowUpPlansList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCRMFollowUpPlansEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_FollowUpPlans, int?>> expPlanStatus;
            expPlanStatus = (p) => p.PlanStatus;
            base.ColNameDataDictionary.TryAdd(expPlanStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(FollowUpPlanStatus)));
        }
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_FollowUpPlans).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_FollowUpPlans>()
                               .AndIF(CurMenuInfo.CaptionCN.Contains("公海客户"), t => t.Employee_ID.Value == 0)
                               .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext) && CurMenuInfo.CaptionCN.Contains("目标客户"),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了销售只看到自己的客户,采 
            .ToExpression();    //拥有权控制

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

    }
}
