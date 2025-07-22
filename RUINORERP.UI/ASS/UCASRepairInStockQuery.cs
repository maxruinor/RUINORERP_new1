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
using RUINORERP.Model.Base;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.UControls;
using LiveChartsCore.Geo;
using Netron.GraphLib;
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;


namespace RUINORERP.UI.ASS
{

    [MenuAttrAssemblyInfo("维修入库单查询", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.维修中心, BizType.维修入库单)]
    public partial class UCASRepairInStockQuery : BaseBillQueryMC<tb_AS_RepairInStock, tb_AS_RepairInStockDetail>
    {
        public UCASRepairInStockQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.RepairInStockNo);
        }
     
        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_AS_RepairInStock, tb_AS_RepairOrder>(c => c.RepairOrderNo, r => r.RepairOrderNo);
            base.SetGridViewDisplayConfig();
        }

        public override void BuildInvisibleCols()
        {
            //引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.RepairOrderID);
            base.MasterInvisibleCols.Add(c => c.RepairInStockID);
            base.ChildInvisibleCols.Add(c => c.RepairOrderDetailID);
        }



        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_AS_RepairInStock>()
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                               .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            base.BuildQueryCondition();
            var lambda = Expressionable.Create<tb_AS_RepairInStock>()
            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            

            base.ChildSummaryCols.Add(c => c.Quantity);
            
            
            
        }

    }

}
