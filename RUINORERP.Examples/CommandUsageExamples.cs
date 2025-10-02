/// <summary>
/// 命令使用示例类
/// 本示例展示如何使用不同类型的命令对象与服务器进行通信
/// 包括强类型命令、泛型命令和基础命令的使用方式
/// 以及Token自动管理的演示
/// </summary>
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;

public class CommandUsageExamples
{
    private readonly IClientCommunicationService _commService;
    
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="commService">客户端通信服务实例，用于发送命令到服务器</param>
    public CommandUsageExamples(IClientCommunicationService commService)
    {
        _commService = commService;
    }
    
    /// <summary>
    /// 示例1：使用强类型命令（推荐）
    /// 强类型命令提供了最直观的API和完整的类型安全保障
    /// </summary>
    /// <returns>异步操作结果</returns>
    /// <exception cref="System.Exception">当网络通信失败或服务器返回错误时可能抛出异常</exception>
    public async Task Example1_StrongTypedCommand()
    {
        // 使用构建器创建强类型登录命令
        var loginCommand = CommandDataBuilder.BuildLoginCommand("admin", "password123");
        
        // 发送命令并等待响应
        var response = await _commService.SendCommandAsync<LoginRequest, LoginResponse>(loginCommand);
        
        // 处理响应结果
        if (response.IsSuccess)
        {
            Console.WriteLine($"登录成功: {response.Data.Username}");
        }
    }
    
    /// <summary>
    /// 示例2：使用泛型命令
    /// 泛型命令提供了更大的灵活性，可以自定义命令的各种属性
    /// </summary>
    /// <returns>异步操作结果</returns>
    /// <exception cref="System.Exception">当网络通信失败或服务器返回错误时可能抛出异常</exception>
    public async Task Example2_GenericCommand()
    {
        // 创建请求对象
        var request = new LoginRequest 
        {
            Username = "user", 
            Password = "pass"
        };
        
        // 创建泛型命令并配置额外属性
        var command = CommandDataBuilder.BuildGenericCommand(
            AuthenticationCommands.Login,
            request, 
            cmd => { 
                // 自定义超时时间
                cmd.TimeoutMs = 45000;
                // 设置命令优先级
                cmd.Priority = CommandPriority.High;
            });
            
        // 发送命令并等待响应
        var response = await _commService.SendCommandAsync<LoginRequest, LoginResponse>(command);
    }
    
    /// <summary>
    /// 示例3：使用基础命令
    /// 基础命令提供了最大的灵活性，可以使用匿名类型或简单对象作为请求数据
    /// </summary>
    /// <returns>异步操作结果</returns>
    /// <exception cref="System.Exception">当网络通信失败或服务器返回错误时可能抛出异常</exception>
    public async Task Example3_BaseCommand()
    {
        // 创建基础命令，使用匿名类型作为请求数据
        var command = CommandDataBuilder.BuildBaseCommand(
            AuthenticationCommands.ValidateToken,
            new { Token = "some-token" });
            
        // 发送命令并等待响应
        var response = await _commService.SendCommandAsync<object, bool>(command);
    }
    
    /// <summary>
    /// 示例4：Token自动管理演示
    /// 演示如何利用框架的自动Token管理功能简化身份验证流程
    /// </summary>
    /// <returns>异步操作结果</returns>
    /// <exception cref="System.Exception">当网络通信失败、Token过期且无法刷新或服务器返回错误时可能抛出异常</exception>
    public async Task Example4_TokenAutoManagement()
    {
        // Token会自动通过BaseCommand的AutoAttachToken处理
        // 业务代码无需关心Token管理的细节
        
        // 创建验证Token的命令
        var command = CommandDataBuilder.BuildGenericCommand(
            AuthenticationCommands.ValidateToken,
            new TokenValidationRequest { Token = "token" });
            
        // Token会自动附加到命令中
        // 如果Token过期，框架会自动尝试刷新Token
        var response = await _commService.SendCommandAsync<TokenValidationRequest, bool>(command);
    }
}