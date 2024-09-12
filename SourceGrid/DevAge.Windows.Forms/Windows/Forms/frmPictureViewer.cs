using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
    public partial class frmPictureViewer : Form
    {
        public frmPictureViewer()
        {
            InitializeComponent();
            // 初始化菜单和 PictureBox
            InitializeControls();
        }
        private void InitializeControls()
        {
            // 初始化 MenuStrip
            MenuStrip menuStrip1 = new MenuStrip();
            this.Controls.Add(menuStrip1);
            this.MainMenuStrip = menuStrip1;

            // 添加 "Image Size Mode" 菜单项
            ToolStripMenuItem imageSizeModeMenu = new ToolStripMenuItem("图像模式");
            menuStrip1.Items.Add(imageSizeModeMenu);

            // 添加 Normal 子菜单项
            ToolStripMenuItem normalMenuItem = new ToolStripMenuItem("正常");
            imageSizeModeMenu.DropDownItems.Add(normalMenuItem);

            // 添加 AutoSize 子菜单项
            ToolStripMenuItem autoSizeMenuItem = new ToolStripMenuItem("自动");
            imageSizeModeMenu.DropDownItems.Add(autoSizeMenuItem);

            // 添加 CenterImage 子菜单项
            ToolStripMenuItem centerImageMenuItem = new ToolStripMenuItem("居中");
            imageSizeModeMenu.DropDownItems.Add(centerImageMenuItem);

            // 添加 StretchImage 子菜单项
            ToolStripMenuItem stretchImageMenuItem = new ToolStripMenuItem("拉伸");
            imageSizeModeMenu.DropDownItems.Add(stretchImageMenuItem);

            // 添加 Zoom 子菜单项
            ToolStripMenuItem zoomMenuItem = new ToolStripMenuItem("缩放");
            imageSizeModeMenu.DropDownItems.Add(zoomMenuItem);

     
            PictureBoxViewer.SizeMode = PictureBoxSizeMode.Zoom;
            

            // 添加事件处理程序
            normalMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.Normal;
            autoSizeMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            centerImageMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.CenterImage;
            stretchImageMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.StretchImage;
            zoomMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private void frmPictureViewer_Load(object sender, EventArgs e)
        {

        }
    }
}
