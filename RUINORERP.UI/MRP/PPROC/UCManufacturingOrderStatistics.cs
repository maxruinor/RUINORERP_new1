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

namespace RUINORERP.UI.MRP.PPROC
{
    /// <summary>
    /// 制令单统计窗体 - 提供制令单明细数据的统计和分析功能
    /// 参考: UCMaterialRequisitionStatistics 和 UCSaleOrderStatistics 的实现方式
    /// </summary>
    [MenuAttrAssemblyInfo("制令单统计", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产, BizType.制令单统计)]
    public partial class UCManufacturingOrderStatistics : BaseNavigatorGeneric<View_ManufacturingOrderItems, View_ManufacturingOrderItems>
    {
        /// <summary>
        /// 构造函数 - 初始化制令单统计窗体
        /// </summary>
        public UCManufacturingOrderStatistics()
        {
            InitializeComponent();
            // 设置关联实体类型为制令单明细视图
            ReladtedEntityType = typeof(View_ManufacturingOrderItems);
            base.WithOutlook = true;
        }

        /// <summary>
        /// 窗体加载事件 - 配置列显示类型和关联信息
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void UCManufacturingOrderStatistics_Load(object sender, EventArgs e)
        {
            // 初始化列显示类型集合
            base._UCMasterQuery.ColDisplayTypes = new List<Type>();

            // 设置制令单的关联信息
            base._UCMasterQuery.GridRelated.SetRelatedInfo<View_ManufacturingOrderItems, tb_ManufacturingOrder>(c => c.MONO, r => r.MONO);

            // 添加需要显示的关联实体类型
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ManufacturingOrder));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ManufacturingOrderDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProdCategories));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Unit));

            // 将相同的配置应用到分组分析控件
            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;
            base._UCOutlookGridGroupAnalysis.GridRelated.SetRelatedInfo<View_ManufacturingOrderItems, tb_ManufacturingOrder>(c => c.MONO, r => r.MONO);
        }

        /// <summary>
        /// 构建权限限制查询条件 - 根据用户权限限制数据访问范围
        /// </summary>
        public override void BuildLimitQueryConditions()
        {
            // 创建查询表达式
            var lambda = Expressionable.Create<View_ManufacturingOrderItems>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), 
                    t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                .ToExpression();
            
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 构建查询条件 - 从Processor获取查询过滤器
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_ManufacturingOrderItems).Name + "Processor");
            QueryFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 构建汇总列 - 指定需要汇总统计的数值列
        /// </summary>
        public override void BuildSummaryCols()
        {
            // 添加制令单相关的数值汇总列
            base.MasterSummaryCols.Add(c => c.ManufacturingQty);
            base.MasterSummaryCols.Add(c => c.ShouldSendQty);
            base.MasterSummaryCols.Add(c => c.ActualSentQty);
            base.MasterSummaryCols.Add(c => c.UnitCost);
            base.MasterSummaryCols.Add(c => c.SubtotalUnitCost);
            base.MasterSummaryCols.Add(c => c.Quantity);
        }

        /// <summary>
        /// 构建隐藏列 - 指定需要在界面中隐藏的列
        /// </summary>
        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrimaryKeyID);
            base.MasterInvisibleCols.Add(c => c.BOM_ID);
        }
    }
}
