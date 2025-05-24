using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    public static class ControlExtensions
    {
        public static bool In<T>(this T value, params T[] values) where T : Enum
        {
            return values.Contains(value);
        }

        public static Task InvokeAsync(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                var tcs = new TaskCompletionSource<bool>();

                control.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        action();
                        tcs.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }));

                return tcs.Task;
            }
            else
            {
                action();
                return Task.FromResult(true);
            }
        }

        public static void InvokeIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                try
                {
                    control.Invoke(action);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Invoke操作出错: {ex.Message}");
                }
            }
            else
            {
                action();
            }
        }
    }
}
