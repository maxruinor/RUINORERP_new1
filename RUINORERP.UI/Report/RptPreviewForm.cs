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
                // 在后台线程准备报表数据，避免UI阻塞
                await Task.Run(() =>
                {
                    try
                    {
                        // 后台线程只负责报表数据的准备工作
                        // UI控件操作必须回到UI线程执行
                    }
                    catch (Exception ex)
                    {
                        // 使用Invoke在UI线程显示错误信息
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show($"报表数据准备失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        });
                    }
                }).ConfigureAwait(false);
                
                // 回到UI线程执行报表预览显示
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        ShowReportPreview();
                    });
                }
                else
                {
                    ShowReportPreview();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预览加载失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 在UI线程中安全显示报表预览
        /// </summary>
        private void ShowReportPreview()
        {
            try
            {
                MyReport.Preview = previewControl1; //设置报表的Preview控件
                MyReport.ShowPrepared();  //显示
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预览显示失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void 导出PDFToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
