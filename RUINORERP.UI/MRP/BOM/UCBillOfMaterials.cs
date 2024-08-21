using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.UI.UCSourceGrid;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Common.CollectionExtension;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using RUINORERP.Model.Dto;
using DevAge.Windows.Forms;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.UI.Report;
using RUINORERP.UI.BaseForm;
using RUINORERP.Model.QueryDto;
using Microsoft.Extensions.Logging;
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using TransInstruction;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using OfficeOpenXml;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using HLH.Lib.List;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.IO;
using OfficeOpenXml.Style;
using NPOI.SS.Util;
using log4net.Core;
using NPOI.SS.Formula.Functions;


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
    [MenuAttrAssemblyInfo("产品物料清单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.MRP基本资料, BizType.BOM物料清单)]
    public partial class UCBillOfMaterials : BaseBillEditGeneric<tb_BOM_S, tb_BOM_SQueryDto>
    {
        public UCBillOfMaterials()
        {
            InitializeComponent();
            base.OnBindDataToUIEvent += UCStockIn_OnBindDataToUIEvent;
            kryptonDockableNavigator1.SelectedPage = kryptonPage1;
            //            kryptonNavigator1.SelectedPage = kryptonPageMain;



        }

        private void ExportExcel_Click(object sender, EventArgs e)
        {
            if (kryptonTreeGridViewBOMDetail.DataSource is DataTable dt)
            {
                //如果展开

                //如果收起时，kryptonTreeGridViewBOMDetail.rows.count是对的。否则是错的。
                ExportExcel(dt);
            }
        }



        /*导出多级的BOM时按级给不同的背景色，固定的*/

        //// 8 colors for when the tab is not selected
        //private Color[] _normal = new Color[]{ Color.FromArgb(156, 193, 182), Color.FromArgb(247, 184, 134),
        //                                       Color.FromArgb(217, 173, 194), Color.FromArgb(165, 194, 215),
        //                                       Color.FromArgb(179, 166, 190), Color.FromArgb(234, 214, 163),
        //                                       Color.FromArgb(246, 250, 125), Color.FromArgb(188, 168, 225) };

        //// 8 colors for when the tab is selected
        //private Color[] _select = new Color[]{ Color.FromArgb(200, 221, 215), Color.FromArgb(251, 216, 188),
        //                                       Color.FromArgb(234, 210, 221), Color.FromArgb(205, 221, 233),
        //                                       Color.FromArgb(213, 206, 219), Color.FromArgb(244, 232, 204),
        //                                       Color.FromArgb(250, 252, 183), Color.FromArgb(218, 207, 239) };


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

            List<System.Data.DataColumn> columns = dt.Columns.CastToList<System.Data.DataColumn>();
            bool rs = false;
            string selectedFile = string.Empty;
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Excel Files (*.xlsx; *.xls)|*.xlsx; *.xls";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
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

                                int currentRow = 2;//这里空出2行，
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

                                // 获取指定列,第一列设置宽一些
                                var column = excelSheet.Column(1);

                                // 设置列宽
                                column.Width = MaxLevelList.Max() * 20;
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
                                MessageBox.Show("导出数据成功！！！");
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
                                MessageBox.Show("导出数据成功！！！");
                            }

                            #endregion

                        }



                    }
                }
            }
            catch (Exception ex)
            {


            }

            return rs;
        }

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
                    currentRow = FillCellsloop(excelSheet, currentRow, node, bomLevel);
                }
            }
            return currentRow;
        }


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

            //基本信息的列头
            ConcurrentDictionary<string, string> BaseProductInfoColNames = UIHelper.GetFieldNameList<BaseProductInfo>()
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
            List<BaseProductInfo> BaseProductInfoList = mapper.Map<List<BaseProductInfo>>(ViewProdlist);


            //合并的实体中有指定的业务主键关联，不然无法给值
            DataTable dtAll = TreeList.ToDataTable<BaseProductInfo, tb_BOM_SDetailTree>(BaseProductInfoList, BaseProductInfoColNames, colNames, c => c.ProdDetailID);
            return dtAll;

            //基本信息+BOM详情 动态合并
            //Type combinedType = Common.EmitHelper.MergeTypes(true, typeof(BaseProductInfo), typeof(tb_BOM_SDetail));
            // DataTable dt = GetTreeDataToUI(EditEntity, combinedType);
            //object SubBomInfo = Activator.CreateInstance(combinedType);

        }

        IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
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
                node = mapper.Map<tb_BOM_SDetailTree>(detail);
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





        private void UCStockIn_OnBindDataToUIEvent(tb_BOM_S entity)
        {
            BindData(entity as tb_BOM_S);
            //加载树
        }

        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_BOM_S);
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

        public void BindData(tb_BOM_S entity)
        {
            if (entity == null)
            {
                MainForm.Instance.uclog.AddLog("实体不能为空", UILogType.警告);
                return;
            }
            EditEntity = entity;

            if (entity.BOM_ID > 0)
            {
                entity.PrimaryKeyID = entity.BOM_ID;
                entity.ActionStatus = ActionStatus.加载;
                if (entity.ProdDetailID > 0)
                {
                    txtProdDetailID.Text = entity.SKU.ToString();
                    BindToTree(entity);
                }
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.BOM_No = BizCodeGenerator.Instance.GetBizBillNo(BizType.BOM物料清单);
                entity.Effective_at = System.DateTime.Now;
            }

            /*****/
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.TotalMaterialQty, txtTotalMaterialQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.BOM_Name, txtBOM_Name, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.property, txtProp, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.Specifications, txtSpec, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4Cmb<tb_ProductType>(entity, k => k.Type_ID, v => v.TypeName, cmbType);
            DataBindingHelper.BindData4Cmb<tb_Files>(entity, k => k.Doc_ID, v => v.FileName, cmbDoc_ID);
            DataBindingHelper.BindData4Cmb<tb_BOMConfigHistory>(entity, k => k.BOM_S_VERID, v => v.VerNo, cmbBOM_S_VERID);
            DataBindingHelper.BindData4DataTime<tb_BOM_S>(entity, t => t.Effective_at, dtpEffective_at, false);
            DataBindingHelper.BindData4CheckBox<tb_BOM_S>(entity, t => t.is_enabled, chkis_enabled, false);
            DataBindingHelper.BindData4CheckBox<tb_BOM_S>(entity, t => t.is_available, chkis_available, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.OutManuCost.ToString(), txtOutManuCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.ManufacturingCost.ToString(), txtLaborCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.OutProductionAllCosts.ToString(), txtOutProductionAllCosts, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.OutputQty.ToString(), txtOutputQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.OutputQty.ToString(), txtPeopleQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.PeopleQty.ToString(), txtPeopleQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.WorkingHour.ToString(), txtWorkingHour, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.MachineHour.ToString(), txtMachineHour, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_BOM_S>(entity, t => t.ExpirationDate, dtpExpirationDate, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.DailyQty.ToString(), txtDailyQty, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.SelfProductionAllCosts.ToString(), txtSelfProductionAllCosts, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4ControlByEnum<tb_BOM_S>(entity, t => t.DataStatus, txtstatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_BOM_S>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_BOM_SDetails != null && entity.tb_BOM_SDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_BOM_SDetail>(grid1, sgd, entity.tb_BOM_SDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_BOM_SDetail>(grid1, sgd, new List<tb_BOM_SDetail>(), c => c.ProdDetailID);
            }

            //先绑定这个。InitFilterForControl 这个才生效, 一共三个来控制，这里分别是绑定ID和SKU。下面InitFilterForControlByExp 是生成快捷按钮
            DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, k => k.SKU, txtProdDetailID, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_BOM_S>(entity, v => v.ProdDetailID, txtProdDetailID, true);


            //如果属性变化 则状态为修改
            EditEntity.PropertyChanged += async (sender, s2) =>
            {
                //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
                //这样在新增加和修改时才会触发添加母件的快捷按钮
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && s2.PropertyName == entity.GetPropertyName<tb_BOM_S>(c => c.ActionStatus))
                {
                    base.InitRequiredToControl(new tb_BOM_SValidator(), kryptonSplitContainer1.Panel1.Controls);
                    //  base.InitEditItemToControl(entity, kryptonPanel1.Controls);
                    //  base.InitFilterForControl<View_ProdDetail, View_ProdDetailQueryDto>(entity, txtProdDetailID, c => c.CNName);

                    //创建表达式  草稿 结案 和没有提交的都不显示
                    var lambdaOrder = Expressionable.Create<View_ProdDetail>()
                                    // .And(t => t.DataStatus == (int)DataStatus.确认)
                                    .ToExpression();//注意 这一句 不能少

                    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_ProdDetail).Name + "Processor");
                    QueryFilter queryFilterC = baseProcessor.GetQueryFilter();

                    ///视图指定成实体表，为了显示关联数据
                    DataBindingHelper.InitFilterForControlByExp<View_ProdDetail>(entity, txtProdDetailID, c => c.SKU, queryFilterC, typeof(tb_Prod));
                }
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    entity.ActionStatus = ActionStatus.修改;
                    base.ToolBarEnabledControl(MenuItemEnums.修改);

                    if (entity.ProdDetailID > 0 && s2.PropertyName == entity.GetPropertyName<tb_BOM_S>(c => c.ProdDetailID))
                    {
                        //视图来的，没有导航查询到相关数据
                        //find bomid
                        kryptonTreeView1.TreeView.Nodes.Clear();
                        BaseController<tb_ProdDetail> ctrProdDetail = Startup.GetFromFacByName<BaseController<tb_ProdDetail>>(typeof(tb_ProdDetail).Name + "Controller");
                        var bomprod = await ctrProdDetail.BaseQueryByIdAsync(entity.ProdDetailID);
                        if (bomprod.BOM_ID != null)
                        {
                            if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                            {
                                //如果这个产品有配方了 
                                if (MessageBox.Show("当前选中产品已经有配方,你确定需要对这个母件增加新的配方吗？\r\n如果是，则会更新当前母件的默认配方为您增加的配方", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                                {
                                    MainForm.Instance.uclog.AddLog("当前选中产品已经有配方", UILogType.错误);
                                    return;
                                }
                                else
                                {
                                    //加载关联数据
                                    if (txtProdDetailID.ButtonSpecs.Count > 0 && txtProdDetailID.ButtonSpecs[0].Tag != null)
                                    {
                                        if (txtProdDetailID.ButtonSpecs[0].Tag is View_ProdDetail vp)
                                        {
                                            entity.Specifications = vp.Specifications;
                                            entity.property = vp.prop;
                                            entity.BOM_Name = vp.CNName + "-" + vp.prop;
                                            entity.SKU = vp.SKU;
                                            entity.Type_ID = vp.Type_ID;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //tb_BOM_SController<tb_BOM_S> ctrBOM = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
                                //var bom = await ctrBOM.BaseQueryByIdNavAsync(bomprod.BOM_ID);
                                // UIPordBOMHelper.BindToTreeView(bom, bom.tb_BOM_SDetails, kryptonTreeView1.TreeView);
                                BindToTree(entity);
                            }
                        }

                    }
                    //加载关联数据
                    if (txtProdDetailID.ButtonSpecs.Count > 0 && txtProdDetailID.ButtonSpecs[0].Tag != null)
                    {
                        if (txtProdDetailID.ButtonSpecs[0].Tag is View_ProdDetail vp)
                        {
                            entity.Specifications = vp.Specifications;
                            entity.property = vp.prop;
                            entity.BOM_Name = vp.CNName + "-" + vp.prop;
                            entity.SKU = vp.SKU;
                            entity.Type_ID = vp.Type_ID;
                        }
                    }
                    //if (entity.CustomerVendor_ID.HasValue && entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.CustomerVendor_ID))
                    //{
                    //    var obj = CacheHelper.Instance.GetEntity<tb_ProdDetail>(entity.CustomerVendor_ID);
                    //    if (obj != null && obj.ToString() != "System.Object")
                    //    {
                    //        if (obj is tb_CustomerVendor cv)
                    //        {
                    //            EditEntity.Employee_ID = cv.Employee_ID;
                    //        }
                    //    }
                    //}
                }

            };
            if (entity.tb_BOM_SDetails == null)
            {
                entity.tb_BOM_SDetails = new List<tb_BOM_SDetail>();
            }
            //如果明细中包含了母件。就是死循环。不能用树来显示
            if (entity.tb_BOM_SDetails.Any(c => c.ProdDetailID == EditEntity.ProdDetailID))
            {
                MessageBox.Show("BOM明细中包含了母件，请修复这个致命数据错误。系统已经跳过树形显示。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadTreeData(TransToDataTableByTreeAsync(entity));
        }


 

        protected override void AddByCopy()
        {
            EditEntity.ActionStatus = ActionStatus.新增;
            EditEntity.BOM_ID = 0;
            EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
            EditEntity.DataStatus = (int)DataStatus.草稿;
            EditEntity.Approver_at = null;
            EditEntity.SKU = null;
            EditEntity.BOM_No = BizCodeGenerator.Instance.GetBizBillNo(BizType.BOM物料清单);
            BusinessHelper.Instance.InitEntity(EditEntity);

            foreach (var item in EditEntity.tb_BOM_SDetails)
            {
                item.BOM_ID = 0;
                item.SubID = 0;
            }
            foreach (var item in EditEntity.tb_BOM_SDetailSecondaries)
            {
                item.BOM_ID = 0;
                item.SecID = 0;
            }
            base.AddByCopy();
        }

        private void BindToTree(tb_BOM_S _bom)
        {
            BOM_Level = 1;
            tb_BOM_SController<tb_BOM_S> ctrBOM = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
            /// var bom = await ctrBOM.BaseQueryByIdNavAsync(entity.MainID);
            var bom = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>()
            .RightJoin<tb_ProdDetail>((a, b) => a.ProdDetailID == b.ProdDetailID)
             .Includes(b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
            .Includes(a => a.tb_producttype)
            .Includes(a => a.tb_BOM_SDetails)
            .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
            .Includes(a => a.tb_BOM_SDetails, b => b.view_ProdDetail)
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
            TreeListViewItem itemRow = new TreeListViewItem(bom.BOM_Name, 0);
            itemRow.Tag = bom;
            itemRow.SubItems.Add(bom.property); //subitems只是从属于itemRow的子项。目前是四列
                                                ////一定会有值
                                                //tb_BOM_S bOM_S = listboms.Where(c => c.ProdDetailID == row.ProdDetailID).FirstOrDefault();
                                                //itemRow.SubItems[0].Tag = bOM_S;
            itemRow.SubItems.Add(bom.Specifications);
            string prodType = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProductType), "Type_ID", bom.Type_ID);

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

                    TreeListViewItem itemSub = new TreeListViewItem(BOM_SDetail.SubItemName, 0);
                    itemSub.Tag = bOM_S;
                    itemSub.SubItems.Add(BOM_SDetail.property);//subitems只是从属于itemRow的子项。目前是四列
                    itemSub.SubItems.Add(BOM_SDetail.SubItemSpec);
                    string prodType = UIHelper.ShowGridColumnsNameValue(typeof(tb_ProductType), "Type_ID", BOM_SDetail.Type_ID);
                    itemSub.SubItems.Add(prodType);
                    itemSub.SubItems.Add(BOM_SDetail.UsedQty.ToString());

                    listViewItem.Items.Add(itemSub);

                    var bomsub = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_BOM_S>()
                   //.RightJoin<tb_ProdDetail>((a, b) => a.ProdDetailID == b.ProdDetailID)
                   // .Includes(b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                   //.Includes(a => a.tb_producttype)
                   .Includes(a => a.tb_BOM_SDetails)  //查出他的子级是不是BOM并且带出他的子项
                                                      //.Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                                                      //.Includes(a => a.tb_BOM_SDetails, b => b.view_ProdDetail)
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
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            LoadGrid1();

            //不能放在构造函数里
            ToolStripMenuItem exportExcel = new ToolStripMenuItem("导出Excel");
            exportExcel.Name = "导出Excel";
            exportExcel.Size = new System.Drawing.Size(139, 22);
            exportExcel.Text = "导出Excel";
            exportExcel.Click += ExportExcel_Click;
            toolStripbtnFunction.DropDownItems.Add(exportExcel);
        }


        private void LoadGrid1()
        {
            // list = dc.Query();
            //DevAge.ComponentModel.IBoundList bd = list.ToBindingSortCollection<View_ProdDetail>()  ;//new DevAge.ComponentModel.BoundDataView(mView);
            // grid1.DataSource = list.ToBindingSortCollection<View_ProdDetail>() as DevAge.ComponentModel.IBoundList;// new DevAge.ComponentModel.BoundDataView(list.ToDataTable().DefaultView); 
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SourceGridDefineColumnItem> listCols = sgh.GetGridColumns<ProductSharePart, tb_BOM_SDetail>(c => c.ProdDetailID, true);

            listCols.SetCol_NeverVisible<tb_BOM_SDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_BOM_SDetail>(c => c.SubID);
            listCols.SetCol_NeverVisible<tb_BOM_SDetail>(c => c.BOM_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.ShortCode);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);

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
            sgd.GridData = EditEntity;
            //要放到初始化sgd后面
            listCols.SetCol_Summary<tb_BOM_SDetail>(c => c.UsedQty);
            listCols.SetCol_Summary<tb_BOM_SDetail>(c => c.SubtotalMaterialCost);
            listCols.SetCol_Summary<tb_BOM_SDetail>(c => c.SubtotalManufacturingCost);
            listCols.SetCol_Summary<tb_BOM_SDetail>(c => c.SubtotalOutManuCost);
            listCols.SetCol_Summary<tb_BOM_SDetail>(c => c.TotalOutsourcingAllCost);
            listCols.SetCol_Summary<tb_BOM_SDetail>(c => c.TotalSelfProductionAllCost);


            listCols.SetCol_Formula<tb_BOM_SDetail>((a, b) => a.MaterialCost * b.UsedQty, c => c.SubtotalMaterialCost);
            listCols.SetCol_Formula<tb_BOM_SDetail>((a, b) => a.ManufacturingCost * b.UsedQty, c => c.SubtotalManufacturingCost);
            listCols.SetCol_Formula<tb_BOM_SDetail>((a, b) => a.OutManuCost * b.UsedQty, c => c.SubtotalOutManuCost);

            listCols.SetCol_Formula<tb_BOM_SDetail>((a, b, c) => a.UsedQty * b.MaterialCost + c.SubtotalManufacturingCost, d => d.TotalSelfProductionAllCost);
            listCols.SetCol_Formula<tb_BOM_SDetail>((a, b, c) => a.UsedQty * b.MaterialCost + c.SubtotalOutManuCost, d => d.TotalOutsourcingAllCost);


            sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.Inv_Cost, t => t.MaterialCost);

            //冗余名称和规格
            sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.CNName, t => t.SubItemName);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.Specifications, t => t.SubItemSpec);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_BOM_SDetail>(sgd, f => f.Type_ID, t => t.Type_ID);
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

            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
        .AndIF(true, w => w.CNName.Length > 0)
        // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
        .ToExpression();//注意 这一句 不能少
                        // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);

            sgd.SetDependencyObject<ProductSharePart, tb_BOM_SDetail>(list);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_BOM_SDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            sgh.OnLoadRelevantFields += Sgh_OnLoadRelevantFields;
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
                _BOM_SDetail.ManufacturingCost = vp.tb_bom_s.ManufacturingCost;
                int ColIndex = griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_BOM_SDetail.ManufacturingCost)).ColIndex;
                griddefine.grid[Position.Row, ColIndex].Value = _BOM_SDetail.ManufacturingCost;


                _BOM_SDetail.MaterialCost = vp.tb_bom_s.TotalMaterialCost;
                ColIndex = griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_BOM_SDetail.MaterialCost)).ColIndex;
                griddefine.grid[Position.Row, ColIndex].Value = _BOM_SDetail.MaterialCost;


                _BOM_SDetail.OutManuCost = vp.tb_bom_s.OutManuCost;
                ColIndex = griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_BOM_SDetail.OutManuCost)).ColIndex;
                griddefine.grid[Position.Row, ColIndex].Value = _BOM_SDetail.OutManuCost;


                //Child_BOM_Node_ID 这个在明细中显示的是ID，没有使用外键关联显示，因为列名不一致。手动显示为配方名称
                var cbni = griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_BOM_SDetail.Child_BOM_Node_ID));
                if (cbni != null)
                {
                    griddefine.grid[Position.Row, cbni.ColIndex].DisplayText = vp.tb_bom_s.BOM_Name;
                }


            }



        }

        protected async override Task<ReturnResults<tb_BOM_S>> Delete()
        {
            ReturnResults<tb_BOM_S> rss = await base.Delete();
            if (rss.Succeeded)
            {
                //清空对应产品明细中的BOM信息
                if (rss.ReturnObject.tb_proddetail.BOM_ID.HasValue)
                {
                    rss.ReturnObject.tb_proddetail.BOM_ID = null;
                    BaseController<tb_ProdDetail> ctrDetail = Startup.GetFromFacByName<BaseController<tb_ProdDetail>>(typeof(tb_ProdDetail).Name + "Controller");
                    await ctrDetail.BaseSaveOrUpdate(rss.ReturnObject.tb_proddetail);
                }
            }
            return rss;
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
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_BOM_SDetail bOM_SDetail = mapper.Map<tb_BOM_SDetail>(item);
                    details.Add(bOM_SDetail);
                }
                //List<tb_BOM_SDetail> details = mapper.Map<List<tb_BOM_SDetail>>(RowDetails);
                sgh.InsertItemDataToGrid<tb_BOM_SDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
            }


        }

        private void Sgh_OnCalculateColumnValue(object _rowObj, SourceGridDefine myGridDefine, SourceGrid.Position position)
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
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalMaterialCost = details.Sum(c => c.SubtotalMaterialCost);
                EditEntity.OutProductionAllCosts = EditEntity.TotalMaterialCost + EditEntity.OutManuCost;
                EditEntity.SelfProductionAllCosts = EditEntity.TotalMaterialCost + EditEntity.ManufacturingCost;
            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }


        List<tb_BOM_SDetail> details = new List<tb_BOM_SDetail>();
        protected async override void Save()
        {
            if (EditEntity == null)
            {
                return;
            }
            // var eer = errorProviderForAllInput.GetError(txtTotalQty);
            bindingSourceSub.EndEdit();
            List<tb_BOM_SDetail> detailentity = bindingSourceSub.DataSource as List<tb_BOM_SDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();

                //如果没有有效的明细。直接提示
                if (details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return;
                }

                //副产出暂时不做。为了不验证给一行空值，验证后清除掉
                EditEntity.tb_BOM_SDetailSecondaries = new List<tb_BOM_SDetailSecondary>();
                EditEntity.tb_BOM_SDetailSecondaries.Add(new tb_BOM_SDetailSecondary());


                EditEntity.tb_BOM_SDetails = details;
                //没有经验通过下面先不计算
                if (!base.Validator(EditEntity))
                {
                    return;
                }
                if (!base.Validator<tb_BOM_SDetail>(details))
                {
                    return;
                }

                //BOM子件中不能包含目标母件本身。
                if (details.Any(c => c.ProdDetailID == EditEntity.ProdDetailID))
                {
                    MessageBox.Show("BOM子件中不能包含目标母件本身。请检查数据后再保存。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                EditEntity.tb_BOM_SDetailSecondaries.Clear();

                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证

                EditEntity.tb_BOM_SDetails = details;
                tb_BOM_SController<tb_BOM_S> ctr = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
                ReturnMainSubResults<tb_BOM_S> SaveResult = await ctr.SaveOrUpdateWithChild<tb_BOM_S>(EditEntity); //await base.Save(EditEntity);
                if (SaveResult.Succeeded)
                {
                    base.ToolBarEnabledControl(EditEntity);
                    base.ToolBarEnabledControl(MenuItemEnums.保存);
                }
            }
            else
            {
                MessageBox.Show("加载状态的BOM，无法更新保存数据，请先修改数据后再保存！");
            }


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


            kryptonTreeGridViewBOMDetail.DataSource = dtAll;
            kryptonTreeGridViewBOMDetail.Columns[kryptonTreeGridViewBOMDetail.IdColumnName].Visible = false;
            kryptonTreeGridViewBOMDetail.ExpandAll();
        }

        protected async override Task<ApprovalEntity> Review()
        {
            if (EditEntity == null)
            {
                return null;
            }
            //如果已经审核通过，则不能重复审核
            if (EditEntity.ApprovalStatus.HasValue)
            {
                if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
                {
                    if (EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
                    {
                        MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据不能重复审核。");
                        return null;
                    }
                }
            }

            Command command = new Command();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_BOM_S oldobj = CloneHelper.DeepCloneObject<tb_BOM_S>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_BOM_S>(EditEntity, oldobj);
            };
            ApprovalEntity ae = await base.Review();
            if (EditEntity == null)
            {
                return null;
            }
            if (ae.ApprovalStatus == (int)ApprovalStatus.未审核)
            {
                return null;
            }

            tb_BOM_SController<tb_BOM_S> ctr = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
            bool Succeeded = await ctr.ApproveAsync(EditEntity, ae);
            if (Succeeded)
            {
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);

                //审核成功
                base.ToolBarEnabledControl(MenuItemEnums.审核);
                //如果审核结果为不通过时，审核不是灰色。
                if (!ae.ApprovalResults)
                {
                    toolStripbtnReview.Enabled = true;
                }
                else
                {
                    UIPordBOMHelper.BindToTreeViewNoRootNode(EditEntity.tb_BOM_SDetails, kryptonTreeView1.TreeView);
                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核成功。");
                }
            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！", Color.Red);
            }

            return ae;
        }


        protected async override void ReReview()
        {
            if (EditEntity == null)
            {
                return;
            }

            //反审，要审核过，并且通过了，才能反审。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
                return;
            }


            if (EditEntity.tb_BOM_SDetails == null || EditEntity.tb_BOM_SDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据。", UILogType.警告);
                return;
            }

            Command command = new Command();

            tb_BOM_S oldobj = CloneHelper.DeepCloneObject<tb_BOM_S>(EditEntity);
            command.UndoOperation = delegate ()
            {
                CloneHelper.SetValues<tb_BOM_S>(EditEntity, oldobj);
            };
            tb_BOM_SController<tb_BOM_S> ctr = Startup.GetFromFac<tb_BOM_SController<tb_BOM_S>>();
            bool Succeeded = await ctr.ReverseApproveAsync(EditEntity);
            if (Succeeded)
            {

                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);

                //审核成功
                base.ToolBarEnabledControl(MenuItemEnums.反审);
                toolStripbtnReview.Enabled = true;

            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{EditEntity.BOM_No}反审失败,请联系管理员！", Color.Red);
            }

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

            //图片特殊处理
            if (kryptonTreeGridViewBOMDetail.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理



        }
    }
}
