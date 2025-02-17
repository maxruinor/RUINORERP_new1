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

namespace RUINORERP.UI.MRP.PQC
{
    [MenuAttrAssemblyInfo("返工入库统计", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.生产品控, BizType.返工入库统计)]
    public partial class UCMRPReworkEntryStatistics : BaseNavigatorGeneric<View_MRP_ReworkEntry, View_MRP_ReworkEntry>
    {
        public UCMRPReworkEntryStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 是不是也一个组，主子表呢？
            ReladtedEntityType = typeof(View_MRP_ReworkEntry);
            base.WithOutlook = true;

        }
        private void UCSaleOrderStatistics_Load(object sender, EventArgs e)
        {
            //这个应该是一个组 多个表
            // base._UCMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();

            //不能在上面的构造函数里面初始化
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_MRP_ReworkEntry, tb_ManufacturingOrder>(c => c.ReworkEntryNo, r => r.MONO);

            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_MRP_ReworkEntry));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_MRP_ReworkEntryDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProdCategories));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Unit));
            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_MRP_ReworkEntry, tb_ManufacturingOrder>(c => c.ReworkEntryNo, r => r.MONO);
        }
        
        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_MRP_ReworkEntry>()
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_MRP_ReworkEntry).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Quantity);
            base.MasterSummaryCols.Add(c => c.SubtotalCostAmount);
            base.MasterSummaryCols.Add(c => c.SubtotalReworkFee);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ReworkEntryID);
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }


    }
}
