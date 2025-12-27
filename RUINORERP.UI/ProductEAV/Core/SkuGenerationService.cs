using RUINORERP.Business.CommService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.ProductEAV.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.ProductEAV.Core
{
    /// <summary>
    /// SKU生成服务
    /// 职责：SKU码的生成、验证、更新
    /// </summary>
    public class SkuGenerationService
    {
        private readonly ClientBizCodeService _bizCodeService;

        public SkuGenerationService()
        {
            _bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
        }

        /// <summary>
        /// 基于属性组合为产品生成多个SKU
        /// </summary>
        public async Task<List<tb_ProdDetail>> GenerateSkuDetailsFromAttributesAsync(
            tb_Prod product,
            List<ProductAttributeInfo> combinations)
        {
            var details = new List<tb_ProdDetail>();

            if (combinations == null || combinations.Count == 0)
            {
                return details;
            }

            // 按ProdDetailID分组（同一SKU对应的所有属性值）
            var grouped = combinations
                .GroupBy(c => c.PropertyID)
                .ToList();

            foreach (var group in grouped)
            {
                var detail = new tb_ProdDetail
                {
                    ProdBaseID = product.ProdBaseID,
                    SKU = await GenerateSingleSkuAsync(product),
                    ActionStatus = ActionStatus.新增,
                    Is_enabled = true,
                    Is_available = true,
                    Created_at = DateTime.Now,
                    Created_by = MainForm.Instance.AppContext.CurUserInfo?.UserInfo?.Employee_ID ?? 0
                };

                // 为该SKU添加属性关系
                detail.tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>();
                foreach (var attr in group)
                {
                    detail.tb_Prod_Attr_Relations.Add(new tb_Prod_Attr_Relation
                    {
                        ProdBaseID = product.ProdBaseID,
                        ProdDetailID = detail.ProdDetailID,
                        Property_ID = attr.PropertyID,
                        PropertyValueID = attr.PropertyValueID,
                        ActionStatus = ActionStatus.新增
                    });
                }

                details.Add(detail);
            }

            return details;
        }

        /// <summary>
        /// 生成单个SKU码
        /// </summary>
        public async Task<string> GenerateSingleSkuAsync(tb_Prod product, tb_ProdDetail prodDetail = null)
        {
            try
            {
                var sku = await _bizCodeService.GenerateProductSKUCodeAsync(BaseInfoType.SKU_No, product, prodDetail);

                return sku ?? $"SKU_{DateTime.Now.Ticks}";
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog($"SKU生成失败：{ex.Message}，使用时间戳作为降级方案");
                return $"SKU_{DateTime.Now.Ticks}";
            }
        }

        /// <summary>
        /// 验证SKU唯一性
        /// </summary>
        public async Task<bool> ValidateSkuUniqueAsync(string sku, long? excludeProdDetailId = null)
        {
            return await Task.Run(() =>
            {
                var query = MainForm.Instance.AppContext.Db
                    .Queryable<tb_ProdDetail>()
                    .Where(d => d.SKU == sku);

                if (excludeProdDetailId.HasValue)
                {
                    query = query.Where(d => d.ProdDetailID != excludeProdDetailId.Value);
                }

                return !query.Any();
            });
        }

        /// <summary>
        /// 批量更新SKU码
        /// </summary>
        public async Task UpdateSkusAsync(List<tb_ProdDetail> details)
        {
            if (details == null || details.Count == 0)
            {
                return;
            }

            var product = details.FirstOrDefault()?.tb_prod;
            if (product == null)
            {
                return;
            }

            foreach (var detail in details.Where(d => string.IsNullOrWhiteSpace(d.SKU)))
            {
                detail.SKU = await GenerateSingleSkuAsync(product);
            }
        }
    }
}
