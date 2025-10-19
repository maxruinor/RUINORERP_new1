using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 配置式业务处理器 - 通过配置定义业务逻辑
    /// 支持运行时配置业务规则，无需重新编译代码
    /// </summary>
    public class ConfigurableBusinessProcessor : BaseCommandHandler
    {
        /// <summary>
        /// 业务配置
        /// </summary>
        private readonly BusinessConfig _config;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<ConfigurableBusinessProcessor> _logger;

        /// <summary>
        /// 规则引擎
        /// </summary>
        private readonly RuleEngine _ruleEngine;

        /// <summary>
        /// 数据访问器
        /// </summary>
        private readonly IDataAccessor _dataAccessor;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigurableBusinessProcessor(BusinessConfig config, ILogger<ConfigurableBusinessProcessor> logger, 
            RuleEngine ruleEngine = null, IDataAccessor dataAccessor = null)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ruleEngine = ruleEngine ?? new RuleEngine();
            _dataAccessor = dataAccessor ?? new InMemoryDataAccessor();
        }

        /// <summary>
        /// 核心处理方法 - 根据配置执行业务逻辑（重构版：提取配置处理流程）
        /// </summary>
        /// <param name="cmd">队列命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        protected override async Task<BaseCommand<IRequest, IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd?.Command == null)
            {
                _logger.LogError("命令或命令数据为空");
                return BaseCommand<IRequest, IResponse>.CreateError("命令数据无效", 400);
            }

            try
            {
                var request = cmd.Command;
                var requestType = request.GetType();
                
                // 获取业务配置
                var config = await GetBusinessConfigurationAsync(requestType);
                if (config == null)
                {
                    return BaseCommand<IRequest, IResponse>.CreateError($"未找到业务配置: {requestType.Name}", 404);
                }

                // 执行业务处理流程
                return await ExecuteBusinessProcessAsync(request, config, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("命令处理被取消");
                return BaseCommand<IRequest, IResponse>.CreateError("操作被取消", 499);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "可配置业务处理时发生异常");
                return BaseCommand<IRequest, IResponse>.CreateError($"处理失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 获取业务配置 - 提取为独立方法
        /// </summary>
        private async Task<BusinessConfigInfo> GetBusinessConfigurationAsync(Type requestType)
        {
            var requestTypeName = requestType.Name;
            var config = _config.GetBusinessConfig(requestTypeName);
            if (config == null)
            {
                _logger.LogWarning($"未找到业务配置: {requestTypeName}");
            }
            return config;
        }

        /// <summary>
        /// 执行业务处理流程 - 提取为独立方法
        /// </summary>
        private async Task<BaseCommand<IRequest, IResponse>> ExecuteBusinessProcessAsync(object request, BusinessConfigInfo config, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"执行配置式业务处理：{request.GetType().Name}");

            // 1. 执行验证规则
            var validationResult = await ExecuteValidationRules(request, config);
            if (!validationResult.IsValid)
            {
                return BaseCommand<IRequest, IResponse>.CreateError($"验证失败: {string.Join(", ", validationResult.Errors)}");
            }

            // 2. 执行前置处理
            await ExecutePreProcessing(request, config.PreProcessingRules);

            // 3. 执行主要业务逻辑
            var result = await ExecuteBusinessLogic(request, config, cancellationToken);

            // 4. 执行后置处理
            await ExecutePostProcessing(request, result, config.PostProcessingRules);

            return result;
        }

        /// <summary>
        /// 执行验证规则
        /// </summary>
        private async Task<ValidationResult> ExecuteValidationRules(object request, BusinessConfigInfo config)
        {
            var errors = new List<ValidationFailure>();

            if (config.ValidationRules?.Any() != true)
            {
                return new ValidationResult(); // 无验证规则时返回成功
            }

            foreach (var rule in config.ValidationRules)
            {
                try
                {
                    // 执行验证规则
                    var ruleResult =await _ruleEngine.ExecuteValidationRule(request, rule);
                    if (!ruleResult.IsValid)
                    {
                        errors.Add(new ValidationFailure(rule.RuleName, rule.ErrorMessage));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"验证规则执行失败: {rule.RuleName}");
                    errors.Add(new ValidationFailure(rule.RuleName, $"验证规则执行失败: {ex.Message}"));
                }
            }

            return errors.Any() ? new ValidationResult(errors) : new ValidationResult();
        }
        

        /// <summary>
        /// 执行前置处理
        /// </summary>
        private async Task ExecutePreProcessing(object request, List<ProcessingRule> rules)
        {
            foreach (var rule in rules)
            {
                try
                {
                    await _ruleEngine.ExecuteProcessingRule(request, rule);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"前置处理规则执行失败: {rule.RuleName}");
                }
            }
        }

        /// <summary>
        /// 执行业务逻辑
        /// </summary>
        private async Task<BaseCommand<IRequest, IResponse>> ExecuteBusinessLogic(object request, BusinessConfigInfo config, CancellationToken cancellationToken)
        {
            return config.BusinessType switch
            {
                BusinessType.Create => await ExecuteCreate(request, config),
                BusinessType.Update => await ExecuteUpdate(request, config),
                BusinessType.Delete => await ExecuteDelete(request, config),
                BusinessType.Query => await ExecuteQuery(request, config, cancellationToken),
                BusinessType.Custom => await ExecuteCustomBusiness(request, config, cancellationToken),
                _ => BaseCommand<IRequest, IResponse>.CreateError($"不支持的業務类型: {config.BusinessType}")
            };
        }

        /// <summary>
        /// 执行创建操作
        /// </summary>
        private async Task<BaseCommand<IRequest, IResponse>> ExecuteCreate(object request, BusinessConfigInfo config)
        {
            var data = ExtractData(request, config.DataMapping);
            var result = await _dataAccessor.CreateAsync(data, config.EntityType);
            var response = new ResponseBase
            {
                Message = result.ToString(),
                IsSuccess = true
            };
            return BaseCommand<IRequest, IResponse>.CreateSuccess(response);
        }

        /// <summary>
        /// 执行更新操作
        /// </summary>
        private async Task<BaseCommand<IRequest, IResponse>> ExecuteUpdate(object request, BusinessConfigInfo config)
        {
            var data = ExtractData(request, config.DataMapping);
            var result = await _dataAccessor.UpdateAsync(data, config.EntityType);
            var response = new ResponseBase
            {
                Message = result.ToString(),
                IsSuccess = true
            };
            return BaseCommand<IRequest, IResponse>.CreateSuccess(response);
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        private async Task<BaseCommand<IRequest, IResponse>> ExecuteDelete(object request, BusinessConfigInfo config)
        {
            var id = ExtractId(request, config.IdMapping);
            var result = await _dataAccessor.DeleteAsync(id, config.EntityType);
            var response = new ResponseBase
            {
                Message = result.ToString(),
                IsSuccess = true
            };
            return BaseCommand<IRequest, IResponse>.CreateSuccess(response);
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        private async Task<BaseCommand<IRequest, IResponse>> ExecuteQuery(object request, BusinessConfigInfo config, CancellationToken cancellationToken)
        {
            var queryParameters = ExtractQueryParameters(request, config.QueryMapping);
            var result = await _dataAccessor.QueryAsync(queryParameters, config.EntityType, cancellationToken);
            var response = new ResponseBase
            {
                Message = "查询成功",
                IsSuccess = true
            };
            response.WithMetadata("QueryResult", result);
            return BaseCommand<IRequest, IResponse>.CreateSuccess(response);
        }

        /// <summary>
        /// 执行自定义业务逻辑
        /// </summary>
        private async Task<BaseCommand<IRequest, IResponse>> ExecuteCustomBusiness(object request, BusinessConfigInfo config, CancellationToken cancellationToken)
        {
            // 这里可以集成脚本引擎或其他动态执行机制
            _logger.LogInformation($"执行自定义业务逻辑: {config.CustomLogic}");
            
            // 示例：简单的表达式执行
            if (!string.IsNullOrEmpty(config.CustomLogic))
            {
                // 实际项目中可以集成更强大的脚本引擎
                return BaseCommand<IRequest, IResponse>.CreateSuccess(new ResponseBase(), "自定义业务执行成功");
            }

            return BaseCommand<IRequest, IResponse>.CreateError("自定义业务逻辑未配置");
        }

        /// <summary>
        /// 执行后置处理
        /// </summary>
        private async Task ExecutePostProcessing(object request, BaseCommand<IRequest, IResponse> result, List<ProcessingRule> rules)
        {
            foreach (var rule in rules)
            {
                try
                {
                    await _ruleEngine.ExecuteProcessingRule(new { Request = request, Result = result }, rule);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"后置处理规则执行失败: {rule.RuleName}");
                }
            }
        }

        /// <summary>
        /// 提取数据
        /// </summary>
        private Dictionary<string, object> ExtractData(object request, Dictionary<string, string> dataMapping)
        {
            var data = new Dictionary<string, object>();
            var requestType = request.GetType();

            foreach (var mapping in dataMapping)
            {
                var property = requestType.GetProperty(mapping.Key);
                if (property != null)
                {
                    var value = property.GetValue(request);
                    data[mapping.Value] = value;
                }
            }

            return data;
        }

        /// <summary>
        /// 提取ID
        /// </summary>
        private string ExtractId(object request, string idProperty)
        {
            var requestType = request.GetType();
            var property = requestType.GetProperty(idProperty);
            return property?.GetValue(request)?.ToString();
        }

        /// <summary>
        /// 提取查询参数
        /// </summary>
        private Dictionary<string, object> ExtractQueryParameters(object request, Dictionary<string, string> queryMapping)
        {
            var parameters = new Dictionary<string, object>();
            var requestType = request.GetType();

            foreach (var mapping in queryMapping)
            {
                var property = requestType.GetProperty(mapping.Key);
                if (property != null)
                {
                    var value = property.GetValue(request);
                    if (value != null)
                    {
                        parameters[mapping.Value] = value;
                    }
                }
            }

            return parameters;
        }
    }

    /// <summary>
    /// 业务配置
    /// </summary>
    public class BusinessConfig
    {
        /// <summary>
        /// 业务配置字典
        /// </summary>
        private readonly Dictionary<string, BusinessConfigInfo> _businessConfigs;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessConfig(Dictionary<string, BusinessConfigInfo> configs = null)
        {
            _businessConfigs = configs ?? new Dictionary<string, BusinessConfigInfo>();
        }

        /// <summary>
        /// 获取业务配置
        /// </summary>
        public BusinessConfigInfo GetBusinessConfig(string requestType)
        {
            return _businessConfigs.TryGetValue(requestType, out var config) ? config : null;
        }

        /// <summary>
        /// 添加业务配置
        /// </summary>
        public void AddBusinessConfig(string requestType, BusinessConfigInfo config)
        {
            _businessConfigs[requestType] = config;
        }
    }

    /// <summary>
    /// 业务配置信息
    /// </summary>
    public class BusinessConfigInfo
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 验证规则
        /// </summary>
        public List<ValidationRule> ValidationRules { get; set; } = new List<ValidationRule>();

        /// <summary>
        /// 前置处理规则
        /// </summary>
        public List<ProcessingRule> PreProcessingRules { get; set; } = new List<ProcessingRule>();

        /// <summary>
        /// 后置处理规则
        /// </summary>
        public List<ProcessingRule> PostProcessingRules { get; set; } = new List<ProcessingRule>();

        /// <summary>
        /// 数据映射
        /// </summary>
        public Dictionary<string, string> DataMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// ID映射
        /// </summary>
        public string IdMapping { get; set; }

        /// <summary>
        /// 查询映射
        /// </summary>
        public Dictionary<string, string> QueryMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 自定义逻辑
        /// </summary>
        public string CustomLogic { get; set; }
    }

    /// <summary>
    /// 业务类型枚举
    /// </summary>
    public enum BusinessType
    {
        Create,
        Update,
        Delete,
        Query,
        Custom
    }

    /// <summary>
    /// 验证规则
    /// </summary>
    public class ValidationRule
    {
        public string RuleName { get; set; }
        public string Expression { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 处理规则
    /// </summary>
    public class ProcessingRule
    {
        public string RuleName { get; set; }
        public string Expression { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 规则引擎
    /// </summary>
    public class RuleEngine
    {
        public Task<ValidationResult> ExecuteValidationRule(object data, ValidationRule rule)
        {
            // 实际项目中可以集成更强大的规则引擎
            return Task.FromResult(new ValidationResult());
        }

        public Task ExecuteProcessingRule(object data, ProcessingRule rule)
        {
            // 实际项目中可以集成更强大的规则引擎
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 数据访问器接口
    /// </summary>
    public interface IDataAccessor
    {
        Task<object> CreateAsync(Dictionary<string, object> data, Type entityType);
        Task<object> UpdateAsync(Dictionary<string, object> data, Type entityType);
        Task<bool> DeleteAsync(string id, Type entityType);
        Task<object> QueryAsync(Dictionary<string, object> parameters, Type entityType, CancellationToken cancellationToken);
    }

    /// <summary>
    /// 内存数据访问器（示例实现）
    /// </summary>
    public class InMemoryDataAccessor : IDataAccessor
    {
        private readonly Dictionary<string, object> _dataStore = new Dictionary<string, object>();

        public Task<object> CreateAsync(Dictionary<string, object> data, Type entityType)
        {
            var id = Guid.NewGuid().ToString();
            _dataStore[id] = data;
            return Task.FromResult<object>(data);
        }

        public Task<object> UpdateAsync(Dictionary<string, object> data, Type entityType)
        {
            return Task.FromResult<object>(data);
        }

        public Task<bool> DeleteAsync(string id, Type entityType)
        {
            return Task.FromResult(_dataStore.Remove(id));
        }

        public Task<object> QueryAsync(Dictionary<string, object> parameters, Type entityType, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(_dataStore.Values.ToList());
        }
    }
}