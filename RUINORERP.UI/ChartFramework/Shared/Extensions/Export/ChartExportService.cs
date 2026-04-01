using OfficeOpenXml;
using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ChartFramework.Shared.Extensions.Export
{
    /// <summary>
    /// 图表导出服务
    /// </summary>
    public static class ChartExportService
    {
        public static void ExportToExcel(this ChartData data, string filePath)
        {
            using var excel = new ExcelPackage();
            var sheet = excel.Workbook.Worksheets.Add("数据导出");

            // 添加表头
            sheet.Cells[1, 1].Value = "分类";
            for (int i = 0; i < data.Series.Count; i++)
            {
                sheet.Cells[1, i + 2].Value = data.Series[i].Name;
            }

            // 填充数据
            if (data.CategoryLabels != null)
            {
                for (int row = 0; row < data.CategoryLabels.Length; row++)
                {
                    sheet.Cells[row + 2, 1].Value = data.CategoryLabels[row];

                    for (int col = 0; col < data.Series.Count; col++)
                    {
                        if (data.Series[col].Values.Count > row)
                        {
                            sheet.Cells[row + 2, col + 2].Value = data.Series[col].Values[row];
                            sheet.Cells[row + 2, col + 2].Style.Numberformat.Format =
                                data.ValueType == RUINORERP.Model.ChartFramework.Models.ValueType.Currency ? "¥#,##0.00" : "#,##0";
                        }
                    }
                }
            }

            excel.SaveAs(new FileInfo(filePath));
        }

        public static void ShowExportDialog(this ChartData data)
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "Excel 文件|*.xlsx",
                FileName = $"{data.Title}_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                data.ExportToExcel(dialog.FileName);
                MessageBox.Show($"成功导出到：{dialog.FileName}", "导出完成",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
