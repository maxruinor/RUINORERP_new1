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
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.UI.BI;
using RUINORERP.Common.Helper;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using System.Collections;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.PSI.INV
{
    /// <summary>
    /// 库存变动流水查询窗体
    /// 用于查询库存变动流水记录，包括入库、出库、调拨等操作记录
    /// </summary>
    [MenuAttrAssemblyInfo("库存变动流水查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.库存管理, BizType.库存流水查询)]
    public partial class UCInventoryTransactionQuery : BaseForm.BaseListGeneric<tb_InventoryTransaction>, IToolStripMenuInfoAuth
    {
        public UCInventoryTransactionQuery()
        {
            InitializeComponent();
            base.EditForm = null;
            
            this.Load += UCInventoryTransactionQuery_Load;
            
            // 隐藏不需要的按钮
            toolStripButtonSave.Visible = false;
            toolStripButtonModify.Visible = false;
            toolStripButtonAdd.Visible = false;
            toolStripButtonDelete.Visible = false;
        }

        private void UCInventoryTransactionQuery_Load(object sender, EventArgs e)
        {
        }

        #region 扩展功能按钮

        /// <summary>
        /// 添加扩展按钮
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <returns>扩展按钮数组</returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] {  };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }



        #endregion

        #region 查询条件构建

        /// <summary>
        /// 构建查询条件
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_InventoryTransaction).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 构建隐藏列
        /// </summary>
        public override void BuildInvisibleCols()
        {
            base.InvisibleColsExp.Add(c => c.TranID);
            base.InvisibleColsExp.Add(c => c.ProdDetailID);
            base.InvisibleColsExp.Add(c => c.Location_ID);
            base.InvisibleColsExp.Add(c => c.ReferenceId);
        }

        /// <summary>
        /// 构建汇总列
        /// </summary>
        public override void BuildSummaryCols()
        {
            SummaryCols.Add(c => c.QuantityChange);
            SummaryCols.Add(c => c.UnitCost);
        }

        #endregion

        #region 查询执行

        /// <summary>
        /// 异步查询
        /// </summary>
        /// <param name="UseNavQuery">是否使用导航查询</param>
        public async override void QueryAsync(bool UseNavQuery = false)
        {
            if (Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }
            
            if (QueryConditionFilter == null || QueryConditionFilter.QueryFields == null || QueryConditionFilter.QueryFields.Count == 0)
            {
                dataGridView1.ReadOnly = true;

                // 基础查询
                Expression<Func<tb_InventoryTransaction, bool>> expression = QueryConditionFilter.GetFilterExpression<tb_InventoryTransaction>();
                List<tb_InventoryTransaction> list = await MainForm.Instance.AppContext.Db.Queryable<tb_InventoryTransaction>()
                    .WhereIF(expression != null, expression)
                    .OrderBy(c => c.TransactionTime, OrderByType.Desc)
                    .ToListAsync();

                // 设置汇总列
                List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
                if (masterlist.Count > 0)
                {
                    dataGridView1.IsShowSumRow = true;
                    dataGridView1.SumColumns = masterlist.ToArray();
                }

                ListDataSoure.DataSource = list.ToBindingSortCollection();
                dataGridView1.DataSource = ListDataSoure;
            }
            else
            {
                ExtendedQuery();
            }
            
            ToolBarEnabledControl(MenuItemEnums.查询);
        }

        /// <summary>
        /// 扩展查询（带条件查询）
        /// </summary>
        /// <param name="UseAutoNavQuery">是否使用自动导航查询</param>
        protected async override void ExtendedQuery(bool UseAutoNavQuery = false)
        {
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;

            dataGridView1.ReadOnly = true;

            int pageNum = 1;
            int pageSize = int.Parse(base.txtMaxRows.Text);

            List<tb_InventoryTransaction> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDtoProxy, pageNum, pageSize) as List<tb_InventoryTransaction>;

            // 设置汇总列
            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
            if (masterlist.Count > 0)
            {
                dataGridView1.IsShowSumRow = true;
                dataGridView1.SumColumns = masterlist.ToArray();
            }

            ListDataSoure.DataSource = list.ToBindingSortCollection();
            dataGridView1.DataSource = ListDataSoure;

            ToolBarEnabledControl(MenuItemEnums.查询);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 根据时间范围筛选数据
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        public async Task<List<tb_InventoryTransaction>> FilterByTimeRange(DateTime startTime, DateTime endTime)
        {
            return await MainForm.Instance.AppContext.Db.Queryable<tb_InventoryTransaction>()
                .Where(c => c.TransactionTime >= startTime && c.TransactionTime <= endTime)
                .OrderBy(c => c.TransactionTime, OrderByType.Desc)
                .ToListAsync();
        }

        /// <summary>
        /// 根据业务类型筛选数据
        /// </summary>
        /// <param name="bizType">业务类型</param>
        public async Task<List<tb_InventoryTransaction>> FilterByBizType(int bizType)
        {
            return await MainForm.Instance.AppContext.Db.Queryable<tb_InventoryTransaction>()
                .Where(c => c.BizType == bizType)
                .OrderBy(c => c.TransactionTime, OrderByType.Desc)
                .ToListAsync();
        }

        /// <summary>
        /// 根据产品详情ID筛选数据
        /// </summary>
        /// <param name="prodDetailId">产品详情ID</param>
        public async Task<List<tb_InventoryTransaction>> FilterByProdDetailId(long prodDetailId)
        {
            return await MainForm.Instance.AppContext.Db.Queryable<tb_InventoryTransaction>()
                .Where(c => c.ProdDetailID == prodDetailId)
                .OrderBy(c => c.TransactionTime, OrderByType.Desc)
                .ToListAsync();
        }

        #endregion
    }
}