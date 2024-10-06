using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.WebServerConsole
{
    static class AppExit
    {
        public async static void WaitFor(CancellationTokenSource cts, params Task[] tasks)
        {
            if (cts == null)
                throw new ArgumentNullException(nameof(cts));

            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));
            Console.WriteLine("\nCtrl+C was pressed. Canceling tasks...");
            // 注册 Ctrl+C 事件处理
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                eArgs.Cancel = true; // 防止进程被终止
                //cts.Cancel();
                cancelTasks(cts);
            };

            //Task.Run(() =>
            //{
            //    Console.WriteLine("------Press [Enter] to stop------");
            //    Console.ReadLine();

            //    cancelTasks(cts);
            //});
            try
            {
                // 等待所有任务完成或取消信号
                waitTasks(tasks);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Tasks were canceled.");
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + innerEx.Message);
                    Console.ResetColor();
                }
            }
            finally
            {
                cts.Dispose();
                Console.CancelKeyPress -= (sender, eArgs) => { }; // 取消注册
            }

        }

        static void cancelTasks(CancellationTokenSource cts)
        {
            Console.WriteLine("\nWaiting for the tasks to complete...");
            cts.Cancel();
        }

        static void waitTasks(Task[] tasks)
        {
            try
            {
                foreach (var t in tasks) //enables exception handling
                {
                    t.Wait();
                }
            }
            catch (Exception ex)
            {
                writeError(ex);
            }
        }

        static void writeError(Exception ex)
        {
            if (ex == null)
                return;

            if (ex is AggregateException)
                ex = ex.InnerException;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + ex.Message);
            Console.ResetColor();
        }
    }
}
