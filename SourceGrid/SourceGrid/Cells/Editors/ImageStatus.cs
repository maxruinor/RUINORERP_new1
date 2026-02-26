using System;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// 图片状态枚举
    /// </summary>
    public enum ImageStatus
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        Normal,
        
        /// <summary>
        /// 待删除状态
        /// </summary>
        PendingDelete,
        
        /// <summary>
        /// 待上传状态
        /// </summary>
        PendingUpload
    }

}