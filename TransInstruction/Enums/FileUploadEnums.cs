using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.Enums
{

    // 管理指令类型
    public enum ManagementCommandType
    {
        GetStorageUsage = 200,      // 获取存储使用情况
        CleanTempFiles = 201,       // 清理临时文件
        CheckConsistency = 202,     // 检查一致性
        GetFileStatistics = 203,    // 获取文件统计信息
        BackupFiles = 204,          // 备份文件
        RestoreFiles = 205,         // 恢复文件
        ListFiles = 206,
    }

    // 文件操作指令枚举
    public enum FileOperationCommand
    {
        UploadFile = 100,
        DownloadFile = 101,
        DeleteFile = 102,
        GetFileInfo = 103,
        ListFiles = 104
    }


}
