using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Document;
using RUINORERP.Business.Document.Converters;
using RUINORERP.Model;

namespace RUINORERP.Tests
{
    /// <summary>
    /// 转换标识符测试类
    /// 用于验证 ConversionIdentifier 在各个环节的正确性
    /// </summary>
    public class ConversionIdentifierTest
    {
        private readonly DocumentConverterFactory _factory;
        private readonly IServiceProvider _serviceProvider;

        public ConversionIdentifierTest(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<DocumentConverterFactory>();
            _factory = new DocumentConverterFactory(serviceProvider, logger);
        }

        /// <summary>
        /// 测试1: 验证转换器注册时的Key生成
        /// </summary>
        public void TestConverterRegistration()
        {
            Console.WriteLine("========== 测试1: 转换器注册Key生成 ==========");

            // 检查预收款单到收付款单的转换器（应该有2个：Normal和Refund）
            var normalConverter = _factory.GetConverter<tb_FM_PreReceivedPayment, tb_FM_PaymentRecord>("Normal");
            var refundConverter = _factory.GetConverter<tb_FM_PreReceivedPayment, tb_FM_PaymentRecord>("Refund");

            Console.WriteLine($"Normal转换器: {normalConverter?.GetType().Name ?? "null"}");
            Console.WriteLine($"Refund转换器: {refundConverter?.GetType().Name ?? "null"}");

            if (normalConverter != null && refundConverter != null)
            {
                Console.WriteLine("✓ 测试通过: 成功获取两个不同的转换器");
                Console.WriteLine($"  - Normal: {normalConverter.GetType().Name}, Identifier: {((IConverterMeta)normalConverter).ConversionIdentifier}");
                Console.WriteLine($"  - Refund: {refundConverter.GetType().Name}, Identifier: {((IConverterMeta)refundConverter).ConversionIdentifier}");
            }
            else
            {
                Console.WriteLine("✗ 测试失败: 无法获取转换器");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 测试2: 验证转换标识符属性
        /// </summary>
        public void TestConversionIdentifierProperty()
        {
            Console.WriteLine("========== 测试2: 转换标识符属性 ==========");

            // 测试 PreReceivedPaymentToPaymentRecordConverter
            var normalConverter = _factory.GetConverter<tb_FM_PreReceivedPayment, tb_FM_PaymentRecord>("Normal");
            if (normalConverter is IConverterMeta meta1)
            {
                Console.WriteLine($"PreReceivedPaymentToPaymentRecordConverter.Identifier: '{meta1.ConversionIdentifier}'");
                Console.WriteLine($"  预期: 'Normal', 实际: '{meta1.ConversionIdentifier}'");
                Console.WriteLine(meta1.ConversionIdentifier == "Normal" ? "  ✓ 正确" : "  ✗ 错误");
            }

            // 测试 PreReceivedPaymentToRefundConverter
            var refundConverter = _factory.GetConverter<tb_FM_PreReceivedPayment, tb_FM_PaymentRecord>("Refund");
            if (refundConverter is IConverterMeta meta2)
            {
                Console.WriteLine($"PreReceivedPaymentToRefundConverter.Identifier: '{meta2.ConversionIdentifier}'");
                Console.WriteLine($"  预期: 'Refund', 实际: '{meta2.ConversionIdentifier}'");
                Console.WriteLine(meta2.ConversionIdentifier == "Refund" ? "  ✓ 正确" : "  ✗ 错误");
            }

            // 测试 SaleOrderCancelConverter
            var cancelConverter = _factory.GetConverter<tb_SaleOrder, tb_SaleOrder>("Cancel");
            if (cancelConverter is IConverterMeta meta3)
            {
                Console.WriteLine($"SaleOrderCancelConverter.Identifier: '{meta3.ConversionIdentifier}'");
                Console.WriteLine($"  预期: 'Cancel', 实际: '{meta3.ConversionIdentifier}'");
                Console.WriteLine(meta3.ConversionIdentifier == "Cancel" ? "  ✓ 正确" : "  ✗ 错误");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 测试3: 验证工厂层的GetAvailableConversions返回正确的标识符
        /// </summary>
        public void TestGetAvailableConversions()
        {
            Console.WriteLine("========== 测试3: GetAvailableConversions ==========");

            var sourceEntity = new tb_FM_PreReceivedPayment();
            var conversions = _factory.GetAvailableConversions(sourceEntity);

            Console.WriteLine($"找到 {conversions.Count} 个转换选项:");
            foreach (var conv in conversions)
            {
                Console.WriteLine($"  - {conv.SourceDocumentType} -> {conv.TargetDocumentType}");
                Console.WriteLine($"    DisplayName: {conv.DisplayName}");
                Console.WriteLine($"    ConversionIdentifier: '{conv.ConversionIdentifier ?? "null"}'");
                Console.WriteLine($"    ConversionType: {conv.ConversionType}");
            }

            // 检查是否同时存在 Normal 和 Refund 转换器
            var normalOption = conversions.FirstOrDefault(c => c.ConversionIdentifier == "Normal");
            var refundOption = conversions.FirstOrDefault(c => c.ConversionIdentifier == "Refund");

            if (normalOption != null && refundOption != null)
            {
                Console.WriteLine("✓ 测试通过: 同时找到 Normal 和 Refund 转换选项");
            }
            else
            {
                Console.WriteLine("✗ 测试失败: 未找到预期的转换选项");
                Console.WriteLine($"  Normal: {(normalOption != null ? "找到" : "未找到")}");
                Console.WriteLine($"  Refund: {(refundOption != null ? "找到" : "未找到")}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 运行所有测试
        /// </summary>
        public void RunAllTests()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("  单据转换标识符测试套件");
            Console.WriteLine("========================================");
            Console.WriteLine();

            try
            {
                TestConverterRegistration();
                TestConversionIdentifierProperty();
                TestGetAvailableConversions();

                Console.WriteLine("========================================");
                Console.WriteLine("  测试完成");
                Console.WriteLine("========================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ 测试过程中发生异常: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
