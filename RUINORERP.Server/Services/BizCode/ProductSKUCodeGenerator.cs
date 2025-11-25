using Microsoft.Extensions.Logging;
using RUINORERP.Business.BNR;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Model.Dto;
using RUINORERP.Server.Workflow.WFReminder;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Services.BizCode
{
    /// <summary>
    /// 产品SKU编码生成器
    /// 负责根据产品的多属性特征生成友好且有意义的SKU编码
    /// </summary>
    public class ProductSKUCodeGenerator
    {
        #region Fields

        private readonly BNRFactory _bnrFactory;
        private readonly ILogger<ProductSKUCodeGenerator> _logger;
        private readonly ISqlSugarClient _db;

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="bnrFactory">编号生成工厂，用于直接生成序号，避免循环依赖</param>
        /// <param name="db">数据库客户端，用于查询产品信息</param>
        public ProductSKUCodeGenerator(ILogger<ProductSKUCodeGenerator> logger, BNRFactory bnrFactory, ISqlSugarClient db)
        {
            _logger = logger;
            _bnrFactory = bnrFactory;
            _db = db;
        }
        #endregion

        /// <summary>
        /// 生成产品SKU编码
        /// 主要入口方法，根据产品ID和属性值ID列表生成包含产品信息和属性特征的友好SKU编码
        /// </summary>
        /// <param name="productId">产品基础ID</param>
        /// <param name="attributeValueIds">属性值ID列表</param>
        /// <returns>生成的SKU编码</returns>
        public async Task<string> GenerateSKUCodeAsync(long productId, List<long> attributeValueIds)
        {
            try
            {
                // 参数验证
                if (productId <= 0)
                {
                    throw new ArgumentException("产品ID无效");
                }

                // 获取产品基础信息和详细信息
                var productBaseInfo = await GetProductBaseInfoAsync(productId);
                var productDetailInfo = await GetProductInfoAsync(productId);

                if (productBaseInfo == null && productDetailInfo == null)
                {
                    throw new ArgumentException($"产品ID {productId} 不存在");
                }

                // 确定使用的产品编号（优先使用产品详细信息中的产品编码）
                string productCode = productDetailInfo?.ProductCode ?? productBaseInfo?.ProductNo ?? "P";

                // 获取属性信息
                var attributeInfos = await GetAttributeInfosAsync(attributeValueIds);

                if (attributeInfos == null || !attributeInfos.Any())
                {
                    // 如果没有属性信息，则使用默认的SKU编码格式
                    return await GenerateDefaultSKUCodeAsync(productCode);
                }

                // 生成包含属性信息的SKU编码
                return await GenerateAttributeBasedSKUCodeAsync(productCode, attributeInfos);
            }
            catch (Exception ex)
            {
                // 记录错误并返回默认编码
                // TODO: 添加日志记录
                // 可以使用项目中的日志服务记录错误信息
                // _logger.LogError(ex, "生成SKU编码时发生错误");
                return await GenerateDefaultSKUCodeAsync("ERROR");
            }
        }

        /// <summary>
        /// 生成产品SKU编码（适配方法，用于兼容现有接口）
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="seqLength">序号长度</param>
        /// <returns>生成的产品SKU编码</returns>
        public async Task<string> GenerateProductSKUNoAsync(long productId, string productCode, int seqLength = 4)
        {
            // 调用现有的GenerateSKUCodeAsync方法，传入空的属性值ID列表
            return await GenerateSKUCodeAsync(productId, new List<long>());
        }

        /// <summary>
        /// 获取产品基础信息
        /// </summary>
        /// <param name="productId">产品基础ID</param>
        /// <returns>产品基础信息</returns>
        private async Task<tb_Prod> GetProductBaseInfoAsync(long productId)
        {
            if (productId <= 0)
            {
                return null;
            }

            try
            {
                // 查询产品基本信息及其相关数据
                return await _db.Queryable<tb_Prod>()
                    .Where(p => p.ProdBaseID == productId)
                    .FirstAsync();
            }
            catch (Exception ex)
            {
                // 记录错误并返回null
                _logger.LogError(ex, "获取产品基础信息时发生错误");
                return null;
            }
        }

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品信息</returns>
        private async Task<ProductInfo> GetProductInfoAsync(long productId)
        {
            if (productId <= 0)
            {
                return null;
            }

            try
            {
                // 查询产品基本信息及其分类
                var product = await _db.Queryable<tb_Prod>()
                    .Includes(p => p.tb_prodcategories)
                    .Where(p => p.ProdBaseID == productId)
                    .FirstAsync();

                if (product == null)
                {
                    return null;
                }

                return new ProductInfo
                {
                    ProductId = product.ProdBaseID,
                    ProductCode = product.ShortCode,
                    ProductName = product.CNName,
                    CategoryId = product.Category_ID.Value,
                    CategoryName = product.tb_prodcategories?.Category_name
                };
            }
            catch (Exception ex)
            {
                // 记录错误并返回null
                // TODO: 添加日志记录
                // _logger.LogError(ex, "获取产品信息时发生错误");
                return null;
            }
        }

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="attributeValueIds">属性值ID列表</param>
        /// <returns>属性信息列表</returns>
        private async Task<List<AttributeInfo>> GetAttributeInfosAsync(List<long> attributeValueIds)
        {
            if (attributeValueIds == null || !attributeValueIds.Any())
            {
                return new List<AttributeInfo>();
            }

            try
            {
                // 使用批量查询提高性能
                var attributeValues = await _db.Queryable<tb_ProdPropertyValue>()
                    .Includes(v => v.tb_prodproperty)
                    .Where(v => attributeValueIds.Contains(v.PropertyValueID))
                    .ToListAsync();

                var attributeInfos = attributeValues
                    .Where(v => v.tb_prodproperty != null)
                    .Select(v => new AttributeInfo
                    {
                        PropertyName = v.tb_prodproperty.PropertyName,
                        PropertyValueName = v.PropertyValueName,
                        SortOrder = v.tb_prodproperty.SortOrder ?? 0,
                        PropertyID = v.Property_ID,
                        PropertyValueID = v.PropertyValueID
                    })
                    .OrderBy(a => a.SortOrder)
                    .ToList();

                return attributeInfos;
            }
            catch (Exception ex)
            {
                // 记录错误并返回空列表
                 _logger.LogError(ex, "获取属性信息时发生错误");
                return new List<AttributeInfo>();
            }
        }

        /// <summary>
        /// 生成基于属性的SKU编码
        /// 格式：产品编号-属性1代码-属性2代码-...-序号
        /// 示例：P123-CRED-SMAL-0001（表示产品P123，红色，小号，序号0001）
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="attributeInfos">属性信息列表</param>
        /// <returns>生成的SKU编码</returns>
        private async Task<string> GenerateAttributeBasedSKUCodeAsync(string productCode, List<AttributeInfo> attributeInfos)
        {
            // 参数验证
            if (string.IsNullOrEmpty(productCode))
            {
                productCode = "P"; // 默认产品前缀
            }

            StringBuilder skuBuilder = new StringBuilder(productCode);

            // 添加属性代码，按照属性排序顺序添加
            foreach (var attribute in attributeInfos.OrderBy(a => a.SortOrder))
            {
                string attributeCode = GenerateAttributeCode(attribute.PropertyName, attribute.PropertyValueName);
                skuBuilder.Append($"-{attributeCode}");
            }

            // 生成序号部分
            string sequenceNo = await GenerateSequenceNoAsync(skuBuilder.ToString());
            skuBuilder.Append($"-{sequenceNo}");

            // 确保SKU编码不超过最大长度（例如50个字符）
            string skuCode = skuBuilder.ToString();
            if (skuCode.Length > 50)
            {
                // 如果超过长度限制，截取产品编号的后10位，保留所有属性代码和序号
                int keepLength = skuCode.Length - productCode.Length + 10;
                if (keepLength > 50)
                {
                    // 如果仍然超过限制，简化属性代码
                    return GenerateSimplifiedSKUCode(productCode, attributeInfos, sequenceNo);
                }
                return skuCode.Substring(skuCode.Length - keepLength);
            }

            return skuCode;
        }

        /// <summary>
        /// 生成简化的SKU编码（当标准编码过长时使用）
        /// 当完整编码过长时使用，将属性代码进行优化组合
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="attributeInfos">属性信息列表</param>
        /// <param name="sequenceNo">序号</param>
        /// <returns>简化的SKU编码</returns>
        private string GenerateSimplifiedSKUCode(string productCode, List<AttributeInfo> attributeInfos, string sequenceNo)
        {
            // 只使用产品编号的后5位
            string shortProductCode = productCode.Length > 5 ? productCode.Substring(productCode.Length - 5) : productCode;

            StringBuilder skuBuilder = new StringBuilder(shortProductCode);

            // 只使用每个属性的第一个字母
            foreach (var attribute in attributeInfos.OrderBy(a => a.SortOrder))
            {
                char firstChar = char.IsLetter(attribute.PropertyValueName[0]) ?
                    char.ToUpper(attribute.PropertyValueName[0]) :
                    attribute.PropertyValueName[0];
                skuBuilder.Append($"-{firstChar}");
            }

            // 添加序号
            skuBuilder.Append($"-{sequenceNo}");

            return skuBuilder.ToString();
        }

        /// <summary>
        /// 产品信息类
        /// 用于存储产品的基本信息
        /// </summary>
        private class ProductInfo
        {
            /// <summary>
            /// 产品ID，唯一标识产品
            /// </summary>
            public long ProductId { get; set; }

            /// <summary>
            /// 产品编号，用于SKU编码的前缀部分
            /// </summary>
            public string ProductCode { get; set; }

            /// <summary>
            /// 产品名称，用于编码生成和错误提示
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// 分类ID，产品所属分类的唯一标识
            /// </summary>
            public long CategoryId { get; set; }

            /// <summary>
            /// 分类名称，产品所属分类的名称
            /// </summary>
            public string CategoryName { get; set; }
        }

        /// <summary>
        /// 生成属性代码
        /// 将属性信息转换为标准化的属性代码，格式为：属性前缀+属性值代码
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValueName">属性值名称</param>
        /// <returns>标准化的属性代码</returns>
        private string GenerateAttributeCode(string propertyName, string propertyValueName)
        {
            // 生成属性名称的首字母代码
            string propertyPrefix = GetPropertyPrefix(propertyName).ToUpper();

            // 生成属性值的代码
            string valueCode = GetPropertyValueCode(propertyValueName);

            // 组合属性代码，格式：属性首字母+属性值代码
            return $"{propertyPrefix}{valueCode}";
        }

        /// <summary>
        /// 获取属性名称的前缀代码
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性前缀代码</returns>
        private string GetPropertyPrefix(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return "U"; // 未知属性的默认前缀
            }

            // 常见属性的标准前缀映射
            var standardPrefixes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "颜色", "C" },
                { "尺寸", "S" },
                { "Size", "S" },
                { "长度", "L" },
                { "宽度", "W" },
                { "高度", "H" },
                { "材质", "M" },
                { "型号", "T" },
                { "规格", "G" },
                { "版本", "V" },
                { "等级", "L" }
            };

            // 检查是否有标准前缀
            foreach (var kvp in standardPrefixes)
            {
                if (propertyName.Contains(kvp.Key))
                {
                    return kvp.Value;
                }
            }

            // 返回属性名称的首字母
            return propertyName.Substring(0, 1);
        }

        /// <summary>
        /// 生成属性值代码
        /// 将属性值名称标准化处理并生成4位代码，用于属性代码的第二部分
        /// </summary>
        /// <param name="valueName">属性值名称</param>
        /// <returns>4位属性值代码</returns>
        private string GetPropertyValueCode(string valueName)
        {
            if (string.IsNullOrEmpty(valueName))
            {
                return "UNK"; // 未知值的默认代码
            }

            // 标准化处理：移除空格和特殊字符
            string normalized = new string(valueName.Where(char.IsLetterOrDigit).ToArray());
            if (string.IsNullOrEmpty(normalized))
            {
                return "UNK";
            }

            // 处理数字或字母组合的情况
            if (normalized.Any(char.IsDigit) || normalized.All(char.IsLetterOrDigit))
            {
                // 截取前3个字符作为代码
                return normalized.Substring(0, Math.Min(3, normalized.Length)).ToUpper();
            }

            // 对于中文，返回前两个汉字（实际项目中应实现中文转拼音功能）
            // 这里暂时保留中文，但在实际应用中应转换为拼音首字母
            return normalized.Substring(0, Math.Min(2, normalized.Length));
        }

        /// <summary>
        /// 生成序号部分
        /// 使用服务器编码生成服务获取序号，保证唯一性
        /// </summary>
        /// <param name="prefix">前缀部分</param>
        /// <returns>4位序号字符串</returns>
        private async Task<string> GenerateSequenceNoAsync(string prefix)
        {
            try
            {
                // 直接使用BNRFactory生成序号，避免通过IBizCodeGenerateService导致的循环依赖
                // 使用SKU_No类型的编号生成规则，生成序号
                // 使用Create方法替代GenerateNoAsync
                string sequenceNo = _bnrFactory.Create("{S:SK}{Hex:yyMM}{DB:SKU_No/0000}");

                // 提取序号部分（去除前缀和日期部分）
                // 假设格式为 SKyyMM0001，我们提取后四位
                if (sequenceNo.Length >= 4)
                {
                    // 提取最后4位作为序号
                    return sequenceNo.Substring(sequenceNo.Length - 4);
                }

                return sequenceNo;
            }
            catch (Exception ex)
            {
                // 记录错误日志
                _logger.LogError(ex, "生成SKU序号失败");
                // 如果数据库序号生成失败，使用备用方案生成随机序号
                // 确保序号格式统一（4位数字）
                return new Random().Next(1, 9999).ToString("0000");
            }
        }

        /// <summary>
        /// 生成默认的SKU编码（适用于无属性或简单产品）
        /// 在特殊情况或异常发生时使用的备用编码生成方法
        /// 格式：产品编号-D-序号 或使用系统默认的SKU编号生成规则
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>默认生成的SKU编码</returns>
        private async Task<string> GenerateDefaultSKUCodeAsync(string productCode)
        {
            try
            {
                // 如果提供了有效的产品编号，则生成基于产品编号的默认SKU编码
                if (!string.IsNullOrEmpty(productCode) && productCode != "ERROR")
                {
                    StringBuilder skuBuilder = new StringBuilder(productCode);
                    skuBuilder.Append("-D"); // D表示默认(default)

                    // 生成序号部分
                    string sequenceNo = await GenerateSequenceNoAsync(skuBuilder.ToString());
                    skuBuilder.Append($"-{sequenceNo}");

                    return skuBuilder.ToString();
                }

                // 否则直接使用BNRFactory生成SKU编号，避免通过IBizCodeGenerateService导致的循环依赖
                return _bnrFactory.Create("{S:SK}{Hex:yyMM}{DB:SKU_No/0000}");
            }
            catch (Exception ex)
            {
                // 记录错误日志
                _logger.LogError(ex, "生成默认SKU编码失败");
                // 如果发生异常，返回基于当前时间的唯一编码
                // 生成格式：SKU-年月日时分秒-随机数
                return $"SKU-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}";
            }
        }

        /// <summary>
        /// 属性信息类
        /// 用于存储属性名称、属性值名称和排序顺序
        /// </summary>
        private class AttributeInfo
        {
            /// <summary>
            /// 属性ID，用于唯一标识属性
            /// </summary>
            public long PropertyID { get; set; }

            /// <summary>
            /// 属性值ID，用于唯一标识属性值
            /// </summary>
            public long PropertyValueID { get; set; }

            /// <summary>
            /// 属性名称，如"颜色"、"尺寸"
            /// </summary>
            public string PropertyName { get; set; }

            /// <summary>
            /// 属性值名称，如"红色"、"XL"
            /// </summary>
            public string PropertyValueName { get; set; }

            /// <summary>
            /// 排序号，控制属性在编码中的顺序
            /// </summary>
            public int SortOrder { get; set; }
        }
    }
}