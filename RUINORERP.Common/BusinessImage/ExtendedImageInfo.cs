using System; using System.Drawing;

namespace RUINORERP.Common.BusinessImage
{
    /// <summary>
    /// 扩展的图片信息类，添加状态管理功能
    /// </summary>
    public class ExtendedImageInfo
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public long ImageId { get; set; }
        public long BusinessId { get; set; }
        /// <summary>
        /// 图片文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 存储的相对路径（不带文件名），用于服务器搜索
        /// </summary>
        public string StoragePath { get; set; }

        /// <summary>
        /// 图片字节数据
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// 图片状态
        /// </summary>
        public ImageStatus Status { get; set; }

        /// <summary>
        /// 图片预览
        /// </summary>
        public Image PreviewImage { get; set; }

        /// <summary>
        /// 单元格引用
        /// </summary>
        public object Cell { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
 