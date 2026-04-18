using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using RUINORERP.Business.ImportEngine.Models;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 导入图片信息
    /// </summary>
    public class ImportImageInfo
    {
        public byte[] Data { get; set; }
        public string Suffix { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
    }

    /// <summary>
    /// Excel 解析服务
    /// </summary>
    public class ExcelParserService
    {
        /// <summary>
        /// �?Excel 文件解析�?DataTable
        /// </summary>
        public async Task<DataTable> ParseAsync(string filePath, int sheetIndex = 0, int maxRows = -1, bool extractImages = false)
        {
            return await Task.Run(() =>
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    else
                    {
                        workbook = new HSSFWorkbook(fs);
                    }

                    ISheet sheet = workbook.GetSheetAt(sheetIndex);
                    var images = extractImages ? ExtractImages(workbook, sheet) : new List<ImportImageInfo>();
                    return SheetToDataTable(sheet, maxRows, images);
                }
            });
        }

        private List<ImportImageInfo> ExtractImages(IWorkbook workbook, ISheet sheet)
        {
            var result = new List<ImportImageInfo>();
            try
            {
                if (workbook is XSSFWorkbook xssfWorkbook)
                {
                    var pictures = xssfWorkbook.GetAllPictures();
                    var patriarch = sheet.CreateDrawingPatriarch();
                    
                    if (patriarch is XSSFDrawing xssfDrawing)
                    {
                        var shapes = xssfDrawing.GetShapes();
                        int shapeIndex = 0;
                        
                        foreach (var picObj in pictures)
                        {
                            if (picObj is IPictureData picData && shapeIndex < shapes.Count)
                            {
                                var shape = shapes[shapeIndex];
                                if (shape is XSSFPicture pic)
                                {
                                    var anchor = pic.ClientAnchor;
                                    if (anchor != null)
                                    {
                                        result.Add(new ImportImageInfo
                                        {
                                            Data = picData.Data,
                                            Suffix = picData.MimeType.Contains("png") ? ".png" : ".jpg",
                                            RowIndex = anchor.Row1,
                                            ColumnIndex = anchor.Col1
                                        });
                                    }
                                }
                                shapeIndex++;
                            }
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        private DataTable SheetToDataTable(ISheet sheet, int maxRows, List<ImportImageInfo> images)
        {
            DataTable dt = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null) throw new Exception("Excel 工作表没有标题行");

            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                ICell cell = headerRow.GetCell(i);
                string colName = cell != null ? GetCellValue(cell).Trim() : $"Column_{i + 1}";
                if (string.IsNullOrEmpty(colName)) colName = $"Column_{i + 1}";

                // 处理重名�?
                if (dt.Columns.Contains(colName))
                {
                    int suffix = 1;
                    while (dt.Columns.Contains($"{colName}_{suffix}")) suffix++;
                    colName = $"{colName}_{suffix}";
                }
                dt.Columns.Add(colName);
            }

            int lastRow = maxRows > 0 ? Math.Min(maxRows + 1, sheet.LastRowNum + 1) : sheet.LastRowNum + 1;
            for (int i = 1; i < lastRow; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                DataRow dr = dt.NewRow();
                bool hasData = false;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = row.GetCell(j);
                    string val = cell != null ? GetCellValue(cell) : string.Empty;
                    dr[j] = val;
                    if (!string.IsNullOrEmpty(val)) hasData = true;
                }

                if (hasData) dt.Rows.Add(dr);
            }

            return dt;
        }

        private string GetCellValue(ICell cell)
        {
            if (cell == null) return string.Empty;
            try
            {
                switch (cell.CellType)
                {
                    case CellType.String: return cell.StringCellValue?.Trim() ?? "";
                    case CellType.Numeric:
                        return DateUtil.IsCellDateFormatted(cell)
                            ? cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss")
                            : cell.NumericCellValue.ToString();
                    case CellType.Boolean: return cell.BooleanCellValue ? "1" : "0";
                    case CellType.Formula:
                        try { return cell.StringCellValue?.Trim() ?? ""; } catch { return ""; }
                    default: return string.Empty;
                }
            }
            catch { return string.Empty; }
        }
    }
}
