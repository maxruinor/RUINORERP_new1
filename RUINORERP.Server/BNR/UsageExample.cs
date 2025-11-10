using System;
using CacheManager.Core;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Server.BNR;

namespace RUINORERP.Server.BNR.Examples
{
    /// <summary>
    /// 编号生成器使用示例和测试工具
    /// 提供各种参数处理器的使用方法、组合规则示例和错误处理测试
    /// </summary>
    public class UsageExample
    {
        /// <summary>
        /// 主程序入口，用于演示和测试
        /// </summary>
        public static void Main(string[] args)
        {
            // 运行基本示例
            RunBasicExamples();
            
            // 运行进阶示例
            RunAdvancedExamples();
            
            // 运行进阶示例
            RunAdvancedExamples();
            
            // 运行重置序号示例
            RunResetSequenceExamples();
            
            // 运行业务场景重置序号示例
            RunBusinessResetSequenceExamples();
            
            // 运行序号信息管理示例
            RunSequenceInfoManagementExamples();
            
            // 运行错误处理测试
            RunErrorHandlingTests();
            
            Console.WriteLine("\n所有测试完成，按任意键退出...");
            Console.ReadKey();
        }

        /// <summary>
        /// 运行基本示例
        /// </summary>
        private static void RunBasicExamples()
        {
            Console.WriteLine("\n========= 基本示例 =========");
            
            // 创建BNR工厂实例
            var factory = Startup.GetFromFac<BNRFactory>();
            
            // 注册所有默认的参数处理器
            factory.Initialize();
            
            // 示例1: 基本序号生成
            Console.WriteLine("\n示例1: 基本常量字符串");
            string rule1 = "{S:ORDER}";
            string result1 = factory.Create(rule1);
            Console.WriteLine($"规则: {rule1} => 结果: {result1}");
            
            // 示例2: 日期参数
            Console.WriteLine("\n示例2: 日期参数");
            string rule2 = "{D:yyyyMMdd}";
            string result2 = factory.Create(rule2);
            Console.WriteLine($"规则: {rule2} => 结果: {result2}");
            
            // 示例3: 中文首字母
            Console.WriteLine("\n示例3: 中文首字母");
            string rule3 = "{CN:销售订单}";
            string result3 = factory.Create(rule3);
            Console.WriteLine($"规则: {rule3} => 结果: {result3}");
            
            // 示例4: 十六进制日期
            Console.WriteLine("\n示例4: 十六进制日期");
            string rule4 = "{Hex:yyMMdd}";
            string result4 = factory.Create(rule4);
            Console.WriteLine($"规则: {rule4} => 结果: {result4}");
            
            // 示例5: 基于数据库的序号
            Console.WriteLine("\n示例5: 基于数据库的序号");
            string rule5 = "{DB:TEST_ORDER/0000}";
            string result5 = factory.Create(rule5);
            Console.WriteLine($"规则: {rule5} => 结果: {result5}");
        }

        /// <summary>
        /// 运行进阶示例
        /// </summary>
        private static void RunAdvancedExamples()
        {
            Console.WriteLine("\n========= 进阶示例 =========");

            // 创建BNR工厂实例
            var factory = Startup.GetFromFac<BNRFactory>();
            factory.Initialize();
            
            // 示例5: 组合规则 - 常量+日期
            Console.WriteLine("\n示例5: 组合规则 - 常量+日期");
            string rule5 = "{S:ORD-}{D:yyyyMMdd}";
            string result5 = factory.Create(rule5);
            Console.WriteLine($"规则: {rule5} => 结果: {result5}");
            
            // 示例6: 组合规则 - 中文+日期+固定值
            Console.WriteLine("\n示例6: 组合规则 - 中文+日期+固定值");
            string rule6 = "{CN:销售}{D:yyMMdd}-{S:001}";
            string result6 = factory.Create(rule6);
            Console.WriteLine($"规则: {rule6} => 结果: {result6}");
            
            // 示例7: 复杂组合规则
            Console.WriteLine("\n示例7: 复杂组合规则");
            string rule7 = "{CN:采购订单}-{D:yyyyMMdd}-{S:0001}";
            string result7 = factory.Create(rule7);
            Console.WriteLine($"规则: {rule7} => 结果: {result7}");
            
            // 示例8: 多层嵌套参数
            Console.WriteLine("\n示例8: 多层嵌套参数");
            string rule8 = "{S:ORD-{D:yyyyMM}}";
            string result8 = factory.Create(rule8);
            Console.WriteLine($"规则: {rule8} => 结果: {result8}");
            
            // 示例9: 按天重置的数据库序号
            Console.WriteLine("\n示例9: 按天重置的数据库序号");
            string rule9 = "{D:yyyyMMdd}-{DB:DAILY_ORDER/0000/daily}";
            string result9 = factory.Create(rule9);
            Console.WriteLine($"规则: {rule9} => 结果: {result9}");
            
            // 示例10: 按月重置的数据库序号
            Console.WriteLine("\n示例10: 按月重置的数据库序号");
            string rule10 = "{D:yyyyMM}-{DB:MONTHLY_REPORT/000/monthly}";
            string result10 = factory.Create(rule10);
            Console.WriteLine($"规则: {rule10} => 结果: {result10}");
        }

