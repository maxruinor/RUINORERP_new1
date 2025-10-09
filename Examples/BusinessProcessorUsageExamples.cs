using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Commands;

namespace RUINORERP.Examples
{
    /// <summary>
    /// 业务处理器使用示例 - 展示如何在不同场景下使用各种处理器
    /// </summary>
    public class BusinessProcessorUsageExamples
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BusinessProcessorFactory _processorFactory;
        private readonly ILogger<BusinessProcessorUsageExamples> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessProcessorUsageExamples(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _processorFactory = serviceProvider.GetService<BusinessProcessorFactory>();
            _logger = serviceProvider.GetService<ILogger<BusinessProcessorUsageExamples>>();
        }

        /// <summary>
        /// 示例1：简单业务场景 - 字符串转换
        /// </summary>
        public async Task Example1_SimpleBusinessScenario()
        {
            _logger.LogInformation("=== 示例1：简单业务场景 ===");

            // 创建简单请求
            var simpleRequest = SimpleRequest.Create("hello world");

            // 获取简单业务处理器
            var simpleHandler = _processorFactory.CreateProcessor("SimpleRequest") as SimpleBusinessHandler;

            // 执行字符串转换操作
            var result = await simpleHandler.HandleSimpleOperation(simpleRequest);

            _logger.LogInformation($"简单业务结果：{result.IsSuccess}, 数据：{result.GetStringValue()}");
        }

        /// <summary>
        /// 示例2：中等复杂度业务 - 用户CRUD操作
        /// </summary>
        public async Task Example2_MediumComplexityBusiness()
        {
            _logger.LogInformation("=== 示例2：中等复杂度业务 ===");

            // 创建用户CRUD处理器
            var userHandler = _processorFactory.CreateProcessor("UserRequest") as UserCrudHandler;

            // 创建用户
            var newUser = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                FullName = "测试用户",
                Phone = "13800138000"
            };

            var createRequest = CrudRequest<User>.Create(OperationType.Create, newUser);
            var createResult = await userHandler.HandleAsync(createRequest);

