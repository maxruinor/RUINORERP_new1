using System.Data;
using Aspose.Cells;

namespace RUINORERP.WebServerConsole.Comm
{
    public class ExcelHelper
    {
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="filePath">Excel上传保存路径</param>
        /// <returns>DataTable</returns>
        public static DataTable Import(string filePath)
        {
            try
            {
                Workbook workbook = new(filePath);
                Worksheet sheet = workbook.Worksheets[0];
                Cells cells = sheet.Cells;
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
            }
            catch (Exception e)
            {
                throw new Exception("导入异常", e);
            }
        }

        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="fileName">导出Excel文件名</param>
        /// <param name="dt">要导出的Excel数据</param>
        /// <returns>PacketResult</returns>
        public static PacketResult Export(string fileName, DataTable dt)
        {
            var ret = new PacketResult();
            try
            {
                Workbook wb = new();
                Worksheet sheet = wb.Worksheets[0];

                #region 单元格样式
                Style style = wb.CreateStyle();
                style.HorizontalAlignment = TextAlignmentType.Center;
                style.VerticalAlignment = TextAlignmentType.Center;
                style.Font.Size = 10;
                style.Font.Name = "微软雅黑";
                #endregion

                #region 添加表头
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    sheet.Cells[0, c].PutValue(dt.Columns[c].ColumnName);
                    sheet.Cells[0, c].SetStyle(style);
                }
                sheet.Cells.SetRowHeight(0, 20);
                #endregion

                #region 添加行内容
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        Cell cell = sheet.Cells[r + 1, c];
                        if (cell != null)
                        {
                            sheet.Cells[r + 1, c].PutValue(dt.Rows[r][c].ToString());
                            sheet.Cells[r + 1, c].SetStyle(style);
                        }
                        else
                        {
                            sheet.Cells[r + 1, c].PutValue("");
                            sheet.Cells[r + 1, c].SetStyle(style);
                        }
                    }
                    sheet.Cells.SetRowHeight(r + 1, 20);
                }
                #endregion

                sheet.AutoFitColumns();

                var downPath = AppConfig.Configuration["Site:FilePath"]?.ToString();
                if (string.IsNullOrEmpty(downPath))
                {
                    ret.Msg = "服务器下载文件路径未配置";
                    return ret;
                }

                if (dt.Rows.Count > 1)
                {
                    var path = $@"{downPath}\{fileName}.csv";
                    wb.Save(path, SaveFormat.Csv);
                }
                else
                {
                    var path = $@"{downPath}\{fileName}.xlsx";
                    wb.Save(path, SaveFormat.Xlsx);
                }
                ret.Flag = true;
            }
            catch (Exception e)
            {
                ret.Msg = e.Message;
            }
            return ret;
        }

    }
}