using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.BusinessImage;

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// 简化的图片上传进度显示窗体
    /// 使用Designer文件定义控件
    /// </summary>
    public partial class SimpleImageUploadProgressForm : Form
    {
        private readonly List<ImageUploadItem> _uploadItems = new List<ImageUploadItem>();
        private bool _isCompleted = false;
        private int _successCount = 0;
        private int _failedCount = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleImageUploadProgressForm()
        {
            InitializeComponent();
            InitializeListViewColumns();
        }

        /// <summary>
        /// 构造函数（带上传项）
        /// </summary>
        /// <param name="uploadItems">上传项列表</param>
        public SimpleImageUploadProgressForm(List<ImageUploadItem> uploadItems) : this()
        {
            if (uploadItems != null)
            {
                _uploadItems.AddRange(uploadItems);
            }
        }

        /// <summary>
        /// 初始化列表视图列
        /// </summary>
        private void InitializeListViewColumns()
        {
            listViewUploads.Columns.Clear();
            listViewUploads.Columns.Add("文件名", 200);
            listViewUploads.Columns.Add("状态", 80);
            listViewUploads.Columns.Add("进度", 80);
            listViewUploads.Columns.Add("错误信息", 200);
        }

        /// <summary>
        /// 添加上传项
        /// </summary>
        /// <param name="item">上传项</param>
        public void AddUploadItem(ImageUploadItem item)
        {
            if (item != null && !IsDisposed)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => AddUploadItemInternal(item)));
                }
                else
                {
                    AddUploadItemInternal(item);
                }
            }
        }

        /// <summary>
        /// 内部添加上传项
        /// </summary>
        /// <param name="item">上传项</param>
        private void AddUploadItemInternal(ImageUploadItem item)
        {
            _uploadItems.Add(item);
            UpdateProgressDisplay();
        }

        /// <summary>
        /// 更新上传项状态
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <param name="status">上传状态</param>
        /// <param name="errorMessage">错误信息</param>
        public void UpdateUploadItemStatus(long imageId, UploadStatus status, string errorMessage = null)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUploadItemStatusInternal(imageId, status, errorMessage)));
            }
            else
            {
                UpdateUploadItemStatusInternal(imageId, status, errorMessage);
            }
        }

        /// <summary>
        /// 内部更新上传项状态
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <param name="status">上传状态</param>
        /// <param name="errorMessage">错误信息</param>
        private void UpdateUploadItemStatusInternal(long imageId, UploadStatus status, string errorMessage)
        {
            var item = _uploadItems.FirstOrDefault(i => i.ImageId == imageId);
            if (item != null)
            {
                item.Status = status;
                item.ErrorMessage = errorMessage;
                item.Progress = status == UploadStatus.Completed ? 100 : item.Progress;
                
                if (status == UploadStatus.Completed)
                {
                    _successCount++;
                }
                else if (status == UploadStatus.Failed)
                {
                    _failedCount++;
                }

                UpdateProgressDisplay();
            }
        }

        /// <summary>
        /// 更新上传进度
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <param name="progress">进度百分比</param>
        public void UpdateUploadProgress(long imageId, int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUploadProgressInternal(imageId, progress)));
            }
            else
            {
                UpdateUploadProgressInternal(imageId, progress);
            }
        }

        /// <summary>
        /// 内部更新上传进度
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <param name="progress">进度百分比</param>
        private void UpdateUploadProgressInternal(long imageId, int progress)
        {
            var item = _uploadItems.FirstOrDefault(i => i.ImageId == imageId);
            if (item != null)
            {
                item.Progress = Math.Min(100, Math.Max(0, progress));
                UpdateProgressDisplay();
            }
        }

        /// <summary>
        /// 标记上传完成
        /// </summary>
        public void MarkAsCompleted()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MarkAsCompletedInternal()));
            }
            else
            {
                MarkAsCompletedInternal();
            }
        }

        /// <summary>
        /// 内部标记上传完成
        /// </summary>
        private void MarkAsCompletedInternal()
        {
            _isCompleted = true;
            UpdateProgressDisplay();
            
            // 3秒后自动关闭
            Task.Delay(3000).ContinueWith(t => 
            {
                if (!IsDisposed)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(Close));
                    }
                    else
                    {
                        Close();
                    }
                }
            });
        }

        /// <summary>
        /// 更新进度显示
        /// </summary>
        private void UpdateProgressDisplay()
        {
            if (IsDisposed)
                return;

            // 更新总体进度
            int totalItems = _uploadItems.Count;
            int completedItems = _uploadItems.Count(i => i.Status == UploadStatus.Completed);
            int failedItems = _uploadItems.Count(i => i.Status == UploadStatus.Failed);
            int inProgressItems = _uploadItems.Count(i => i.Status == UploadStatus.Uploading);

            int overallProgress = totalItems > 0 ? (int)((completedItems + failedItems) * 100.0 / totalItems) : 0;

            lblTotalProgress.Text = $"总体进度: {overallProgress}% ({completedItems + failedItems}/{totalItems})";
            progressBarOverall.Value = overallProgress;

            lblSuccessCount.Text = $"成功: {_successCount}";
            lblFailedCount.Text = $"失败: {_failedCount}";

            // 更新列表显示
            UpdateUploadListDisplay();
        }

        /// <summary>
        /// 更新上传列表显示
        /// </summary>
        private void UpdateUploadListDisplay()
        {
            listViewUploads.BeginUpdate();
            listViewUploads.Items.Clear();

            foreach (var item in _uploadItems)
            {
                var listViewItem = new ListViewItem(item.FileName)
                {
                    Tag = item
                };

                listViewItem.SubItems.Add(GetStatusText(item.Status));
                listViewItem.SubItems.Add($"{item.Progress}%");
                listViewItem.SubItems.Add(item.ErrorMessage ?? "等待中...");

                listViewUploads.Items.Add(listViewItem);
            }

            listViewUploads.EndUpdate();
        }

        /// <summary>
        /// 获取状态文本
        /// </summary>
        /// <param name="status">上传状态</param>
        /// <returns>状态文本</returns>
        private string GetStatusText(UploadStatus status)
        {
            switch (status)
            {
                case UploadStatus.Pending:
                    return "等待中";
                case UploadStatus.Uploading:
                    return "上传中";
                case UploadStatus.Completed:
                    return "已完成";
                case UploadStatus.Failed:
                    return "失败";
                case UploadStatus.Retrying:
                    return "重试中";
                default:
                    return "未知";
            }
        }

        /// <summary>
        /// 显示进度窗体
        /// </summary>
        /// <param name="owner">父窗体</param>
        public void ShowProgress(IWin32Window owner)
        {
            if (owner != null)
            {
                StartPosition = FormStartPosition.CenterParent;
                Show(owner);
            }
            else
            {
                StartPosition = FormStartPosition.CenterScreen;
                Show();
            }
        }

        /// <summary>
        /// 获取上传结果
        /// </summary>
        /// <returns>上传结果</returns>
        public ImageUploadResult GetUploadResult()
        {
            return new ImageUploadResult
            {
                TotalCount = _uploadItems.Count,
                SuccessCount = _successCount,
                FailedCount = _failedCount,
                IsCompleted = _isCompleted,
                UploadItems = _uploadItems
            };
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lblTotalProgress?.Dispose();
                _progressBarOverall?.Dispose();
                _lblSuccessCount?.Dispose();
                _lblFailedCount?.Dispose();
                _listViewUploads?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// 图片上传项
    /// </summary>
    public class ImageUploadItem
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public long ImageId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 上传状态
        /// </summary>
        public UploadStatus Status { get; set; } = UploadStatus.Pending;

        /// <summary>
        /// 进度百分比
        /// </summary>
        public int Progress { get; set; } = 0;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; } = 0;
    }

    /// <summary>
    /// 上传状态
    /// </summary>
    public enum UploadStatus
    {
        /// <summary>
        /// 等待中
        /// </summary>
        Pending,
        
        /// <summary>
        /// 上传中
        /// </summary>
        Uploading,
        
        /// <summary>
        /// 已完成
        /// </summary>
        Completed,
        
        /// <summary>
        /// 失败
        /// </summary>
        Failed,
        
        /// <summary>
        /// 重试中
        /// </summary>
        Retrying
    }

    /// <summary>
    /// 图片上传结果
    /// </summary>
    public class ImageUploadResult
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// 上传项列表
        /// </summary>
        public List<ImageUploadItem> UploadItems { get; set; }
    }
}