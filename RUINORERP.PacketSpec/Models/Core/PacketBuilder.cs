using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands;
using static RUINORERP.PacketSpec.Commands.FileTransfer.FileCommands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands.FileTransfer;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 数据包构建器 - 提供流畅的API构建数据包
    /// 使用建造者模式简化数据包创建过程
    /// </summary>
    public class PacketBuilder
    {
        private PacketModel _packet;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private PacketBuilder()
        {
            _packet = new PacketModel();
        }


        /// <summary>
        /// 创建新的构建器实例
        /// </summary>
        /// <returns>构建器实例</returns>
        public static PacketBuilder Create(PacketModel packet)
        {
            PacketBuilder packetBuilder = new PacketBuilder();
            packetBuilder._packet = packet;
            return packetBuilder;
        }


        /// <summary>
        /// 创建新的构建器实例
        /// </summary>
        /// <returns>构建器实例</returns>
        public static PacketBuilder Create()
        {
            return new PacketBuilder();
        }

        /// <summary>
        /// 设置命令类型
        /// </summary>
        /// <param name="command">命令类型</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithCommand(CommandId command)
        {
            _packet.CommandId = command;
            return this;
        }


        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="direction">方向</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithDirection(PacketDirection direction)
        {
            _packet.Direction = direction;
            return this;
        }

        /// <summary>
        /// 设置为请求方向（快捷方法）
        /// </summary>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder AsRequest()
        {
            return WithDirection(PacketDirection.Request);
        }

        /// <summary>
        /// 设置为响应方向（快捷方法）
        /// </summary>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder AsResponse()
        {
            return WithDirection(PacketDirection.Response);
        }

        /// <summary>
        /// 设置会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithSession(string sessionId, string clientId = null)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                _packet.SessionId = sessionId;
                _packet.Extensions["SessionId"] = sessionId;
            }
            return this;
        }


        /// <summary>
        /// 设置二进制数据
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithBinaryData(byte[] data)
        {
            _packet.SetData(data);
            return this;
        }

        /// <summary>
        /// 设置文本数据
        /// </summary>
        /// <param name="text">文本数据</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithTextData(string text, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            _packet.SetData(encoding.GetBytes(text));
            return this;
        }

        /// <summary>
        /// 设置JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据对象</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithJsonData<T>(T data)
        {
            _packet.SetJsonData(data);
            return this;
        }

        public PacketBuilder WithMessagePackData<T>(T data)
        {
            _packet.SetCommandDataByMessagePack(data);
            return this;
        }

        public PacketBuilder WithRequest(IRequest request)
        {
            _packet.Request= request;
            return this;
        }

        /// <summary>
        /// 设置扩展属性
        /// </summary>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithExtension(string key, object value)
        {
            _packet.Extensions[key] = value;
            return this;
        }

        /// <summary>
        /// 设置请求标识
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithRequestId(string requestId)
        {
            return WithExtension("RequestId", requestId);
        }

        /// <summary>
        /// 设置响应超时时间
        /// </summary>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithTimeout(int timeoutMs)
        {
            return WithExtension("Timeout", timeoutMs);
        }

        /// <summary>
        /// 设置是否需要响应
        /// </summary>
        /// <param name="requiresResponse">是否需要响应</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithResponseRequired(bool requiresResponse = true)
        {
            return WithExtension("RequiresResponse", requiresResponse);
        }

        #region 特定命令的快捷方法

        /// <summary>
        /// 创建登录请求数据包
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientVersion">客户端版本</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder AsLoginRequest(string username, string password, string clientVersion = null)
        {
            return WithCommand(AuthenticationCommands.Login)

                .WithDirection(PacketDirection.ClientToServer)
                .WithJsonData(new { Username = username, Password = password, ClientVersion = clientVersion })
                .WithExtension("AuthType", "Basic");
        }

        /// <summary>
        /// 创建心跳请求数据包
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder AsHeartbeatRequest(string sessionId, string clientId = null)
        {
            // 使用系统管理命令中的心跳命令
            return WithCommand(new CommandId(CommandCategory.System, 0xF0))
                .WithDirection(PacketDirection.ClientToServer)
                .WithSession(sessionId, clientId)
                .WithJsonData(new { Timestamp = DateTime.Now });
        }

        /// <summary>
        /// 创建文件下载请求数据包
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="category">文件分类</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder AsFileDownloadRequest(string fileId, string category = null)
        {
            return WithCommand(FileCommands.FileDownload)
                .WithDirection(PacketDirection.ClientToServer)
                .WithJsonData(new { FileId = fileId, Category = category });
        }

        /// <summary>
        /// 创建文件上传请求数据包
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileSize">文件大小</param>
        /// <param name="category">文件分类</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder AsFileUploadRequest(string fileName, long fileSize, string category = null)
        {
            return WithCommand(FileCommands.FileUpload)
                .WithDirection(PacketDirection.ClientToServer)
                .WithJsonData(new { FileName = fileName, FileSize = fileSize, Category = category });
        }

        /// <summary>
        /// 创建查询请求数据包
        /// </summary>
        /// <typeparam name="T">查询条件类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder AsQueryRequest<T>(T query)
        {
            // 使用系统命令作为查询命令
            return WithCommand(new CommandId(CommandCategory.System, 0xF1))
                .WithDirection(PacketDirection.ClientToServer)
                .WithJsonData(query);
        }

        #endregion

        /// <summary>
        /// 构建最终的数据包
        /// </summary>
        /// <returns>构建完成的数据包实例</returns>
        public PacketModel Build()
        {
            // 验证数据包有效性
            if (!_packet.IsValid())
            {
                throw new InvalidOperationException("构建的数据包无效");
            }

            return _packet;
        }

        /// <summary>
        /// 构建并克隆数据包（避免后续修改影响已构建的包）
        /// </summary>
        /// <returns>克隆的数据包实例</returns>
        public PacketModel BuildAndClone()
        {
            return Build().Clone();
        }

        /// <summary>
        /// 构建为JSON字符串
        /// </summary>
        /// <param name="formatting">格式化选项</param>
        /// <returns>JSON字符串</returns>
        public string BuildToJson(Formatting formatting = Formatting.None)
        {
            return Build().ToJson(formatting);
        }

        /// <summary>
        /// 构建为二进制数据
        /// </summary>
        /// <returns>二进制数据</returns>
        public byte[] BuildToBinary()
        {
            return Build().ToBinary();
        }
    }

    /// <summary>
    /// 数据包构建器扩展方法
    /// </summary>
    public static class PacketBuilderExtensions
    {
        /// <summary>
        /// 快速创建登录请求
        /// </summary>
        /// <param name="builder">构建器实例</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>构建器实例</returns>
        public static PacketBuilder ForLogin(this PacketBuilder builder, string username, string password)
        {
            return builder.AsLoginRequest(username, password);
        }

        /// <summary>
        /// 快速创建心跳请求
        /// </summary>
        /// <param name="builder">构建器实例</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>构建器实例</returns>
        public static PacketBuilder ForHeartbeat(this PacketBuilder builder, string sessionId)
        {
            return builder.AsHeartbeatRequest(sessionId);
        }

        /// <summary>
        /// 快速创建文件下载请求
        /// </summary>
        /// <param name="builder">构建器实例</param>
        /// <param name="fileId">文件ID</param>
        /// <returns>构建器实例</returns>
        public static PacketBuilder ForFileDownload(this PacketBuilder builder, string fileId)
        {
            return builder.AsFileDownloadRequest(fileId);
        }

        /// <summary>
        /// 快速创建查询请求
        /// </summary>
        /// <typeparam name="T">查询条件类型</typeparam>
        /// <param name="builder">构建器实例</param>
        /// <param name="query">查询条件</param>
        /// <returns>构建器实例</returns>
        public static PacketBuilder ForQuery<T>(this PacketBuilder builder, T query)
        {
            return builder.AsQueryRequest(query);
        }




        /// <summary>
        /// 设置客户端到服务器方向
        /// </summary>
        /// <param name="builder">构建器实例</param>
        /// <returns>构建器实例</returns>
        public static PacketBuilder FromClientToServer(this PacketBuilder builder)
        {
            return builder.WithDirection(PacketDirection.ClientToServer);
        }

        /// <summary>
        /// 设置服务器到客户端方向
        /// </summary>
        /// <param name="builder">构建器实例</param>
        /// <returns>构建器实例</returns>
        public static PacketBuilder FromServerToClient(this PacketBuilder builder)
        {
            return builder.WithDirection(PacketDirection.ServerToClient);
        }

        /// <summary>
        /// 为请求创建数据包
        /// </summary>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="builder">构建器实例</param>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求数据</param>
        /// <returns>构建器实例</returns>
        public static PacketBuilder ForRequest<TRequest>(this PacketBuilder builder, RUINORERP.PacketSpec.Commands.CommandId commandId, TRequest request)
        {
            return builder.WithCommand(commandId)
                         .WithDirection(PacketDirection.ClientToServer)
                         .WithJsonData(request);
        }

        /// <summary>
        /// 为响应创建数据包
        /// </summary>
        /// <typeparam name="TResponse">响应数据类型</typeparam>
        /// <param name="builder">构建器实例</param>
        /// <param name="commandId">命令标识符</param>
        /// <param name="response">响应数据</param>
        /// <returns>构建器实例</returns>
        public static PacketBuilder ForResponse<TResponse>(this PacketBuilder builder, RUINORERP.PacketSpec.Commands.CommandId commandId, TResponse response)
        {
            return builder.WithCommand(commandId)
                         .WithDirection(PacketDirection.ServerToClient)
                         .WithJsonData(response);
        }
    }
}
