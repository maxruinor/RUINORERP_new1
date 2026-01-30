using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Network.Services;

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
            this.prodDetail = detail;
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void frmSKUImageEdit_Load(object sender, EventArgs e)
        {
            try
            {
                // 设置窗体标题
                this.Text = $"SKU图片编辑 - {prodDetail.PropertyGroupName ?? prodDetail.SKU ?? "未命名"}";

                // 初始化MagicPictureBox
                InitializeMagicPictureBox();

                // 加载现有图片
                LoadImages();
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
            magicPictureBox.BorderStyle = BorderStyle.FixedSingle;

            // MagicPictureBox已经有内置的双击查看大图功能，不需要额外注册
            // 如果需要自定义行为，可以在这里注册事件

            // 注册拖拽事件
            magicPictureBox.DragEnter += MagicPictureBox_DragEnter;
            magicPictureBox.DragDrop += MagicPictureBox_DragDrop;

            // 注册粘贴事件
            this.KeyDown += frmSKUImageEdit_KeyDown;
        }

        /// <summary>
        /// 加载现有图片
        /// </summary>
        private void LoadImages()
        {
            if (prodDetail == null || string.IsNullOrEmpty(prodDetail.ImagesPath))
            {
                return;
            }

            try
            {
                // 解析图片路径（多张图片用分号分隔）
                string[] imagePaths = prodDetail.ImagesPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (imagePaths.Length > 0)
                {
                    MainForm.Instance.uclog.AddLog($"SKU {prodDetail.SKU} 准备下载 {imagePaths.Length} 张图片");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"解析SKU图片路径失败: {ex.Message}", Global.UILogType.错误);
            }
        }

        /// <summary>
        /// 异步下载图片
        /// </summary>
        public async Task DownloadImagesAsync()
        {
            if (prodDetail == null || prodDetail.ProdDetailID <= 0 || string.IsNullOrEmpty(prodDetail.ImagesPath))
            {
                return;
            }

            try
            {
                var fileService = Startup.GetFromFac<FileBusinessService>();
                var downloadResponse = await fileService.DownloadImageAsync(prodDetail, "ImagesPath");

                if (downloadResponse == null || downloadResponse.Count == 0)
                {
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
                    MainForm.Instance.uclog.AddLog($"成功加载 {imageDataList.Count} 张SKU图片到编辑器");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"下载SKU图片出错: {ex.Message}", Global.UILogType.错误);
            }
        }

        /// <summary>
        /// 拖拽进入事件
        /// </summary>
        private void MagicPictureBox_DragEnter(object sender, DragEventArgs e)
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

        /// <summary>
        /// 拖拽放下事件
        /// </summary>
        private void MagicPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    List<byte[]> imageBytesList = new List<byte[]>();
                    List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo> imageInfos = new List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>();

                    foreach (string file in files)
                    {
                        if (IsImageFile(file))
                        {
                            // 读取图片文件
                            byte[] imageBytes = System.IO.File.ReadAllBytes(file);
                            imageBytesList.Add(imageBytes);

                            // 创建ImageInfo对象
                            var imageInfo = new RUINOR.WinFormsUI.CustomPictureBox.ImageInfo
                            {
                                OriginalFileName = System.IO.Path.GetFileName(file),
                                FileSize = imageBytes.Length,
                                IsUpdated = true
                            };
                            imageInfos.Add(imageInfo);
                        }
                    }

                    // 加载所有拖拽的图片
                    if (imageBytesList.Count > 0)
                    {
                        magicPictureBox.LoadImages(imageBytesList, imageInfos, false);
                        MainForm.Instance.uclog.AddLog($"成功添加 {imageBytesList.Count} 张图片");
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"拖拽图片失败: {ex.Message}", Global.UILogType.错误);
                MessageBox.Show($"拖拽图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 键盘按下事件（处理粘贴）
        /// </summary>
        private void frmSKUImageEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                PasteImageFromClipboard();
            }
        }

        /// <summary>
        /// 从剪贴板粘贴图片
        /// </summary>
        private void PasteImageFromClipboard()
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    Image image = Clipboard.GetImage();
                    if (image != null)
                    {
                        // 将Image转换为byte[]
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageBytes = ms.ToArray();

                            // 创建ImageInfo对象
                            var imageInfo = new RUINOR.WinFormsUI.CustomPictureBox.ImageInfo
                            {
                                OriginalFileName = $"截图_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                                FileSize = imageBytes.Length,
                                IsUpdated = true
                            };

                            // 加载图片
                            magicPictureBox.LoadImages(new List<byte[]> { imageBytes }, new List<RUINOR.WinFormsUI.CustomPictureBox.ImageInfo> { imageInfo }, false);

                            MainForm.Instance.uclog.AddLog("成功从剪贴板粘贴图片");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("剪贴板中没有图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"粘贴图片失败: {ex.Message}", Global.UILogType.错误);
            }
        }

        /// <summary>
        /// 检查文件是否为图片
        /// </summary>
        private bool IsImageFile(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath).ToLower();
            return extension == ".jpg" || extension == ".jpeg" ||
                   extension == ".png" || extension == ".gif" ||
                   extension == ".bmp" || extension == ".ico";
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否有图片更改
                var updatedImages = magicPictureBox.GetImagesNeedingUpdate();
                var deletedImages = magicPictureBox.GetDeletedImages();

                if ((updatedImages != null && updatedImages.Count > 0) ||
                    (deletedImages != null && deletedImages.Count > 0))
                {
                    prodDetail.HasUnsavedImageChanges = true;
                    MainForm.Instance.uclog.AddLog($"SKU {prodDetail.SKU} 图片已标记为需要保存");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"保存图片状态失败: {ex.Message}", Global.UILogType.错误);
                MessageBox.Show($"保存图片状态失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
