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
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("其他入库单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.其他出入库管理, BizType.其他入库单)]
    public partial class UCStockInQuery : BaseBillQueryMC<tb_StockIn, tb_StockInDetail>
    {

        public UCStockInQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.BillNo);
            base.tsbtnAntiApproval.Visible = false;
    

        }
         


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_StockIn>()
                             .And(t => t.isdeleted == false)
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_StockIn).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_StockIn>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
              .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.ChildSummaryCols.Add(c => c.Qty);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.TotalCost);
            base.ChildInvisibleCols.Add(c => c.Cost);
            base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }


    }
}
