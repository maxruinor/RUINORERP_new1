namespace RUINORERP.Lib.BusinessImage
{
    /// <summary>
    /// 图片同步状态枚举1
    /// </summary>
    public enum ImageStatus
    {
        /// <summary>
        /// 正常状态 - 已有图片，从服务器加载
        /// </summary>
        Normal,
        
        /// <summary>
        /// 待上传 - 新增或修改的图片需要上传到服务器
        /// </summary>
        PendingUpload,
        
        /// <summary>
        /// 待删除 - 需要从服务器删除的图片
        /// </summary>
        PendingDelete
    }
}
