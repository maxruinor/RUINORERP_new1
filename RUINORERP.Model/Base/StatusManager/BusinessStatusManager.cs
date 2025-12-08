using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 文件: BusinessStatusManager.cs
    /// 版本: V3增强版 - 业务性状态动态管理实现
    /// 说明: 实现业务性状态的动态管理功能，支持不同业务场景的状态定义和转换
    /// 创建日期: 2024年
    /// 作者: RUINOR ERP开发团队
    /// 
    /// 版本标识：
    /// V3增强版: 实现业务性状态的动态管理，支持业务上下文感知
    /// 功能: 提供业务性状态的动态注册、验证、转换和上下文管理
    /// </summary>

    /// <summary>
    /// 业务性状态管理器实现
    /// 提供业务性状态的动态管理功能，支持不同业务场景的状态定义和转换
    /// </summary>
    public class BusinessStatusManager : IBusinessStatusManager
    {
        #region 私有字段

        /// <summary>
        /// 业务状态类型配置缓存
        /// </summary>
        private readonly Dictionary<Type, BusinessStatusTypeConfiguration> _statusTypeConfigurations;

        /// <summary>
        /// 业务状态转换规则缓存
        /// </summary>
        private readonly Dictionary<Type, Dictionary<object, List<BusinessTransitionRule>>> _transitionRules;

        /// <summary>
        /// 业务状态操作权限规则缓存
        /// </summary>
        private readonly Dictionary<Type, Dictionary<object, Dictionary<string, BusinessActionRule>>> _actionRules;

        /// <summary>
        /// 业务状态元数据缓存
        /// </summary>
        private readonly Dictionary<Type, BusinessStatusMetadata> _statusMetadata;

        /// <summary>
        /// 内存缓存
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<BusinessStatusManager> _logger;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化业务状态管理器
        /// </summary>
        /// <param name="cache">内存缓存</param>
        /// <param name="logger">日志记录器</param>
        public BusinessStatusManager(IMemoryCache cache, ILogger<BusinessStatusManager> logger = null)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger;
            
            _statusTypeConfigurations = new Dictionary<Type, BusinessStatusTypeConfiguration>();
            _transitionRules = new Dictionary<Type, Dictionary<object, List<BusinessTransitionRule>>>();
            _actionRules = new Dictionary<Type, Dictionary<object, Dictionary<string, BusinessActionRule>>>();
            _statusMetadata = new Dictionary<Type, BusinessStatusMetadata>();

            // 初始化默认业务状态类型
            InitializeDefaultBusinessStatusTypes();
        }

        #endregion

        #region IBusinessStatusManager 实现

        /// <summary>
        /// 注册业务状态类型
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="configuration">状态配置</param>
        public void RegisterBusinessStatusType(Type statusType, BusinessStatusTypeConfiguration configuration)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));
            
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _statusTypeConfigurations[statusType] = configuration;
            
            // 初始化状态元数据
            if (!_statusMetadata.ContainsKey(statusType))
            {
                _statusMetadata[statusType] = new BusinessStatusMetadata
                {
                    StatusType = statusType,
                    StatusValues = configuration.StatusValueProvider?.GetAllStatusValues(new BusinessContext())?.ToList() ?? new List<object>()
                };
            }

            // 初始化转换规则字典
            if (!_transitionRules.ContainsKey(statusType))
            {
                _transitionRules[statusType] = new Dictionary<object, List<BusinessTransitionRule>>();
            }

            // 初始化操作规则字典
            if (!_actionRules.ContainsKey(statusType))
            {
                _actionRules[statusType] = new Dictionary<object, Dictionary<string, BusinessActionRule>>();
            }

            _logger?.LogInformation($"已注册业务状态类型: {statusType.Name}");
        }

        /// <summary>
        /// 获取业务状态类型配置
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <returns>状态配置</returns>
        public BusinessStatusTypeConfiguration GetBusinessStatusTypeConfiguration(Type statusType)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            _statusTypeConfigurations.TryGetValue(statusType, out var configuration);
            return configuration;
        }

        /// <summary>
        /// 注册业务状态转换规则
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="rule">转换规则</param>
        public void RegisterBusinessTransitionRule(Type statusType, object fromStatus, object toStatus, BusinessTransitionRule rule)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));
            
            if (fromStatus == null)
                throw new ArgumentNullException(nameof(fromStatus));
            
            if (toStatus == null)
                throw new ArgumentNullException(nameof(toStatus));
            
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            if (!_transitionRules.ContainsKey(statusType))
            {
                _transitionRules[statusType] = new Dictionary<object, List<BusinessTransitionRule>>();
            }

            if (!_transitionRules[statusType].ContainsKey(fromStatus))
            {
                _transitionRules[statusType][fromStatus] = new List<BusinessTransitionRule>();
            }

            _transitionRules[statusType][fromStatus].Add(rule);

            // 更新状态元数据
            if (!_statusMetadata.ContainsKey(statusType))
            {
                _statusMetadata[statusType] = new BusinessStatusMetadata { StatusType = statusType };
            }

            if (!_statusMetadata[statusType].TransitionRules.ContainsKey(fromStatus))
            {
                _statusMetadata[statusType].TransitionRules[fromStatus] = new List<object>();
            }

            _statusMetadata[statusType].TransitionRules[fromStatus].Add(toStatus);

            _logger?.LogInformation($"已注册业务状态转换规则: {statusType.Name} {fromStatus} -> {toStatus}");
        }

        /// <summary>
        /// 验证业务状态转换是否允许
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">业务上下文</param>
        /// <returns>验证结果</returns>
        public BusinessTransitionValidationResult ValidateBusinessTransition(Type statusType, object fromStatus, object toStatus, BusinessContext context)
        {
            var result = new BusinessTransitionValidationResult();

            try
            {
                // 检查状态类型是否已注册
                if (!_statusTypeConfigurations.ContainsKey(statusType))
                {
                    result.IsAllowed = false;
                    result.Message = $"未注册的业务状态类型: {statusType.Name}";
                    return result;
                }

                // 检查状态值是否有效
                var config = _statusTypeConfigurations[statusType];
                if (config.StatusValueProvider != null && 
                    !config.StatusValueProvider.IsValidStatusValue(fromStatus, context))
                {
                    result.IsAllowed = false;
                    result.Message = $"无效的源状态: {fromStatus}";
                    return result;
                }

                if (config.StatusValueProvider != null && 
                    !config.StatusValueProvider.IsValidStatusValue(toStatus, context))
                {
                    result.IsAllowed = false;
                    result.Message = $"无效的目标状态: {toStatus}";
                    return result;
                }

                // 检查转换规则
                if (_transitionRules.ContainsKey(statusType) && 
                    _transitionRules[statusType].ContainsKey(fromStatus))
                {
                    var rules = _transitionRules[statusType][fromStatus];
                    var applicableRules = rules.Where(r => r.Validator == null || r.Validator(context)).ToList();

                    // 检查是否有规则允许转换到目标状态
                    var hasValidRule = false;
                    int maxApprovalLevel = 0;

                    foreach (var rule in applicableRules)
                    {
                        // 这里简化处理，实际应该检查规则是否允许转换到目标状态
                        // 在实际实现中，可能需要更复杂的规则匹配逻辑
                        hasValidRule = true;
                        if (rule.RequiresApproval)
                        {
                            maxApprovalLevel = Math.Max(maxApprovalLevel, 1); // 简化处理，实际应该有审批级别
                        }
                    }

                    if (hasValidRule)
                    {
                        result.IsAllowed = true;
                        result.RequiredApprovalLevel = maxApprovalLevel;
                        result.Message = "状态转换验证通过";
                    }
                    else
                    {
                        result.IsAllowed = false;
                        result.Message = "没有适用的转换规则允许此状态转换";
                    }
                }
                else
                {
                    result.IsAllowed = false;
                    result.Message = "未找到适用的转换规则";
                }
            }
            catch (Exception ex)
            {
                result.IsAllowed = false;
                result.Message = $"验证状态转换时发生错误: {ex.Message}";
                _logger?.LogError(ex, $"验证业务状态转换时发生错误: {statusType.Name} {fromStatus} -> {toStatus}");
            }

            return result;
        }

        /// <summary>
        /// 执行业务状态转换
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="entity">实体对象</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">业务上下文</param>
        /// <returns>转换结果</returns>
        public BusinessTransitionResult ExecuteBusinessTransition(Type statusType, object entity, object fromStatus, object toStatus, BusinessContext context)
        {
            var result = new BusinessTransitionResult
            {
                FromStatus = fromStatus,
                ToStatus = toStatus,
                User = context?.CurrentUser
            };

            try
            {
                // 验证转换是否允许
                var validationResult = ValidateBusinessTransition(statusType, fromStatus, toStatus, context);
                if (!validationResult.IsAllowed)
                {
                    result.IsSuccess = false;
                    result.Message = validationResult.Message;
                    return result;
                }

                // 执行前置处理
                if (_transitionRules.ContainsKey(statusType) && 
                    _transitionRules[statusType].ContainsKey(fromStatus))
                {
                    var rules = _transitionRules[statusType][fromStatus];
                    var applicableRules = rules.Where(r => r.Validator == null || r.Validator(context)).ToList();

                    foreach (var rule in applicableRules)
                    {
                        if (rule.PreTransitionHandler != null)
                        {
                            var preResult = rule.PreTransitionHandler(context);
                            if (!preResult.IsSuccess)
                            {
                                result.IsSuccess = false;
                                result.Message = $"前置处理失败: {preResult.Message}";
                                return result;
                            }
                        }
                    }
                }

                // 执行状态转换 - 这里简化处理，实际应该通过反射设置实体状态
                // 在实际实现中，可能需要更复杂的状态设置逻辑
                if (entity != null)
                {
                    // 通过反射设置状态属性
                    var statusProperty = entity.GetType().GetProperties()
                        .FirstOrDefault(p => p.PropertyType == statusType);
                    
                    if (statusProperty != null && statusProperty.CanWrite)
                    {
                        statusProperty.SetValue(entity, toStatus);
                    }
                }

                // 执行后置处理
                if (_transitionRules.ContainsKey(statusType) && 
                    _transitionRules[statusType].ContainsKey(fromStatus))
                {
                    var rules = _transitionRules[statusType][fromStatus];
                    var applicableRules = rules.Where(r => r.Validator == null || r.Validator(context)).ToList();

                    foreach (var rule in applicableRules)
                    {
                        if (rule.PostTransitionHandler != null)
                        {
                            var postResult = rule.PostTransitionHandler(context);
                            if (!postResult.IsSuccess)
                            {
                                result.IsSuccess = false;
                                result.Message = $"后置处理失败: {postResult.Message}";
                                return result;
                            }
                        }
                    }
                }

                result.IsSuccess = true;
                result.Message = "状态转换成功";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"执行状态转换时发生错误: {ex.Message}";
                result.Exception = ex;
                _logger?.LogError(ex, $"执行业务状态转换时发生错误: {statusType.Name} {fromStatus} -> {toStatus}");
            }

            return result;
        }

        /// <summary>
        /// 获取业务状态可转换的状态列表
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="context">业务上下文</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<object> GetAvailableBusinessTransitions(Type statusType, object fromStatus, BusinessContext context)
        {
            var result = new List<object>();

            try
            {
                // 检查状态类型是否已注册
                if (!_statusTypeConfigurations.ContainsKey(statusType))
                {
                    return result;
                }

                // 获取所有可能的目标状态
                var config = _statusTypeConfigurations[statusType];
                if (config.StatusValueProvider != null)
                {
                    var allStatusValues = config.StatusValueProvider.GetAllStatusValues(context).ToList();
                    
                    // 检查每个状态是否可以转换
                    foreach (var toStatus in allStatusValues)
                    {
                        if (toStatus.Equals(fromStatus))
                            continue;

                        var validationResult = ValidateBusinessTransition(statusType, fromStatus, toStatus, context);
                        if (validationResult.IsAllowed)
                        {
                            result.Add(toStatus);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取可转换状态列表时发生错误: {statusType.Name} {fromStatus}");
            }

            return result;
        }

        /// <summary>
        /// 注册业务状态操作权限规则
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态</param>
        /// <param name="action">操作名称</param>
        /// <param name="rule">权限规则</param>
        public void RegisterBusinessActionRule(Type statusType, object status, string action, BusinessActionRule rule)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));
            
            if (status == null)
                throw new ArgumentNullException(nameof(status));
            
            if (string.IsNullOrEmpty(action))
                throw new ArgumentNullException(nameof(action));
            
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            if (!_actionRules.ContainsKey(statusType))
            {
                _actionRules[statusType] = new Dictionary<object, Dictionary<string, BusinessActionRule>>();
            }

            if (!_actionRules[statusType].ContainsKey(status))
            {
                _actionRules[statusType][status] = new Dictionary<string, BusinessActionRule>();
            }

            _actionRules[statusType][status][action] = rule;

            _logger?.LogInformation($"已注册业务状态操作权限规则: {statusType.Name} {status} {action}");
        }

        /// <summary>
        /// 检查业务状态操作权限
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态</param>
        /// <param name="action">操作名称</param>
        /// <param name="context">业务上下文</param>
        /// <returns>权限检查结果</returns>
        public bool CheckBusinessActionPermission(Type statusType, object status, string action, BusinessContext context)
        {
            try
            {
                // 检查状态类型是否已注册
                if (!_statusTypeConfigurations.ContainsKey(statusType))
                {
                    return false;
                }

                // 检查操作规则
                if (_actionRules.ContainsKey(statusType) && 
                    _actionRules[statusType].ContainsKey(status) &&
                    _actionRules[statusType][status].ContainsKey(action))
                {
                    var rule = _actionRules[statusType][status][action];
                    
                    // 检查角色权限
                    if (rule.ApplicableRoles.Length > 0 && 
                        context?.UserRoles != null && 
                        !rule.ApplicableRoles.Any(role => context.UserRoles.Contains(role)))
                    {
                        return false;
                    }

                    // 执行权限验证
                    if (rule.PermissionValidator != null)
                    {
                        return rule.PermissionValidator(context);
                    }

                    return true;
                }

                // 默认拒绝权限
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"检查业务状态操作权限时发生错误: {statusType.Name} {status} {action}");
                return false;
            }
        }

        /// <summary>
        /// 获取业务状态显示信息
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态</param>
        /// <param name="context">业务上下文</param>
        /// <returns>显示信息</returns>
        public BusinessStatusDisplayInfo GetBusinessStatusDisplayInfo(Type statusType, object status, BusinessContext context)
        {
            var result = new BusinessStatusDisplayInfo();

            try
            {
                // 检查状态类型是否已注册
                if (!_statusTypeConfigurations.ContainsKey(statusType))
                {
                    result.DisplayText = status?.ToString() ?? "未知状态";
                    return result;
                }

                // 获取状态显示信息
                var config = _statusTypeConfigurations[statusType];
                if (config.StatusValueProvider != null)
                {
                    var displayInfo = config.StatusValueProvider.GetStatusDisplayInfo(status, context);
                    if (displayInfo != null)
                    {
                        return displayInfo;
                    }
                }

                // 默认显示信息
                result.DisplayText = status?.ToString() ?? "未知状态";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取业务状态显示信息时发生错误: {statusType.Name} {status}");
                result.DisplayText = status?.ToString() ?? "未知状态";
            }

            return result;
        }

        /// <summary>
        /// 获取业务状态元数据
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <returns>状态元数据</returns>
        public BusinessStatusMetadata GetBusinessStatusMetadata(Type statusType)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            _statusMetadata.TryGetValue(statusType, out var metadata);
            return metadata;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化默认业务状态类型
        /// </summary>
        private void InitializeDefaultBusinessStatusTypes()
        {
            try
            {
                // 注册付款状态类型
                RegisterPaymentStatusType();

                // 注册预付款状态类型
                RegisterPrePaymentStatusType();

                // 注册应收应付状态类型
                RegisterARAPStatusType();

                // 注册退款状态类型
                RegisterRefundStatusType();

                // 注册审批状态类型
                RegisterApprovalStatusType();

                _logger?.LogInformation("默认业务状态类型初始化完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化默认业务状态类型时发生错误");
            }
        }

        /// <summary>
        /// 注册付款状态类型
        /// </summary>
        private void RegisterPaymentStatusType()
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

            RegisterBusinessStatusType(statusType, configuration);

            // 注册转换规则
            RegisterBusinessTransitionRule(statusType, PaymentStatus.草稿, PaymentStatus.待审核, new BusinessTransitionRule
            {
                Name = "提交审核",
                Description = "提交付款单进行审核",
                Validator = ctx => true,
                RequiresApproval = true
            });

            RegisterBusinessTransitionRule(statusType, PaymentStatus.待审核, PaymentStatus.已支付, new BusinessTransitionRule
            {
                Name = "支付完成",
                Description = "付款单支付完成",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, PaymentStatus.待审核, PaymentStatus.草稿, new BusinessTransitionRule
            {
                Name = "退回草稿",
                Description = "审核不通过，退回草稿状态",
                Validator = ctx => true,
                RequiresApproval = false
            });

            // 注册操作权限规则
            RegisterBusinessActionRule(statusType, PaymentStatus.草稿, "编辑", new BusinessActionRule
            {
                ActionName = "编辑",
                Description = "编辑草稿状态的付款单",
                PermissionValidator = ctx => true,
                ApplicableRoles = new[] { "财务", "付款员" }
            });

            RegisterBusinessActionRule(statusType, PaymentStatus.草稿, "删除", new BusinessActionRule
            {
                ActionName = "删除",
                Description = "删除草稿状态的付款单",
                PermissionValidator = ctx => true,
                ApplicableRoles = new[] { "财务", "付款员" }
            });

            RegisterBusinessActionRule(statusType, PaymentStatus.待审核, "审核", new BusinessActionRule
            {
                ActionName = "审核",
                Description = "审核付款单",
                PermissionValidator = ctx => true,
                ApplicableRoles = new[] { "财务主管" }
            });
        }

        /// <summary>
        /// 注册预付款状态类型
        /// </summary>
        private void RegisterPrePaymentStatusType()
        {
            var statusType = typeof(PrePaymentStatus);
            var configuration = new BusinessStatusTypeConfiguration
            {
                Name = "预付款状态",
                Description = "预付款单的状态管理",
                ApplicableModules = new[] { "财务", "预付款管理" },
                SupportsDynamicStatus = false,
                StatusValueProvider = new EnumStatusValueProvider<PrePaymentStatus>()
            };

            RegisterBusinessStatusType(statusType, configuration);

            // 注册转换规则
            RegisterBusinessTransitionRule(statusType, PrePaymentStatus.草稿, PrePaymentStatus.待审核, new BusinessTransitionRule
            {
                Name = "提交审核",
                Description = "提交预付款单进行审核",
                Validator = ctx => true,
                RequiresApproval = true
            });

            RegisterBusinessTransitionRule(statusType, PrePaymentStatus.待审核, PrePaymentStatus.已生效, new BusinessTransitionRule
            {
                Name = "审核通过",
                Description = "预付款单审核通过",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, PrePaymentStatus.已生效, PrePaymentStatus.待核销, new BusinessTransitionRule
            {
                Name = "开始核销",
                Description = "预付款开始核销",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, PrePaymentStatus.待核销, PrePaymentStatus.部分核销, new BusinessTransitionRule
            {
                Name = "部分核销",
                Description = "预付款部分核销",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, PrePaymentStatus.部分核销, PrePaymentStatus.全额核销, new BusinessTransitionRule
            {
                Name = "全额核销",
                Description = "预付款全额核销",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, PrePaymentStatus.全额核销, PrePaymentStatus.已结案, new BusinessTransitionRule
            {
                Name = "结案",
                Description = "预付款结案",
                Validator = ctx => true,
                RequiresApproval = false
            });
        }

        /// <summary>
        /// 注册应收应付状态类型
        /// </summary>
        private void RegisterARAPStatusType()
        {
            var statusType = typeof(ARAPStatus);
            var configuration = new BusinessStatusTypeConfiguration
            {
                Name = "应收应付状态",
                Description = "应收应付单据的状态管理",
                ApplicableModules = new[] { "财务", "应收应付管理" },
                SupportsDynamicStatus = false,
                StatusValueProvider = new EnumStatusValueProvider<ARAPStatus>()
            };

            RegisterBusinessStatusType(statusType, configuration);

            // 注册转换规则
            RegisterBusinessTransitionRule(statusType, ARAPStatus.草稿, ARAPStatus.待审核, new BusinessTransitionRule
            {
                Name = "提交审核",
                Description = "提交应收应付单进行审核",
                Validator = ctx => true,
                RequiresApproval = true
            });

            RegisterBusinessTransitionRule(statusType, ARAPStatus.待审核, ARAPStatus.待支付, new BusinessTransitionRule
            {
                Name = "审核通过",
                Description = "应收应付单审核通过",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, ARAPStatus.待支付, ARAPStatus.部分支付, new BusinessTransitionRule
            {
                Name = "部分支付",
                Description = "应收应付单部分支付",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, ARAPStatus.部分支付, ARAPStatus.全部支付, new BusinessTransitionRule
            {
                Name = "全部支付",
                Description = "应收应付单全部支付",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, ARAPStatus.全部支付, ARAPStatus.已冲销, new BusinessTransitionRule
            {
                Name = "冲销",
                Description = "应收应付单冲销",
                Validator = ctx => true,
                RequiresApproval = false
            });
        }

        /// <summary>
        /// 注册退款状态类型
        /// </summary>
        private void RegisterRefundStatusType()
        {
            var statusType = typeof(RefundStatus);
            var configuration = new BusinessStatusTypeConfiguration
            {
                Name = "退款状态",
                Description = "退款单的状态管理",
                ApplicableModules = new[] { "销售", "售后", "财务" },
                SupportsDynamicStatus = false,
                StatusValueProvider = new EnumStatusValueProvider<RefundStatus>()
            };

            RegisterBusinessStatusType(statusType, configuration);

            // 注册转换规则
            RegisterBusinessTransitionRule(statusType, RefundStatus.未退款等待退货, RefundStatus.已退款等待退货, new BusinessTransitionRule
            {
                Name = "已退款",
                Description = "已退款，等待退货",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, RefundStatus.未退款等待退货, RefundStatus.未退款已退货, new BusinessTransitionRule
            {
                Name = "已退货",
                Description = "已退货，未退款",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, RefundStatus.已退款等待退货, RefundStatus.已退款已退货, new BusinessTransitionRule
            {
                Name = "已退款已退货",
                Description = "已退款且已退货",
                Validator = ctx => true,
                RequiresApproval = false
            });

            RegisterBusinessTransitionRule(statusType, RefundStatus.未退款已退货, RefundStatus.已退款已退货, new BusinessTransitionRule
            {
                Name = "退款",
                Description = "对已退货的商品进行退款",
                Validator = ctx => true,
                RequiresApproval = false
            });
        }

        /// <summary>
        /// 注册审批状态类型
        /// </summary>
        private void RegisterApprovalStatusType()
        {
            var statusType = typeof(ApprovalStatus);
            var configuration = new BusinessStatusTypeConfiguration
            {
                Name = "审批状态",
                Description = "单据的审批状态管理",
                ApplicableModules = new[] { "通用" },
                SupportsDynamicStatus = false,
                StatusValueProvider = new EnumStatusValueProvider<ApprovalStatus>()
            };

            RegisterBusinessStatusType(statusType, configuration);

            // 注册转换规则
            RegisterBusinessTransitionRule(statusType, ApprovalStatus.未审核, ApprovalStatus.已审核, new BusinessTransitionRule
            {
                Name = "审核",
                Description = "审核单据",
                Validator = ctx => true,
                RequiresApproval = false
            });
        }

        #endregion
    }

    /// <summary>
    /// 枚举状态值提供程序
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    public class EnumStatusValueProvider<T> : IBusinessStatusValueProvider where T : Enum
    {
        /// <summary>
        /// 获取所有状态值
        /// </summary>
        /// <param name="context">业务上下文</param>
        /// <returns>状态值列表</returns>
        public IEnumerable<object> GetAllStatusValues(BusinessContext context)
        {
            return Enum.GetValues(typeof(T)).Cast<object>();
        }

        /// <summary>
        /// 获取状态值显示信息
        /// </summary>
        /// <param name="statusValue">状态值</param>
        /// <param name="context">业务上下文</param>
        /// <returns>显示信息</returns>
        public BusinessStatusDisplayInfo GetStatusDisplayInfo(object statusValue, BusinessContext context)
        {
            if (statusValue != null && Enum.IsDefined(typeof(T), statusValue))
            {
                var displayInfo = new BusinessStatusDisplayInfo();
                
                // 获取枚举的描述信息
                var fieldInfo = typeof(T).GetField(statusValue.ToString());
                var descriptionAttribute = fieldInfo?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                    .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;
                
                displayInfo.DisplayText = descriptionAttribute?.Description ?? statusValue.ToString();
                
                // 根据状态设置颜色
                displayInfo.ColorCode = GetStatusColorCode(statusValue);
                
                return displayInfo;
            }
            
            return null;
        }

        /// <summary>
        /// 验证状态值是否有效
        /// </summary>
        /// <param name="statusValue">状态值</param>
        /// <param name="context">业务上下文</param>
        /// <returns>是否有效</returns>
        public bool IsValidStatusValue(object statusValue, BusinessContext context)
        {
            return statusValue != null && Enum.IsDefined(typeof(T), statusValue);
        }

        /// <summary>
        /// 获取状态颜色代码
        /// </summary>
        /// <param name="status">状态值</param>
        /// <returns>颜色代码</returns>
        private string GetStatusColorCode(object status)
        {
            // 根据不同的枚举类型和状态值返回不同的颜色
            // 这里简化处理，实际应该根据业务需求设置
            var statusStr = status?.ToString();
            
            switch (statusStr)
            {
                case "草稿":
                    return "#FFA500"; // 橙色
                
                case "待审核":
                    return "#1E90FF"; // 蓝色
                
                case "已支付":
                case "已生效":
                case "全部支付":
                case "已审核":
                    return "#32CD32"; // 绿色
                
                case "坏账":
                    return "#FF0000"; // 红色
                
                default:
                    return "#808080"; // 灰色
            }
        }
    }
}