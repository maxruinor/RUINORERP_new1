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
        /// 当前文件ID
        /// </summary>
        public string CurrentFileId { get; private set; }

        /// <summary>
        /// 是否使用新的命名策略
        /// </summary>
        public bool UseNewNamingStrategy { get; set; } = true;

        #endregion



        public readonly static ImageWebPickEditor Default = new ImageWebPickEditor(typeof(string));

        #region Constructor
        
        ///web下载图片 只是显示图片名称
        public ImageWebPickEditor(Type p_Type) : base(p_Type)
        {

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
                        fileName = $"Clipboard_{DateTime.Now:yyyyMMddHHmmss}";
                        
                        bool success = await SetImageToPathAsync(image);
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
        /// 增强版：集成新的命名策略和智能处理功能
        /// </summary>
        /// <param name="image">原始图片对象</param>
        /// <returns>处理是否成功</returns>
        private async Task<bool> SetImageToPathAsync(System.Drawing.Image image)
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
                System.Drawing.Image processedImage = await PreprocessImageAsync(image);
                if (processedImage == null)
                {
                    return false;
                }

                // 2. 智能图片增强（可选）
                if (EnableImageEnhancement)
                {
                    processedImage = await EnhanceImageAsync(processedImage);
                }

                // 3. 转换为字节数组并压缩
                byte[] compressedBytes = await CompressImageAsync(processedImage);
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

                // 5. 生成文件ID（使用新策略）
                string newHash = ImageHashHelper.GenerateHash(compressedBytes);
                string fileId;
                
                if (UseNewNamingStrategy)
                {
                    fileId = ImageNamingStrategy.GenerateUniqueFileId(compressedBytes, fileName);
                    CurrentFileId = fileId;
                }
                else
                {
                    // 使用原有策略，保持兼容性
                    fileId = valueImageWeb.realName + "-" + newHash;
                }

                // 6. 检查是否需要更新
                if (!AreHashesEqual(valueImageWeb.GetImageNewHash(), newHash))
                {
                    // 7. 保存处理后的图片数据
                    valueImageWeb.SetImageNewHash(newHash);
                    valueImageWeb.CellImageBytes = compressedBytes;
                    Control.Tag = compressedBytes;

                    // 8. 更新显示图片
                    using (MemoryStream ms = new MemoryStream(compressedBytes))
                    {
                        PickerImage = System.Drawing.Image.FromStream(ms, true);
                    }

                    // 9. 清理临时图片对象
                    if (processedImage != image)
                    {
                        processedImage.Dispose();
                    }

                    // 10. 添加到缓存
                    if (UseNewNamingStrategy)
                    {
                        await AddToCacheAsync(fileId, compressedBytes);
                    }
                }

                // 11. 更新控件值
                Control.Value = UseNewNamingStrategy ? fileId : valueImageWeb.CellImageHashName;
                ValueType = typeof(string);

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
            });
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
                            fileName = Path.GetFileNameWithoutExtension(filePath);
                            
                            bool success = await SetImageToPathAsync(img);
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
        /// </summary>
        /// <returns></returns>
        public override object GetEditedValue()
        {
            var model = this.EditCell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

            //这里取值。要判断比较哪个最新通过哈希值比较一下。
            if (Control is DevAge.Windows.Forms.TextBoxUITypeEditorWebImage txtWebImage)
            {
                if (!string.IsNullOrEmpty(txtWebImage.SelectedFilePath))
                {
                    byte[] NewbuffByte = ImageProcessor.CompressImage(txtWebImage.SelectedFilePath);
                    string NewHash = ImageHashHelper.GenerateHash(NewbuffByte);

                    //看原来有不有哈希值或新旧是否相同，如果不同则更新
                    if (!AreHashesEqual(valueImageWeb.GetImageNewHash(), NewHash))
                    {
                        //将图片保存到内存中。用于后面的显示，或保存到本地临时文件夹中，或上传到服务器
                        byte[] destination = new byte[NewbuffByte.Length];
                        Buffer.BlockCopy(NewbuffByte, 0, destination, 0, NewbuffByte.Length);
                        valueImageWeb.CellImageBytes = destination;
                        valueImageWeb.SetImageNewHash(NewHash);
                        Control.Tag = destination;
                    }
                    txtWebImage.SelectedFilePath = string.Empty;//用完了清空。
                    //return valueImageWeb.CellImageName;
                }
            }
            //三种形式都将byte[]保存到tag中
            if (Control.Value != null && !string.IsNullOrEmpty(Control.Value.ToString()) && valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
            {
                Control.Tag = valueImageWeb.CellImageBytes;
            }

            object val = Control.Tag;
            if (val == null)
                return null;
            else if (val is System.Drawing.Image img)
            {
                SetImageToPathAsync(img);
                //ValueType = typeof(string);
                Control.Tag = null;//清空。让第二个单元格可以选择新的图片。

            }
            else if (val is byte[] buffByte)
            {
                //实际上比较一下。如果还是相同的图片不用赋值
                string NewHash = ImageHashHelper.GenerateHash(buffByte);
                //看原来有不有哈希值或新旧是否相同，如果不同则更新
                if (!AreHashesEqual(valueImageWeb.GetImageNewHash(), NewHash))
                {
                    byte[] NewbuffByte = val as byte[];
                    byte[] destination = new byte[NewbuffByte.Length];
                    Buffer.BlockCopy(NewbuffByte, 0, destination, 0, NewbuffByte.Length);
                    valueImageWeb.CellImageBytes = destination;
                    valueImageWeb.SetImageNewHash(NewHash);
                    Control.Tag = destination;
                    using (MemoryStream ms = new MemoryStream(NewbuffByte))
                    {
                        PickerImage = System.Drawing.Image.FromStream(ms, true);
                    }
                }
            }
            else if (val is string)
            {
                //保存为图片
                //string newIamgeFilePath = @"../temp/" + new Guid().ToString();
                //System.IO.File.Exists(newIamgeFilePath);
                //Control.Value = val;//= newIamgeFilePath;
                return val;
            }
            Control.Value = valueImageWeb.CellImageHashName;
            return Control.Value;
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
                try
                {
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
            });
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
                return ImageProcessor.CompressImage(image);
            });
        }

        /// <summary>
        /// 异步添加到缓存
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="imageData">图片数据</param>
        private async Task AddToCacheAsync(string fileId, byte[] imageData)
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
                    var matrix = new System.Drawing.Imaging.ColorMatrix {
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

        /*
        public override object GetEditedTagValue()
        {
            var model = this.EditCell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
            Control.Tag = valueImageWeb.CellImageBytes;
            if (valueImageWeb.CellImageBytes != null)
            {
                return valueImageWeb.CellImageBytes;
            }
            else
            {

            }
            object val = Control.Value;
            if (val is System.Drawing.Image)
            {
                DevAge.ComponentModel.Validator.ValidatorTypeConverter imageValidator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
                return imageValidator.ValueToObject(val, typeof(byte[]));
            }
            else if (val is byte[])
                return val;
            else if (val is string)
            {
            }
            return val;  
        }
        */
    }




}
