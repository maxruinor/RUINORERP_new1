using RUINORERP.Global;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 多属性产品导入器
    /// 根据配置将Excel数据导入为多属性产品
    /// </summary>
    public class MultiAttributeProductImporter
    {
        private readonly ISqlSugarClient _db;
        private readonly MultiAttributeImportConfig _config;

        /// <summary>
        /// 导入结果
        /// </summary>
        public class ImportResult
        {
            /// <summary>
            /// 基础产品数
            /// </summary>
            public int BaseProductCount { get; set; }

            /// <summary>
            /// SKU明细数
            /// </summary>
            public int SKUDetailCount { get; set; }

            /// <summary>
            /// 创建的属性类型数
            /// </summary>
            public int PropertyTypeCount { get; set; }

            /// <summary>
            /// 创建的属性值数
            /// </summary>
            public int PropertyValueCount { get; set; }
        }

        public MultiAttributeProductImporter(ISqlSugarClient db, MultiAttributeImportConfig config)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// 导入多属性产品
        /// </summary>
        /// <param name="excelData">Excel数据</param>
        /// <returns>导入结果</returns>
        public async Task<ImportResult> ImportAsync(DataTable excelData)
        {
            var result = new ImportResult();

            // 按分组字段分组
            var groupedData = GroupByField(excelData);

            // 导入每个分组（基础产品）
            foreach (var group in groupedData)
            {
                await ImportBaseProductWithSKUs(group.Value, result);
            }

            return result;
        }

        /// <summary>
        /// 按分组字段对数据进行分组
        /// </summary>
        private Dictionary<string, List<DataRow>> GroupByField(DataTable excelData)
        {
            var grouped = new Dictionary<string, List<DataRow>>();

            foreach (DataRow row in excelData.Rows)
            {
                string groupKey = GetGroupKey(row);

                if (!string.IsNullOrEmpty(groupKey))
                {
                    if (!grouped.ContainsKey(groupKey))
                    {
                        grouped[groupKey] = new List<DataRow>();
                    }
                    grouped[groupKey].Add(row);
                }
            }

            return grouped;
        }

        /// <summary>
        /// 获取分组键
        /// </summary>
        private string GetGroupKey(DataRow row)
        {
            if (string.IsNullOrEmpty(_config.GroupByField))
            {
                return null;
            }

            if (!excelDataContainsColumn(row.Table, _config.GroupByField))
            {
                return null;
            }

            return row[_config.GroupByField]?.ToString();
        }

        /// <summary>
        /// 导入基础产品及其SKU明细
        /// </summary>
        private async Task ImportBaseProductWithSKUs(List<DataRow> productRows, ImportResult result)
        {
            if (productRows == null || productRows.Count == 0)
            {
                return;
            }

            // 提取基础产品信息
            var baseProduct = ExtractBaseProduct(productRows[0]);
            if (baseProduct == null)
            {
                return;
            }

            // 检查产品是否已存在
            var existingProduct = await _db.Queryable<tb_Prod>()
                .Where(p => p.ProductNo == baseProduct.ProductNo)
                .FirstAsync();

            if (existingProduct != null)
            {
                baseProduct.ProdBaseID = existingProduct.ProdBaseID;
                await _db.Updateable(baseProduct).ExecuteCommandAsync();
            }
            else
            {
                baseProduct.ProdBaseID = await _db.Insertable(baseProduct).ExecuteReturnIdentityAsync();
            }

            result.BaseProductCount++;

            // 提取并导入属性和SKU明细
            var attributesInfo = ExtractAttributes(productRows);
            await ImportAttributesAndSKUs(baseProduct.ProdBaseID, productRows, attributesInfo, result);
        }

        /// <summary>
        /// 提取基础产品信息
        /// </summary>
        private tb_Prod ExtractBaseProduct(DataRow firstRow)
        {
            var product = new tb_Prod
            {
                PropertyType = (int)ProductAttributeType.可配置多属性 // 多属性
            };

            // 品号
            if (!string.IsNullOrEmpty(_config.BaseProductFields.ProductNoColumn))
            {
                product.ProductNo = GetColumnValue(firstRow, _config.BaseProductFields.ProductNoColumn);
            }

            // 品名
            string cnName = GetColumnValue(firstRow, _config.BaseProductFields.CNNameColumn ?? "供货商来货品名");
            if (!string.IsNullOrEmpty(cnName))
            {
                // 清理品名（移除属性信息）
                if (!string.IsNullOrEmpty(_config.BaseProductFields.CNNameCleanupPattern))
                {
                    cnName = Regex.Replace(cnName, _config.BaseProductFields.CNNameCleanupPattern, "").Trim();
                }
                product.CNName = cnName;
            }

            // 规格等字段
            if (!string.IsNullOrEmpty(_config.BaseProductFields.SpecificationsColumn))
            {
                product.Specifications = GetColumnValue(firstRow, _config.BaseProductFields.SpecificationsColumn);
            }

            // 可以继续映射其他字段...

            return product;
        }

        /// <summary>
        /// 提取属性信息
        /// </summary>
        private Dictionary<string, HashSet<string>> ExtractAttributes(List<DataRow> productRows)
        {
            var attributesInfo = new Dictionary<string, HashSet<string>>();

            foreach (var row in productRows)
            {
                // 从"供货商来货品名"列提取属性
                string productName = GetColumnValue(row, "供货商来货品名");
                if (!string.IsNullOrEmpty(productName))
                {
                    foreach (var rule in _config.AttributeExtractionRules)
                    {
                        if (string.IsNullOrEmpty(rule.ExcelColumn))
                        {
                            // 从品名列提取
                            string value = rule.ExtractValue(productName);
                            if (!string.IsNullOrEmpty(value))
                            {
                                AddAttributeValue(attributesInfo, rule.AttributeName, value);
                            }
                        }
                    }
                }

                // 从指定的列提取属性
                foreach (var rule in _config.AttributeExtractionRules)
                {
                    if (!string.IsNullOrEmpty(rule.ExcelColumn))
                    {
                        string columnValue = GetColumnValue(row, rule.ExcelColumn);
                        if (!string.IsNullOrEmpty(columnValue))
                        {
                            string value = rule.ExtractValue(columnValue);
                            if (!string.IsNullOrEmpty(value))
                            {
                                AddAttributeValue(attributesInfo, rule.AttributeName, value);
                            }
                        }
                    }
                }
            }

            return attributesInfo;
        }

        /// <summary>
        /// 添加属性值
        /// </summary>
        private void AddAttributeValue(Dictionary<string, HashSet<string>> attributesInfo, string attributeName, string value)
        {
            // 标准化属性值
            string standardizedValue = StandardizeAttributeValue(attributeName, value);

            if (!attributesInfo.ContainsKey(attributeName))
            {
                attributesInfo[attributeName] = new HashSet<string>();
            }

            attributesInfo[attributeName].Add(standardizedValue);
        }

        /// <summary>
        /// 标准化属性值
        /// </summary>
        private string StandardizeAttributeValue(string attributeName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            // 查找标准化映射
            var mapping = _config.AttributeValueMappings
                .FirstOrDefault(m => m.AttributeName == attributeName && m.OriginalValue == value);

            return mapping?.StandardizedValue ?? value;
        }

        /// <summary>
        /// 导入属性和SKU明细
        /// </summary>
        private async Task ImportAttributesAndSKUs(long baseProductId, List<DataRow> productRows,
            Dictionary<string, HashSet<string>> attributesInfo, ImportResult result)
        {
            // 导入属性类型和属性值
            var attributeMappings = await ImportAttributeTypesAndValues(baseProductId, attributesInfo, result);

            // 导入SKU明细
            foreach (var row in productRows)
            {
                await ImportSKUDetail(baseProductId, row, attributeMappings, result);
            }
        }

        /// <summary>
        /// 导入属性类型和属性值
        /// </summary>
        private async Task<Dictionary<string, Dictionary<string, long>>> ImportAttributeTypesAndValues(
            long baseProductId, Dictionary<string, HashSet<string>> attributesInfo, ImportResult result)
        {
            var attributeMappings = new Dictionary<string, Dictionary<string, long>>();

            foreach (var attrPair in attributesInfo)
            {
                string attributeName = attrPair.Key;
                var values = attrPair.Value;

                // 检查或创建属性
                var property = await _db.Queryable<tb_ProdProperty>()
                    .Where(p => p.PropertyName == attributeName)
                    .FirstAsync();

                if (property == null)
                {
                    property = new tb_ProdProperty
                    {
                        PropertyName = attributeName,
                        isdeleted = false
                    };
                    property.Property_ID = await _db.Insertable(property).ExecuteReturnIdentityAsync();
                    result.PropertyTypeCount++;
                }

                // 创建属性值映射
                attributeMappings[attributeName] = new Dictionary<string, long>();

                // 导入属性值
                foreach (var value in values)
                {
                    var propertyValue = await _db.Queryable<tb_ProdPropertyValue>()
                        .Where(v => v.Property_ID == property.Property_ID && v.PropertyValueName == value)
                        .FirstAsync();

                    if (propertyValue == null)
                    {
                        propertyValue = new tb_ProdPropertyValue
                        {
                            Property_ID = property.Property_ID,
                            PropertyValueName = value,
                            isdeleted = false
                        };
                        propertyValue.PropertyValueID = await _db.Insertable(propertyValue).ExecuteReturnIdentityAsync();
                        result.PropertyValueCount++;
                    }

                    attributeMappings[attributeName][value] = propertyValue.PropertyValueID;
                }
            }

            return attributeMappings;
        }

        /// <summary>
        /// 导入SKU明细
        /// </summary>
        private async Task ImportSKUDetail(long baseProductId, DataRow row,
            Dictionary<string, Dictionary<string, long>> attributeMappings, ImportResult result)
        {
            var detail = new tb_ProdDetail
            {
                ProdBaseID = baseProductId
            };

            // 从Excel列映射SKU明细字段
            foreach (var fieldMapping in _config.SKUDetailFields)
            {
                string excelValue = GetColumnValue(row, fieldMapping.ExcelColumn);
                if (!string.IsNullOrEmpty(excelValue))
                {
                    // 根据数据类型转换并赋值
                    SetFieldValue(detail, fieldMapping.SystemField, excelValue, fieldMapping.DataType);
                }
            }

            // 保存SKU明细
            await _db.Insertable(detail).ExecuteCommandAsync();
            result.SKUDetailCount++;

            // 提取并创建属性关系记录
            await CreateAttributeRelations(baseProductId, detail.ProdDetailID, row, attributeMappings);
        }

        /// <summary>
        /// 提取属性组合
        /// </summary>
        private async Task CreateAttributeRelations(long baseProductId, long prodDetailId, DataRow row,
            Dictionary<string, Dictionary<string, long>> attributeMappings)
        {
            foreach (var rule in _config.AttributeExtractionRules)
            {
                // 从品名列提取
                string productName = GetColumnValue(row, "供货商来货品名");
                string value = rule.ExtractValue(productName);

                // 如果品名列没提取到，从指定列提取
                if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(rule.ExcelColumn))
                {
                    string columnValue = GetColumnValue(row, rule.ExcelColumn);
                    value = rule.ExtractValue(columnValue);
                }

                if (!string.IsNullOrEmpty(value))
                {
                    // 标准化属性值
                    value = StandardizeAttributeValue(rule.AttributeName, value);

                    // 获取属性值ID
                    if (attributeMappings.ContainsKey(rule.AttributeName) &&
                        attributeMappings[rule.AttributeName].ContainsKey(value))
                    {
                        long valueId = attributeMappings[rule.AttributeName][value];

                        // 创建属性关系记录
                        var relation = new tb_Prod_Attr_Relation
                        {
                            ProdBaseID = baseProductId,
                            ProdDetailID = prodDetailId,
                            PropertyValueID = valueId,
                            Property_ID = null, // 需要从tb_ProdPropertyValue获取Property_ID
                            isdeleted = false
                        };

                        // 从属性值获取属性ID
                        var propertyValue = await _db.Queryable<tb_ProdPropertyValue>()
                            .Where(v => v.PropertyValueID == valueId)
                            .FirstAsync();

                        if (propertyValue != null)
                        {
                            relation.Property_ID = propertyValue.Property_ID;
                        }

                        await _db.Insertable(relation).ExecuteCommandAsync();
                    }
                }
            }
        }

        /// <summary>
        /// 提取属性组合（已弃用，使用CreateAttributeRelations替代）
        /// </summary>
        private string ExtractAttributeCombination(DataRow row,
            Dictionary<string, Dictionary<string, long>> attributeMappings)
        {
            var combinations = new List<string>();

            foreach (var rule in _config.AttributeExtractionRules)
            {
                // 从品名列提取
                string productName = GetColumnValue(row, "供货商来货品名");
                string value = rule.ExtractValue(productName);

                // 如果品名列没提取到，从指定列提取
                if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(rule.ExcelColumn))
                {
                    string columnValue = GetColumnValue(row, rule.ExcelColumn);
                    value = rule.ExtractValue(columnValue);
                }

                if (!string.IsNullOrEmpty(value))
                {
                    // 标准化属性值
                    value = StandardizeAttributeValue(rule.AttributeName, value);

                    // 获取属性值ID
                    if (attributeMappings.ContainsKey(rule.AttributeName) &&
                        attributeMappings[rule.AttributeName].ContainsKey(value))
                    {
                        long valueId = attributeMappings[rule.AttributeName][value];
                        combinations.Add($"{rule.AttributeName}:{value}:{valueId}");
                    }
                }
            }

            return string.Join(";", combinations);
        }

        /// <summary>
        /// 设置字段值
        /// </summary>
        private void SetFieldValue(tb_ProdDetail detail, string fieldName, string value, string dataType)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            try
            {
                var property = typeof(tb_ProdDetail).GetProperty(fieldName);
                if (property == null)
                {
                    return;
                }

                object convertedValue = ConvertToType(value, dataType);
                property.SetValue(detail, convertedValue);
            }
            catch
            {
                // 转换失败，忽略
            }
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        private object ConvertToType(string value, string dataType)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            switch (dataType?.ToLower())
            {
                case "int":
                case "int32":
                    if (int.TryParse(value, out int intVal))
                        return intVal;
                    break;
                case "long":
                case "int64":
                    if (long.TryParse(value, out long longVal))
                        return longVal;
                    break;
                case "decimal":
                    if (decimal.TryParse(value, out decimal decimalVal))
                        return decimalVal;
                    break;
                case "double":
                    if (double.TryParse(value, out double doubleVal))
                        return doubleVal;
                    break;
                case "bool":
                case "boolean":
                    if (bool.TryParse(value, out bool boolVal))
                        return boolVal;
                    break;
                case "datetime":
                    if (DateTime.TryParse(value, out DateTime dateVal))
                        return dateVal;
                    break;
                default:
                    return value;
            }

            return null;
        }

        /// <summary>
        /// 获取Excel列值
        /// </summary>
        private string GetColumnValue(DataRow row, string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return null;
            }

            if (!excelDataContainsColumn(row.Table, columnName))
            {
                return null;
            }

            return row[columnName]?.ToString();
        }

        /// <summary>
        /// 检查DataTable是否包含指定列
        /// </summary>
        private bool excelDataContainsColumn(DataTable table, string columnName)
        {
            if (table == null || string.IsNullOrEmpty(columnName))
            {
                return false;
            }

            return table.Columns.Contains(columnName);
        }
    }
}
