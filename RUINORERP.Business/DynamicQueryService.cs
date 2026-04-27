using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    /// <summary>
    /// 通用动态查询服务 - 为数据清理和导入提供预览支持
    /// </summary>
    public class DynamicQueryService
    {
        private readonly ISqlSugarClient _db;

        public DynamicQueryService(ISqlSugarClient db)
        {
            _db = db;
        }

        /// <summary>
        /// 根据表名和条件动态查询数据（返回 DataTable 以便 UI 展示）
        /// </summary>
        public async Task<DataTable> QueryTableAsync(string tableName, Dictionary<string, object> conditions, int topCount = 100)
        {
            if (string.IsNullOrEmpty(tableName)) return new DataTable();

            var sql = $"SELECT TOP {topCount} * FROM [{tableName}]";
            var whereClauses = new List<string>();
            var parameters = new List<SugarParameter>();

            if (conditions != null && conditions.Any())
            {
                int index = 0;
                foreach (var kvp in conditions)
                {
                    if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.ToString()))
                    {
                        whereClauses.Add($"[{kvp.Key}] LIKE @p{index}");
                        parameters.Add(new SugarParameter($"@p{index}", $"%{kvp.Value}%"));
                        index++;
                    }
                }
            }

            if (whereClauses.Any())
            {
                sql += " WHERE " + string.Join(" AND ", whereClauses);
            }

            return await _db.Ado.GetDataTableAsync(sql, parameters.ToArray());
        }

        /// <summary>
        /// 获取符合条件的总行数
        /// </summary>
        public async Task<int> GetCountAsync(string tableName, string customWhereClause)
        {
            var sql = $"SELECT COUNT(1) FROM [{tableName}]";
            if (!string.IsNullOrEmpty(customWhereClause))
            {
                sql += $" WHERE {customWhereClause}";
            }
            return await _db.Ado.GetIntAsync(sql);
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        public async Task<int> ExecuteDeleteAsync(string tableName, string customWhereClause)
        {
            var sql = $"DELETE FROM [{tableName}]";
            if (!string.IsNullOrEmpty(customWhereClause))
            {
                sql += $" WHERE {customWhereClause}";
            }
            return await _db.Ado.ExecuteCommandAsync(sql);
        }

        /// <summary>
        /// 获取表的列定义信息（用于 UI 生成搜索框）
        /// </summary>
        public List<DynamicQueryColumnInfo> GetTableColumns(string tableName)
        {
            var columns = new List<DynamicQueryColumnInfo>();
            try
            {
                // 尝试从 Model 程序集中获取实体类型以提取中文描述
                var assembly = Assembly.Load("RUINORERP.Model");
                var entityType = assembly.GetTypes().FirstOrDefault(t => 
                    t.GetCustomAttribute<SugarTable>()?.TableName == tableName || 
                    t.Name == tableName);

                // 使用 SQL Server 系统视图查询列信息（参考：所有外键关系.sql）
                var dbCols = _db.Ado.GetDataTable(
                    @"SELECT 
                        c.name AS COLUMN_NAME,
                        t.name AS DATA_TYPE,
                        ep.value AS COLUMN_COMMENT
                      FROM sys.columns c
                      INNER JOIN sys.tables tb ON c.object_id = tb.object_id
                      INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
                      LEFT JOIN sys.extended_properties ep 
                        ON ep.major_id = c.object_id 
                        AND ep.minor_id = c.column_id 
                        AND ep.name = 'MS_Description'
                      WHERE tb.name = @tn", 
                    new { tn = tableName });

                foreach (DataRow dr in dbCols.Rows)
                {
                    string colName = dr["COLUMN_NAME"].ToString();
                    string description = colName;

                    // 优先使用数据库中的列注释
                    if (dr["COLUMN_COMMENT"] != DBNull.Value && !string.IsNullOrEmpty(dr["COLUMN_COMMENT"].ToString()))
                    {
                        description = dr["COLUMN_COMMENT"].ToString();
                    }
                    else if (entityType != null)
                    {
                        // 如果数据库没有注释，尝试从实体元数据获取
                        var prop = entityType.GetProperties().FirstOrDefault(p => 
                            p.GetCustomAttribute<SugarColumn>()?.ColumnName == colName || 
                            p.Name == colName);
                        
                        if (prop != null)
                        {
                            description = prop.GetCustomAttribute<SugarColumn>()?.ColumnDescription ?? colName;
                        }
                    }

                    columns.Add(new DynamicQueryColumnInfo 
                    { 
                        Name = colName, 
                        Description = description,
                        DataType = dr["DATA_TYPE"].ToString()
                    });
                }
            }
            catch { }
            return columns;
        }
    }

    public class DynamicQueryColumnInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DataType { get; set; }
    }
}
