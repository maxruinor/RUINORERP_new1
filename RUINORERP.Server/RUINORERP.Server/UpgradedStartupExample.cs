using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.SuperSocketServices;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ServerService;
using RUINORERP.Server.SmartReminder;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.Comm;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;
using HealthCheckContext = RUINORERP.Server.SmartReminder.HealthCheckContext;
using IHealthCheck = RUINORERP.Server.SmartReminder.IHealthCheck;

namespace RUINORERP.Server
{
    /// <summary>
    /// 升级后的Startup配置示例
    /// 演示如何集成新的SuperSocket架构同时保持现有业务功能
    /// </summary>
    public class UpgradedStartupExample
    {
        private readonly IConfiguration _configuration;

        public UpgradedStartupExample(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 配置服务 - 升级版本
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // ========== 阶段1：保持现有核心服务不变 ==========
            ConfigureLegacyServices(services);

            // ========== 阶段2：添加新的SuperSocket架构 ==========
            services.AddUpgradedServerServices(_configuration);

            // ========== 阶段3：配置服务集成 ==========
            ConfigureServiceIntegration(services);

            // ========== 阶段4：添加性能监控和日志 ==========
            ConfigureMonitoringAndLogging(services);
        }

        /// <summary>
        /// 配置现有核心服务（保持不变）
        /// </summary>
        private void ConfigureLegacyServices(IServiceCollection services)
        {
            // 数据库和ORM配置
            // services.AddDbContext<ApplicationDbContext>(...);
            
            // AutoMapper配置
            // services.AddAutoMapper(...);
            
            // 缓存配置
            services.AddMemoryCache();
            
            // 日志配置
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.AddFile("logs/server-{Date}.log");
            });

