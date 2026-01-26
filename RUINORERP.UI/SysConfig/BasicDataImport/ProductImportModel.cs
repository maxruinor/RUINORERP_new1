using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 产品导入数据模型
    /// </summary>
    public class ProductImportModel
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        
        /// <summary>
        /// 产品分类路径（例如：电子产品>手机>智能手机）
        /// </summary>
        public string CategoryPath { get; set; }
        
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }
        
        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification { get; set; }
        
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { get; set; }
        
        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockQuantity { get; set; }
        
        /// <summary>
        /// 产品描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 图片路径列表（多个图片路径用分号分隔）
        /// </summary>
        public string ImagePaths { get; set; }
        
        /// <summary>
        /// 状态（1：启用，0：禁用）
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 导入行号
        /// </summary>
        public int RowNumber { get; set; }
        
        /// <summary>
        /// 导入状态（成功/失败）
        /// </summary>
        public bool ImportStatus { get; set; }
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
    
    /// <summary>
    /// 分类导入数据模型
    /// </summary>
    public class CategoryImportModel
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        
        /// <summary>
        /// 分类路径（例如：电子产品>手机>智能手机）
        /// </summary>
        public string CategoryPath { get; set; }
        
        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode { get; set; }
        
        /// <summary>
        /// 父分类ID
        /// </summary>
        public long ParentId { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder { get; set; }
        
        /// <summary>
        /// 状态（1：启用，0：禁用）
        /// </summary>
        public int Status { get; set; }
    }
    
    /// <summary>
    /// 导入结果模型
    /// </summary>
    public class ImportResult
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// 成功记录数
        /// </summary>
        public int SuccessCount { get; set; }
        
        /// <summary>
        /// 失败记录数
        /// </summary>
        public int FailedCount { get; set; }
        
        /// <summary>
        /// 失败记录列表
        /// </summary>
        public List<ProductImportModel> FailedRecords { get; set; }
        
        /// <summary>
        /// 导入耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }
        
        /// <summary>
        /// 导入开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// 导入结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}