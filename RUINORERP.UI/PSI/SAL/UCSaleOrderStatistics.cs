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
    [MenuAttrAssemblyInfo("销售订单统计", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.销售订单统计)]
    public partial class UCSaleOrderStatistics : BaseMasterQueryWithCondition<View_SaleOrderItems>
    {
        public UCSaleOrderStatistics()
        {
            InitializeComponent();
            //生成查询条件的相关实体 是不是也一个组，主子表呢？
            ReladtedEntityType = typeof(View_SaleOrderItems);
            base.WithOutlook = true;
            
        }
        private void UCSaleOrderStatistics_Load(object sender, EventArgs e)
        {
            //这个应该是一个组 多个表
            // base._UCBillMasterQuery.ColDisplayType = typeof(tb_Prod);
            base._UCBillMasterQuery.ColDisplayTypes = new List<Type>();

            //不能在上面的构造函数里面初始化
            base._UCBillMasterQuery.GridRelated.SetRelatedInfo<View_SaleOrderItems, tb_SaleOrder>(c => c.SOrderNo, r => r.SOrderNo);

            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_SaleOrder));
            base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_SaleOrderDetail));
            base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_ProdCategories));
            base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_Unit));
            base._UCBillOutlookGridAnalysis.ColDisplayTypes = base._UCBillMasterQuery.ColDisplayTypes;
        }
        public override void BuildColNameDataDictionary()
        {
           
        }

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_SaleOrderItems>()
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_SaleOrderItems).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Quantity);
            base.MasterSummaryCols.Add(c => c.SubtotalTransAmount);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.SOrder_ID);
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
        }

       
    }
}
