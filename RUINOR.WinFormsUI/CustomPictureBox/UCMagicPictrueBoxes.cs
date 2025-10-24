using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RUINOR.WinFormsUI.CustomPictureBox
{
    /// <summary>
    /// 多个图片显示控件
    /// 支持显示多张图片，路径用;分隔
    /// </summary>
    public partial class UCMagicPictrueBoxes : UserControl
    {
        private FlowLayoutPanel flowLayoutPanel;
        private List<MagicPictureBox> pictureBoxes;
        private int maxImageCount = 10; // 默认最大图片数量
        private string imagePaths = ""; // 存储图片路径，用;分隔

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("最大图片数量")]
        public int MaxImageCount
        {
            get { return maxImageCount; }
            set 
            { 
                maxImageCount = value;
                UpdatePictureBoxes();
            }
        }

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("图片路径，用;分隔")]
        public string ImagePaths
        {
            get { return imagePaths; }
            set 
            { 
                imagePaths = value;
                LoadImages();
            }
        }

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("是否允许上传图片")]
        public bool AllowUpload { get; set; } = true;

        public UCMagicPictrueBoxes()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // 创建FlowLayoutPanel用于自动排列图片框
            flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            
            this.Controls.Add(flowLayoutPanel);
            
            // 初始化图片框列表
            pictureBoxes = new List<MagicPictureBox>();
            
            // 创建默认的图片框
            CreatePictureBoxes();
        }

        /// <summary>
        /// 创建图片框
        /// </summary>
        private void CreatePictureBoxes()
        {
            // 清空现有控件
            flowLayoutPanel.Controls.Clear();
            pictureBoxes.Clear();

            // 创建指定数量的图片框
            for (int i = 0; i < maxImageCount; i++)
            {
                var pictureBox = new MagicPictureBox
                {
                    Size = new Size(150, 150),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    MultiImageSupport = true // 启用多图片支持
                };

                // 如果允许上传，注册双击事件
                if (AllowUpload)
                {
                    pictureBox.DoubleClick += PictureBox_DoubleClick;
                }

                flowLayoutPanel.Controls.Add(pictureBox);
                pictureBoxes.Add(pictureBox);
            }
        }

        /// <summary>
        /// 更新图片框数量
        /// </summary>
        private void UpdatePictureBoxes()
        {
            CreatePictureBoxes();
            LoadImages();
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        private void LoadImages()
        {
            // 清除所有现有图片
            foreach (var pictureBox in pictureBoxes)
            {
                // 清除图片
                pictureBox.Image = null;
                
                // 如果支持多图片，清除图片路径
                var imagePathsProperty = pictureBox.GetType().GetProperty("ImagePaths");
                if (imagePathsProperty != null)
                {
                    imagePathsProperty.SetValue(pictureBox, "");
                }
            }

            if (string.IsNullOrEmpty(imagePaths))
                return;

            var paths = imagePaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            for (int i = 0; i < pictureBoxes.Count && i < paths.Length; i++)
            {
                if (File.Exists(paths[i]))
                {
                    try
                    {
                        // 设置MagicPictureBox的图片路径属性
                        var imagePathsProperty = pictureBoxes[i].GetType().GetProperty("ImagePaths");
                        if (imagePathsProperty != null)
                        {
                            imagePathsProperty.SetValue(pictureBoxes[i], paths[i]);
                        }
                        else
                        {
                            // 如果不支持多图片属性，则直接设置Image
                            pictureBoxes[i].Image = Image.FromFile(paths[i]);
                        }
                    }
                    catch
                    {
                        // 加载失败，保持空图片框
                    }
                }
            }
        }

        /// <summary>
        /// 图片框双击事件
        /// </summary>
        private void PictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (sender is MagicPictureBox pictureBox)
            {
                // 检查是否支持多图片
                var multiImageSupportProperty = pictureBox.GetType().GetProperty("MultiImageSupport");
                if (multiImageSupportProperty != null && (bool)multiImageSupportProperty.GetValue(pictureBox))
                {
                    // 对于支持多图片的控件，打开文件选择对话框允许多选
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Title = "选择图片";
                        openFileDialog.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                        openFileDialog.Multiselect = true; // 允许多选
                        
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                // 获取当前图片路径
                                var imagePathsProperty = pictureBox.GetType().GetProperty("ImagePaths");
                                if (imagePathsProperty != null)
                                {
                                    var currentPaths = imagePathsProperty.GetValue(pictureBox)?.ToString() ?? "";
                                    var pathList = currentPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    
                                    // 添加新选择的图片路径
                                    foreach (var fileName in openFileDialog.FileNames)
                                    {
                                        pathList.Add(fileName);
                                    }
                                    
                                    // 更新图片路径
                                    var newPathString = string.Join(";", pathList);
                                    imagePathsProperty.SetValue(pictureBox, newPathString);
                                }
                                
                                UpdateImagePaths();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    // 对于不支持多图片的控件，保持原来的单图片逻辑
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Title = "选择图片";
                        openFileDialog.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                                UpdateImagePaths();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新图片路径
        /// </summary>
        private void UpdateImagePaths()
        {
            var paths = new List<string>();
            
            foreach (var pictureBox in pictureBoxes)
            {
                // 检查是否有图片路径属性
                var imagePathsProperty = pictureBox.GetType().GetProperty("ImagePaths");
                if (imagePathsProperty != null)
                {
                    var pictureBoxImagePaths = imagePathsProperty.GetValue(pictureBox)?.ToString();
                    if (!string.IsNullOrEmpty(pictureBoxImagePaths))
                    {
                        // 如果MagicPictureBox支持多图片，获取第一个图片路径
                        var pathsArray = pictureBoxImagePaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pathsArray.Length > 0)
                        {
                            paths.Add(pathsArray[0]);
                        }
                    }
                }
                else if (pictureBox.Image != null)
                {
                    // 对于普通PictureBox，这里需要特殊处理
                    // 在实际应用中，可能需要将图片保存到临时位置并记录路径
                    paths.Add("temp_path"); // 占位符，实际使用时需要替换
                }
            }
            
            imagePaths = string.Join(";", paths);
        }

        /// <summary>
        /// 获取所有图片路径
        /// </summary>
        /// <returns>图片路径数组</returns>
        public string[] GetImagePaths()
        {
            return imagePaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        public void AddImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                return;

            // 查找第一个空的图片框
            foreach (var pictureBox in pictureBoxes)
            {
                // 检查MagicPictureBox是否有多图片支持
                var multiImageSupportProperty = pictureBox.GetType().GetProperty("MultiImageSupport");
                if (multiImageSupportProperty != null)
                {
                    var isMultiImageSupport = (bool)multiImageSupportProperty.GetValue(pictureBox);
                    if (isMultiImageSupport)
                    {
                        // 对于支持多图片的控件，检查是否有图片
                        var imagePathsProperty = pictureBox.GetType().GetProperty("ImagePaths");
                        if (imagePathsProperty != null)
                        {
                            var pictureBoxImagePaths = imagePathsProperty.GetValue(pictureBox)?.ToString();
                            if (string.IsNullOrEmpty(pictureBoxImagePaths))
                            {
                                // 设置图片路径
                                imagePathsProperty.SetValue(pictureBox, imagePath);
                                UpdateImagePaths();
                                return;
                            }
                        }
                    }
                    else if (pictureBox.Image == null)
                    {
                        pictureBox.Image = Image.FromFile(imagePath);
                        UpdateImagePaths();
                        return;
                    }
                }
                else if (pictureBox.Image == null)
                {
                    pictureBox.Image = Image.FromFile(imagePath);
                    UpdateImagePaths();
                    return;
                }
            }

            // 如果没有空的图片框，且未达到最大数量，可以考虑增加
            // 但根据设计，我们限制最大数量
            MessageBox.Show("已达到最大图片数量限制", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 清除所有图片
        /// </summary>
        public void ClearImages()
        {
            foreach (var pictureBox in pictureBoxes)
            {
                pictureBox.Image = null;
            }
            imagePaths = "";
        }

        /// <summary>
        /// 获取图片数量
        /// </summary>
        /// <returns>图片数量</returns>
        public int GetImageCount()
        {
            return pictureBoxes.Count(p => p.Image != null);
        }
        
        /// <summary>
        /// 获取所有图片路径
        /// </summary>
        /// <returns>所有图片路径数组</returns>
        public string[] GetAllImagePaths()
        {
            var allPaths = new List<string>();
            
            foreach (var pictureBox in pictureBoxes)
            {
                // 检查MagicPictureBox是否有多图片支持
                var multiImageSupportProperty = pictureBox.GetType().GetProperty("MultiImageSupport");
                if (multiImageSupportProperty != null && (bool)multiImageSupportProperty.GetValue(pictureBox))
                {
                    // 获取图片路径
                    var imagePathsProperty = pictureBox.GetType().GetProperty("ImagePaths");
                    if (imagePathsProperty != null)
                    {
                        var pictureBoxImagePaths = imagePathsProperty.GetValue(pictureBox)?.ToString();
                        if (!string.IsNullOrEmpty(pictureBoxImagePaths))
                        {
                            var paths = pictureBoxImagePaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            allPaths.AddRange(paths);
                        }
                    }
                }
                else if (pictureBox.Image != null)
                {
                    // 对于普通PictureBox，这里需要特殊处理
                    // 在实际应用中，可能需要获取图片的实际路径
                    allPaths.Add("temp_path"); // 占位符
                }
            }
            
            return allPaths.ToArray();
        }
    }
}