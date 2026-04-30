using System.Collections.Generic;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列映射集合
    /// 提供对列映射的集合管理
    /// </summary>
    public class ColumnMappingCollection : List<ColumnMapping>
    {
        /// <summary>
        /// 初始化列映射集合
        /// </summary>
        public ColumnMappingCollection()
        {
        }

        /// <summary>
        /// 使用现有列表初始化列映射集合
        /// </summary>
        /// <param name="mappings">现有映射列表</param>
        public ColumnMappingCollection(IEnumerable<ColumnMapping> mappings) : base(mappings)
        {
        }
    }
}