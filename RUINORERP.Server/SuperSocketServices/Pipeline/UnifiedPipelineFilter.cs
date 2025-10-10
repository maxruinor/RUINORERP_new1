using System;
using System.Buffers;
using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Security;
using RUINORERP.Server.Network.Core;

namespace RUINORERP.Server.Network.Pipeline
{
    /// <summary>
    /// ✅ [统一架构] 统一管道过滤器 - 使用新的PacketSpec统一架构
    /// 合并了原有的BizPipelineFilter功能，支持统一数据包处理
    /// </summary>
    public class UnifiedPipelineFilter : FixedHeaderPipelineFilter<BizPackageInfo>
    {
        private static readonly int HeaderLength = 18; // 包头长度
        private readonly ILogger<UnifiedPipelineFilter> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public UnifiedPipelineFilter(ILogger<UnifiedPipelineFilter> logger = null) 
            : base(HeaderLength)
        {
            _logger = logger;
            LogDebug("统一管道过滤器初始化完成");
        }

        /// <summary>
        /// 获取包体长度
        /// 通过包头18个字节的内容解析出还需要多少字节才是一个完整的包
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <returns>包体长度</returns>
        protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
        {
            try
            {
                var reader = new SequenceReader<byte>(buffer);
                
                // 跳过前面的字段，读取长度字段
                // 根据BizPackageInfo的结构，长度字段通常在特定位置
                reader.Advance(8); // 跳过前8个字节
                
                if (reader.TryReadLittleEndian(out int bodyLength))
                {
                    LogDebug($"解析包体长度: {bodyLength}");
                    return bodyLength;
                }
                
                LogWarning("无法解析包体长度，使用默认值0");
                return 0;
            }
            catch (Exception ex)
            {
                LogError($"解析包体长度时出错: {ex.Message}", ex);
                return 0;
            }
        }

        /// <summary>
        /// 解码数据包
        /// 将接收到的字节序列解码为BizPackageInfo对象
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>解码后的数据包</returns>
        protected override BizPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            try
            {
                LogDebug($"开始解码数据包，缓冲区长度: {buffer.Length}");

                // 将缓冲区转换为字节数组
                var packageBytes = buffer.ToArray();
                
                // 使用统一的数据包解码器
                var decoder = new UnifiedPackageDecoder(_logger as ILogger<UnifiedPackageDecoder>);
                var bizPackage = decoder.DecodeBizPackage(packageBytes);
                
                if (bizPackage != null)
                {
                    LogDebug($"数据包解码成功: Command={bizPackage.cmd}, SessionID={bizPackage.sessionid}, DataLength={bizPackage.Data?.Length ?? 0}");
                }
                else
                {
                    LogWarning("数据包解码失败，返回null");
                }

                return bizPackage;
            }
            catch (Exception ex)
            {
                LogError($"解码数据包时出错: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void LogDebug(string message)
        {
            _logger?.LogDebug($"[UnifiedPipelineFilter] {message}");
            // 调试模式下输出到控制台
            #if DEBUG
            Console.WriteLine($"[UnifiedPipelineFilter] DEBUG: {message}");
            #endif
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">警告消息</param>
        private void LogWarning(string message)
        {
            _logger?.LogWarning($"[UnifiedPipelineFilter] {message}");
            Console.WriteLine($"[UnifiedPipelineFilter] WARNING: {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="ex">异常对象</param>
        private void LogError(string message, Exception ex = null)
        {
            _logger?.LogError(ex, $"[UnifiedPipelineFilter] {message}");
            Console.WriteLine($"[UnifiedPipelineFilter] ERROR: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[UnifiedPipelineFilter] Exception: {ex}");
            }
        }
    }

    /// <summary>
    /// 统一数据包解码器
    /// </summary>
    public class UnifiedPackageDecoder
    {
        private readonly ILogger<UnifiedPackageDecoder> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public UnifiedPackageDecoder(ILogger<UnifiedPackageDecoder> logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 解码BizPackage
        /// </summary>
        /// <param name="packageBytes">数据包字节数组</param>
        /// <returns>解码后的BizPackageInfo</returns>
        public BizPackageInfo DecodeBizPackage(byte[] packageBytes)
        {
            try
            {
                if (packageBytes == null || packageBytes.Length < 18)
                {
                    LogWarning($"数据包长度不足，需要至少18字节，实际: {packageBytes?.Length ?? 0}");
                    return null;
                }

                // 使用BinaryPackageDecoder解码
                var decoder = new BinaryPackageDecoder();
                var bizPackage = decoder.DecodeBizPackage(packageBytes);

                if (bizPackage != null)
                {
                    LogDebug($"BizPackage解码成功: Command={bizPackage.cmd}");
                    
                    // 验证数据完整性
                    if (!ValidatePackage(bizPackage))
                    {
                        LogWarning("数据包验证失败");
                        return null;
                    }
                }

                return bizPackage;
            }
            catch (Exception ex)
            {
                LogError($"解码BizPackage时出错: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 验证数据包完整性
        /// </summary>
        /// <param name="package">数据包</param>
        /// <returns>验证结果</returns>
        private bool ValidatePackage(BizPackageInfo package)
        {
            try
            {
                // 基本验证
                if (package.cmd <= 0)
                {
                    LogWarning($"无效的命令ID: {package.cmd}");
                    return false;
                }

                if (string.IsNullOrEmpty(package.sessionid))
                {
                    LogWarning("会话ID为空");
                    return false;
                }

                // 可以添加更多验证逻辑
                return true;
            }
            catch (Exception ex)
            {
                LogError($"验证数据包时出错: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void LogDebug(string message)
        {
            _logger?.LogDebug($"[UnifiedPackageDecoder] {message}");
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">警告消息</param>
        private void LogWarning(string message)
        {
            _logger?.LogWarning($"[UnifiedPackageDecoder] {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="ex">异常对象</param>
        private void LogError(string message, Exception ex = null)
        {
            _logger?.LogError(ex, $"[UnifiedPackageDecoder] {message}");
        }
    }
}