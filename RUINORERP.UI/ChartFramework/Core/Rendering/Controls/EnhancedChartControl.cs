using LiveChartsCore.Kernel.Sketches;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ChartFramework.Rendering.WinForms
{
    // Controls/EnhancedChartControl.cs
    public partial class EnhancedChartControl : UserControl
    {
        private readonly ChartDataSet _data;

        public EnhancedChartControl(IChartView chartView, ChartDataSet data)
        {
            _data = data;
            InitializeChartHost(chartView);
            InitializeContextMenu();
        }

        private void InitializeContextMenu()
        {
            var menu = new ContextMenuStrip();

            // 基础菜单项
            menu.Items.Add("导出Excel", null, (_, _) => _data.ExportToExcel());

            // 动态添加元数据驱动的菜单项
            if (_data.MetaData.AllowDrillDown)
            {
                menu.Items.Add("钻取详情", null, OnDrillDown);
            }

            this.ContextMenuStrip = menu;
        }

        private void OnDrillDown(object sender, EventArgs e)
        {
            var point = GetSelectedDataPoint();
            ShowDrillDownWindow(point);
        }
    }
}
