using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINOR.WinFormsUI.CustomPictureBox
{
    public partial class TestForm : Form
    {
        private UCMagicPictrueBoxes ucMagicPictrueBoxes;
        private Button btnLoadImages;
        private Button btnGetPaths;
        private TextBox txtResult;

        public TestForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.ucMagicPictrueBoxes = new UCMagicPictrueBoxes();
            this.btnLoadImages = new Button();
            this.btnGetPaths = new Button();
            this.txtResult = new TextBox();
            this.SuspendLayout();
            
            // 
            // ucMagicPictrueBoxes
            // 
            this.ucMagicPictrueBoxes.Location = new Point(12, 12);
            this.ucMagicPictrueBoxes.Name = "ucMagicPictrueBoxes";
            this.ucMagicPictrueBoxes.Size = new Size(500, 300);
            this.ucMagicPictrueBoxes.TabIndex = 0;
            this.ucMagicPictrueBoxes.MaxImageCount = 5;
            this.ucMagicPictrueBoxes.AllowUpload = true;
            
            // 
            // btnLoadImages
            // 
            this.btnLoadImages.Location = new Point(12, 330);
            this.btnLoadImages.Name = "btnLoadImages";
            this.btnLoadImages.Size = new Size(100, 30);
            this.btnLoadImages.TabIndex = 1;
            this.btnLoadImages.Text = "加载测试图片";
            this.btnLoadImages.UseVisualStyleBackColor = true;
            this.btnLoadImages.Click += new EventHandler(this.BtnLoadImages_Click);
            
            // 
            // btnGetPaths
            // 
            this.btnGetPaths.Location = new Point(130, 330);
            this.btnGetPaths.Name = "btnGetPaths";
            this.btnGetPaths.Size = new Size(100, 30);
            this.btnGetPaths.TabIndex = 2;
            this.btnGetPaths.Text = "获取图片路径";
            this.btnGetPaths.UseVisualStyleBackColor = true;
            this.btnGetPaths.Click += new EventHandler(this.BtnGetPaths_Click);
            
            // 
            // txtResult
            // 
            this.txtResult.Location = new Point(12, 370);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Size = new Size(500, 100);
            this.txtResult.TabIndex = 3;
            
            // 
            // TestForm
            // 
            this.ClientSize = new Size(530, 480);
            this.Controls.Add(this.ucMagicPictrueBoxes);
            this.Controls.Add(this.btnLoadImages);
            this.Controls.Add(this.btnGetPaths);
            this.Controls.Add(this.txtResult);
            this.Name = "TestForm";
            this.Text = "图片控件测试";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnLoadImages_Click(object sender, EventArgs e)
        {
            // 模拟设置一些图片路径
            string testPaths = @"C:\temp\image1.jpg;C:\temp\image2.png;C:\temp\image3.bmp";
            this.ucMagicPictrueBoxes.ImagePaths = testPaths;
            this.txtResult.Text = "已设置测试图片路径";
        }

        private void BtnGetPaths_Click(object sender, EventArgs e)
        {
            string[] paths = this.ucMagicPictrueBoxes.GetAllImagePaths();
            this.txtResult.Text = "获取到的图片路径:\r\n" + string.Join("\r\n", paths);
        }
    }
}