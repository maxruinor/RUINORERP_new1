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
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.FM.FMBase
{

    //损益费用单
    public partial class UCProfitLossQuery : BaseBillQueryMC<tb_FM_ProfitLoss, tb_FM_ProfitLossDetail>
    {
        public UCProfitLossQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ProfitLossNo);
        }


        /// <summary>
        /// 收付款方式决定是不收入还是支出。收款=收入， 支出=付款
        /// </summary>
        public ProfitLossDirection profitLossDirect { get; set; }

        public override void BuildLimitQueryConditions()
        {


            //创建表达式
            var lambda = Expressionable.Create<tb_FM_ProfitLoss>()
                              .And(t => t.ProfitLossDirection == (int)profitLossDirect)
                             .And(t => t.isdeleted == false)
                            .ToExpression(); 
            base.LimitQueryConditions = lambda;
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TaxTotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.UntaxedTotalAmont);
            

            base.ChildSummaryCols.Add(c => c.SubtotalAmont);
            base.ChildSummaryCols.Add(c => c.TaxSubtotalAmont);
            base.ChildSummaryCols.Add(c => c.UntaxedSubtotalAmont);
            base.ChildSummaryCols.Add(c => c.Quantity);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ProfitLossDirection);
            base.MasterInvisibleCols.Add(c => c.ProfitLossId);
            base.MasterInvisibleCols.Add(c => c.SourceBillId);
            base.ChildInvisibleCols.Add(c => c.ProfitLossDetail_ID);
        }



        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            bool isIncome = true;
            

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_ProfitLoss).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            var lambda = Expressionable.Create<tb_FM_ProfitLoss>()
                            .And(t => t.isdeleted == false)
                            .And(t => t.ProfitLossDirection == (int)profitLossDirect)
                           .ToExpression() ;
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);


        }

        private void UCProfitLossQuery_Load(object sender, EventArgs e)
        {
            if (base._UCBillMasterQuery == null)
            {
                return;
            }
            base._UCBillMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_ProfitLoss>(c => c.SourceBizType, c => c.SourceBillNo);
            BizTypeMapper mapper = new BizTypeMapper();
            //将枚举中的值循环
            foreach (var biztype in Enum.GetValues(typeof(BizType)))
            {
                var tableName = mapper.GetTableType((BizType)biztype);
                if (tableName == null)
                {
                    continue;
                }
                ////这个参数中指定要双击的列单号。是来自另一组  一对一的指向关系
                //因为后面代码去查找时，直接用的 从一个对象中找这个列的值。但是枚举显示的是名称。所以这里直接传入枚举的值。
                KeyNamePair keyNamePair = new KeyNamePair(((int)((BizType)biztype)).ToString(), tableName.Name);
                base._UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FM_ProfitLoss>(c => c.SourceBillNo, keyNamePair);
            }
        }
    }



}
