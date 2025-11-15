using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Polly;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using System.Reflection;
using System.Linq;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令类型分类器
    /// 用于根据命令特性对命令进行分类，以应用不同的熔断器策略
    /// </summary>
    public class CommandTypeClassifier
    {
        // 命令ID到命令类型的缓存
        // 命令ID到命令类型的缓存
        private readonly ConcurrentDictionary<string, CommandCategory> _commandCategoryCache;

        // 命令前缀规则映射
        private readonly Dictionary<string, CommandCategory> _prefixRules;

        // 命令ID规则映射
        private readonly Dictionary<string, CommandCategory> _exactMatchRules;

        // 命令名称关键词规则映射
        private readonly Dictionary<string, CommandCategory> _keywordRules;

        // 命令类别到熔断器类别的映射
        private readonly Dictionary<CommandCategory, CommandCategory> _categoryMappings;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandTypeClassifier()
        {
            _commandCategoryCache = new ConcurrentDictionary<string, CommandCategory>();
            _prefixRules = new Dictionary<string, CommandCategory>(StringComparer.OrdinalIgnoreCase);
            _exactMatchRules = new Dictionary<string, CommandCategory>(StringComparer.OrdinalIgnoreCase);
            _keywordRules = new Dictionary<string, CommandCategory>(StringComparer.OrdinalIgnoreCase);
            _categoryMappings = new Dictionary<CommandCategory, CommandCategory>();

            // 初始化默认规则和映射
            InitializeDefaultRules();
            InitializeCategoryMappings();
            InitializeCommandCodeMappings();
        }

        /// <summary>
        /// 初始化默认分类规则
        /// 这些规则用于处理未在CommandCatalog中明确定义但遵循命名约定的命令
        /// </summary>
        private void InitializeDefaultRules()
        {
            // 注意：这些规则是作为反射自动映射的补充，处理特殊命名约定的命令
            // 在DetermineCommandCategory方法中，精确匹配规则（来自反射映射）的优先级高于这些规则

            // 前缀匹配规则 - 针对特定业务领域的命令命名
            _prefixRules["TXN_"] = CommandCategory.Authentication;
            _prefixRules["PAY_"] = CommandCategory.Authentication;
            _prefixRules["ORDER_"] = CommandCategory.Authentication;

            // 这些批处理和数据同步前缀可能需要特别处理
            _prefixRules["PROCESS_"] = CommandCategory.DataSync;
            _prefixRules["MIGRATE_"] = CommandCategory.DataSync;

            // 对于外部系统集成的特殊前缀
            _prefixRules["EXT_"] = CommandCategory.Workflow;
            _prefixRules["GATEWAY_"] = CommandCategory.Workflow;

            // 关键词匹配规则 - 增强现有的只读命令识别
            string[] additionalQueryKeywords = { "VIEW", "SEARCH", "FIND" };
            foreach (var keyword in additionalQueryKeywords)
            {
                _keywordRules[keyword] = CommandCategory.Message;
            }
        }

        /// <summary>
        /// 初始化命令类别到自身的映射
        /// 简化实现，直接返回命令本身的类别
        /// </summary>
        private void InitializeCategoryMappings()
        {
            // 为所有系统命令类别创建直接映射
            foreach (CommandCategory category in Enum.GetValues(typeof(CommandCategory)))
            {
                _categoryMappings[category] = category;
            }
        }

        /// <summary>
        /// 为CommandCatalog中的特定命令码添加精确映射规则
        /// 基于CommandCatalog.cs文件中的命令定义实现精确映射
        /// 使用反射自动从CommandCatalog建立命令码到类别的映射，避免硬编码
        /// </summary>
        private void InitializeCommandCodeMappings()
        {
            try
            {
                // 使用反射获取CommandCatalog中的所有命令常量
                var commandFields = typeof(CommandCatalog).GetFields(BindingFlags.Public |
                                                                 BindingFlags.Static |
                                                                 BindingFlags.FlattenHierarchy);

                foreach (var field in commandFields)
                {
                    // 只处理ushort类型的常量字段
                    if (field.FieldType == typeof(ushort) && field.IsLiteral && !field.IsInitOnly)
                    {
                        ushort commandCode = (ushort)field.GetValue(null);
                        string fieldName = field.Name;
                        string commandHex = $"0x{commandCode:X4}";

                        // 从字段名中提取类别前缀（例如从"Authentication_Login"中提取"Authentication"）
                        int underscoreIndex = fieldName.IndexOf('_');
                        if (underscoreIndex > 0)
                        {
                            string categoryPrefix = fieldName.Substring(0, underscoreIndex);

                            // 尝试将前缀映射到对应的CommandCategory枚举
                            if (Enum.TryParse<CommandCategory>(categoryPrefix, true, out CommandCategory category))
                            {
                                // 添加精确匹配规则
                                AddExactMatchRule(commandHex, category);
                            }
                        }
                    }
                }

                // 添加特殊映射（如果有需要覆盖默认映射的情况）
                AddSpecialMappings();
            }
            catch (Exception ex)
            {
                // 如果反射过程出错，回退到基本的范围判断
                // 这种情况不应该在正常运行时发生，但为了安全起见保留
                InitializeFallbackMappings();
            }
        }

        /// <summary>
        /// 添加需要特殊处理的命令映射
        /// 用于那些不能通过命名约定自动映射的命令
        /// </summary>
        private void AddSpecialMappings()
        {
            // 这里可以添加需要覆盖默认映射的特殊命令
            // 例如：AddExactMatchRule("0x0003", CommandCategory.Message); // 特定命令的特殊处理
        }

        /// <summary>
        /// 当反射映射失败时的回退机制
        /// 基于命令码范围建立基本映射
        /// </summary>
        private void InitializeFallbackMappings()
        {
            // 为关键命令范围添加基本映射
            // 这些映射是基于CommandCategories中定义的命令范围
            for (ushort code = 0x0100; code <= 0x01FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.Authentication);
            }

            for (ushort code = 0x0200; code <= 0x02FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.Cache);
            }

            for (ushort code = 0x0300; code <= 0x03FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.Message);
            }

            for (ushort code = 0x0400; code <= 0x04FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.Workflow);
            }

            for (ushort code = 0x0600; code <= 0x06FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.File);
            }

            for (ushort code = 0x0700; code <= 0x07FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.DataSync);
            }

            for (ushort code = 0x0800; code <= 0x08FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.Lock);
            }

            for (ushort code = 0x0900; code <= 0x09FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.SystemManagement);
            }

            // 系统命令默认为System类别
            for (ushort code = 0x0000; code <= 0x00FF; code++)
            {
                AddExactMatchRule($"0x{code:X4}", CommandCategory.System);
            }
        }

        /// <summary>
        /// 获取命令分类
        /// </summary>
        /// <param name="packet">数据包对象</param>
        /// <returns>命令分类</returns>
        public CommandCategory GetCommandCategory(PacketModel packet)
        {
            if (packet == null || packet.CommandId == null)
                return CommandCategory.System; // 默认使用System类别

            string commandKey = GetCommandKey(packet.CommandId);

            // 首先检查缓存
            if (_commandCategoryCache.TryGetValue(commandKey, out var cachedCategory))
                return cachedCategory;

            // 确定命令分类
            var category = DetermineCommandCategory(packet.CommandId);

            // 缓存结果
            _commandCategoryCache[commandKey] = category;

            return category;
        }

        /// <summary>
        /// 根据CommandId获取命令分类
        /// 优先使用CommandCategory枚举进行分类
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令分类</returns>
        public CommandCategory GetCircuitBreakerCategory(Commands.CommandId commandId)
        {
            if (commandId == null)
                return CommandCategory.System; // 默认使用System类别

            string commandKey = GetCommandKey(commandId);

            // 首先检查缓存
            if (_commandCategoryCache.TryGetValue(commandKey, out var cachedCategory))
                return cachedCategory;

            // 确定命令分类
            var category = DetermineCommandCategory(commandId);

            // 缓存结果
            _commandCategoryCache[commandKey] = category;

            return category;
        }

        /// <summary>
        /// 获取命令键（用于缓存）
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令键</returns>
        private string GetCommandKey(CommandId commandId)
        {
            return $"{commandId.FullCode}:{commandId.Name}";
        }

        /// <summary>
        /// 确定命令分类
        /// 智能分类逻辑，基于CommandId的多种特性进行综合判断
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令分类</returns>
        private CommandCategory DetermineCommandCategory(Commands.CommandId commandId)
        {
            // 1. 首先使用CommandCategory进行分类（最基础的分类方式）
            if (_categoryMappings.TryGetValue(commandId.Category, out var categoryByEnum))
                return categoryByEnum;

            string fullCode = commandId.FullCode.ToString();
            string name = commandId.Name ?? string.Empty;

            // 2. 精确匹配规则（最高优先级的自定义规则）
            if (_exactMatchRules.TryGetValue(fullCode, out var exactCategory))
                return exactCategory;

            if (_exactMatchRules.TryGetValue(name, out exactCategory))
                return exactCategory;

            // 3. 智能命令名称模式识别
            if (IsReadOnlyCommandPattern(name))
                return CommandCategory.Message; // 只读命令映射到Message类别

            if (IsCriticalCommandPattern(name))
                return CommandCategory.Authentication; // 关键命令映射到Authentication类别

            if (IsBatchCommandPattern(name))
                return CommandCategory.DataSync; // 批处理命令映射到DataSync类别

            if (IsExternalCommandPattern(name))
                return CommandCategory.Workflow; // 外部命令映射到Workflow类别

            // 4. 前缀匹配规则
            foreach (var rule in _prefixRules)
            {
                if (name.StartsWith(rule.Key, StringComparison.OrdinalIgnoreCase))
                    return rule.Value;
            }

            // 5. 关键词匹配规则
            foreach (var rule in _keywordRules)
            {
                if (name.IndexOf(rule.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                    return rule.Value;
            }

            // 6. 基于CommandCatalog的特殊命令范围判断
            if (IsAuthenticationCommand(fullCode))
                return CommandCategory.Authentication;

            if (IsDataSyncCommand(fullCode))
                return CommandCategory.DataSync;

            // 7. 基于命令码范围的智能分类
            if (IsSystemManagementCommandRange(fullCode))
                return CommandCategory.SystemManagement;

            if (IsQueryCommandRange(fullCode))
                return CommandCategory.Message;


            // 9. 默认分类
            return CommandCategory.System;
        }

        /// <summary>
        /// 判断命令名称是否匹配只读命令模式
        /// </summary>
        /// <param name="name">命令名称</param>
        /// <returns>是否为只读命令</returns>
        private bool IsReadOnlyCommandPattern(string name)
        {
            // 更智能的只读命令模式识别
            string[] readOnlyPatterns = {
                "SELECT_", "FETCH_", "RETRIEVE_", "FIND_", "LOOKUP_", "VIEW_",
                "READ_", "QUERY_", "LIST_", "GET_", "INFO_", "STAT_", "REPORT_"
            };

            name = name.ToUpperInvariant();
            foreach (var pattern in readOnlyPatterns)
            {
                if (name.StartsWith(pattern))
                    return true;
            }

            // 检查命令名是否同时包含查询类动词和结果类名词
            bool hasQueryVerb = name.Contains("QUERY") || name.Contains("GET") || name.Contains("LIST");
            bool hasResultNoun = name.Contains("RESULT") || name.Contains("DATA") || name.Contains("INFO");

            return hasQueryVerb && hasResultNoun;
        }

        /// <summary>
        /// 判断命令名称是否匹配关键命令模式
        /// </summary>
        /// <param name="name">命令名称</param>
        /// <returns>是否为关键命令</returns>
        private bool IsCriticalCommandPattern(string name)
        {
            // 更智能的关键命令模式识别
            string[] criticalPatterns = {
                "AUTH_", "LOGIN_", "LOGOUT_", "VALIDATE_", "SECURITY_", "TRANSACTION_",
                "PAYMENT_", "BILLING_", "ORDER_", "TXN_", "PAY_", "CHECKOUT_"
            };

            name = name.ToUpperInvariant();
            foreach (var pattern in criticalPatterns)
            {
                if (name.StartsWith(pattern))
                    return true;
            }

            // 检查命令名是否同时包含操作动词和关键系统名词
            bool hasActionVerb = name.Contains("CREATE") || name.Contains("UPDATE") ||
                                name.Contains("DELETE") || name.Contains("PROCESS");
            bool hasCriticalNoun = name.Contains("USER") || name.Contains("ACCOUNT") ||
                                 name.Contains("AUTH") || name.Contains("SECURITY");

            return hasActionVerb && hasCriticalNoun;
        }

        /// <summary>
        /// 判断命令名称是否匹配批处理命令模式
        /// </summary>
        /// <param name="name">命令名称</param>
        /// <returns>是否为批处理命令</returns>
        private bool IsBatchCommandPattern(string name)
        {
            // 更智能的批处理命令模式识别
            string[] batchPatterns = {
                "BATCH_", "BULK_", "IMPORT_", "EXPORT_", "SYNC_", "BACKUP_",
                "RESTORE_", "PROCESS_", "MIGRATE_", "REPORT_"
            };

            name = name.ToUpperInvariant();
            foreach (var pattern in batchPatterns)
            {
                if (name.StartsWith(pattern))
                    return true;
            }

            // 检查命令名是否包含批量相关关键词
            string[] batchKeywords = { "BATCH", "BULK", "MULTIPLE", "ALL", "EVERY" };
            foreach (var keyword in batchKeywords)
            {
                if (name.Contains(keyword))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判断命令名称是否匹配外部命令模式
        /// </summary>
        /// <param name="name">命令名称</param>
        /// <returns>是否为外部命令</returns>
        private bool IsExternalCommandPattern(string name)
        {
            // 更智能的外部命令模式识别
            string[] externalPatterns = {
                "EXT_", "API_", "EXTERNAL_", "INTEGRATE_", "CONNECT_", "REMOTE_",
                "GATEWAY_", "PROXY_", "BRIDGE_", "PARTNER_"
            };

            name = name.ToUpperInvariant();
            foreach (var pattern in externalPatterns)
            {
                if (name.StartsWith(pattern))
                    return true;
            }

            // 检查命令名是否包含外部系统相关关键词
            string[] externalKeywords = { "API", "EXTERNAL", "INTEGRATION", "REMOTE" };
            foreach (var keyword in externalKeywords)
            {
                if (name.Contains(keyword))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判断是否为系统管理命令范围
        /// </summary>
        /// <param name="fullCode">完整命令码</param>
        /// <returns>是否为系统管理命令</returns>
        private bool IsSystemManagementCommandRange(string fullCode)
        {
            ushort code;
            if (ushort.TryParse(fullCode, out code))
            {
                return (code >= 0x0900 && code <= 0x09FF); // 系统管理命令范围
            }
            return false;
        }

        /// <summary>
        /// 判断是否为查询命令范围
        /// </summary>
        /// <param name="fullCode">完整命令码</param>
        /// <returns>是否为查询命令</returns>
        private bool IsQueryCommandRange(string fullCode)
        {
            ushort code;
            if (ushort.TryParse(fullCode, out code))
            {
                // 检查是否在特定的查询命令范围内
                // 这里只是示例，实际需要根据项目的命令码定义调整
                return (code >= 0x0A00 && code <= 0x0AFF);
            }
            return false;
        }

        /// <summary>
        /// 判断是否为认证命令
        /// </summary>
        /// <param name="fullCode">完整命令码</param>
        /// <returns>是否为认证命令</returns>
        private bool IsAuthenticationCommand(string fullCode)
        {
            // 从CommandCatalog中判断是否为认证相关命令
            ushort code;
            if (ushort.TryParse(fullCode, out code))
            {
                return (code >= 0x0100 && code <= 0x01FF); // 认证命令范围
            }
            return false;
        }

        /// <summary>
        /// 判断是否为数据同步命令
        /// </summary>
        /// <param name="fullCode">完整命令码</param>
        /// <returns>是否为数据同步命令</returns>
        private bool IsDataSyncCommand(string fullCode)
        {
            // 从CommandCatalog中判断是否为数据同步相关命令
            ushort code;
            if (ushort.TryParse(fullCode, out code))
            {
                return (code >= 0x0700 && code <= 0x07FF); // 数据同步命令范围
            }
            return false;
        }

        /// <summary>
        /// 添加精确匹配规则
        /// </summary>
        /// <param name="commandIdentifier">命令标识符（代码或名称）</param>
        /// <param name="category">熔断器分类</param>
        public void AddExactMatchRule(string commandIdentifier, CommandCategory category)
        {
            _exactMatchRules[commandIdentifier] = category;
            // 清除缓存，以便下次重新计算
            ClearCache();
        }

        /// <summary>
        /// 添加前缀匹配规则
        /// </summary>
        /// <param name="prefix">命令前缀</param>
        /// <param name="category">熔断器分类</param>
        public void AddPrefixRule(string prefix, CommandCategory category)
        {
            _prefixRules[prefix] = category;
            // 清除缓存，以便下次重新计算
            ClearCache();
        }

        /// <summary>
        /// 添加关键词匹配规则
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="category">熔断器分类</param>
        public void AddKeywordRule(string keyword, CommandCategory category)
        {
            _keywordRules[keyword] = category;
            // 清除缓存，以便下次重新计算
            ClearCache();
        }

        /// <summary>
        /// 清除分类缓存
        /// </summary>
        public void ClearCache()
        {
            _commandCategoryCache.Clear();
        }
    }

    /// <summary>
    /// 差异化熔断器策略管理器
    /// 为不同类型的命令提供不同的熔断器策略配置
    /// </summary>
    public class CircuitBreakerPolicyManager
    {
        // 命令类型到策略的映射
        private readonly Dictionary<CommandCategory, IAsyncPolicy<IResponse>> _categoryPolicies;

        // 命令类型分类器
        private readonly CommandTypeClassifier _classifier;

        // 策略配置
        private readonly CircuitBreakerPolicyConfig _config;

        /// <summary>
        /// 获取命令类型分类器
        /// </summary>
        public CommandTypeClassifier Classifier => _classifier;

        /// <summary>
        /// 获取策略配置
        /// </summary>
        public CircuitBreakerPolicyConfig Config => _config;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">策略配置</param>
        public CircuitBreakerPolicyManager(CircuitBreakerPolicyConfig config = null)
        {
            _config = config ?? new CircuitBreakerPolicyConfig();
            _classifier = new CommandTypeClassifier();
            _categoryPolicies = new Dictionary<CommandCategory, IAsyncPolicy<IResponse>>();

            // 初始化所有类型的策略
            InitializePolicies();
        }

        /// <summary>
        /// 初始化所有类型的熔断器策略
        /// </summary>
        private void InitializePolicies()
        {
            // 关键命令策略 - 只处理异常和空响应
            _categoryPolicies[CommandCategory.Authentication] = Policy
                .Handle<Exception>()
                .OrResult<IResponse>(r => r == null)
                .CircuitBreakerAsync<IResponse>(
                    handledEventsAllowedBeforeBreaking: _config.CriticalFailureThreshold,
                    durationOfBreak: _config.CriticalRecoveryTime,
                    onBreak: OnCircuitBreak,
                    onReset: OnCircuitReset,
                    onHalfOpen: OnCircuitHalfOpen);

            // 标准命令策略 - 只处理异常和空响应
            _categoryPolicies[CommandCategory.System] = Policy
                .Handle<Exception>()
                .OrResult<IResponse>(r => r == null)
                .CircuitBreakerAsync<IResponse>(
                    handledEventsAllowedBeforeBreaking: _config.StandardFailureThreshold,
                    durationOfBreak: _config.StandardRecoveryTime,
                    onBreak: OnCircuitBreak,
                    onReset: OnCircuitReset,
                    onHalfOpen: OnCircuitHalfOpen);

            // 批处理命令策略 - 只处理异常和空响应
            _categoryPolicies[CommandCategory.DataSync] = Policy
                .Handle<Exception>()
                .OrResult<IResponse>(r => r == null)
                .CircuitBreakerAsync<IResponse>(
                    handledEventsAllowedBeforeBreaking: _config.BatchFailureThreshold,
                    durationOfBreak: _config.BatchRecoveryTime,
                    onBreak: OnCircuitBreak,
                    onReset: OnCircuitReset,
                    onHalfOpen: OnCircuitHalfOpen);

            // 只读命令策略 - 只处理异常和空响应
            _categoryPolicies[CommandCategory.Message] = Policy
                .Handle<Exception>()
                .OrResult<IResponse>(r => r == null)
                .CircuitBreakerAsync<IResponse>(
                    handledEventsAllowedBeforeBreaking: _config.ReadOnlyFailureThreshold,
                    durationOfBreak: _config.ReadOnlyRecoveryTime,
                    onBreak: OnCircuitBreak,
                    onReset: OnCircuitReset,
                    onHalfOpen: OnCircuitHalfOpen);

            // 外部服务命令策略 - 只处理异常和空响应
            _categoryPolicies[CommandCategory.Workflow] = Policy
                .Handle<Exception>()
                .OrResult<IResponse>(r => r == null)
                .CircuitBreakerAsync<IResponse>(
                    handledEventsAllowedBeforeBreaking: _config.ExternalFailureThreshold,
                    durationOfBreak: _config.ExternalRecoveryTime,
                    onBreak: OnCircuitBreak,
                    onReset: OnCircuitReset,
                    onHalfOpen: OnCircuitHalfOpen);
        }

        /// <summary>
        /// 获取命令对应的熔断器策略
        /// 命令分类机制 ：系统通过 CommandTypeClassifier 将命令按功能特性分类为不同的 CommandCategory （如Authentication、System、DataSync、Message、Workflow等）。
        //- 差异化策略管理 ： CircuitBreakerPolicyManager 为每种命令类别维护独立的熔断器策略，不同类别可以有不同的失败阈值和恢复时间配置。
        //- 策略应用流程 ：在处理命令时，系统会：

        //- 首先使用 _circuitBreakerPolicyManager.GetPolicyForCommand(cmd.Packet) 获取适合该命令类别的熔断器策略
        //- 只有当没有找到特定类别策略时，才会回退使用 _defaultCircuitBreakerPolicy
        //- 命令分类逻辑 ：分类器通过多种方式智能判断命令类别，包括命令ID枚举、精确匹配规则、命名模式识别、前缀匹配和关键词匹配等。
        /// </summary>
        /// <param name="packet">数据包对象</param>
        /// <returns>熔断器策略</returns>
        public IAsyncPolicy<IResponse> GetPolicyForCommand(PacketModel packet)
        {
            var category = _classifier.GetCommandCategory(packet);

            if (_categoryPolicies.TryGetValue(category, out var policy))
                return policy;

            // 默认返回标准命令策略
            return _categoryPolicies[CommandCategory.System];
        }

        /// <summary>
        /// 熔断器打开回调
        /// </summary>
        /// <param name="result">委托执行结果</param>
        /// <param name="breakDuration">熔断持续时间</param>
        private void OnCircuitBreak(Polly.DelegateResult<IResponse> result, TimeSpan breakDuration)
        {
            // 这里可以添加日志记录
            string message = result?.Exception?.Message ?? "unknown reason";
            // 使用Console.WriteLine作为临时日志记录方式
            Console.WriteLine($"熔断器已打开，持续时间: {breakDuration.TotalSeconds}秒，原因: {message}");
        }

        /// <summary>
        /// 熔断器重置回调
        /// </summary>
        private void OnCircuitReset()
        {
            // 这里可以添加日志记录
        }

        /// <summary>
        /// 熔断器半开回调
        /// </summary>
        private void OnCircuitHalfOpen()
        {
            // 这里可以添加日志记录
        }
    }

    /// <summary>
    /// 熔断器策略配置
    /// </summary>
    public class CircuitBreakerPolicyConfig
    {
        // 关键命令配置
        public int CriticalFailureThreshold { get; set; } = 3;  // 失败阈值较低，因为关键命令需要更谨慎
        public TimeSpan CriticalRecoveryTime { get; set; } = TimeSpan.FromSeconds(30);  // 恢复时间较短，关键命令需要快速恢复

        // 标准命令配置
        public int StandardFailureThreshold { get; set; } = 10;  // 默认的10次失败阈值
        public TimeSpan StandardRecoveryTime { get; set; } = TimeSpan.FromMinutes(1);  // 默认的1分钟恢复时间

        // 批处理命令配置
        public int BatchFailureThreshold { get; set; } = 5;  // 批处理命令可能更容易失败，但不应轻易熔断
        public TimeSpan BatchRecoveryTime { get; set; } = TimeSpan.FromMinutes(2);  // 批处理命令恢复时间较长

        // 只读命令配置
        public int ReadOnlyFailureThreshold { get; set; } = 20;  // 只读命令可以容忍更多失败
        public TimeSpan ReadOnlyRecoveryTime { get; set; } = TimeSpan.FromMinutes(30);  // 只读命令恢复时间较长

        // 外部服务命令配置
        public int ExternalFailureThreshold { get; set; } = 5;  // 外部服务可能不稳定，失败阈值较低
        public TimeSpan ExternalRecoveryTime { get; set; } = TimeSpan.FromMinutes(5);  // 外部服务恢复时间较长

        /// <summary>
        /// 设置关键命令配置
        /// </summary>
        /// <param name="failureThreshold">失败阈值</param>
        /// <param name="recoveryTime">恢复时间</param>
        public CircuitBreakerPolicyConfig WithCriticalConfig(int failureThreshold, TimeSpan recoveryTime)
        {
            CriticalFailureThreshold = failureThreshold;
            CriticalRecoveryTime = recoveryTime;
            return this;
        }

        /// <summary>
        /// 设置标准命令配置
        /// </summary>
        /// <param name="failureThreshold">失败阈值</param>
        /// <param name="recoveryTime">恢复时间</param>
        public CircuitBreakerPolicyConfig WithStandardConfig(int failureThreshold, TimeSpan recoveryTime)
        {
            StandardFailureThreshold = failureThreshold;
            StandardRecoveryTime = recoveryTime;
            return this;
        }

        /// <summary>
        /// 设置批处理命令配置
        /// </summary>
        /// <param name="failureThreshold">失败阈值</param>
        /// <param name="recoveryTime">恢复时间</param>
        public CircuitBreakerPolicyConfig WithBatchConfig(int failureThreshold, TimeSpan recoveryTime)
        {
            BatchFailureThreshold = failureThreshold;
            BatchRecoveryTime = recoveryTime;
            return this;
        }

        /// <summary>
        /// 设置只读命令配置
        /// </summary>
        /// <param name="failureThreshold">失败阈值</param>
        /// <param name="recoveryTime">恢复时间</param>
        public CircuitBreakerPolicyConfig WithReadOnlyConfig(int failureThreshold, TimeSpan recoveryTime)
        {
            ReadOnlyFailureThreshold = failureThreshold;
            ReadOnlyRecoveryTime = recoveryTime;
            return this;
        }

        /// <summary>
        /// 设置外部服务命令配置
        /// </summary>
        /// <param name="failureThreshold">失败阈值</param>
        /// <param name="recoveryTime">恢复时间</param>
        public CircuitBreakerPolicyConfig WithExternalConfig(int failureThreshold, TimeSpan recoveryTime)
        {
            ExternalFailureThreshold = failureThreshold;
            ExternalRecoveryTime = recoveryTime;
            return this;
        }
    }
}
