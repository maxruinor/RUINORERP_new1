﻿using System;
using System.IO;
using SourceGrid.Cells.Editors;

namespace SourceGrid.Cells.Models
{
    public class NullValueModel : IValueModel
    {
        public readonly static NullValueModel Default = new NullValueModel();

        /// <summary>
        /// Constructor
        /// </summary>
        public NullValueModel()
        {
        }
        #region IModel Members
        public object GetValue(CellContext cellContext)
        {
            return null;
        }

        public void SetValue(CellContext cellContext, object p_Value)
        {
            throw new ApplicationException("This model doesn't support editing");
        }
        public string GetDisplayText(CellContext cellContext)
        {
            return null;
        }

        public object GetTagValue(CellContext cellContext)
        {
            throw new NotImplementedException();
        }

        public void SetTagValue(CellContext cellContext, object p_Value)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// A model that contains the value of cell. Usually used for a Real Cell or cells with a static text.
    /// </summary>
    public class ValueModel : IValueModel
    {
        public ValueModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val">value to set for this model</param>
        public ValueModel(object val)
        {
            m_Value = val;
        }

        private object m_Value;
        private object m_TagValue;
        #region IModel Members

        public object GetValue(CellContext cellContext)
        {
            return m_Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="newValue">new value of this model</param>
        public void SetValue(CellContext cellContext, object newValue)
        {
            // 如果新值与当前值相同，或者新值为 null 且当前值也是 null，不做任何操作
            //if (newValue == null ? m_Value == null : newValue.Equals(m_Value))
            //{
            //    return;
            //}
            //if (newValue == null)
            //{
            //    m_Value = null;
            //    return;
            //}
            //if (!string.IsNullOrEmpty(newValue.ToString()))
            //{
            //    if (IsNewValueEqual(newValue) == true)
            //        return;
            //}
            if (IsNewValueEqual(newValue) == true)
                return;
            ValueChangeEventArgs valArgs = new ValueChangeEventArgs(m_Value, newValue);
            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);
            m_Value = valArgs.NewValue;
            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
        }




        public bool IsNewValueEqual(object newValue)
        {

            if (newValue == m_Value)
                return true;
            object valueModel = m_Value;
            if (valueModel == null)
                valueModel = string.Empty;
            if (newValue == null)
                newValue = string.Empty;
            return newValue.Equals(valueModel);
        }


        public bool IsNewTagValueEqual(object newTagValue)
        {
            if (newTagValue == m_TagValue)
                return true;
            object valueModel = m_TagValue;
            if (valueModel == null)
                valueModel = string.Empty;
            if (newTagValue == null)
                newTagValue = string.Empty;

            return newTagValue.Equals(valueModel);
        }


        public object GetTagValue(CellContext cellContext)
        {
            return m_TagValue;
        }

        /// <summary>
        /// by watson TODO
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="p_Value"></param>
        public void SetTagValue(CellContext cellContext, object p_Value)
        {
            if (IsNewTagValueEqual(p_Value) == true)
                return;
            ValueChangeEventArgs valArgs = new ValueChangeEventArgs(m_Value, p_Value);
            //if (cellContext.Grid != null)
            //    cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);
            m_TagValue = p_Value;
            if (cellContext.Grid != null)
                cellContext.Grid.Controller.OnTagValueChanged(cellContext, EventArgs.Empty);
        }
        #endregion
    }

    /// <summary>
    /// CheckBox model.
    /// </summary>
    public class CheckBox : ICheckBox
    {
        #region ICheckBox Members

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        public CheckBoxStatus GetCheckBoxStatus(CellContext cellContext)
        {
            bool enableEdit = false;
            if (cellContext.Cell.Editor != null && cellContext.Cell.Editor.EnableEdit)
                enableEdit = true;

            object val = cellContext.Cell.Model.ValueModel.GetValue(cellContext);
            if (val == null)
                return new CheckBoxStatus(enableEdit, DevAge.Drawing.CheckBoxState.Undefined, m_Caption);
            else if (val is bool)
                return new CheckBoxStatus(enableEdit, (bool)val, m_Caption);
            else if (val.ToString().ToLower() == "true" || val.ToString().ToLower() == "false")
            {
                return new CheckBoxStatus(enableEdit, bool.Parse(val.ToString().ToLower()), m_Caption);
            }
            else

                throw new SourceGridException("Cell value not supported for this cell. Expected bool value or null.");
        }
        /// <summary>
        /// Set the checked value
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="pChecked"></param>
        public void SetCheckedValue(CellContext cellContext, bool? pChecked)
        {
            if (cellContext.Cell.Editor != null && cellContext.Cell.Editor.EnableEdit)
                cellContext.Cell.Editor.SetCellValue(cellContext, cellContext.Tag, pChecked);
        }
        #endregion

