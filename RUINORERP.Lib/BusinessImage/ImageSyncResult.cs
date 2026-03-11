namespace RUINORERP.Lib.BusinessImage
{
    /// <summary>
    /// 图片同步类型
    /// </summary>
    public enum ImageSyncType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add,

        /// <summary>
        /// 删除
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
        /// 图片ID列表
        /// </summary>
        public System.Collections.Generic.List<long> ImageIds { get; set; } = new System.Collections.Generic.List<long>();

        /// <summary>
        /// 同步类型
        /// </summary>
        public ImageSyncType SyncType { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 成功上传的图片数量
        /// </summary>
        public int UploadedCount { get; set; }

        /// <summary>
        /// 成功删除的图片数量
        /// </summary>
        public int DeletedCount { get; set; }

        /// <summary>
        /// 失败的图片ID列表
        /// </summary>
        public System.Collections.Generic.List<long> FailedImageIds { get; set; } = new System.Collections.Generic.List<long>();

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static ImageSyncResult CreateSuccess(int uploadedCount = 0, int deletedCount = 0)
        {
            return new ImageSyncResult
            {
                IsSuccess = true,
                UploadedCount = uploadedCount,
                DeletedCount = deletedCount
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static ImageSyncResult CreateFailure(string errorMessage)
        {
            return new ImageSyncResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
