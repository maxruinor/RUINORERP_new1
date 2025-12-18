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
                    System.Diagnostics.Debug.WriteLine("服务提供者未初始化");
                    return;
                }

                // 获取业务编码服务
                var bizCodeService = serviceProvider.GetService<IBizCodeGenerateService>();
                if (bizCodeService == null)
                {
                    System.Diagnostics.Debug.WriteLine("无法获取业务编码服务");
                    return;
                }

                // 测试生成基础SKU编号
                System.Diagnostics.Debug.WriteLine("测试生成基础SKU编号...");
                string skuNo = await bizCodeService.GenerateBaseInfoNoAsync(BaseInfoType.SKU_No);
                System.Diagnostics.Debug.WriteLine($"生成的SKU编号: {skuNo}");

                // 测试生成业务单据编号
                System.Diagnostics.Debug.WriteLine("\n测试生成业务单据编号...");
                string orderNo = await bizCodeService.GenerateBizBillNoAsync(BizType.销售订单);
                System.Diagnostics.Debug.WriteLine($"生成的销售订单编号: {orderNo}");

                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"测试过程中发生错误: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }
        }
    }
}