        private string m_Caption = null;
        public string Caption
        {
            get { return m_Caption; }
            set { m_Caption = value; }
        }
    }

    public class SortableHeader : ISortableHeader
    {
        #region ISortableHeader Members

        public SortStatus GetSortStatus(CellContext cellContext)
        {
            return m_SortStatus;
        }

        public void SetSortMode(CellContext cellContext, DevAge.Drawing.HeaderSortStyle pStyle)
        {
            m_SortStatus.Style = pStyle;
        }

        #endregion

        private SortStatus m_SortStatus = new SortStatus(DevAge.Drawing.HeaderSortStyle.None, null);
        public SortStatus SortStatus
        {
            get { return m_SortStatus; }
            set { m_SortStatus = value; }
        }
    }

    public class ToolTip : IToolTipText
    {
        #region IToolTipText Members

        public string GetToolTipText(CellContext cellContext)
        {
            if (string.IsNullOrEmpty(m_ToolTipText) && !cellContext.IsEmpty())
                return cellContext.DisplayText;
            return m_ToolTipText;
        }

        #endregion

        private string m_ToolTipText;
        public string ToolTipText
        {
            get { return m_ToolTipText; }
            set { m_ToolTipText = value; }
        }
    }

    /// <summary>
    /// 原来的图片模型
    /// </summary>
    public class CellImageModel : IImage
    {
        public CellImageModel()
        {
        }

        public CellImageModel(System.Drawing.Image image)
        {
            mImage = image;
        }

        #region IImage Members
        /// <summary>
        /// Get the image of the specified cell. 
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Image GetImage(CellContext cellContext)
        {
            return mImage;
        }
        #endregion

        private System.Drawing.Image mImage;
        public System.Drawing.Image ImageValue
        {
            get { return mImage; }
            set { mImage = value; }
        }
    }

    /// <summary>
    /// Model that implements the IImage interface, used to read the Image directly from the Value of the cell.
    /// 用于直接保存到数据库这种情况的图片。
    /// </summary>
    public class ValueImage : IImage
    {
        public static readonly ValueImage Default = new ValueImage();

        private DevAge.ComponentModel.Validator.ValidatorTypeConverter imageConverter = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
        #region IImage Members

        public System.Drawing.Image GetImage(CellContext cellContext)
        {
            object val = cellContext.Cell.Model.ValueModel.GetValue(cellContext);
            return (System.Drawing.Image)imageConverter.ObjectToValue(val);
        }
        #endregion
    }


    /// <summary>
    /// 为了对应 远程图片单元格实现的。里面有一个转换器后面再看否也重新写一个。
    /// 1
    /// </summary>
    public class ValueImageWeb : IImageWeb
    {
        #region 核心图片数据属性
        
        private byte[] _imageData;
        /// <summary>
        /// 图片的二进制数据
        /// 设置时会自动计算并更新哈希值和其他相关属性
        /// </summary>
        public byte[] ImageData 
        { 
            get { return _imageData; }
            set 
            { 
                _imageData = value;
                UpdateImageProperties();
            } 
        }
        
        /// <summary>
        /// 图片内容的哈希值，用于快速比较图片是否发生变化
        /// </summary>
        public string ContentHash { get; private set; } = string.Empty;
        
        #endregion
        
        #region 文件信息属性
        
        /// <summary>
        /// 文件ID，用于识别和分辨图片的唯一性
        /// </summary>
        public long FileId { get; set; }
        
        private string _originalFileName = string.Empty;
        /// <summary>
        /// 原始文件名（包含扩展名）
        /// </summary>
        public string OriginalFileName
        {
            get { return _originalFileName; }
            set 
            { 
                _originalFileName = value ?? string.Empty;
                UpdateFileExtension();
            }
        }
        
