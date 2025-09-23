using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 优化后的登录命令实现示例
    /// 直接使用PacketSpec中的BaseCommand基类
    /// </summary>
    [Command("Login", CommandCategory.Authentication, Description = "用户登录命令")]
    public class OptimizedLoginCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// 使用认证命令类别和登录操作码
        /// </summary>
        public override CommandId CommandIdentifier => AuthenticationCommands.Login;

        /// <summary>
        /// 登录请求数据
        /// </summary>
        public LoginRequest LoginRequest { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OptimizedLoginCommand()
        {
            Priority = CommandPriority.High; // 登录命令优先级高
            TimeoutMs = 30000; // 登录超时时间30秒
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientInfo">客户端信息</param>
        public OptimizedLoginCommand(string username, string password, string clientInfo = null)
        {
            LoginRequest = LoginRequest.Create(username, password, clientInfo);
            Priority = CommandPriority.High; // 登录命令优先级高
            TimeoutMs = 30000; // 登录超时时间30秒
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的登录数据</returns>
        public override object GetSerializableData()
        {
            return LoginRequest;
        }

        /// <summary>
        /// 命令执行的具体逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        protected override async Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 客户端登录命令通常只构建数据，实际的发送由通信服务处理
                // 这里可以添加一些自定义逻辑，例如验证输入数据等
                
                // 验证登录请求数据
                if (LoginRequest == null)
                {
                    return CommandResult.Failure("登录请求数据不能为空", "EMPTY_LOGIN_REQUEST");
                }

                if (!LoginRequest.IsValid())
                {
                    return CommandResult.Failure("登录请求数据无效", "INVALID_LOGIN_REQUEST");
                }

                // 构建登录数据
                var loginData = GetSerializableData();
                
                // 返回成功结果，实际的网络请求由通信服务处理
                return CommandResult.Success(loginData, "登录命令构建成功");
            }
            catch (Exception ex)
            {
                return CommandResult.Failure($"登录命令执行异常: {ex.Message}", "LOGIN_EXCEPTION", ex);
            }
        }

        /// <summary>
        /// 验证命令数据
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            // 调用基类验证
            var baseResult = base.Validate();
            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            // 验证登录请求数据
            if (LoginRequest == null)
            {
                return CommandValidationResult.Failure("登录请求数据不能为空", "EMPTY_LOGIN_REQUEST");
            }

            if (!LoginRequest.IsValid())
            {
                return CommandValidationResult.Failure("登录请求数据无效", "INVALID_LOGIN_REQUEST");
            }

            return CommandValidationResult.Success();
        }
    }

    /// <summary>
    /// 优化后的客户端通信服务使用示例
    /// </summary>
    public class OptimizedClientCommunicationServiceExample
    {
        private readonly IClientCommunicationService _communicationService;

        public OptimizedClientCommunicationServiceExample(
            IClientCommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        /// <summary>
        /// 执行登录操作示例
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录响应</returns>
        public async Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password)
        {
            // 创建登录命令
            var loginCommand = new OptimizedLoginCommand(username, password);
            
            // 验证命令
            var validationResult = loginCommand.Validate();
            if (!validationResult.IsValid)
            {
                return ApiResponse<LoginResponse>.Failure(validationResult.ErrorMessage, 400);
            }

            // 发送命令并等待响应
            var response = await _communicationService.SendCommandAsync<LoginResponse>(
                loginCommand, 
                CancellationToken.None);

            return response;
        }
    }

    /// <summary>
    /// 优化后的命令调度器使用示例
    /// 直接使用客户端命令调度器
    /// </summary>
    public class OptimizedCommandDispatcherExample
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ClientCommandDispatcher _commandDispatcher;

        public OptimizedCommandDispatcherExample(
            IClientCommunicationService communicationService,
            ClientCommandDispatcher commandDispatcher)
        {
            _communicationService = communicationService;
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// 执行登录操作示例
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录响应</returns>
        public async Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password)
        {
            // 使用命令调度器创建命令
            var loginCommand = _commandDispatcher.CreateCommand(0x0100, username, password) as OptimizedLoginCommand;

            // 发送命令并等待响应
            var response = await _communicationService.SendCommandAsync<LoginResponse>(
                loginCommand, 
                CancellationToken.None);

            return response;
        }
    }
}