        /// <summary>
        /// 运行错误处理测试
        /// </summary>
        private static void RunErrorHandlingTests()
        {
            Console.WriteLine("\n========= 错误处理测试 =========");

            // 创建BNR工厂实例
            var factory = Startup.GetFromFac<BNRFactory>();
            factory.Initialize();
            
            // 测试9: 未知参数处理器
            Console.WriteLine("\n测试9: 未知参数处理器");
            string rule9 = "{Unknown:Value}";
            try
            {
                string result9 = factory.Create(rule9);
                Console.WriteLine($"规则: {rule9} => 结果: {result9}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"捕获到异常: {ex.Message}");
            }
            
            // 测试10: 格式错误的规则
            Console.WriteLine("\n测试10: 格式错误的规则");
            string rule10 = "{S:Value{MissingCloseBracket}";
            try
            {
                string result10 = factory.Create(rule10);
                Console.WriteLine($"规则: {rule10} => 结果: {result10}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"捕获到异常: {ex.Message}");
            }
            
            // 测试11: 空规则
            Console.WriteLine("\n测试11: 空规则");
            string rule11 = "";
            try
            {
                string result11 = factory.Create(rule11);
                Console.WriteLine($"规则: 空字符串 => 结果: {result11}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"捕获到异常: {ex.Message}");
            }
            
            // 测试12: 无占位符规则
            Console.WriteLine("\n测试12: 无占位符规则");
            string rule12 = "SIMPLE_ORDER_001";
            try
            {
                string result12 = factory.Create(rule12);
                Console.WriteLine($"规则: {rule12} => 结果: {result12}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"捕获到异常: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 运行重置序号示例
        /// </summary>
        private static void RunResetSequenceExamples()
        {
            Console.WriteLine("\n========= 重置序号示例 =========");

            // 获取DatabaseSequenceService实例（通过依赖注入获取）
            var sequenceService = Startup.GetFromFac<DatabaseSequenceService>();
            
            // 获取BNRFactory实例（通过依赖注入获取）
            var factory = Startup.GetFromFac<BNRFactory>();
            
            // 示例1: 创建一个测试序号并重置它
            Console.WriteLine("\n示例1: 创建测试序号并重置");
            
            string testRule = "{DB:TEST_SEQUENCE/0000}";
            Console.WriteLine("生成3个测试序号:");
            for (int i = 0; i < 3; i++)
            {
                string result = factory.Create(testRule);
                Console.WriteLine($"  第{i + 1}个: {result}");
            }
            
            // 重置序号
            Console.WriteLine("\n重置序号 'TEST_SEQUENCE'...");
            try
            {
                sequenceService.ResetSequence("TEST_SEQUENCE");
                Console.WriteLine("序号重置成功");
                
                // 验证重置结果
                string resultAfterReset = factory.Create(testRule);
                Console.WriteLine($"重置后生成的第一个序号: {resultAfterReset} (应为 0001)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重置序号失败: {ex.Message}");
            }
            
            // 示例2: 按天重置的序号
            Console.WriteLine("\n示例2: 按天重置的序号");
            string dailyRule = "{DB:DAILY_TEST/000/daily}";
            Console.WriteLine("生成按天重置的序号:");
            string dailyResult1 = factory.Create(dailyRule);
            Console.WriteLine($"  今日第一个序号: {dailyResult1}");
            
            // 演示如何手动重置按天重置的序号
            Console.WriteLine("\n手动重置按天重置的序号:");
            try
            {
                sequenceService.ResetSequence("DAILY_TEST");
                string dailyResult2 = factory.Create(dailyRule);
                Console.WriteLine($"  重置后第一个序号: {dailyResult2} (应为 001)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重置按天重置序号失败: {ex.Message}");
            }
            
            // 示例3: 重置序号到指定值
            Console.WriteLine("\n示例3: 重置序号到指定值");
            try
            {
                sequenceService.ResetSequenceValue("TEST_SEQUENCE", 100);
                Console.WriteLine("序号 'TEST_SEQUENCE' 已重置为 100");
                
                // 验证重置结果
                string resultAfterValueReset = factory.Create(testRule);
                Console.WriteLine($"重置后生成的序号: {resultAfterValueReset} (应为 0101)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重置序号到指定值失败: {ex.Message}");
            }
            
            // 示例4: 获取当前序号值
            Console.WriteLine("\n示例4: 获取当前序号值");
            try
            {
                long currentValue = sequenceService.GetCurrentSequenceValue("TEST_SEQUENCE");
                Console.WriteLine($"序号 'TEST_SEQUENCE' 的当前值: {currentValue}");
                
                long currentValueWithReset = sequenceService.GetCurrentSequenceValue("DAILY_TEST", "daily");
                Console.WriteLine($"序号 'DAILY_TEST' 的当前值: {currentValueWithReset}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取序号值失败: {ex.Message}");
            }
            
            // 示例5: 查询所有序号
            Console.WriteLine("\n示例5: 查询所有序号");
            try
            {
                var allSequences = sequenceService.GetAllSequences();
                Console.WriteLine($"数据库中共有 {allSequences.Count} 个序号记录");
                
                // 显示前几个序号记录
                int displayCount = Math.Min(5, allSequences.Count);
                for (int i = 0; i < displayCount; i++)
                {
                    var seq = allSequences[i];
                    Console.WriteLine($"  {seq.SequenceKey}: {seq.CurrentValue} (重置类型: {seq.ResetType ?? "None"})");
                }
                
                if (allSequences.Count > 5)
                {
                    Console.WriteLine($"  ... 还有 {allSequences.Count - 5} 个序号记录");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"查询序号记录失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 运行业务场景中的重置序号示例
        /// 演示在实际业务中如何使用重置序号功能
        /// </summary>
        private static void RunBusinessResetSequenceExamples()
        {
            Console.WriteLine("\n========= 业务场景重置序号示例 =========");

            // 获取DatabaseSequenceService实例（通过依赖注入获取）
            var sequenceService = Startup.GetFromFac<DatabaseSequenceService>();
            
            // 获取BNRFactory实例（通过依赖注入获取）
            var factory = Startup.GetFromFac<BNRFactory>();
            
            // 业务场景1: 月末重置月报序号
            Console.WriteLine("\n业务场景1: 月末重置月报序号");
            string monthlyReportRule = "{D:yyyyMM}-{DB:MONTHLY_REPORT/000/monthly}";
            Console.WriteLine("生成本月月报序号:");
            string monthlyReport1 = factory.Create(monthlyReportRule);
            Console.WriteLine($"  本月第一个月报序号: {monthlyReport1}");
            
            // 模拟月末操作 - 重置月报序号
            Console.WriteLine("\n模拟月末操作 - 重置月报序号:");
            try
            {
                sequenceService.ResetSequence("MONTHLY_REPORT");
                Console.WriteLine("月报序号已重置，为下个月做准备");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重置月报序号失败: {ex.Message}");
            }
            
            // 业务场景2: 年初重置年度序号
            Console.WriteLine("\n业务场景2: 年初重置年度序号");
            string yearlyRule = "{D:yyyy}-{DB:YEARLY_CONTRACT/0000/yearly}";
            Console.WriteLine("生成本年度合同序号:");
            string yearlyContract1 = factory.Create(yearlyRule);
            Console.WriteLine($"  本年度第一个合同序号: {yearlyContract1}");
            
            // 模拟年初操作 - 重置年度序号
            Console.WriteLine("\n模拟年初操作 - 重置年度序号:");
            try
            {
                sequenceService.ResetSequence("YEARLY_CONTRACT");
                Console.WriteLine("年度合同序号已重置，为下一年度做准备");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重置年度序号失败: {ex.Message}");
            }
            
            // 业务场景3: 错误修正 - 重置序号到指定值
            Console.WriteLine("\n业务场景3: 错误修正 - 重置序号到指定值");
            string orderRule = "{DB:SALES_ORDER/00000}";
            Console.WriteLine("生成销售订单序号:");
            for (int i = 0; i < 3; i++)
            {
                string orderNo = factory.Create(orderRule);
                Console.WriteLine($"  销售订单{i + 1}: {orderNo}");
            }
            
            // 模拟错误修正场景 - 将序号重置到正确的值
            Console.WriteLine("\n模拟错误修正 - 将序号重置到正确的值:");
            try
            {
                sequenceService.ResetSequenceValue("SALES_ORDER", 500);
                Console.WriteLine("销售订单序号已重置为500，用于修正数据错误");
                
                string correctedOrderNo = factory.Create(orderRule);
                Console.WriteLine($"  修正后的第一个订单号: {correctedOrderNo} (应为 00501)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重置销售订单序号失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 运行序号信息管理示例
        /// 演示如何更新和管理序号信息
        /// </summary>
        private static void RunSequenceInfoManagementExamples()
        {
            Console.WriteLine("\n========= 序号信息管理示例 =========");

            // 获取DatabaseSequenceService实例（通过依赖注入获取）
            var sequenceService = Startup.GetFromFac<DatabaseSequenceService>();
            
            // 获取BNRFactory实例（通过依赖注入获取）
            var factory = Startup.GetFromFac<BNRFactory>();
            
            // 示例1: 更新序号信息
            Console.WriteLine("\n示例1: 更新序号信息");
            string testRule = "{DB:INFO_TEST/000}";
            Console.WriteLine("生成测试序号:");
            string testNo1 = factory.Create(testRule);
            Console.WriteLine($"  测试序号: {testNo1}");
            
            // 更新序号信息
            Console.WriteLine("\n更新序号信息:");
            try
            {
                sequenceService.UpdateSequenceInfo(
                    "INFO_TEST",
                    resetType: "daily",
                    formatMask: "0000",
                    description: "信息测试序号",
                    businessType: "TEST"
                );
                Console.WriteLine("序号信息更新成功");
                
                // 验证更新结果
                string testNo2 = factory.Create("{DB:INFO_TEST/0000/daily}");
                Console.WriteLine($"  更新后的序号: {testNo2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新序号信息失败: {ex.Message}");
            }
            
            // 示例2: 按业务类型查询序号
            Console.WriteLine("\n示例2: 按业务类型查询序号");
            try
            {
                var testSequences = sequenceService.GetSequencesByBusinessType("TEST");
                Console.WriteLine($"业务类型 'TEST' 的序号记录数: {testSequences.Count}");
                
                if (testSequences.Count > 0)
                {
                    var seq = testSequences[0];
                    Console.WriteLine($"  序号键: {seq.SequenceKey}");
                    Console.WriteLine($"  当前值: {seq.CurrentValue}");
                    Console.WriteLine($"  重置类型: {seq.ResetType ?? "None"}");
                    Console.WriteLine($"  格式掩码: {seq.FormatMask ?? ""}");
                    Console.WriteLine($"  描述: {seq.Description ?? ""}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"按业务类型查询序号失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 使用依赖注入的示例方法
        /// 在实际应用中，您应该通过依赖注入容器获取BNRFactory实例
        /// </summary>
        /// <param name="factory">通过依赖注入获取的BNRFactory实例</param>
        public static void RunWithDI(BNRFactory factory)
        {
            Console.WriteLine("\n========= 依赖注入示例 =========");
            
            // 使用工厂生成订单编号
            string orderNumber = factory.Create("{S:ORD-{D:yyyyMMdd}-{S:001}}");
            Console.WriteLine($"生成的订单编号: {orderNumber}");
            
            // 使用工厂生成产品编号
            string productNumber = factory.Create("{S:PRD-{CN:电子}{D:yyMM}}");
            Console.WriteLine($"生成的产品编号: {productNumber}");
            
            // 使用工厂生成基于数据库的序号
            string dbOrderNumber = factory.Create("{CN:销售订单}-{D:yyyyMMdd}-{DB:SALES_ORDER/0000}");
            Console.WriteLine($"生成的销售订单号: {dbOrderNumber}");
            
            // 使用工厂生成按天重置的序号
            string dailyOrderNumber = factory.Create("{D:yyyyMMdd}-{DB:DAILY_ORDER/0000/daily}");
            Console.WriteLine($"生成的按天重置订单号: {dailyOrderNumber}");
        }
    }
}