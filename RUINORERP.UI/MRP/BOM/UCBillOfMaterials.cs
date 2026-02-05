using AutoMapper;
using DevAge.Windows.Forms;
using HLH.Lib.List;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using log4net.Core;
using Microsoft.Extensions.Logging;
using Mysqlx.Crud;
using Netron.GraphLib;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.Report;
using RUINORERP.UI.UCSourceGrid;
using SourceGrid;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using FileInfo = System.IO.FileInfo;




namespace RUINORERP.UI.MRP.BOM
{



    /*
    直接材料成本：用于生产产品的直接原材料、零部件、组件等的成本。
    直接劳动力成本：直接参与产品生产的员工的工资、福利和奖金等成本。
    制造费用：与生产过程相关的间接费用，如设备折旧费、水电费、间接劳动力成本等。
    采购成本：购买原材料、零部件和外协加工服务的成本。
    质量控制成本：与确保产品质量相关的成本，包括检测、检验和质量管理活动的费用。
    包装和运输成本：产品包装材料的成本以及将产品运送到客户或仓库的费用。
    间接费用：管理和支持生产活动的间接成本，如管理人员工资、办公费用、保险费等。
    报废和返工成本：由于生产过程中的缺陷或错误导致的报废品和返工所需的成本。
     
     */
    [MenuAttrAssemblyInfo("产品配方清单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.MRP基本资料, BizType.BOM物料清单)]
    public partial class UCBillOfMaterials : BaseBillEditGeneric<tb_BOM_S, tb_BOM_SDetail>, IPublicEntityObject
    {
        public UCBillOfMaterials()
        {
            InitializeComponent();

            kryptonDockableNavigator1.SelectedPage = kryptonPage1;
            if (!this.DesignMode)
            {
                DisplayTextResolver = new GridViewDisplayTextResolverGeneric<tb_BOM_SDetail>();
                DisplayTextResolver.AddReferenceKeyMapping<tb_Unit, tb_BOM_SDetail>(c => c.Unit_ID, t => t.Unit_ID, t => t.UnitName);
                DisplayTextResolver.Initialize(kryptonTreeGridViewBOMDetail);
            }
        }


        #region 导出excel
        ToolStripButton toolStripButton导出excel = new System.Windows.Forms.ToolStripButton();

        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            toolStripButton导出excel.Text = "导出Excel";
            toolStripButton导出excel.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton导出excel.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton导出excel.Name = "导出Excel";
            toolStripButton导出excel.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton导出excel);
            toolStripButton导出excel.Click += new System.EventHandler(this.ExportExcel_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[]
            {
                toolStripButton导出excel };

            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }


        #endregion

        // 忽略属性配置
        // 重写忽略属性配置
        protected override IgnorePropertyConfiguration ConfigureIgnoreProperties()
        {
            return base.ConfigureIgnoreProperties()
                // 主表忽略的属性
                .Ignore<tb_BOM_S>(
                    e => e.BOM_ID,
                    e => e.PrimaryKeyID,
                    e => e.DataStatus,
                    e => e.ApprovalStatus,
                    e => e.ApprovalResults,
                    e => e.Approver_by,
                    e => e.Approver_at

              )
                // 明细表忽略的属性
                .Ignore<tb_BOM_SDetail>(
                    e => e.SubID,
                    e => e.Substitute);
        }

        private void ExportExcel_Click(object sender, EventArgs e)
        {
            //// 数据更新后强制刷新滚动条
            //grid1.AutoStretchRowsToFitHeight = false;
            //grid1.VScrollBar.Maximum = grid1.Rows.Count * 1;
            //grid1.Invalidate();
            if (kryptonTreeGridViewBOMDetail.DataSource is DataTable dt)
            {
                //如果展开

                //如果收起时，kryptonTreeGridViewBOMDetail.rows.count是对的。否则是错的。
                //先刷新一下
                if (kryptonTreeGridViewBOMDetail.Rows.Count == 0)
                {
                    Refreshs();
                }
                if (kryptonTreeGridViewBOMDetail.Rows.Count > 0)
                {
                    //如果明细中没有值的列。不显示
                }
                ExportExcel(dt);
            }
        }

        #region BOM配方导出


        public bool ExportExcelNew(DataTable dt = null)
        {
            if (EditEntity == null)
            {
                MessageBox.Show("没有可导出的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            bool rs = false;
            string selectedFile = string.Empty;
            Stopwatch stopwatch = null;

            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files (*.xlsx; *.xls)|*.xlsx; *.xls";
                    if (saveFileDialog.ShowDialog() != DialogResult.OK) return false;

                    selectedFile = saveFileDialog.FileName;
                    stopwatch = Stopwatch.StartNew();

                    OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    FileInfo fileInfo = new FileInfo(selectedFile);

                    using (var package = new ExcelPackage(fileInfo))
                    {
                        ExcelWorksheet worksheet = GetOrCreateWorksheet(package, "Sheet1");
                        worksheet.Cells.Clear();

                        // 写入主标题
                        CreateMainTitle(worksheet, dt != null);

                        // 添加主表汇总数据
                        AddSummaryInfo(worksheet);

                        if (dt != null)
                        {
                            ExportExpandedData(worksheet, dt);
                        }
                        else
                        {
                            ExportCollapsedData(worksheet);
                        }
                        //第一列 品名 设置宽点
                        worksheet.Column(1).Width = 30;
                        package.Save();
                    }
                }

                stopwatch?.Stop();
                ShowExportResult(stopwatch?.Elapsed, selectedFile);
                return true;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError("Excel导出错误", ex);
                MessageBox.Show($"导出失败：{ex.Message}");
                return false;
            }
        }

        private void ExportCollapsedData(ExcelWorksheet worksheet)
        {
            // 列头从第4行开始（主标题1行 + 汇总信息2行）
            CreateColumnHeaders(worksheet, 4);

            // 数据从第5行开始
            int currentRow = 5;
            var visibleColumns = GetVisibleColumns().ToList();

            // 遍历根节点
            foreach (KryptonTreeGridNodeRow rootRow in kryptonTreeGridViewBOMDetail.Rows
                .Cast<KryptonTreeGridNodeRow>()
                .Where(r => r.Parent == null))
            {
                // 写入父节点
                currentRow = WriteCollapsedRow(worksheet, rootRow, currentRow, visibleColumns, 0);

                // 如果节点展开则处理子节点
                if (rootRow.IsExpanded)
                {
                    currentRow = ProcessChildNodes(worksheet, rootRow, currentRow, visibleColumns, 1);
                }
            }

            // 自动调整所有列宽
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private int WriteCollapsedRow(ExcelWorksheet worksheet,
                                    KryptonTreeGridNodeRow gridRow,
                                    int currentRow,
                                    List<DataGridViewColumn> visibleColumns,
                                    int indentLevel)
        {

            // 增加缩进量（每级4个空格+竖线）
            string indentPrefix = new string(' ', indentLevel * 4);
            if (indentLevel > 0)
            {
                indentPrefix = new string('│', indentLevel) + " ";
            }



            // 设置行高
            worksheet.Row(currentRow).Height = 20;

            // 设置背景色
            worksheet.Row(currentRow).Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Row(currentRow).Style.Fill.BackgroundColor.SetColor(BomBackColors[indentLevel % BomBackColors.Length]);

            // 写入单元格数据
            int colIndex = 1;
            foreach (DataGridViewColumn col in visibleColumns)
            {
                var cell = worksheet.Cells[currentRow, colIndex];
                var gridCell = gridRow.Cells[col.Index];

                // 第一列添加树形缩进
                if (colIndex == 1)
                {
                    cell.Value = indentPrefix + GetCellDisplayValue(gridCell);
                }
                else
                {
                    cell.Value = GetCellDisplayValue(gridCell);
                }

                // 设置数值格式
                if (IsNumericColumn(col))
                {
                    cell.Style.Numberformat.Format = "#,##0.00";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                colIndex++;
            }

            return currentRow + 1;
        }

        private int ProcessChildNodes(ExcelWorksheet worksheet,
                                    KryptonTreeGridNodeRow parentRow,
                                    int currentRow,
                                    List<DataGridViewColumn> visibleColumns,
                                    int indentLevel)
        {
            foreach (KryptonTreeGridNodeRow childRow in parentRow.Nodes)
            {
                currentRow = WriteCollapsedRow(worksheet, childRow, currentRow, visibleColumns, indentLevel);

                // 递归处理子节点
                if (childRow.IsExpanded && childRow.Nodes.Count > 0)
                {
                    currentRow = ProcessChildNodes(worksheet, childRow, currentRow, visibleColumns, indentLevel + 1);
                }
            }
            return currentRow;
        }

        private string GetCellDisplayValue(DataGridViewCell cell)
        {
            return cell.FormattedValue?.ToString()
                ?? cell.Value?.ToString()
                ?? string.Empty;
        }

        // 完整的FillCellsloop实现（原代码中的递归方法）
        private int FillCellsloopNew(ExcelWorksheet worksheet,
                                int startRow,
                                KryptonTreeGridNodeRow parentRow,
                                int indentLevel)
        {
            int currentRow = startRow;
            foreach (KryptonTreeGridNodeRow childRow in parentRow.Nodes)
            {
                currentRow = WriteCollapsedRow(worksheet, childRow, currentRow, GetVisibleColumns().ToList(), indentLevel);

                // 递归处理子节点
                if (childRow.Nodes.Count > 0)
                {
                    currentRow = FillCellsloopNew(worksheet, currentRow, childRow, indentLevel + 1);
                }
            }
            return currentRow;
        }

        private ExcelWorksheet GetOrCreateWorksheet(ExcelPackage package, string sheetName)
        {
            var worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == sheetName);
            return worksheet ?? package.Workbook.Worksheets.Add(sheetName);
        }

        private void CreateMainTitle(ExcelWorksheet worksheet, bool isExpanded)
        {


            // //第一行开始写BOM主表内容。

            //// 合并指定单元格
            //excelSheet.Cells[1, 1, 1, kryptonTreeGridViewBOMDetail.ColumnCount].Merge = true;

            //// 设置合并后单元格的内容
            //excelSheet.Cells[1, 1].Value = $"BOM清单：【sku:{EditEntity.SKU}】 " + EditEntity.BOM_Name;
            //// 获取指定单元格
            //var cellTitle = excelSheet.Cells[1, 1];

            //// 设置单元格内容为加粗
            //cellTitle.Style.Font.Bold = true;
            //// 设置单元格内容的字体大小
            //cellTitle.Style.Font.Size = 28;
            //// 设置单元格内容居中对齐
            //cellTitle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            //cellTitle.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            int visibleColumnCount = GetVisibleColumns().Count();
            var titleCell = worksheet.Cells[1, 1, 1, visibleColumnCount]; // 根据可见列数量合并
            //var titleCell = worksheet.Cells[1, 1, 1, kryptonTreeGridViewBOMDetail.ColumnCount];
            titleCell.Merge = true;
            worksheet.Cells[1, 1].Value = isExpanded
                ? $"BOM清单：【sku:{EditEntity.SKU}】{EditEntity.BOM_Name}"
                : $"{EditEntity.BOM_Name}BOM清单：";

            titleCell.Style.SetTitleStyle();
        }

        private void AddSummaryInfo(ExcelWorksheet worksheet, int StartRowIndex = 2)
        {
            int visibleColumnCount = GetVisibleColumns().Count();
            //// 第二行：总材料数量
            //worksheet.Cells[2, 1].Value = $"总材料数量：{EditEntity.TotalMaterialQty}";
            //worksheet.Cells[2, 1, 2, visibleColumnCount].Merge = true;
            //worksheet.Cells[2, 1].Style.SetSummaryStyle();

            //// 第三行：其他汇总信息（优化合并方式）
            //worksheet.Cells[3, 1].Value = $"自产分摊费用：{EditEntity.SelfApportionedCost}";
            //worksheet.Cells[3, 1, 3, 2].Merge = true;

            //worksheet.Cells[3, 3].Value = $"外发分摊费用：{EditEntity.OutApportionedCost}";
            //worksheet.Cells[3, 3, 3, 4].Merge = true;

            //worksheet.Cells[3, 5].Value = $"自产总成本：{EditEntity.SelfProductionAllCosts}";
            //worksheet.Cells[3, 5, 3, 6].Merge = true;

            //worksheet.Cells[3, 7].Value = $"外发总成本：{EditEntity.OutProductionAllCosts}";
            //worksheet.Cells[3, 7, 3, 8].Merge = true;

            //// 设置样式
            //Enumerable.Range(1, 7).Where(i => i % 2 == 1).ForEach(col =>
            //{
            //    worksheet.Cells[3, col].Style.SetSummaryStyle();
            //});

            //以单位成本为文本截止靠右： 成本小计显示数值。加总.
            int costIndex = GetVisibleColumns().ToList().FirstOrDefault(c => c.HeaderText == "单位成本").Index;

            // 第StartRowIndex行：总材料数量
            int row = 0;
            worksheet.Cells[StartRowIndex + row, 1].Value = $"总物料费用：";
            worksheet.Cells[StartRowIndex + row, 1, StartRowIndex + row, costIndex].Merge = true;
            worksheet.Cells[StartRowIndex + row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[StartRowIndex + row, costIndex + 1].Value = EditEntity.TotalMaterialCost;

            row++;
            worksheet.Cells[StartRowIndex + row, 1].Value = $"自产分摊费用：";
            worksheet.Cells[StartRowIndex + row, 1, StartRowIndex + row, costIndex].Merge = true;
            worksheet.Cells[StartRowIndex + row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[StartRowIndex + row, costIndex + 1].Value = EditEntity.SelfApportionedCost;
            row++;
            worksheet.Cells[StartRowIndex + row, 1].Value = $"外发分摊费用：";
            worksheet.Cells[StartRowIndex + row, 1, StartRowIndex + row, costIndex].Merge = true;
            worksheet.Cells[StartRowIndex + row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[StartRowIndex + row, costIndex + 1].Value = EditEntity.OutApportionedCost;
            row++;
            worksheet.Cells[StartRowIndex + row, 1].Value = $"自行制造费用：";
            worksheet.Cells[StartRowIndex + row, 1, StartRowIndex + row, costIndex].Merge = true;
            worksheet.Cells[StartRowIndex + row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[StartRowIndex + row, costIndex + 1].Value = EditEntity.TotalSelfManuCost;
            row++;
            worksheet.Cells[StartRowIndex + row, 1].Value = $"外发加工费用：";
            worksheet.Cells[StartRowIndex + row, 1, StartRowIndex + row, costIndex].Merge = true;
            worksheet.Cells[StartRowIndex + row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[StartRowIndex + row, costIndex + 1].Value = EditEntity.TotalOutManuCost;
            row++;

            worksheet.Cells[StartRowIndex + row, 1].Value = $"自产总成本：";
            worksheet.Cells[StartRowIndex + row, 1, StartRowIndex + row, costIndex].Merge = true;
            worksheet.Cells[StartRowIndex + row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[StartRowIndex + row, costIndex + 1].Value = EditEntity.SelfProductionAllCosts;
            row++;
            worksheet.Cells[StartRowIndex + row, 1].Value = $"外发总成本：";
            worksheet.Cells[StartRowIndex + row, 1, StartRowIndex + row, costIndex].Merge = true;
            worksheet.Cells[StartRowIndex + row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[StartRowIndex + row, costIndex + 1].Value = EditEntity.OutProductionAllCosts;





            #region    导出日期
            row++;
            worksheet.Cells[StartRowIndex + row, 1].Value = $"导出时间：" + System.DateTime.Now.ToString();
            worksheet.Cells[StartRowIndex + row, 1, StartRowIndex + row, visibleColumnCount].Merge = true;
            worksheet.Cells[StartRowIndex + row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[StartRowIndex + row, visibleColumnCount].Style.Font.Size = 20;
            // 设置单元格内容居中对齐
            worksheet.Cells[StartRowIndex + row, visibleColumnCount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[StartRowIndex + row, visibleColumnCount].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[StartRowIndex + row, visibleColumnCount].Style.Font.Bold = false;

            #endregion



            //5行设置一个背景色

            for (int i = 0; i < 4; i++)
            {
                worksheet.Cells[StartRowIndex + i, 2].Style.SetSummaryStyle();
            }




        }

        private void ExportExpandedData(ExcelWorksheet worksheet, DataTable dt)
        {
            // 列头从第4行开始
            CreateColumnHeaders(worksheet, 4);

            // 数据从第5行开始
            int currentRow = 5;
            var visibleColumns = GetVisibleColumns().ToList();

            // 生成数据行（已优化树形结构处理）
            foreach (KryptonTreeGridNodeRow row in kryptonTreeGridViewBOMDetail.Rows)
            {
                var dataRow = worksheet.Row(currentRow);
                dataRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
                dataRow.Style.Fill.BackgroundColor.SetColor(BomBackColors[row.Level]);

                int colIndex = 1;
                foreach (DataGridViewColumn col in visibleColumns)
                {
                    var cell = worksheet.Cells[currentRow, colIndex];
                    FormatCell(cell, row.Cells[col.Index], colIndex == 1 ? row.Level : 0);
                    colIndex++;
                }

                currentRow++;
            }


        }
        // 在类内添加这个方法
        private void CreateColumnHeaders(ExcelWorksheet worksheet, int startRow)
        {
            List<DataGridViewColumn> visibleColumns = GetVisibleColumns().ToList();

            int colIndex = 1;
            foreach (DataGridViewColumn column in visibleColumns)
            {
                var headerCell = worksheet.Cells[startRow, colIndex];
                headerCell.Value = column.HeaderText;

                // 设置列头样式
                headerCell.Style.Font.Bold = true;
                headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // 根据标题长度设置初始列宽（每个字符7像素）
                // 基础宽度 = 字符数 * 2像素 + 5像素边距
                double baseWidth = column.HeaderText.Length * 2 + 5;
                worksheet.Column(colIndex).Width = baseWidth;

                // 根据数据类型设置对齐方式
                // 数值列额外增加宽度
                if (IsNumericColumn(column))
                {
                    headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    // 数值列额外增加宽度
                    worksheet.Column(colIndex).Width += 5;
                }
                colIndex++;
            }

            // 自动调整列宽（在初始宽度基础上自动调整）
            // 最后自动调整
            //  worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }


        // 辅助方法：获取可见列
        private IEnumerable<DataGridViewColumn> GetVisibleColumns()
        {
            return kryptonTreeGridViewBOMDetail.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Visible && !string.IsNullOrEmpty(c.HeaderText));
        }

        // 辅助方法：判断是否是数值列
        private bool IsNumericColumn(DataGridViewColumn column)
        {
            return column.ValueType == typeof(decimal)
                || column.ValueType == typeof(int)
                || column.ValueType == typeof(double);
        }
        private void FormatCell(ExcelRange excelCell, DataGridViewCell gridCell, int indentLevel)
        {
            var indent = new string(' ', indentLevel * 3);
            excelCell.Value = indent + (gridCell.FormattedValue ?? gridCell.Value);

            if (gridCell.OwningColumn.ValueType == typeof(decimal))
            {
                excelCell.Style.Numberformat.Format = "#,##0.00";
                excelCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
        }

        /// <summary>
        /// 网格显示文本解析器，用于设置特殊的映射关系
        /// </summary>
        public GridViewDisplayTextResolverGeneric<tb_BOM_SDetail> DisplayTextResolver { get; set; }
        private void ShowExportResult(TimeSpan? elapsed, string filePath)
        {
            var message = $"成功导出 BOM数据，耗时 {elapsed?.TotalSeconds:F2} 秒。\n是否立即打开文件？";
            if (MessageBox.Show(message, "导出完成", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
        }

        // 8 colors  
        private Color[] BomBackColors = new Color[]{ Color.FromArgb(91, 155, 213), Color.FromArgb(221, 235, 247),
                                               Color.FromArgb(217, 173, 194), Color.FromArgb(165, 194, 215),
                                               Color.FromArgb(179, 166, 190), Color.FromArgb(234, 214, 163),
                                               Color.FromArgb(246, 250, 125), Color.FromArgb(188, 168, 225) };




        /// <summary>
        /// dt是为了得到展开时的行数。因为这时不准备。收起时才准。但是收起时，类型这种名称的值。没办法显示出来。
        /// 这里特殊一点。暂时没有优化掉。后面可以优化到UIExcelHelper
        /// </summary>
        /// <param name="dt">为null是收起</param>
        /// <returns></returns>
        public bool ExportExcel(DataTable dt = null)
        {
            if (EditEntity == null)
            {
                MessageBox.Show("没有可导出的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            List<System.Data.DataColumn> columns = dt.Columns.CastToList<System.Data.DataColumn>();
            bool rs = false;
            string selectedFile = string.Empty;
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Excel Files (*.xlsx; *.xls)|*.xlsx; *.xls";
                openFileDialog.FilterIndex = 1;
                openFileDialog.FileName = EditEntity.BOM_Name + "_" + EditEntity.SKU + "_BOM清单";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var stopwatch = Stopwatch.StartNew();
                    selectedFile = openFileDialog.FileName;
                    // MessageBox.Show($"您选中的文件路径为：{selectedFile}");
                    OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    //ConcurrentDictionary<string, string> columnskv = UIHelper.GetFieldNameList<tb_BOM_SDetailTree>();
                    using (var package = new ExcelPackage(new System.IO.FileInfo(selectedFile)))
                    {
                        ExcelWorksheet excelSheet = null;
                        if (!package.Workbook.Worksheets.Any(c => c.Name.Contains("Sheet1")))
                        {
                            excelSheet = package.Workbook.Worksheets.Add("Sheet1");
                        }
                        else
                        {
                            excelSheet = package.Workbook.Worksheets.First(c => c.Name.Contains("Sheet1"));
                        }
                        //清空
                        excelSheet.Cells.Clear();
                        if (dt != null)
                        {
                            #region 展开
                            try
                            {

                                // //第一行开始写BOM主表内容。
                                // 合并指定单元格
                                excelSheet.Cells[1, 1, 1, kryptonTreeGridViewBOMDetail.ColumnCount].Merge = true;

                                // 设置合并后单元格的内容
                                excelSheet.Cells[1, 1].Value = $"BOM清单：【sku:{EditEntity.SKU}】 " + EditEntity.BOM_Name;
                                // 获取指定单元格
                                var cellTitle = excelSheet.Cells[1, 1];

                                // 设置单元格内容为加粗
                                cellTitle.Style.Font.Bold = true;
                                // 设置单元格内容的字体大小
                                cellTitle.Style.Font.Size = 28;
                                // 设置单元格内容居中对齐
                                cellTitle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cellTitle.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                #region


                                //第三行开始给数据
                                int startRowIndexForData = 2;

                                //生成字段名称
                                int k = 0;
                                for (int i = 0; i < kryptonTreeGridViewBOMDetail.ColumnCount; i++)
                                {
                                    if (kryptonTreeGridViewBOMDetail.Columns[i].Visible && !string.IsNullOrEmpty(kryptonTreeGridViewBOMDetail.Columns[i].HeaderText)) //不导出隐藏的列
                                    {
                                        //第二行开始写列名。
                                        excelSheet.Cells[startRowIndexForData, k + 1].Value = kryptonTreeGridViewBOMDetail.Columns[i].HeaderText;
                                        // 获取指定单元格
                                        var cellHeader = excelSheet.Cells[startRowIndexForData, k + 1];
                                        // 设置单元格内容为加粗
                                        cellHeader.Style.Font.Bold = true;
                                        cellHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        //如果是数值类型则靠右？
                                        DataColumn dc = columns.FirstOrDefault(c => c.ColumnName == kryptonTreeGridViewBOMDetail.Columns[i].Name);
                                        if (dc.DataType.FullName == "System.Decimal")
                                        {
                                            cellHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                        }
                                        k++;
                                    }
                                }

                                int currentRow = startRowIndexForData;//这里空出2行，
                                                                      //填充数据
                                                                      //这里行数直接算到数据行数，因为是树形结构

                                List<int> MaxLevelList = new List<int>();//最大层级
                                int bomLevel = 0;//层级
                                                 //                                for (int i = 0; i < dt.Rows.Count; i++)
                                for (int i = 0; i < kryptonTreeGridViewBOMDetail.Rows.Count; i++)
                                {
                                    KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridViewBOMDetail.Rows[i] as KryptonTreeGridNodeRow;

                                    if (kryptonTreeGridNodeRow != null)
                                    {
                                        bomLevel = kryptonTreeGridNodeRow.Level;
                                        //当前行，
                                        currentRow++;//再空一行

                                        //如果他有子集则累各一下，算出最大层，为了后面扩宽第一列的宽度来参考计算,
                                        if (kryptonTreeGridNodeRow.HasChildren)
                                        {
                                            bomLevel = kryptonTreeGridNodeRow.Level;
                                            MaxLevelList.Add(bomLevel);//最大层级
                                                                       //// 设置行的背景色
                                                                       //row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                                       //row.Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);

                                        }

                                        var row = excelSheet.Row(currentRow);
                                        //设置背景色，按级别设置颜色 设置行的背景色
                                        row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        row.Style.Fill.BackgroundColor.SetColor(BomBackColors[kryptonTreeGridNodeRow.Level]);

                                        int IndentNumSpaces = 0;
                                        k = 0;
                                        for (int j = 0; j < kryptonTreeGridViewBOMDetail.ColumnCount; j++)
                                        {

                                            if (kryptonTreeGridViewBOMDetail.Columns[j].Visible && !string.IsNullOrEmpty(kryptonTreeGridViewBOMDetail.Columns[j].HeaderText)) //不导出隐藏的列
                                            {

                                                //如果是数值类型则靠右？
                                                DataColumn dcData = columns.FirstOrDefault(c => c.ColumnName == kryptonTreeGridViewBOMDetail.Columns[j].Name);

                                                //只是第一列缩进
                                                if (j == 0)
                                                {
                                                    IndentNumSpaces = (bomLevel - 1) * 10;//设置缩进2个Spaces,只是第一列缩进
                                                }
                                                else
                                                {
                                                    IndentNumSpaces = 0;//设置缩进2个Spaces,只是第一列缩进
                                                }

                                                //实际 64long型基本是ID 像单位，变成名称了。
                                                //if (kryptonTreeGridViewBOMDetail[j, i].ValueType == typeof(string))
                                                if (dcData.DataType.FullName == "System.String")
                                                {
                                                    if (kryptonTreeGridViewBOMDetail[j, i].Value != null)
                                                    {
                                                        if (kryptonTreeGridViewBOMDetail[j, i].FormattedValue != null)
                                                        {
                                                            excelSheet.Cells[currentRow, k + 1].Value = new string(' ', IndentNumSpaces) + kryptonTreeGridViewBOMDetail[j, i].FormattedValue.ToString();
                                                        }
                                                        else
                                                        {
                                                            excelSheet.Cells[currentRow, k + 1].Value = new string(' ', IndentNumSpaces) + kryptonTreeGridViewBOMDetail[j, i].Value.ToString();
                                                        }

                                                    }
                                                }
                                                //实际 64long型基本是ID 像单位，变成名称了。所有用显示模式的值
                                                else if (dcData.DataType.FullName == "System.Int64")
                                                {
                                                    excelSheet.Cells[currentRow, k + 1].Value = Common.CommonHelper.Instance.GetRealValueByDataType(kryptonTreeGridViewBOMDetail[j, i].ValueType, kryptonTreeGridViewBOMDetail[j, i].FormattedValue);
                                                }
                                                else if (dcData.DataType.FullName == "System.Decimal")
                                                {
                                                    excelSheet.Cells[currentRow, k + 1].Value = Common.CommonHelper.Instance.GetRealValueByDataType(dcData.DataType, kryptonTreeGridViewBOMDetail[j, i].Value);
                                                }
                                                else
                                                {
                                                    excelSheet.Cells[currentRow, k + 1].Value = Common.CommonHelper.Instance.GetRealValueByDataType(dcData.DataType, kryptonTreeGridViewBOMDetail[j, i].Value);

                                                }
                                                k++;
                                            }

                                        }
                                    }
                                }



                                // 添加主表汇总数据 占用了两行
                                AddSummaryInfo(excelSheet, currentRow + 1);


                                // 获取指定列,第一列设置宽一些
                                var column = excelSheet.Column(1);
                                if (MaxLevelList.Count > 0)
                                {
                                    // 设置列宽
                                    column.Width = MaxLevelList.Max() * 20;
                                }

                                // 保存更改
                                package.Save();
                                #endregion

                            }
                            catch (Exception ex)
                            {
                                MainForm.Instance.logger.LogError("Excel导出时出错！", ex.Message);
                                MessageBox.Show("导出数据出错，已忽略！");

                            }
                            finally
                            {
                                FileInfo fileInfo = new FileInfo(selectedFile);
                                package.SaveAs(fileInfo);
                                stopwatch.Stop();
                                // MessageBox.Show("导出数据成功！！！");
                            }

                            #endregion
                        }
                        else
                        {
                            #region 收起
                            try
                            {

                                // //第一行开始写BOM主表内容。

                                // 合并指定单元格
                                excelSheet.Cells[1, 1, 1, kryptonTreeGridViewBOMDetail.ColumnCount].Merge = true;

                                // 设置合并后单元格的内容
                                excelSheet.Cells[1, 1].Value = EditEntity.BOM_Name + "BOM清单：";
                                // 获取指定单元格
                                var cellTitle = excelSheet.Cells[1, 1];

                                // 设置单元格内容为加粗
                                cellTitle.Style.Font.Bold = true;
                                // 设置单元格内容的字体大小
                                cellTitle.Style.Font.Size = 28;
                                // 设置单元格内容居中对齐
                                cellTitle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cellTitle.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                #region



                                //生成字段名称
                                int k = 0;
                                for (int i = 0; i < kryptonTreeGridViewBOMDetail.ColumnCount; i++)
                                {
                                    if (kryptonTreeGridViewBOMDetail.Columns[i].Visible && !string.IsNullOrEmpty(kryptonTreeGridViewBOMDetail.Columns[i].HeaderText)) //不导出隐藏的列
                                    {
                                        //第二行开始写列名。
                                        excelSheet.Cells[2, k + 1].Value = kryptonTreeGridViewBOMDetail.Columns[i].HeaderText;
                                        // 获取指定单元格
                                        var cellHeader = excelSheet.Cells[2, k + 1];
                                        // 设置单元格内容为加粗
                                        cellHeader.Style.Font.Bold = true;
                                        k++;
                                    }
                                }

                                int currentRow = 2;//这里空出2行，
                                                   //填充数据
                                                   //这里行数直接算到数据行数，因为是树形结构
                                for (int i = 0; i < kryptonTreeGridViewBOMDetail.RowCount; i++)
                                {
                                    KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridViewBOMDetail.Rows[i] as KryptonTreeGridNodeRow;
                                    if (kryptonTreeGridNodeRow != null)
                                    {
                                        //当前行，
                                        currentRow++;//再空一行
                                        k = 0;
                                        for (int j = 0; j < kryptonTreeGridViewBOMDetail.ColumnCount; j++)
                                        {
                                            if (kryptonTreeGridViewBOMDetail.Columns[j].Visible && !string.IsNullOrEmpty(kryptonTreeGridViewBOMDetail.Columns[j].HeaderText)) //不导出隐藏的列
                                            {
                                                //实际 64long型基本是ID 像单位，变成名称了。
                                                if (kryptonTreeGridViewBOMDetail[j, i].ValueType == typeof(string))
                                                {
                                                    if (kryptonTreeGridViewBOMDetail[j, i].Value != null)
                                                    {
                                                        if (kryptonTreeGridViewBOMDetail[j, i].FormattedValue != null)
                                                        {
                                                            excelSheet.Cells[currentRow, k + 1].Value = kryptonTreeGridViewBOMDetail[j, i].FormattedValue.ToString();
                                                        }
                                                        else
                                                        {
                                                            excelSheet.Cells[currentRow, k + 1].Value = kryptonTreeGridViewBOMDetail[j, i].Value.ToString();
                                                        }

                                                    }
                                                }

                                                else
                                                {
                                                    //这里是准备要判断是不是数字，加' 暂时没有处理
                                                    if (kryptonTreeGridViewBOMDetail[j, i].FormattedValue != null)
                                                    {
                                                        //excelSheet.Cells[currentRow, k + 1].Value = kryptonTreeGridViewBOMDetail[j, i].FormattedValue.ToString();
                                                        excelSheet.Cells[currentRow, k + 1].Value = Common.CommonHelper.Instance.GetRealValueByDataType(kryptonTreeGridViewBOMDetail[j, i].ValueType, kryptonTreeGridViewBOMDetail[j, i].FormattedValue);
                                                    }
                                                    else
                                                    {
                                                        //excelSheet.Cells[currentRow, k + 1].Value = kryptonTreeGridViewBOMDetail[j, i].Value.ToString();
                                                        excelSheet.Cells[currentRow, k + 1].Value = Common.CommonHelper.Instance.GetRealValueByDataType(kryptonTreeGridViewBOMDetail[j, i].ValueType, kryptonTreeGridViewBOMDetail[j, i].Value);

                                                    }
                                                }
                                                k++;
                                            }

                                        }

                                        //如果他有子集行则另处理,
                                        if (kryptonTreeGridNodeRow.HasChildren)
                                        {
                                            var row = excelSheet.Row(currentRow);
                                            // 设置行的背景色
                                            row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            row.Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);

                                            currentRow = FillCellsloop(excelSheet, currentRow, kryptonTreeGridNodeRow, 1);
                                        }

                                    }

                                }
                                #endregion

                            }
                            catch (Exception ex)
                            {
                                MainForm.Instance.logger.LogError("Excel导出时出错！", ex.Message);

                            }
                            finally
                            {
                                FileInfo fileInfo = new FileInfo(selectedFile);
                                package.SaveAs(fileInfo);
                                // MessageBox.Show("导出数据成功！！！");

                            }

                            #endregion

                        }
                    }

                    if (MessageBox.Show($"成功导出 BOM数据，耗时 {stopwatch.Elapsed.TotalSeconds:F2} 秒。\n是否立即打开文件？",
                            "导出完成", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(selectedFile) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {


            }

            return rs;
        }


        #endregion




        /// <summary>
        /// 子集中，比方类型，在上级是用grid formating事件查出来的，这里需要递归填充
        /// 如果强制将树展开，也可以显示
        /// </summary>
        /// <param name="excelSheet"></param>
        /// <param name="currentRow"></param>
        /// <param name="Parentnode"></param>
        /// <param name="bomLevel"></param>
        /// <returns></returns>
        private int FillCellsloop(ExcelWorksheet excelSheet, int currentRow, KryptonTreeGridNodeRow Parentnode, int bomLevel)
        {
            int IndentNumSpaces = bomLevel * 10;//设置缩进2个Spaces
            foreach (KryptonTreeGridNodeRow node in Parentnode.Nodes)
            {
                currentRow++;
                int k = 0;
                for (int j = 0; j < kryptonTreeGridViewBOMDetail.ColumnCount; j++)
                {
                    if (kryptonTreeGridViewBOMDetail.Columns[j].Visible && !string.IsNullOrEmpty(kryptonTreeGridViewBOMDetail.Columns[j].HeaderText)) //不导出隐藏的列
                    {
                        //实际 64long型基本是ID 像单位，变成名称了。
                        //node.Cells[j].FormattedValue
                        //实际 64long型基本是ID 像单位，变成名称了。
                        if (node.Cells[j].ValueType == typeof(string))
                        {
                            #region 给单元格赋值
                            if (node.Cells[j].Value != null)
                            {
                                if (node.Cells[j].FormattedValue != null)
                                {
                                    // 在单元格内容前面添加缩进空格
                                    if (k == 0)
                                    {
                                        excelSheet.Cells[currentRow, k + 1].Value = new string(' ', IndentNumSpaces) + node.Cells[j].FormattedValue.ToString();
                                    }
                                    else
                                    {
                                        excelSheet.Cells[currentRow, k + 1].Value = new string(' ', IndentNumSpaces) + node.Cells[j].FormattedValue.ToString();
                                    }

                                }
                                else
                                {
                                    // 在单元格内容前面添加缩进空格
                                    if (k == 0)
                                    {
                                        excelSheet.Cells[currentRow, k + 1].Value = new string(' ', IndentNumSpaces) + node.Cells[j].Value.ToString();
                                    }
                                    else
                                    {
                                        excelSheet.Cells[currentRow, k + 1].Value = new string(' ', IndentNumSpaces) + node.Cells[j].Value.ToString();
                                    }
                                }
                            }
                            #endregion

                        }

                        else
                        {
                            //这里是准备要判断是不是数字，加' 暂时没有处理
                            #region 给单元格赋值
                            if (node.Cells[j].Value != null)
                            {
                                if (node.Cells[j].FormattedValue != null)
                                {
                                    // 在单元格内容前面添加缩进空格
                                    if (k == 0)
                                    {
                                        excelSheet.Cells[currentRow, k + 1].Value = new string(' ', IndentNumSpaces) + node.Cells[j].FormattedValue.ToString();
                                    }
                                    else
                                    {
                                        excelSheet.Cells[currentRow, k + 1].Value = node.Cells[j].FormattedValue.ToString();
                                    }

                                }
                                else
                                {
                                    // 在单元格内容前面添加缩进空格
                                    if (k == 0)
                                    {
                                        excelSheet.Cells[currentRow, k + 1].Value = new string(' ', IndentNumSpaces) + node.Cells[j].Value.ToString();
                                    }
                                    else
                                    {
                                        excelSheet.Cells[currentRow, k + 1].Value = node.Cells[j].Value.ToString();
                                    }
                                }
                            }
                            #endregion
                        }

                        k++;
                    }
                }
                if (node.HasChildren)
                {
                    bomLevel++;
                    var row = excelSheet.Row(currentRow);
                    // 设置行的背景色
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.HotPink);
                    currentRow = FillCellsloopNew(excelSheet, currentRow, node, bomLevel);
                }
            }
            return currentRow;
        }
        // 在类开始处添加：
        private static IEntityCacheManager _cacheManager;
        private static IEntityCacheManager CacheManager => _cacheManager ?? (_cacheManager = Startup.GetFromFac<IEntityCacheManager>());

        private DataTable TransToDataTableByTreeAsync(tb_BOM_S entity)
        {
            //主要业务表的列头
            ConcurrentDictionary<string, string> colNames = UIHelper.GetFieldNameList<tb_BOM_SDetail>();
            //必须添加这两个列进DataTable,后面会通过这两个列来树型结构
            colNames.TryAdd("ID", "ID");
            colNames.TryAdd("ParentId", "上级ID");

            //要排除的列头
            List<Expression<Func<BaseProductInfo, object>>> expressions = new List<Expression<Func<BaseProductInfo, object>>>();
            expressions.Add(c => c.ProductNo);

            //基本信息的列头  这里要取主产品明细的主键。作为业务主键关联
            ConcurrentDictionary<string, string> BaseProductInfoColNames = UIHelper.GetFieldNameList<BaseProductInfo>(true)
                .exclude<BaseProductInfo>(expressions);


            //得到所有的BOM明细
            List<tb_BOM_SDetailTree> TreeList = GetNextBOMInfo(entity.BOM_ID);


            //通过BOM明细对应的料号去找到基本信息。
            var _detail_ids = TreeList.Select(x => new { x.ProdDetailID }).ToList();
            List<long> longids = new List<long>();
            foreach (var item in _detail_ids)
            {
                if (!longids.Contains(item.ProdDetailID))
                {
                    longids.Add(item.ProdDetailID);
                }
            }


            //设计关联列和目标列
            View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            List<View_ProdDetail> ViewProdlist = new List<View_ProdDetail>();
            ViewProdlist = MainForm.Instance.AppContext.Db.CopyNew().Queryable<View_ProdDetail>()
                .Where(m => longids.ToArray().Contains(m.ProdDetailID)).ToList();

            //将产品详情转换为基本信息列表
            List<BaseProductInfo> BaseProductInfoList = MainForm.Instance.mapper.Map<List<BaseProductInfo>>(ViewProdlist);
            for (int i = 0; i < BaseProductInfoList.Count; i++)
            {
                var item = BaseProductInfoList[i];
                item.TypeName = CacheManager.GetEntity<tb_ProductType>(item.Type_ID).TypeName;
            }

            //合并的实体中有指定的业务主键关联，不然无法给值
            DataTable dtAll = TreeList.ToDataTable<BaseProductInfo, tb_BOM_SDetailTree>(BaseProductInfoList, BaseProductInfoColNames, colNames, c => c.ProdDetailID);
            return dtAll;

            //基本信息+BOM详情 动态合并
            //Type combinedType = Common.EmitHelper.MergeTypes(true, typeof(BaseProductInfo), typeof(tb_BOM_SDetail));
            // DataTable dt = GetTreeDataToUI(EditEntity, combinedType);
            //object SubBomInfo = Activator.CreateInstance(combinedType);

        }


        /// <summary>
        /// 获取下一级bom需要的原料ok
        /// </summary>
        /// <param name="NeedQuantity">需要的数量</param>
        /// <param name="RequirementDate">需要的日期</param>
        /// <param name="PID">父级ID</param>
        /// <param name="BOM_ID">父级bom的ID,由这个查出BOM详情的相关子组件</param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public List<tb_BOM_SDetailTree> GetNextBOMInfo(long BOM_ID, long PID = 0)
        {
            List<tb_BOM_SDetailTree> drs = new List<tb_BOM_SDetailTree>();
            List<tb_BOM_SDetail> bomDetails = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_SDetail>()
                    .Includes(c => c.tb_proddetail, d => d.tb_bom_s)
                    //.Includes(c => c.tb_proddetail, d => d.tb_prod)
                    //.Includes(c => c.view_ProdDetail)
                    // .Includes(c => c.tb_proddetail, d => d.tb_Inventories)
                    //   .Includes(c => c.tb_bom_s)
                    .Where(c => c.BOM_ID == BOM_ID).ToList();
            foreach (tb_BOM_SDetail detail in bomDetails)
            {
                tb_BOM_SDetailTree node = new tb_BOM_SDetailTree();
                node = MainForm.Instance.mapper.Map<tb_BOM_SDetailTree>(detail);
                node.ParentId = BOM_ID;
                long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                node.ID = sid;
                node.ParentId = PID;
                if (detail.tb_proddetail.BOM_ID.HasValue)
                {
                    var nextSublist = GetNextBOMInfo(detail.tb_proddetail.BOM_ID.Value, node.ID);
                    drs.AddRange(nextSublist);
                }

                drs.Add(node);
            }
            return drs;
        }







        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_BOM_S, actionStatus);
        }



        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblReview.Text = "";
            //DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            //DataBindingHelper.InitDataToCmb<tb_Files>(k => k.Doc_ID, v => v.FileName, cmbDoc_ID);
            //DataBindingHelper.InitDataToCmb<tb_BOMConfigHistory>(k => k.BOM_S_VERID, v => v.VerNo, cmbBOM_S_VERID);
            //DataBindingHelper.InitDataToCmb<tb_OutInStockType>(k => k.Type_ID, v => v.TypeName, cmbType_ID, c => c.OutIn == true);
            DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v => v.TypeName, cmbType);
            txtSpec.Enabled = false;
            txtProp.Enabled = false;
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_BOM_S).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public async override void BindData(tb_BOM_S entity, ActionStatus actionStatus)
        {

            if (entity == null)
            {
                return;
            }
            EditEntity = entity;
            if (EditEntity.BOM_ID > 0 || actionStatus == ActionStatus.加载)
            {
                EditEntity.PrimaryKeyID = EditEntity.BOM_ID;
                EditEntity.ActionStatus = ActionStatus.加载;

                if (EditEntity.ProdDetailID > 0)
                {
                    if (EditEntity.view_ProdInfo == null)
                    {
                        var detail = MainForm.Instance.AppContext.Db.Queryable<View_ProdInfo>().Where(c => c.ProdDetailID == EditEntity.ProdDetailID).Single();
                        EditEntity.view_ProdInfo = detail;
                    }
                    txtProdDetailID.Text = EditEntity.SKU.ToString();
                    txtSpec.Text = EditEntity.view_ProdInfo.Specifications;
                    txtProp.Text = EditEntity.view_ProdInfo.prop;
                    cmbType.SelectedValue = EditEntity.view_ProdInfo.Type_ID;
                    BindToTree(EditEntity);
                }
                if (EditEntity.tb_BOM_SDetails != null && EditEntity.tb_BOM_SDetails.Count > 0)
                {
                    foreach (var item in EditEntity.tb_BOM_SDetails)
                    {
                        //高级查询出来的就是这样。如果列表查询至少为0
                        if (item.tb_BOM_SDetailSubstituteMaterials == null)
                        {
                            item.tb_BOM_SDetailSubstituteMaterials = await MainForm.Instance.AppContext.Db.Queryable<tb_BOM_SDetailSubstituteMaterial>().Where(c => c.SubID == item.SubID).ToListAsync();
                        }
                    }
                }
            }
            else
            {
                txtProdDetailID.ToolTipValues.Heading = "请选择物料编码，不能手动输入SKU码";
                EditEntity.DataStatus = (int)DataStatus.草稿;
                EditEntity.ActionStatus = ActionStatus.新增;

                if (string.IsNullOrEmpty(entity.BOM_No))
                {
                    entity.BOM_No = ClientBizCodeService.GetBizBillNo(BizType.BOM物料清单);
                }
                EditEntity.Effective_at = System.DateTime.Now;
                EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                EditEntity.ApprovalResults = false;
                EditEntity.is_enabled = true;
                EditEntity.is_available = true;
                EditEntity.ProdDetailID = 0;
                EditEntity.SKU = string.Empty;
                EditEntity.BOM_ID = 0;
                //EditEntity.BOM_S_VERID = 0;
                //EditEntity.Doc_ID = 0;
                EditEntity.BOM_Name = string.Empty;
                EditEntity.TotalMaterialQty = 0;
                BusinessHelper.Instance.InitEntity(EditEntity);
                EditEntity.property = string.Empty;
                if (EditEntity.tb_BOM_SDetails != null && EditEntity.tb_BOM_SDetails.Count > 0)
                {
                    EditEntity.tb_BOM_SDetails.ForEach(c => c.BOM_ID = 0);
                    EditEntity.tb_BOM_SDetails.ForEach(c => c.SubID = 0);
                }
            }

            /*****/
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.TotalMaterialQty, txtTotalMaterialQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.BOM_Name, txtBOM_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.property, txtProp, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(EditEntity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee);
            DataBindingHelper.BindData4Cmb<tb_Files>(EditEntity, k => k.Doc_ID, v => v.FileName, cmbDoc_ID);
            DataBindingHelper.BindData4Cmb<tb_BOMConfigHistory>(EditEntity, k => k.BOM_S_VERID, v => v.VerNo, cmbBOM_S_VERID);
            DataBindingHelper.BindData4DataTime<tb_BOM_S>(EditEntity, t => t.Effective_at, dtpEffective_at, false);
            DataBindingHelper.BindData4CheckBox<tb_BOM_S>(EditEntity, t => t.is_enabled, chkis_enabled, false);
            DataBindingHelper.BindData4CheckBox<tb_BOM_S>(EditEntity, t => t.is_available, chkis_available, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.OutApportionedCost.ToString(), txtOutApportionedCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.SelfApportionedCost.ToString(), txtSelfApportionedCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.TotalOutManuCost.ToString(), txtTotalOutManuCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.TotalSelfManuCost.ToString(), txtTotalSelfManuCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.OutProductionAllCosts.ToString(), txtOutProductionAllCosts, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.SelfProductionAllCosts.ToString(), txtSelfProductionAllCosts, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.OutputQty.ToString(), txtOutputQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.PeopleQty.ToString(), txtPeopleQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.WorkingHour.ToString(), txtWorkingHour, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.MachineHour.ToString(), txtMachineHour, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_BOM_S>(EditEntity, t => t.ExpirationDate, dtpExpirationDate, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.DailyQty.ToString(), txtDailyQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);


            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            DataBindingHelper.BindData4ControlByEnum<tb_BOM_S>(EditEntity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_BOM_S>(EditEntity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (EditEntity.tb_BOM_SDetails == null)
            {
                EditEntity.tb_BOM_SDetails = new List<tb_BOM_SDetail>();
            }
            sgh.LoadItemDataToGrid<tb_BOM_SDetail>(grid1, sgd, EditEntity.tb_BOM_SDetails, c => c.ProdDetailID);

            SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();
            toolTipController.ToolTipTitle = "有替代料";
            toolTipController.ToolTipIcon = ToolTipIcon.Info;
            toolTipController.IsBalloon = true;

            EditEntity.tb_BOM_SDetails.ForEach(x =>
                {
                    #region 如果有替换料的则标记出来
                    if (x.tb_BOM_SDetailSubstituteMaterials.Count > 0)
                    {
                        //添加右键控制器？
                        //grid[0, i].Controller.AddController(popupMenuForDelete);
                        foreach (GridRow grow in grid1.Rows)
                        {
                            if (grow.RowData != null)
                            {
                                long subid = grow.RowData.GetPropertyValue<tb_BOM_SDetail>(x => x.SubID).ToLong();
                                if (subid > 0 && x.SubID == subid)
                                {
                                    grid1[grow.Range.Start.Row, 0].View.ForeColor = Color.Red;
                                    grid1[grow.Range.Start.Row, 0].View.Font = new Font("Arial", 10, FontStyle.Bold);
                                    grid1[grow.Range.Start.Row, 0].AddController(toolTipController);
                                }

                            }

                        }

                    }

                    #endregion
                }

            );



            //加载时清空
            sgh2.LoadItemDataToGrid<tb_BOM_SDetailSubstituteMaterial>(gridSubstituteMaterial, sgd2, new List<tb_BOM_SDetailSubstituteMaterial>(), c => c.ProdDetailID);

            //先绑定这个。InitFilterForControl 这个才生效, 一共三个来控制，这里分别是绑定ID和SKU。下面InitFilterForControlByExp 是生成快捷按钮
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(EditEntity, k => k.SKU, txtProdDetailID, BindDataType4TextBox.Text, true);

            //如果属性变化 则状态为修改
            EditEntity.PropertyChanged += async (sender, s2) =>
            {
                //权限允许,草稿新建或修改状态时，才允许修改（修改要未审核或审核未通过）
                if (((true && EditEntity.DataStatus == (int)DataStatus.草稿) ||
                (true && EditEntity.DataStatus == (int)DataStatus.新建)))
                {
                    if (EditEntity.ProdDetailID > 0 &&
                    (s2.PropertyName == EditEntity.GetPropertyName<tb_BOM_S>(c => c.ProdDetailID) ||
                    s2.PropertyName == EditEntity.GetPropertyName<tb_BOM_S>(c => c.SKU)))
                    {
                        //视图来的，没有导航查询到相关数据
                        //find bomid
                        kryptonTreeView1.TreeView.Nodes.Clear();
                        //BaseController<tb_ProdDetail> ctrProdDetail = Startup.GetFromFacByName<BaseController<tb_ProdDetail>>(typeof(tb_ProdDetail).Name + "Controller");
                        //var bomprod = await ctrProdDetail.BaseQueryByIdAsync(EditEntity.ProdDetailID);
                        BaseController<View_ProdInfo> ctrProdDetail = Startup.GetFromFacByName<BaseController<View_ProdInfo>>(typeof(View_ProdInfo).Name + "Controller");
                        var vp = await ctrProdDetail.BaseQueryByIdAsync(EditEntity.ProdDetailID);
                        if (vp.BOM_ID != null)
                        {
                            if ((true && EditEntity.DataStatus == (int)DataStatus.草稿) || (true && EditEntity.DataStatus == (int)DataStatus.新建))
                            {
                                //如果这个产品有配方了 
                                if (MessageBox.Show("当前选中产品已经有配方,你确定需要对这个母件增加新的配方吗？\r\n如果是，则会更新当前母件的默认配方为您增加的配方", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                                {
                                    MainForm.Instance.uclog.AddLog("当前选中产品已经有配方", UILogType.错误);
                                    return;
                                }



                            }
                            else
                            {
                                BindToTree(EditEntity);
                            }
                        }
                        //加载母件关联的显示用的数据
                        txtSpec.Text = vp.Specifications;
                        EditEntity.property = vp.prop;
                        if (!string.IsNullOrEmpty(vp.prop))
                        {
                            EditEntity.BOM_Name = vp.CNName + "-" + vp.prop;
                        }
                        else
                        {
                            EditEntity.BOM_Name = vp.CNName;
                        }

                        EditEntity.SKU = vp.SKU;

                        cmbType.SelectedValue = vp.Type_ID;


                    }
                }


                EditEntity.OutProductionAllCosts = EditEntity.TotalMaterialCost + EditEntity.TotalOutManuCost + EditEntity.OutApportionedCost;
                EditEntity.SelfProductionAllCosts = EditEntity.TotalMaterialCost + EditEntity.TotalSelfManuCost + EditEntity.SelfApportionedCost;


            };
            if (EditEntity.tb_BOM_SDetails == null)
            {
                EditEntity.tb_BOM_SDetails = new List<tb_BOM_SDetail>();
            }
            //如果明细中包含了母件。就是死循环。不能用树来显示
            if (EditEntity.tb_BOM_SDetails.Any(c => c.ProdDetailID == EditEntity.ProdDetailID))
            {
                MessageBox.Show("BOM明细中包含了母件，请修复这个致命数据错误。系统已经跳过树形显示。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadTreeData(TransToDataTableByTreeAsync(EditEntity));

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            //这样在新增加和修改时才会触发添加母件的快捷按钮
            if ((EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改))
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_BOM_SValidator>(), kryptonSplitContainer1.Panel1.Controls);
                //  base.InitEditItemToControl(EditEntity, kryptonPanel1.Controls);
                //  base.InitFilterForControl<View_ProdDetail, View_ProdDetailQueryDto>(EditEntity, txtProdDetailID, c => c.CNName);

                //创建表达式  草稿 结案 和没有提交的都不显示
                var lambdaOrder = Expressionable.Create<View_ProdDetail>()
                                // .And(t => t.DataStatus == (int)DataStatus.确认)
                                .ToExpression();//注意 这一句 不能少

                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_ProdDetail).Name + "Processor");
                QueryFilter queryFilterC = baseProcessor.GetQueryFilter();

                ///视图指定成实体表，为了显示关联数据
                //DataBindingHelper.InitFilterForControlByExp<View_ProdDetail>(EditEntity, txtProdDetailID, c => c.SKU, queryFilterC, typeof(tb_Prod));

                ControlBindingHelper.ConfigureControlFilter<tb_BOM_S, View_ProdDetail>(entity, txtProdDetailID, t => t.SKU,
              f => f.SKU, queryFilterC, a => a.ProdDetailID, b => b.ProdDetailID, null, false);
            }
            base.BindData(entity);
        }

        protected override tb_BOM_S AddByCopy()
        {
            if (EditEntity == null)
            {
                MessageBox.Show("请先选择一个BOM清单作为复制的基准。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            EditEntity = base.AddByCopy();
            EditEntity.ActionStatus = ActionStatus.新增;
            EditEntity.BOM_ID = 0;
            EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
            EditEntity.DataStatus = (int)DataStatus.草稿;
            EditEntity.Approver_at = null;
            EditEntity.ProdDetailID = 0;
            EditEntity.SKU = null;
            EditEntity.BOM_No = ClientBizCodeService.GetBizBillNo(BizType.BOM物料清单);
            EditEntity.PrimaryKeyID = 0;
            BusinessHelper.Instance.InitEntity(EditEntity);
            foreach (var item in EditEntity.tb_BOM_SDetails)
            {
                item.BOM_ID = 0;
                item.SubID = 0;
            }
            if (EditEntity.tb_BOM_SDetailSecondaries != null)
            {
                foreach (var item in EditEntity.tb_BOM_SDetailSecondaries)
                {
                    item.BOM_ID = 0;
                    item.SecID = 0;
                }
            }

            return EditEntity;
        }

        private void BindToTree(tb_BOM_S _bom)
        {
            BOM_Level = 1;
            tb_BOM_SController<tb_BOM_S> ctrBOM = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
            /// var bom = await ctrBOM.BaseQueryByIdNavAsync(entity.MainID);
            tb_BOM_S bom = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>()
            .RightJoin<tb_ProdDetail>((a, b) => a.ProdDetailID == b.ProdDetailID)
             .Includes(b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
             // .Includes(a => a.tb_producttype)
             .Includes(a => a.view_ProdInfo)
            .Includes(a => a.tb_BOM_SDetails, b => b.tb_BOM_SDetailSubstituteMaterials)
            .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
            .Includes(a => a.tb_BOM_SDetails, b => b.view_ProdInfo)
            .Where(a => a.BOM_ID == _bom.BOM_ID)
           .Single();

            //UIPordBOMHelper.BindToTreeView(bom, bom.tb_BOM_SDetails, kryptonTreeView1.TreeView);
            treeListView1.Items.Clear();
            AddItems(bom);
            treeListView1.ExpandAll();
        }


        int BOM_Level = 1;

        private void AddItems(tb_BOM_S bom)
        {
            if (bom == null)
            {
                return;
            }
            TreeListViewItem itemRow = new TreeListViewItem(bom.BOM_Name, 0);
            itemRow.Tag = bom;
            itemRow.SubItems.Add(bom.property); //subitems只是从属于itemRow的子项。目前是四列
                                                ////一定会有值
                                                //tb_BOM_S bOM_S = listboms.Where(c => c.ProdDetailID == row.ProdDetailID).FirstOrDefault();
                                                //itemRow.SubItems[0].Tag = bOM_S;
            itemRow.SubItems.Add(bom.view_ProdInfo.Specifications);
            string prodType = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProductType), "Type_ID", bom.view_ProdInfo.Type_ID);

            itemRow.SubItems.Add(prodType);
            itemRow.SubItems.Add("产出量:" + bom.OutputQty.ToString());

            treeListView1.Items.Add(itemRow);
            Loadbom(bom, itemRow);
        }


        private void Loadbom(tb_BOM_S bOM_S, TreeListViewItem listViewItem)
        {
            if (bOM_S != null && bOM_S.tb_BOM_SDetails != null)
            {
                BOM_Level++;
                listViewItem.ImageIndex = 1;//如果有配方，则图标不一样
                foreach (var BOM_SDetail in bOM_S.tb_BOM_SDetails)
                {
                    TreeListViewItem itemSub = new TreeListViewItem(BOM_SDetail.view_ProdInfo.CNName, 0);
                    itemSub.Tag = bOM_S;
                    itemSub.SubItems.Add(BOM_SDetail.view_ProdInfo.prop);//subitems只是从属于itemRow的子项。目前是四列
                    itemSub.SubItems.Add(BOM_SDetail.SKU);//subitems只是从属于itemRow的子项。目前是四列
                    itemSub.SubItems.Add(BOM_SDetail.view_ProdInfo.Specifications);
                    string prodType = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProductType), "Type_ID", BOM_SDetail.view_ProdInfo.Type_ID);
                    itemSub.SubItems.Add(prodType);
                    itemSub.SubItems.Add(BOM_SDetail.UsedQty.ToString());

                    listViewItem.Items.Add(itemSub);

                    var bomsub = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>()
                   //.RightJoin<tb_ProdDetail>((a, b) => a.ProdDetailID == b.ProdDetailID)
                   // .Includes(b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                   .Includes(a => a.view_ProdInfo)
                   .Includes(a => a.tb_BOM_SDetails, b => b.tb_BOM_SDetailSubstituteMaterials)  //查出他的子级是不是BOM并且带出他的子项
                                                                                                //.Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                    .Includes(a => a.tb_BOM_SDetails, b => b.view_ProdInfo)
                   .Where(a => a.ProdDetailID == BOM_SDetail.ProdDetailID)
                   .ToList();
                    if (bomsub.Count > 0)
                    {
                        foreach (var item in bomsub)
                        {
                            //保存BOM的母件实体值
                            itemSub.SubItems[0].Tag = item;
                            if (BOM_Level < 6)
                            {
                                Loadbom(item, itemSub);
                            }
                            else
                            {
                                MessageBox.Show("BOM层级过深，已经超过5层。请检查数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        }
                    }


                }
            }
        }


        SourceGridDefine sgd = null;
        SourceGridDefine sgd2 = null;

        SourceGridHelper sgh = new SourceGridHelper();
        SourceGridHelper sgh2 = new SourceGridHelper();

        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UCStockIn_Load(object sender, EventArgs e)
        {
            LoadGrid1();
            LoadgridSubstituteMaterial();

        }


        private void LoadGrid1()
        {
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); 
            InitDataTocmbbox();


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SGDefineColumnItem> listCols = sgh.GetGridColumns<ProductSharePart, tb_BOM_SDetail>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<tb_BOM_SDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_BOM_SDetail>(c => c.SubID);
            listCols.SetCol_NeverVisible<tb_BOM_SDetail>(c => c.BOM_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.ShortCode);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            //实际在中间实体定义时加了只读属性，功能相同
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }

            // listCols.SetCol_ReadOnly<tb_BOM_SDetail>(c => c.Substitute);

            listCols.SetCol_Format<tb_BOM_SDetail>(c => c.OutputRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_BOM_SDetail>(c => c.LossRate, CustomFormatType.PercentFormat);
            listCols.SetCol_DefaultValue<tb_BOM_SDetail>(c => c.IsKeyMaterial, true);
            //排除指定列不在相关列内
            //listCols.SetCol_Exclude<ProductSharePart>(c => c.Location_ID);

            //具体审核权限的人才显示
            /*
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //  listCols.SetCol_NeverVisible<tb_BOM_S_Detail>(c => c.Cost);
            }
            */
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_BOM_SDetail>(c => c.UsedQty);
            listCols.SetCol_Summary<tb_BOM_SDetail>(c => c.SubtotalUnitCost);
            listCols.SetCol_Formula<tb_BOM_SDetail>((a, b) => a.UnitCost * b.UsedQty, c => c.SubtotalUnitCost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.Inv_Cost, t => t.UnitCost);

            //冗余名称和规格
            //  sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.CNName, t => t.SubItemName);
            //  sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.Specifications, t => t.SubItemSpec);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.prop, t => t.property);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.Type_ID, t => t.Type_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.Unit_ID, t => t.Unit_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.SKU, t => t.SKU);

            //由单位来决定转换率.!!!注意底层代码写死了
            //Unit_ID 是单位表主键，在换算表中是指向 Source_unit_id,但是保存到BOM详情T表中字段是换算表主键，作为外键
            sgh.SetCol_LimitedConditionsForSelectionRange<tb_BOM_SDetail>(sgd, t => t.Unit_ID, f => f.UnitConversion_ID);

            sgh.SetQueryItemToColumnPairs<View_ProdDetail, tb_BOM_SDetail>(sgd, f => f.BOM_ID, t => t.Child_BOM_Node_ID);

            //应该只提供一个结构
            List<tb_BOM_SDetail> lines = new List<tb_BOM_SDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            //要在初始化添加前
            sgh.OnAddRightClick += Sgh_OnAddRightClick;

            sgd.SetDependencyObject<ProductSharePart, tb_BOM_SDetail>(MainForm.Instance.View_ProdDetailList);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_BOM_SDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            sgh.OnLoadRelevantFields += Sgh_OnLoadRelevantFields;
        }

        private void Sgh_OnAddRightClick(PopupMenuCustomize pmc, Position position)
        {
            if (pmc != null)
            {
                //如果没有要的右键菜单。就在这里添加。AddReplaceMaterial
                if (!pmc.MyMenu.Items.ContainsKey("AddReplaceMaterial"))
                {
                    ToolStripMenuItem toolStrip = new ToolStripMenuItem($"添加替代料【" + position.Row + "】");
                    toolStrip.Tag = position.Row;
                    toolStrip.Name = "AddReplaceMaterial";
                    toolStrip.Size = new System.Drawing.Size(153, 26);
                    toolStrip.Click += toolStrip_Click;
                    pmc.MyMenu.Items.Add(toolStrip);
                }
            }
        }

        private void toolStrip_Click(object sender, EventArgs e)
        {
            int row = (int)((sender as ToolStripMenuItem).Tag);
            if (grid1.Rows[row].RowData != null)
            {
                if (grid1.Rows[row].RowData is tb_BOM_SDetail bOM_SDetail)
                {
                    kryptonHeaderGroup1.ValuesPrimary.Heading = bOM_SDetail.SKU + ":替代料明细";
                    kryptonHeaderGroup1.Tag = bOM_SDetail;
                    if (bOM_SDetail.tb_BOM_SDetailSubstituteMaterials == null)
                    {
                        bOM_SDetail.tb_BOM_SDetailSubstituteMaterials = new List<tb_BOM_SDetailSubstituteMaterial>();
                    }

                    BsSubstituteMaterial.Clear();
                    //清空
                    if (bOM_SDetail.tb_BOM_SDetailSubstituteMaterials.Count > 0)
                    {
                        SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();
                        toolTipController.ToolTipTitle = "有替代料";
                        toolTipController.ToolTipIcon = ToolTipIcon.Info;
                        toolTipController.IsBalloon = true;
                        if (grid1[row, 0].FindController(typeof(SourceGrid.Cells.Controllers.ToolTipText)) == null)
                        {
                            grid1[row, 0].Controller.AddController(toolTipController);
                            grid1[row, 0].View.ForeColor = Color.Red;
                            grid1[row, 0].View.Font = new Font("Arial", 10, FontStyle.Bold);
                        }
                    }
                    else
                    {
                        grid1[row, 0].View.ForeColor = Color.FromArgb(255, 0, 0, 0);
                        grid1[row, 0].View.Font = null;
                    }
                    sgh2.LoadItemDataToGrid<tb_BOM_SDetailSubstituteMaterial>(gridSubstituteMaterial, sgd2, bOM_SDetail.tb_BOM_SDetailSubstituteMaterials, c => c.ProdDetailID);
                }
                else
                {
                    kryptonHeaderGroup1.Tag = null;
                    kryptonHeaderGroup1.ValuesPrimary.Heading = "替代料明细";
                }
            }



        }


        //加载替代品明细
        private void LoadgridSubstituteMaterial()
        {
            InitDataTocmbbox();

            gridSubstituteMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            gridSubstituteMaterial.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = sgh2.GetGridColumns<ProductSharePart, tb_BOM_SDetailSubstituteMaterial>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<tb_BOM_SDetailSubstituteMaterial>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_BOM_SDetailSubstituteMaterial>(c => c.SubID);
            listCols.SetCol_NeverVisible<tb_BOM_SDetailSubstituteMaterial>(c => c.SubstituteMaterialID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.ShortCode);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            //实际在中间实体定义时加了只读属性，功能相同
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            listCols.SetCol_DefaultValue<tb_BOM_SDetailSubstituteMaterial>(c => c.IsKeyMaterial, true);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }

            listCols.SetCol_Format<tb_BOM_SDetailSubstituteMaterial>(c => c.OutputRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_BOM_SDetailSubstituteMaterial>(c => c.LossRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_BOM_SDetailSubstituteMaterial>(c => c.PriorityUseType, CustomFormatType.EnumOptions, null, typeof(PriorityUseType));
            //排除指定列不在相关列内
            //listCols.SetCol_Exclude<ProductSharePart>(c => c.Location_ID);

            //具体审核权限的人才显示
            /*
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //  listCols.SetCol_NeverVisible<tb_BOM_S_Detail>(c => c.Cost);
            }
            */
            sgd2 = new SourceGridDefine(gridSubstituteMaterial, listCols, true);
            sgd2.GridMasterData = EditEntity;
            //要放到初始化sgd2后面
            listCols.SetCol_Summary<tb_BOM_SDetailSubstituteMaterial>(c => c.UsedQty);
            listCols.SetCol_Summary<tb_BOM_SDetailSubstituteMaterial>(c => c.SubtotalUnitCost);
            listCols.SetCol_Formula<tb_BOM_SDetailSubstituteMaterial>((a, b) => a.UnitCost * b.UsedQty, c => c.SubtotalUnitCost);
            sgh2.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetailSubstituteMaterial>(sgd2, f => f.Inv_Cost, t => t.UnitCost);


            sgh2.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetailSubstituteMaterial>(sgd2, f => f.Unit_ID, t => t.Unit_ID);
            sgh2.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetailSubstituteMaterial>(sgd2, f => f.SKU, t => t.SKU);

            //由单位来决定转换率.!!!注意底层代码写死了
            //Unit_ID 是单位表主键，在换算表中是指向 Source_unit_id,但是保存到BOM详情T表中字段是换算表主键，作为外键
            sgh2.SetCol_LimitedConditionsForSelectionRange<tb_BOM_SDetailSubstituteMaterial>(sgd2, t => t.Unit_ID, f => f.UnitConversion_ID);

            sgh2.SetQueryItemToColumnPairs<View_ProdDetail, tb_BOM_SDetailSubstituteMaterial>(sgd2, f => f.BOM_ID, t => t.Child_BOM_Node_ID);




            //应该只提供一个结构
            List<tb_BOM_SDetailSubstituteMaterial> lines = new List<tb_BOM_SDetailSubstituteMaterial>();
            BsSubstituteMaterial.DataSource = lines;
            sgd2.BindingSourceLines = BsSubstituteMaterial;


            sgd2.SetDependencyObject<ProductSharePart, tb_BOM_SDetailSubstituteMaterial>(MainForm.Instance.View_ProdDetailList);
            sgd2.HasRowHeader = true;
            sgh2.InitGrid(gridSubstituteMaterial, sgd2, true, nameof(tb_BOM_SDetailSubstituteMaterial));

        }
        private void Sgh_OnLoadRelevantFields(object _View_ProdDetail, object rowObj, SourceGridDefine griddefine, Position Position)
        {
            View_ProdDetail vp = (View_ProdDetail)_View_ProdDetail;
            tb_BOM_SDetail _BOM_SDetail = (tb_BOM_SDetail)rowObj;
            //通过产品查询页查出来后引过来才有值，如果直接在输入框输入SKU这种唯一的。就没有则要查一次。这时是缓存了？
            if (vp.BOM_ID.HasValue)
            {
                if (vp.tb_bom_s == null)
                {
                    vp.tb_bom_s = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>().Where(a => a.BOM_ID == vp.BOM_ID).Single();
                }
                // _BOM_SDetail.SelfManuCost = vp.tb_bom_s.TotalSelfManuCost;
                // int ColIndex = griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_BOM_SDetail.SelfManuCost)).ColIndex;
                // griddefine.grid[Position.Row, ColIndex].Value = _BOM_SDetail.SelfManuCost;

                // MessageBox.Show("这里要调试确认成本来源方式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //如果子件只是材料。就直接取材料的进货成本。如果他有配方则加上其它加工费。
                //如果系统正常。带BOM配方的成本已经是加上了加工费的，缴库时算好的。所以这里不做处理了。
                //这里只是修正错误。如果有配方的并且目前成本为0，则改为他的BOM的总价格 先自制再外发。
                if (_BOM_SDetail.UnitCost == 0)
                {
                    // 如果 vp.tb_bom_s.SelfProductionAllCosts值为0时则用vp.OutProductionAllCosts.
                    _BOM_SDetail.UnitCost = vp.tb_bom_s.SelfProductionAllCosts == 0 ? vp.tb_bom_s.OutProductionAllCosts : vp.tb_bom_s.SelfProductionAllCosts;
                }

                var Col = griddefine.grid.Columns.GetColumnInfo(griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_BOM_SDetail.UnitCost)).UniqueId);
                if (Col != null)
                {
                    griddefine.grid[Position.Row, Col.Index].Value = _BOM_SDetail.UnitCost;
                }


                //Child_BOM_Node_ID 这个在明细中显示的是ID，没有使用外键关联显示，因为列名不一致。手动显示为配方名称
                var cbni = griddefine.grid.Columns.GetColumnInfo(griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_BOM_SDetail.Child_BOM_Node_ID)).UniqueId);
                if (cbni != null)
                {
                    griddefine.grid[Position.Row, cbni.Index].DisplayText = vp.tb_bom_s.BOM_Name;
                }


            }



        }

        protected async override Task<ReturnResults<tb_BOM_S>> Delete()
        {

            //删除配置前，如果其它地方没有使用，则产品默认指向了，在删除前将成品指向的配置清空
            var ctrbom = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
            ReturnResults<tb_BOM_S> rrs = await ctrbom.DeleteBOM_SDetail_Clear_ProdDetailMapping(EditEntity as tb_BOM_S);
            if (rrs.Succeeded)
            {
                MainForm.Instance.auditLogHelper.CreateAuditLog<tb_BOM_S>("删除", EditEntity);
            }
            return rrs;
        }

        private void Sgh_OnLoadMultiRowData(object rows, Position position)
        {

            List<View_ProdDetail> RowDetails = new List<View_ProdDetail>();
            var rowss = ((IEnumerable<dynamic>)rows).ToList();
            foreach (var item in rowss)
            {
                RowDetails.Add(item);
            }
            if (RowDetails != null)
            {
                List<tb_BOM_SDetail> details = new List<tb_BOM_SDetail>();

                foreach (var item in RowDetails)
                {
                    tb_BOM_SDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_BOM_SDetail>(item);
                    bOM_SDetail.UsedQty = 0;
                    details.Add(bOM_SDetail);
                }
                //List<tb_BOM_SDetail> details = mapper.Map<List<tb_BOM_SDetail>>(RowDetails);
                sgh.InsertItemDataToGrid<tb_BOM_SDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
            }


        }

        private void Sgh_OnCalculateColumnValue(object _rowObj, SourceGridDefine myGridDefine, SourceGrid.Position position)
        {
            Summation();
        }


        private void Summation()
        {
            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            try
            {
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_BOM_SDetail> details = sgd.BindingSourceLines.DataSource as List<tb_BOM_SDetail>;
                if (details == null) return;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                for (int i = 0; i < details.Count; i++)
                {
                    var detail = details[i];

                    decimal tempSubtotal = detail.UsedQty * detail.UnitCost;
                    tempSubtotal = Math.Round(tempSubtotal, MainForm.Instance.authorizeController.GetMoneyDataPrecision());

                    detail.SubtotalUnitCost = tempSubtotal;
                }
                EditEntity.TotalMaterialCost = details.Sum(c => c.SubtotalUnitCost);
                EditEntity.TotalMaterialQty = details.Sum(c => c.UsedQty);
                EditEntity.OutProductionAllCosts = EditEntity.TotalMaterialCost + EditEntity.TotalOutManuCost + EditEntity.OutApportionedCost;
                EditEntity.SelfProductionAllCosts = EditEntity.TotalMaterialCost + EditEntity.TotalSelfManuCost + EditEntity.SelfApportionedCost;
            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_BOM_SDetail> details = new List<tb_BOM_SDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtBOM_Name);
            bindingSourceSub.EndEdit();
            List<tb_BOM_SDetail> detailentity = bindingSourceSub.DataSource as List<tb_BOM_SDetail>;
            Summation();
            //删除明细的机制应该是  ROM框架中 先全部删除明细再新增。说是性能好。
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();


                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0 && NeedValidated)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }



                //副产出暂时不做。为了不验证给一行空值，验证后清除掉
                EditEntity.tb_BOM_SDetailSecondaries = new List<tb_BOM_SDetailSecondary>();
                EditEntity.tb_BOM_SDetailSecondaries.Add(new tb_BOM_SDetailSecondary());
                EditEntity.DataStatus = (int)DataStatus.草稿;
                if (NeedValidated && EditEntity.is_enabled && !EditEntity.is_enabled)
                {
                    if (MessageBox.Show("BOM清单没有启用，确定保存吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                    {
                        return false;
                    }
                }


                if (NeedValidated && EditEntity.is_available && !EditEntity.is_available)
                {
                    if (MessageBox.Show("BOM清单设置为不可用，确定保存吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                    {
                        return false;
                    }
                }


                EditEntity.tb_BOM_SDetails = details;
                Summation();
                var maxCost = Math.Max(EditEntity.OutProductionAllCosts, EditEntity.SelfProductionAllCosts);
                //检测总成本
                if (NeedValidated && maxCost == 0)
                {
                    MessageBox.Show("配方总成本不能为零。请检查数据后再保存。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_BOM_SDetail>(details))
                {
                    return false;
                }

                //BOM子件中不能包含目标母件本身。
                if (NeedValidated && details.Any(c => c.ProdDetailID == EditEntity.ProdDetailID))
                {
                    MessageBox.Show("BOM子件中不能包含目标母件本身。请检查数据后再保存。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }


                EditEntity.tb_BOM_SDetailSecondaries.Clear();

                tb_BOM_SController<tb_BOM_S> ctr = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
                ReturnMainSubResults<tb_BOM_S> SaveResult = new ReturnMainSubResults<tb_BOM_S>();
                if (NeedValidated)
                {
                    SaveResult = await ctr.SaveOrUpdateWithChild<tb_BOM_S>(EditEntity); //await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {

                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.BOM_No}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;
            }

            return false;

        }

        private void LoadTreeData(DataTable dtAll)
        {
            kryptonTreeGridViewBOMDetail.DataSource = null;
            kryptonTreeGridViewBOMDetail.GridNodes.Clear();
            kryptonTreeGridViewBOMDetail.GridNodes.Clear();
            kryptonTreeGridViewBOMDetail.Columns.Clear();
            kryptonTreeGridViewBOMDetail.FontParentBold = true;
            kryptonTreeGridViewBOMDetail.UseParentRelationship = true;

            dtAll.DefaultView.Sort = "ParentId";
            dtAll = dtAll.DefaultView.ToTable();

            //注意指定这个ID和父ID，为了树型结构的展现
            kryptonTreeGridViewBOMDetail.IdColumnName = "ID";
            ////注意指定这个ID和父ID，为了树型结构的展现
            kryptonTreeGridViewBOMDetail.ParentIdColumnName = "ParentId";

            kryptonTreeGridViewBOMDetail.ParentIdRootValue = 0;

            //排第一列
            kryptonTreeGridViewBOMDetail.GroupByColumnIndex = dtAll.Columns.IndexOf("CNName");

            kryptonTreeGridViewBOMDetail.IsOneLevel = true;
            kryptonTreeGridViewBOMDetail.HideColumns.Clear();
            //要在datatable的列中，可以不显示出来。因为只是一个结构显示
            kryptonTreeGridViewBOMDetail.SetHideColumns(kryptonTreeGridViewBOMDetail.IdColumnName);
            kryptonTreeGridViewBOMDetail.SetHideColumns(kryptonTreeGridViewBOMDetail.ParentIdColumnName);
            kryptonTreeGridViewBOMDetail.SetHideColumns<tb_BOM_SDetailTree>(c => c.ProdDetailID);
            kryptonTreeGridViewBOMDetail.SetHideColumns<tb_BOM_SDetailTree>(c => c.BOM_ID);
            kryptonTreeGridViewBOMDetail.SetHideColumns<BaseProductInfo>(c => c.Type_ID);

            kryptonTreeGridViewBOMDetail.DataSource = dtAll;
            kryptonTreeGridViewBOMDetail.Columns[kryptonTreeGridViewBOMDetail.IdColumnName].Visible = false;
            kryptonTreeGridViewBOMDetail.ExpandAll();
        }


        protected async override Task<ReviewResult> Review()
        {
            ReviewResult reviewResult = await base.Review();
            if (reviewResult.approval.ApprovalResults)
            {
                UIPordBOMHelper.BindToTreeViewNoRootNode(EditEntity.tb_BOM_SDetails, kryptonTreeView1.TreeView);
            }
            return reviewResult;
        }


        private void kryptonSplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDic { get => _DataDictionary; set => _DataDictionary = value; }


        private void kryptonTreeGridViewBOMDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!kryptonTreeGridViewBOMDetail.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //图片特殊处理
            if (kryptonTreeGridViewBOMDetail.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                    return;
                }
            }

            //固定字典值显示
            string colDbName = kryptonTreeGridViewBOMDetail.Columns[e.ColumnIndex].Name;
            if (ColNameDataDic.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDic.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }
                }
            }

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_Prod>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }


        private void gridSubstituteMaterial_Validated(object sender, EventArgs e)
        {
            if (kryptonHeaderGroup1.Tag != null)
            {
                if (kryptonHeaderGroup1.Tag is tb_BOM_SDetail detail)
                {

                    List<tb_BOM_SDetailSubstituteMaterial> RefurbishedMaterials = BsSubstituteMaterial.DataSource as List<tb_BOM_SDetailSubstituteMaterial>;

                    //产品ID有值才算有效值
                    List<tb_BOM_SDetailSubstituteMaterial> LastRefurbishedMaterials = RefurbishedMaterials.Where(t => t.ProdDetailID > 0).ToList();
                    var bb = LastRefurbishedMaterials.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (bb.Count > 1)
                    {
                        System.Windows.Forms.MessageBox.Show("替换料明细中，不能有相同重复的代替项!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gridSubstituteMaterial.Focus(true);
                    }
                    //修改式添加替换料时
                    if (detail.SubID > 0)
                    {
                        LastRefurbishedMaterials.ForEach(c => c.SubID = detail.SubID);
                    }
                    detail.tb_BOM_SDetailSubstituteMaterials = LastRefurbishedMaterials;
                }
            }

        }


        private void grid1_DoubleClick(object sender, EventArgs e)
        {
            if (sender is SourceGrid.Grid BomDetailGrid)
            {
                if (BomDetailGrid.Selection != null && BomDetailGrid.Selection.ActivePosition.Row != -1)
                {
                    if (grid1.Rows[BomDetailGrid.Selection.ActivePosition.Row].RowData != null)
                    {
                        if (grid1.Rows[BomDetailGrid.Selection.ActivePosition.Row].RowData is tb_BOM_SDetail bOM_SDetail)
                        {
                            kryptonHeaderGroup1.ValuesPrimary.Heading = bOM_SDetail.SKU + ":替代料明细";
                            kryptonHeaderGroup1.Tag = bOM_SDetail;
                            if (bOM_SDetail.tb_BOM_SDetailSubstituteMaterials == null)
                            {
                                bOM_SDetail.tb_BOM_SDetailSubstituteMaterials = new List<tb_BOM_SDetailSubstituteMaterial>();
                            }

                            BsSubstituteMaterial.Clear();
                            //清空
                            if (bOM_SDetail.tb_BOM_SDetailSubstituteMaterials.Count > 0)
                            {
                                SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();
                                toolTipController.ToolTipTitle = "有替代料";
                                toolTipController.ToolTipIcon = ToolTipIcon.Info;
                                toolTipController.IsBalloon = true;
                                if (grid1[BomDetailGrid.Selection.ActivePosition.Row, 0].FindController(typeof(SourceGrid.Cells.Controllers.ToolTipText)) == null)
                                {
                                    grid1[BomDetailGrid.Selection.ActivePosition.Row, 0].Controller.AddController(toolTipController);
                                    grid1[BomDetailGrid.Selection.ActivePosition.Row, 0].View.ForeColor = Color.Red;
                                    grid1[BomDetailGrid.Selection.ActivePosition.Row, 0].View.Font = new Font("Arial", 10, FontStyle.Bold);
                                }
                            }
                            else
                            {
                                grid1[BomDetailGrid.Selection.ActivePosition.Row, 0].View.ForeColor = Color.FromArgb(255, 0, 0, 0);
                                grid1[BomDetailGrid.Selection.ActivePosition.Row, 0].View.Font = null;
                            }
                            sgh2.LoadItemDataToGrid<tb_BOM_SDetailSubstituteMaterial>(gridSubstituteMaterial, sgd2, bOM_SDetail.tb_BOM_SDetailSubstituteMaterials, c => c.ProdDetailID);
                        }
                        else
                        {
                            kryptonHeaderGroup1.Tag = null;
                            kryptonHeaderGroup1.ValuesPrimary.Heading = "替代料明细";
                        }
                    }
                    else
                    {
                        kryptonHeaderGroup1.Tag = null;
                        kryptonHeaderGroup1.ValuesPrimary.Heading = "替代料明细";
                    }
                }
                else
                {
                    kryptonHeaderGroup1.Tag = null;
                    kryptonHeaderGroup1.ValuesPrimary.Heading = "替代料明细";
                }
            }

        }

        private void gridSubstituteMaterial_Enter(object sender, EventArgs e)
        {
            if (kryptonHeaderGroup1.Tag == null)
            {
                //  请选择要添加替代料的配方明细
                MessageBox.Show("请选择要添加替代料的配方明细", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                grid1.Focus();
            }
        }
    }
}

