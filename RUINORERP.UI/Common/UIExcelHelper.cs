using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RUINORERP.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        public static void ExportExcel(UControls.NewSumDataGridView newSumDataGridViewMaster)
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

    }
}
