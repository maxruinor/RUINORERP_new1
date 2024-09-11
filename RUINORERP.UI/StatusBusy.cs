using System;
using System.Windows.Forms;

namespace RUINORERP.UI
{
    public class StatusBusy : IDisposable
    {
        private string _oldStatus;
        private Cursor _oldCursor;

        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();

        public StatusBusy(string statusText)
        {
            st.Start();
            //            _oldStatus = MainForm.Instance.SystemOperatorState.Text;
            _oldStatus = MainForm.Instance.lblStatusGlobal.Text;
            //MainForm.Instance.SystemOperatorState.Text = statusText + "...";
            MainForm.Instance.lblStatusGlobal.Text= statusText + "...";
            _oldCursor = MainForm.Instance.Cursor;
            MainForm.Instance.Cursor = Cursors.WaitCursor;
            Application.DoEvents();

        }

        // IDisposable
        private bool _disposedValue = false; // To detect redundant calls

        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
                if (disposing)
                {
                    st.Stop();
                    //MainForm.Instance.SystemOperatorState.Text = "加载完成,用时:" + st.Elapsed.Add(TimeSpan.FromSeconds(2)).TotalMinutes.ToString("0.000000") + "秒";
                    //MainForm.Instance.SystemOperatorState.Text = _oldStatus;
                    MainForm.Instance.lblStatusGlobal.Text = "加载完成,用时:" + st.Elapsed.Add(TimeSpan.FromSeconds(2)).TotalMinutes.ToString("0.000000") + "秒";
                    MainForm.Instance.lblStatusGlobal.Text = _oldStatus;
                    MainForm.Instance.Cursor = Cursors.Arrow;
                    MainForm.Instance.Refresh();
                }
            _disposedValue = true;
        }








        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
