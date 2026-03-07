using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using DevAge.Drawing;
using DevAge.Drawing.VisualElements;
using System.Threading.Tasks;
using RUINORERP.Common.BusinessImage;

namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// 单图片单元格视图
    /// 用于显示单张本地图片
    /// </summary>
    [Serializable]
    public class SingleImageView : ImageCellBase, IImageCellView
    {
        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SingleImageView() : base()
        {
        }

        /// <summary>
        /// 使用图片对象构造
        /// </summary>
        /// <param name="image">图片对象</param>
        public SingleImageView(Image image) : base(image)
        {
        }

        #endregion

        #region IImageCellView Implementation

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        public void LoadImage(long fileId)
        {
            _currentFileId = fileId;
            _pendingFileId = fileId;
            CurrentFileId = fileId;
        }

        /// <summary>
        /// 异步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>加载任务</returns>
        public Task LoadImageAsync(long fileId)
        {
            _currentFileId = fileId;
            CurrentFileId = fileId;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 清空图片
        /// </summary>
        public void ClearImage()
        {
            GridImage = null;
            _currentImageHash = string.Empty;
        }

        /// <summary>
        /// 获取图片ID
        /// </summary>
        /// <returns>图片ID</returns>
        public long GetImageId()
        {
            return CurrentFileId;
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <returns>图片信息</returns>
        public ImageInfo GetImageInfo()
        {
            return ImageStateManager.Instance.GetImageInfo(CurrentFileId);
        }

        #endregion

        #region 刷新

        /// <summary>
        /// 刷新视图
        /// </summary>
        /// <param name="context">单元格上下文</param>
        public override void Refresh(CellContext context)
        {
            // 刷新逻辑
        }

        #endregion

        #region Clone

        /// <summary>
        /// 克隆当前对象
        /// </summary>
        /// <returns>克隆的对象</returns>
        public override object Clone()
        {
            return new SingleImageView();
        }

        #endregion
    }

    /// <summary>
    /// SingleImage的兼容性类
    /// 保留旧类名以保持向后兼容
    /// </summary>
    [Serializable]
    [Obsolete("请使用SingleImageView替代SingleImage")]
    public class SingleImage : SingleImageView
    {
        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SingleImage() : base()
        {
        }

        /// <summary>
        /// 使用图片对象构造
        /// </summary>
        /// <param name="image">图片对象</param>
        public SingleImage(Image image) : base(image)
        {
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="other">源对象</param>
        public SingleImage(SingleImage other) : base()
        {
            // 复制属性
            if (other != null)
            {
                CurrentFileId = other.CurrentFileId;
                // 其他属性复制
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// 克隆当前对象
        /// </summary>
        /// <returns>克隆的对象</returns>
        public override object Clone()
        {
            return new SingleImage(this);
        }

        #endregion
    }
}
