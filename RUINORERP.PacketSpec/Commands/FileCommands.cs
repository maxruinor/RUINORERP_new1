using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 文件相关命令
    /// </summary>
    public static class FileCommands
    {
        #region 文件命令 (0x06xx)
        /// <summary>
        /// 文件操作 - 执行文件相关操作
        /// </summary>

        public static readonly CommandId FileOperation = new CommandId(CommandCategory.File, 0x00);
        
        /// <summary>
        /// 文件上传 - 上传文件到服务器
        /// </summary>

        public static readonly CommandId FileUpload = new CommandId(CommandCategory.File, 0x01);
        
        /// <summary>
        /// 文件下载 - 从服务器下载文件
        /// </summary>

        public static readonly CommandId FileDownload = new CommandId(CommandCategory.File, 0x02);
        
        /// <summary>
        /// 文件删除 - 从服务器删除文件
        /// </summary>
        public static readonly CommandId FileDelete = new CommandId(CommandCategory.File, 0x03);
        
        /// <summary>
        /// 文件信息查询 - 查询文件信息
        /// </summary>
        public static readonly CommandId FileInfoQuery = new CommandId(CommandCategory.File, 0x04);
        
        /// <summary>
        /// 文件列表 - 获取文件列表
        /// </summary>
        public static readonly CommandId FileList = new CommandId(CommandCategory.File, 0x05);
        
        /// <summary>
        /// 文件权限检查 - 检查文件访问权限
        /// </summary>
        public static readonly CommandId FilePermissionCheck = new CommandId(CommandCategory.File, 0x06);
        
        /// <summary>
        /// 文件重命名 - 重命名文件
        /// </summary>
        public static readonly CommandId FileRename = new CommandId(CommandCategory.File, 0x07);
        
        /// <summary>
        /// 文件移动 - 移动文件位置
        /// </summary>
        public static readonly CommandId FileMove = new CommandId(CommandCategory.File, 0x08);
        
        /// <summary>
        /// 文件复制 - 复制文件
        /// </summary>
        public static readonly CommandId FileCopy = new CommandId(CommandCategory.File, 0x09);
        #endregion
    }
}
