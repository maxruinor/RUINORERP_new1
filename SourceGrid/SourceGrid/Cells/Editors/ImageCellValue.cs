using System;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// 图片状态枚举
    /// </summary>
    public enum ImageStatus
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        Normal,
        
        /// <summary>
        /// 待删除状态
        /// </summary>
        PendingDelete,
        
        /// <summary>
        /// 待上传状态
        /// </summary>
        PendingUpload
    }

    /// <summary>
    /// 图片单元格值类
    /// 轻量级包装器，用于存储二进制图片数据或图片路径引用
    /// 支持图片单元格的两种存储模式：内存中的二进制数据或路径引用
    /// </summary>
    [Serializable]
    public class ImageCellValue
    {
        /// <summary>
        /// 内存中的图片数据（编辑/显示时优先使用）
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// 图片路径/标识符（用于传统或外部存储）
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 相对文件路径（包含文件名和扩展名）
        /// </summary>
        public string RelativeFilePath { get; set; }

        /// <summary>
        /// 文件存储表ID
        /// </summary>
        public long? FileStorageId { get; set; }

        /// <summary>
        /// 便捷属性：当前值是否存储二进制数据？
        /// </summary>
        public bool IsBinary => ImageData != null && ImageData.Length > 0;
    }
}