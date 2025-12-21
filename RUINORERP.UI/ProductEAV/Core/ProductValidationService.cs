using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.ProductEAV.Core
{
    /// <summary>
    /// 产品编辑的数据验证服务
    /// 职责：集中管理产品编辑过程中的所有验证规则
    /// </summary>
    public class ProductValidationService
    {
        /// <summary>
        /// 验证属性维度一致性
        /// 问题：所有SKU的属性个数必须相同
        /// </summary>
        public ValidationResult ValidateAttributeDimensionConsistency(List<tb_ProdDetail> details)
        {
            if (details == null || details.Count == 0)
            {
                return ValidationResult.Success();
            }

            var firstDetail = details.FirstOrDefault();
            if (firstDetail?.tb_Prod_Attr_Relations == null || firstDetail.tb_Prod_Attr_Relations.Count == 0)
            {
                return ValidationResult.Success();
            }

            int referenceDimension = firstDetail.tb_Prod_Attr_Relations.Count;

            foreach (var detail in details)
            {
                if (detail.tb_Prod_Attr_Relations == null)
                {
                    return ValidationResult.Failure($"产品详情[{detail.SKU}]的属性关系为空");
                }

                if (detail.tb_Prod_Attr_Relations.Count != referenceDimension)
                {
                    return ValidationResult.Failure(
                        $"产品详情[{detail.SKU}]的属性维度({detail.tb_Prod_Attr_Relations.Count})与标准维度({referenceDimension})不一致");
                }
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// 验证属性值重复
        /// 问题：不能有两个SKU拥有完全相同的属性值组合
        /// </summary>
        public ValidationResult ValidateDuplicateAttributes(List<tb_ProdDetail> details)
        {
            if (details == null || details.Count <= 1)
            {
                return ValidationResult.Success();
            }

            var attributeSignatures = new Dictionary<string, string>();

            foreach (var detail in details)
            {
                if (detail.tb_Prod_Attr_Relations == null || detail.tb_Prod_Attr_Relations.Count == 0)
                {
                    continue;
                }

                var signature = string.Join(",",
                    detail.tb_Prod_Attr_Relations
                        .OrderBy(r => r.Property_ID)
                        .Select(r => r.tb_prodpropertyvalue?.PropertyValueName ?? "")
                        .Where(n => !string.IsNullOrEmpty(n)));

                if (attributeSignatures.ContainsKey(signature))
                {
                    return ValidationResult.Failure(
                        $"发现属性值重复：SKU[{detail.SKU}]与SKU[{attributeSignatures[signature]}]有相同的属性组合");
                }

                attributeSignatures[signature] = detail.SKU;
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// 验证必填项
        /// 问题：产品必须有名称、必须有规格或分类
        /// </summary>
        public ValidationResult ValidateRequiredFields(tb_Prod product)
        {
            if (product == null)
            {
                return ValidationResult.Failure("产品对象为空");
            }

            if (string.IsNullOrWhiteSpace(product.CNName))
            {
                return ValidationResult.Failure("产品名称不能为空");
            }

            if (product.tb_ProdDetails == null || product.tb_ProdDetails.Count == 0)
            {
                return ValidationResult.Failure("产品必须至少有一个SKU");
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// 验证属性关系完整性
        /// 问题：所有SKU都必须包含所有应有的属性值
        /// </summary>
        public ValidationResult ValidateAttributeRelationCompleteness(tb_Prod product)
        {
            if (product?.tb_ProdDetails == null || product.tb_ProdDetails.Count == 0)
            {
                return ValidationResult.Success();
            }

            var firstDetail = product.tb_ProdDetails.FirstOrDefault();
            if (firstDetail?.tb_Prod_Attr_Relations == null || firstDetail.tb_Prod_Attr_Relations.Count == 0)
            {
                return ValidationResult.Success();
            }

            var requiredPropertyIds = firstDetail.tb_Prod_Attr_Relations
                .Where(r => r.Property_ID.HasValue)
                .Select(r => r.Property_ID.Value)
                .Distinct()
                .ToList();

            foreach (var detail in product.tb_ProdDetails)
            {
                if (detail.tb_Prod_Attr_Relations == null)
                {
                    return ValidationResult.Failure($"SKU[{detail.SKU}]缺少属性关系");
                }

                var currentPropertyIds = detail.tb_Prod_Attr_Relations
                    .Where(r => r.Property_ID.HasValue)
                    .Select(r => r.Property_ID.Value)
                    .Distinct()
                    .ToList();

                var missingProperties = requiredPropertyIds.Except(currentPropertyIds).ToList();
                if (missingProperties.Count > 0)
                {
                    return ValidationResult.Failure(
                        $"SKU[{detail.SKU}]缺少属性：{string.Join(",", missingProperties)}");
                }
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// 综合验证所有规则
        /// 执行顺序很重要：必填项 → 维度一致性 → 完整性 → 重复检查
        /// </summary>
        public ValidationResult ValidateAll(tb_Prod product)
        {
            var requiredResult = ValidateRequiredFields(product);
            if (!requiredResult.IsSuccess)
            {
                return requiredResult;
            }

            var dimensionResult = ValidateAttributeDimensionConsistency(product.tb_ProdDetails);
            if (!dimensionResult.IsSuccess)
            {
                return dimensionResult;
            }

            var completenessResult = ValidateAttributeRelationCompleteness(product);
            if (!completenessResult.IsSuccess)
            {
                return completenessResult;
            }

            var duplicateResult = ValidateDuplicateAttributes(product.tb_ProdDetails);
            if (!duplicateResult.IsSuccess)
            {
                return duplicateResult;
            }

            return ValidationResult.Success();
        }
    }

    /// <summary>
    /// 验证结果模型
    /// </summary>
    public class ValidationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static ValidationResult Success()
        {
            return new ValidationResult { IsSuccess = true, Message = "" };
        }

        public static ValidationResult Failure(string message)
        {
            return new ValidationResult { IsSuccess = false, Message = message };
        }
    }
}
