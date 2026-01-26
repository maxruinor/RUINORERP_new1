using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;
using RUINORERP.Model;
using SqlSugar;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 产品导入器
    /// 负责处理产品数据的导入，包括产品主信息、详情和图片
    /// </summary>
    public class ProductImporter
    {
        private readonly ISqlSugarClient _db;
        private readonly CategoryImporter _categoryImporter;
        private readonly ImageProcessor _imageProcessor;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">SqlSugar数据库客户端</param>
        /// <param name="categoryImporter">分类导入器</param>
        /// <param name="imageProcessor">图片处理器</param>
        public ProductImporter(ISqlSugarClient db, CategoryImporter categoryImporter, ImageProcessor imageProcessor)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _categoryImporter = categoryImporter ?? throw new ArgumentNullException(nameof(categoryImporter));
            _imageProcessor = imageProcessor ?? throw new ArgumentNullException(nameof(imageProcessor));
        }
        
        /// <summary>
        /// 导入产品数据
        /// </summary>
        /// <param name="product">产品导入模型</param>
        /// <returns>导入结果，包含成功状态和错误信息</returns>
        public (bool Success, string ErrorMessage) ImportProduct(ProductImportModel product)
        {
            if (product == null)
            {
                return (false, "产品数据不能为空");
            }
            
            try
            {
                // 开始事务
                _db.Ado.BeginTran();
                
                // 1. 处理产品分类
                long categoryId = _categoryImporter.ImportCategory(product.CategoryPath);
                
                // 2. 处理产品主信息
                var prodBase = CreateProductBase(product, categoryId);
                long prodBaseId = _db.Insertable(prodBase).ExecuteReturnBigIdentity();
                prodBase.ProdBaseID = prodBaseId;
                
                // 3. 处理产品详情
                var prodDetail = CreateProductDetail(product, prodBaseId);
                _db.Insertable(prodDetail).ExecuteReturnBigIdentity();
                
                // 4. 处理产品图片
                if (!string.IsNullOrWhiteSpace(product.ImagePaths))
                {
                    var imagePaths = _imageProcessor.ExtractImagePaths(product.ImagePaths);
                    var savedImagePaths = _imageProcessor.ProcessAndSaveImages(imagePaths, product.ProductCode);
                    
                    if (savedImagePaths.Count > 0)
                    {
                        // 更新产品主表的图片路径
                        prodBase.ImagesPath = string.Join(";", savedImagePaths);
                        _db.Updateable(prodBase).ExecuteCommand();
                    }
                }
                
                // 提交事务
                _db.Ado.CommitTran();
                
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                // 回滚事务
                _db.Ado.RollbackTran();
                return (false, $"产品导入失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 批量导入产品数据
        /// </summary>
        /// <param name="products">产品导入模型列表</param>
        /// <returns>导入结果统计</returns>
        public ImportResult BatchImportProducts(List<ProductImportModel> products)
        {
            var result = new ImportResult
            {
                TotalCount = products.Count,
                SuccessCount = 0,
                FailedCount = 0,
                FailedRecords = new List<ProductImportModel>(),
                StartTime = DateTime.Now
            };
            
            foreach (var product in products)
            {
                var importResult = ImportProduct(product);
                product.ImportStatus = importResult.Success;
                product.ErrorMessage = importResult.ErrorMessage;
                
                if (importResult.Success)
                {
                    result.SuccessCount++;
                }
                else
                {
                    result.FailedCount++;
                    result.FailedRecords.Add(product);
                }
            }
            
            result.EndTime = DateTime.Now;
            result.ElapsedMilliseconds = (result.EndTime - result.StartTime).Milliseconds;
            
            return result;
        }
        
        /// <summary>
        /// 创建产品主信息实体
        /// </summary>
        /// <param name="product">产品导入模型</param>
        /// <param name="categoryId">分类ID</param>
        /// <returns>产品主信息实体</returns>
        private tb_Prod CreateProductBase(ProductImportModel product, long categoryId)
        {
            return new tb_Prod
            {
                ProductNo = product.ProductCode,
                CNName = product.ProductName,
                Category_ID = categoryId,
                Model = product.Specification,
                Specifications = product.Specification,
                Is_enabled = product.Status == 1,
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
                SourceType = 1, // 默认为自有产品
                Notes = product.Description
            };
        }
        
        /// <summary>
        /// 创建产品详情实体
        /// </summary>
        /// <param name="product">产品导入模型</param>
        /// <param name="prodBaseId">产品主信息ID</param>
        /// <returns>产品详情实体</returns>
        private tb_ProdDetail CreateProductDetail(ProductImportModel product, long prodBaseId)
        {
            return new tb_ProdDetail
            {
                ProdBaseID = prodBaseId,
                SKU = product.ProductCode, // 使用产品编码作为SKU
                // 这里可以根据实际需求添加更多产品详情字段
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now
            };
        }
        
        /// <summary>
        /// 检查产品是否已存在
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>是否已存在</returns>
        public bool IsProductExists(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
            {
                return false;
            }
            
            return _db.Queryable<tb_Prod>()
                .Where(p => p.ProductNo == productCode)
                .Any();
        }
        
        /// <summary>
        /// 获取已存在的产品编码列表
        /// </summary>
        /// <param name="productCodes">产品编码列表</param>
        /// <returns>已存在的产品编码列表</returns>
        public List<string> GetExistingProductCodes(List<string> productCodes)
        {
            if (productCodes == null || productCodes.Count == 0)
            {
                return new List<string>();
            }
            
            return _db.Queryable<tb_Prod>()
                .Where(p => productCodes.Contains(p.ProductNo))
                .Select(p => p.ProductNo)
                .ToList();
        }
    }
}