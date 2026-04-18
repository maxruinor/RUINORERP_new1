namespace RUINORERP.Business.ImportEngine.Models
{
    /// <summary>
    /// 外键配置信息（Business层专用，用于服务间传递）
    /// </summary>
    public class ForeignKeyConfig
    {
        /// <summary>
        /// 外键表名
        /// </summary>
        public string TableName { get; set; }
        
        /// <summary>
        /// 外键字段名（关联表的主键字段）
        /// </summary>
        public string KeyField { get; set; }
        
        /// <summary>
        /// 来源字段名（用于匹配的显示字段，如供应商名称）
        /// </summary>
        public string SourceField { get; set; }
    }
}
