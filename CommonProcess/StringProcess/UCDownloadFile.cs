using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonProcess.StringProcess
{
    public partial class UCDownloadFile : UCMyBase, IUCBase
    {
        public string ImgSaveDirectory { get; set; }

        public UCDownloadFile()
        {
            InitializeComponent();
        }

        public void SaveDataFromUI(UCBasePara aa)
        {
            UCDownloadFilePara para = new UCDownloadFilePara();
            para = aa as UCDownloadFilePara;

            para.Is缓存HTML内容 = chk缓存HTML内容.Checked;
            para.HTML缓存内容 = txtEditorControlHTML缓存内容.Text;
            para.Is将相对地址补全为绝对地址 = chk将相对地址补全为绝对地址.Checked;
            para.Is探测文件地址并下载 = chk探测文件并直接下载.Checked;
            para.Is下载图片 = chk下载图片.Checked;
            para.ImgSaveDirectory = txtImgSaveDirectory.Text;
            para.IsAbsolutePath = chk保存时使用相对路径还是绝对路径.Checked;
            para.MultipleFileDelimiter = txt分割字符.Text;
            if (cmb下载文件名保存格式.SelectedItem != null)
            {
                para.FileSaveformat = cmb下载文件名保存格式.SelectedItem.ToString();
            }
            para.DownloadTypeisHtmlPage = rdbHtml网页.Checked;
        }

        public void LoadDataToUI(UCBasePara aa)
        {
            UCDownloadFilePara para = new UCDownloadFilePara();
            para = aa as UCDownloadFilePara;
            if (para.DownloadTypeisHtmlPage)
            {
                rdbHtml网页.Checked = true;
            }
            else
            {
                rdbFile.Checked = true;
            }
            txt分割字符.Text = para.MultipleFileDelimiter; 
            chk缓存HTML内容.Checked = para.Is缓存HTML内容;
            txtEditorControlHTML缓存内容.Text = para.HTML缓存内容;
            cmb下载文件名保存格式.SelectedIndex = cmb下载文件名保存格式.FindString(para.FileSaveformat);
            chk保存时使用相对路径还是绝对路径.Checked = para.IsAbsolutePath;
            chk将相对地址补全为绝对地址.Checked = para.Is将相对地址补全为绝对地址;
            chk探测文件并直接下载.Checked = para.Is探测文件地址并下载;
            chk下载图片.Checked = para.Is下载图片;
            txtImgSaveDirectory.Text = para.ImgSaveDirectory;

        }

        private void chk缓存HTML内容_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chk保存时使用相对路径还是绝对路径_CheckedChanged(object sender, EventArgs e)
        {
            if (chk保存时使用相对路径还是绝对路径.Checked)
            {
                if (!System.IO.Directory.Exists(ImgSaveDirectory))
                {
                    MessageBox.Show("请在【文件保存及部分高级设置】中设置好图片文件保存目录。");
                }

            }
        }

        private void btnFindNeedPathForIMG_Click(object sender, EventArgs e)
        {
            string sourcePath = string.Empty;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                sourcePath = fbd.SelectedPath;
                txtImgSaveDirectory.Text = sourcePath;
            }
        }

        private void rdbHtml网页_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
