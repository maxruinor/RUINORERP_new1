using RUINORERP.Model;
using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.ProductEAV.Core
{
    /// <summary>
    /// 产品编辑服务
    /// 职责：产品编辑的主业务流程协调
    /// </summary>
    public class ProductEditService
    {
        private readonly ProductValidationService _validationService;
        private readonly ProductAttrService _attrService;
        private readonly SkuGenerationService _skuService;
        private readonly PropertyCombinationService _combinationService;

        public ProductEditService()
        {
            _validationService = new ProductValidationService();
            _attrService = new ProductAttrService();
            _skuService = new SkuGenerationService();
            _combinationService = new PropertyCombinationService();
        }

        /// <summary>
        /// 加载产品编辑全量数据
        /// </summary>
        public async Task<tb_Prod> LoadProductForEditAsync(long prodBaseId)
        {
            if (prodBaseId <= 0)
            {
                throw new ArgumentException("产品ID无效");
            }

            return await Task.Run(() =>
            {
                var product = MainForm.Instance.AppContext.Db
                    .Queryable<tb_Prod>()
                    .Where(p => p.ProdBaseID == prodBaseId)
                    .Includes(p => p.tb_ProdDetails)
                    .Includes(p => p.tb_Prod_Attr_Relations)
                    .FirstAsync();

                if (product == null)
                {
                    throw new InvalidOperationException($"产品ID {prodBaseId} 不存在");
                }

                return product;
            });
        }

        /// <summary>
        /// 保存产品编辑结果
        /// </summary>
        public async Task<SaveProductResult> SaveProductEditAsync(tb_Prod product)
        {
            try
            {
                // 第一步：验证数据完整性
                var validationResult = _validationService.ValidateAll(product);
                if (!validationResult.IsSuccess)
                {
                    return SaveProductResult.Failure(validationResult.Message);
                }

                // 第二步：更新SKU码（确保所有SKU有值）
                await _skuService.UpdateSkusAsync(product.tb_ProdDetails);

                // 第三步：标记为修改状态
                product.ActionStatus = ActionStatus.修改;
                product.Modified_at = DateTime.Now;
                product.Modified_by = MainForm.Instance.AppContext.CurUserInfo?.UserInfo?.Employee_ID ?? 0;

                // 第四步：保存到数据库
                var db = MainForm.Instance.AppContext.Db;
                var result = await Task.Run(() => db.Updateable(product).ExecuteCommandAsync());

                if (result > 0)
                {
                    _attrService.ClearCache();
                    return SaveProductResult.Success("产品编辑保存成功");
                }
                else
                {
                    return SaveProductResult.Failure("产品编辑保存失败，请重试");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog($"产品编辑保存异常：{ex.Message}");
                return SaveProductResult.Failure($"产品编辑保存异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 验证产品数据完整性
        /// </summary>
        public ValidationResult ValidateProductData(tb_Prod product)
        {
            return _validationService.ValidateAll(product);
        }
    }

    /// <summary>
    /// 产品保存结果
    /// </summary>
    public class SaveProductResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string ErrorMsg { get; set; }

        public static SaveProductResult Success(string message = "成功")
        {
            return new SaveProductResult
            {
                Succeeded = true,
                Message = message,
                ErrorMsg = ""
            };
        }

        public static SaveProductResult Failure(string errorMsg)
        {
            return new SaveProductResult
            {
                Succeeded = false,
                Message = "",
                ErrorMsg = errorMsg
            };
        }
    }
}
