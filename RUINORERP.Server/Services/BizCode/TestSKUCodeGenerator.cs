using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.IServices;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Server;
using RUINORERP.Model.Context;

namespace RUINORERP.Server.Services.BizCode
{
    /// <summary>
    /// 测试SKU编码生成器的类
    /// </summary>
    public class TestSKUCodeGenerator
    {
        /// <summary>
        /// 测试ProductSKUCodeGenerator功能
        /// </summary>
        public static async Task TestProductSKUCodeGenerator()
        {
            try
            {
                // 获取服务提供者
                var serviceProvider = Startup.ServiceProvider;
                if (serviceProvider == null)
                {
                    Console.WriteLine("服务提供者未初始化");
                    return;
                }

                // 获取业务编码服务
                var bizCodeService = serviceProvider.GetService<IBizCodeGenerateService>();
                if (bizCodeService == null)
                {
                    Console.WriteLine("无法获取业务编码服务");
                    return;
                }

                // 测试生成基础SKU编号
                Console.WriteLine("测试生成基础SKU编号...");
                string skuNo = await bizCodeService.GenerateBaseInfoNoAsync(BaseInfoType.SKU_No);
                Console.WriteLine($"生成的SKU编号: {skuNo}");

                // 测试生成业务单据编号
                Console.WriteLine("\n测试生成业务单据编号...");
                string orderNo = await bizCodeService.GenerateBizBillNoAsync(BizType.销售订单);
                Console.WriteLine($"生成的销售订单编号: {orderNo}");

                // 测试生成产品SKU编码（使用ProductSKUCodeGenerator）
                Console.WriteLine("\n测试生成产品SKU编码...");
                var productSKUCodeGenerator = serviceProvider.GetService<ProductSKUCodeGenerator>();
                if (productSKUCodeGenerator != null)
                {
                    // 测试默认SKU编码生成
                    string productSKUNo = await productSKUCodeGenerator.GenerateProductSKUNoAsync(1, "P001", 4);
                    Console.WriteLine($"生成的产品SKU编号: {productSKUNo}");

                    // 测试基于属性的SKU编码生成
                    var attributeValueIds = new List<long> { 1, 2, 3 }; // 示例属性值ID
                    string attributeBasedSKUNo = await productSKUCodeGenerator.GenerateSKUCodeAsync(1, attributeValueIds);
                    Console.WriteLine($"生成的基于属性的SKU编号: {attributeBasedSKUNo}");
                }
                else
                {
                    Console.WriteLine("无法获取ProductSKUCodeGenerator服务");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试过程中发生错误: {ex.Message}");
                Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }
        }
    }
}