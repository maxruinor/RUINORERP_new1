using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RUINORERP.Model;
using RUINORERP.UI.CommonUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// UI层专门用于导出
    /// </summary>
    public class UIExcelHelper
    {




        ////public static bool ExportExcel<T>(UControls.NewSumDataGridView newSumDataGridViewMaster, ConcurrentDictionary<string, string> columnskv = null)
        ////{
        ////    bool rs = false;
        ////    这里可以添加美化标题这些操作 下面i设置一个起点。从第3行开始
        ////    int StartRowIndex = 3; int i = StartRowIndex;
        ////    ConcurrentDictionary<string, string> columnskv = UIHelper.GetFieldNameList<T>();
        ////    if (columnskv == null)
        ////    {
        ////        columnskv = UIHelper.GetFieldNameList<T>();
        ////    }
        ////    ExportExcel(newSumDataGridViewMaster);
        ////    rs = true;
        ////    return rs;
        ////}

        /// <summary>
        /// 导出 用于特殊的绑定了数据源的
        /// </summary>
        /// <param name="newSumDataGridViewMaster">NewSumDataGridView类型的控件内容</param>
        /// <returns></returns>
        public static void ExportExcel_old(UControls.NewSumDataGridView newSumDataGridViewMaster)
        {

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
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

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
                        try
                        {
                            #region
                            //生成字段名称
                            int k = 0;
                            for (int i = 0; i < newSumDataGridViewMaster.ColumnCount; i++)
                            {
                                if (newSumDataGridViewMaster.Columns[i].Visible && !string.IsNullOrEmpty(newSumDataGridViewMaster.Columns[i].HeaderText)) //不导出隐藏的列
                                {
                                    excelSheet.Cells[1, k + 1].Value = newSumDataGridViewMaster.Columns[i].HeaderText;
                                    // 设置列宽
                                    //一个字符的宽度大约是 7.5 像素（基于默认字体和字号）。
                                    excelSheet.Column(k + 1).Width = newSumDataGridViewMaster.Columns[i].Width/7; // 第一列宽度设置为 20
                                    k++;
                                }
                            }
                             

                            //填充数据

                            for (int i = 0; i < newSumDataGridViewMaster.RowCount; i++)
                            {

                                k = 0;
                                for (int j = 0; j < newSumDataGridViewMaster.ColumnCount; j++)
                                {
                                    if (newSumDataGridViewMaster.Columns[j].Visible && !string.IsNullOrEmpty(newSumDataGridViewMaster.Columns[j].HeaderText)) //不导出隐藏的列
                                    {

                                        //    下面有 k++;不能跳过用continue
                                        //实际 64long型基本是ID 像单位，变成名称了。
                                        if (Common.CommonHelper.Instance.GetRealType(newSumDataGridViewMaster[j, i].ValueType) == typeof(DateTime))
                                        {
                                            excelSheet.Cells[i + 2, k + 1].Value = newSumDataGridViewMaster[j, i].FormattedValue.ToString();
                                        }
                                        else if (Common.CommonHelper.Instance.GetRealType(newSumDataGridViewMaster[j, i].ValueType) == typeof(string)
                                            || Common.CommonHelper.Instance.GetRealType(newSumDataGridViewMaster[j, i].ValueType) == typeof(Int64)
                                            )
                                        {
                                            if (newSumDataGridViewMaster[j, i].Value != null)
                                            {
                                                excelSheet.Cells[i + 2, k + 1].Value = newSumDataGridViewMaster[j, i].FormattedValue.ToString();
                                            }
                                        }
                                        else if (Common.CommonHelper.Instance.GetRealType(newSumDataGridViewMaster[j, i].ValueType) == typeof(Int32))
                                        {
                                            //如果是枚举时 看显示的和值是不是一样。是一样优化值。否则显示所见
                                            if (newSumDataGridViewMaster[j, i].Value != null && !newSumDataGridViewMaster[j, i].Value.ToString().Equals(newSumDataGridViewMaster[j, i].FormattedValue.ToString()))
                                            {
                                                excelSheet.Cells[i + 2, k + 1].Value = newSumDataGridViewMaster[j, i].FormattedValue.ToString();
                                            }
                                            else
                                            {
                                                excelSheet.Cells[i + 2, k + 1].Value = newSumDataGridViewMaster[j, i].Value;
                                            }
                                        }
                                        else
                                        {
                                            if (newSumDataGridViewMaster[j, i].Value != null)
                                            {
                                                excelSheet.Cells[i + 2, k + 1].Value = Common.CommonHelper.Instance.GetRealValueByDataType(newSumDataGridViewMaster[j, i].ValueType, newSumDataGridViewMaster[j, i].Value);

                                            }
                                        }
                                        k++;
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
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(selectedFile);
                            package.SaveAs(fileInfo);
                            MessageBox.Show($"成功导出{newSumDataGridViewMaster.RowCount}行数据！");
                            /*
 *   ExcelWorksheet excelSheet = excelDoc.Workbook.Worksheets.Add("Sheet1");
excelSheet.Cells[1, 1].Value = "这是第一行第一列的值";
excelSheet.Cells[1, 2].Value = "这是第一行第二列的值";

FileInfo fileInfo = new FileInfo("C:\\Temp\\ExportTest.xlsx");
excelDoc.SaveAs(fileInfo);
 */
                            //for (int row = 0; row < rowCount - 1; row++)
                            //{
                            //    RawData data = new RawData();
                            //    data.id = (double)worksheet.Cells[row + 2, 1].Value;
                            //    data.speed = (double)worksheet.Cells[row + 2, 2].Value;
                            //    data.power = (double)worksheet.Cells[row + 2, 3].Value;
                            //    data.fcrr = (double)worksheet.Cells[row + 2, 4].Value;
                            //    DataList.Add(data);

                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }


        }

        /// <summary>
        /// 导出DataGridView数据到Excel（支持特殊绑定的数据源）
        /// </summary>
        /// <param name="dataGridView">NewSumDataGridView控件</param>
        public static void ExportExcel(UControls.NewSumDataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.RowCount == 0)
            {
                MessageBox.Show("没有可导出的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel文件 (*.xlsx)|*.xlsx|Excel 97-2003文件 (*.xls)|*.xls",
                    FilterIndex = 1,
                    Title = "导出Excel文件",
                    FileName = $"导出数据_{DateTime.Now:yyyyMMddHHmmss}",
                    RestoreDirectory = true
                })
                {
                    if (saveDialog.ShowDialog() != DialogResult.OK) return;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var stopwatch = Stopwatch.StartNew();

                    using (var progressForm = new ProgressForm("正在导出数据..."))
                    using (var package = new ExcelPackage())
                    {
                        progressForm.Show();
                        Application.DoEvents();

                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        int exportedRows = 0;

                        try
                        {
                            // 生成表头
                            int colIndex = 1;
                            var visibleColumns = dataGridView.Columns.Cast<DataGridViewColumn>()
                                .Where(c => c.Visible && !string.IsNullOrEmpty(c.HeaderText))
                                .ToList();

                            foreach (var column in visibleColumns)
                            {
                                worksheet.Cells[1, colIndex].Value = column.HeaderText;
                                worksheet.Column(colIndex).Width = column.Width / 7.5; // 更精确的列宽计算
                                colIndex++;
                            }

                            // 填充数据
                            for (int rowIndex = 0; rowIndex < dataGridView.RowCount; rowIndex++)
                            {
                                colIndex = 1;
                                foreach (var column in visibleColumns)
                                {
                                    var cell = dataGridView[column.Index, rowIndex];
                                    if (cell.Value == null) continue;

                                    var excelCell = worksheet.Cells[rowIndex + 2, colIndex];
                                    SetCellValue(excelCell, cell);
                                    colIndex++;
                                }
                                exportedRows++;

                                // 每100行更新一次进度
                                if (rowIndex % 100 == 0)
                                {
                                    progressForm.SetProgress((int)((rowIndex + 1) * 100f / dataGridView.RowCount));
                                    Application.DoEvents();
                                }
                            }

                            // 自动调整列宽（基于内容）
                            worksheet.Cells.AutoFitColumns();
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance?.logger?.LogError($"Excel导出出错: {ex.Message}", ex);
                            throw;
                        }
                        finally
                        {
                            package.SaveAs(new FileInfo(saveDialog.FileName));
                            stopwatch.Stop();

                            progressForm.Close();
                            progressForm.Dispose();
                        }

                        if (MessageBox.Show($"成功导出 {exportedRows} 行数据，耗时 {stopwatch.Elapsed.TotalSeconds:F2} 秒。\n是否立即打开文件？",
                            "导出完成", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
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
        /// 智能设置单元格值和格式
        /// </summary>
        private static void SetCellValue(ExcelRange excelCell, DataGridViewCell dataCell)
        {
            Type cellType = Common.CommonHelper.Instance.GetRealType(dataCell.ValueType);
            object cellValue = dataCell.Value;
            string formattedValue = dataCell.FormattedValue?.ToString();

            if (cellType == typeof(DateTime))
            {
                excelCell.Value = DateTime.Parse(formattedValue);
                excelCell.Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
            }
            else if (cellType == typeof(int) || cellType == typeof(long))
            {
                // 处理枚举值
                if (!cellValue.ToString().Equals(formattedValue))
                {
                    excelCell.Value = formattedValue;
                }
                else
                {
                    excelCell.Value = Convert.ToInt64(cellValue);
                }
            }
            else if (cellType == typeof(decimal) || cellType == typeof(double) || cellType == typeof(float))
            {
                excelCell.Value = Convert.ToDouble(cellValue);
                excelCell.Style.Numberformat.Format = "#,##0.00";
            }
            else if (cellType == typeof(bool))
            {
                excelCell.Value = (bool)cellValue ? "是" : "否";
            }
            else
            {
                excelCell.Value = formattedValue ?? cellValue?.ToString();
            }
        }


    }
}
