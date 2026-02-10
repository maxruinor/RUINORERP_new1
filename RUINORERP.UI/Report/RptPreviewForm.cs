using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport;
using FastReport.Data;
using FastReport.Export.Pdf;
using FastReport.Preview;
using FastReport.Dialog;


namespace RUINORERP.UI.Report
{
    public partial class RptPreviewForm : KryptonForm
    {
        private string reprotfileName = string.Empty;

        public RptPreviewForm()
        {
            InitializeComponent();
        }
        private FastReport.Report myReport; //新建一个私有变量

        public FastReport.Report MyReport { get => myReport; set => myReport = value; }

        private async void RptPreviewForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 在后台线程加载预览，避免UI阻塞
                await Task.Run(() =>
                {
                    try
                    {
                        MyReport.Preview = previewControl1; //设置报表的Preview控件
                        MyReport.ShowPrepared();  //显示
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"预览加载失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预览加载失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void 导出PDFToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
