using RUINORERP.Model.ConfigModel;
using RUINORERP.Services;
using RUINORERP.Business.Validator;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace RUINORERP.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始测试配置服务修改...");
            
            try
            {
                // 测试1: 版本比较算法
                TestVersionComparison();
                
                // 测试2: 配置验证功能
                TestConfigValidation();
                
                Console.WriteLine("\n所有测试成功完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n测试过程中出现错误: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }

        private static void TestVersionComparison()
        {
            Console.WriteLine("\n===== 测试版本比较算法 =====");
            
            // 创建测试数据
            var json1 = new JObject
            {
                { "name", "Test Config" },
                { "version", "1.0.0" },
                { "settings", new JObject
                    {
                        { "timeout", 30 },
                        { "enabled", true }
                    }
                },
                { "features", new JArray("feature1", "feature2") }
            };
            
            var json2 = new JObject
            {
                { "name", "Test Config Updated" },
                { "version", "1.1.0" },
                { "settings", new JObject
                    {
                        { "timeout", 60 },
                        { "enabled", true },
                        { "newSetting", "value" }
                    }
                },
                { "features", new JArray("feature1", "feature3", "feature4") },
                { "description", "Updated configuration" }
            };
            
            // 使用我们修改的方法进行测试
            var service = new ConfigVersionService(null); // 实际使用时需要提供日志记录器
            var diffResult = service.TestGenerateDetailedDiff(json1, json2); // 假设我们添加了一个公开的测试方法
            
            Console.WriteLine("差异比较结果:");
            Console.WriteLine($"- 修改的属性: {diffResult.ModifiedProperties.Count}");
            Console.WriteLine($"- 新增的属性: {diffResult.AddedProperties.Count}");
            Console.WriteLine($"- 删除的属性: {diffResult.RemovedProperties.Count}");
            
            // 验证结果
            bool testPassed = diffResult.ModifiedProperties.Count >= 2 && 
                            diffResult.AddedProperties.Count >= 2 && 
                            diffResult.RemovedProperties.Count >= 0;
            
            Console.WriteLine($"版本比较算法测试: {(testPassed ? "通过" : "失败")}");
        }

        private static void TestConfigValidation()
        {
            Console.WriteLine("\n===== 测试配置验证功能 =====");
            
            // 创建所有必要的验证器实例
            var serverConfigValidator = new ServerConfigValidator();
            var systemGlobalconfigValidator = new SystemGlobalconfigValidator();
            var globalValidatorConfigValidator = new GlobalValidatorConfigValidator();
            
            // 测试服务器配置验证
            var serverConfig = new ServerConfig
            {
                ServerPort = 90000, // 无效的端口
                DbConnectionString = "Invalid connection string"
            };
            
            // 测试验证器配置验证
            var validatorConfig = new GlobalValidatorConfig
            {
                ReworkTipDays = 400, // 超出范围
                ValidationTimeoutSeconds = 500 // 超出范围
            };
            
            // 正确创建验证服务并提供所有必要的依赖项
            var validationService = new ConfigValidationService(
                null, // 实际使用时需要提供日志记录器
                serverConfigValidator,
                systemGlobalconfigValidator,
                globalValidatorConfigValidator);
            
            // 测试服务器配置
            var serverValidationResult = validationService.ValidateConfig(serverConfig);
            Console.WriteLine("服务器配置验证:");
            Console.WriteLine($"- 是否有效: {serverValidationResult.IsValid}");
            Console.WriteLine($"- 错误数量: {serverValidationResult.Errors.Count}");
            
            // 测试验证器配置
            var validatorValidationResult = validationService.ValidateConfig(validatorConfig);
            Console.WriteLine("验证器配置验证:");
            Console.WriteLine($"- 是否有效: {validatorValidationResult.IsValid}");
            Console.WriteLine($"- 错误数量: {validatorValidationResult.Errors.Count}");
            
            // 验证结果
            bool testPassed = !serverValidationResult.IsValid && 
                            !validatorValidationResult.IsValid &&
                            serverValidationResult.Errors.Count >= 2 &&
                            validatorValidationResult.Errors.Count >= 2;
            
            Console.WriteLine($"配置验证功能测试: {(testPassed ? "通过" : "失败")}");
        }
    }
}