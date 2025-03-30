using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.ChartFramework.Rendering.Controls;

namespace RUINORERP.UI.ChartFramework.Shared.Extensions.UI
{
    // Extensions/UI/ChartContextMenuExtensions.cs
    public static class ChartContextMenuExtensions
    {
        public static ChartControl AddExportMenu(this ChartControl chart,
            params (string Text, Action<ChartData> Action)[] customItems)
        {
            // 默认菜单项
           // chart.AddMenuItem("导出Excel", ds => ds.ShowExportDialog());
            chart.AddMenuItem("复制数据", ds => {
                var dt = ds.ToDataTable();
                Clipboard.SetDataObject(dt);
                MessageBox.Show("已复制到剪贴板");
            });

            // 自定义菜单项
            foreach (var item in customItems)
            {
                chart.AddMenuItem(item.Text, item.Action);
            }
            return chart;
        }
    }

    // 扩展方法
    public static class ColumnChartExtensions
    {
        public static ChartControl WithStackedOptions(this ChartControl chart, bool isStacked)
        {
            if (isStacked && chart.ChartView is CartesianChart cartesianChart)
            {
                foreach (var series in cartesianChart.Series.Cast<ColumnSeries<double>>())
                {
                    //series.IsStacked = true;
                    series.DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Middle;
                }
            }
            return chart;
        }
    }

}
