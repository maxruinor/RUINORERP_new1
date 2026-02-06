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
        private List<Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>> preloadedImages;

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
            List<Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>> existingImages = null)
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
                List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo> imageInfos = new List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>();

                foreach (var item in preloadedImages)
                {
                    if (item.Item1 != null && item.Item1.Length > 0)
                    {
                        imageDataList.Add(item.Item1);
                        imageInfos.Add(item.Item2 ?? new RUINOR.WinFormsUI.CustomPictureBox.ImageInfo());
                    }
                }

                if (imageDataList.Count > 0)
                {
                    // 加载到MagicPictureBox（不清除现有图片，标记为非更新状态）
                    magicPictureBox.LoadImages(imageDataList, imageInfos, false);
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
                var downloadResponse = await fileService.DownloadImageAsync(prodDetail, "ImagesPath");

                if (downloadResponse == null || downloadResponse.Count == 0)
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
                List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo> imageInfos = new List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>();

                foreach (var response in downloadResponse)
                {
                    if (response.IsSuccess && response.FileStorageInfos != null)
                    {
                        foreach (var fileStorageInfo in response.FileStorageInfos)
                        {
                            if (fileStorageInfo.FileData != null && fileStorageInfo.FileData.Length > 0)
                            {
                                imageDataList.Add(fileStorageInfo.FileData);
                                imageInfos.Add(fileService.ConvertToImageInfo(fileStorageInfo));
                            }
                        }
                    }
                }

                if (imageDataList.Count > 0)
                {
                    // 直接加载服务器图片（覆盖现有图片）
                    magicPictureBox.LoadImages(imageDataList, imageInfos, true);
                    lblInfo.Values.Text = $"成功加载 {imageDataList.Count} 张图片";

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
        /// 处理所有图片变更：新增、替换、删除
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有需要处理的图片变更
                // 1. 获取需要上传的图片（包括新增的和替换的）
                var updatedImages = magicPictureBox.GetImagesNeedingUpdate();
                // 2. 获取已删除的图片
                var deletedImages = magicPictureBox.GetDeletedImages();
                // 3. 获取当前所有图片（用于更新ImagesPath字段）
                var currentImages = magicPictureBox.GetImages();

                int updateCount = updatedImages?.Count ?? 0;
                int deleteCount = deletedImages?.Count ?? 0;
                int totalCount = currentImages?.Count ?? 0;

                if (updateCount > 0 || deleteCount > 0)
                {
                    prodDetail.HasUnsavedImageChanges = true;

                    // 记录详细的变更信息
                    string changeInfo = $"图片变更统计：新增/替换 {updateCount} 张，删除 {deleteCount} 张，当前共 {totalCount} 张";
                    lblInfo.Values.Text = changeInfo;
                    MainForm.Instance.uclog.AddLog($"SKU {prodDetail.SKU} {changeInfo}");

                    // 如果有替换操作（既有删除又有新增），需要特别标记
                    if (updateCount > 0 && deleteCount > 0)
                    {
                        MainForm.Instance.uclog.AddLog($"SKU {prodDetail.SKU} 检测到图片替换操作");
                    }
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
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 获取需要上传的图片
        /// </summary>
        /// <returns>需要上传的图片列表</returns>
        public List<Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>> GetUpdatedImages()
        {
            return magicPictureBox.GetImagesNeedingUpdate();
        }

        /// <summary>
        /// 获取已删除的图片
        /// </summary>
        /// <returns>已删除的图片列表</returns>
        public List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo> GetDeletedImages()
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
