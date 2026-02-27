using System;
using System.Collections.Generic;
using System.Drawing;

namespace RUINORERP.Common.BusinessImage
{
    /// <summary>
    /// 图片信息类，包含基本信息和状态管理功能
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public long ImageId { get; set; }
        
        /// <summary>
        /// 业务ID，用于关联业务实体
        /// </summary>
        public long BusinessId { get; set; }
        
        /// <summary>
        /// 存储图片的原始文件名
        /// 如果是上传的图片，则为上传时的文件名；
        /// 如果是粘贴或无法获取原始名称的图片，则为自动生成的默认名称
        /// </summary>
        public string OriginalFileName { get; set; }
        
        /// <summary>
        /// 图片文件名（用于存储）
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// 存储的相对路径（不带文件名），用于服务器搜索
        /// </summary>
        public string StoragePath { get; set; }
        
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension { get; set; }
        
        /// <summary>
        /// 文件哈希值
        /// </summary>
        public string HashValue { get; set; }
        
        /// <summary>
        /// 元数据
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedAt { get; set; }
        
        /// <summary>
        /// 文件ID，用于版本控制
        /// </summary>
        public long FileId { get; set; } = 0;
        
        /// <summary>
        /// 图片字节数据
        /// </summary>
        public byte[] ImageData { get; set; }
        
        /// <summary>
        /// 图片状态
        /// </summary>
        public ImageStatus Status { get; set; } = ImageStatus.Normal;
        
        /// <summary>
        /// 图片预览
        /// </summary>
        public Image PreviewImage { get; set; }
        
        /// <summary>
        /// 单元格引用
        /// </summary>
        public object Cell { get; set; }
        
        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get; set; } = 0;
        
        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; } = 0;
    }
}
