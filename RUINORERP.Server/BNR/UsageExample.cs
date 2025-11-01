using System;
using CacheManager.Core;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}