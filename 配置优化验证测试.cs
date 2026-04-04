using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RUINORERP.Model.ConfigModel;

namespace ConfigOptimizationTest
{
    /// <summary>
    /// 配置优化验证测试
    /// 用于验证 IOptionsMonitor + reloadOnChange 机制是否正常工作
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 配置管理优化验证测试 ===\n");

            // 1. 创建测试配置文件
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            string configPath = Path.Combine(configDirectory, "SystemGlobalConfig.json");
            
            // 创建初始配置
            var initialConfig = new SystemGlobalConfig
            {
                SomeSetting = "初始值",
                DirectPrinting = false
            };
            
            var configWrapper = new { SystemGlobalConfig = initialConfig };
            File.WriteAllText(configPath, Newtonsoft.Json.JsonConvert.SerializeObject(configWrapper, Newtonsoft.Json.Formatting.Indented));
            Console.WriteLine($"✓ 创建测试配置文件: {configPath}");

            // 2. 配置 IConfiguration (启用 reloadOnChange)
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(configDirectory)
                .AddJsonFile("SystemGlobalConfig.json", optional: false, reloadOnChange: true);
            
            var configuration = configurationBuilder.Build();
            Console.WriteLine("✓ IConfiguration 已配置 (reloadOnChange: true)");

            // 3. 注册 Options 服务
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            services.AddOptions();
            services.Configure<SystemGlobalConfig>(configuration.GetSection("SystemGlobalConfig"));
            
            var serviceProvider = services.BuildServiceProvider();
            Console.WriteLine("✓ Options 服务已注册");

            // 4. 获取 IOptionsMonitor
            var monitor = serviceProvider.GetRequiredService<IOptionsMonitor<SystemGlobalConfig>>();
            var currentConfig = monitor.CurrentValue;
            Console.WriteLine($"✓ 初始配置加载成功: SomeSetting = '{currentConfig.SomeSetting}'");

            // 5. 订阅变更事件
            bool changeDetected = false;
            monitor.OnChange(newConfig =>
            {
                Console.WriteLine($"\n⚡ 检测到配置变更!");
                Console.WriteLine($"   新值: SomeSetting = '{newConfig.SomeSetting}'");
                Console.WriteLine($"   新值: DirectPrinting = {newConfig.DirectPrinting}");
                changeDetected = true;
            });
            Console.WriteLine("✓ 已订阅 OnChange 事件\n");

            // 6. 模拟配置更新(写入文件)
            Console.WriteLine("--- 模拟配置更新 ---");
            var updatedConfig = new SystemGlobalConfig
            {
                SomeSetting = "更新后的值",
                DirectPrinting = true
            };
            
            var updatedWrapper = new { SystemGlobalConfig = updatedConfig };
            File.WriteAllText(configPath, Newtonsoft.Json.JsonConvert.SerializeObject(updatedWrapper, Newtonsoft.Json.Formatting.Indented));
            Console.WriteLine("✓ 配置文件已更新");

            // 7. 等待文件监听器触发
            Console.WriteLine("\n等待文件监听器触发...");
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                if (changeDetected)
                {
                    break;
                }
                Console.Write(".");
            }
            Console.WriteLine();

            // 8. 验证结果
            Console.WriteLine("\n--- 验证结果 ---");
            if (changeDetected)
            {
                Console.WriteLine("✅ 测试通过: IOptionsMonitor 成功检测到配置变更");
                
                // 验证最新值
                var latestConfig = monitor.CurrentValue;
                if (latestConfig.SomeSetting == "更新后的值" && latestConfig.DirectPrinting == true)
                {
                    Console.WriteLine("✅ 配置值已正确更新");
                }
                else
                {
                    Console.WriteLine("❌ 配置值未正确更新");
                    Console.WriteLine($"   期望: SomeSetting='更新后的值', DirectPrinting=true");
                    Console.WriteLine($"   实际: SomeSetting='{latestConfig.SomeSetting}', DirectPrinting={latestConfig.DirectPrinting}");
                }
            }
            else
            {
                Console.WriteLine("❌ 测试失败: 未检测到配置变更");
                Console.WriteLine("   可能原因:");
                Console.WriteLine("   1. reloadOnChange 未正确启用");
                Console.WriteLine("   2. 文件路径不匹配");
                Console.WriteLine("   3. 文件系统监听器延迟过高");
            }

            // 9. 清理测试文件
            Console.WriteLine("\n--- 清理 ---");
            if (File.Exists(configPath))
            {
                File.Delete(configPath);
                Console.WriteLine("✓ 测试文件已删除");
            }

            Console.WriteLine("\n=== 测试完成 ===");
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