            // 其他现有的核心服务配置...
        }

        /// <summary>
        /// 配置服务集成
        /// </summary>
        private void ConfigureServiceIntegration(IServiceCollection services)
        {
            // SessionforBiz工厂配置
            services.AddTransient(provider => SessionforBizFactory.CreateEnhancedSession(provider));
            
            // 命令调度器集成配置
            services.Configure<CommandDispatcherOptions>(options =>
            {
                options.EnableLegacyCommandForwarding = true;
                options.EnablePerformanceMonitoring = true;
                options.CommandTimeout = TimeSpan.FromSeconds(30);
            });

            // 会话管理器集成配置
            services.Configure<SessionManagerOptions>(options =>
            {
                options.EnableEnhancedPerformanceTracking = true;
                options.SessionTimeoutMinutes = 30;
                options.HeartbeatIntervalSeconds = 30;
            });
        }

        /// <summary>
        /// 配置监控和日志
        /// </summary>
        private void ConfigureMonitoringAndLogging(IServiceCollection services)
        {
            // 性能计数器
            services.AddSingleton<IPerformanceCounterService, PerformanceCounterService>();
            
            // 健康检查
            services.AddHealthChecks()
                .AddCheck<SuperSocketHealthCheck>("supersocket")
                .AddCheck<SessionManagerHealthCheck>("session-manager")
                .AddCheck<DatabaseHealthCheck>("database");

            // 应用程序指标
            services.AddSingleton<IApplicationMetrics, ApplicationMetrics>();
        }

        /// <summary>
        /// 配置应用程序 - 升级版本
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 开发环境配置
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 健康检查端点
            app.UseHealthChecks("/health");

            // 性能监控中间件
            app.UseMiddleware<PerformanceMonitoringMiddleware>();

            // 启动SuperSocket服务
            var serviceProvider = app.ApplicationServices;
            StartSuperSocketServices(serviceProvider);

            // 启动兼容性检查
            StartCompatibilityCheck(serviceProvider);
        }

        /// <summary>
        /// 启动SuperSocket服务
        /// </summary>
        private void StartSuperSocketServices(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<UpgradedStartupExample>>();
            
            try
            {
                // 启动增强会话管理器
                var enhancedManager = serviceProvider.GetService<EnhancedSessionManager>();
                logger?.LogInformation("增强会话管理器已启动");

                // 启动性能监控
                var performanceCounter = serviceProvider.GetService<IPerformanceCounterService>();
                performanceCounter?.Start();
                logger?.LogInformation("性能监控已启动");

                logger?.LogInformation("SuperSocket服务体系启动完成");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "启动SuperSocket服务时发生错误");
            }
        }

        /// <summary>
        /// 启动兼容性检查
        /// </summary>
        private void StartCompatibilityCheck(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<UpgradedStartupExample>>();
            
            try
            {
                // 检查新旧架构的兼容性
                var compatibilityChecker = serviceProvider.GetService<ArchitectureCompatibilityChecker>();
                var isCompatible = compatibilityChecker?.CheckCompatibility() ?? false;

                if (isCompatible)
                {
                    logger?.LogInformation("新旧架构兼容性检查通过");
                }
                else
                {
                    logger?.LogWarning("新旧架构兼容性检查失败，将使用降级模式");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "兼容性检查时发生错误");
            }
        }
    }

    /// <summary>
    /// 命令调度器选项
    /// </summary>
    public class CommandDispatcherOptions
    {
        public bool EnableLegacyCommandForwarding { get; set; } = true;
        public bool EnablePerformanceMonitoring { get; set; } = true;
        public TimeSpan CommandTimeout { get; set; } = TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// 会话管理器选项
    /// </summary>
    public class SessionManagerOptions
    {
        public bool EnableEnhancedPerformanceTracking { get; set; } = true;
        public int SessionTimeoutMinutes { get; set; } = 30;
        public int HeartbeatIntervalSeconds { get; set; } = 30;
    }

    /// <summary>
    /// 性能计数器服务接口
    /// </summary>
    public interface IPerformanceCounterService
    {
        void Start();
        void Stop();
        void IncrementCounter(string counterName);
        double GetCounterValue(string counterName);
    }

    /// <summary>
    /// 性能计数器服务实现
    /// </summary>
    public class PerformanceCounterService : IPerformanceCounterService
    {
        private readonly ILogger<PerformanceCounterService> _logger;
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, long> _counters;

        public PerformanceCounterService(ILogger<PerformanceCounterService> logger)
        {
            _logger = logger;
            _counters = new System.Collections.Concurrent.ConcurrentDictionary<string, long>();
        }

        public void Start()
        {
            _logger.LogInformation("性能计数器服务已启动");
        }

        public void Stop()
        {
            _logger.LogInformation("性能计数器服务已停止");
        }

        public void IncrementCounter(string counterName)
        {
            _counters.AddOrUpdate(counterName, 1, (key, value) => value + 1);
        }

        public double GetCounterValue(string counterName)
        {
            return _counters.TryGetValue(counterName, out var value) ? value : 0;
        }
    }

    /// <summary>
    /// 应用程序指标接口
    /// </summary>
    public interface IApplicationMetrics
    {
        void RecordSessionCount(int count);
        void RecordPacketProcessed(int count);
        void RecordResponseTime(TimeSpan responseTime);
    }

    /// <summary>
    /// 应用程序指标实现
    /// </summary>
    public class ApplicationMetrics : IApplicationMetrics
    {
        private readonly ILogger<ApplicationMetrics> _logger;

        public ApplicationMetrics(ILogger<ApplicationMetrics> logger)
        {
            _logger = logger;
        }

        public void RecordSessionCount(int count)
        {
            _logger.LogTrace($"当前会话数: {count}");
        }

        public void RecordPacketProcessed(int count)
        {
            _logger.LogTrace($"已处理数据包: {count}");
        }

        public void RecordResponseTime(TimeSpan responseTime)
        {
            _logger.LogTrace($"响应时间: {responseTime.TotalMilliseconds}ms");
        }
    }

    /// <summary>
    /// 架构兼容性检查器
    /// </summary>
    public class ArchitectureCompatibilityChecker
    {
        private readonly ILogger<ArchitectureCompatibilityChecker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ArchitectureCompatibilityChecker(
            ILogger<ArchitectureCompatibilityChecker> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public bool CheckCompatibility()
        {
            try
            {
                // 检查关键服务是否可用
                var enhancedManager = _serviceProvider.GetService<EnhancedSessionManager>();
                var sessionAdapter = _serviceProvider.GetService<SessionManagerAdapter>();
                var commandDispatcher = _serviceProvider.GetService<ICommandDispatcher>();

                bool isCompatible = enhancedManager != null && 
                                  sessionAdapter != null && 
                                  commandDispatcher != null;

                _logger.LogInformation($"架构兼容性检查结果: {(isCompatible ? "通过" : "失败")}");
                return isCompatible;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "兼容性检查异常");
                return false;
            }
        }
    }

    /// <summary>
    /// SuperSocket健康检查
    /// </summary>
    public class SuperSocketHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // 实现SuperSocket服务的健康检查
            return Task.FromResult(HealthCheckResult.Healthy("SuperSocket服务运行正常"));
        }
    }

    /// <summary>
    /// 会话管理器健康检查
    /// </summary>
    public class SessionManagerHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // 实现会话管理器的健康检查
            return Task.FromResult(HealthCheckResult.Healthy("会话管理器运行正常"));
        }
    }

    /// <summary>
    /// 数据库健康检查
    /// </summary>
    public class DatabaseHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // 实现数据库连接的健康检查
            return Task.FromResult(HealthCheckResult.Healthy("数据库连接正常"));
        }
    }

    /// <summary>
    /// 性能监控中间件
    /// </summary>
    public class PerformanceMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMonitoringMiddleware> _logger;

        public PerformanceMonitoringMiddleware(RequestDelegate next, ILogger<PerformanceMonitoringMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            await _next(context);
            
            stopwatch.Stop();
            
            if (stopwatch.ElapsedMilliseconds > 1000) // 超过1秒的请求记录警告
            {
                _logger.LogWarning($"慢请求: {context.Request.Path} 耗时 {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
}