using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.ReportData;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Report;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.PSI.INV
{

    [MenuAttrAssemblyInfo("转换单统计", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.转换单统计)]
    public partial class UCProdConversionStatistics : BaseNavigatorGeneric<View_ProdConversionItems, View_ProdConversionItems>
    {
        public UCProdConversionStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 视图时要指定
            ReladtedEntityType = typeof(View_ProdConversionItems);
        }

        public override List<NavParts[]> AddNavParts()
        {
            List<NavParts[]> strings = new List<NavParts[]>();
            strings.Add(new NavParts[] { NavParts.查询结果, NavParts.分组显示 });
            return strings;
        }



        private void UCPurEntryStatistics_Load(object sender, EventArgs e)
        {

            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_ProdConversionItems, tb_ProdConversion>(c => c.ConversionNo, r => r.ConversionNo);
            //base._UCMasterQuery.GridRelated.SetRelatedInfo<View_ProdConversionItems, tb_ProdReturning>(c => c.BorrowNo, r => r.ReturnNo);
            ////这个应该是一个组 多个表
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail)); 
            ////是否能通过一两个主表，通过 外键去找多级关联的表？
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_PurOrder));
            //base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_PurOrderDetail));

            //base._UCBillOutlookGridAnalysis.ColDisplayTypes = base._UCBillMasterQuery.ColDisplayTypes;
            //base._UCMasterQuery.newSumDataGridViewMaster.Use是否使用内置右键功能 = false;
            //base._UCMasterQuery.newSumDataGridViewMaster.ContextMenuStrip = contextMenuStrip1;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_ProdConversionItems, tb_ProdConversion>(c => c.ConversionNo, r => r.ConversionNo);
        }


        public override void BuildColDisplayTypes()
        {
            MasterColDisplayTypes = new List<Type>();
            MasterColDisplayTypes.Add(typeof(tb_Prod));
            MasterColDisplayTypes.Add(typeof(tb_ProductType));
            MasterColDisplayTypes.Add(typeof(tb_PaymentMethod));
            MasterColDisplayTypes.Add(typeof(View_ProdDetail));
        }

     

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_ProdConversionItems>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            //.And(t => t.isdeleted == false)

                            //.And(t => t.Is_enabled == true)

                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_ProdConversionItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.ConversionQty);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }

     
    }
}