        private string _fileExtension = string.Empty;
        /// <summary>
        /// 文件扩展名（如 .jpg, .png）
        /// </summary>
        public string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value ?? string.Empty; }
        }
        
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; private set; }
        
        /// <summary>
        /// 文件类型（MIME类型）
        /// </summary>
        public string FileType { get; set; } = string.Empty;
        
        #endregion
        
        #region 存储路径属性
        
        private string _storagePath = string.Empty;
        /// <summary>
        /// 存储路径（相对路径）
        /// </summary>
        public string StoragePath
        {
            get { return _storagePath; }
            set { _storagePath = value ?? string.Empty; }
        }
        
        private string _storageFileName = string.Empty;
        /// <summary>
        /// 存储文件名（不包含路径）
        /// </summary>
        public string StorageFileName
        {
            get { return _storageFileName; }
            set { _storageFileName = value ?? string.Empty; }
        }
        
        #endregion
        
        #region 兼容性属性（为向后兼容保留）
        
        /// <summary>
        /// 兼容性属性：图片的原始文件名
        /// </summary>
        public string CellImageHashName
        {
            get { return OriginalFileName; }
            set { OriginalFileName = value ?? string.Empty; }
        }
        
        /// <summary>
        /// 兼容性属性：图片的二进制数据
        /// </summary>
        public byte[] CellImageBytes
        {
            get { return ImageData; }
            set { ImageData = value; }
        }
        
        #endregion
        
        #region 构造函数
        
        public static readonly ValueImageWeb Default = new ValueImageWeb();
        
        public ValueImageWeb()
        {
        }
        
        public ValueImageWeb(byte[] imageData, string originalFileName = null)
        {
            ImageData = imageData;
            OriginalFileName = originalFileName ?? $"Image_{DateTime.Now:yyyyMMddHHmmss}";
        }
        
        #endregion
        
        #region 公共方法
        
        /// <summary>
        /// 获取图片内容的哈希值
        /// </summary>
        /// <returns></returns>
        public string GetImageNewHash()
        {
            return ContentHash ?? string.Empty;
        }
        
        /// <summary>
        /// 手动设置图片哈希值（通常不需要直接调用）
        /// </summary>
        /// <param name="paraNewHash"></param>
        public void SetImageNewHash(string paraNewHash)
        {
            ContentHash = paraNewHash ?? string.Empty;
        }
        
        /// <summary>
        /// 判断图片数据是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return ImageData == null || ImageData.Length == 0;
        }
        
        /// <summary>
        /// 清空所有图片数据
        /// </summary>
        public void Clear()
        {
            _imageData = null;
            ContentHash = string.Empty;
            FileSize = 0;
            OriginalFileName = string.Empty;
            FileExtension = string.Empty;
            StoragePath = string.Empty;
            StorageFileName = string.Empty;
            FileId = 0;
        }
        
        /// <summary>
        /// 克隆当前对象
        /// </summary>
        /// <returns></returns>
        public ValueImageWeb Clone()
        {
            var clone = new ValueImageWeb
            {
                FileId = this.FileId,
                OriginalFileName = this.OriginalFileName,
                FileExtension = this.FileExtension,
                FileSize = this.FileSize,
                FileType = this.FileType,
                StoragePath = this.StoragePath,
                StorageFileName = this.StorageFileName,
                ContentHash = this.ContentHash
            };
            
            if (this.ImageData != null)
            {
                clone.ImageData = new byte[this.ImageData.Length];
                Buffer.BlockCopy(this.ImageData, 0, clone.ImageData, 0, this.ImageData.Length);
            }
            
            return clone;
        }
        
        #endregion
        
        #region 私有辅助方法
        
        /// <summary>
        /// 更新图片相关属性（哈希值、文件大小等）
        /// </summary>
        private void UpdateImageProperties()
        {
            if (_imageData != null && _imageData.Length > 0)
            {
                ContentHash = ImageHashHelper.GenerateHash(_imageData);
                FileSize = _imageData.Length;
            }
            else
            {
                ContentHash = string.Empty;
                FileSize = 0;
            }
        }
        
        /// <summary>
        /// 根据原始文件名更新文件扩展名
        /// </summary>
        private void UpdateFileExtension()
        {
            if (!string.IsNullOrEmpty(_originalFileName))
            {
                _fileExtension = Path.GetExtension(_originalFileName)?.ToLowerInvariant() ?? string.Empty;
            }
            else
            {
                _fileExtension = string.Empty;
            }
        }
        
        #endregion
        
        //这一行确定了这个值的类型，所以这里不用再写一个转换器了
        private DevAge.ComponentModel.Validator.ValidatorTypeConverter imageConverter =
            new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(string));
        #region IImage Members

        /// <summary>
        /// 获取图片,方式 要变为下载 或本地缓存 获取
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        public System.Drawing.Image GetImage(CellContext cellContext)
        {
            object val = cellContext.Cell.Model.ValueModel.GetValue(cellContext);
            return (System.Drawing.Image)imageConverter.ObjectToValue(val);
        }
        #endregion
    }

}


