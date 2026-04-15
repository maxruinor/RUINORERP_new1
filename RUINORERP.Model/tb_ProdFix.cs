

// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/15/2025 10:00:00
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using System.Drawing;
using RUINORERP.Global;
using RUINORERP.Global.Model;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Model
{
    /// <summary>
    /// tb_Prod 扩展类, 用于存放临时字段和辅助功能
    /// 用于产品主图的PendingImages管理
    /// </summary>
    public partial class tb_Prod
    {
        #region 图片管理扩展字段

        /// <summary>
        /// 待处理的图片操作列表(UI临时字段,不存储到数据库)
        /// 用于在编辑期间暂存产品主图的新增/删除/替换操作,等待UCProductList.Save()时统一提交
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public List<PendingImageInfo> PendingImages { get; set; } = new List<PendingImageInfo>();

        /// <summary>
        /// 是否有未保存的图片更改（UI临时字段）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public bool HasUnsavedImageChanges
        {
            get => PendingImages != null && PendingImages.Count > 0;
        }

        /// <summary>
        /// 添加待上传的图片
        /// </summary>
        /// <param name="imageData">图片原始数据</param>
        /// <param name="fileName">文件名</param>
        /// <param name="description">描述</param>
        /// <param name="sortOrder">排序</param>
        public void AddPendingImage(byte[] imageData, string fileName, string description = null, int sortOrder = 0)
        {
            if (PendingImages == null)
                PendingImages = new List<PendingImageInfo>();

            PendingImages.Add(PendingImageInfo.CreateAdd(imageData, fileName, description, sortOrder));
        }

        /// <summary>
        /// 标记删除已存在的图片
        /// </summary>
        /// <param name="fileId">要删除的文件ID</param>
        public void MarkImageForDeletion(long fileId)
        {
            if (PendingImages == null)
                PendingImages = new List<PendingImageInfo>();

            PendingImages.Add(PendingImageInfo.CreateDelete(fileId));
        }

        /// <summary>
        /// 标记替换图片(删除旧的,上传新的)
        /// </summary>
        /// <param name="existingFileId">现有文件ID</param>
        /// <param name="newImageData">新图片数据</param>
        /// <param name="fileName">新文件名</param>
        /// <param name="description">描述</param>
        /// <param name="sortOrder">排序</param>
        public void MarkImageForReplacement(long existingFileId, byte[] newImageData, string fileName, string description = null, int sortOrder = 0)
        {
            if (PendingImages == null)
                PendingImages = new List<PendingImageInfo>();

            PendingImages.Add(PendingImageInfo.CreateReplace(existingFileId, newImageData, fileName, description, sortOrder));
        }

        /// <summary>
        /// 清除所有待处理的图片操作(提交成功后调用)
        /// </summary>
        public void ClearPendingImages()
        {
            PendingImages?.Clear();
        }

        /// <summary>
        /// 获取待新增的图片列表
        /// </summary>
        public List<PendingImageInfo> GetPendingAddImages()
        {
            return PendingImages?.Where(p => p.Operation == PendingImageOperation.Add).ToList() ?? new List<PendingImageInfo>();
        }

        /// <summary>
        /// 获取待删除的图片列表
        /// </summary>
        public List<PendingImageInfo> GetPendingDeleteImages()
        {
            return PendingImages?.Where(p => p.Operation == PendingImageOperation.Delete).ToList() ?? new List<PendingImageInfo>();
        }

        /// <summary>
        /// 获取待替换的图片列表
        /// </summary>
        public List<PendingImageInfo> GetPendingReplaceImages()
        {
            return PendingImages?.Where(p => p.Operation == PendingImageOperation.Replace).ToList() ?? new List<PendingImageInfo>();
        }

        #endregion
    }
}

