using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.WinForms;
using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Windows.Forms;

namespace RUINORERP.UI.ChartFramework.Rendering.WinForms
{
    /// <summary>
    /// 增强图表控件 (简化版)
    /// </summary>
    public partial class EnhancedChartControl : UserControl
    {
        private readonly CartesianChart _chartHost;
        private readonly ChartData _data;

        public EnhancedChartControl(CartesianChart chartHost, ChartData data)
        {
            _chartHost = chartHost;
            _data = data;
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            this.SuspendLayout();
            this.Controls.Add(_chartHost);
            _chartHost.Dock = DockStyle.Fill;
            this.ResumeLayout(false);
        }
    }
}

