using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SqlSugar;
using RUINORERP.Model;
using System.Data;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 智能列匹配器
    /// 借鉴 frmDbColumnToExlColumnConfig 的智能匹配算法
    /// 自动匹配 Excel 列与数据库字段，减少用户手动配置工作量
    /// </summary>
    public class SmartColumnMatcher
    {
        private readonly ISqlSugarClient _db;
        
        /// <summary>
        /// 相似度阈值（低于此值不认为是匹配）
        /// </summary>
        public double SimilarityThreshold { get; set; } = 0.65;
        
        public SmartColumnMatcher(ISqlSugarClient db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        /// <summary>
        /// 执行智能列匹配
        /// </summary>
        /// <param name="excelColumns">Excel列名列表</param>
        /// <param name="entityType">目标实体类型</param>
        /// <returns>匹配结果列表</returns>
        public List<ColumnMatchResult> MatchColumns(List<string> excelColumns, Type entityType)
        {
            if (excelColumns == null || excelColumns.Count == 0)
                return new List<ColumnMatchResult>();
            
            // 获取数据库列信息
            var dbColumns = GetDbColumnInfos(entityType);
            
            // 获取实体字段元数据
            var fieldInfos = GetEntityFieldInfos(entityType);
            
            return MatchColumnsInternal(excelColumns, dbColumns, fieldInfos);
        }
        
        /// <summary>
        /// 执行智能列匹配（带数据库列信息）
        /// </summary>
        public List<ColumnMatchResult> MatchColumns(
            List<string> excelColumns, 
            List<DbColumnInfo> dbColumns,
            Type entityType)
        {
            var fieldInfos = GetEntityFieldInfos(entityType);
            return MatchColumnsInternal(excelColumns, dbColumns, fieldInfos);
        }
        
        /// <summary>
        /// 内部匹配逻辑
        /// </summary>
        private List<ColumnMatchResult> MatchColumnsInternal(
            List<string> excelColumns,
            List<DbColumnInfo> dbColumns,
            List<FieldMetadata> fieldInfos)
        {
            var results = new List<ColumnMatchResult>();
            var matchedDbCols = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var excelCol in excelColumns.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                DbColumnInfo bestMatch = null;
                double maxScore = 0;
                
                foreach (var dbCol in dbColumns)
                {
                    if (matchedDbCols.Contains(dbCol.ColumnName))
                        continue;
                    
                    double score = CalculateMatchScore(excelCol, dbCol, fieldInfos);
                    if (score > maxScore && score >= SimilarityThreshold)
                    {
                        maxScore = score;
                        bestMatch = dbCol;
                    }
                }
                
                if (bestMatch != null)
                {
                    results.Add(new ColumnMatchResult
                    {
                        ExcelColumn = excelCol,
                        DbColumn = bestMatch.ColumnName,
                        Score = maxScore,
                        IsPrimaryKey = IsPrimaryKeyCandidate(bestMatch, fieldInfos),
                        DataType = bestMatch.DataType,
                        Description = bestMatch.Description
                    });
                    
                    matchedDbCols.Add(bestMatch.ColumnName);
                }
            }
            
            return results.OrderByDescending(r => r.Score).ToList();
        }
        
        /// <summary>
        /// 计算匹配分数（综合算法）
        /// </summary>
        private double CalculateMatchScore(string excelCol, DbColumnInfo dbCol, List<FieldMetadata> fieldInfos)
        {
            string excelLower = excelCol.ToLower().Trim();
            string dbColLower = dbCol.ColumnName.ToLower().Trim();
            string dbDescLower = dbCol.Description?.ToLower().Trim() ?? "";
            
            // 1. 精确匹配（最高分）
            if (excelLower == dbColLower || excelLower == dbDescLower)
                return 1.0;
            
            // 2. 包含匹配
            if (dbDescLower.Contains(excelLower) || excelLower.Contains(dbDescLower))
                return 0.9;
            
            // 3. 编辑距离相似度（列名）
            double nameScore = CalculateSimilarity(excelLower, dbColLower);
            
            // 4. 编辑距离相似度（描述）
            double descScore = 0;
            if (!string.IsNullOrEmpty(dbDescLower))
            {
                descScore = CalculateSimilarity(excelLower, dbDescLower);
            }
            
            // 5. 字段元数据增强
            double metadataBoost = 0;
            var fieldInfo = fieldInfos?.FirstOrDefault(f => 
                f.ColumnName.Equals(dbCol.ColumnName, StringComparison.OrdinalIgnoreCase) ||
                f.PropertyName.Equals(dbCol.ColumnName, StringComparison.OrdinalIgnoreCase));
            
            if (fieldInfo != null && !string.IsNullOrEmpty(fieldInfo.Description))
            {
                metadataBoost = CalculateSimilarity(excelLower, fieldInfo.Description.ToLower()) * 0.1;
            }
            
            return Math.Max(nameScore, descScore) * 0.8 + metadataBoost;
        }
        
        /// <summary>
        /// 计算字符串相似度（基于编辑距离）
        /// </summary>
        private double CalculateSimilarity(string s, string t)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t))
                return 0;
            
            int distance = LevenshteinDistance(s, t);
            int maxLen = Math.Max(s.Length, t.Length);
            
            return maxLen == 0 ? 1.0 : 1.0 - (double)distance / maxLen;
        }
        
        /// <summary>
        /// 莱文斯坦距离（编辑距离）算法
        /// 计算两个字符串之间的最小编辑操作数
        /// </summary>
        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            
            if (n == 0) return m;
            if (m == 0) return n;
            
            // 使用滚动数组优化空间复杂度
            int[] previous = new int[m + 1];
            int[] current = new int[m + 1];
            
            for (int j = 0; j <= m; j++)
                previous[j] = j;
            
            for (int i = 1; i <= n; i++)
            {
                current[0] = i;
                
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    current[j] = Math.Min(
                        Math.Min(previous[j] + 1, current[j - 1] + 1),
                        previous[j - 1] + cost);
                }
                
                // 交换数组
                var temp = previous;
                previous = current;
                current = temp;
            }
            
            return previous[m];
        }
        
        /// <summary>
        /// 判断是否为主键候选字段
        /// </summary>
        private bool IsPrimaryKeyCandidate(DbColumnInfo dbCol, List<FieldMetadata> fieldInfos)
        {
            string[] keyWords = { "code", "no", "number", "编号", "代码", "单号", "id", "key", "编号", "序号" };
            string checkStr = (dbCol.ColumnName + " " + dbCol.Description).ToLower();
            
            // 检查关键字
            bool hasKeyword = keyWords.Any(k => checkStr.Contains(k));
            
            // 检查实体元数据
            var fieldInfo = fieldInfos?.FirstOrDefault(f => 
                f.ColumnName.Equals(dbCol.ColumnName, StringComparison.OrdinalIgnoreCase) ||
                f.PropertyName.Equals(dbCol.ColumnName, StringComparison.OrdinalIgnoreCase));
            
            bool isPrimaryKey = fieldInfo?.IsPrimaryKey ?? false;
            
            return hasKeyword || isPrimaryKey;
        }
        
        /// <summary>
        /// 获取数据库列信息
        /// </summary>
        private List<DbColumnInfo> GetDbColumnInfos(Type entityType)
        {
            var tableName = GetTableName(entityType);
            if (string.IsNullOrEmpty(tableName))
                return new List<DbColumnInfo>();
            
            try
            {
                var dt = _db.Ado.GetDataTable(
                    @"SELECT COLUMN_NAME, DATA_TYPE, COLUMN_COMMENT 
                      FROM information_schema.columns 
                      WHERE table_name = @table_name AND table_schema = DATABASE()",
                    new { table_name = tableName });
                
                var columns = new List<DbColumnInfo>();
                foreach (DataRow dr in dt.Rows)
                {
                    columns.Add(new DbColumnInfo
                    {
                        ColumnName = dr["COLUMN_NAME"].ToString(),
                        DataType = dr["DATA_TYPE"].ToString(),
                        Description = dr["COLUMN_COMMENT"]?.ToString()
                    });
                }
                
                return columns;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取数据库列信息失败: {ex.Message}");
                return new List<DbColumnInfo>();
            }
        }
        
        /// <summary>
        /// 获取实体字段元数据
        /// </summary>
        private List<FieldMetadata> GetEntityFieldInfos(Type entityType)
        {
            if (entityType == null) return new List<FieldMetadata>();
            
            var infos = new List<FieldMetadata>();
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var prop in properties)
            {
                var sugarCol = prop.GetCustomAttribute<SugarColumn>();
                if (sugarCol != null)
                {
                    infos.Add(new FieldMetadata
                    {
                        PropertyName = prop.Name,
                        ColumnName = sugarCol.ColumnName ?? prop.Name,
                        Description = sugarCol.ColumnDescription,
                        IsPrimaryKey = sugarCol.IsPrimaryKey,
                        DataType = sugarCol.ColumnDataType
                    });
                }
            }
            
            return infos;
        }
        
        /// <summary>
        /// 获取表名
        /// </summary>
        private string GetTableName(Type entityType)
        {
            var tableAttr = entityType.GetCustomAttribute<SugarTable>();
            return tableAttr?.TableName ?? entityType.Name;
        }
    }
    
    /// <summary>
    /// 列匹配结果
    /// </summary>
    public class ColumnMatchResult
    {
        /// <summary>
        /// Excel 列名
        /// </summary>
        public string ExcelColumn { get; set; }
        
        /// <summary>
        /// 数据库列名
        /// </summary>
        public string DbColumn { get; set; }
        
        /// <summary>
        /// 匹配分数 (0-1)
        /// </summary>
        public double Score { get; set; }
        
        /// <summary>
        /// 是否为主键候选
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        
        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 获取匹配置信度描述
        /// </summary>
        public string ConfidenceLevel
        {
            get
            {
                if (Score >= 0.9) return "高";
                if (Score >= 0.75) return "中";
                return "低";
            }
        }
    }
    
    /// <summary>
    /// 数据库列信息
    /// </summary>
    public class DbColumnInfo
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public bool IsNullable { get; set; }
    }
    
    /// <summary>
    /// 字段元数据
    /// </summary>
    public class FieldMetadata
    {
        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
        public string Description { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string DataType { get; set; }
    }
}
