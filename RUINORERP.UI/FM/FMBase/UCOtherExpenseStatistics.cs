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
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.FM.FMBase
{
   // [MenuAttrAssemblyInfo("其他费用统计", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用管理, BizType.其他费用统计)]
    public partial class UCOtherExpenseStatistics : BaseNavigatorGeneric<View_FM_OtherExpenseItems, View_FM_OtherExpenseItems>
    {
        public UCOtherExpenseStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 是不是也一个组，主子表呢？
            ReladtedEntityType = typeof(View_FM_OtherExpenseItems);
            base.WithOutlook = true;

        }

        /// <summary>
        /// 收付款方式决定是不收入还是支出。收款=收入， 支出=付款
        /// </summary>
        public ReceivePaymentType PaymentType { get; set; }

        private void UCSaleOrderStatistics_Load(object sender, EventArgs e)
        {
            //这个应该是一个组 多个表
            // base._UCMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();

            //不能在上面的构造函数里面初始化
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_FM_OtherExpenseItems, tb_FM_OtherExpense>(c => c.ExpenseNo, r => r.ExpenseNo);

            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProjectGroup));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_Subject));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Department));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_Account));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Employee));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_OtherExpense));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Currency));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_FM_ExpenseType));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_CustomerVendor));
            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_FM_OtherExpenseItems, tb_FM_OtherExpense>(c => c.ExpenseNo, r => r.ExpenseNo);

        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_FM_OtherExpenseItems>()
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_FM_OtherExpenseItems).Name + "Processor");
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
