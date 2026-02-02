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
        /// 构造函数
        /// </summary>
        /// <param name="detail">产品明细实体</param>
        public frmSKUImageEdit(tb_ProdDetail detail)
        {
            this.prodDetail = detail ?? throw new ArgumentNullException(nameof(detail));
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



                // 异步加载现有图片
                await LoadImagesAsync();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"初始化SKU图片编辑器失败: {ex.Message}", Global.UILogType.错误);
                MessageBox.Show($"初始化SKU图片编辑器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// </summary>
        public async Task DownloadImagesAsync()
        {
            if (prodDetail == null || prodDetail.ProdDetailID <= 0 || string.IsNullOrEmpty(prodDetail.ImagesPath))
            {
                lblInfo.Values.Text = "暂无图片";
                return;
            }

            try
            {
                lblInfo.Values.Text = "正在下载图片...";

                var fileService = Startup.GetFromFac<FileBusinessService>();
                var downloadResponse = await fileService.DownloadImageAsync(prodDetail, "ImagesPath");

                if (downloadResponse == null || downloadResponse.Count == 0)
                {
                    lblInfo.Values.Text = "未找到图片文件";
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
                    magicPictureBox.LoadImages(imageDataList, imageInfos, true);
                    lblInfo.Values.Text = $"成功加载 {imageDataList.Count} 张图片";
                    MainForm.Instance.uclog.AddLog($"成功加载 {imageDataList.Count} 张SKU图片到编辑器");
                }
                else
                {
                    lblInfo.Values.Text = "图片数据为空";
                }
            }
            catch (Exception ex)
            {
                lblInfo.Values.Text = $"下载失败: {ex.Message}";
                MainForm.Instance.uclog.AddLog($"下载SKU图片出错: {ex.Message}", Global.UILogType.错误);
                MessageBox.Show($"下载图片失败:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否有图片更改1
                var updatedImages = magicPictureBox.GetImagesNeedingUpdate();
                var deletedImages = magicPictureBox.GetDeletedImages();

                if ((updatedImages != null && updatedImages.Count > 0) ||
                    (deletedImages != null && deletedImages.Count > 0))
                {
                    prodDetail.HasUnsavedImageChanges = true;
                    lblInfo.Values.Text = $"已标记 {updatedImages?.Count ?? 0 + deletedImages?.Count ?? 0} 项更改，保存后生效";
                    MainForm.Instance.uclog.AddLog($"SKU {prodDetail.SKU} 图片已标记为需要保存");
                }
                else
                {
                    lblInfo.Values.Text = "没有需要保存的更改";
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
