using System;
using System.Collections.Generic;
using System.ComponentModel;
using SqlSugar;

namespace RUINORERP.Model
{
    /// <summary>
    /// 待处理的图片操作类型1
    /// </summary>
    public enum PendingImageOperation
    {
        /// <summary>
        /// 新增图片(需要上传)
        /// </summary>
        Add,
        
        /// <summary>
        /// 删除图片(需要解除关联并标记删除)
        /// </summary>
        Delete,
        
        /// <summary>
        /// 替换图片(先删除旧的,再上传新的)
        /// </summary>
        Replace
    }

    /// <summary>
    /// 待处理的图片信息2
    /// 用于在UI编辑期间暂存图片数据,等待统一提交
    /// </summary>
    public class PendingImageInfo
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public PendingImageOperation Operation { get; set; }

        /// <summary>
        /// 图片原始数据(byte数组)
        /// 仅在Operation为Add或Replace时有值
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// 图片文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 已存在的文件ID
        /// 仅在Operation为Delete或Replace时有值(表示要删除的旧图片)
        /// </summary>
        public long? ExistingFileId { get; set; }

        /// <summary>
        /// 图片备注/描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 构造函数 - 新增图片
        /// </summary>
        public static PendingImageInfo CreateAdd(byte[] imageData, string fileName, string description = null, int sortOrder = 0)
        {
            return new PendingImageInfo
            {
                Operation = PendingImageOperation.Add,
                ImageData = imageData,
                FileName = fileName,
                Description = description,
                SortOrder = sortOrder
            };
        }

        /// <summary>
        /// 构造函数 - 删除图片
        /// </summary>
        public static PendingImageInfo CreateDelete(long fileId)
        {
            return new PendingImageInfo
            {
                Operation = PendingImageOperation.Delete,
                ExistingFileId = fileId
            };
        }

        /// <summary>
        /// 构造函数 - 替换图片
        /// </summary>
        public static PendingImageInfo CreateReplace(long existingFileId, byte[] newImageData, string fileName, string description = null, int sortOrder = 0)
        {
            return new PendingImageInfo
            {
                Operation = PendingImageOperation.Replace,
                ExistingFileId = existingFileId,
                ImageData = newImageData,
                FileName = fileName,
                Description = description,
                SortOrder = sortOrder
            };
        }
    }
}
