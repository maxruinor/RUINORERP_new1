using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;
using SourceGrid;
using static RUINORERP.UI.Log.UClog;
namespace RUINORERP.UI
{
    public class UILogManager
    {
        private readonly MainForm _mainForm;
        private readonly Grid _grid;
        private readonly SourceGrid.Cells.Views.Cell _viewGreen;
        private readonly LogPopupMenu _menuController;
        private readonly LogShowController _logShowController;

        // 线程安全的日志队列
        private readonly ConcurrentQueue<(string Type, string Description)> _logQueue = new ConcurrentQueue<(string, string)>();
        private readonly ManualResetEvent _stopEvent = new ManualResetEvent(false);
        private readonly Thread _logProcessingThread;
        private const int MaxLogCount = 100;
        private const int BatchSize = 20;
        private const int ProcessingIntervalMs = 100;

        public UILogManager(MainForm mainForm, Grid grid, SourceGrid.Cells.Views.Cell viewGreen)
        {
            _mainForm = mainForm;
            _grid = grid;
            _viewGreen = viewGreen;
            _menuController = new LogPopupMenu(grid);
            _logShowController = new LogShowController(grid);

            // 启动日志处理线程
            _logProcessingThread = new Thread(ProcessLogQueue);
            _logProcessingThread.IsBackground = true;
            _logProcessingThread.Start();
        }

        public void Dispose()
        {
            _stopEvent.Set();
            if (!_logProcessingThread.Join(1000))
            {
                _logProcessingThread.Abort();
            }
            _stopEvent.Dispose();
        }

        // 外部调用的日志添加方法
        public void AddLog(string type, string description)
        {
            if (string.IsNullOrEmpty(description))
                description = string.Empty;

            // 将日志添加到队列，不直接操作UI
            _logQueue.Enqueue((type, description));
        }

        // 日志处理线程的主方法
        private void ProcessLogQueue()
        {
            while (!_stopEvent.WaitOne(ProcessingIntervalMs))
            {
                if (_logQueue.IsEmpty || !_mainForm.IsHandleCreated)
                    continue;

                try
                {
                    // 批量处理日志，避免频繁更新UI
                    var logsToProcess = new (string Type, string Description)[BatchSize];
                    int count = 0;

                    while (count < BatchSize && _logQueue.TryDequeue(out var log))
                    {
                        logsToProcess[count++] = log;
                    }

                    if (count > 0)
                    {
                        _mainForm.Invoke(new Action(() => ProcessLogsInUiThread(logsToProcess, count)));
                    }
                }
                catch (Exception ex)
                {
                    // 记录日志处理线程中的异常
                    System.Diagnostics.Debug.WriteLine($"Error processing logs: {ex.Message}");
                }
            }
        }

        // 在UI线程中处理日志
        private void ProcessLogsInUiThread((string Type, string Description)[] logs, int count)
        {
            try
            {
                // 挂起布局以优化性能
                _grid.SuspendLayout();

                // 先清理旧日志
                int currentRows = _grid.RowsCount;
                if (currentRows + count > MaxLogCount)
                {
                    int rowsToRemove = (currentRows + count) - MaxLogCount;
                    if (rowsToRemove > 0 && currentRows > 1) // 确保至少保留标题行
                    {
                        _grid.Rows.RemoveRange(1, Math.Min(rowsToRemove, currentRows - 1));
                        AddSystemLog($"自动清除 {rowsToRemove} 行旧日志");
                    }
                }

                // 批量添加新日志
                for (int i = 0; i < count; i++)
                {
                    var (type, description) = logs[i];
                    AddSingleLog(type, description);
                }

                // 确保只显示最后一条日志的状态文本
                if (count > 0)
                {
                    string displayText = logs[count - 1].Description.Trim();
                    if (displayText.Length > 100)
                        displayText = displayText.Substring(0, 100) + "...";
                    _mainForm.ShowStatusText(displayText);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating UI with logs: {ex.Message}");
            }
            finally
            {
                // 恢复布局并强制刷新
                _grid.ResumeLayout(true);
            }
        }

        // 添加单条日志到UI
        private void AddSingleLog(string type, string description)
        {
            // 插入新行到标题行之后（第1行）
            _grid.Rows.Insert(1);

            // 设置新行内容
            _grid[1, 0] = new SourceGrid.Cells.Cell(_grid.Rows.Count - 1);
            _grid[1, 1] = new SourceGrid.Cells.Cell(DateTime.Now);
            _grid[1, 2] = new SourceGrid.Cells.Cell(type);
            _grid[1, 3] = new SourceGrid.Cells.Cell(description);

            // 添加控制器
            for (int i = 0; i < 4; i++)
            {
                _grid[1, i].AddController(_menuController);
                _grid[1, i].AddController(_logShowController);
            }

            // 更新样式
            for (int i = 0; i < 4; i++)
            {
                _grid[1, i].View = _viewGreen;
            }
        }

        // 添加系统日志
        private void AddSystemLog(string message)
        {
            AddSingleLog("system", message);
        }
    }
}

