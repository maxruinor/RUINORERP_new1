using RUINORERP.Model; using SqlSugar; using System; using System.Data; using System.Diagnostics; using System.Linq;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 外键处理服务
    /// 负责外键值的获取和验证
    /// </summary>
    public class ForeignKeyService
    {
        private readonly ISqlSugarClient _db;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">SqlSugar数据库客户端</param>
        public ForeignKeyService(ISqlSugarClient db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// 获取外键值
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mapping">列映射配置</param>
        /// <param name="rowNumber">行号</param>
        /// <param name="errorMessage">错误消息输出</param>
        /// <returns>外键ID值</returns>
        public object GetForeignKeyValue(DataRow row, ColumnMapping mapping, int rowNumber, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // 从Excel原始列或映射后的数据表中获取代码值
                string foreignKeyValue = null;
                string sourceColumnName = null;
                string sourceColumnDisplayName = null;

                // 优先从指定的外键来源列获取（Excel列）
                if (mapping.ForeignKeySourceColumn != null && !string.IsNullOrEmpty(mapping.ForeignKeySourceColumn.Key))
                {
                    // 使用配置的外键来源列（如"供应商"列）
                    sourceColumnName = mapping.ForeignKeySourceColumn.Key;
                    sourceColumnDisplayName = mapping.ForeignKeySourceColumn.Value ?? sourceColumnName;
                    if (dataTableContainsColumn(row.Table, sourceColumnName))
                    {
                        foreignKeyValue = row[sourceColumnName]?.ToString();
                    }
                }
                else if (!string.IsNullOrEmpty(mapping.ExcelColumn) &&
                         !mapping.ExcelColumn.StartsWith("[") &&
                         !mapping.ExcelColumn.StartsWith("("))
                {
                    // 如果没有指定外键来源列，但映射有Excel列，尝试使用映射的Excel列
                    sourceColumnName = mapping.ExcelColumn;
                    sourceColumnDisplayName = mapping.ExcelColumn;
                    if (dataTableContainsColumn(row.Table, sourceColumnName))
                    {
                        foreignKeyValue = row[sourceColumnName]?.ToString();
                    }
                }
                else
                {
                    // 尝试从系统字段列获取（映射后的列名）
                    sourceColumnName = mapping.SystemField?.Value;
                    sourceColumnDisplayName = mapping.SystemField?.Value;
                    if (dataTableContainsColumn(row.Table, sourceColumnName))
                    {
                        foreignKeyValue = row[sourceColumnName]?.ToString();
                    }
                }

                // 如果获取到了值，查询关联表获取主键ID
                if (!string.IsNullOrEmpty(foreignKeyValue) &&
                    !string.IsNullOrEmpty(mapping.ForeignKeyTable?.Key) &&
                    !string.IsNullOrEmpty(mapping.ForeignKeyField?.Key))
                {
                    // 查找关联表中的对应值（通过代码字段）
                    object foreignKeyId = GetForeignKeyId(foreignKeyValue, mapping.ForeignKeyTable?.Key, mapping.ForeignKeyField?.Key);
                    if (foreignKeyId != null)
                    {
                        return foreignKeyId;
                    }
                    else
                    {
                        errorMessage = $"行 {rowNumber} 外键值 '{foreignKeyValue}' (来源列: {sourceColumnDisplayName ?? sourceColumnName}) " +
                            $"在关联表 {mapping.ForeignKeyTable?.Key} 的字段 {mapping.ForeignKeyField?.Key} 中未找到对应记录。";

                        // 如果是供应商表，提供额外提示
                        if (mapping.ForeignKeyTable?.Key == "tb_CustomerVendor")
                        {
                            errorMessage += "\n\n提示：请确保供应商名称在供应商表中已存在，或者先导入供应商数据。";
                        }

                        return null;
                    }
                }
                else if (mapping.IsRequired)
                {
                    // 如果是必填字段但没有获取到外键值
                    errorMessage = $"行 {rowNumber} 字段 {mapping.SystemField?.Value} 是必填字段，但无法从列 '{sourceColumnDisplayName ?? sourceColumnName}' 获取有效的外键值";
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                errorMessage = $"行 {rowNumber} 处理外键值时发生错误: {ex.Message}";
                return null;
            }
        }

        /// <summary>
        /// 获取外键ID
        /// </summary>
        /// <param name="foreignKeyValue">外键代码值</param>
        /// <param name="relatedTableName">关联表名</param>
        /// <param name="relatedTableField">关联表字段名</param>
        /// <returns>外键主键ID</returns>
        public object GetForeignKeyId(string foreignKeyValue, string relatedTableName, string relatedTableField)
        {
            try
            {
                if (string.IsNullOrEmpty(foreignKeyValue) ||
                    string.IsNullOrEmpty(relatedTableName) ||
                    string.IsNullOrEmpty(relatedTableField))
                {
                    return null;
                }

                // 构建查询SQL，通过代码字段查询主键ID
                // 例如：SELECT ID FROM tb_ProdCategories WHERE CategoryCode = 'CATEGORY001'
                // 例如：SELECT ID FROM tb_CustomerVendor WHERE VendorName = '供应商A'
                string sql = $"SELECT ID FROM {relatedTableName} WHERE {relatedTableField} = @value";

                // 使用参数化查询防止SQL注入
                var parameters = new { value = foreignKeyValue.Trim() };

                // 执行查询
                var result = _db.Ado.GetDataTable(sql, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    // 检查是否有多个匹配记录（唯一性验证）
                    if (result.Rows.Count > 1)
                    {
                        // 对于供应商表等需要唯一性验证的表，记录警告
                        Debug.WriteLine(
                            $"警告：外键查询返回多个结果。表：{relatedTableName}，字段：{relatedTableField}，值：{foreignKeyValue}，匹配数：{result.Rows.Count}");

                        // 如果启用了严格模式，可以抛出异常
                        // throw new Exception($"外键值 '{foreignKeyValue}' 在表 {relatedTableName} 中不唯一，找到 {result.Rows.Count} 条记录");
                    }

                    return result.Rows[0]["ID"];
                }

                // 如果没有找到，尝试模糊匹配（对于供应商名称等可能包含空格的情况）
                if (relatedTableField.ToLower().Contains("name") ||
                    relatedTableField.ToLower().Contains("vendor") ||
                    relatedTableField.ToLower().Contains("supplier"))
                {
                    sql = $"SELECT ID FROM {relatedTableName} WHERE {relatedTableField} LIKE @value";
                    parameters = new { value = $"%{foreignKeyValue.Trim()}%" };
                    result = _db.Ado.GetDataTable(sql, parameters);

                    if (result != null && result.Rows.Count > 0)
                    {
                        if (result.Rows.Count > 1)
                        {
                            Debug.WriteLine(
                                $"警告：模糊匹配返回多个结果。表：{relatedTableName}，字段：{relatedTableField}，值：{foreignKeyValue}");
                        }
                        return result.Rows[0]["ID"];
                    }
                }

                // 如果没有找到，返回null
                return null;
            }
            catch (Exception ex)
            {
                // 记录错误信息
                Debug.WriteLine($"查询外键ID失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 验证外键
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mapping">列映射配置</param>
        /// <param name="rowNumber">行号</param>
        /// <param name="errorMessage">错误消息输出</param>
        /// <returns>是否验证通过</returns>
        public bool ValidateForeignKey(DataRow row, ColumnMapping mapping, int rowNumber, out string errorMessage)
        {
            object foreignKeyId = GetForeignKeyValue(row, mapping, rowNumber, out errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        /// <summary>
        /// 检查DataTable是否包含指定列
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="columnName">列名</param>
        /// <returns>是否包含</returns>
        private bool dataTableContainsColumn(DataTable table, string columnName)
        {
            return table?.Columns.Contains(columnName) ?? false;
        }
    }
}
