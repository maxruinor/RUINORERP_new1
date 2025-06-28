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
using SqlSugar;
using RUINORERP.Common.Helper;
using RUINOR.Core;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.FM.FMBase
{

    //其他费用单
    public partial class UCOtherExpenseQuery : BaseBillQueryMC<tb_FM_OtherExpense, tb_FM_OtherExpenseDetail>
    {
        public UCOtherExpenseQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ExpenseNo);
            //base.ChildRelatedEntityType = typeof(tb_PurOrderDetail);
        }


        /// <summary>
        /// 收付款方式决定是不收入还是支出。收款=收入， 支出=付款
        /// </summary>
        public ReceivePaymentType PaymentType { get; set; }

        public override void BuildLimitQueryConditions()
        {


            //创建表达式
            var lambda = Expressionable.Create<tb_FM_OtherExpense>()
                             .AndIF(PaymentType == ReceivePaymentType.收款, t => t.EXPOrINC == true)
                              .AndIF(PaymentType == ReceivePaymentType.付款, t => t.EXPOrINC == false)
                             .And(t => t.isdeleted == false)
                            // .And(t => t.Is_enabled == true)
                            //报销人员限制，财务不限制
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TaxAmount);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.UntaxedAmount);
            

            base.ChildSummaryCols.Add(c => c.TaxAmount);
            base.ChildSummaryCols.Add(c => c.TotalAmount);
            base.ChildSummaryCols.Add(c => c.UntaxedAmount);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.EXPOrINC);
            //base.ChildInvisibleCols.Add(c => c.Cost);
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }



        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            bool isIncome = true;
            if (PaymentType == ReceivePaymentType.收款)
            {
                isIncome = true;
            }
            else
            {
                isIncome = false;
            }

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_OtherExpense).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            var lambda = Expressionable.Create<tb_FM_OtherExpense>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            .And(t => t.isdeleted == false)
                                    .And(t => t.EXPOrINC == isIncome)
                           // .And(t => t.Is_enabled == true)
                           //报销人员限制，财务不限制
                           //  .AndIF(MainForm.Instance.AppContext.SysConfig.SaleBizLimited && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                           .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);


        }



    }



}
