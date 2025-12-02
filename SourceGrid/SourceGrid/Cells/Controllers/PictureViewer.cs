using SourceGrid.Cells.Editors;
using System;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;

namespace SourceGrid.Cells.Controllers
{
    /// <summary>
    /// 图片预览控制器，鼠标移动到单元格时显示图片预览
    /// 增强版：支持懒加载、缓存机制和异步图片加载
    /// </summary>
    public class PictureViewerController : ControllerBase
    {
        /// <summary>
        /// 静态预览窗体实例，用于快速显示图片
        /// </summary>
        private static ImagePreviewForm _previewForm;

        /// <summary>
        /// 预加载任务
        /// </summary>
        private Task<Image> _preloadTask;

        /// <summary>
        /// 当前单元格上下文
        /// </summary>
        private CellContext _currentContext;

        /// <summary>
        /// 预览图片延迟时间（毫秒）
        /// </summary>
        public int PreviewDelayMs { get; set; } = 500;

        /// <summary>
        /// 是否启用预加载
        /// </summary>
        public bool EnablePreload { get; set; } = true;

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        public bool EnableCache { get; set; } = true;

        /// <summary>
        /// 获取预览窗体实例（延迟创建）
        /// </summary>
        private static ImagePreviewForm PreviewForm
        {
            get
            {
                if (_previewForm == null || _previewForm.IsDisposed)
                {
                    _previewForm = new ImagePreviewForm();
                }
                return _previewForm;
            }
        }

        #region IBehaviorModel Members


        public override void OnMouseEnter(CellContext sender, EventArgs e)
        {
            base.OnMouseEnter(sender, e);
            
            // 取消之前的预加载任务
            _preloadTask?.Dispose();
            
            // 保存当前上下文
            _currentContext = sender;
            
            if (PreviewDelayMs > 0)
            {
                // 延迟预览，避免快速移动鼠标时频繁触发
                Task.Delay(PreviewDelayMs).ContinueWith(async (t) =>
                {
                    if (!_currentContext.IsEmpty() && EnablePreload)
                    {
                        await ApplyPreviewImageAsync(_currentContext, e);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                // 立即预览
                _ = Task.Run(async () => await ApplyPreviewImageAsync(sender, e));
            }
        }

        public override void OnMouseLeave(CellContext sender, EventArgs e)
        {
            base.OnMouseLeave(sender, e);
            
            // 取消预加载任务
            _preloadTask?.Dispose();
            _preloadTask = null;
            
            ResetPreviewImage(sender, e);
        }
        #endregion

        private string mToolTipTitle = string.Empty;
        public string ToolTipTitle
        {
            get { return mToolTipTitle; }
            set { mToolTipTitle = value; }
        }



        private System.Drawing.Color mBackColor = System.Drawing.Color.Empty;
        public System.Drawing.Color BackColor
        {
            get { return mBackColor; }
            set { mBackColor = value; }
        }
        private System.Drawing.Color mForeColor = System.Drawing.Color.Empty;
        public System.Drawing.Color ForeColor
        {
            get { return mForeColor; }
            set { mForeColor = value; }
        }
        /// <summary>
        /// 异步显示图片预览
        /// </summary>
        public virtual async Task ApplyPreviewImageAsync(CellContext sender, EventArgs e)
        {
            try
            {
                System.Drawing.Image image = await GetImageAsync(sender);
                
                if (image != null)
                {
                    // 在UI线程中更新预览
                    var previewForm = PreviewForm;
                    if (previewForm.InvokeRequired)
                    {
                        previewForm.Invoke((Action)(() =>
                        {
                            try
                            {
                                previewForm.SetImage(image);

                                // 获取鼠标当前位置并稍微偏移，避免完全遮挡鼠标
                                System.Drawing.Point cursorPosition = System.Windows.Forms.Cursor.Position;
                                cursorPosition.Offset(20, 20);

                                // 设置预览窗体的位置
                                previewForm.Location = cursorPosition;
                                previewForm.Text = "图片预览";

                                // 显示窗体但不激活焦点
                                previewForm.Show();
                                previewForm.BringToFront();
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"显示预览失败: {ex.Message}");
                            }
                        }));
                    }
                    else
                    {
                        try
                        {
                            previewForm.SetImage(image);

                            // 获取鼠标当前位置并稍微偏移，避免完全遮挡鼠标
                            System.Drawing.Point cursorPosition = System.Windows.Forms.Cursor.Position;
                            cursorPosition.Offset(20, 20);

                            // 设置预览窗体的位置
                            previewForm.Location = cursorPosition;
                            previewForm.Text = "图片预览";

                            // 显示窗体但不激活焦点
                            previewForm.Show();
                            previewForm.BringToFront();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"显示预览失败: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取预览图片失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 同步显示图片预览（保持向后兼容）
        /// </summary>
        public virtual void ApplyPreviewImage(CellContext sender, EventArgs e)
        {
            _ = ApplyPreviewImageAsync(sender, e);
        }

        /// <summary>
        /// 异步获取图片
        /// </summary>
        private async Task<System.Drawing.Image> GetImageAsync(CellContext sender)
        {
            if (sender.Value == null)
                return null;

            // 如果值已经是Image对象，直接返回
            if (sender.Value is System.Drawing.Image directImage)
                return directImage;

            // 处理字节数组
            if (sender.Value is byte[] bytes)
            {
                return await Task.Run(() =>
                {
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        return System.Drawing.Image.FromStream(ms, true);
                    }
                });
            }

            // 处理字符串（文件ID或文件路径）
            if (sender.Value is string stringValue)
            {
                if (EnableCache && stringValue.Contains("/"))
                {
                    // 看起来像文件ID，使用缓存加载
                    return await ImageCacheManager.Instance.GetImageAsync(
                        stringValue, 
                        async (fileId) => await LoadImageFromSourceAsync(fileId)
                    );
                }
                else if (File.Exists(stringValue))
                {
                    // 本地文件路径
                    return await Task.Run(() => System.Drawing.Image.FromFile(stringValue));
                }
            }

            return null;
        }

        /// <summary>
        /// 从源加载图片数据
        /// </summary>
        private async Task<byte[]> LoadImageFromSourceAsync(string fileId)
        {
            try
            {
                // 这里应该根据实际的图片源来实现加载逻辑
                // 可能是从服务器、数据库或文件系统加载
                
                // 检查是否为临时文件
                if (fileId.StartsWith("TEMP_"))
                {
                    // 从临时目录加载
                    var tempPath = Path.Combine(Path.GetTempPath(), "ImageCache", fileId);
                    if (File.Exists(tempPath))
                    {
                        return await Task.Run(() => File.ReadAllBytes(tempPath));
                    }
                }
                else
                {
                    // 从ValueImageWeb模型获取
                    if (_currentContext.Cell.Editor is ImageWebPickEditor webPicker)
                    {
                        var model = _currentContext.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                        if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                        {
                            return valueImageWeb.CellImageBytes;
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载图片失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 隐藏图片预览
        /// </summary>
        protected virtual void ResetPreviewImage(CellContext sender, EventArgs e)
        {
            if (_previewForm != null && !_previewForm.IsDisposed)
            {
                _previewForm.Hide();
            }
        }
    }
}
