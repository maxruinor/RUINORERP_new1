using System.Collections.Generic;

namespace RUINORERP.Common.BusinessImage
{
    /// <summary>
    /// 图片同步类型
    /// </summary>
    public enum ImageSyncType
    {
        /// <summary>
        /// 新增图片
        /// </summary>
        Add,
        /// <summary>
        /// 删除图片
        /// </summary>
        Delete
    }

    /// <summary>
    /// 图片同步结果
    /// </summary>
    public class ImageSyncResult
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public long BusinessId { get; set; }
        /// <summary>
        /// 同步的图片ID集合
        /// </summary>
        public List<long> ImageIds { get; set; }
        /// <summary>
        /// 同步类型
        /// </summary>
        public ImageSyncType SyncType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="businessId">业务ID</param>
        /// <param name="imageIds">图片ID集合</param>
        /// <param name="syncType">同步类型</param>
        public ImageSyncResult(long businessId, List<long> imageIds, ImageSyncType syncType)
        {
            BusinessId = businessId;
            ImageIds = imageIds;
            SyncType = syncType;
        }
    }
}
