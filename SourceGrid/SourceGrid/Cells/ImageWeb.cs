using System;
using System.Windows.Forms;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Models;
using SourceGrid.Cells.Views;

namespace SourceGrid.Cells
{
    /// <summary>
    /// 远程图片单元格
    /// 增强版：支持完整的图片处理流程
    /// 与现有的非远程图片单元格完全兼容
    /// </summary>
    public class ImageWebCell : Cell
    {
        #region Constructor

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ImageWebCell() : this(null)
        {
        }

        /// <summary>
        /// 使用值创建图片单元格
        /// 支持多种值类型：Image对象、字节数组、文件路径字符串等
        /// </summary>
        /// <param name="value">单元格值</param>
        public ImageWebCell(object value) : base(value)
        {
            // 移除旧的图片模型
            Model.RemoveModel(Model.FindModel(typeof(Models.Image)));

            // 添加ValueImageWeb模型以支持远程图片功能
            var valueImageWeb = new ValueImageWeb();
            Model.AddModel(valueImageWeb);

            // 设置增强的图片编辑器
            if (Editor == null)
            {
                Editor = new ImageWebPickEditor(typeof(string))
                {
                    EditableMode = EditableMode.Default,
                    AllowNull = true,
                    EnableEdit = true
                };
            }

            // 设置远程图片视图以支持异步加载和显示
            if (View == null)
            {
                View = new RemoteImageView();
            }

            // 如果传入了初始值，进行初始化
            if (value != null)
            {
                InitializeCellValue(value, valueImageWeb);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化单元格值
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="valueImageWeb">图片Web模型</param>
        private void InitializeCellValue(object value, ValueImageWeb valueImageWeb)
        {
            try
            {
                // 处理不同的值类型
                if (value is System.Drawing.Image image)
                {
                    // Image对象
                    byte[] imageBytes = ImageProcessor.ImageToByteArray(image);
                    string hash = ImageHashHelper.GenerateHash(imageBytes);
                    
                    valueImageWeb.SetImageNewHash(hash);
                    valueImageWeb.CellImageBytes = imageBytes;
                    valueImageWeb.CellImageHashName = hash;
                }
                else if (value is byte[] imageBytes)
                {
                    // 字节数组
                    string hash = ImageHashHelper.GenerateHash(imageBytes);
                    
                    valueImageWeb.SetImageNewHash(hash);
                    valueImageWeb.CellImageBytes = imageBytes;
                    valueImageWeb.CellImageHashName = hash;
                }
                else if (value is string stringValue)
                {
                    // 字符串（可能是文件路径或URL）
                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        // 尝试作为文件路径处理
                        if (System.IO.File.Exists(stringValue))
                        {
                            using (var fileImage = System.Drawing.Image.FromFile(stringValue))
                            {
                                byte[] fileImageBytes = ImageProcessor.ImageToByteArray(fileImage);
                                string hash = ImageHashHelper.GenerateHash(fileImageBytes);
                                
                                valueImageWeb.SetImageNewHash(hash);
                                valueImageWeb.CellImageBytes = fileImageBytes;
                                valueImageWeb.CellImageHashName = hash;
                            }
                        }
                        else
                        {
                            // 作为文件名或ID处理
                            valueImageWeb.CellImageHashName = stringValue;
                        }
                    }
                }
                
                // 更新单元格的显示值
                Value = valueImageWeb.CellImageHashName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化图片单元格值时出错: {ex.Message}");
            }
        }

        #endregion

        #region 公共属性和方法

        /// <summary>
        /// 获取单元格中的图片Web模型
        /// </summary>
        public ValueImageWeb ImageWebModel
        {
            get
            {
                return Model.FindModel(typeof(ValueImageWeb)) as ValueImageWeb;
            }
        }

        /// <summary>
        /// 获取单元格中的图片数据
        /// </summary>
        public byte[] ImageData
        {
            get
            {
                var model = ImageWebModel;
                return model?.CellImageBytes;
            }
        }

        /// <summary>
        /// 设置图片数据
        /// </summary>
        /// <param name="imageData">图片字节数据</param>
        public void SetImageData(byte[] imageData)
        {
            try
            {
                if (imageData == null || imageData.Length == 0)
                    return;

                var model = ImageWebModel;
                if (model != null)
                {
                    string hash = ImageHashHelper.GenerateHash(imageData);
                    model.SetImageNewHash(hash);
                    model.CellImageBytes = imageData;
                    model.CellImageHashName = hash;
                    Value = hash;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"设置图片数据时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 清除图片数据
        /// </summary>
        public void ClearImageData()
        {
            try
            {
                var model = ImageWebModel;
                if (model != null)
                {
                    model.CellImageBytes = null;
                    model.CellImageHashName = string.Empty;
                    model.SetImageNewHash(string.Empty);
                    Value = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"清除图片数据时出错: {ex.Message}");
            }
        }

        #endregion
    }
}