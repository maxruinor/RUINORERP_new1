using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateTools
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }





        private void btnCreator_Click(object sender, EventArgs e)
        {
            string path = txtSourcePath.Text;
            List<string> files = GetFiles(path);
        }

        private List<string> GetFiles(string path)
        {
            List<string> files = new List<string>();
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] fileInfos = root.GetFiles();
            foreach (FileInfo f in fileInfos)
            {
                files.Add(f.FullName);
            }

            //目录
            DirectoryInfo rootDir = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
               files.AddRange(GetFiles(d.FullName));
            }

            return files;
        }
        


        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                // 设置默认显示路径（可选）
                folderBrowser.SelectedPath = "C:\\";

                // 打开文件夹选择对话框
                DialogResult result = folderBrowser.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string selectedFolderPath = folderBrowser.SelectedPath;
                    txtSourcePath.Text = selectedFolderPath;
                    // TODO: 处理所选的文件夹路径

                    // 将所选的文件夹路径添加到历史记录列表中
                    //AddToHistory(selectedFolderPath);
                }
            }
        }
    }


}
