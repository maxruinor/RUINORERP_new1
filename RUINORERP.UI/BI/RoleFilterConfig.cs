using System.Collections.Generic;

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// 角色过滤配置类
    /// </summary>
    public class RoleFilterConfig
    {
        /// <summary>
        /// 是否启用角色过滤
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 适用角色ID列表（支持多角色）
        /// </summary>
        public List<long> RoleIds { get; set; } = new List<long>();

        /// <summary>
        /// 过滤字段名称（基础数据表中的字段）
        /// 例如：tb_Prod表的ProductType_ID字段
        /// </summary>
        public string FilterField { get; set; }

        /// <summary>
        /// 过滤值（可以是固定值或参数占位符）
        /// 例如：1 或 {ProductTypeId}
        /// </summary>
        public string FilterValue { get; set; }

        /// <summary>
        /// 是否使用参数化（动态替换占位符）
        /// </summary>
        public bool IsParameterized { get; set; }

        /// <summary>
        /// 生成过滤条件
        /// </summary>
        /// <returns>过滤条件字符串</returns>
        public string GenerateFilterClause()
        {
            if (string.IsNullOrEmpty(FilterField) || string.IsNullOrEmpty(FilterValue))
            {
                return string.Empty;
            }

            string value = FilterValue;
            if (IsParameterized)
            {
                // 使用占位符
                value = FilterValue;
            }

            return $"[{FilterField}] = {value}";
        }
    }
}
