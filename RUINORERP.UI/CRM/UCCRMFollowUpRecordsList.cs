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

    [MenuAttrAssemblyInfo("跟进记录", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.跟进管理)]
    public partial class UCCRMFollowUpRecordsList : BaseForm.BaseListGeneric<tb_CRM_FollowUpRecords>
    {
        public UCCRMFollowUpRecordsList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCRMFollowUpRecordsEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_FollowUpRecords, int?>> exp;
            exp = (p) => p.FollowUpMethod;
            base.ColNameDataDictionary.TryAdd(exp.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(FollowUpMethod)));
            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.FollowUpMethod, typeof(FollowUpMethod));
        }
        //public override void QueryConditionBuilder()
        //{
        //    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_FollowUpRecords).Name + "Processor");
        //    QueryConditionFilter = baseProcessor.GetQueryFilter();
        //}

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_FollowUpRecords>()
                               .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了只看到自己的
            .ToExpression();    //拥有权控制

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        /// <summary>
        /// 如果删除记录时判断是否有上属的计划。如果有则判断是否还有其它记录。如果没有看时间。如果到期了。状态是没有执行。如果没有到期，则是没有开始。
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> Delete()
        {
            bool rs = false;
            tb_CRM_FollowUpRecords record = (tb_CRM_FollowUpRecords)this.bindingSourceList.Current;
            //只能自己删除 或超级用户来删除
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if ((record.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID))
                {
                    //只能删除自己的收款信息。
                    MessageBox.Show("只能删除自己名下的跟踪信息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //rs = Task.FromResult(false);
                    rs = false;
                    return rs;
                }
            }
            else
            {
                if ((record.Employee_ID != MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID))
                {
                    //只能删除自己的收款信息。
                    if (MessageBox.Show("当前数据不属于您名下，您确定要删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        rs = false;
                        return rs;
                    }

                }
            }
            rs = await base.Delete();
            if (rs)
            {
                if (record.tb_crm_followupplans != null)
                {

                }
            }
            return rs;
        }
    }
}
