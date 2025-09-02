using System.Text.RegularExpressions;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 行级权限规则配置类
    /// 用于存储规则的详细配置信息
    /// </summary>
    public class RuleConfiguration
    {
        /// <summary>
        /// 是否需要关联表
        /// </summary>
        public bool IsJoinRequired { get; set; }

        /// <summary>
        /// 关联表名称
        /// </summary>
        public string JoinTable { get; set; }

        /// <summary>
        /// 关联类型（如：INNER、LEFT、RIGHT等）
        /// </summary>
        public string JoinType { get; set; }

        /// <summary>
        /// 关联条件模板
        /// 支持{0}作为主表名称的占位符
        /// </summary>
        public string JoinOnClauseTemplate { get; set; }

        /// <summary>
        /// 过滤条件SQL子句
        /// </summary>
        public string FilterClause { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 规则描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 基于模板生成关联条件
        /// </summary>
        /// <param name="mainTableName">主表名称</param>
        /// <returns>完整的关联条件SQL</returns>
        public string GenerateJoinOnClause(string mainTableName)
        {
            if (string.IsNullOrEmpty(JoinOnClauseTemplate))
            {
                return string.Empty;
            }

            // 安全地格式化模板，避免SQL注入风险
            return string.Format(JoinOnClauseTemplate, SanitizeTableName(mainTableName));
        }

        /// <summary>
        /// 验证规则配置的有效性
        /// </summary>
        /// <returns>配置是否有效</returns>
        public bool IsValid()
        {
            // 如果需要关联，但关联表为空，则无效
            if (IsJoinRequired && string.IsNullOrEmpty(JoinTable))
                return false;

            // 如果需要关联，但关联条件模板为空，则无效
            if (IsJoinRequired && string.IsNullOrEmpty(JoinOnClauseTemplate))
                return false;

            // 过滤条件不能为空
            if (string.IsNullOrEmpty(FilterClause))
                return false;

            return true;
        }

        /// <summary>
        /// 对表名进行安全处理，防止SQL注入
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>处理后的安全表名</returns>
        private string SanitizeTableName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return string.Empty;
            }

            // 只允许字母、数字、下划线和点
            return Regex.Replace(tableName, @"[^a-zA-Z0-9_.]", "");
        }
    }
}