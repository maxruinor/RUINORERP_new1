namespace RUINORERP.Model.ImportEngine.Enums
{
    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// Excel列
        /// </summary>
        ExcelColumn,
        
        /// <summary>
        /// 外键引用
        /// </summary>
        ForeignKey,
        
        /// <summary>
        /// 常量值
        /// </summary>
        Constant,
        
        /// <summary>
        /// 系统生成
        /// </summary>
        SystemGenerated,
        
        /// <summary>
        /// 默认值
        /// </summary>
        DefaultValue
    }
    
    /// <summary>
    /// 去重策略
    /// </summary>
    public enum DeduplicationStrategy
    {
        /// <summary>
        /// 保留首次出现
        /// </summary>
        FirstOccurrence,
        
        /// <summary>
        /// 保留最后一次出现
        /// </summary>
        LastOccurrence
    }
}
