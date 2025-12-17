using Microsoft.EntityFrameworkCore.Infrastructure;
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
        /// 生成基于属性的SKU编码
        /// 格式：产品编号-属性1代码-属性2代码-...-序号
        /// 示例：P123-CRED-SMAL-0001（表示产品P123，红色，小号，序号0001）
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="attributeInfos">属性信息列表</param>
        /// <returns>生成的SKU编码</returns>
        public string GenerateSKUCodeAsync(tb_Prod prod)
        {

            string skuCode = string.Empty;

            return skuCode;
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
        /// 生成默认的SKU编码（适用于无属性或简单产品）
        /// 在特殊情况或异常发生时使用的备用编码生成方法
        /// 格式：产品编号-D-序号 或使用系统默认的SKU编号生成规则
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>默认生成的SKU编码</returns>
        private string GenerateDefaultSKUCodeAsync(string productCode)
        {
            try
            {
                // 如果提供了有效的产品编号，则生成基于产品编号的默认SKU编码
                if (!string.IsNullOrEmpty(productCode) && productCode != "ERROR")
                {
                    StringBuilder skuBuilder = new StringBuilder(productCode);

                    // 生成序号部分
                    string sequenceNo = _bnrFactory.Create("{Hex:yyMM}{DB:SKU_No/0000}");
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


        public string GenerateProdNoAsync(tb_Prod prod)
        {
            try
            {
                //产品编号（ProductNo）：
                //-格式： [分类代码] - [产品类型] - [年月] - [流水号]
                //- 示例： ELEC - PHON - 2305 - 0012
                //- 优点：保留分类信息，便于管理，同时包含时间和序列信息确保唯一性

                string prodno = _bnrFactory.Create("{CN:{" + prod.tb_prodcategories.Category_name.Substring(0, 3) + "}}{DB:ProdNo/0000}");
                return prodno;
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

        public string GenerateShortCodeAsync(tb_Prod prod)
        {
            try
            {
                // 如果提供了有效的产品编号，则生成基于产品编号的默认SKU编码
                //-格式： [产品编号] - [关键属性代码]
                //- 示例： ELEC - PHON - 2305 - 0012 - 5G
                //- 优点：将具有相同关键属性的产品变体归为一档，便于库存和销售管理
                if (prod.tb_prodcategories == null && prod.Category_ID > 0)
                {
                    prod.tb_prodcategories = Business.Cache.EntityCacheHelper.GetEntity<tb_ProdCategories>(prod.Category_ID);
                }
                

                var Cate = prod.tb_prodcategories.Category_name.Substring(0, 3);

                string shortcode = _bnrFactory.Create("{CN:{" + Cate + "}}{DB:SHortCode/000}");

                return $"{shortcode}";
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

    }
}