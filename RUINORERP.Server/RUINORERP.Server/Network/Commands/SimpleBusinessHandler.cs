using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;
using System.Linq;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 简单业务处理器 - 演示简单业务场景的使用
    /// 适用于简单的字符串转换、数据验证等基础业务逻辑
    /// </summary>
    public class SimpleBusinessHandler : BaseCommandHandler
    {
        private readonly ILogger<SimpleBusinessHandler> _logger;

        public SimpleBusinessHandler(ILogger<SimpleBusinessHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 实现抽象方法 - 处理队列命令
        /// </summary>
        protected override async Task<ResponseBase> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"开始处理简单业务请求: {cmd?.Command?.GetType().Name}");

                // 获取请求对象
                var request = cmd?.Command;
                if (request == null)
                {
                    return ResponseBase.CreateError("请求对象为空");
                }

                // 根据请求类型分发处理
                return request switch
                {
                    SimpleRequest simpleRequest => await HandleSimpleOperation(simpleRequest),
                    BooleanRequest booleanRequest => await HandleBooleanOperation(booleanRequest),
                    NumericRequest numericRequest => await HandleNumericOperation(numericRequest),
                    _ => ResponseBase.CreateError($"不支持的请求类型: {request.GetType().Name}")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理简单业务请求时发生异常");
                return ResponseBase.CreateError($"处理请求失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理简单字符串操作
        /// </summary>
        private async Task<ResponseBase> HandleSimpleOperation(SimpleRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("执行简单字符串操作");

                // 使用FluentValidation验证请求
                var validator = new RequestBaseValidator();
                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessage = validationResult.GetValidationErrors();
                    var errorCode = validationResult.GetValidationErrorCode();
                    _logger.LogWarning($"请求验证失败: {errorMessage}");
                    return ResponseBase.CreateError(errorMessage, errorCode);
                }

                // 获取字符串值并转换为大写
                var inputValue = request.GetStringValue();
                if (string.IsNullOrEmpty(inputValue))
                {
                    return ResponseBase.CreateError("输入值不能为空", UnifiedErrorCodes.Biz_DataInvalid.Code);
                }

                var result = inputValue.ToUpper();
                _logger.LogInformation($"字符串转换完成：{inputValue} -> {result}");

                return SimpleResponse.CreateSuccessString(result, "处理成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "简单字符串操作处理失败");
                return ResponseBase.CreateError($"处理失败: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code);
            }
        }

        /// <summary>
        /// 处理简单布尔操作
        /// </summary>
        public async Task<ResponseBase> HandleBooleanOperation(BooleanRequest request)
        {
            try
            {
                _logger.LogInformation("执行简单布尔操作");

                // 使用FluentValidation验证请求
                var validator = new RequestBaseValidator();
                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessage = validationResult.GetValidationErrors();
                    var errorCode = validationResult.GetValidationErrorCode();
                    _logger.LogWarning($"请求验证失败: {errorMessage}");
                    return ResponseBase.CreateError(errorMessage, errorCode);
                }

                // 获取布尔值并取反
                var inputValue = request.GetBoolValue();
                var result = !inputValue;

                _logger.LogInformation($"布尔值转换完成：{inputValue} -> {result}");

                return SimpleResponse.CreateSuccessBool(result, "布尔操作成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "简单布尔操作处理失败");
                return ResponseBase.CreateError($"处理失败: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code);
            }
        }

        /// <summary>
        /// 处理简单数值操作
        /// </summary>
        public async Task<ResponseBase> HandleNumericOperation(NumericRequest request)
        {
            try
            {
                _logger.LogInformation("执行简单数值操作");

                if (request == null)
                {
                    return SimpleResponse.CreateFailure("请求对象不能为空");
                }

                // 获取整数值并加1
                var inputValue = request.GetIntValue();
                var result = inputValue + 1;

                _logger.LogInformation($"数值计算完成：{inputValue} + 1 = {result}");

                return SimpleResponse.CreateSuccessInt(result, "数值操作成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "简单数值操作处理失败");
                return SimpleResponse.CreateFailure($"处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理简单验证操作
        /// </summary>
        public async Task<ResponseBase> HandleValidationOperation(SimpleRequest request)
        {
            try
            {
                _logger.LogInformation("执行简单验证操作");

                if (request == null)
                {
                    return SimpleResponse.CreateFailure("请求对象不能为空");
                }

                // 验证字符串长度
                var inputValue = request.GetStringValue();
                if (string.IsNullOrEmpty(inputValue))
                {
                    return SimpleResponse.CreateFailure("输入值不能为空");
                }

                if (inputValue.Length < 3)
                {
                    return SimpleResponse.CreateFailure("输入值长度不能少于3个字符");
                }

                if (inputValue.Length > 20)
                {
                    return SimpleResponse.CreateFailure("输入值长度不能超过20个字符");
                }

                _logger.LogInformation($"验证通过：{inputValue}");

                return SimpleResponse.CreateSuccessString(inputValue, "验证成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "简单验证操作处理失败");
                return SimpleResponse.CreateFailure($"验证失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理时间戳操作
        /// </summary>
        public async Task<ResponseBase> HandleTimestampOperation(SimpleRequest request)
        {
            try
            {
                _logger.LogInformation("执行时间戳操作");

                if (request == null)
                {
                    return SimpleResponse.CreateFailure("请求对象不能为空");
                }

                // 获取当前时间戳
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var timestampString = timestamp.ToString();

                _logger.LogInformation($"生成时间戳：{timestampString}");

                return SimpleResponse.CreateSuccessString(timestampString, "时间戳生成成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "时间戳操作处理失败");
                return SimpleResponse.CreateFailure($"处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理简单格式化操作
        /// </summary>
        public async Task<ResponseBase> HandleFormattingOperation(SimpleRequest request)
        {
            try
            {
                _logger.LogInformation("执行简单格式化操作");

                if (request == null)
                {
                    return SimpleResponse.CreateFailure("请求对象不能为空");
                }

                // 获取字符串值并格式化
                var inputValue = request.GetStringValue();
                if (string.IsNullOrEmpty(inputValue))
                {
                    return SimpleResponse.CreateFailure("输入值不能为空");
                }

                // 移除空格并转换为大写
                var result = inputValue.Replace(" ", "").ToUpper();

                _logger.LogInformation($"格式化完成：'{inputValue}' -> '{result}'");

                return SimpleResponse.CreateSuccessString(result, "格式化成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "简单格式化操作处理失败");
                return SimpleResponse.CreateFailure($"处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理简单状态检查
        /// </summary>
        public async Task<ResponseBase> HandleStatusCheck(SimpleRequest request)
        {
            try
            {
                _logger.LogInformation("执行简单状态检查");

                if (request == null)
                {
                    return SimpleResponse.CreateFailure("请求对象不能为空");
                }

                // 检查系统状态
                var isHealthy = true; // 这里可以添加实际的健康检查逻辑
                var statusMessage = isHealthy ? "系统运行正常" : "系统状态异常";

                _logger.LogInformation($"状态检查结果：{statusMessage}");

                return SimpleResponse.CreateSuccessBool(isHealthy, statusMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "简单状态检查处理失败");
                return SimpleResponse.CreateFailure($"检查失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 用户CRUD处理器 - 演示中等复杂度业务场景
    /// 继承自CrudCommandHandler，实现用户相关的CRUD操作
    /// </summary>
    public class UserCrudHandler : CrudCommandHandler<User>
    {

        /// <summary>
        /// 模拟用户数据存储
        /// </summary>
        private readonly Dictionary<string, User> _userStore;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<UserCrudHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserCrudHandler(ILogger<UserCrudHandler> logger) : base(logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userStore = new Dictionary<string, User>();
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        protected override async Task<ResponseBase<User>> CreateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"创建用户：{user.Username}");

                // 验证用户数据
                if (string.IsNullOrEmpty(user.Username))
                {
                    return ResponseBase<User>.CreateError("用户名不能为空");
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    return ResponseBase<User>.CreateError("邮箱不能为空");
                }

                // 检查用户名是否已存在
                if (_userStore.Values.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    return ResponseBase<User>.CreateError("用户名已存在");
                }

                // 生成用户ID
                user.Id = Guid.NewGuid().ToString();
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                user.IsActive = true;

                // 保存用户
                _userStore[user.Id] = user;

                _logger.LogInformation($"用户创建成功：{user.Username} (ID: {user.Id})");

                return ResponseBase<User>.CreateSuccess(user, "用户创建成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建用户失败");
                return ResponseBase<User>.CreateError($"创建用户失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        protected override async Task<ResponseBase<User>> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"更新用户：{user.Id}");

                // 检查用户是否存在
                if (!_userStore.ContainsKey(user.Id))
                {
                    return ResponseBase<User>.CreateError("用户不存在");
                }

                var existingUser = _userStore[user.Id];

                // 验证用户数据
                if (string.IsNullOrEmpty(user.Username))
                {
                    return ResponseBase<User>.CreateError("用户名不能为空");
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    return ResponseBase<User>.CreateError("邮箱不能为空");
                }

                // 检查用户名是否与其他用户冲突
                if (_userStore.Values.Any(u => u.Id != user.Id && u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    return ResponseBase<User>.CreateError("用户名已存在");
                }

                // 更新用户信息
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.FullName = user.FullName;
                existingUser.Phone = user.Phone;
                existingUser.UpdatedAt = DateTime.UtcNow;

                _logger.LogInformation($"用户更新成功：{user.Username} (ID: {user.Id})");

                return ResponseBase<User>.CreateSuccess(existingUser, "用户更新成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用户失败");
                return ResponseBase<User>.CreateError($"更新用户失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        protected override async Task<ResponseBase<User>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"删除用户：{id}");

                // 检查用户是否存在
                if (!_userStore.ContainsKey(id))
                {
                    return ResponseBase<User>.CreateError("用户不存在");
                }

                var user = _userStore[id];

                // 删除用户
                _userStore.Remove(id);

                _logger.LogInformation($"用户删除成功：{user.Username} (ID: {id})");

                return ResponseBase<User>.CreateSuccess(user, "用户删除成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除用户失败");
                return ResponseBase<User>.CreateError($"删除用户失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取单个用户
        /// </summary>
        protected override async Task<ResponseBase<User>> GetAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"获取用户：{id}");

                // 检查用户是否存在
                if (!_userStore.ContainsKey(id))
                {
                    return ResponseBase<User>.CreateError("用户不存在");
                }

                var user = _userStore[id];
                return ResponseBase<User>.CreateSuccess(user, "用户查询成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户失败");
                return ResponseBase<User>.CreateError($"获取用户失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        protected override async Task<ResponseBase<User>> GetListAsync(Dictionary<string, object> queryParameters, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("获取用户列表");

                // 获取查询参数
                var searchTerm = queryParameters.ContainsKey("search") ? queryParameters["search"]?.ToString() : "";
                var isActive = queryParameters.ContainsKey("isActive") ? (bool?)queryParameters["isActive"] : null;

                // 查询用户
                var query = _userStore.Values.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(u =>
                        u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        u.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                }

                if (isActive.HasValue)
                {
                    query = query.Where(u => u.IsActive == isActive.Value);
                }

                var users = query.ToList();

                _logger.LogInformation($"用户列表查询完成，共 {users.Count} 条记录");

                // 返回用户列表的第一个用户，如果没有用户则返回null
                return ResponseBase<User>.CreateSuccess(users.FirstOrDefault(), $"查询到 {users.Count} 个用户");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户列表失败");
                return ResponseBase<User>.CreateError($"获取用户列表失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 用户实体类
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}