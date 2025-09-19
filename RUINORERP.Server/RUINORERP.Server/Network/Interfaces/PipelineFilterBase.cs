using System;
using System.Buffers;

namespace RUINORERP.Server.Network.Interfaces
{
    /// <summary>
    /// ✅ [统一架构] 管道过滤器基类接口
    /// 定义管道过滤器的基本行为和结构
    /// </summary>
    /// <typeparam name="TPackageInfo">包信息类型</typeparam>
    public abstract class PipelineFilterBase<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 过滤器状态
        /// </summary>
        public FilterState State { get; protected set; } = FilterState.Normal;

        /// <summary>
        /// 执行过滤操作
        /// </summary>
        /// <param name="buffer">输入缓冲区</param>
        /// <returns>过滤后的包信息</returns>
        public abstract TPackageInfo Filter(ref ReadOnlySequence<byte> buffer);

        /// <summary>
        /// 重置过滤器状态
        /// </summary>
        public virtual void Reset()
        {
            State = FilterState.Normal;
        }

        /// <summary>
        /// 过滤器状态枚举
        /// </summary>
        public enum FilterState
        {
            /// <summary>
            /// 正常状态
            /// </summary>
            Normal,

            /// <summary>
            /// 错误状态
            /// </summary>
            Error,

            /// <summary>
            /// 缓存状态
            /// </summary>
            Cached
        }
    }

    /// <summary>
    /// 包解码器接口
    /// </summary>
    /// <typeparam name="TPackageInfo">包信息类型</typeparam>
    public interface IPackageDecoder<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 解码数据包
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <returns>解码后的包信息</returns>
        TPackageInfo Decode(ReadOnlySpan<byte> data);
    }

    /// <summary>
    /// 包编码器接口
    /// </summary>
    /// <typeparam name="TPackageInfo">包信息类型</typeparam>
    public interface IPackageEncoder<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 编码数据包
        /// </summary>
        /// <param name="package">包信息</param>
        /// <returns>编码后的数据</returns>
        byte[] Encode(TPackageInfo package);
    }

    /// <summary>
    /// 包验证器接口
    /// </summary>
    /// <typeparam name="TPackageInfo">包信息类型</typeparam>
    public interface IPackageValidator<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 验证数据包
        /// </summary>
        /// <param name="package">包信息</param>
        /// <returns>验证结果</returns>
        bool Validate(TPackageInfo package);
    }

    /// <summary>
    /// 包处理器接口
    /// </summary>
    /// <typeparam name="TPackageInfo">包信息类型</typeparam>
    public interface IPackageProcessor<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 处理数据包
        /// </summary>
        /// <param name="package">包信息</param>
        void Process(TPackageInfo package);
    }

    /// <summary>
    /// 包工厂接口
    /// </summary>
    /// <typeparam name="TPackageInfo">包信息类型</typeparam>
    public interface IPackageFactory<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 创建包信息实例
        /// </summary>
        /// <returns>包信息实例</returns>
        TPackageInfo Create();
    }
}