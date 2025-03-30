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
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // 添加图表控件
            if (chartView is Control chartControl)
            {
                chartView.DataPointerDown += (s, e) =>
                {
                    
                };
                chartControl.Dock = DockStyle.Fill;
                this.Controls.Add(chartControl);
            }

            // 初始化右键菜单
            _menu = new ContextMenuStrip();
            AddDefaultMenuItems();

            // 绑定右键点击事件
            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    _menu.Show(this, e.Location);
                }
            };
        }

        private void AddDefaultMenuItems()
        {
            _menu.Items.Add("导出Excel", null, (s, e) =>
            {
                using var dialog = new SaveFileDialog
                {
                    Filter = "Excel文件|*.xlsx",
                    FileName = _dataSet.Title + ".xlsx"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 这里实现导出逻辑
                    // _dataSet.ExportToExcel(dialog.FileName);
                    MessageBox.Show("导出成功!");
                }
            });

            _menu.Items.Add("复制数据", null, (s, e) =>
            {
                var dt = _dataSet.ToDataTable();
                Clipboard.SetDataObject(dt);
                MessageBox.Show("数据已复制到剪贴板");
            });
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
