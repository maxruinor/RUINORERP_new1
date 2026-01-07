using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
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
using OfficeOpenXml;

namespace RUINORERP.UI.PSI.INV
{
    /// <summary>
    /// 库存快照查询窗体
    /// 用于查询库存快照数据，包括期初库存、在途库存、拟销售量等信息
    /// </summary>
    [MenuAttrAssemblyInfo("库存快照查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.库存管理, BizType.库存快照查询)]
    public partial class UCInventorySnapshotQuery : BaseForm.BaseListGeneric<tb_InventorySnapshot>, IToolStripMenuInfoAuth
    {
        public UCInventorySnapshotQuery()
        {
            InitializeComponent();
            base.EditForm = null;
            
            this.Load += UCInventorySnapshotQuery_Load;
            
            // 隐藏不需要的按钮
            toolStripButtonSave.Visible = false;
            toolStripButtonModify.Visible = false;
            toolStripButtonAdd.Visible = false;
            toolStripButtonDelete.Visible = false;
        }

        private void UCInventorySnapshotQuery_Load(object sender, EventArgs e)
        {
            // 添加扩展按钮
            AddExtendButton(CurMenuInfo);
        }

        #region 扩展功能按钮

        /// <summary>
        /// 添加扩展按钮
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <returns>扩展按钮数组</returns>
        public ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            ToolStripButton toolStripButton导出Excel = new System.Windows.Forms.ToolStripButton();
            toolStripButton导出Excel.Text = "导出Excel";
            toolStripButton导出Excel.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton导出Excel.Name = "导出Excel";
            toolStripButton导出Excel.Visible = true;
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton导出Excel);
            toolStripButton导出Excel.ToolTipText = "将库存快照数据导出到Excel文件。";
            toolStripButton导出Excel.Click += new System.EventHandler(this.toolStripButton导出Excel_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton导出Excel };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }

        /// <summary>
        /// 导出Excel按钮点击事件
        /// </summary>
        private void toolStripButton导出Excel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel文件 (*.xlsx)|*.xlsx";
                    saveDialog.FileName = $"库存快照查询_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var data = bindingSourceList.List.Cast<tb_InventorySnapshot>().ToList();
                        if (data.Count > 0)
                        {
                            // 使用现有项目中的导出方式
                            ExportDataToExcel(data, saveDialog.FileName, "库存快照查询");
                            MainForm.Instance.ShowStatusText("导出Excel成功!");
                        }
                        else
                        {
                            MessageBox.Show("没有数据可导出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <param name="data">数据列表</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">工作表名称</param>
        private void ExportDataToExcel(List<tb_InventorySnapshot> data, string filePath, string sheetName)
        {
            try
            {
                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(sheetName);
                    
                    // 设置表头
                    int row = 1;
                    int col = 1;
                    
                    // 添加表头
                    worksheet.Cells[row, col++].Value = "快照ID";
                    worksheet.Cells[row, col++].Value = "产品详情ID";
                    worksheet.Cells[row, col++].Value = "库位ID";
                    worksheet.Cells[row, col++].Value = "实际库存";
                    worksheet.Cells[row, col++].Value = "期初数量";
                    worksheet.Cells[row, col++].Value = "在途库存";
                    worksheet.Cells[row, col++].Value = "拟销售量";
                    worksheet.Cells[row, col++].Value = "在制数量";
                    worksheet.Cells[row, col++].Value = "未发料量";
                    worksheet.Cells[row, col++].Value = "快照时间";
                    worksheet.Cells[row, col++].Value = "备注说明";
                    
                    // 添加数据
                    row++;
                    foreach (var item in data)
                    {
                        col = 1;
                        worksheet.Cells[row, col++].Value = item.SnapshotID;
                        worksheet.Cells[row, col++].Value = item.ProdDetailID;
                        worksheet.Cells[row, col++].Value = item.Location_ID;
                        worksheet.Cells[row, col++].Value = item.Quantity;
                        worksheet.Cells[row, col++].Value = item.InitInventory;
                        worksheet.Cells[row, col++].Value = item.On_the_way_Qty;
                        worksheet.Cells[row, col++].Value = item.Sale_Qty;
                        worksheet.Cells[row, col++].Value = item.MakingQty;
                        worksheet.Cells[row, col++].Value = item.NotOutQty;
                        worksheet.Cells[row, col++].Value = item.SnapshotTime?.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[row, col++].Value = item.Notes;
                        row++;
                    }
                    
                    // 保存文件
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"导出Excel失败: {ex.Message}");
            }
        }

        #endregion

        #region 查询条件构建

        /// <summary>
        /// 构建查询条件
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_InventorySnapshot).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 构建隐藏列
        /// </summary>
        public override void BuildInvisibleCols()
        {
            base.InvisibleColsExp.Add(c => c.SnapshotID);
            base.InvisibleColsExp.Add(c => c.ProdDetailID);
            base.InvisibleColsExp.Add(c => c.Location_ID);
        }

        /// <summary>
        /// 构建汇总列
        /// </summary>
        public override void BuildSummaryCols()
        {
            SummaryCols.Add(c => c.Quantity);
            SummaryCols.Add(c => c.InitInventory);
            SummaryCols.Add(c => c.On_the_way_Qty);
            SummaryCols.Add(c => c.Sale_Qty);
            SummaryCols.Add(c => c.MakingQty);
            SummaryCols.Add(c => c.NotOutQty);
            SummaryCols.Add(c => c.Inv_SubtotalCostMoney);
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
                Expression<Func<tb_InventorySnapshot, bool>> expression = QueryConditionFilter.GetFilterExpression<tb_InventorySnapshot>();
                List<tb_InventorySnapshot> list = await MainForm.Instance.AppContext.Db.Queryable<tb_InventorySnapshot>()
                    .WhereIF(expression != null, expression)
                    .OrderBy(c => c.SnapshotTime, OrderByType.Desc)
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

            List<tb_InventorySnapshot> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDtoProxy, pageNum, pageSize) as List<tb_InventorySnapshot>;

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
        /// 根据快照时间筛选数据
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        public async Task<List<tb_InventorySnapshot>> FilterBySnapshotTime(DateTime startTime, DateTime endTime)
        {
            return await MainForm.Instance.AppContext.Db.Queryable<tb_InventorySnapshot>()
                .Where(c => c.SnapshotTime >= startTime && c.SnapshotTime <= endTime)
                .OrderBy(c => c.SnapshotTime, OrderByType.Desc)
                .ToListAsync();
        }

        #endregion
    }
}