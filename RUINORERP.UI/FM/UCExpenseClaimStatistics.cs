using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Business;
using RUINORERP.AutoMapper;
using AutoMapper;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.PSI.SAL
{
    [MenuAttrAssemblyInfo("费用报销统计", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.费用报销单)]
    public partial class UCExpenseClaimStatistics : BaseNavigatorGeneric<View_FM_ExpenseClaimItems, View_FM_ExpenseClaimItems>
    {
        public UCExpenseClaimStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 是不是也一个组，主子表呢？
            ReladtedEntityType = typeof(View_FM_ExpenseClaimItems);
            base.WithOutlook = true;

        }

        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            System.Linq.Expressions.Expression<Func<tb_FM_ExpenseClaim, int?>> exprApprovalStatus;
            exprApprovalStatus = (p) => p.ApprovalStatus;
            base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus)));
            
            System.Linq.Expressions.Expression<Func<tb_FM_ExpenseClaim, int?>> exprDataStatus;
            exprDataStatus = (p) => p.DataStatus;
            base.MasterColNameDataDictionary.TryAdd(exprDataStatus.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));

        }

        private void UCSaleOrderStatistics_Load(object sender, EventArgs e)
        {
            //这个应该是一个组 多个表
            // base._UCMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();

            //不能在上面的构造函数里面初始化
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_FM_ExpenseClaimItems, tb_FM_ExpenseClaim>(c => c.ClaimNo, r => r.ClaimNo);

            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProjectGroup));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_Subject));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Department));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_Account));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Employee));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Currency));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_ExpenseType));
            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_FM_ExpenseClaimItems, tb_FM_ExpenseClaim>(c => c.ClaimNo, r => r.ClaimNo);
        }
  

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_FM_ExpenseClaimItems>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            // .And(t => t.isdeleted == false)

                            //.And(t => t.Is_enabled == true)

                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_FM_ExpenseClaimItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalAmount);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }


    }
}
