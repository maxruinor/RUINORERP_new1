using System;using System.Collections.Generic;using System.IO;using System.Linq;using System.Text;using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// Excel数据解析器
    /// 负责从Excel文件中读取并解析产品数据
    /// </summary>
    public class ExcelDataParser
    {
        /// <summary>
        /// 检测标题行
        /// </summary>
        /// <param name="sheet">Excel工作表</param>
        /// <returns>标题行对象</returns>
        private IRow DetectHeaderRow(ISheet sheet)
        {
            // 尝试前5行，寻找包含关键标题的行
            for (int i = 0; i < Math.Min(5, sheet.LastRowNum + 1); i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                
                // 检查是否包含产品编码或产品名称等关键列
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell == null) continue;
                    
                    var cellValue = GetCellValue(cell).Trim();
                    if (IsHeaderCell(cellValue))
                    {
                        return row;
                    }
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// 判断是否为标题单元格
        /// </summary>
        /// <param name="cellValue">单元格值</param>
        /// <returns>是否为标题单元格</returns>
        private bool IsHeaderCell(string cellValue)
        {
            var headerKeywords = new List<string> { "产品编码", "产品名称", "分类路径", "品牌", "规格型号", "单位", "成本价", "销售价", "库存数量", "产品描述", "图片路径" };
            return headerKeywords.Any(keyword => cellValue.Contains(keyword));
        }
        
        /// <summary>
        /// 获取列索引映射
        /// </summary>
        /// <param name="headerRow">标题行</param>
        /// <returns>列名到索引的映射字典</returns>
        private Dictionary<string, int> GetColumnIndexMap(IRow headerRow)
        {
            var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                var cell = headerRow.GetCell(i);
                if (cell == null) continue;
                
                var cellValue = GetCellValue(cell).Trim();
                if (string.IsNullOrWhiteSpace(cellValue)) continue;
                
                // 根据标题内容映射到对应的属性名
                string propertyName = GetPropertyNameByHeader(cellValue);
                if (!string.IsNullOrWhiteSpace(propertyName))
                {
                    columnMap[propertyName] = i;
                }
            }
            
            return columnMap;
        }
        
        /// <summary>
        /// 根据标题获取属性名
        /// </summary>
        /// <param name="header">标题文本</param>
        /// <returns>对应的属性名</returns>
        private string GetPropertyNameByHeader(string header)
        {
            var headerMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "产品编码", "ProductCode" },
                { "产品名称", "ProductName" },
                { "分类路径", "CategoryPath" },
                { "品牌", "BrandName" },
                { "品牌名称", "BrandName" },
                { "规格型号", "Specification" },
                { "单位", "Unit" },
                { "成本价", "CostPrice" },
                { "销售价", "SalePrice" },
                { "库存数量", "StockQuantity" },
                { "产品描述", "Description" },
                { "图片路径", "ImagePaths" }
            };
            
            foreach (var kvp in headerMap)
            {
                if (header.Contains(kvp.Key))
                {
                    return kvp.Value;
                }
            }
            
            return null;
        }
 
        /// <summary>
        /// 获取单元格值
        /// </summary>
        /// <param name="cell">单元格对象</param>
        /// <returns>单元格文本值</returns>
        private string GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }
            
            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue.Trim();
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue.ToString();
                    }
                    else
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    try
                    {
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            return cell.DateCellValue.ToString();
                        }
                        else
                        {
                            return cell.NumericCellValue.ToString();
                        }
                    }
                    catch
                    {
                        return cell.StringCellValue.Trim();
                    }
                default:
                    return string.Empty;
            }
        }
    }
}