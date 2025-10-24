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
    public partial class TestViewerForm : Form
    {
        private Button btnTestSingle;
        private Button btnTestMultiple;

        public TestViewerForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnTestSingle = new Button();
            this.btnTestMultiple = new Button();
            this.SuspendLayout();
            
            // 
            // btnTestSingle
            // 
            this.btnTestSingle.Location = new Point(50, 50);
            this.btnTestSingle.Name = "btnTestSingle";
            this.btnTestSingle.Size = new Size(150, 40);
            this.btnTestSingle.TabIndex = 0;
            this.btnTestSingle.Text = "测试单图片查看器";
            this.btnTestSingle.UseVisualStyleBackColor = true;
            this.btnTestSingle.Click += new EventHandler(this.BtnTestSingle_Click);
            
            // 
            // btnTestMultiple
            // 
            this.btnTestMultiple.Location = new Point(50, 120);
            this.btnTestMultiple.Name = "btnTestMultiple";
            this.btnTestMultiple.Size = new Size(150, 40);
            this.btnTestMultiple.TabIndex = 1;
            this.btnTestMultiple.Text = "测试多图片查看器";
            this.btnTestMultiple.UseVisualStyleBackColor = true;
            this.btnTestMultiple.Click += new EventHandler(this.BtnTestMultiple_Click);
            
            // 
            // TestViewerForm
            // 
            this.ClientSize = new Size(250, 200);
            this.Controls.Add(this.btnTestSingle);
            this.Controls.Add(this.btnTestMultiple);
            this.Name = "TestViewerForm";
            this.Text = "图片查看器测试";
            this.ResumeLayout(false);
        }

        private void BtnTestSingle_Click(object sender, EventArgs e)
        {
            // 创建单图片查看器实例
            var viewer = new frmPictureViewer();
            // 这里可以设置图片，例如：
            // viewer.PictureBoxViewer.Image = someImage;
            viewer.ShowDialog();
        }

        private void BtnTestMultiple_Click(object sender, EventArgs e)
        {
            // 创建多图片查看器实例
            var imagePaths = new List<string>
            {
                @"C:\temp\image1.jpg",
                @"C:\temp\image2.png",
                @"C:\temp\image3.bmp"
            };
            
            var viewer = new frmPictureViewer(imagePaths, 0);
            viewer.ShowDialog();
        }
    }
}