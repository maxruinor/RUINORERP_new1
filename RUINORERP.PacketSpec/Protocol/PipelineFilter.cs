using SuperSocket.ProtoBase;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Enums;
using System;
using System.Buffers;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Protocol
{
    /// <summary>
    /// 基础管道过滤器
    /// </summary>
    /// <typeparam name="TPackageInfo">包信息类型</typeparam>
    public abstract class BasePipelineFilter<TPackageInfo> : PipelineFilterBase<TPackageInfo>
        where TPackageInfo : class
    {
        /// <summary>
        /// 包解码器
        /// </summary>
        protected IPackageDecoder<TPackageInfo> Decoder { get; set; }

        /// <summary>
        /// 包编码器
        /// </summary>
        protected IPackageEncoder<TPackageInfo> Encoder { get; set; }

        /// <summary>
        /// 过滤器状态
        /// </summary>
        public FilterState State { get; protected set; }

        /// <summary>
        /// 最后错误信息
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// 处理包计数
        /// </summary>
        public long ProcessedPacketsCount { get; protected set; }

        /// <summary>
        /// 最大包长度限制
        /// </summary>
        public int MaxPackageLength { get; set; } = 1024 * 64; // 默认64KB

        /// <summary>
        /// 初始化管道过滤器
        /// </summary>
        protected BasePipelineFilter(IPackageDecoder<TPackageInfo> decoder, IPackageEncoder<TPackageInfo> encoder)
        {
            Decoder = decoder;
            Encoder = encoder;
            State = FilterState.Ready;
        }

        /// <summary>
        /// 重置过滤器状态
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            State = FilterState.Ready;
            LastError = null;
        }

        /// <summary>
        /// 验证缓冲区数据
        /// </summary>
        protected virtual bool ValidateBuffer(ReadOnlySequence<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                LastError = "空数据缓冲区";
                return false;
            }

            if (buffer.Length > MaxPackageLength)
            {
                LastError = $"包大小超过限制: {buffer.Length} > {MaxPackageLength}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 解码包数据
        /// </summary>
        protected virtual TPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            try
            {
                var package = Decoder.Decode(ref buffer, this);
                if (package != null)
                {
                    ProcessedPacketsCount++;
                    return package;
                }

                LastError = "包解码失败";
                return null;
            }
            catch (Exception ex)
            {
                LastError = $"解码包时发生错误: {ex.Message}";
                return null;
            }
        }
    }

    /// <summary>
    /// 默认管道过滤器
    /// </summary>
    public class DefaultPipelineFilter : BasePipelineFilter<BizPackageInfo>
    {
        /// <summary>
        /// 初始化默认管道过滤器
        /// </summary>
        public DefaultPipelineFilter() 
            : base(new BinaryPackageDecoder(), new BinaryPackageEncoder())
        {
        }

        /// <summary>
        /// 过滤数据包
        /// </summary>
        public override BizPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            if (!ValidateBuffer(reader.Sequence))
            {
                return null;
            }

            try
            {
                // 读取包长度前缀
                if (!reader.TryReadLittleEndian(out int packageLength) || packageLength <= 0)
                {
                    LastError = "无效的包长度";
                    return null;
                }

                // 检查是否有足够的数据
                if (reader.Remaining < packageLength)
                {
                    // 需要更多数据
                    return null;
                }

                // 读取包数据
                var packageData = reader.Sequence.Slice(reader.Position, packageLength);
                reader.Advance(packageLength);

                // 解码包数据
                return DecodePackage(ref packageData);
            }
            catch (Exception ex)
            {
                LastError = $"过滤数据时发生错误: {ex.Message}";
                State = FilterState.Error;
                return null;
            }
        }
    }
}