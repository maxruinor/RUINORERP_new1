﻿using RUINORERP.Business;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;
using SqlSugar;
using RUINORERP.UI.UControls;
using RUINORERP.Business.Processor;
//using System.Windows.Forms.DataVisualization.Charting;
using Krypton.Navigator;
using System.Collections;
using System.Linq.Expressions;
using AutoMapper.Internal;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.CommService;
using RUINORERP.Model.CommonModel;
using RUINORERP.Global.Model;
using OfficeOpenXml;
using RUINORERP.UI.CommonUI;
using System.IO;
using Krypton.Toolkit.Suite.Extended.Outlook.Grid;


namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("库存跟踪", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.库存管理, BizType.库存跟踪)]
    public partial class UCInventoryTracking : BaseNavigatorGeneric<View_Inventory, View_Inventory>
    {
        public UCInventoryTracking()
        {
            InitializeComponent();

            //生成查询条件的相关实体
            ReladtedEntityType = typeof(View_Inventory);

            #region 准备枚举值在列表中显示


            System.Linq.Expressions.Expression<Func<tb_Prod, int?>> expr;
            expr = (p) => p.SourceType;
            base.MasterColNameDataDictionary.TryAdd(expr.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(GoodsSource)));

            #endregion



            txtMaxRow.Text = "100";
            //base.RelatedBillEditCol = (c => c.PurEntryNo);
            //base.ChildRelatedEntityType = typeof(tb_PurOrderDetail);
            //base.OnQueryRelatedChild += UCPurEntryQuery_OnQueryRelatedChild;
        }


        public override List<NavParts[]> AddNavParts()
        {
            List<NavParts[]> strings = new List<NavParts[]>();
            //strings.Add(new NavParts[] { NavParts.查询条件 });
            strings.Add(new NavParts[] { NavParts.查询结果, NavParts.分组显示, NavParts.结果分析1 });
            return strings;
        }


        public override void BuildQueryCondition()
        {
            //var lambdaOrder = Expressionable.Create<View_Inventory>()
            // .And(t => t.DataStatus == (int)DataStatus.确认)
            //  .And(t => t.isdeleted == false)
            // .ToExpression();//注意 这一句 不能少
            ////如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            //Filter.SetFieldLimitCondition(lambdaOrder);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.SKU);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.CNName);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.ProductNo);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Location_ID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Type_ID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Model);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Specifications);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.prop);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.DepartmentID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Rack_ID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.BOM_ID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Unit_ID);
        }


        private void UCInventoryTracking_Load(object sender, EventArgs e)
        {
            ReladtedEntityType = typeof(View_Inventory);

            base._UCMasterQuery.ColDisplayTypes = new List<Type>();
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Prod));
            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_Inventory));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;

            base._UCMasterQuery.newSumDataGridViewMaster.Use是否使用内置右键功能 = true;
            base._UCMasterQuery.newSumDataGridViewMaster.ContextMenuStrip = contextMenuStrip1;

            KryptonPage page1 = kryptonWorkspace1.AllPages().FirstOrDefault(c => c.UniqueName == NavParts.结果分析1.ToString());
            page1.Text = "纵向库存跟踪";
            base._UCOutlookGridAnalysis1.GridRelated.FromMenuInfo = this.CurMenuInfo;
            base._UCOutlookGridAnalysis1.GridRelated.ComplexType = true;
            base._UCOutlookGridAnalysis1.GridRelated.SetComplexTargetField<Proc_InventoryTracking>(c => c.业务类型, c => c.单据编号);
            _UCOutlookGridAnalysis1.kryptonOutlookGrid1.ContextMenuStrip = this.contextMenuStripTracker;

            //base._UCOutlookGridAnalysis1.GridRelated.ComplexTargtetField = "业务类型";
            var mappings = new Dictionary<string, string>
        {
            { "采购入库", "tb_PurEntry" },
            { "采购退回", "tb_PurEntryRe" },
            { "采购退回入库", "tb_PurReturnEntry" },
            { "销售出库", "tb_SaleOut" },
            { "销售退回", "tb_SaleOutRe" },
            { "退货翻新领用", "tb_SaleOutReRefurbishedMaterials" },
            { "借出", "tb_ProdBorrowing" },
            { "归还", "tb_ProdReturning" },
            { "其他出库", "tb_StockOut" },
            { "其他入库", "tb_StockIn" },
            { "库存盘点", "tb_Stocktake" },
            { "领料单", "tb_MaterialRequisition" },
            { "退料单", "tb_MaterialReturn" },
            { "分割单加", "tb_ProdSplit" },
            { "分割单减", "tb_ProdSplit" },
            { "组合单加", "tb_ProdMerge" },
            { "组合单减", "tb_ProdMerge" },
            { "调拨入库", "tb_StockTransfer" },
            { "调拨出库", "tb_StockTransfer" },
            { "转换单减", "tb_ProdConversion" },
            { "转换单加", "tb_ProdConversion" },
            { "返工退库", "tb_MRP_ReworkReturn" },
            { "返工入库", "tb_MRP_ReworkEntry" },
            { "缴库", "tb_FinishedGoodsInv" }
        };//还要添加调拨单 转换单，采购退回及采购退回入库。后面还要实现的返厂入库，返厂出库，返厂退回，返厂退回入库，返厂领用，返厂领用退回，返
            foreach (KeyValuePair<string, string> item in mappings)
            {
                //取编号为条件，目标表为在kv
                KeyNamePair keyNamePair = new KeyNamePair(item.Key, item.Value);
                base._UCOutlookGridAnalysis1.GridRelated.SetRelatedInfo<Proc_InventoryTracking>(c => c.单据编号, keyNamePair);
            }

        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Quantity);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.Inv_Cost);
        }

        private void 纵向库存跟踪ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_UCMasterQuery.bindingSourceMaster.Current != null)
            {
                if (_UCMasterQuery.bindingSourceMaster.Current is View_Inventory view_Inventory)
                {
                    //带有output的存储过程 
                    var Location_ID = new SugarParameter("@Location_ID", view_Inventory.Location_ID);
                    var ProdDetailID = new SugarParameter("@ProdDetailID", view_Inventory.ProdDetailID);
                    // var ageP = new SugarParameter("@age", null, true);//设置为output
                    //var list = db.Ado.UseStoredProcedure().SqlQuery<Class1>("sp_school", nameP, ageP);//返回List
                    var list = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_InventoryTracking>("Proc_InventoryTracking", Location_ID, ProdDetailID);//返回List

                    _UCOutlookGridAnalysis1.FieldNameList = UIHelper.GetFieldNameColList(typeof(Proc_InventoryTracking));
                    List<string> SummaryCols = new List<string>();
                    SummaryCols.Add("数量");//这里要优化，按理可以是引用类型来处理
                    _UCOutlookGridAnalysis1.kryptonOutlookGrid1.SubtotalColumns = SummaryCols;

                    _UCOutlookGridAnalysis1.ColDisplayTypes = new List<Type>();
                    //这个视图是用SQL语句生成的,用生成器。
                    _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(Proc_InventoryTracking));
                    _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(tb_Location));


                    // _UCOutlookGridAnalysis2.GridRelated.SetRelatedInfo<View_Inventory, tb_BOM_S>(c => c.BOM_ID, r => r.BOM_ID);

                    _UCOutlookGridAnalysis1.LoadDataToGrid<Proc_InventoryTracking>(list);
                    kryptonWorkspace1.ActivePage = kryptonWorkspace1.AllPages().FirstOrDefault(c => c.UniqueName == NavParts.结果分析1.ToString());

                }
            }

        }



        private void 库存异常检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //进出加起来不等于期末的
            int ErrorCounter = 0;
            //暂时的思路 是用 纵向库存跟踪的存储过程，查出来后再将进出明细(排除期初数据)和最后结余分别总计对比。不同的就有问题。
            foreach (DataGridViewRow dr in base._UCMasterQuery.newSumDataGridViewMaster.SelectedRows)
            {
                if (dr.DataBoundItem is View_Inventory view_Inventory)
                {
                    //带有output的存储过程 
                    var Location_ID = new SugarParameter("@Location_ID", view_Inventory.Location_ID);
                    var ProdDetailID = new SugarParameter("@ProdDetailID", view_Inventory.ProdDetailID);
                    // var ageP = new SugarParameter("@age", null, true);//设置为output
                    //var list = db.Ado.UseStoredProcedure().SqlQuery<Class1>("sp_school", nameP, ageP);//返回List
                    var list = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_InventoryTracking>("Proc_InventoryTracking", Location_ID, ProdDetailID);//返回List
                    if (list.Where(c => c.经营历程 == "进出明细").Sum(c => c.数量) != list.Where(c => c.经营历程 == "最后结余").Sum(c => c.数量))
                    {
                        //异常的行，背景色标识为红黄色。
                        // 设置指定行（这里假设设置第一行）的背景颜色为红黄色
                        dr.DefaultCellStyle.BackColor = Color.FromArgb(255, 64, 0);
                        dr.Cells[2].Style.ForeColor = Color.WhiteSmoke;
                        ErrorCounter++;
                    }
                }
            }
            if (ErrorCounter > 0)
            {
                MessageBox.Show("库存异常的数据行数为：" + ErrorCounter);
            }
            else
            {
                MainForm.Instance.uclog.AddLog("功能", "库存异常的数据行数为：" + ErrorCounter);
            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ExportToExcel(_UCOutlookGridAnalysis1.kryptonOutlookGrid1);
        }


        /// <summary>
        /// 导出 OutlookGrid 数据到 Excel
        /// </summary>
        public void ExportToExcel(KryptonOutlookGrid kryptonOutlookGrid1)
        {
            if (kryptonOutlookGrid1.RowCount == 0)
            {
                MessageBox.Show("没有可导出的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files (*.xlsx; *.xls)|*.xlsx; *.xls";
                    saveDialog.FilterIndex = 1;
                    saveDialog.Title = "导出Excel文件";
                    saveDialog.FileName = $"分析结果_{DateTime.Now:yyyyMMddHHmmss}";
                    saveDialog.RestoreDirectory = true;

                    if (saveDialog.ShowDialog() != DialogResult.OK)
                        return;

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                    using (var progressForm = new ExcelProgressForm("正在导出数据..."))
                    using (var package = new ExcelPackage())
                    {
                        progressForm.Show();
                        Application.DoEvents();

                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("分析结果");
                        int exportedRows = 0;

                        try
                        {
                            // 生成表头
                            int colIndex = 1;
                            for (int i = 0; i < kryptonOutlookGrid1.ColumnCount; i++)
                            {
                                if (kryptonOutlookGrid1.Columns[i].Visible &&
                                    !string.IsNullOrEmpty(kryptonOutlookGrid1.Columns[i].HeaderText))
                                {
                                    worksheet.Cells[1, colIndex].Value = kryptonOutlookGrid1.Columns[i].HeaderText;
                                    worksheet.Column(colIndex).Width = kryptonOutlookGrid1.Columns[i].Width / 7.5;
                                    colIndex++;
                                }
                            }

                            // 填充数据
                            int rowCount = kryptonOutlookGrid1.Rows.Count;
                            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                            {
                                DataGridViewRow gridRow = kryptonOutlookGrid1.Rows[rowIndex];
                                if (gridRow.Visible)
                                {
                                    colIndex = 1;
                                    for (int col = 0; col < kryptonOutlookGrid1.ColumnCount; col++)
                                    {
                                        if (kryptonOutlookGrid1.Columns[col].Visible &&
                                            !string.IsNullOrEmpty(kryptonOutlookGrid1.Columns[col].HeaderText))
                                        {
                                            var cell = kryptonOutlookGrid1[col, rowIndex];
                                            var excelCell = worksheet.Cells[exportedRows + 2, colIndex];

                                            if (cell.Value != null)
                                            {
                                                if (cell.Value is DateTime)
                                                {
                                                    excelCell.Value = cell.FormattedValue.ToString();
                                                }
                                                else if (cell.Value is string || cell.Value is long)
                                                {
                                                    excelCell.Value = cell.FormattedValue.ToString();
                                                }
                                                else if (cell.Value is int && !cell.Value.ToString().Equals(cell.FormattedValue.ToString()))
                                                {
                                                    excelCell.Value = cell.FormattedValue.ToString();
                                                }
                                                else
                                                {
                                                    excelCell.Value = cell.Value;
                                                }
                                            }
                                            colIndex++;
                                        }
                                    }
                                    exportedRows++;
                                }

                                // 每100行更新一次进度
                                if (rowIndex % 100 == 0)
                                {
                                    progressForm.SetProgress((int)((rowIndex + 1) * 100f / rowCount));
                                    Application.DoEvents();
                                }
                            }

                            // 自动调整列宽以获得更好的显示效果
                            worksheet.Cells.AutoFitColumns();
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance?.uclog?.AddLog("Excel导出时出错！" + ex.Message);
                            throw;
                        }
                        finally
                        {
                            package.SaveAs(new FileInfo(saveDialog.FileName));
                            stopwatch.Stop();
                            progressForm.Close();
                        }

                        if (MessageBox.Show($"成功导出 {exportedRows} 行数据，耗时 {stopwatch.Elapsed.TotalSeconds:F2} 秒。\n是否立即打开文件？",
                            "导出完成", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




    }
}

