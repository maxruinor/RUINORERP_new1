using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.File
{
    /// <summary>
    /// 文件操作命令枚举
    /// </summary>
    public enum FileCommand : uint
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        [Description("文件上传")]
        FileUpload = 0x0600,

        /// <summary>
        /// 文件下载
        /// </summary>
        [Description("文件下载")]
        FileDownload = 0x0601,

        /// <summary>
        /// 文件删除
        /// </summary>
        [Description("文件删除")]
        FileDelete = 0x0602,

        /// <summary>
        /// 文件列表
        /// </summary>
        [Description("文件列表")]
        FileList = 0x0603,

        /// <summary>
        /// 文件信息
        /// </summary>
        [Description("文件信息")]
        FileInfo = 0x0604,

        /// <summary>
        /// 文件操作
        /// </summary>
        [Description("文件操作")]
        FileOperation = 0x0605,

        /// <summary>
        /// 文件传输
        /// </summary>
        [Description("文件传输")]
        FileTransfer = 0x0606
    }


    /// <summary>
    /// 过滤器状态枚举
    /// </summary>
    public enum FilterState
    {
        /// <summary>
        /// 准备就绪
        /// </summary>
        Ready,

        /// <summary>
        /// 处理中
        /// </summary>
        Processing,

        /// <summary>
        /// 错误状态
        /// </summary>
        Error,

        /// <summary>
        /// 完成状态
        /// </summary>
        Completed
    }


}
