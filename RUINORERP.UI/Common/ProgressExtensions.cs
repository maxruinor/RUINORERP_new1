using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public static class ProgressExtensions
    {
        public static void ReportSafeProgress(
            this BackgroundWorker worker,
            int percentage,
            string message = null)
        {
            try
            {
                worker.ReportProgress(
                    Clamp(percentage, 0, 100),
                    message ?? $"Progress: {percentage}%");
            }
            catch (InvalidOperationException)
            {
                // 忽略已完成的进度报告
            }
        }
        private static int Clamp(int value, int min, int max)
        {
            return value < min ? min : (value > max ? max : value);
        }
        public static bool ShouldContinue(this BackgroundWorker worker)
        {
            return !worker.CancellationPending;
        }
    }
}
