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
        /// 文件上传 - 上传文件到服务器
        /// </summary>
        public static readonly CommandId FileUpload = new CommandId(CommandCategory.File, (byte)(CommandCatalog.File_FileUpload & 0xFF));

        /// <summary>
        /// 文件下载 - 从服务器下载文件
        /// </summary>
        public static readonly CommandId FileDownload = new CommandId(CommandCategory.File, (byte)(CommandCatalog.File_FileDownload & 0xFF));

        /// <summary>
        /// 文件删除 - 从服务器删除文件
        /// </summary>
        public static readonly CommandId FileDelete = new CommandId(CommandCategory.File, (byte)(CommandCatalog.File_FileDelete & 0xFF));

        /// <summary>
        /// 文件信息查询 - 查询文件信息
        /// </summary>
        public static readonly CommandId FileInfoQuery = new CommandId(CommandCategory.File, (byte)(CommandCatalog.File_FileInfoQuery & 0xFF));

        /// <summary>
        /// 文件列表 - 获取文件列表
        /// </summary>
        public static readonly CommandId FileList = new CommandId(CommandCategory.File, (byte)(CommandCatalog.File_FileList & 0xFF));

        /// <summary>
        /// 文件权限检查 - 检查文件访问权限
        /// </summary>
        public static readonly CommandId FilePermissionCheck = new CommandId(CommandCategory.File, (byte)(CommandCatalog.File_FilePermissionCheck & 0xFF));

        /// <summary>
        /// 文件存储信息 - 获取文件存储使用情况
        /// </summary>
        public static readonly CommandId FileStorageInfo = new CommandId(CommandCategory.File, (byte)(CommandCatalog.File_FileStorageInfo & 0xFF));
 
        #endregion
    }
}
