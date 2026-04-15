
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/22/2024 18:15:11
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
using RUINORERP.Model.BusinessImage;
namespace RUINORERP.Model
{
    /// <summary>
    /// tb_ProdDetail 扩展类, 用于存放临时字段和辅助功能1
    /// 后面按这个规律统一处理。如果实现中带有图片的字段。则需要额外处理
    /// 字段名：RowImage
    /// 后面如果支持多图则可能是List<key,value> key是图片名, value是图片对象
    /// </summary>
    public partial class tb_ProdDetail 
    {
        #region 图片管理扩展字段

        /// <summary>
        /// ✅ 简化: 待处理的图片操作列表(UI临时字段,不存储到数据库)22
        /// 直接使用ImageInfo,通过Status字段管理状态,消除PendingImageInfo冗余
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public List<ImageInfo> PendingImages { get; set; } = new List<ImageInfo>();

        /// <summary>
        /// ✅ 简化: 是否有未保存的图片更改（UI临时字段）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public bool HasUnsavedImageChanges 
        { 
            get => PendingImages?.Any(img => img.Status != ImageStatus.Normal) ?? false;
        }

        /// <summary>
        /// ✅ 简化: 添加待上传的图片
        /// </summary>
        /// <param name="imageData">图片原始数据</param>
        /// <param name="fileName">文件名</param>
        /// <param name="description">描述</param>
        /// <param name="sortOrder">排序</param>
        public void AddPendingImage(byte[] imageData, string fileName, string description = null, int sortOrder = 0)
        {
            if (PendingImages == null)
                PendingImages = new List<ImageInfo>();
            
            PendingImages.Add(new ImageInfo
            {
                FileId = 0,
                ImageData = imageData,
                FileName = fileName,
                Description = description,
                SortOrder = sortOrder,
                Status = ImageStatus.PendingUpload  // ✅ 直接使用ImageStatus
            });
        }

        /// <summary>
        /// ✅ 简化: 标记删除已存在的图片
        /// </summary>
        /// <param name="fileId">要删除的文件ID</param>
        public void MarkImageForDeletion(long fileId)
        {
            if (PendingImages == null)
                PendingImages = new List<ImageInfo>();
            
            PendingImages.Add(new ImageInfo
            {
                FileId = fileId,
                Status = ImageStatus.PendingDelete  // ✅ 直接使用ImageStatus
            });
        }

        /// <summary>
        /// ✅ 简化: 标记替换图片(删除旧的,上传新的)
        /// </summary>
        /// <param name="existingFileId">现有文件ID</param>
        /// <param name="newImageData">新图片数据</param>
        /// <param name="fileName">新文件名</param>
        /// <param name="description">描述</param>
        /// <param name="sortOrder">排序</param>
        public void MarkImageForReplacement(long existingFileId, byte[] newImageData, string fileName, string description = null, int sortOrder = 0)
        {
            if (PendingImages == null)
                PendingImages = new List<ImageInfo>();
            
            // 先标记旧图片为删除
            PendingImages.Add(new ImageInfo
            {
                FileId = existingFileId,
                Status = ImageStatus.PendingDelete
            });
            
            // 再添加新图片
            PendingImages.Add(new ImageInfo
            {
                FileId = 0,
                ImageData = newImageData,
                FileName = fileName,
                Description = description,
                SortOrder = sortOrder,
                Status = ImageStatus.PendingUpload
            });
        }

        /// <summary>
        /// ✅ 简化: 清除所有待处理的图片操作(提交成功后调用)
        /// </summary>
        public void ClearPendingImages()
        {
            PendingImages?.Clear();
        }

        /// <summary>
        /// ✅ 简化: 获取待新增/替换的图片列表(Status=PendingUpload且ImageData不为空)
        /// </summary>
        public List<ImageInfo> GetPendingAddImages()
        {
            return PendingImages?.Where(p => p.Status == ImageStatus.PendingUpload && p.ImageData != null && p.ImageData.Length > 0).ToList() 
                   ?? new List<ImageInfo>();
        }

        /// <summary>
        /// ✅ 简化: 获取待删除的图片列表(Status=PendingDelete且FileId>0)
        /// </summary>
        public List<ImageInfo> GetPendingDeleteImages()
        {
            return PendingImages?.Where(p => p.Status == ImageStatus.PendingDelete && p.FileId > 0).ToList() 
                   ?? new List<ImageInfo>();
        }

        /// <summary>
        /// ✅ 简化: 获取待替换的图片列表(已废弃,统一使用GetPendingAddImages和GetPendingDeleteImages)
        /// </summary>
        [Obsolete("请使用GetPendingAddImages和GetPendingDeleteImages代替")]
        public List<ImageInfo> GetPendingReplaceImages()
        {
            // 替换操作被拆分为: 一个Delete记录 + 一个Add记录
            // 这个方法保留是为了兼容旧代码,返回空列表
            return new List<ImageInfo>();
        }

        #endregion

        /// <summary>
        /// 属性组名称, 用于多属性组合显示, 此字段不存储到数据库
        /// </summary>
        [SugarColumn(IsIgnore = true, ColumnDescription = "多属性组合")]
        [Browsable(true)]
        public string PropertyGroupName { get; set; }



        /// <summary>
        /// 多属性新组合序号, 用于多属性组合生成时的唯一标识, 此字段不存储到数据库
        /// </summary>
        [SugarColumn(IsIgnore = true, ColumnDescription = "多属性新组合序号")]
        [Browsable(true)]
        public string MultiPropertyEditorSEQ { get; set; }

        /// <summary>
        /// 产品信息显示文本
        /// </summary>
        [SugarColumn(IsIgnore = true, ColumnDescription = "产品信息")]
        [Browsable(true)]
        public string DisplayText
        {
            get
            {
                if (ProdDetailID == -1)
                    return "请选择";

                var uniqueParts = new HashSet<string>();

                if (!string.IsNullOrWhiteSpace(tb_prod?.CNName))
                    uniqueParts.Add(tb_prod.CNName);

                if (!string.IsNullOrWhiteSpace(SKU))
                    uniqueParts.Add(SKU);

                if (!string.IsNullOrWhiteSpace(tb_prod?.Model))
                    uniqueParts.Add(tb_prod.Model);

                if (!string.IsNullOrWhiteSpace(tb_prod?.Specifications))
                    uniqueParts.Add(tb_prod.Specifications);

                return string.Join("-", uniqueParts);
            }
        }
    }
}

