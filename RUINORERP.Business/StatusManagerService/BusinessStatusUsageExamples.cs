using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Business.StatusManagerService
{
    /// <summary>
    /// 文件: BusinessStatusUsageExamples.cs
    /// 版本: V3增强版 - 业务性状态动态管理使用示例
    /// 说明: 提供业务性状态动态管理的使用示例和最佳实践
    /// 创建日期: 2024年
    /// 作者: RUINOR ERP开发团队
    /// 
    /// 版本标识：
    /// V3增强版: 提供业务性状态动态管理的使用示例和最佳实践
    /// 功能: 演示如何使用业务性状态动态管理功能
    /// </summary>

    /// <summary>
    /// 业务性状态动态管理使用示例
    /// 提供各种业务场景下的状态管理示例和最佳实践
    /// </summary>
    public class BusinessStatusUsageExamples
    {
        #region 示例1: 基本业务状态管理

        /// <summary>
        /// 示例1: 基本业务状态管理
        /// 演示如何注册业务状态类型、转换规则和操作权限
        /// </summary>
        public static void BasicBusinessStatusManagement()
        {
            // 创建服务容器
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddLogging(builder => builder.AddConsole());
            
            // 注册业务状态管理器
            services.AddSingleton<IBusinessStatusManager, BusinessStatusManager>();
            
            // 注册统一状态管理器
            services.AddSingleton<IUnifiedStateManager, UnifiedStateManager>();
            
            var serviceProvider = services.BuildServiceProvider();
            
            // 获取业务状态管理器
            var businessStatusManager = serviceProvider.GetService<IBusinessStatusManager>();
            var logger = serviceProvider.GetService<ILogger<BusinessStatusUsageExamples>>();
            
            try
            {
                // 1. 注册付款状态类型
                RegisterPaymentStatusType(businessStatusManager, logger);
                
                // 2. 注册付款状态转换规则
                RegisterPaymentStatusTransitionRules(businessStatusManager, logger);
                
                // 3. 注册付款状态操作权限规则
                RegisterPaymentStatusActionRules(businessStatusManager, logger);
                
                // 4. 创建业务上下文
                var context = new BusinessContext
                {
                    CurrentUser = "张三",
                    UserRoles = new[] { "财务员" },
                    CurrentModule = "付款管理",
                    CurrentOperation = "提交审核",
                    BusinessType = "付款单",
                    Department = "财务部",
                    Amount = 10000m
                };
                
                // 5. 验证状态转换
                var validationResult = businessStatusManager.ValidateBusinessTransition(
                    typeof(PaymentStatus), 
                    PaymentStatus.草稿, 
                    PaymentStatus.待审核, 
                    context);
                
                logger?.LogInformation($"状态转换验证结果: {validationResult.IsAllowed}, 消息: {validationResult.Message}");
                
                if (validationResult.IsAllowed)
                {
                    // 6. 执行状态转换
                    var paymentEntity = new { Id = 1, Amount = 10000m, Status = PaymentStatus.草稿 };
                    
                    var transitionResult = businessStatusManager.ExecuteBusinessTransition(
                        typeof(PaymentStatus), 
                        paymentEntity, 
                        PaymentStatus.草稿, 
                        PaymentStatus.待审核, 
                        context);
                    
                    logger?.LogInformation($"状态转换执行结果: {transitionResult.IsSuccess}, 消息: {transitionResult.Message}");
                }
                
                // 7. 检查操作权限
                var canEdit = businessStatusManager.CheckBusinessActionPermission(
                    typeof(PaymentStatus), 
                    PaymentStatus.草稿, 
                    "编辑", 
                    context);
                
                logger?.LogInformation($"编辑权限检查结果: {canEdit}");
                
                // 8. 获取状态显示信息
                var displayInfo = businessStatusManager.GetBusinessStatusDisplayInfo(
                    typeof(PaymentStatus), 
                    PaymentStatus.草稿, 
                    context);
                
                logger?.LogInformation($"状态显示信息: {displayInfo.DisplayText}, 颜色: {displayInfo.ColorCode}");
                
                // 9. 获取可转换的状态列表
                var availableTransitions = businessStatusManager.GetAvailableBusinessTransitions(
                    typeof(PaymentStatus), 
                    PaymentStatus.草稿, 
                    context);
                
                logger?.LogInformation($"可转换的状态: {string.Join(", ", availableTransitions)}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "基本业务状态管理示例执行失败");
            }
        }

        /// <summary>
        /// 注册付款状态类型
        /// </summary>
        /// <param name="businessStatusManager">业务状态管理器</param>
        /// <param name="logger">日志记录器</param>
        private static void RegisterPaymentStatusType(IBusinessStatusManager businessStatusManager, ILogger logger)
        {
            var statusType = typeof(PaymentStatus);
            var configuration = new BusinessStatusTypeConfiguration
            {
                Name = "付款状态",
                Description = "付款单的状态管理",
                ApplicableModules = new[] { "财务", "付款管理" },
                SupportsDynamicStatus = false,
                StatusValueProvider = new EnumStatusValueProvider<PaymentStatus>()
            };

            businessStatusManager.RegisterBusinessStatusType(statusType, configuration);
            logger?.LogInformation($"已注册付款状态类型: {statusType.Name}");
        }

        /// <summary>
        /// 注册付款状态转换规则
        /// </summary>
        /// <param name="businessStatusManager">业务状态管理器</param>
        /// <param name="logger">日志记录器</param>
        private static void RegisterPaymentStatusTransitionRules(IBusinessStatusManager businessStatusManager, ILogger logger)
        {
            var statusType = typeof(PaymentStatus);
            
            // 草稿 -> 待审核
            businessStatusManager.RegisterBusinessTransitionRule(
                statusType, 
                PaymentStatus.草稿, 
                PaymentStatus.待审核, 
                new BusinessTransitionRule
                {
                    Name = "提交审核",
                    Description = "提交付款单进行审核",
                    Validator = ctx => ctx.UserRoles.Contains("财务员"),
                    RequiresApproval = true,
                    PreTransitionHandler = ctx => {
                        logger?.LogInformation($"用户 {ctx.CurrentUser} 正在提交付款单审核");
                        return new BusinessTransitionResult { IsSuccess = true };
                    },
                    PostTransitionHandler = ctx => {
                        logger?.LogInformation($"用户 {ctx.CurrentUser} 已成功提交付款单审核");
                        return new BusinessTransitionResult { IsSuccess = true };
                    }
                });
            
            // 待审核 -> 已支付
            businessStatusManager.RegisterBusinessTransitionRule(
                statusType, 
                PaymentStatus.待审核, 
                PaymentStatus.已支付, 
                new BusinessTransitionRule
                {
                    Name = "支付完成",
                    Description = "付款单支付完成",
                    Validator = ctx => ctx.UserRoles.Contains("财务主管") || ctx.UserRoles.Contains("出纳"),
                    RequiresApproval = false
                });
            
            // 待审核 -> 草稿
            businessStatusManager.RegisterBusinessTransitionRule(
                statusType, 
                PaymentStatus.待审核, 
                PaymentStatus.草稿, 
                new BusinessTransitionRule
                {
                    Name = "退回草稿",
                    Description = "审核不通过，退回草稿状态",
                    Validator = ctx => ctx.UserRoles.Contains("财务主管"),
                    RequiresApproval = false
                });
            
            logger?.LogInformation($"已注册付款状态转换规则");
        }

        /// <summary>
        /// 注册付款状态操作权限规则
        /// </summary>
        /// <param name="businessStatusManager">业务状态管理器</param>
        /// <param name="logger">日志记录器</param>
        private static void RegisterPaymentStatusActionRules(IBusinessStatusManager businessStatusManager, ILogger logger)
        {
            var statusType = typeof(PaymentStatus);
            
            // 草稿状态的操作权限
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                PaymentStatus.草稿, 
                "编辑", 
                new BusinessActionRule
                {
                    ActionName = "编辑",
                    Description = "编辑草稿状态的付款单",
                    PermissionValidator = ctx => ctx.UserRoles.Contains("财务员"),
                    ApplicableRoles = new[] { "财务员", "财务主管" }
                });
            
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                PaymentStatus.草稿, 
                "删除", 
                new BusinessActionRule
                {
                    ActionName = "删除",
                    Description = "删除草稿状态的付款单",
                    PermissionValidator = ctx => ctx.UserRoles.Contains("财务员") && ctx.Amount <= 50000,
                    ApplicableRoles = new[] { "财务员", "财务主管" }
                });
            
            // 待审核状态的操作权限
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                PaymentStatus.待审核, 
                "审核", 
                new BusinessActionRule
                {
                    ActionName = "审核",
                    Description = "审核付款单",
                    PermissionValidator = ctx => ctx.UserRoles.Contains("财务主管"),
                    ApplicableRoles = new[] { "财务主管" }
                });
            
            // 已支付状态的操作权限
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                PaymentStatus.已支付, 
                "查看", 
                new BusinessActionRule
                {
                    ActionName = "查看",
                    Description = "查看已支付的付款单",
                    PermissionValidator = ctx => true,
                    ApplicableRoles = new[] { "财务员", "财务主管", "出纳" }
                });
            
            logger?.LogInformation($"已注册付款状态操作权限规则");
        }

        #endregion

        #region 示例2: 动态业务状态管理

        /// <summary>
        /// 示例2: 动态业务状态管理
        /// 演示如何实现动态业务状态管理，根据业务上下文动态生成状态值
        /// </summary>
        public static void DynamicBusinessStatusManagement()
        {
            // 创建服务容器
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddLogging(builder => builder.AddConsole());
            
            // 注册业务状态管理器
            services.AddSingleton<IBusinessStatusManager, BusinessStatusManager>();
            
            var serviceProvider = services.BuildServiceProvider();
            
            // 获取业务状态管理器
            var businessStatusManager = serviceProvider.GetService<IBusinessStatusManager>();
            var logger = serviceProvider.GetService<ILogger<BusinessStatusUsageExamples>>();
            
            try
            {
                // 1. 注册动态业务状态类型
                RegisterDynamicBusinessStatusType(businessStatusManager, logger);
                
                // 2. 创建不同的业务上下文
                var normalContext = new BusinessContext
                {
                    CurrentUser = "张三",
                    UserRoles = new[] { "销售员" },
                    CurrentModule = "销售管理",
                    BusinessType = "普通订单",
                    Department = "销售部"
                };
                
                var urgentContext = new BusinessContext
                {
                    CurrentUser = "李四",
                    UserRoles = new[] { "销售主管" },
                    CurrentModule = "销售管理",
                    BusinessType = "紧急订单",
                    Department = "销售部",
                    IsUrgent = true
                };
                
                // 3. 获取不同业务上下文下的状态值
                var normalStatusValues = businessStatusManager.GetBusinessStatusMetadata(typeof(DynamicOrderStatus))?
                    .StatusValueProvider?.GetAllStatusValues(normalContext);
                
                var urgentStatusValues = businessStatusManager.GetBusinessStatusMetadata(typeof(DynamicOrderStatus))?
                    .StatusValueProvider?.GetAllStatusValues(urgentContext);
                
                logger?.LogInformation($"普通订单状态: {string.Join(", ", normalStatusValues ?? Enumerable.Empty<object>())}");
                logger?.LogInformation($"紧急订单状态: {string.Join(", ", urgentStatusValues ?? Enumerable.Empty<object>())}");
                
                // 4. 验证不同业务上下文下的状态转换
                var normalValidationResult = businessStatusManager.ValidateBusinessTransition(
                    typeof(DynamicOrderStatus), 
                    "待处理", 
                    "已处理", 
                    normalContext);
                
                var urgentValidationResult = businessStatusManager.ValidateBusinessTransition(
                    typeof(DynamicOrderStatus), 
                    "紧急待处理", 
                    "紧急已处理", 
                    urgentContext);
                
                logger?.LogInformation($"普通订单状态转换验证结果: {normalValidationResult.IsAllowed}");
                logger?.LogInformation($"紧急订单状态转换验证结果: {urgentValidationResult.IsAllowed}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "动态业务状态管理示例执行失败");
            }
        }

        /// <summary>
        /// 注册动态业务状态类型
        /// </summary>
        /// <param name="businessStatusManager">业务状态管理器</param>
        /// <param name="logger">日志记录器</param>
        private static void RegisterDynamicBusinessStatusType(IBusinessStatusManager businessStatusManager, ILogger logger)
        {
            var statusType = typeof(DynamicOrderStatus);
            var configuration = new BusinessStatusTypeConfiguration
            {
                Name = "动态订单状态",
                Description = "根据业务上下文动态生成的订单状态",
                ApplicableModules = new[] { "销售", "订单管理" },
                SupportsDynamicStatus = true,
                StatusValueProvider = new DynamicOrderStatusValueProvider()
            };

            businessStatusManager.RegisterBusinessStatusType(statusType, configuration);
            logger?.LogInformation($"已注册动态订单状态类型: {statusType.Name}");
        }

        #endregion

        #region 示例3: 统一状态管理集成

        /// <summary>
        /// 示例3: 统一状态管理集成
        /// 演示如何将业务性状态管理与统一状态管理器集成使用
        /// </summary>
        public static void UnifiedStateManagerIntegration()
        {
            // 创建服务容器
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddLogging(builder => builder.AddConsole());
            
            // 注册业务状态管理器
            services.AddSingleton<IBusinessStatusManager, BusinessStatusManager>();
            
            // 注册统一状态管理器
            services.AddSingleton<IUnifiedStateManager, UnifiedStateManager>();
            
            var serviceProvider = services.BuildServiceProvider();
            
            // 获取状态管理器
            var businessStatusManager = serviceProvider.GetService<IBusinessStatusManager>();
            var unifiedStateManager = serviceProvider.GetService<IUnifiedStateManager>();
            var logger = serviceProvider.GetService<ILogger<BusinessStatusUsageExamples>>();
            
            try
            {
                // 1. 注册业务状态类型到业务状态管理器
                RegisterPaymentStatusType(businessStatusManager, logger);
                
                // 2. 注册业务状态类型到统一状态管理器
                var statusType = typeof(PaymentStatus);
                var configuration = new BusinessStatusTypeConfiguration
                {
                    Name = "付款状态",
                    Description = "付款单的状态管理",
                    ApplicableModules = new[] { "财务", "付款管理" },
                    SupportsDynamicStatus = false,
                    StatusValueProvider = new EnumStatusValueProvider<PaymentStatus>()
                };
                
                unifiedStateManager.RegisterBusinessStatusType(statusType, configuration);
                
                // 3. 注册业务状态转换规则到统一状态管理器
                unifiedStateManager.RegisterBusinessTransitionRule(
                    statusType, 
                    PaymentStatus.草稿, 
                    PaymentStatus.待审核, 
                    new BusinessTransitionRule
                    {
                        Name = "提交审核",
                        Description = "提交付款单进行审核",
                        Validator = ctx => ctx.UserRoles.Contains("财务员"),
                        RequiresApproval = true
                    });
                
                // 4. 创建实体对象
                var paymentEntity = new PaymentEntity
                {
                    Id = 1,
                    Amount = 10000m,
                    PaymentStatus = PaymentStatus.草稿,
                    DataStatus = DataStatus.新增,
                    ApprovalStatus = ApprovalStatus.未审核
                };
                
                // 5. 创建状态转换上下文
                var context = new StatusTransitionContext
                {
                    User = "张三",
                    UserRoles = new[] { "财务员" },
                    Module = "付款管理",
                    Operation = "提交审核",
                    Entity = paymentEntity,
                    AdditionalData = new Dictionary<string, object>
                    {
                        { "BusinessType", "付款单" },
                        { "Department", "财务部" }
                    }
                };
                
                // 6. 获取实体的所有状态信息
                var entityStatuses = unifiedStateManager.GetEntityStatuses(paymentEntity, context);
                
                logger?.LogInformation("实体状态信息:");
                foreach (var statusInfo in entityStatuses)
                {
                    logger?.LogInformation($"  属性: {statusInfo.PropertyName}, 状态: {statusInfo.DisplayInfo.DisplayText}, 类型: {statusInfo.StatusType.Name}");
                }
                
                // 7. 获取实体的可用操作列表
                var availableActions = unifiedStateManager.GetEntityAvailableActions(paymentEntity, context);
                
                logger?.LogInformation("实体可用操作:");
                foreach (var actionInfo in availableActions)
                {
                    logger?.LogInformation($"  操作: {actionInfo.ActionName}, 属性: {actionInfo.PropertyName}, 允许: {actionInfo.IsAllowed}");
                }
                
                // 8. 验证业务状态转换
                var validationResult = unifiedStateManager.ValidateBusinessTransition(
                    statusType, 
                    PaymentStatus.草稿, 
                    PaymentStatus.待审核, 
                    context);
                
                logger?.LogInformation($"业务状态转换验证结果: {validationResult.IsAllowed}, 消息: {validationResult.Message}");
                
                if (validationResult.IsAllowed)
                {
                    // 9. 执行业务状态转换
                    var transitionResult = unifiedStateManager.ExecuteBusinessTransition(
                        statusType, 
                        paymentEntity, 
                        PaymentStatus.草稿, 
                        PaymentStatus.待审核, 
                        context);
                    
                    logger?.LogInformation($"业务状态转换执行结果: {transitionResult.IsSuccess}, 消息: {transitionResult.Message}");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "统一状态管理集成示例执行失败");
            }
        }

        #endregion

        #region 示例4: 复杂业务场景

        /// <summary>
        /// 示例4: 复杂业务场景
        /// 演示如何处理复杂的业务场景，包括多状态组合、条件转换和权限控制
        /// </summary>
        public static void ComplexBusinessScenario()
        {
            // 创建服务容器
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddLogging(builder => builder.AddConsole());
            
            // 注册业务状态管理器
            services.AddSingleton<IBusinessStatusManager, BusinessStatusManager>();
            
            var serviceProvider = services.BuildServiceProvider();
            
            // 获取业务状态管理器
            var businessStatusManager = serviceProvider.GetService<IBusinessStatusManager>();
            var logger = serviceProvider.GetService<ILogger<BusinessStatusUsageExamples>>();
            
            try
            {
                // 1. 注册复杂业务状态类型
                RegisterComplexBusinessStatusType(businessStatusManager, logger);
                
                // 2. 注册复杂业务状态转换规则
                RegisterComplexBusinessTransitionRules(businessStatusManager, logger);
                
                // 3. 注册复杂业务状态操作权限规则
                RegisterComplexBusinessActionRules(businessStatusManager, logger);
                
                // 4. 创建不同角色的业务上下文
                var salesContext = new BusinessContext
                {
                    CurrentUser = "销售员张三",
                    UserRoles = new[] { "销售员" },
                    CurrentModule = "销售管理",
                    BusinessType = "销售订单",
                    Department = "销售部",
                    Amount = 10000m
                };
                
                var managerContext = new BusinessContext
                {
                    CurrentUser = "销售主管李四",
                    UserRoles = new[] { "销售主管" },
                    CurrentModule = "销售管理",
                    BusinessType = "销售订单",
                    Department = "销售部",
                    Amount = 10000m
                };
                
                var financeContext = new BusinessContext
                {
                    CurrentUser = "财务主管王五",
                    UserRoles = new[] { "财务主管" },
                    CurrentModule = "销售管理",
                    BusinessType = "销售订单",
                    Department = "财务部",
                    Amount = 10000m
                };
                
                // 5. 验证不同角色下的状态转换
                var salesValidationResult = businessStatusManager.ValidateBusinessTransition(
                    typeof(ComplexOrderStatus), 
                    ComplexOrderStatus.草稿, 
                    ComplexOrderStatus.待审核, 
                    salesContext);
                
                var managerValidationResult = businessStatusManager.ValidateBusinessTransition(
                    typeof(ComplexOrderStatus), 
                    ComplexOrderStatus.草稿, 
                    ComplexOrderStatus.待审核, 
                    managerContext);
                
                var financeValidationResult = businessStatusManager.ValidateBusinessTransition(
                    typeof(ComplexOrderStatus), 
                    ComplexOrderStatus.草稿, 
                    ComplexOrderStatus.待审核, 
                    financeContext);
                
                logger?.LogInformation($"销售员状态转换验证结果: {salesValidationResult.IsAllowed}");
                logger?.LogInformation($"销售主管状态转换验证结果: {managerValidationResult.IsAllowed}");
                logger?.LogInformation($"财务主管状态转换验证结果: {financeValidationResult.IsAllowed}");
                
                // 6. 验证不同金额下的状态转换
                var smallAmountContext = new BusinessContext
                {
                    CurrentUser = "销售员张三",
                    UserRoles = new[] { "销售员" },
                    CurrentModule = "销售管理",
                    BusinessType = "销售订单",
                    Department = "销售部",
                    Amount = 5000m
                };
                
                var largeAmountContext = new BusinessContext
                {
                    CurrentUser = "销售员张三",
                    UserRoles = new[] { "销售员" },
                    CurrentModule = "销售管理",
                    BusinessType = "销售订单",
                    Department = "销售部",
                    Amount = 50000m
                };
                
                var smallAmountValidationResult = businessStatusManager.ValidateBusinessTransition(
                    typeof(ComplexOrderStatus), 
                    ComplexOrderStatus.草稿, 
                    ComplexOrderStatus.待审核, 
                    smallAmountContext);
                
                var largeAmountValidationResult = businessStatusManager.ValidateBusinessTransition(
                    typeof(ComplexOrderStatus), 
                    ComplexOrderStatus.草稿, 
                    ComplexOrderStatus.待审核, 
                    largeAmountContext);
                
                logger?.LogInformation($"小金额订单状态转换验证结果: {smallAmountValidationResult.IsAllowed}");
                logger?.LogInformation($"大金额订单状态转换验证结果: {largeAmountValidationResult.IsAllowed}");
                
                // 7. 检查不同状态下的操作权限
                var canEditDraft = businessStatusManager.CheckBusinessActionPermission(
                    typeof(ComplexOrderStatus), 
                    ComplexOrderStatus.草稿, 
                    "编辑", 
                    salesContext);
                
                var canEditApproved = businessStatusManager.CheckBusinessActionPermission(
                    typeof(ComplexOrderStatus), 
                    ComplexOrderStatus.已审核, 
                    "编辑", 
                    salesContext);
                
                logger?.LogInformation($"草稿状态编辑权限: {canEditDraft}");
                logger?.LogInformation($"已审核状态编辑权限: {canEditApproved}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "复杂业务场景示例执行失败");
            }
        }

        /// <summary>
        /// 注册复杂业务状态类型
        /// </summary>
        /// <param name="businessStatusManager">业务状态管理器</param>
        /// <param name="logger">日志记录器</param>
        private static void RegisterComplexBusinessStatusType(IBusinessStatusManager businessStatusManager, ILogger logger)
        {
            var statusType = typeof(ComplexOrderStatus);
            var configuration = new BusinessStatusTypeConfiguration
            {
                Name = "复杂订单状态",
                Description = "包含多状态组合和条件转换的复杂订单状态",
                ApplicableModules = new[] { "销售", "订单管理", "财务" },
                SupportsDynamicStatus = false,
                StatusValueProvider = new EnumStatusValueProvider<ComplexOrderStatus>()
            };

            businessStatusManager.RegisterBusinessStatusType(statusType, configuration);
            logger?.LogInformation($"已注册复杂订单状态类型: {statusType.Name}");
        }

        /// <summary>
        /// 注册复杂业务状态转换规则
        /// </summary>
        /// <param name="businessStatusManager">业务状态管理器</param>
        /// <param name="logger">日志记录器</param>
        private static void RegisterComplexBusinessTransitionRules(IBusinessStatusManager businessStatusManager, ILogger logger)
        {
            var statusType = typeof(ComplexOrderStatus);
            
            // 草稿 -> 待审核 (销售员可以提交，但需要审核)
            businessStatusManager.RegisterBusinessTransitionRule(
                statusType, 
                ComplexOrderStatus.草稿, 
                ComplexOrderStatus.待审核, 
                new BusinessTransitionRule
                {
                    Name = "提交审核",
                    Description = "提交订单进行审核",
                    Validator = ctx => ctx.UserRoles.Contains("销售员") || ctx.UserRoles.Contains("销售主管"),
                    RequiresApproval = true
                });
            
            // 草稿 -> 待审核 (销售主管可以直接提交，无需审核)
            businessStatusManager.RegisterBusinessTransitionRule(
                statusType, 
                ComplexOrderStatus.草稿, 
                ComplexOrderStatus.待审核, 
                new BusinessTransitionRule
                {
                    Name = "主管提交",
                    Description = "销售主管提交订单",
                    Validator = ctx => ctx.UserRoles.Contains("销售主管") && ctx.Amount <= 10000m,
                    RequiresApproval = false
                });
            
            // 待审核 -> 已审核 (销售主管审核)
            businessStatusManager.RegisterBusinessTransitionRule(
                statusType, 
                ComplexOrderStatus.待审核, 
                ComplexOrderStatus.已审核, 
                new BusinessTransitionRule
                {
                    Name = "审核通过",
                    Description = "销售主管审核通过",
                    Validator = ctx => ctx.UserRoles.Contains("销售主管") && ctx.Amount <= 10000m,
                    RequiresApproval = false
                });
            
            // 待审核 -> 待财务审核 (大金额订单需要财务审核)
            businessStatusManager.RegisterBusinessTransitionRule(
                statusType, 
                ComplexOrderStatus.待审核, 
                ComplexOrderStatus.待财务审核, 
                new BusinessTransitionRule
                {
                    Name = "提交财务审核",
                    Description = "大金额订单提交财务审核",
                    Validator = ctx => ctx.UserRoles.Contains("销售主管") && ctx.Amount > 10000m,
                    RequiresApproval = false
                });
            
            // 待财务审核 -> 已审核 (财务主管审核)
            businessStatusManager.RegisterBusinessTransitionRule(
                statusType, 
                ComplexOrderStatus.待财务审核, 
                ComplexOrderStatus.已审核, 
                new BusinessTransitionRule
                {
                    Name = "财务审核通过",
                    Description = "财务主管审核通过",
                    Validator = ctx => ctx.UserRoles.Contains("财务主管"),
                    RequiresApproval = false
                });
            
            logger?.LogInformation($"已注册复杂订单状态转换规则");
        }

        /// <summary>
        /// 注册复杂业务状态操作权限规则
        /// </summary>
        /// <param name="businessStatusManager">业务状态管理器</param>
        /// <param name="logger">日志记录器</param>
        private static void RegisterComplexBusinessActionRules(IBusinessStatusManager businessStatusManager, ILogger logger)
        {
            var statusType = typeof(ComplexOrderStatus);
            
            // 草稿状态的操作权限
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                ComplexOrderStatus.草稿, 
                "编辑", 
                new BusinessActionRule
                {
                    ActionName = "编辑",
                    Description = "编辑草稿状态的订单",
                    PermissionValidator = ctx => ctx.UserRoles.Contains("销售员") || ctx.UserRoles.Contains("销售主管"),
                    ApplicableRoles = new[] { "销售员", "销售主管" }
                });
            
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                ComplexOrderStatus.草稿, 
                "删除", 
                new BusinessActionRule
                {
                    ActionName = "删除",
                    Description = "删除草稿状态的订单",
                    PermissionValidator = ctx => (ctx.UserRoles.Contains("销售员") && ctx.Amount <= 5000m) || 
                                              ctx.UserRoles.Contains("销售主管"),
                    ApplicableRoles = new[] { "销售员", "销售主管" }
                });
            
            // 待审核状态的操作权限
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                ComplexOrderStatus.待审核, 
                "审核", 
                new BusinessActionRule
                {
                    ActionName = "审核",
                    Description = "审核待审核状态的订单",
                    PermissionValidator = ctx => ctx.UserRoles.Contains("销售主管") && ctx.Amount <= 10000m,
                    ApplicableRoles = new[] { "销售主管" }
                });
            
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                ComplexOrderStatus.待审核, 
                "提交财务审核", 
                new BusinessActionRule
                {
                    ActionName = "提交财务审核",
                    Description = "提交大金额订单进行财务审核",
                    PermissionValidator = ctx => ctx.UserRoles.Contains("销售主管") && ctx.Amount > 10000m,
                    ApplicableRoles = new[] { "销售主管" }
                });
            
            // 待财务审核状态的操作权限
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                ComplexOrderStatus.待财务审核, 
                "财务审核", 
                new BusinessActionRule
                {
                    ActionName = "财务审核",
                    Description = "财务主管审核订单",
                    PermissionValidator = ctx => ctx.UserRoles.Contains("财务主管"),
                    ApplicableRoles = new[] { "财务主管" }
                });
            
            // 已审核状态的操作权限
            businessStatusManager.RegisterBusinessActionRule(
                statusType, 
                ComplexOrderStatus.已审核, 
                "查看", 
                new BusinessActionRule
                {
                    ActionName = "查看",
                    Description = "查看已审核的订单",
                    PermissionValidator = ctx => true,
                    ApplicableRoles = new[] { "销售员", "销售主管", "财务主管" }
                });
            
            logger?.LogInformation($"已注册复杂订单状态操作权限规则");
        }

        #endregion

        #region 辅助类和枚举

        /// <summary>
        /// 付款实体类
        /// </summary>
        public class PaymentEntity
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
            public PaymentStatus PaymentStatus { get; set; }
            public DataStatus DataStatus { get; set; }
            public ApprovalStatus ApprovalStatus { get; set; }
        }

        /// <summary>
        /// 动态订单状态枚举
        /// </summary>
        public enum DynamicOrderStatus
        {
            [System.ComponentModel.Description("待处理")]
            待处理,
            
            [System.ComponentModel.Description("已处理")]
            已处理,
            
            [System.ComponentModel.Description("紧急待处理")]
            紧急待处理,
            
            [System.ComponentModel.Description("紧急已处理")]
            紧急已处理
        }

        /// <summary>
        /// 动态订单状态值提供程序
        /// </summary>
        public class DynamicOrderStatusValueProvider : IBusinessStatusValueProvider
        {
            public IEnumerable<object> GetAllStatusValues(BusinessContext context)
            {
                if (context.IsUrgent)
                {
                    return new object[] { DynamicOrderStatus.紧急待处理, DynamicOrderStatus.紧急已处理 };
                }
                else
                {
                    return new object[] { DynamicOrderStatus.待处理, DynamicOrderStatus.已处理 };
                }
            }

            public BusinessStatusDisplayInfo GetStatusDisplayInfo(object statusValue, BusinessContext context)
            {
                if (statusValue is DynamicOrderStatus enumValue)
                {
                    var displayInfo = new BusinessStatusDisplayInfo();
                    
                    var fieldInfo = typeof(DynamicOrderStatus).GetField(enumValue.ToString());
                    var descriptionAttribute = fieldInfo?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                        .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;
                    
                    displayInfo.DisplayText = descriptionAttribute?.Description ?? enumValue.ToString();
                    displayInfo.ColorCode = enumValue.ToString().Contains("紧急") ? "#FF0000" : "#1E90FF";
                    
                    return displayInfo;
                }
                
                return null;
            }

            public bool IsValidStatusValue(object statusValue, BusinessContext context)
            {
                return statusValue is DynamicOrderStatus && Enum.IsDefined(typeof(DynamicOrderStatus), statusValue);
            }
        }

        /// <summary>
        /// 复杂订单状态枚举
        /// </summary>
        public enum ComplexOrderStatus
        {
            [System.ComponentModel.Description("草稿")]
            草稿,
            
            [System.ComponentModel.Description("待审核")]
            待审核,
            
            [System.ComponentModel.Description("待财务审核")]
            待财务审核,
            
            [System.ComponentModel.Description("已审核")]
            已审核,
            
            [System.ComponentModel.Description("已发货")]
            已发货,
            
            [System.ComponentModel.Description("已完成")]
            已完成
        }

        #endregion
    }
}