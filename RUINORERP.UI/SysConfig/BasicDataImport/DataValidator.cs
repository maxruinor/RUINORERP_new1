using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据验证器
    /// 负责验证导入数据的合法性
    /// </summary>
    public class DataValidator
    {
        /// <summary>
        /// 验证产品数据
        /// </summary>
        /// <param name="product">产品导入模型</param>
        /// <returns>验证结果，包含验证状态和错误信息</returns>
        public (bool IsValid, string ErrorMessage) ValidateProduct(ProductImportModel product)
        {
            if (product == null)
            {
                return (false, "产品数据不能为空");
            }
            
            // 验证产品编码
            if (string.IsNullOrWhiteSpace(product.ProductCode))
            {
                return (false, "产品编码不能为空");
            }
            
            // 验证产品名称
            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                return (false, "产品名称不能为空");
            }
            
            // 验证产品编码长度
            if (product.ProductCode.Length > 50)
            {
                return (false, "产品编码长度不能超过50个字符");
            }
            
            // 验证产品名称长度
            if (product.ProductName.Length > 100)
            {
                return (false, "产品名称长度不能超过100个字符");
            }
            
            // 验证分类路径
            if (string.IsNullOrWhiteSpace(product.CategoryPath))
            {
                return (false, "产品分类路径不能为空");
            }
            
            // 验证单位
            if (string.IsNullOrWhiteSpace(product.Unit))
            {
                return (false, "产品单位不能为空");
            }
            
            // 验证单位长度
            if (product.Unit.Length > 10)
            {
                return (false, "产品单位长度不能超过10个字符");
            }
            
            // 验证成本价
            if (product.CostPrice < 0)
            {
                return (false, "成本价不能为负数");
            }
            
            // 验证销售价
            if (product.SalePrice < 0)
            {
                return (false, "销售价不能为负数");
            }
            
            // 验证库存数量
            if (product.StockQuantity < 0)
            {
                return (false, "库存数量不能为负数");
            }
            
            // 验证品牌名称长度
            if (!string.IsNullOrWhiteSpace(product.BrandName) && product.BrandName.Length > 50)
            {
                return (false, "品牌名称长度不能超过50个字符");
            }
            
            // 验证规格型号长度
            if (!string.IsNullOrWhiteSpace(product.Specification) && product.Specification.Length > 100)
            {
                return (false, "规格型号长度不能超过100个字符");
            }
            
            // 验证产品描述长度
            if (!string.IsNullOrWhiteSpace(product.Description) && product.Description.Length > 500)
            {
                return (false, "产品描述长度不能超过500个字符");
            }
            
            // 验证状态值
            if (product.Status != 0 && product.Status != 1)
            {
                return (false, "产品状态值无效，只能是0或1");
            }
            
            return (true, string.Empty);
        }
        
        /// <summary>
        /// 批量验证产品数据
        /// </summary>
        /// <param name="products">产品导入模型列表</param>
        /// <returns>验证后的产品列表，包含验证状态和错误信息</returns>
        public List<ProductImportModel> ValidateProducts(List<ProductImportModel> products)
        {
            if (products == null || products.Count == 0)
            {
                return new List<ProductImportModel>();
            }
            
            var validatedProducts = new List<ProductImportModel>();
            
            foreach (var product in products)
            {
                var validationResult = ValidateProduct(product);
                product.ImportStatus = validationResult.IsValid;
                product.ErrorMessage = validationResult.ErrorMessage;
                
                validatedProducts.Add(product);
            }
            
            return validatedProducts;
        }
        
        /// <summary>
        /// 验证分类数据
        /// </summary>
        /// <param name="category">分类导入模型</param>
        /// <returns>验证结果，包含验证状态和错误信息</returns>
        public (bool IsValid, string ErrorMessage) ValidateCategory(CategoryImportModel category)
        {
            if (category == null)
            {
                return (false, "分类数据不能为空");
            }
            
            // 验证分类名称
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return (false, "分类名称不能为空");
            }
            
            // 验证分类名称长度
            if (category.CategoryName.Length > 50)
            {
                return (false, "分类名称长度不能超过50个字符");
            }
            
            // 验证分类编码长度
            if (!string.IsNullOrWhiteSpace(category.CategoryCode) && category.CategoryCode.Length > 50)
            {
                return (false, "分类编码长度不能超过50个字符");
            }
            
            // 验证分类路径
            if (string.IsNullOrWhiteSpace(category.CategoryPath))
            {
                return (false, "分类路径不能为空");
            }
            
            // 验证状态值
            if (category.Status != 0 && category.Status != 1)
            {
                return (false, "分类状态值无效，只能是0或1");
            }
            
            // 验证排序值
            if (category.SortOrder < 0)
            {
                return (false, "分类排序值不能为负数");
            }
            
            return (true, string.Empty);
        }
        
        /// <summary>
        /// 检查产品编码是否重复
        /// </summary>
        /// <param name="products">产品列表</param>
        /// <returns>重复的产品编码列表</returns>
        public List<string> CheckDuplicateProductCodes(List<ProductImportModel> products)
        {
            if (products == null || products.Count == 0)
            {
                return new List<string>();
            }
            
            // 统计每个产品编码出现的次数
            var codeCount = products
                .GroupBy(p => p.ProductCode)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            
            return codeCount;
        }
    }
}