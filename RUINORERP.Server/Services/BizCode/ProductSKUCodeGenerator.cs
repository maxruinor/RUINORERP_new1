using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using NPinyin;
using RUINORERP.Business.BNR;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Model.Dto;
using RUINORERP.Server.Workflow.WFReminder;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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

        // SKU缓存，用于提高唯一性检查性能
        private static readonly ConcurrentDictionary<string, bool> _skuCache = new ConcurrentDictionary<string, bool>();

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
        /// 格式：类目缩写(2-4位) + 产品基础码(4-6位) + 属性组合码(按需变长)
        /// 示例：CZ0001C-W（表示车载设备类目，产品编号0001，颜色白色）
        /// 支持基于类目的独立序列生成
        /// </summary>
        /// <param name="prod">产品实体，包含产品信息和属性</param>
        /// <returns>生成的SKU编码</returns>
        public string GenerateSKUCodeAsync(tb_Prod prod)
        {
            if (prod == null)
            {
                _logger.LogError("产品信息为空，无法生成SKU编码");
                return GenerateDefaultSKUCodeAsync("");
            }

            try
            {
                ProductAttributeType attributeType = ProductAttributeType.单属性;
                if (prod.PropertyType > 0)
                {
                    attributeType = (ProductAttributeType)prod.PropertyType;
                }

                // 1. 获取类目缩写（2-4位）
                string categoryCode = GetCategoryCode(prod);

                // 2. 获取产品基础码（4-6位） - 基于类目的独立序列
                string productBaseCode = GetProductBaseCodeByCategoryAsync(prod, categoryCode);

                // 3. 获取属性组合码（按需变长）
                string attributeCode = GetAttributeCombinationCode(prod);

                string prodType = GetProdType(prod);
                string attr = string.Empty;
                if (attributeType == ProductAttributeType.可配置多属性)
                {
                    //prod.tb_Prod_Attr_Relations

                    //attr = GenerateAttributeCode();
                }

                // 4. 组合生成SKU编码
                string skuCode = $"{categoryCode}{productBaseCode}{attributeCode}-{prodType}";

                // 5. 检查SKU是否已存在，如果存在则添加序号
                skuCode = EnsureUniqueSKU(skuCode);

                return skuCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成SKU编码时发生错误，产品编号: {ProductNo}", prod.ProductNo);
                return GenerateDefaultSKUCodeAsync(prod.ProductNo ?? "");
            }
        }

        /// <summary>
        /// 更新SKU编码的属性部分（保持序号不变，仅更新属性标识）
        /// 用于产品编辑时，当属性发生变化需要更新SKU编码的情况
        /// </summary>
        /// <param name="existingSku">现有的SKU编码</param>
        /// <param name="prod">更新后的产品实体</param>
        /// <returns>更新后的SKU编码</returns>
        public string UpdateSKUAttributePart(string existingSku, tb_Prod prod)
        {
            if (string.IsNullOrEmpty(existingSku))
            {
                // 如果现有SKU为空，则生成全新的SKU
                return GenerateSKUCodeAsync(prod);
            }

            try
            {
                // 1. 解析现有SKU，提取序号部分
                // 格式：类目代码 + 序号 + 属性代码
                // 示例：CZ0001C-W → 类目:CZ, 序号:0001, 属性:C-W

                // 获取类目代码（通常是2-4个字母）
                string categoryCode = ExtractCategoryCode(existingSku);

                // 获取序号部分（通常是数字）
                string sequencePart = ExtractSequencePart(existingSku);

                // 2. 生成新的属性组合码
                string newAttributeCode = GetAttributeCombinationCode(prod);

                // 3. 组合新的SKU编码
                string newSkuCode = $"{categoryCode}{sequencePart}{newAttributeCode}";

                // 4. 检查SKU是否已存在（排除自身），如果存在则添加序号
                if (newSkuCode != existingSku)
                {
                    newSkuCode = EnsureUniqueSKUExceptSelf(newSkuCode, existingSku);
                }

                return newSkuCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新SKU属性部分时发生错误，现有SKU: {ExistingSku}", existingSku);
                // 出错时返回原有SKU，避免丢失数据
                return existingSku;
            }
        }

        /// <summary>
        /// 初始化或刷新SKU缓存
        /// 在批量生成SKU前调用，可显著提高性能
        /// </summary>
        /// <param name="categoryCode">可选的类目编码，如果提供则只加载该类目的SKU</param>
        /// <returns>异步任务</returns>
        public async Task RefreshSKUCacheAsync()
        {
            try
            {
                // 清空现有缓存
                _skuCache.Clear();

                // 根据是否指定类目编码决定查询范围
                var query = _db.Queryable<tb_ProdDetail>().Select(p => p.SKU);


                // 批量加载SKU到缓存
                var skus = await query.ToListAsync();

                foreach (var sku in skus)
                {
                    _skuCache.TryAdd(sku, true);
                }

                _logger.LogInformation("SKU缓存刷新完成，共加载 {Count} 个SKU", skus.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新SKU缓存失败，类目编码: {CategoryCode}", "全部");
            }
        }

        /// <summary>
        /// 获取类目缩写（1位）
        /// 获取类型名称首字符的拼音首字母大写
        /// </summary>
        /// <param name="prod">产品实体</param>
        /// <returns>类目缩写</returns>
        private string GetProdType(tb_Prod prod)
        {
            try
            {
                // 确保产品类目信息已加载
                if (prod.tb_producttype == null && prod.Type_ID > 0)
                {
                    prod.tb_producttype = Business.Cache.EntityCacheHelper.GetEntity<tb_ProductType>(prod.Type_ID);
                }

                if (prod.tb_producttype == null || string.IsNullOrEmpty(prod.tb_producttype.TypeName))
                {
                    return "U";
                }

                string typeName = prod.tb_producttype.TypeName;

                // 获取第一个字符
                string firstChar = typeName.Substring(0, 1);

                try
                {
                    // 使用NPinyin获取第一个字符的拼音首字母
                    string pinyinInitial = Pinyin.GetInitials(firstChar).ToUpper();

                    // 确保返回单个字符
                    return string.IsNullOrEmpty(pinyinInitial) ? "U" : pinyinInitial.Substring(0, 1);
                }
                catch
                {
                    // 如果NPinyin处理失败，尝试直接返回字符本身
                    return firstChar.ToUpper();
                }
            }
            catch (Exception ex)
            {
                return "U";
            }
        }


        /// <summary>
        /// 获取类目缩写（2-4位）
        /// 使用NPinyin提取类目名称的首字母
        /// </summary>
        /// <param name="prod">产品实体</param>
        /// <returns>类目缩写</returns>
        private string GetCategoryCode(tb_Prod prod)
        {
            try
            {
                // 确保产品类目信息已加载
                if (prod.tb_prodcategories == null && prod.Category_ID > 0)
                {
                    prod.tb_prodcategories = Business.Cache.EntityCacheHelper.GetEntity<tb_ProdCategories>(prod.Category_ID);
                }

                if (prod.tb_prodcategories == null || string.IsNullOrEmpty(prod.tb_prodcategories.Category_name))
                {
                    return "UNC"; // Unknown Category的缩写
                }

                // 使用BNRFactory和ChineseSpellCodeParameter提取类目名称的首字母
                string categoryName = prod.tb_prodcategories.Category_name;
                // 取类目名称的前3个字符，确保生成2-4位的缩写
                string shortCategoryName = categoryName.Length > 3 ? categoryName.Substring(0, 3) : categoryName;
                string categoryCode = _bnrFactory.Create("{CN:" + shortCategoryName + "}");

                // 确保类目代码在2-4位之间
                if (categoryCode.Length > 4)
                {
                    categoryCode = categoryCode.Substring(0, 4);
                }
                else if (categoryCode.Length < 2)
                {
                    // 如果生成的代码太短，使用类目名称前两个字符的拼音
                    categoryCode = _bnrFactory.Create("{CN:" + categoryName.Substring(0, Math.Min(2, categoryName.Length)) + "}");
                    if (categoryCode.Length < 2)
                    {
                        categoryCode = categoryCode.PadRight(2, 'X').Substring(0, 2);
                    }
                }

                return categoryCode.ToUpper();
            }
            catch (Exception ex)
            {
                return "UNC";
            }
        }

        /// <summary>
        /// 获取产品基础码（4-6位）
        /// 从产品编号中提取数字部分并补位到4位
        /// </summary>
        /// <param name="prod">产品实体</param>
        /// <returns>产品基础码</returns>
        private string GetProductBaseCode(tb_Prod prod)
        {
            try
            {
                if (string.IsNullOrEmpty(prod.ProductNo))
                {
                    // 如果没有产品编号，使用产品ID补位
                    return prod.ProductNo.ToString().PadLeft(4, '0').Substring(0, Math.Min(6, prod.ProductNo.ToString().Length));
                }

                // 从产品编号中提取数字部分
                var numericPart = new string(prod.ProductNo.Where(char.IsDigit).ToArray());

                if (string.IsNullOrEmpty(numericPart))
                {
                    // 如果没有数字部分，使用产品编号的哈希码
                    int hash = prod.ProductNo.GetHashCode();
                    numericPart = Math.Abs(hash % 10000).ToString();
                }

                // 补位到4位，但不超过6位
                numericPart = numericPart.PadLeft(4, '0');
                if (numericPart.Length > 6)
                {
                    numericPart = numericPart.Substring(0, 6);
                }

                return numericPart;
            }
            catch (Exception ex)
            {
                return prod.ProductNo.ToString().PadLeft(4, '0').Substring(0, Math.Min(6, prod.ProductNo.ToString().Length));
            }
        }

        /// <summary>
        /// 获取基于类目的产品基础码（4-6位）
        /// 同一类目下的产品编号独立递增，支持按类目分类管理
        /// </summary>
        /// <param name="prod">产品实体</param>
        /// <param name="categoryCode">类目代码</param>
        /// <returns>基于类目的产品基础码</returns>
        private string GetProductBaseCodeByCategoryAsync(tb_Prod prod, string categoryCode)
        {
            try
            {
                // 生成基于类目的序列键
                // 格式: {categoryCode}/0000 - 每个类目有独立的序列
                string rule = $"{{DB:{categoryCode}/0000}}";
                string baseCode = _bnrFactory.Create(rule);

                // 确保生成的代码为4-6位
                if (baseCode.Length > 6)
                {
                    baseCode = baseCode.Substring(0, 6);
                }
                else if (baseCode.Length < 4)
                {
                    baseCode = baseCode.PadLeft(4, '0');
                }

                return baseCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成基于类目的产品基础码失败，类目代码: {CategoryCode}", categoryCode);
                // 降级处理：使用全局序列
                return GetProductBaseCode(prod);
            }
        }

        /// <summary>
        /// 获取属性组合码（按需变长）
        /// 格式：属性类型缩写(1位) + 属性值缩写，多属性则拼接
        /// </summary>
        /// <param name="prod">产品实体</param>
        /// <returns>属性组合码</returns>
        private string GetAttributeCombinationCode(tb_Prod prod)
        {
            try
            {
                var attributeCodes = new List<string>();

                // TODO: 根据实际的产品属性表结构获取属性信息
                // 这里需要根据实际的产品属性表结构来实现
                // 假设产品有属性列表，每个属性有属性名和属性值

                // 示例代码（需要根据实际数据结构调整）:
                // var attributes = GetProductAttributes(prod.Prod_ID);
                // foreach (var attr in attributes)
                // {
                //     string attrCode = GenerateAttributeCode(attr.AttributeName, attr.AttributeValue);
                //     attributeCodes.Add(attrCode);
                // }

                // 临时返回空字符串，表示无属性
                // 实际项目中应该根据产品的属性表来获取属性信息
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取属性组合码失败，产品ProductNo: {ProductNo}", prod.ProductNo);
                return string.Empty;
            }
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
        /// 根据常用属性名称映射到标准前缀
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性前缀代码</returns>
        private string GetPropertyPrefix(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return "X"; // 默认前缀
            }

            // 常用属性名称映射表
            var propertyPrefixMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "颜色", "C" },
                { "colour", "C" },
                { "color", "C" },
                { "型号", "M" },
                { "model", "M" },
                { "款式", "M" },
                { "材质", "T" },
                { "material", "T" },
                { "材料", "T" },
                { "尺寸", "S" },
                { "size", "S" },
                { "规格", "S" },
                { "容量", "R" },
                { "ram", "R" },
                { "内存", "R" },
                { "重量", "W" },
                { "weight", "W" },
                { "版本", "V" },
                { "version", "V" },
                { "长度", "L" },
                { "length", "L" },
                { "宽度", "W" },
                { "width", "W" },
                { "高度", "H" },
                { "height", "H" }
            };

            // 尝试从映射表中获取
            if (propertyPrefixMap.TryGetValue(propertyName, out string prefix))
            {
                return prefix;
            }

            // 如果映射表中没有，则取属性名称的第一个字母
            try
            {
                // 使用NPinyin获取中文首字母
                return Pinyin.GetInitials(propertyName.Substring(0, 1)).ToUpper();
            }
            catch
            {
                // 如果NPinyin处理失败，直接返回第一个字符
                return propertyName.Substring(0, 1).ToUpper();
            }
        }

        /// <summary>
        /// 获取属性值的代码
        /// 根据属性值生成简短的代码表示
        /// </summary>
        /// <param name="propertyValueName">属性值名称</param>
        /// <returns>属性值代码</returns>
        private string GetPropertyValueCode(string propertyValueName)
        {
            if (string.IsNullOrWhiteSpace(propertyValueName))
            {
                return "X"; // 默认值代码
            }

            // 常用属性值映射表
            var valueCodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // 颜色值
                { "红色", "R" },
                { "red", "R" },
                { "蓝色", "B" },
                { "blue", "B" },
                { "绿色", "G" },
                { "green", "G" },
                { "黄色", "Y" },
                { "yellow", "Y" },
                { "黑色", "K" },
                { "black", "K" },
                { "白色", "W" },
                { "white", "W" },
                { "粉色", "P" },
                { "pink", "P" },
                { "紫色", "P" },
                { "purple", "P" },
                { "橙色", "O" },
                { "orange", "O" },
                { "灰色", "G" },
                { "gray", "G" },
                { "银色", "S" },
                { "silver", "S" },
                { "金色", "G" },
                { "gold", "G" },
                
                // 尺寸值
                { "小", "S" },
                { "small", "S" },
                { "中", "M" },
                { "medium", "M" },
                { "大", "L" },
                { "large", "L" },
                { "特大", "XL" },
                { "xlarge", "XL" },
                
                // 材质值
                { "塑料", "SL" },
                { "plastic", "SL" },
                { "金属", "JS" },
                { "metal", "JS" },
                { "木质", "MZ" },
                { "wood", "MZ" },
                { "布料", "BL" },
                { "cloth", "BL" },
                { "皮革", "PG" },
                { "leather", "PG" }
            };

            // 尝试从映射表中获取
            if (valueCodeMap.TryGetValue(propertyValueName, out string code))
            {
                return code;
            }

            // 如果映射表中没有，则尝试生成代码
            try
            {
                // 如果是数字，直接使用
                if (decimal.TryParse(propertyValueName, out decimal numValue))
                {
                    return propertyValueName;
                }

                // 如果包含数字，提取数字部分
                var numericPart = System.Text.RegularExpressions.Regex.Match(propertyValueName, @"\d+");
                if (numericPart.Success)
                {
                    return numericPart.Value;
                }

                // 使用NPinyin获取中文首字母
                return Pinyin.GetInitials(propertyValueName.Substring(0, 1)).ToUpper();
            }
            catch
            {
                // 如果处理失败，返回前两个字符
                return propertyValueName.Length > 2
                    ? propertyValueName.Substring(0, 2).ToUpper()
                    : propertyValueName.ToUpper();
            }
        }

        /// <summary>
        /// 确保SKU编码唯一性
        /// 使用内存缓存和批量查询优化性能，避免频繁数据库访问
        /// </summary>
        /// <param name="baseSkuCode">基础SKU编码</param>
        /// <returns>唯一的SKU编码</returns>
        private string EnsureUniqueSKU(string baseSkuCode)
        {
            try
            {
                // 使用线程安全的字典缓存已存在的SKU，避免重复查询
                if (!_skuCache.ContainsKey(baseSkuCode))
                {
                    // 批量查询相似SKU，减少数据库访问次数
                    var similarSkus = _db.Queryable<tb_ProdDetail>()
                        .Where(p => p.SKU.StartsWith(baseSkuCode))
                        .Select(p => p.SKU)
                        .ToList();

                    // 将查询结果缓存
                    foreach (var sku in similarSkus)
                    {
                        _skuCache.TryAdd(sku, true);
                    }

                    // 如果基础SKU不在缓存中，则它是唯一的
                    if (!_skuCache.ContainsKey(baseSkuCode))
                    {
                        return baseSkuCode;
                    }
                }

                // 如果SKU已存在，生成唯一变体
                return GenerateUniqueVariant(baseSkuCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查SKU唯一性时发生错误，基础SKU: {BaseSkuCode}", baseSkuCode);
                // 如果检查失败，添加时间戳确保唯一性
                return $"{baseSkuCode}{DateTime.Now:HHmm}";
            }
        }

        /// <summary>
        /// 生成SKU的唯一变体
        /// 使用更高效的算法生成唯一变体，避免循环查询
        /// </summary>
        /// <param name="baseSkuCode">基础SKU编码</param>
        /// <returns>唯一的SKU变体</returns>
        private string GenerateUniqueVariant(string baseSkuCode)
        {
            // 获取所有匹配的SKU
            var existingSkus = _skuCache.Keys
                .Where(sku => sku.StartsWith(baseSkuCode))
                .ToList();

            // 如果没有匹配的SKU，直接返回基础SKU
            if (!existingSkus.Any())
            {
                return baseSkuCode;
            }

            // 提取所有已使用的序号
            var usedNumbers = new HashSet<int>();
            foreach (var sku in existingSkus)
            {
                // 尝试从SKU中提取序号部分
                var remainingPart = sku.Substring(baseSkuCode.Length);
                if (int.TryParse(remainingPart, out int number))
                {
                    usedNumbers.Add(number);
                }
            }

            // 找到最小的未使用序号
            int sequence = 1;
            while (usedNumbers.Contains(sequence) && sequence < 100)
            {
                sequence++;
            }

            // 生成新的SKU
            string uniqueSkuCode = $"{baseSkuCode}{sequence:D2}";

            // 将新SKU添加到缓存
            _skuCache.TryAdd(uniqueSkuCode, true);

            return uniqueSkuCode;
        }

        /// <summary>
        /// 生成SKU的唯一变体（排除指定的SKU）
        /// 用于更新SKU属性时，确保新SKU唯一但不与原SKU冲突
        /// </summary>
        /// <param name="baseSkuCode">基础SKU编码</param>
        /// <param name="excludeSku">需要排除的SKU编码</param>
        /// <returns>唯一的SKU变体</returns>
        private string GenerateUniqueVariantExcludeSelf(string baseSkuCode, string excludeSku)
        {
            // 获取所有匹配的SKU（排除自身）
            var existingSkus = _skuCache.Keys
                .Where(sku => sku.StartsWith(baseSkuCode) && sku != excludeSku)
                .ToList();

            // 如果没有匹配的SKU，直接返回基础SKU
            if (!existingSkus.Any())
            {
                return baseSkuCode;
            }

            // 提取所有已使用的序号
            var usedNumbers = new HashSet<int>();
            foreach (var sku in existingSkus)
            {
                // 尝试从SKU中提取序号部分
                var remainingPart = sku.Substring(baseSkuCode.Length);
                if (int.TryParse(remainingPart, out int number))
                {
                    usedNumbers.Add(number);
                }
            }

            // 找到最小的未使用序号
            int sequence = 1;
            while (usedNumbers.Contains(sequence) && sequence < 100)
            {
                sequence++;
            }

            // 生成新的SKU
            string uniqueSkuCode = $"{baseSkuCode}{sequence:D2}";

            // 将新SKU添加到缓存
            _skuCache.TryAdd(uniqueSkuCode, true);

            return uniqueSkuCode;
        }

        /// <summary>
        /// 确保SKU编码唯一性（排除指定的SKU）
        /// 用于更新SKU属性时，确保新SKU唯一但不与原SKU冲突
        /// </summary>
        /// <param name="baseSkuCode">基础SKU编码</param>
        /// <param name="excludeSku">需要排除的SKU编码</param>
        /// <returns>唯一的SKU编码</returns>
        private string EnsureUniqueSKUExceptSelf(string baseSkuCode, string excludeSku)
        {
            try
            {
                // 使用线程安全的字典缓存已存在的SKU，避免重复查询
                // 检查除了excludeSku之外是否还有相同的SKU
                var similarSkus = _skuCache.Keys
                    .Where(sku => sku.StartsWith(baseSkuCode) && sku != excludeSku)
                    .ToList();

                // 如果没有冲突的SKU，则它是唯一的
                if (!similarSkus.Any())
                {
                    return baseSkuCode;
                }

                // 如果SKU已存在，生成唯一变体
                return GenerateUniqueVariantExcludeSelf(baseSkuCode, excludeSku);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查SKU唯一性时发生错误，基础SKU: {BaseSkuCode}", baseSkuCode);
                // 如果检查失败，添加时间戳确保唯一性
                return $"{baseSkuCode}{DateTime.Now:HHmm}";
            }
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

        /// <summary>
        /// 从SKU编码中提取类目代码
        /// </summary>
        /// <param name="sku">SKU编码</param>
        /// <returns>类目代码</returns>
        private string ExtractCategoryCode(string sku)
        {
            if (string.IsNullOrEmpty(sku))
                return "UNK"; // Unknown

            // 类目代码通常是开头的2-4个字母
            var match = System.Text.RegularExpressions.Regex.Match(sku, @"^[A-Z]{2,4}");
            return match.Success ? match.Value : "UNK";
        }

        /// <summary>
        /// 从SKU编码中提取序号部分
        /// </summary>
        /// <param name="sku">SKU编码</param>
        /// <returns>序号部分</returns>
        private string ExtractSequencePart(string sku)
        {
            if (string.IsNullOrEmpty(sku))
                return "0001";

            // 序号部分通常是类目代码后的数字部分
            var match = System.Text.RegularExpressions.Regex.Match(sku, @"^[A-Z]{2,4}(\d+)");
            if (match.Success)
            {
                // 获取数字部分
                string numberPart = match.Groups[1].Value;

                // 补齐到至少4位
                return numberPart.PadLeft(4, '0');
            }

            // 如果无法解析，返回默认值
            return "0001";
        }

        /// <summary>
        /// 从SKU编码中提取属性部分
        /// </summary>
        /// <param name="sku">SKU编码</param>
        /// <returns>属性部分</returns>
        private string ExtractAttributePart(string sku)
        {
            if (string.IsNullOrEmpty(sku))
                return string.Empty;

            // 属性部分通常是数字后的部分
            var match = System.Text.RegularExpressions.Regex.Match(sku, @"^[A-Z]{2,4}\d+(.*)$");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }


        public string GenerateProdNoAsync(tb_Prod prod)
        {
            try
            {
                // 产品编号（ProductNo）格式优化：
                // 原格式：[分类代码] - [产品类型] - [年月] - [流水号]
                // 优化格式：[分类代码][产品序号]
                // 示例：CZ0001（表示车载CZ类，该类第0001个产品）
                // 优点：同一类目下的产品编号独立递增，便于分类管理

                string categoryCode = GetCategoryCode(prod);

                // 基于类目的产品编号规则
                // {categoryCode}/000000 确保每个类目有独立的产品编号序列
                string rule = $"{{DB:{categoryCode}/000000}}";
                string productSequence = _bnrFactory.Create(rule);
                string prodType = GetProdType(prod);

                string prodNo = $"{categoryCode}{productSequence}-{prodType}";
                return prodNo;
            }
            catch (Exception ex)
            {
                // 记录错误日志
                _logger.LogError(ex, "生成产品编号失败");
                // 如果发生异常，返回基于当前时间的唯一编码
                return $"PROD-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}";
            }
        }

        public string GenerateShortCodeAsync(tb_Prod prod)
        {
            try
            {
                // 如果提供了有效的产品编号，则生成基于产品编号的默认SKU编码11
                //-格式： [产品编号] - [关键属性代码]
                //- 示例： ELEC - PHON - 2305 - 0012 - 5G
                //- 优点：将具有相同关键属性的产品变体归为一档，便于库存和销售管理
                var Cate = GetCategoryCode(prod);
                string prodType = GetProdType(prod);
                string shortcode = _bnrFactory.Create("{CN:{" + Cate + "}}{DB:SHortCode/000}");

                return $"{shortcode}-{prodType}";
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