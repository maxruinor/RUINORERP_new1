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
        /// 解析Excel文件，提取产品数据
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <returns>产品导入模型列表</returns>
        /// <exception cref="ArgumentException">文件路径无效时抛出</exception>
        /// <exception cref="InvalidOperationException">Excel格式错误时抛出</exception>
        public List<ProductImportModel> ParseExcel(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("文件路径不能为空", nameof(filePath));
            }
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的Excel文件不存在", filePath);
            }
            
            var productList = new List<ProductImportModel>();
            
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = null;
                
                // 根据文件扩展名创建不同的Workbook实例
                if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new XSSFWorkbook(fileStream);
                }
                else if (filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new HSSFWorkbook(fileStream);
                }
                else
                {
                    throw new InvalidOperationException("不支持的文件格式，仅支持.xls和.xlsx格式的Excel文件");
                }
                
                // 获取第一个工作表
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet == null)
                {
                    throw new InvalidOperationException("Excel文件中没有工作表");
                }
                
                // 检测标题行
                var headerRow = DetectHeaderRow(sheet);
                if (headerRow == null)
                {
                    throw new InvalidOperationException("无法识别Excel文件的标题行");
                }
                
                // 获取标题行各列的索引映射
                var columnIndexMap = GetColumnIndexMap(headerRow);
                
                // 从第二行开始读取数据（第一行是标题行）
                for (int rowIndex = headerRow.RowNum + 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    var dataRow = sheet.GetRow(rowIndex);
                    if (dataRow == null) continue;
                    
                    // 解析行数据
                    var product = ParseRow(dataRow, columnIndexMap, rowIndex + 1);
                    if (product != null)
                    {
                        productList.Add(product);
                    }
                }
            }
            
            return productList;
        }
        
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
        /// 解析单行数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="columnMap">列索引映射</param>
        /// <param name="rowNumber">行号</param>
        /// <returns>产品导入模型</returns>
        private ProductImportModel ParseRow(IRow row, Dictionary<string, int> columnMap, int rowNumber)
        {
            var product = new ProductImportModel { RowNumber = rowNumber };
            
            try
            {
                // 解析产品编码
                if (columnMap.TryGetValue("ProductCode", out int codeIndex))
                {
                    product.ProductCode = GetCellValue(row.GetCell(codeIndex));
                }
                
                // 解析产品名称
                if (columnMap.TryGetValue("ProductName", out int nameIndex))
                {
                    product.ProductName = GetCellValue(row.GetCell(nameIndex));
                }
                
                // 解析分类路径
                if (columnMap.TryGetValue("CategoryPath", out int categoryIndex))
                {
                    product.CategoryPath = GetCellValue(row.GetCell(categoryIndex));
                }
                
                // 解析品牌名称
                if (columnMap.TryGetValue("BrandName", out int brandIndex))
                {
                    product.BrandName = GetCellValue(row.GetCell(brandIndex));
                }
                
                // 解析规格型号
                if (columnMap.TryGetValue("Specification", out int specIndex))
                {
                    product.Specification = GetCellValue(row.GetCell(specIndex));
                }
                
                // 解析单位
                if (columnMap.TryGetValue("Unit", out int unitIndex))
                {
                    product.Unit = GetCellValue(row.GetCell(unitIndex));
                }
                
                // 解析成本价
                if (columnMap.TryGetValue("CostPrice", out int costIndex))
                {
                    string costValue = GetCellValue(row.GetCell(costIndex));
                    if (!string.IsNullOrWhiteSpace(costValue))
                    {
                        product.CostPrice = decimal.TryParse(costValue, out decimal cost) ? cost : 0;
                    }
                }
                
                // 解析销售价
                if (columnMap.TryGetValue("SalePrice", out int saleIndex))
                {
                    string saleValue = GetCellValue(row.GetCell(saleIndex));
                    if (!string.IsNullOrWhiteSpace(saleValue))
                    {
                        product.SalePrice = decimal.TryParse(saleValue, out decimal sale) ? sale : 0;
                    }
                }
                
                // 解析库存数量
                if (columnMap.TryGetValue("StockQuantity", out int stockIndex))
                {
                    string stockValue = GetCellValue(row.GetCell(stockIndex));
                    if (!string.IsNullOrWhiteSpace(stockValue))
                    {
                        product.StockQuantity = int.TryParse(stockValue, out int stock) ? stock : 0;
                    }
                }
                
                // 解析产品描述
                if (columnMap.TryGetValue("Description", out int descIndex))
                {
                    product.Description = GetCellValue(row.GetCell(descIndex));
                }
                
                // 解析图片路径
                if (columnMap.TryGetValue("ImagePaths", out int imageIndex))
                {
                    product.ImagePaths = GetCellValue(row.GetCell(imageIndex));
                }
                
                // 默认状态为启用
                product.Status = 1;
                
                // 如果产品编码或产品名称为空，则跳过该行
                if (string.IsNullOrWhiteSpace(product.ProductCode) && string.IsNullOrWhiteSpace(product.ProductName))
                {
                    return null;
                }
                
                return product;
            }
            catch (Exception ex)
            {
                product.ImportStatus = false;
                product.ErrorMessage = $"行数据解析错误: {ex.Message}";
                return product;
            }
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