            if (createResult.IsSuccess)
            {
                _logger.LogInformation($"用户创建成功：{createResult.Data.Username}");

                // 更新用户
                createResult.Data.FullName = "更新后的用户名";
                var updateRequest = CrudRequest<User>.Create(OperationType.Update, createResult.Data);
                var updateResult = await userHandler.HandleAsync(updateRequest);

                _logger.LogInformation($"用户更新结果：{updateResult.IsSuccess}");

                // 查询用户
                var getRequest = CrudRequest<User>.Create(OperationType.Get, id: createResult.Data.Id);
                var getResult = await userHandler.HandleAsync(getRequest);

                _logger.LogInformation($"用户查询结果：{getResult.IsSuccess}, 用户名：{getResult.Data?.Username}");
            }
        }

        /// <summary>
        /// 示例3：复杂业务 - 登录处理
        /// </summary>
        public async Task Example3_ComplexBusiness()
        {
            _logger.LogInformation("=== 示例3：复杂业务 ===");

            // 创建登录请求
            var loginRequest = LoginRequest.Create("admin", "password123", "1.0.0");

            // 获取登录处理器（复杂业务）
            var loginHandler = _processorFactory.CreateProcessor("LoginRequest") as LoginCommandHandler;

            // 执行登录处理
            var result = await loginHandler.HandleAsync(loginRequest);

            _logger.LogInformation($"登录处理结果：{result.IsSuccess}");
        }

        /// <summary>
        /// 示例4：动态路由配置
        /// </summary>
        public async Task Example4_DynamicRouting()
        {
            _logger.LogInformation("=== 示例4：动态路由配置 ===");

            // 配置动态路由
            var dynamicConfig = new DynamicRouterConfig();
            dynamicConfig.Routes.Add(new DynamicRouterConfig.RouteConfig
            {
                CommandType = "CustomRequest",
                HandlerTypeName = typeof(SimpleBusinessHandler).FullName,
                HandlerType = typeof(SimpleBusinessHandler),
                Priority = 1,
                Description = "自定义请求路由"
            });

            // 创建动态路由处理器
            var dynamicRouter = new DynamicCommandRouter(_serviceProvider, dynamicConfig, 
                _serviceProvider.GetService<ILogger<DynamicCommandRouter>>());

            // 创建自定义请求
            var customRequest = SimpleRequest.Create("dynamic test");

            // 通过动态路由处理
            var result = await dynamicRouter.HandleAsync(customRequest);

            _logger.LogInformation($"动态路由处理结果：{result.IsSuccess}");
        }

        /// <summary>
        /// 示例5：配置式业务处理
        /// </summary>
        public async Task Example5_ConfigurableBusiness()
        {
            _logger.LogInformation("=== 示例5：配置式业务处理 ===");

            // 创建业务配置
            var businessConfig = new BusinessConfig();
            var configInfo = new BusinessConfigInfo
            {
                BusinessType = BusinessType.Create,
                EntityType = typeof(User),
                ValidationRules = new List<ValidationRule>
                {
                    new ValidationRule
                    {
                        RuleName = "RequiredValidation",
                        Expression = "Username != null && Email != null",
                        ErrorMessage = "用户名和邮箱不能为空"
                    }
                },
                DataMapping = new Dictionary<string, string>
                {
                    { "Name", "Username" },
                    { "EmailAddress", "Email" }
                }
            };

            businessConfig.AddBusinessConfig("CreateUserRequest", configInfo);

            // 创建配置式处理器
            var configurableProcessor = new ConfigurableBusinessProcessor(
                businessConfig,
                _serviceProvider.GetService<ILogger<ConfigurableBusinessProcessor>>()
            );

            // 创建请求数据
            var requestData = new { Name = "configuser", EmailAddress = "config@example.com" };

            // 执行配置式处理
            var result = await configurableProcessor.HandleAsync(requestData);

            _logger.LogInformation($"配置式处理结果：{result.IsSuccess}");
        }

        /// <summary>
        /// 示例6：业务复杂度配置
        /// </summary>
        public async Task Example6_BusinessComplexityConfiguration()
        {
            _logger.LogInformation("=== 示例6：业务复杂度配置 ===");

            // 配置新的业务复杂度
            var newConfigurations = new Dictionary<string, BusinessComplexity>
            {
                { "NewSimpleRequest", BusinessComplexity.Simple },
                { "NewMediumRequest", BusinessComplexity.Medium },
                { "NewComplexRequest", BusinessComplexity.Complex }
            };

            _processorFactory.ConfigureComplexities(newConfigurations);

            // 创建对应的处理器
            var simpleHandler = _processorFactory.CreateProcessor("NewSimpleRequest");
            var mediumHandler = _processorFactory.CreateProcessor("NewMediumRequest");
            var complexHandler = _processorFactory.CreateProcessor("NewComplexRequest");

            _logger.LogInformation($"处理器创建完成：简单={simpleHandler?.GetType().Name}, 中等={mediumHandler?.GetType().Name}, 复杂={complexHandler?.GetType().Name}");

            // 获取统计信息
            var statistics = _processorFactory.GetStatistics();
            _logger.LogInformation($"处理器统计：缓存={statistics.CachedHandlersCount}, 配置={statistics.ComplexityConfigurationsCount}");
        }

        /// <summary>
        /// 示例7：混合使用不同处理器
        /// </summary>
        public async Task Example7_MixedProcessorUsage()
        {
            _logger.LogInformation("=== 示例7：混合使用不同处理器 ===");

            // 简单业务：字符串验证
            var simpleHandler = _processorFactory.CreateProcessor("SimpleRequest") as SimpleBusinessHandler;
            var validationRequest = SimpleRequest.Create("test123");
            var validationResult = await simpleHandler.HandleValidationOperation(validationRequest);
            _logger.LogInformation($"字符串验证结果：{validationResult.IsSuccess}");

            // 中等业务：用户查询
            var userHandler = _processorFactory.CreateProcessor("UserRequest") as UserCrudHandler;
            var queryParameters = new Dictionary<string, object> { { "search", "test" } };
            var queryRequest = CrudRequest<User>.Create(OperationType.GetList, queryParameters: queryParameters);
            var queryResult = await userHandler.HandleAsync(queryRequest);
            _logger.LogInformation($"用户查询结果：{queryResult.IsSuccess}");

            // 复杂业务：状态检查
            var statusResult = await simpleHandler.HandleStatusCheck(SimpleRequest.Create(""));
            _logger.LogInformation($"系统状态检查结果：{statusResult.IsSuccess}");
        }

        /// <summary>
        /// 运行所有示例
        /// </summary>
        public async Task RunAllExamples()
        {
            _logger.LogInformation("开始运行所有业务处理器示例...");

            try
            {
                await Example1_SimpleBusinessScenario();
                await Example2_MediumComplexityBusiness();
                await Example3_ComplexBusiness();
                await Example4_DynamicRouting();
                await Example5_ConfigurableBusiness();
                await Example6_BusinessComplexityConfiguration();
                await Example7_MixedProcessorUsage();

                _logger.LogInformation("所有示例运行完成！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "运行示例时发生错误");
            }
        }
    }

    /// <summary>
    /// 程序入口示例
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 主方法
        /// </summary>
        public static async Task Main(string[] args)
        {
            // 配置依赖注入
            var services = new ServiceCollection();
            
            // 注册日志服务
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // 注册业务处理器
            services.AddTransient<SimpleBusinessHandler>();
            services.AddTransient<UserCrudHandler>();
            services.AddTransient<LoginCommandHandler>();
            services.AddTransient<DynamicCommandRouter>();
            services.AddTransient<ConfigurableBusinessProcessor>();
            services.AddSingleton<BusinessProcessorFactory>();

            // 构建服务提供器
            var serviceProvider = services.BuildServiceProvider();

            // 运行示例
            var examples = new BusinessProcessorUsageExamples(serviceProvider);
            await examples.RunAllExamples();

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}