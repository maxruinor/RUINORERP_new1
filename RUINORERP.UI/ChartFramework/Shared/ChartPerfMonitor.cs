using LiveChartsCore.Kernel.Sketches;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Diagnostics
{
    // Diagnostics/ChartPerfMonitor.cs
    public class ChartPerfMonitor : IDisposable
    {
        private readonly Stopwatch _renderWatch = new();
        private readonly IChartView _chart;

        public ChartPerfMonitor(IChartView chart)
        {
            _chart = chart;
           // _chart.Measuring += OnStartMeasuring;
            //_chart.UpdateFinished += OnFinishMeasuring;
        }

        private void OnStartMeasuring()
        {
            _renderWatch.Restart();
            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
        }

        private void OnFinishMeasuring()
        {
            _renderWatch.Stop();
            LogPerfMetrics(_renderWatch.ElapsedMilliseconds);
        }

        private void LogPerfMetrics(long elapsedMs)
        {
            var metrics = new
            {
                TimestampUtc = DateTime.UtcNow,
                RenderTime = elapsedMs,
               // SeriesCount = _chart.Series.Count,
               // DataPoints = _chart.Series.Sum(s => s.Values?.Count ?? 0),
                MemoryUsage = GC.GetTotalMemory(false) / 1024
            };

           // TelemetryClient.TrackEvent("ChartRender", metrics);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
