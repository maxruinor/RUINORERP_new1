using System.Threading.Tasks;
using RUINORERP.Common.BusinessImage;
using SourceGrid.Cells;

namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// 图片单元格视图接口
    /// 定义图片单元格的通用操作
    /// </summary>
    public interface IImageCellView
    {
        #region Image Loading

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        void LoadImage(long fileId);

        /// <summary>
        /// 异步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>加载任务</returns>
        Task LoadImageAsync(long fileId);

        /// <summary>
        /// 清空图片
        /// </summary>
        void ClearImage();

        #endregion

        #region Image Information

        /// <summary>
        /// 获取图片ID
        /// </summary>
        /// <returns>图片ID</returns>
        long GetImageId();

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <returns>图片信息</returns>
        ImageInfo GetImageInfo();

        #endregion
    }
}
