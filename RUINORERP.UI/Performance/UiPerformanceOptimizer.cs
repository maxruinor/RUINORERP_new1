using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Performance
{
    /// <summary>
    /// UI 性能优化工具类
    /// 提供常用的 UI 性能优化方法
    /// </summary>
    public static class UiPerformanceOptimizer
    {
        private static ILogger _logger;

        /// <summary>
        /// 初始化日志记录器
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public static void Initialize(ILogger logger)
        {
            _logger = logger;
        }

        #region 数据绑定优化

        /// <summary>
        /// 批量绑定数据到 DataGridView（暂停重绘）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="dataGridView">目标 DataGridView</param>
        /// <param name="dataList">数据列表</param>
        /// <param name="autoGenerateColumns">是否自动生成列</param>
        public static void BindDataBatch<T>(
            this DataGridView dataGridView,
            IList<T> dataList,
            bool autoGenerateColumns = true)
        {
            if (dataGridView == null)
                throw new ArgumentNullException(nameof(dataGridView));
            
            if (dataList == null)
            {
                _logger?.LogWarning("数据列表为空，绑定空数据源");
                dataGridView.DataSource = null;
                return;
            }

            try
            {
                // 1. 暂停重绘
                dataGridView.SuspendLayout();
                
                // 2. 获取 BindingSource
                var bindingSource = dataGridView.DataSource as BindingSource;
                
                // 3. 暂停事件通知
                if (bindingSource != null)
                {
                    bindingSource.RaiseListChangedEvents = false;
                }
                
                // 4. 清空现有数据
                dataGridView.DataSource = null;
                dataGridView.Rows.Clear();
                
                // 5. 绑定新数据
                if (dataList.Count > 0)
                {
                    var bindingList = new BindingList<T>(dataList);
                    bindingSource = new BindingSource { DataSource = bindingList };
                    dataGridView.DataSource = bindingSource;
                    dataGridView.AutoGenerateColumns = autoGenerateColumns;
                }
                
                // 6. 恢复事件通知
                if (bindingSource != null)
                {
                    bindingSource.RaiseListChangedEvents = true;
                    bindingSource.ResetBindings(false);
                }
                
                // 7. 恢复重绘
                dataGridView.ResumeLayout();
                
                _logger?.LogDebug($"批量绑定数据成功，数据量：{dataList.Count}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量绑定数据失败");
                dataGridView.ResumeLayout();
                throw;
            }
        }

        /// <summary>
        /// 批量绑定 DataTable 到 DataGridView
        /// </summary>
        /// <param name="dataGridView">目标 DataGridView</param>
        /// <param name="dataTable">数据表</param>
        public static void BindDataBatch(
            this DataGridView dataGridView,
            DataTable dataTable)
        {
            if (dataGridView == null)
                throw new ArgumentNullException(nameof(dataGridView));

            try
            {
                dataGridView.SuspendLayout();
                
                var bindingSource = dataGridView.DataSource as BindingSource;
                if (bindingSource != null)
                {
                    bindingSource.RaiseListChangedEvents = false;
                }
                
                dataGridView.DataSource = null;
                
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    bindingSource = new BindingSource { DataSource = dataTable };
                    dataGridView.DataSource = bindingSource;
                }
                
                if (bindingSource != null)
                {
                    bindingSource.RaiseListChangedEvents = true;
                    bindingSource.ResetBindings(false);
                }
                
                dataGridView.ResumeLayout();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "绑定 DataTable 失败");
                dataGridView.ResumeLayout();
                throw;
            }
        }

        #endregion

        #region 控件优化

        /// <summary>
        /// 启用双缓冲（减少闪烁）
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="enable">是否启用</param>
        public static void EnableDoubleBuffered(this Control control, bool enable = true)
        {
            if (control == null)
                return;

            try
            {
                var propertyInfo = control.GetType().GetProperty(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(control, enable, null);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "启用双缓冲失败");
            }
        }

        /// <summary>
        /// 批量设置控件双缓冲
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="enable">是否启用</param>
        public static void EnableDoubleBuffered(this Control.ControlCollection controls, bool enable = true)
        {
            foreach (Control control in controls)
            {
                control.EnableDoubleBuffered(enable);
                
                // 递归处理子控件
                if (control.HasChildren)
                {
                    control.Controls.EnableDoubleBuffered(enable);
                }
            }
        }

        #endregion

        #region 异步加载优化

        /// <summary>
        /// 异步加载窗体数据（带进度提示）
        /// </summary>
        /// <param name="form">目标窗体</param>
        /// <param name="loadAction">数据加载操作</param>
        /// <param name="loadingMessage">加载提示信息</param>
        /// <param name="showCursorWait">是否显示等待光标</param>
        public static async Task LoadDataAsync(
            this Form form,
            Func<Task> loadAction,
            string loadingMessage = "正在加载数据...",
            bool showCursorWait = true)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var originalCursor = form.Cursor;
            
            try
            {
                // 显示等待光标
                if (showCursorWait)
                {
                    form.Cursor = Cursors.WaitCursor;
                }
                
                // 显示加载提示
                var loadingLabel = ShowLoadingLabel(form, loadingMessage);
                
                // 异步加载数据
                await Task.Run(async () =>
                {
                    await loadAction();
                });
                
                // 隐藏加载提示
                HideLoadingLabel(form, loadingLabel);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "异步加载数据失败");
                throw;
            }
            finally
            {
                // 恢复光标
                if (showCursorWait)
                {
                    form.Cursor = originalCursor;
                }
            }
        }

        /// <summary>
        /// 显示加载标签
        /// </summary>
        private static Label ShowLoadingLabel(Form form, string message)
        {
            var label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(form.Font.FontFamily, 12, FontStyle.Bold),
                ForeColor = Color.Blue,
                BackColor = Color.White,
                Name = "LoadingLabel"
            };
            
            form.Controls.Add(label);
            label.BringToFront();
            label.Refresh();
            
            Application.DoEvents();
            
            return label;
        }

        /// <summary>
        /// 隐藏加载标签
        /// </summary>
        private static void HideLoadingLabel(Form form, Label loadingLabel)
        {
            if (loadingLabel != null && form.Controls.Contains(loadingLabel))
            {
                form.Controls.Remove(loadingLabel);
                loadingLabel.Dispose();
            }
        }

        #endregion

        #region 图片加载优化

        /// <summary>
        /// 加载并缩放图片（节省内存）
        /// </summary>
        /// <param name="pictureBox">目标 PictureBox</param>
        /// <param name="imagePath">图片路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <param name="useCache">是否使用缓存</param>
        public static async Task LoadAndResizeImageAsync(
            this PictureBox pictureBox,
            string imagePath,
            int maxWidth = 800,
            int maxHeight = 600,
            bool useCache = true)
        {
            if (pictureBox == null)
                throw new ArgumentNullException(nameof(pictureBox));

            if (string.IsNullOrEmpty(imagePath))
            {
                pictureBox.Image = null;
                return;
            }

            try
            {
                // 显示加载中的占位图
                pictureBox.Image = GetPlaceholderImage();
                
                // 异步加载和缩放图片
                var image = await Task.Run(() =>
                {
                    using (var originalImage = Image.FromFile(imagePath))
                    {
                        return ResizeImage(originalImage, maxWidth, maxHeight);
                    }
                });
                
                pictureBox.Image = image;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"加载图片失败：{imagePath}");
                pictureBox.Image = GetErrorImage();
            }
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        private static Image ResizeImage(Image originalImage, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / originalImage.Width;
            var ratioY = (double)maxHeight / originalImage.Height;
            var ratio = Math.Min(ratioX, ratioY);
            
            var newWidth = (int)(originalImage.Width * ratio);
            var newHeight = (int)(originalImage.Height * ratio);
            
            var thumbnail = new Bitmap(newWidth, newHeight);
            
            using (var graphics = Graphics.FromImage(thumbnail))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }
            
            return thumbnail;
        }

        /// <summary>
        /// 获取占位图
        /// </summary>
        private static Image GetPlaceholderImage()
        {
            var bitmap = new Bitmap(100, 100);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.LightGray);
                graphics.DrawString("加载中...", 
                    new Font("宋体", 10), 
                    Brushes.DarkGray, 
                    new PointF(20, 40));
            }
            return bitmap;
        }

        /// <summary>
        /// 获取错误提示图
        /// </summary>
        private static Image GetErrorImage()
        {
            var bitmap = new Bitmap(100, 100);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.PaleVioletRed);
                graphics.DrawString("加载失败", 
                    new Font("宋体", 10), 
                    Brushes.White, 
                    new PointF(15, 40));
            }
            return bitmap;
        }

        #endregion

        #region 性能监控

        /// <summary>
        /// 测量方法执行时间
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="methodName">方法名称</param>
        /// <returns>执行时间（毫秒）</returns>
        public static long MeasureExecutionTime(Action action, string methodName = "Unknown")
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            action();
            
            stopwatch.Stop();
            
            _logger?.LogDebug($"方法 {methodName} 执行时间：{stopwatch.ElapsedMilliseconds}ms");
            
            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// 异步测量方法执行时间
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="methodName">方法名称</param>
        /// <returns>执行时间（毫秒）</returns>
        public static async Task<long> MeasureExecutionTimeAsync(
            Func<Task> action, 
            string methodName = "Unknown")
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            await action();
            
            stopwatch.Stop();
            
            _logger?.LogDebug($"异步方法 {methodName} 执行时间：{stopwatch.ElapsedMilliseconds}ms");
            
            return stopwatch.ElapsedMilliseconds;
        }

        #endregion

        #region 内存优化

        /// <summary>
        /// 清理资源并触发 GC
        /// </summary>
        public static void CleanupAndForceGc()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            _logger?.LogDebug($"执行垃圾回收，当前内存：{Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024}MB");
        }

        /// <summary>
        /// 获取当前内存使用情况
        /// </summary>
        /// <returns>内存使用量（MB）</returns>
        public static long GetCurrentMemoryUsageMB()
        {
            return Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
        }

        #endregion
    }

    /// <summary>
    /// 双缓冲 DataGridView
    /// </summary>
    public class DoubleBufferedDataGridView : DataGridView
    {
        public DoubleBufferedDataGridView()
        {
            this.DoubleBuffered = true;
        }
    }

    /// <summary>
    /// 双缓冲 Panel
    /// </summary>
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
        }
    }
}
