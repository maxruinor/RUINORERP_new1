using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.WinForms;
using Org.BouncyCastle.Asn1.Crmf;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Drawing;
using LiveChartsCore.SkiaSharpView;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using RUINORERP.UI.CommonUI;
using NPOI.SS.UserModel.Charts;

namespace RUINORERP.UI.ChartFramework.Rendering.Controls
{
    public class ChartControl : UserControl
    {
        private readonly IChartView _chartView;
        private readonly ChartData _dataSet;
        private readonly ContextMenuStrip _menu;

        public IChartView ChartView => _chartView;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartView"></param>
        /// <param name="dataSet">导出数据源</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ChartControl(IChartView chartView, ChartData dataSet)
        {
            _chartView = chartView ?? throw new ArgumentNullException(nameof(chartView));
            _dataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));

            // 设置控件属性
            //设置后可能不显示
            //this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;


            // 初始化右键菜单
            _menu = new ContextMenuStrip();
            AddDefaultMenuItems(dataSet);

            // 绑定右键点击事件
            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    _menu.Show(this, e.Location);
                }

                if (e.Button == MouseButtons.Right && _chartView is CartesianChart cartesian)
                {
                    // 处理右键逻辑...
                }

            };

            // 添加图表控件
            if (chartView is Control chartControl)
            {
                chartControl.ContextMenuStrip = _menu;
                chartView.DataPointerDown += (s, e) =>
                {

                };
                chartControl.Dock = DockStyle.Fill;
                this.Controls.Add(chartControl);
            }
        }
        public async Task ExportChartDataToExcel(ChartData chartData, string defaultFileName = null)
        {
            try
            {
                if (chartData == null)
                {
                    MessageBox.Show("没有可导出的图表数据");
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel 文件 (*.xlsx)|*.xlsx",
                    Title = "保存图表数据",
                    FileName = defaultFileName ?? $"图表数据_{DateTime.Now:yyyyMMddHHmmss}",
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var progressForm = new ProgressForm("正在导出图表数据..."))
                    {
                        progressForm.Show();
                        Application.DoEvents();

                        await Task.Run(() =>
                        {
                            try
                            {
                                using (var fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                                {
                                    IWorkbook workbook = new XSSFWorkbook();

                                    // 导出主数据
                                    ISheet mainSheet = workbook.CreateSheet("图表数据");

                                    // 添加标题
                                    IRow titleRow = mainSheet.CreateRow(0);
                                    titleRow.CreateCell(0).SetCellValue(chartData.Title);

                                    // 添加类别标签
                                    IRow categoryRow = mainSheet.CreateRow(2);
                                    categoryRow.CreateCell(0).SetCellValue("类别");
                                    for (int i = 0; i < chartData.CategoryLabels.Length; i++)
                                    {
                                        categoryRow.CreateCell(i + 1).SetCellValue(chartData.CategoryLabels[i]);
                                    }

                                    // 添加系列数据
                                    int rowIndex = 3;
                                    foreach (var series in chartData.Series)
                                    {
                                        IRow seriesRow = mainSheet.CreateRow(rowIndex++);
                                        seriesRow.CreateCell(0).SetCellValue(series.Name);

                                        for (int i = 0; i < series.Values.Count; i++)
                                        {
                                            seriesRow.CreateCell(i + 1).SetCellValue(series.Values[i]);
                                        }
                                    }

                                    // 添加额外信息
                                    IRow infoRow = mainSheet.CreateRow(rowIndex + 2);
                                    infoRow.CreateCell(0).SetCellValue($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                                    workbook.Write(fs);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        });

                        progressForm.Close();
                    }

                    if (MessageBox.Show("图表数据导出成功，是否立即打开文件？", "导出完成",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出图表数据出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



       
        private void AddDefaultMenuItems(ChartData dataSet)
        {
            _menu.Items.Add("导出Excel", null, async (s, e) =>
            {
                    // 这里实现导出逻辑
                    // 导出图表数据
                    await ExportChartDataToExcel(dataSet, dataSet.Title + ".xlsx");
                
            });

            //_menu.Items.Add("复制数据", null, (s, e) =>
            //{
            //    var dt = _dataSet.ToDataTable();
            //    Clipboard.SetDataObject(dt);
            //    MessageBox.Show("数据已复制到剪贴板");
            //});
        }

        public void AddMenuItem(string text, Action<ChartData> action)
        {
            _menu.Items.Add(text, null, (s, e) => action(_dataSet));
        }

        public void UpdateData(ChartData newData)
        {
            //_dataSet = newData;
            //if (_chartView is CartesianChart cartesianChart)
            //{
            //    cartesianChart.Series = newData.Series..ToArray();
            //    cartesianChart.XAxes = new Axis[] { /* 更新X轴 */ };
            //    cartesianChart.YAxes = new Axis[] { /* 更新Y轴 */ };
            //}
            //else if (_chartView is PieChart pieChart)
            //{
            //    pieChart.Series = newData.Series.ToArray();
            //}
            // 其他图表类型的更新逻辑...
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ChartControl
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "ChartControl";
            this.Size = new System.Drawing.Size(438, 411);
            this.ResumeLayout(false);

        }
    }
}
