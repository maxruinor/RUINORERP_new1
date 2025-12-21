using RUINORERP.Model;
using RUINORERP.UI.ProductEAV.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.ProductEAV.Core
{
    /// <summary>
    /// 属性组合服务
    /// 职责：生成属性组合、分析差异、排序标准化
    /// </summary>
    public class PropertyCombinationService
    {
        private readonly PropertyCombinationHelper _combinationHelper;

        public PropertyCombinationService()
        {
            _combinationHelper = new PropertyCombinationHelper();
        }

        /// <summary>
        /// 根据选中的属性值生成所有组合
        /// 输入：{属性1: [值1,值2], 属性2: [值A,值B]}
        /// 输出：[(值1,值A), (值1,值B), (值2,值A), (值2,值B)]
        /// </summary>
        public List<ProductAttributeInfo> GenerateCombinations(
            Dictionary<long, List<long>> selectedAttributes,
            Dictionary<long, tb_ProdProperty> propertyDict = null,
            Dictionary<long, tb_ProdPropertyValue> valueDict = null)
        {
            if (selectedAttributes == null || selectedAttributes.Count == 0)
            {
                return new List<ProductAttributeInfo>();
            }

            int combinationCount = _combinationHelper.CalculateCombinationCount(selectedAttributes);
            if (combinationCount > 1000)
            {
                throw new InvalidOperationException(
                    $"属性组合数量({combinationCount})超过限制(1000)，请减少选择的属性值");
            }

            return _combinationHelper.GenerateCombinations(selectedAttributes, propertyDict, valueDict);
        }

        /// <summary>
        /// 与现有组合进行差异分析
        /// 返回新增和删除的组合列表
        /// </summary>
        public CombinationDiff AnalyzeCombinationDiff(
            List<ProductAttributeInfo> newCombinations,
            List<tb_Prod_Attr_Relation> existingRelations)
        {
            var diff = new CombinationDiff();

            if (newCombinations == null)
            {
                newCombinations = new List<ProductAttributeInfo>();
            }

            if (existingRelations == null)
            {
                existingRelations = new List<tb_Prod_Attr_Relation>();
            }

            var newKeys = NormalizeCombinationKeys(newCombinations);
            var existingKeys = existingRelations
                .GroupBy(r => r.ProdDetailID)
                .Select(g => string.Join(",",
                    g.OrderBy(r => r.Property_ID)
                     .Select(r => $"{r.PropertyValueID}")))
                .ToList();

            var addedKeys = newKeys.Except(existingKeys).ToList();
            foreach (var key in addedKeys)
            {
                diff.NewCombinations.Add(new ProductAttributeInfo { Key = key });
            }

            var deletedKeys = existingKeys.Except(newKeys).ToList();
            foreach (var key in deletedKeys)
            {
                diff.DeletedCombinations.Add(new ProductAttributeInfo { Key = key });
            }

            return diff;
        }

        /// <summary>
        /// 对组合进行标准化排序
        /// 确保相同的组合无论顺序如何都能被识别
        /// </summary>
        public List<string> NormalizeCombinationKeys(List<ProductAttributeInfo> combinations)
        {
            if (combinations == null || combinations.Count == 0)
            {
                return new List<string>();
            }

            return combinations
                .Select(c => c.GetNormalizedKey())
                .OrderBy(k => k)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// 从属性关系列表提取组合键
        /// </summary>
        public string ExtractCombinationKey(List<tb_Prod_Attr_Relation> relations)
        {
            if (relations == null || relations.Count == 0)
            {
                return "";
            }

            return string.Join(",",
                relations
                    .OrderBy(r => r.Property_ID)
                    .Select(r => $"{r.PropertyValueID}"));
        }
    }

    /// <summary>
    /// 产品属性信息
    /// </summary>
    public class ProductAttributeInfo
    {
        public long PropertyID { get; set; }
        public string PropertyName { get; set; }
        public long PropertyValueID { get; set; }
        public string PropertyValueName { get; set; }
        public string Key { get; set; }

        public string GetNormalizedKey()
        {
            if (!string.IsNullOrEmpty(Key))
            {
                return Key;
            }

            return $"{PropertyID}:{PropertyValueID}:{PropertyValueName}";
        }
    }

    /// <summary>
    /// 组合差异分析结果
    /// </summary>
    public class CombinationDiff
    {
        public List<ProductAttributeInfo> NewCombinations { get; set; } = new List<ProductAttributeInfo>();
        public List<ProductAttributeInfo> DeletedCombinations { get; set; } = new List<ProductAttributeInfo>();
    }
}
