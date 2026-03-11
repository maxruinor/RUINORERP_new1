using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Collections.Generic;
using SourceGrid.Cells;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Lib.BusinessImage;
using RUINORERP.Lib.BusinessImage;



namespace SourceGrid.Cells.Editors
{
    /// <summary>
    ///  Web型的图片选择器, 第一列都是一样的。所以只是一个过桥。数据得以cell为单位保存
    /// 创建列时的可编辑的列的编辑器。一列共用一个编辑器
    /// 对话框选择图片文件，和内存中粘贴，以及拖入的图片处理。都是通过这个编辑器来实现的
    /// 并且保存在当前的control.tag和model中
    /// 增强版：完善图片上传前的准备工作，支持多种图片格式的处理，集成新的命名策略和缓存机制
    /// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class ImageWebPickEditor : EditorControlBase
    {
        #region 私有字段

        /// <summary>
        /// 支持的图片文件扩展名
        /// </summary>
        private static readonly string[] SupportedImageExtensions =
            { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".ico" };

        /// <summary>
        /// 最大文件大小（默认5MB）
        /// </summary>
        private const long MaxFileSizeBytes = 5 * 1024 * 1024;

        /// <summary>
        /// 最大图片尺寸（默认4096x4096）
        /// </summary>
        private const int MaxImageDimension = 4096;

        /// <summary>
        /// 是否启用图片压缩
        /// </summary>
        public bool EnableImageCompression { get; set; } = true;

        /// <summary>
        /// 图片压缩质量（1-100，默认85）
        /// </summary>
        public int CompressionQuality { get; set; } = 85;

        /// <summary>
        /// 是否自动调整超大图片尺寸
        /// </summary>
        public bool AutoResizeLargeImages { get; set; } = true;

        /// <summary>
        /// 是否启用智能图片处理
        /// </summary>
        public bool EnableSmartProcessing { get; set; } = true;

        /// <summary>
        /// 是否启用图片增强功能
        /// </summary>
        public bool EnableImageEnhancement { get; set; } = true;

        /// <summary>
        /// 当前文件ID（long型）
        /// </summary>
        public long CurrentFileIdLong { get; private set; }

        /// <summary>
        /// 获取当前编辑单元格对应的业务ID
        /// </summary>
        /// <returns>业务ID</returns>
        private long GetCurrentBusinessId()
        {
            try
            {
                // 首先尝试从ValueImageWeb模型中获取业务ID
                if (this.EditCellContext != null)
                {
                    var model = this.EditCellContext.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                    if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb && valueImageWeb.BusinessId > 0)
                    {
                        return valueImageWeb.BusinessId;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取业务ID失败: {ex.Message}");
            }
            
            // 如果无法获取业务ID，返回0
            return 0;
        }
        
        /// <summary>
        /// 获取当前编辑单元格对应的业务表名
        /// </summary>
        /// <returns>业务表名</returns>
        private string GetCurrentOwnerTableName()
        {
            try
            {
                // 首先尝试从ValueImageWeb模型中获取业务表名
                if (this.EditCellContext != null)
                {
                    var model = this.EditCellContext.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                    if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb && !string.IsNullOrEmpty(valueImageWeb.OwnerTableName))
                    {
                        return valueImageWeb.OwnerTableName;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取业务表名失败: {ex.Message}");
            }
            
            // 如果无法获取业务表名，返回空字符串
            return string.Empty;
        }
        
        /// <summary>
        /// 获取当前编辑单元格对应的关联字段名
        /// </summary>
        /// <returns>关联字段名</returns>
        private string GetCurrentRelatedField()
        {
            try
            {
                // 首先尝试从ValueImageWeb模型中获取关联字段名
                if (this.EditCellContext != null)
                {
                    var model = this.EditCellContext.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                    if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb && !string.IsNullOrEmpty(valueImageWeb.RelatedField))
                    {
                        return valueImageWeb.RelatedField;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取关联字段名失败: {ex.Message}");
            }
            
            // 如果无法获取关联字段名，返回空字符串
            return string.Empty;
        }



        #endregion

        #region 静态方法

        /// <summary>
        /// 随机数生成器
        /// </summary>
        private static readonly Random _random = new Random();

        /// <summary>
        /// 生成唯一的long型ID
        /// </summary>
        /// <returns>唯一的long型ID</returns>
        public static long GenerateUniqueLongId()
        {
            // 使用时间戳作为基础
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // 添加随机数以确保唯一性
            int randomPart = _random.Next(1000, 9999);

            // 组合时间戳和随机数
            long uniqueId = timestamp * 10000 + randomPart;

            return uniqueId;
        }

        /// <summary>
        /// 生成唯一的long型ID（带前缀）
        /// </summary>
        /// <param name="prefix">前缀数字（1-999）</param>
        /// <returns>唯一的long型ID</returns>
        public static long GenerateUniqueLongId(int prefix)
        {
            // 确保前缀在有效范围内
            if (prefix < 1)
                prefix = 1;
            else if (prefix > 999)
                prefix = 999;

            // 使用时间戳作为基础
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // 添加随机数以确保唯一性
            int randomPart = _random.Next(10, 99);

            // 组合前缀、时间戳和随机数
            long uniqueId = prefix * 10000000000000000L + timestamp * 100 + randomPart;

            return uniqueId;
        }

        #endregion



        public readonly static ImageWebPickEditor Default = new ImageWebPickEditor(typeof(string));

        #region Constructor

        private readonly ImageStateManager _stateManager;
        private readonly ImageCache _cache;

        ///web下载图片 只是显示图片名称
        public ImageWebPickEditor(Type p_Type) : base(p_Type)
        {
            // 使用单例实例初始化依赖项
            _stateManager = ImageStateManager.Instance;
            _cache = ImageCache.Instance;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="p_Type">值类型</param>
        /// <param name="stateManager">图片状态管理器</param>
        /// <param name="cache">图片缓存</param>
        public ImageWebPickEditor(Type p_Type, ImageStateManager stateManager, ImageCache cache) : base(p_Type)
        {
            _stateManager = stateManager;
            _cache = cache;
        }

        #endregion

        public System.Drawing.Image PickerImage { get; set; }



        private string _fileName = string.Empty;


        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName
        {
            get { return _fileName; }

            set
            {
                _fileName = value;
            }
        }



        /// <summary>
        /// Temp绝对路径
        /// </summary>
        public string AbsolutelocPath
        {
            get
            {
                return Path.Combine(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "temp"), _fileName);
            }
        }





        #region 添加了右键菜单
        public bool Use是否使用内置右键功能 { get; private set; } = true;

        /// <summary>
        /// 因为暂时事件无法通过属性中的数据传输，先用名称再从这里搜索来匹配
        /// </summary>
        private List<EventHandler> ContextClickList = new List<EventHandler>();
        private List<ContextMenuController> _ContextMenucCnfigurator = new List<ContextMenuController>();

        private ContextMenuStrip _ContextMenuStrip;

        /// <summary>
        /// 重写右键菜单 为了合并
        /// 如果使用内置的菜单则这里设置时就要合并处理
        /// </summary>
        public ContextMenuStrip ContextMenuStrip
        {
            get { return _ContextMenuStrip; }
            set
            {
                _ContextMenuStrip = value;
            }
        }

        /// <summary>
        /// 获取当前是否处于设计器模式
        /// </summary>
        /// <remarks>
        /// 在程序初始化时获取一次比较准确，若需要时获取可能由于布局嵌套导致获取不正确，如GridControl-GridView组合。
        /// </remarks>
        /// <returns>是否为设计器模式</returns>
        private bool GetIsDesignMode()
        {
            return (this.GetService(typeof(System.ComponentModel.Design.IDesignerHost)) == null
                || LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        }

        /// <summary>
        /// 设置右键菜单，但是对不对参数进行设置。因为是引用的，会改变值
        /// </summary>
        /// <param name="_contextMenuStrip"></param>
        public ContextMenuStrip GetContextMenu(ContextMenuStrip _contextMenuStrip = null)
        {
            // 创建一个全新的右键菜单副本
            ContextMenuStrip newContextMenuStrip = new ContextMenuStrip();

            //初始化右键菜单
            // 初始化内置右键菜单
            ContextMenuStrip internalMenu = new ContextMenuStrip();
            internalMenu.BackColor = System.Drawing.Color.FromArgb(192, 255, 255);

            //如果需要通过设计时对属性值修改。
            //则不能在这个构造函数中操作。因为这时属性值不能获取
            internalMenu.Items.Clear();

            // 合并传入的菜单和内置菜单
            if (_contextMenuStrip != null)
            {
                //外面菜单有设置则加一个分隔符
                if (_contextMenuStrip.Items.Count > 0)
                {
                    ToolStripSeparator MyTss = new ToolStripSeparator();
                    _contextMenuStrip.Items.Add(MyTss);
                }

                ToolStripItem[] ts = new ToolStripItem[_contextMenuStrip.Items.Count];
                _contextMenuStrip.Items.CopyTo(ts, 0);
                internalMenu.Items.AddRange(ts);
            }


            //或者也可以指定内置哪些生效 合并外面的？
            if (Use是否使用内置右键功能)
            {
                #region 生成内置的右键菜单

                if (ContextClickList == null)
                {
                    ContextClickList = new List<EventHandler>();
                }
                ContextClickList.Clear();
                ContextClickList.Add(ContextMenu_查看大图);
                if (_ContextMenucCnfigurator == null)
                {
                    _ContextMenucCnfigurator = new List<ContextMenuController>();
                }
                _ContextMenucCnfigurator.Clear();
                //只是初始化不重复添加
                if (_ContextMenucCnfigurator.Count == 0 && GetIsDesignMode())
                {
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【查看大图】", true, false, "ContextMenu_查看大图"));
                    _ContextMenucCnfigurator.Add(new ContextMenuController("【line】", true, true, ""));
                }
                #endregion

                foreach (var item in _ContextMenucCnfigurator)
                {
                    if (!item.IsShow)
                    {
                        continue;
                    }

                    if (item.IsSeparator)
                    {
                        ToolStripSeparator ts1 = new ToolStripSeparator();
                        internalMenu.Items.Add(ts1);
                    }
                    else
                    {
                        EventHandler ehh = ContextClickList.Find(
                            delegate (EventHandler eh)
                            {
                                return eh.Method.Name == item.ClickEventName;
                            });
                        //如果较多的外部事件也可以做一个集合
                        //if (ehh == null && item.ClickEventName == "删除选中行")
                        //{
                        //    ehh = 删除选中行;
                        //}
                        internalMenu.Items.Add(item.MenuText, null, ehh);
                    }
                }
            }
            newContextMenuStrip = internalMenu;
            // 设置最终的右键菜单
            ContextMenuStrip = newContextMenuStrip;
            return newContextMenuStrip;
        }

        private void ContextMenu_查看大图(object sender, EventArgs e)
        {
            if (PickerImage != null)
            {
                try
                {
                    // 使用增强的图片预览窗体
                    var previewForm = new ImagePreviewForm(PickerImage);
                    previewForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"预览图片时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("没有可预览的图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion



        #region Edit Control
        /// <summary>
        /// Create the editor control
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
        {
            // DevAge.Windows.Forms.TextBoxUITypeEditor editor = new DevAge.Windows.Forms.TextBoxUITypeEditor();
            DevAge.Windows.Forms.TextBoxUITypeEditorWebImage editor = new DevAge.Windows.Forms.TextBoxUITypeEditorWebImage();

            editor.BorderStyle = DevAge.Drawing.BorderStyle.None;
            //editor.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
            editor.Validator = new DevAge.ComponentModel.Validator.ValidatorImagePathTypeConverter(typeof(string));

            editor.ContextMenuStrip = GetContextMenu();
            editor.TextBox.AllowDrop = true;
            editor.TextBox.DragDrop += Editor_DragDrop;
            editor.TextBox.DragEnter += Editor_DragEnter;
            editor.TextBox.KeyDown += Editor_KeyDown;
            return editor;
        }

        private async void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            // 检查是否按下了 Ctrl+V
            if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
            {
                // 检查剪贴板中是否有图像
                if (Clipboard.ContainsImage())
                {
                    try
                    {
                        // 获取图像
                        System.Drawing.Image image = Clipboard.GetImage();
                        // 生成带扩展名的文件名
                        fileName = $"Clipboard_{DateTime.Now:yyyyMMddHHmmss}.jpg";

                        bool success = SetImageToPath(image);
                        if (success)
                        {
                            ValueType = typeof(string);
                            Control.Value = fileName;
                        }

                        // 释放剪贴板图片资源
                        image.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"从剪贴板获取图片失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (Clipboard.ContainsText())
                {
                    // 处理文本粘贴（可能是图片URL）
                    string text = Clipboard.GetText().Trim();
                    if (!string.IsNullOrEmpty(text) && (text.StartsWith("http://") || text.StartsWith("https://")))
                    {
                        // 这里可以添加从URL下载图片的逻辑
                        MessageBox.Show("暂不支持从URL直接下载图片，请下载后使用文件选择功能", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        /// <summary>
        /// 处理图片上传前的准备工作
        /// 包括：图片验证、压缩、格式转换、哈希计算、智能优化等
        /// 增强版：集成新的命名策略和智能处理功能，并与状态管理系统集成
        /// </summary>
        /// <param name="image">原始图片对象</param>
        /// <returns>处理是否成功</returns>
        private bool SetImageToPath(System.Drawing.Image image)
        {
            if (image == null)
            {
                MessageBox.Show("图片对象为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                var model = this.EditCell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                if (model == null)
                {
                    MessageBox.Show("未找到图片数据模型", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

                // 1. 图片预处理（调整尺寸、压缩、增强等）
                System.Drawing.Image processedImage = PreprocessImage(image);
                if (processedImage == null)
                {
                    return false;
                }

                // 2. 智能图片增强（可选）
                if (EnableImageEnhancement)
                {
                    processedImage = EnhanceImage(processedImage);
                }

                // 3. 转换为字节数组并压缩
                byte[] compressedBytes = CompressImage(image);
                if (compressedBytes == null || compressedBytes.Length == 0)
                {
                    MessageBox.Show("图片压缩失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 4. 验证压缩后的图片
                if (!ValidateImageBytes(compressedBytes))
                {
                    MessageBox.Show("压缩后的图片数据无效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 5. 创建ImageInfo对象，包含完整的业务信息
                var imageInfo = new ImageInfo
                {
                    OriginalFileName = fileName,
                    ImageData = compressedBytes,
                    FileExtension = Path.GetExtension(fileName),
                    FileSize = compressedBytes.Length,
                    Status = ImageStatus.PendingUpload,
                    BusinessId = GetCurrentBusinessId(),
                    OwnerTableName = GetCurrentOwnerTableName(),
                    RelatedField = GetCurrentRelatedField(),
                    Cell = this.EditCell
                };

                // 生成唯一的long型ID
                long fileIdLong = GenerateUniqueLongId();
                imageInfo.FileId = fileIdLong;

                // 保存处理后的图片数据
                valueImageWeb.CellImageBytes = compressedBytes;
                valueImageWeb.FileId = fileIdLong; // 设置FileId属性
                Control.Tag = imageInfo; // 存储完整的ImageInfo对象

                // 更新显示图片
                using (MemoryStream ms = new MemoryStream(compressedBytes))
                {
                    PickerImage = System.Drawing.Image.FromStream(ms, true);
                }

                // 清理临时图片对象
                if (processedImage != image)
                {
                    processedImage.Dispose();
                }

                // 添加到缓存
                var cache = _cache ?? ImageCache.Instance;
                if (cache != null)
                {
                    cache.AddImage(fileIdLong, PickerImage);
                }

                // 与图片状态管理系统集成
                var stateManager = _stateManager ?? ImageStateManager.Instance;
                if (stateManager != null)
                {
                    stateManager.AddImage(imageInfo);
                }

                // 更新控件值
                Control.Value = fileIdLong;
                ValueType = typeof(long);

                // 更新CurrentFileIdLong属性
                CurrentFileIdLong = fileIdLong;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理图片时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        /// <summary>
        /// 异步图片预处理：尺寸调整、格式标准化等
        /// </summary>
        /// <param name="originalImage">原始图片</param>
        /// <returns>预处理后的图片</returns>
        private async Task<System.Drawing.Image> PreprocessImageAsync(System.Drawing.Image originalImage)
        {
            return await Task.Run(() =>
            {
                return PreprocessImage(originalImage);
            });
        }

        private System.Drawing.Image PreprocessImage(System.Drawing.Image originalImage)
        {
            try
            {
                // 检查是否需要调整尺寸
                if (AutoResizeLargeImages && (originalImage.Width > MaxImageDimension || originalImage.Height > MaxImageDimension))
                {
                    return ResizeImage(originalImage, MaxImageDimension, MaxImageDimension);
                }

                // 检查图片格式，如果不是标准格式，转换为标准格式
                if (!(originalImage is Bitmap))
                {
                    return new Bitmap(originalImage);
                }

                return originalImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"图片预处理失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// 调整图片尺寸，保持宽高比
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <returns>调整后的图片</returns>
        private System.Drawing.Image ResizeImage(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            try
            {
                // 计算缩放比例
                float ratioX = (float)maxWidth / image.Width;
                float ratioY = (float)maxHeight / image.Height;
                float ratio = Math.Min(ratioX, ratioY);

                // 如果不需要缩放，直接返回原图
                if (ratio >= 1.0f)
                {
                    return image;
                }

                int newWidth = (int)(image.Width * ratio);
                int newHeight = (int)(image.Height * ratio);

                // 创建新的位图
                var newImage = new Bitmap(newWidth, newHeight);
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }

                return newImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"调整图片尺寸失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return image; // 失败时返回原图
            }
        }

        /// <summary>
        /// 验证图片字节数据
        /// </summary>
        /// <param name="imageBytes">图片字节数据</param>
        /// <returns>是否有效</returns>
        private bool ValidateImageBytes(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return false;
            }

            if (imageBytes.Length > MaxFileSizeBytes)
            {
                MessageBox.Show($"图片文件过大，最大支持 {MaxFileSizeBytes / (1024 * 1024)}MB", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                using (var ms = new MemoryStream(imageBytes))
                using (var testImage = System.Drawing.Image.FromStream(ms))
                {
                    return testImage.Width > 0 && testImage.Height > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证图片文件扩展名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否为支持的格式</returns>
        private bool IsSupportedImageFormat(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            string extension = Path.GetExtension(filePath)?.ToLowerInvariant();
            return !string.IsNullOrEmpty(extension) && SupportedImageExtensions.Contains(extension);
        }

        private void Editor_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private async void Editor_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    string filePath = files[0];
                    if (IsSupportedImageFormat(filePath))
                    {
                        try
                        {
                            var img = System.Drawing.Image.FromFile(filePath);
                            // 保留完整的文件名（包含扩展名）
                            fileName = Path.GetFileName(filePath);

                            bool success = SetImageToPath(img);
                            if (success)
                            {
                                ValueType = typeof(string);
                                Control.Value = fileName;
                            }

                            // 释放图片资源
                            img.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"加载图片文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string supportedFormats = string.Join(", ", SupportedImageExtensions);
                        MessageBox.Show($"只支持以下图片格式：{supportedFormats}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the control used for editing the cell.
        /// </summary>
        public new DevAge.Windows.Forms.TextBoxUITypeEditorWebImage Control
        {
            get
            {
                return (DevAge.Windows.Forms.TextBoxUITypeEditorWebImage)base.Control;
            }
        }


        #endregion


        public override bool SetCellTagValue(CellContext cellContext, object p_NewTagValue)
        {
            Control.Tag = p_NewTagValue;
            return base.SetCellTagValue(cellContext, p_NewTagValue); ;
        }

        /// <summary>
        /// This method is called just before the edit start. You can use this method to customize the editor with the cell informations.
        /// </summary>
        /// <param name="cellContext"></param>
        /// <param name="editorControl"></param>
        protected override void OnStartingEdit(CellContext cellContext, Control editorControl)
        {
            base.OnStartingEdit(cellContext, editorControl);
            DevAge.Windows.Forms.TextBoxUITypeEditorWebImage l_TxtBox = (DevAge.Windows.Forms.TextBoxUITypeEditorWebImage)editorControl;
            l_TxtBox.TextBox.WordWrap = cellContext.Cell.View.WordWrap;
            l_TxtBox.TextBox.TextAlign = DevAge.Windows.Forms.Utilities.ContentToHorizontalAlignment(cellContext.Cell.View.TextAlignment);
            //to set the scroll of the textbox to the initial position (otherwise the textbox use the previous scroll position)
            l_TxtBox.TextBox.SelectionStart = 0;
            l_TxtBox.TextBox.SelectionLength = 0;
        }

        /// <summary>
        /// 对话框会直接到这里
        /// 修复：正确处理相同图片在编辑后退出的情况1
        /// </summary>
        /// <returns></returns>
        public override object GetEditedValue()
        {
            var model = this.EditCell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

            // 记录编辑前的状态，用于原子性替换
            long oldFileId = valueImageWeb.FileId;
            byte[] originalImageData = valueImageWeb.ImageData;
            string originalFileName = valueImageWeb.OriginalFileName;
            
            string selectedFilePath = string.Empty;
            
            try
            {
                // 处理文件选择器选择的图片
                if (Control is DevAge.Windows.Forms.TextBoxUITypeEditorWebImage txtWebImage)
                {
                    if (!string.IsNullOrEmpty(txtWebImage.SelectedFilePath))
                    {
                        selectedFilePath = txtWebImage.SelectedFilePath;
                        fileName = Path.GetFileName(selectedFilePath);

                        // 原子性处理图片替换
                        if (!AtomicImageReplace(txtWebImage, valueImageWeb, oldFileId))
                        {
                            // 替换失败，恢复原状态
                            RestoreOriginalState(valueImageWeb, oldFileId, originalImageData, originalFileName);
                            return oldFileId > 0 ? (object)oldFileId : null;
                        }
                        
                        txtWebImage.SelectedFilePath = string.Empty; // 用完了清空
                    }
                }
                
                // 处理控件中的数据
                object val = Control.Tag;
                if (val == null)
                {
                    // 如果没有新数据且原有数据存在，保持原有数据
                    if (originalImageData != null && originalImageData.Length > 0)
                    {
                        return valueImageWeb.FileId > 0 ? (object)valueImageWeb.FileId : valueImageWeb.OriginalFileName;
                    }
                    return null;
                }
                
                if (val is System.Drawing.Image img)
                {
                    // 原子性处理图片对象替换
                    if (!AtomicImageReplace(img, valueImageWeb, oldFileId))
                    {
                        // 替换失败，恢复原状态
                        RestoreOriginalState(valueImageWeb, oldFileId, originalImageData, originalFileName);
                        return oldFileId > 0 ? (object)oldFileId : null;
                    }
                    Control.Tag = null; // 清空，让第二个单元格可以选择新的图片
                }
                else if (val is byte[] buffByte)
                {
                    // 原子性处理字节数组替换
                    if (!AtomicImageReplace(buffByte, selectedFilePath, valueImageWeb, oldFileId))
                    {
                        // 替换失败，恢复原状态
                        RestoreOriginalState(valueImageWeb, oldFileId, originalImageData, originalFileName);
                        return oldFileId > 0 ? (object)oldFileId : null;
                    }
                }
                else if (val is ImageInfo imageInfo)
                {
                    // 确保将ImageInfo添加到状态管理器
                    var stateManager = _stateManager ?? ImageStateManager.Instance;
                    if (stateManager != null)
                    {
                        stateManager.AddImage(imageInfo);
                    }
                    // 直接返回ImageInfo的FileId
                    return imageInfo.FileId;
                }
                else if (val is string)
                {
                    return val;
                }
                
                return Control.Value;
            }
            catch (Exception ex)
            {
                // 发生异常时恢复原状态
                RestoreOriginalState(valueImageWeb, oldFileId, originalImageData, originalFileName);
                
                System.Diagnostics.Debug.WriteLine($"图片替换过程中发生异常: {ex.Message}");
                return oldFileId > 0 ? (object)oldFileId : null;
            }
        }

        /// <summary>
        /// 原子性替换图片（从文件选择器）
        /// </summary>
        /// <param name="txtWebImage">文件选择器控件</param>
        /// <param name="valueImageWeb">图片数据模型</param>
        /// <param name="oldFileId">旧图片ID</param>
        /// <returns>是否替换成功</returns>
        private bool AtomicImageReplace(DevAge.Windows.Forms.TextBoxUITypeEditorWebImage txtWebImage, 
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb, long oldFileId)
        {
            try
            {
                if (string.IsNullOrEmpty(txtWebImage.SelectedFilePath)) return false;

                using (var selectedImg = System.Drawing.Image.FromFile(txtWebImage.SelectedFilePath))
                {
                    return AtomicImageReplace(selectedImg, valueImageWeb, oldFileId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"原子性图片替换失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 原子性替换图片（从Image对象）
        /// </summary>
        /// <param name="newImage">新图片对象</param>
        /// <param name="valueImageWeb">图片数据模型</param>
        /// <param name="oldFileId">旧图片ID</param>
        /// <returns>是否替换成功</returns>
        private bool AtomicImageReplace(System.Drawing.Image newImage, 
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb, long oldFileId)
        {
            try
            {
                var stateManager = _stateManager ?? ImageStateManager.Instance;
                
                // 第一步：标记旧图片为待删除（如果存在）
                if (oldFileId > 0 && stateManager != null)
                {
                    var oldImageInfo = stateManager.GetImageInfo(oldFileId);
                    if (oldImageInfo != null)
                    {
                        // 标记为待删除，但不立即删除
                        stateManager.UpdateImageStatus(oldFileId, ImageStatus.PendingDelete);
                    }
                }

                // 第二步：处理新图片
                if (!SetImageToPath(newImage))
                {
                    // 新图片处理失败，恢复旧图片状态为Normal
                    if (oldFileId > 0 && stateManager != null)
                    {
                        stateManager.UpdateImageStatus(oldFileId, ImageStatus.Normal);
                    }
                    return false;
                }

                // 第三步：新图片处理成功，此时旧图片已标记为待删除，新图片为待上传
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"原子性图片替换失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 原子性替换图片（从字节数组）
        /// </summary>
        /// <param name="buffByte">新图片字节数组</param>
        /// <param name="selectedFilePath">选择的文件路径</param>
        /// <param name="valueImageWeb">图片数据模型</param>
        /// <param name="oldFileId">旧图片ID</param>
        /// <returns>是否替换成功</returns>
        private bool AtomicImageReplace(byte[] buffByte, string selectedFilePath,
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb, long oldFileId)
        {
            try
            {
                var stateManager = _stateManager ?? ImageStateManager.Instance;
                
                // 第一步：标记旧图片为待删除（如果存在）
                if (oldFileId > 0 && stateManager != null)
                {
                    var oldImageInfo = stateManager.GetImageInfo(oldFileId);
                    if (oldImageInfo != null)
                    {
                        // 标记为待删除，但不立即删除
                        stateManager.UpdateImageStatus(oldFileId, ImageStatus.PendingDelete);
                    }
                }

                // 第二步：处理新图片数据
                string imageFileName = fileName;
                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    imageFileName = Path.GetFileName(selectedFilePath);
                }
                else if (string.IsNullOrEmpty(imageFileName))
                {
                    imageFileName = $"Image_{DateTime.Now:yyyyMMddHHmmss}.jpg";
                }
                else if (string.IsNullOrEmpty(Path.GetExtension(imageFileName)))
                {
                    imageFileName += ".jpg";
                }
                
                // 创建ImageInfo对象，包含完整的业务信息
                var imageInfo = new ImageInfo
                {
                    OriginalFileName = imageFileName,
                    ImageData = buffByte,
                    FileExtension = Path.GetExtension(imageFileName),
                    FileSize = buffByte.Length,
                    Status = ImageStatus.PendingUpload,
                    BusinessId = GetCurrentBusinessId(),
                    OwnerTableName = GetCurrentOwnerTableName(),
                    RelatedField = GetCurrentRelatedField(),
                    Cell = this.EditCell
                };
                
                // 生成唯一的long型ID
                long newFileId = GenerateUniqueLongId();
                imageInfo.FileId = newFileId;
                
                valueImageWeb.ImageData = buffByte;
                valueImageWeb.OriginalFileName = imageFileName;
                valueImageWeb.FileId = newFileId;
                CurrentFileIdLong = newFileId;
                Control.Tag = imageInfo;
                
                // 更新显示
                using (MemoryStream ms = new MemoryStream(buffByte))
                {
                    PickerImage = System.Drawing.Image.FromStream(ms, true);
                }

                // 添加到缓存
                var cache = _cache ?? ImageCache.Instance;
                if (cache != null)
                {
                    cache.AddImage(newFileId, PickerImage);
                }

                // 与图片状态管理系统集成（使用前面已声明的stateManager）
                if (stateManager != null)
                {
                    stateManager.AddImage(imageInfo);
                }

                // 第三步：新图片处理成功，此时旧图片已标记为待删除，新图片为待上传
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"原子性图片替换失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 恢复原始状态（用于替换失败时的回滚）
        /// </summary>
        /// <param name="valueImageWeb">图片数据模型</param>
        /// <param name="oldFileId">旧图片ID</param>
        /// <param name="originalImageData">原始图片数据</param>
        /// <param name="originalFileName">原始文件名</param>
        private void RestoreOriginalState(SourceGrid.Cells.Models.ValueImageWeb valueImageWeb, 
            long oldFileId, byte[] originalImageData, string originalFileName)
        {
            try
            {
                // 恢复FileId
                valueImageWeb.FileId = oldFileId;
                
                // 恢复图片数据
                if (originalImageData != null && originalImageData.Length > 0)
                {
                    valueImageWeb.ImageData = originalImageData;
                    valueImageWeb.CellImageBytes = originalImageData;
                    
                    // 恢复显示图片
                    using (MemoryStream ms = new MemoryStream(originalImageData))
                    {
                        PickerImage = System.Drawing.Image.FromStream(ms, true);
                    }
                }
                else
                {
                    // 如果原始数据为空，清空显示
                    valueImageWeb.ImageData = null;
                    valueImageWeb.CellImageBytes = null;
                    PickerImage = null;
                }
                
                // 恢复文件名
                if (!string.IsNullOrEmpty(originalFileName))
                {
                    valueImageWeb.OriginalFileName = originalFileName;
                }
                
                // 恢复控件值
                Control.Value = oldFileId > 0 ? oldFileId : (object)null;
                CurrentFileIdLong = oldFileId;
                
                // 恢复旧图片的状态为Normal（如果之前被标记为待删除）
                if (oldFileId > 0)
                {
                    var stateManager = _stateManager ?? ImageStateManager.Instance;
                    if (stateManager != null)
                    {
                        stateManager.UpdateImageStatus(oldFileId, ImageStatus.Normal);
                    }
                }
                
                System.Diagnostics.Debug.WriteLine("已恢复图片原始状态");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"恢复图片原始状态失败: {ex.Message}");
            }
        }


        //比较两个byte[]是否相同，使用哈希值来比较
        /// <summary>
        /// 图片增强处理
        /// 包括：亮度调整、对比度优化、锐化等
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <returns>增强后的图片</returns>
        private async Task<System.Drawing.Image> EnhanceImageAsync(System.Drawing.Image image)
        {
            return await Task.Run(() =>
            {
                return EnhanceImage(image);
            });
        }

        private System.Drawing.Image EnhanceImage(System.Drawing.Image image)
        {
            try
            {
                // 验证图片是否有效
                if (image == null)
                    return image;

                // 检查图片是否有效
                try
                {
                    // 尝试访问图片属性，验证图片是否有效
                    var width = image.Width;
                    var height = image.Height;
                }
                catch
                {
                    // 图片无效，直接返回
                    return image;
                }

                if (!EnableSmartProcessing)
                    return image;

                var bitmap = new Bitmap(image);

                // 简单的亮度调整
                return AdjustBrightness(bitmap, 1.1f);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"图片增强失败：{ex.Message}");
                return image;
            }
        }

        /// <summary>
        /// 异步图片压缩
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <returns>压缩后的字节数据</returns>
        private async Task<byte[]> CompressImageAsync(System.Drawing.Image image)
        {
            return await Task.Run(() =>
            {
                return CompressImage(image);
            });
        }

        private byte[] CompressImage(System.Drawing.Image image)
        {
            return ImageProcessor.CompressImage(image);
        }

        /// <summary>
        /// 异步添加到缓存
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="imageData">图片数据</param>
        private async Task AddToCacheAsync(long fileId, byte[] imageData)
        {
            try
            {
                await ImageCacheManager.Instance.GetImageAsync(
                    fileId,
                    async (id) => await Task.FromResult(imageData)
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"添加到缓存失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 调整图片亮度
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="brightness">亮度系数（1.0为原始值）</param>
        /// <returns>调整后的图片</returns>
        private System.Drawing.Image AdjustBrightness(Bitmap image, float brightness)
        {
            var result = new Bitmap(image.Width, image.Height);
            using (var graphics = Graphics.FromImage(result))
            {
                using (var attributes = new System.Drawing.Imaging.ImageAttributes())
                {
                    var matrix = new System.Drawing.Imaging.ColorMatrix
                    {
                        Matrix00 = brightness,
                        Matrix11 = brightness,
                        Matrix22 = brightness,
                        Matrix33 = 1.0f,
                        Matrix44 = 1.0f
                    };

                    attributes.SetColorMatrix(matrix);
                    graphics.DrawImage(image, new Rectangle(0, 0, result.Width, result.Height),
                        0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return result;
        }

        /// <summary>
        /// 比较两个byte[]是否相同，使用哈希值来比较
        /// </summary>
        /// <param name="hash1"></param>
        /// <param name="hash2"></param>
        /// <returns></returns>
        public static bool AreHashesEqual(string hash1, string hash2)
        {
            //如果哈希值为空，认为两个byte[]相同
            if (string.IsNullOrEmpty(hash1) && string.IsNullOrEmpty(hash2))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(hash1) && string.IsNullOrEmpty(hash2))
            {
                return false;
            }
            if (string.IsNullOrEmpty(hash1) && !string.IsNullOrEmpty(hash2))
            {
                return false;
            }
            //如果哈希值相同，则认为两个byte[]相同
            return ImageHashHelper.AreHashesEqual(hash1, hash2);
        }


        public byte[] GetByteImage(System.Drawing.Image img)
        {
            byte[] bt = null;
            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);
                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Bmp);//将图像以指定的格式存入缓存内存流
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;//设置留的初始位置
                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
                }
            }
            return bt;
        }
        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public override void SetEditValue(object editValue)
        {
            Control.Value = editValue;
            Control.TextBox.SelectAll();
        }

        /// <summary>
        /// Used to returns the display string for a given value. In this case return null.
        /// </summary>
        /// <param name="p_Value"></param>
        /// <returns></returns>
        public override string ValueToDisplayString(object p_Value)
        {
            return null;
        }



        protected override void OnSendCharToEditor(char key)
        {
            //No implementation
        }

        public override object GetEditedTagValue()
        {
            return Control.Tag;
        }

        /// <summary>
        /// 与图片状态管理系统集成
        /// 将图片注册到状态管理器，统一管理上传队列
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        private void RegisterImageWithStateManager(ImageInfo imageInfo)
        {
            try
            {
                // 获取当前单元格上下文
                if (this.EditCell != null && this.EditCell is SourceGrid.Cells.Cell cell)
                {
                    // 直接调用ImageStateManager注册图片，确保与删除时使用相同的方式
                    var stateManager = _stateManager ?? ImageStateManager.Instance;
                    if (stateManager != null)
                    {
                        stateManager.AddImage(imageInfo);
                    }

                    // 获取单元格位置信息（通过EditCellContext）
                    if (this.EditCellContext != null && !this.EditCellContext.IsEmpty())
                    {
                        var grid = this.EditCellContext.Grid;
                        var position = this.EditCellContext.Position;
                        if (grid != null && !position.IsEmpty())
                        {
                            System.Diagnostics.Debug.WriteLine($"[ImageStateManager] 单元格位置: Row={position.Row}, Column={position.Column}, ImageId={imageInfo.FileId}");
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"[ImageStateManager] 图片已注册为待上传状态: {imageInfo.FileId}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[ImageStateManager] 无法获取单元格上下文，状态注册失败");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ImageStateManager] 状态管理器注册失败: {ex.Message}");
            }
        }

    }




}
