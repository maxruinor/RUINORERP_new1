using System;
using System.Collections.Generic;

namespace RUINORERP.Lib.BusinessImage
{
    /// <summary>
    /// 图片信息类
    /// 用于统一管理图片的各种属性和状态，支持业务场景中的图片处理
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ImageInfo()
        {
            Metadata = new Dictionary<string, string>();
        }

        /// <summary>
        /// 图片ID
        /// </summary>
        public long FileId { get; set; }
        
        /// <summary>
        /// 图片ID（兼容属性）
        /// </summary>
        public long ImageId { get; set; }
        
        /// <summary>
        /// 原始文件名
        /// </summary>
        public string OriginalFileName { get; set; }
        
        /// <summary>
        /// 图片字节数据
        /// </summary>
        public byte[] ImageData { get; set; }
        
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension { get; set; }
        
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// 存储路径
        /// </summary>
        public string StoragePath { get; set; }
        
        /// <summary>
        /// 存储文件名
        /// </summary>
        public string StorageFileName { get; set; }
        
        /// <summary>
        /// 业务ID
        /// </summary>
        public long BusinessId { get; set; }
        
        /// <summary>
        /// 业务表名
        /// </summary>
        public string OwnerTableName { get; set; }
        
        /// <summary>
        /// 图片状态
        /// </summary>
        public ImageStatus Status { get; set; }
        
        /// <summary>
        /// 关联字段
        /// </summary>
        public string RelatedField { get; set; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedAt { get; set; }
        
        /// <summary>
        /// 元数据
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
        
        /// <summary>
        /// 哈希值
        /// </summary>
        public string HashValue { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get; set; }
        
        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; }
        
        /// <summary>
        /// 单元格对象
        /// </summary>
        public object Cell { get; set; }
        
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }
}
