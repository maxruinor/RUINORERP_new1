using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.WinForms;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChartSystem.Core.Rendering.Utilities
{

//    // 在WinForms/WPF中的使用
//    var chart = new CartesianChart();
//    var updater = new ThreadSafeChartUpdater(chart);

//    // 后台线程更新
//    Task.Run(() =>
//{
//    var newData = GetDataFromService();
//    updater.UpdateData(chartView => 
//    {
//        chartView.Series = newData.Series;
//        chartView.XAxes = newData.XAxes;
//    });
//});


/// <summary>
/// 线程安全的图表更新器（支持跨线程UI更新）
/// </summary>
public class ThreadSafeChartUpdater
    {
        private readonly IChartView _chartView;
        private readonly SynchronizationContext _syncContext;

        public ThreadSafeChartUpdater(IChartView chartView)
        {
            _chartView = chartView;
            _syncContext = SynchronizationContext.Current
                ?? throw new InvalidOperationException("必须在UI线程初始化");
        }

        public void UpdateData(Action<IChartView> updateAction)
        {
            if (SynchronizationContext.Current == _syncContext)
            {
                updateAction(_chartView);
            }
            else
            {
                _syncContext.Post(_ => updateAction(_chartView), null);
            }
        }
   
    }
}
