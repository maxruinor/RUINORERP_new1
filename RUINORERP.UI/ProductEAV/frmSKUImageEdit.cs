using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Network.Services;
using Krypton.Toolkit;
using RUINORERP.Model.BusinessImage;

namespace RUINORERP.UI.ProductEAV
{
    /// <summary>
    /// SKU图片编辑对话框
    /// 支持多图片的上传、删除、预览功能
    /// </summary>
    public partial class frmSKUImageEdit : Form
    {
        /// <summary>
        /// 产品明细实体
        /// </summary>
        private tb_ProdDetail prodDetail;

        /// <summary>
        /// 预加载的图片数据（从父窗口传递）
        /// </summary>
        private List<Tuple<byte[], ImageInfo>> preloadedImages;

        /// <summary>
        /// 是否从服务器加载了图片
        /// </summary>
        private bool _imagesLoadedFromServer = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="detail">产品明细实体</param>
        /// <param name="existingImages">预加载的图片数据（可选，用于显示已缓存的图片）</param>
        public frmSKUImageEdit(tb_ProdDetail detail,
            List<Tuple<byte[], ImageInfo>> existingImages = null)
        {
            this.prodDetail = detail ?? throw new ArgumentNullException(nameof(detail));
            this.preloadedImages = existingImages;
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private async void frmSKUImageEdit_Load(object sender, EventArgs e)
        {
            try
            {
                // 设置窗体标题
                this.Text = $"SKU图片编辑 - {prodDetail.PropertyGroupName ?? prodDetail.SKU ?? "未命名"}";

                // 初始化MagicPictureBox
                InitializeMagicPictureBox();

                // 如果有预加载的图片数据，先显示这些图片
                if (preloadedImages != null && preloadedImages.Count > 0)
                {
                    LoadPreloadedImages();
                }

                // 异步加载服务器上的现有图片
                await LoadImagesAsync();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"初始化SKU图片编辑器失败: {ex.Message}", Global.UILogType.错误);
                MessageBox.Show($"初始化SKU图片编辑器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载预缓存的图片数据
        /// 优化：在图片上显示状态标识（新增/修改）
        /// </summary>
        private void LoadPreloadedImages()
        {
            try
            {
                if (preloadedImages == null || preloadedImages.Count == 0)
                    return;

                lblInfo.Values.Text = "正在加载缓存图片...";

                // 提取图片数据和元数据
                List<byte[]> imageDataList = new List<byte[]>();
                List<ImageInfo> imageInfos = new List<ImageInfo>();

                foreach (var item in preloadedImages)
                {
                    if (item.Item1 != null && item.Item1.Length > 0)
                    {
                        imageDataList.Add(item.Item1);
                        
                        // 为预加载的图片添加状态标记
                        var info = item.Item2 ?? new ImageInfo();
                        
                        // ✅ 关键修复: 根据FileId判断图片来源,设置正确的Status
                        if (info.FileId == 0)
                        {
                            // 新增图片: FileId=0,标记为待上传
                            info.Tag = "新增";
                            info.Status = ImageStatus.PendingUpload;
                        }
                        else
                        {
                            // 已有图片: FileId>0,标记为正常(不需要重新上传)
                            info.Tag = "修改";
                            info.Status = ImageStatus.Normal;
                        }
                        imageInfos.Add(info);
                    }
                }

                if (imageDataList.Count > 0)
                {
                    // ✅ 关键修复: 传入true表示这些图片已经存在于系统中
                    // 这样LoadImagesInternal会根据FileId正确设置Status
                    magicPictureBox.LoadImages(imageDataList, imageInfos, true);
                    lblInfo.Values.Text = $"已加载 {imageDataList.Count} 张缓存图片，正在检查服务器...";
                    MainForm.Instance.uclog.AddLog($"SKU编辑器从缓存加载了 {imageDataList.Count} 张图片");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"加载缓存图片失败: {ex.Message}", Global.UILogType.警告);
            }
        }

        /// <summary>
        /// 初始化MagicPictureBox控件
        /// </summary>
        private void InitializeMagicPictureBox()
        {
            magicPictureBox.MultiImageSupport = true;
            magicPictureBox.AllowDrop = true;
            magicPictureBox.BackColor = Color.White;
            magicPictureBox.BorderStyle = BorderStyle.FixedSingle;

            // 设置工具提示
            toolTip.SetToolTip(magicPictureBox, "拖拽图片到此处上传，或按Ctrl+V粘贴截图");
        }

        /// <summary>
        /// 异步加载现有图片
        /// </summary>
        private async Task LoadImagesAsync()
        {
            if (prodDetail == null || string.IsNullOrEmpty(prodDetail.ImagesPath))
            {
                MainForm.Instance.uclog.AddLog($"[LoadImagesAsync] SKU无图片路径，SKU: {prodDetail?.SKU ?? "N/A"}, ImagesPath: {prodDetail?.ImagesPath ?? "null"}");
                return;
            }

            try
            {
                lblInfo.Values.Text = "正在加载图片...";

                // 异步下载图片
                await DownloadImagesAsync();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"加载SKU图片失败: {ex.Message}", Global.UILogType.错误);
                lblInfo.Values.Text = $"加载失败: {ex.Message}";
            }
            finally
            {
                lblInfo.Values.Text = "提示：支持拖拽上传、Ctrl+V粘贴截图、双击查看大图";
                // 显示当前图片数量供调试
                var currentImages = magicPictureBox.GetImages();
                MainForm.Instance.uclog.AddLog($"[LoadImagesAsync] 完成，当前MagicPictureBox中有 {currentImages?.Count ?? 0} 张图片");
            }
        }

        /// <summary>
        /// 异步下载图片（带进度提示）
        /// 优化：与预加载图片合并显示，避免重复
        /// </summary>
        public async Task DownloadImagesAsync()
        {
            if (prodDetail == null || string.IsNullOrEmpty(prodDetail.ImagesPath))
            {
                // 如果没有图片路径但有预加载图片，仍然显示预加载图片
                if (preloadedImages == null || preloadedImages.Count == 0)
                {
                    lblInfo.Values.Text = "暂无图片 - 拖拽或粘贴添加";
                }
                else
                {
                    lblInfo.Values.Text = $"已加载 {preloadedImages.Count} 张图片 - 拖拽或粘贴添加更多";
                }
                return;
            }

            try
            {
                lblInfo.Values.Text = "正在从服务器加载图片...";

                var fileService = Startup.GetFromFac<FileBusinessService>();
                var downloadResponse = await fileService.DownloadImageAsync<tb_ProdDetail>(prodDetail,c=>c.ImagesPath);

                if (downloadResponse == null || !downloadResponse.IsSuccess)
                {
                    // 如果没有从服务器下载到图片，但有预加载图片
                    if (preloadedImages != null && preloadedImages.Count > 0)
                    {
                        lblInfo.Values.Text = $"已加载 {preloadedImages.Count} 张本地图片 - 尚未保存到服务器";
                    }
                    else
                    {
                        lblInfo.Values.Text = "未找到图片文件 - 拖拽或粘贴添加";
                    }
                    return;
                }

                List<byte[]> imageDataList = new List<byte[]>();
                List<ImageInfo> imageInfos = new List<ImageInfo>();

               
                    if (downloadResponse.IsSuccess && downloadResponse.FileStorageInfos != null)
                    {
                        foreach (var fileStorageInfo in downloadResponse.FileStorageInfos)
                        {
                            if (fileStorageInfo.FileData != null && fileStorageInfo.FileData.Length > 0)
                            {
                                imageDataList.Add(fileStorageInfo.FileData);
                                imageInfos.Add(fileService.ConvertToImageInfo(fileStorageInfo));
                            }
                        }
                    }
                

                if (imageDataList.Count > 0)
                {
                    // 切换回 UI 线程更新控件，避免阻塞
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => 
                        {
                            magicPictureBox.LoadImages(imageDataList, imageInfos, true);
                            lblInfo.Values.Text = $"成功加载 {imageDataList.Count} 张图片";
                        }));
                    }
                    else
                    {
                        magicPictureBox.LoadImages(imageDataList, imageInfos, true);
                        lblInfo.Values.Text = $"成功加载 {imageDataList.Count} 张图片";
                    }

                    _imagesLoadedFromServer = true;
                    MainForm.Instance.uclog.AddLog($"成功从服务器加载 {imageDataList.Count} 张SKU图片到编辑器");
                }
                else
                {
                    if (preloadedImages != null && preloadedImages.Count > 0)
                    {
                        lblInfo.Values.Text = $"已加载 {preloadedImages.Count} 张本地图片 - 服务器数据为空";
                    }
                    else
                    {
                        lblInfo.Values.Text = "图片数据为空 - 拖拽或粘贴添加";
                    }
                }
            }
            catch (Exception ex)
            {
                lblInfo.Values.Text = $"服务器加载失败: {ex.Message}";
                MainForm.Instance.uclog.AddLog($"从服务器下载SKU图片出错: {ex.Message}", Global.UILogType.错误);

                // 如果有预加载图片，仍然可以使用
                if (preloadedImages != null && preloadedImages.Count > 0)
                {
                    lblInfo.Values.Text = $"服务器加载失败，使用本地 {preloadedImages.Count} 张图片";
                }
                else
                {
                    MessageBox.Show($"下载图片失败:\n{ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// ✅ 简化: 直接将MagicPictureBox的变更同步到prodDetail.PendingImages
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有需要处理的图片变更
                var updatedImages = magicPictureBox.GetImagesNeedingUpdate();
                var deletedImages = magicPictureBox.GetDeletedImages();
                var currentImages = magicPictureBox.GetImages();

                int updateCount = updatedImages?.Count ?? 0;
                int deleteCount = deletedImages?.Count ?? 0;
                int totalCount = currentImages?.Count ?? 0;

                MainForm.Instance.uclog.AddLog($"[btnOK_Click] SKU: {prodDetail.SKU}, 待上传: {updateCount} 张, 待删除: {deleteCount} 张, 当前总数: {totalCount} 张");

                if (updateCount > 0 || deleteCount > 0)
                {
                    // ✅ 简化: 直接操作prodDetail.PendingImages,不再需要同步转换
                    SyncChangesToPendingImages(updatedImages, deletedImages);
                    
                    string changeInfo = $"图片变更统计：新增/替换 {updateCount} 张，删除 {deleteCount} 张，当前共 {totalCount} 张";
                    lblInfo.Values.Text = changeInfo;
                    MainForm.Instance.uclog.AddLog($"SKU {prodDetail.SKU} {changeInfo}");
                }
                else
                {
                    lblInfo.Values.Text = "没有需要保存的更改";
                    MainForm.Instance.uclog.AddLog($"SKU {prodDetail.SKU} 图片无变更");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblInfo.Values.Text = $"保存失败: {ex.Message}";
                MainForm.Instance.uclog.AddLog($"保存图片状态失败: {ex.Message}", Global.UILogType.错误);
                MessageBox.Show($"保存图片状态失败:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ✅ 简化: 将MagicPictureBox的变更直接添加到prodDetail.PendingImages
        /// 使用ImageInfo.Status管理状态,不再需要PendingImageInfo转换
        /// </summary>
        private void SyncChangesToPendingImages(
            List<Tuple<byte[], ImageInfo>> updatedImages,
            List<ImageInfo> deletedImages)
        {
            try
            {
                // ✅ 关键修复: 构建已处理FileId集合,避免重复处理
                var processedFileIds = new HashSet<long>();
                
                // 1. 先处理替换操作(有FileId且ImageData不为空)
                if (updatedImages != null && updatedImages.Count > 0)
                {
                    foreach (var imgTuple in updatedImages)
                    {
                        var imageData = imgTuple.Item1;
                        var imageInfo = imgTuple.Item2;
                        
                        if (imageData != null && imageData.Length > 0 && imageInfo.FileId > 0)
                        {
                            // 替换: 标记旧图为删除,添加新图
                            prodDetail.MarkImageForReplacement(
                                existingFileId: imageInfo.FileId,
                                newImageData: imageData,
                                fileName: imageInfo.FileName ?? $"image_{DateTime.Now.Ticks}.jpg",
                                description: imageInfo.Description,
                                sortOrder: imageInfo.SortOrder
                            );
                            processedFileIds.Add(imageInfo.FileId);
                            MainForm.Instance.uclog.AddLog($"  - 标记替换图片 FileId={imageInfo.FileId}");
                        }
                    }
                }
                
                // 2. 再处理新增操作(没有FileId的图片)
                if (updatedImages != null && updatedImages.Count > 0)
                {
                    foreach (var imgTuple in updatedImages)
                    {
                        var imageData = imgTuple.Item1;
                        var imageInfo = imgTuple.Item2;
                        
                        if (imageData != null && imageData.Length > 0 && imageInfo.FileId == 0)
                        {
                            // 新增: 直接添加到PendingImages
                            prodDetail.AddPendingImage(
                                imageData: imageData,
                                fileName: imageInfo.FileName ?? $"image_{DateTime.Now.Ticks}.jpg",
                                description: imageInfo.Description,
                                sortOrder: imageInfo.SortOrder
                            );
                            MainForm.Instance.uclog.AddLog($"  - 标记新增图片 {imageInfo.FileName}");
                        }
                    }
                }

                // 3. 最后处理删除操作(排除已经被替换的图片)
                if (deletedImages != null && deletedImages.Count > 0)
                {
                    foreach (var delImg in deletedImages)
                    {
                        if (delImg.FileId > 0 && !processedFileIds.Contains(delImg.FileId))
                        {
                            prodDetail.MarkImageForDeletion(delImg.FileId);
                            MainForm.Instance.uclog.AddLog($"  - 标记删除图片 FileId={delImg.FileId}");
                        }
                        else if (processedFileIds.Contains(delImg.FileId))
                        {
                            MainForm.Instance.uclog.AddLog($"  - 跳过删除 FileId={delImg.FileId} (已被标记为替换)");
                        }
                    }
                }

                MainForm.Instance.uclog.AddLog($"已同步{updatedImages?.Count ?? 0}个新增/替换, {deletedImages?.Count ?? 0}个删除到PendingImages");
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"同步PendingImages失败: {ex.Message}", Global.UILogType.错误);
                throw;
            }
        }

        /// <summary>
        /// 取消按钮点击事件 - 确保状态回滚
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 关键修复：取消时重置 MagicPictureBox 的所有变更状态，防止脏数据污染
            magicPictureBox.ClearDeletedImagesList();
            magicPictureBox.ResetImageChangeStatus();
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 窗体关闭时清理资源，防止内存泄漏
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // 强制清理控件内的图片引用，释放内存
            magicPictureBox.ClearImage();
        }

        /// <summary>
        /// 获取需要上传的图片
        /// </summary>
        /// <returns>需要上传的图片列表</returns>
        public List<Tuple<byte[], ImageInfo>> GetUpdatedImages()
        {
            return magicPictureBox.GetImagesNeedingUpdate();
        }

        /// <summary>
        /// 获取已删除的图片
        /// </summary>
        /// <returns>已删除的图片列表</returns>
        public List<ImageInfo> GetDeletedImages()
        {
            return magicPictureBox.GetDeletedImages();
        }

        /// <summary>
        /// 清空删除列表
        /// </summary>
        public void ClearDeletedImagesList()
        {
            magicPictureBox.ClearDeletedImagesList();
        }

        /// <summary>
        /// 重置图片更改状态
        /// </summary>
        public void ResetImageChangeStatus()
        {
            magicPictureBox.ResetImageChangeStatus();
        }

        /// <summary>
        /// 获取当前SKU信息（调试用）
        /// </summary>
        public string GetSKUInfo()
        {
            int imageCount = magicPictureBox.GetImages()?.Count ?? 0;
            return $"SKU: {prodDetail.SKU ?? "N/A"}, ID: {prodDetail.ProdDetailID}, 图片数: {imageCount}";
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
