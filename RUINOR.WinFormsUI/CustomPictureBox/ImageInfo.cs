using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Generic;

namespace RUINOR.WinFormsUI.CustomPictureBox
{

    /// <summary>
    /// 图片信息类
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 存储图片的原始文件名
        /// 如果是上传的图片，则为上传时的文件名；
        /// 如果是粘贴或无法获取原始名称的图片，则为自动生成的默认名称
        /// </summary>
        public string OriginalFileName { get; set; }
        public long FileSize { get; set; }
        public DateTime CreateTime { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public string HashValue { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
        public DateTime? ModifiedAt { get; set; }
        
        /// <summary>
        /// 文件ID，用于版本控制
        /// </summary>
        public long FileId { get; set; } = 0;

        /// <summary>
        /// 标记图片是否已更新，用于业务单据更新时只上传变更的图片
        /// </summary>
        public bool IsUpdated { get; set; } = false;
        
        /// <summary>
        /// 标记图片是否已删除
        /// 当用户删除已有图片时，设置为true，用于后续删除服务器上的文件
        /// </summary>
        public bool IsDeleted { get; set; } = false;
        
        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get; set; } = 0;
        
        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; } = 0;
    }
    /// <summary>
    /// 缓存的图片类
    /// </summary>
    public class CachedImage
    {
        public Image Image { get; set; }
        public byte[] ImageBytes { get; set; }
        public DateTime LastAccessTime { get; set; }
    }
}
