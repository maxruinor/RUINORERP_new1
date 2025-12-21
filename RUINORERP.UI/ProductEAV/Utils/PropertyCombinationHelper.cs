using RUINORERP.Model;
using RUINORERP.UI.ProductEAV.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.ProductEAV.Utils
{
    /// <summary>
    /// 属性组合计算工具类
    /// 改进自ArrayCombination.cs，增加对象支持和性能优化
    /// </summary>
    public class PropertyCombinationHelper
    {
        /// <summary>
        /// 计算属性组合的总数，防止过大的组合集合
        /// </summary>
        public int CalculateCombinationCount(
            Dictionary<long, List<long>> selectedAttributes,
            int maxLimit = 1000)
        {
            if (selectedAttributes == null || selectedAttributes.Count == 0)
            {
                return 0;
            }

            int count = 1;
            foreach (var attr in selectedAttributes.Values)
            {
                if (attr == null || attr.Count == 0)
                {
                    continue;
                }

                count *= attr.Count;
                if (count > maxLimit)
                {
                    return -1;
                }
            }

            return count;
        }

        /// <summary>
        /// 生成属性组合（笛卡尔积）
        /// </summary>
        public List<ProductAttributeInfo> GenerateCombinations(
            Dictionary<long, List<long>> selectedAttributes,
            Dictionary<long, tb_ProdProperty> propertyDict = null,
            Dictionary<long, tb_ProdPropertyValue> valueDict = null)
        {
            var result = new List<ProductAttributeInfo>();

            if (selectedAttributes == null || selectedAttributes.Count == 0)
            {
                return result;
            }

            var sortedAttributes = selectedAttributes
                .OrderBy(x => x.Key)
                .ToList();

            var currentCombos = new List<List<ProductAttributeInfo>>();

            // 初始化第一个属性的所有值
            if (sortedAttributes.Count > 0)
            {
                var firstAttr = sortedAttributes[0];
                var firstProp = propertyDict?.ContainsKey(firstAttr.Key) == true ? propertyDict[firstAttr.Key] : null;
                
                foreach (var valueId in firstAttr.Value)
                {
                    var value = valueDict?.ContainsKey(valueId) == true ? valueDict[valueId] : null;
                    currentCombos.Add(new List<ProductAttributeInfo>
                    {
                        new ProductAttributeInfo
                        {
                            PropertyID = firstAttr.Key,
                            PropertyName = firstProp?.PropertyName ?? "",
                            PropertyValueID = valueId,
                            PropertyValueName = value?.PropertyValueName ?? ""
                        }
                    });
                }
            }

            // 逐个添加剩余属性（笛卡尔积）
            for (int i = 1; i < sortedAttributes.Count; i++)
            {
                var attr = sortedAttributes[i];
                var prop = propertyDict?.ContainsKey(attr.Key) == true ? propertyDict[attr.Key] : null;
                var newCombos = new List<List<ProductAttributeInfo>>();

                foreach (var comboPart in currentCombos)
                {
                    foreach (var valueId in attr.Value)
                    {
                        var value = valueDict?.ContainsKey(valueId) == true ? valueDict[valueId] : null;
                        var newCombo = new List<ProductAttributeInfo>(comboPart);
                        newCombo.Add(new ProductAttributeInfo
                        {
                            PropertyID = attr.Key,
                            PropertyName = prop?.PropertyName ?? "",
                            PropertyValueID = valueId,
                            PropertyValueName = value?.PropertyValueName ?? ""
                        });
                        newCombos.Add(newCombo);
                    }
                }

                currentCombos = newCombos;
            }

            // 转换为平面结果列表
            foreach (var combo in currentCombos)
            {
                result.AddRange(combo);
            }

            return result;
        }

        /// <summary>
        /// 对组合进行标准化排序
        /// </summary>
        public List<string> NormalizeAndSort(List<ProductAttributeInfo> combinations)
        {
            if (combinations == null || combinations.Count == 0)
            {
                return new List<string>();
            }

            return combinations
                .Select(c => c.GetNormalizedKey())
                .Distinct()
                .OrderBy(k => k)
                .ToList();
        }
    }
}
