using DevAge.Drawing;
using DevAge.Drawing.VisualElements;
using RUINORERP.Model.BusinessImage;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// 图片单元格基类，提供图片显示、加载和管理的通用功能
    /// </summary>
    [Serializable]
    public abstract class ImageCellBase : Cell, IDisposable
    {
        #region 字段



        /// <summary>
        /// 图片缓存
        /// </summary>
        protected readonly ImageCache _cache;

        /// <summary>
        /// 图片对象
        /// </summary>
        protected System.Drawing.Image _gridImage;

        /// <summary>
        /// 是否已释放
        /// </summary>
        protected bool _disposed = false;

        /// <summary>
        /// 当前文件ID
        /// </summary>
        protected long _currentFileId;

        /// <summary>
        /// 待处理的文件ID
        /// </summary>
        protected long _pendingFileId;

        /// <summary>
        /// 图片加载任务
        /// </summary>
        protected Task<System.Drawing.Image> _imageLoadTask;

        /// <summary>
        /// 取消令牌源
        /// </summary>
        protected CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 是否启用异步加载
        /// </summary>
        protected bool _enableAsyncLoading = true;

        /// <summary>
        /// 是否启用内存优化
        /// </summary>
        protected bool _enableMemoryOptimization = true;

        /// <summary>
        /// 当前图片哈希值
        /// </summary>
        protected string _currentImageHash = string.Empty;

        /// <summary>
        /// 缓存的图片状态（默认正常）
        /// </summary>
        protected ImageStatus _cachedImageStatus = ImageStatus.Normal;

        /// <summary>
        /// 第一种背景
        /// </summary>
        protected DevAge.Drawing.VisualElements.BackgroundSolid _firstBackground;

        /// <summary>
        /// 第二种背景
        /// </summary>
        protected DevAge.Drawing.VisualElements.BackgroundSolid _secondBackground;

        /// <summary>
        /// 任务字典，用于管理并发任务
        /// </summary>
        private static readonly Dictionary<long, Task<System.Drawing.Image>> _taskDictionary = new Dictionary<long, Task<System.Drawing.Image>>();

        /// <summary>
        /// 并发锁，用于线程安全地访问任务字典
        /// </summary>
        private static readonly object _taskDictionaryLock = new object();

        #endregion

        #region 属性

        /// <summary>
        /// 是否启用异步加载
        /// </summary>
        public bool EnableAsyncLoading
        {
            get => _enableAsyncLoading;
            set => _enableAsyncLoading = value;
        }

        /// <summary>
        /// 是否启用内存优化
        /// </summary>
        public bool EnableMemoryOptimization
        {
            get => _enableMemoryOptimization;
            set => _enableMemoryOptimization = value;
        }

        /// <summary>
        /// 当前文件ID
        /// </summary>
        public long CurrentFileId
        {
            get => _currentFileId;
            protected set => _currentFileId = value;
        }

        /// <summary>
        /// 图片对象（优化内存管理）
        /// </summary>
        public System.Drawing.Image GridImage
        {
            get => _gridImage;
            set
            {
                // 释放旧图片
                if (_enableMemoryOptimization && _gridImage != null && _gridImage != value)
                {
                    _gridImage.Dispose();
                }
                _gridImage = value;
            }
        }

        #endregion


        #region 构造函数

        /// <summary>
        /// 使用默认设置
        /// </summary>
        protected ImageCellBase()
        {
            InitializeDefaults();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache">图片缓存</param>
        protected ImageCellBase(ImageCache cache)
        {
            _cache = cache;
            InitializeDefaults();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="image">图片对象</param>
        protected ImageCellBase(System.Drawing.Image image)
        {
            _gridImage = image;
            InitializeDefaults();
        }

        /// <summary>
        /// 初始化默认设置
        /// </summary>
        private void InitializeDefaults()
        {
            ElementsDrawMode = ElementsDrawMode.Covering;
            _firstBackground = new BackgroundSolid(Color.White);
            _secondBackground = new BackgroundSolid(Color.LightCyan);
            BorderLine border = new BorderLine(Color.DarkKhaki, 1);
            RectangleBorder cellBorder = new RectangleBorder(border, border);
            Border = cellBorder;
        }

        #endregion

        #region 初始化和准备

        /// <summary>
        /// 准备视图
        /// </summary>
        /// <param name="context">单元格上下文</param>
        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);
            
            // 设置背景
            if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                Background = _firstBackground;
            else
                Background = _secondBackground;

            // 处理图片数据
            ProcessImageData(context);

            // 缓存图片状态
            CacheImageStatus();
        }

        /// <summary>
        /// 处理图片数据
        /// </summary>
        /// <param name="context">单元格上下文</param>
        protected virtual void ProcessImageData(CellContext context)
        {
            if (context.Value == null)
            {
                ClearImage(context);
                return;
            }

            // 处理ValueImageWeb类型
            if (context.Value is ValueImageWeb valueImageWeb)
            {
                ProcessValueImageWeb(context, valueImageWeb);
            }
            // 处理字节数组类型
            else if (context.Value is byte[] byteArray)
            {
                ProcessByteArray(context, byteArray);
            }
            // 处理图片对象类型
            else if (context.Value is Bitmap || context.Value is System.Drawing.Image)
            {
                ProcessImageObject(context, context.Value as System.Drawing.Image);
            }
            // 处理文件ID类型
            else if (context.Value is long fileId)
            {
                ProcessFileId(context, fileId);
            }
        }

        /// <summary>
        /// 处理ValueImageWeb类型
        /// </summary>
        /// <param name="context">单元格上下文</param>
        /// <param name="valueImageWeb">ValueImageWeb对象</param>
        protected virtual void ProcessValueImageWeb(CellContext context, ValueImageWeb valueImageWeb)
        {
            if (valueImageWeb.ImageData != null && valueImageWeb.ImageData.Length > 0)
            {
                string newHash = ImageHashHelper.GenerateHash(valueImageWeb.ImageData);
                if (newHash != _currentImageHash)
                {
                    DisplayImageFromBytes(context, valueImageWeb.ImageData);
                    _currentImageHash = newHash;
                }
            }
            else if (valueImageWeb.FileId > 0)
            {
                string newHash = valueImageWeb.FileId.ToString();
                if (newHash != _currentImageHash)
                {
                    _pendingFileId = valueImageWeb.FileId;
                    CurrentFileId = _pendingFileId;
                    LoadImage(context, _pendingFileId);
                    _currentImageHash = newHash;
                }
            }
        }

        /// <summary>
        /// 处理字节数组类型
        /// </summary>
        /// <param name="context">单元格上下文</param>
        /// <param name="byteArray">字节数组</param>
        protected virtual void ProcessByteArray(CellContext context, byte[] byteArray)
        {
            string newHash = ImageHashHelper.GenerateHash(byteArray);
            if (newHash != _currentImageHash)
            {
                DisplayImageFromBytes(context, byteArray);
                _currentImageHash = newHash;
            }
        }

        /// <summary>
        /// 处理图片对象类型
        /// </summary>
        /// <param name="context">单元格上下文</param>
        /// <param name="image">图片对象</param>
        protected virtual void ProcessImageObject(CellContext context, System.Drawing.Image image)
        {
            if (GridImage != image)
            {
                GridImage = image;
                _currentImageHash = image.GetHashCode().ToString();
            }
        }

        /// <summary>
        /// 处理文件ID类型
        /// </summary>
        /// <param name="context">单元格上下文</param>
        /// <param name="fileId">文件ID</param>
        protected virtual void ProcessFileId(CellContext context, long fileId)
        {
            string newHash = fileId.ToString();
            if (newHash != _currentImageHash)
            {
                _pendingFileId = fileId;
                CurrentFileId = _pendingFileId;
                LoadImage(context, _pendingFileId);
                _currentImageHash = newHash;
            }
        }

        #endregion

        #region 图片加载

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="context">单元格上下文</param>
        /// <param name="fileId">文件ID</param>
        protected virtual void LoadImage(CellContext context, long fileId)
        {
            if (_enableAsyncLoading)
            {
                LoadImageAsync(fileId, context);
            }
            else
            {
                LoadImageSync(fileId, context);
            }
        }

        /// <summary>
        /// 异步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="context">单元格上下文</param>
        protected virtual async void LoadImageAsync(long fileId, CellContext context)
        {
            if (fileId == 0)
                return;

            // 取消之前的加载任务
            CancelPreviousTask();

            // 创建新的取消令牌源
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            // 检查任务字典中是否已有该文件的加载任务
            Task<System.Drawing.Image> existingTask = null;
            lock (_taskDictionaryLock)
            {
                if (_taskDictionary.TryGetValue(fileId, out existingTask) && !existingTask.IsCompleted)
                {
                    // 使用现有任务
                    _imageLoadTask = existingTask;
                }
                else
                {
                    // 创建新任务并添加到字典
                    _imageLoadTask = Task.Run(async () =>
                    {
                        try
                        {
                            // 检查取消令牌
                            cancellationToken.ThrowIfCancellationRequested();
                            
                            // 使用缓存管理器异步加载图片
                            return await ImageCacheManager.Instance.GetImageAsync(
                                fileId,
                                async (id) =>
                                {
                                    // 检查取消令牌
                                    cancellationToken.ThrowIfCancellationRequested();
                                    return await LoadImageDataAsync(fileId, context);
                                }
                            );
                        }
                        catch (OperationCanceledException)
                        {
                            // 任务被取消，正常退出
                            return null;
                        }
                        catch (Exception ex)
                        {
                            // 触发错误事件
                            return null;
                        }
                        finally
                        {
                            // 任务完成后从字典中移除
                            lock (_taskDictionaryLock)
                            {
                                _taskDictionary.Remove(fileId);
                            }
                        }
                    }, cancellationToken);

                    // 将新任务添加到字典
                    _taskDictionary[fileId] = _imageLoadTask;
                }
            }

            try
            {
                var image = await _imageLoadTask;
                if (image != null && !_disposed && !cancellationToken.IsCancellationRequested)
                {
                    // 在UI线程中更新
                    if (context.Grid.InvokeRequired)
                    {
                        context.Grid.Invoke((Action)(() =>
                        {
                            if (!_disposed && !cancellationToken.IsCancellationRequested)
                            {
                                GridImage = image;
                                // 触发重绘
                                context.Grid.InvalidateCell(context.Position);
                            }
                        }));
                    }
                    else
                    {
                        if (!_disposed && !cancellationToken.IsCancellationRequested)
                        {
                            GridImage = image;
                            // 触发重绘
                            context.Grid.InvalidateCell(context.Position);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 任务被取消，正常退出
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 同步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="context">单元格上下文</param>
        protected virtual void LoadImageSync(long fileId, CellContext context)
        {
            try
            {
                if (context.Cell.Editor is ImageWebPickEditor webPicker)
                {
                    var model = context.Cell.Model.FindModel(typeof(ValueImageWeb));
                    if (model is ValueImageWeb valueImageWeb)
                    {
                        if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                        {
                            GridImage = ImageProcessor.ByteArrayToImage(valueImageWeb.CellImageBytes);
                        }
                        else
                        {
                            // 尝试从缓存加载
                            GridImage = ImageCacheManager.Instance.GetImage(
                                fileId,
                                (id) => LoadImageDataSync(id, context)
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 异步加载图片数据
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="context">单元格上下文</param>
        /// <returns>图片字节数据</returns>
        protected virtual async Task<byte[]> LoadImageDataAsync(long fileId, CellContext context)
        {
            return await Task.Run(() => LoadImageDataSync(fileId, context));
        }

        /// <summary>
        /// 同步加载图片数据
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="context">单元格上下文</param>
        /// <returns>图片字节数据</returns>
        protected virtual byte[] LoadImageDataSync(long fileId, CellContext context)
        {
            try
            {
                // 从ValueImageWeb获取图片数据
                if (context.Cell.Editor is ImageWebPickEditor webPicker)
                {
                    var model = context.Cell.Model.FindModel(typeof(ValueImageWeb));
                    if (model is ValueImageWeb valueImageWeb)
                    {
                        // 检查FileId是否匹配，确保获取正确的图片数据
                        if (valueImageWeb.FileId == fileId || valueImageWeb.FileId == 0) // 0表示未设置
                        {
                            return valueImageWeb.CellImageBytes;
                        }
                    }
                }

                // 尝试从本地文件加载
                string filePath = fileId.ToString();
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载图片数据失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 取消之前的任务
        /// </summary>
        protected virtual void CancelPreviousTask()
        {
            if (_cancellationTokenSource != null)
            {
                try
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Dispose();
                }
                catch { }
            }

            if (_imageLoadTask != null)
            {
                try
                {
                    _imageLoadTask.Dispose();
                }
                catch { }
            }
        }

        #endregion

        #region 图片显示

        /// <summary>
        /// 从字节数据显示图片
        /// </summary>
        /// <param name="context">单元格上下文</param>
        /// <param name="imageData">图片字节数据</param>
        protected virtual void DisplayImageFromBytes(CellContext context, byte[] imageData)
        {
            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var image = System.Drawing.Image.FromStream(ms);
                    
                    // 直接更新当前视图的属性
                    GridImage = new Bitmap(image);
                    
                    // 更新哈希值
                    _currentImageHash = ImageHashHelper.GenerateHash(imageData);
                    
                    // 强制刷新显示
                    context.Grid?.InvalidateCell(context.Position);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示图片失败: {ex.Message}");
                // 清空图片和哈希值
                ClearImage(context);
            }
        }

        /// <summary>
        /// 清空图片
        /// </summary>
        /// <param name="context">单元格上下文</param>
        protected virtual void ClearImage(CellContext context)
        {
            if (!string.IsNullOrEmpty(_currentImageHash))
            {
                GridImage = null;
                _currentImageHash = string.Empty;
                context.Grid?.InvalidateCell(context.Position);
            }
        }

        #endregion

        #region 绘制

        /// <summary>
        /// 绘制内容
        /// </summary>
        /// <param name="graphics">图形对象</param>
        /// <param name="area">绘制区域</param>
        protected override void OnDrawContent(GraphicsCache graphics, RectangleF area)
        {
            base.OnDrawContent(graphics, area);
            using (MeasureHelper measure = new MeasureHelper(graphics))
            {
                // 使用局部变量避免多线程问题
                var currentImage = GridImage;

                // 检查GridImage是否有效
                if (currentImage != null)
                {
                    try
                    {
                        // 先检查图片是否有效，避免 ArgumentException
                        if (IsImageValid(currentImage))
                        {
                            graphics.Graphics.DrawImage(currentImage, Rectangle.Round(area));
                        }
                        else
                        {
                            // 图片无效，释放并设置为null
                            GridImage = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 绘制失败，将GridImage设置为null
                        GridImage = null;
                        System.Diagnostics.Debug.WriteLine("绘制图片失败: " + ex.Message);
                    }
                }

                // 绘制图片状态标记
                DrawImageStatus(graphics, area);
            }
        }

        /// <summary>
        /// 绘制图片状态标记
        /// </summary>
        /// <param name="graphics">图形对象</param>
        /// <param name="area">绘制区域</param>
        protected virtual void DrawImageStatus(GraphicsCache graphics, RectangleF area)
        {
            try
            {
                // 使用缓存的图片状态，避免在绘制时访问 ImageStateManager
                var status = _cachedImageStatus;

                // 根据状态绘制不同的标记
                switch (status)
                {
                    case ImageStatus.PendingDelete:
                        // 绘制待删除标记
                        DrawPendingDeleteMark(graphics, area);
                        break;
                    case ImageStatus.PendingUpload:
                        // 绘制待上传标记
                        DrawPendingUploadMark(graphics, area);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("绘制图片状态标记失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 绘制待删除标记
        /// </summary>
        /// <param name="graphics">图形对象</param>
        /// <param name="area">绘制区域</param>
        protected virtual void DrawPendingDeleteMark(GraphicsCache graphics, RectangleF area)
        {
            // 绘制红色边框
            using (var pen = new Pen(Color.Red, 2))
            {
                graphics.Graphics.DrawRectangle(pen, Rectangle.Round(area));
            }

            // 绘制红色对角线
            using (var pen = new Pen(Color.Red, 2))
            {
                var rect = Rectangle.Round(area);
                var topLeft = new Point(rect.X, rect.Y);
                var bottomRight = new Point(rect.X + rect.Width, rect.Y + rect.Height);
                var topRight = new Point(rect.X + rect.Width, rect.Y);
                var bottomLeft = new Point(rect.X, rect.Y + rect.Height);
                graphics.Graphics.DrawLine(pen, topLeft, bottomRight);
                graphics.Graphics.DrawLine(pen, topRight, bottomLeft);
            }

            // 绘制"待删除"文字
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Red))
            {
                var text = "待删除";
                var textSize = graphics.Graphics.MeasureString(text, font);
                var textX = area.X + (area.Width - textSize.Width) / 2;
                var textY = area.Y + (area.Height - textSize.Height) / 2;
                graphics.Graphics.DrawString(text, font, brush, textX, textY);
            }
        }

        /// <summary>
        /// 绘制待上传标记
        /// </summary>
        /// <param name="graphics">图形对象</param>
        /// <param name="area">绘制区域</param>
        protected virtual void DrawPendingUploadMark(GraphicsCache graphics, RectangleF area)
        {
            // 绘制蓝色边框
            using (var pen = new Pen(Color.Blue, 2))
            {
                graphics.Graphics.DrawRectangle(pen, Rectangle.Round(area));
            }

            // 绘制蓝色点状边框
            using (var pen = new Pen(Color.Blue, 1))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                var rect = Rectangle.Round(area);
                rect.Inflate(-2, -2);
                graphics.Graphics.DrawRectangle(pen, rect);
            }

            // 绘制"待上传"文字
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Blue))
            {
                var text = "待上传";
                var textSize = graphics.Graphics.MeasureString(text, font);
                var textX = area.X + (area.Width - textSize.Width) / 2;
                var textY = area.Y + (area.Height - textSize.Height) / 2;
                graphics.Graphics.DrawString(text, font, brush, textX, textY);
            }
        }

        /// <summary>
        /// 检查图片是否有效
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <returns>是否有效</returns>
        protected virtual bool IsImageValid(System.Drawing.Image image)
        {
            if (image == null)
                return false;

            try
            {
                // 尝试访问图片属性，如果无效会抛出异常
                var size = image.Size;
                var format = image.RawFormat;
                return size.Width > 0 && size.Height > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

     
        #region 状态管理

        /// <summary>
        /// 缓存图片状态
        /// 在数据准备阶段缓存，避免在绘制时同步访问ImageStateManager
        /// </summary>
        protected virtual void CacheImageStatus()
        {
            try
            {
                // 直接调用 ImageStateManager 获取图片信息
                var imageInfo = ImageStateManager.Instance.GetImageInfo(CurrentFileId);
                if (imageInfo != null)
                {
                    _cachedImageStatus = imageInfo.Status;
                }
                else
                {
                    _cachedImageStatus = ImageStatus.Normal;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("缓存图片状态失败: " + ex.Message);
                _cachedImageStatus = ImageStatus.Normal;
            }
        }

        /// <summary>
        /// 检查图片是否需要更新
        /// </summary>
        /// <returns>是否需要更新</returns>
        public virtual bool IsImageNeedingUpdate()
        {
            // 这里可以实现图片更新检测逻辑
            // 例如比较图片哈希值等
            return false;
        }

        /// <summary>
        /// 重置图片状态
        /// </summary>
        public virtual void ResetImageStatus()
        {
            // 这里可以实现重置图片状态的逻辑
        }

        #endregion

        #region 内存管理

        /// <summary>
        /// 释放内存资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的具体实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // 取消加载任务
                CancelPreviousTask();

                // 释放图片资源
                if (_gridImage != null)
                {
                    _gridImage.Dispose();
                    _gridImage = null;
                }

                 

                // 清空哈希值
                _currentImageHash = string.Empty;

                _disposed = true;
            }
        }

        #endregion

        #region 抽象方法

        /// <summary>
        /// 刷新视图
        /// </summary>
        /// <param name="context">单元格上下文</param>
        public abstract override void Refresh(CellContext context);

        #endregion
    }


}
