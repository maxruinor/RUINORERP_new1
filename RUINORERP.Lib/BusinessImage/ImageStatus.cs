namespace RUINORERP.Lib.BusinessImage
{
    /// <summary>
    /// 图片状态枚举12
    /// 定义图片在不同业务场景下的状态
    /// </summary>
    public enum ImageStatus
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        Normal,
        
        /// <summary>
        /// 待上传
        /// </summary>
        PendingUpload,
        
        /// <summary>
        /// 处理中（防止重复处理）
        /// </summary>
        Processing,
        
        /// <summary>
        /// 已上传
        /// </summary>
        Uploaded,
        
        /// <summary>
        /// 待删除
        /// </summary>
        PendingDelete,
        
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted
    }
}
