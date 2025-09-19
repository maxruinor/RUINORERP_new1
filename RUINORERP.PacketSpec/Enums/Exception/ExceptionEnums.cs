using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Exception
{
    /// <summary>
    /// 异常处理命令枚举
    /// </summary>
    public enum ExceptionCommand : uint
    {
        /// <summary>
        /// 实时汇报异常
        /// </summary>
        [Description("实时汇报异常")]
        ReportException = 0x0500,

        /// <summary>
        /// 转发异常
        /// </summary>
        [Description("转发异常")]
        ForwardException = 0x0501,

        /// <summary>
        /// 请求协助处理
        /// </summary>
        [Description("请求协助处理")]
        RequestAssistance = 0x0502,

        /// <summary>
        /// 转发协助处理
        /// </summary>
        [Description("转发协助处理")]
        ForwardAssistance = 0x0503
    }
}