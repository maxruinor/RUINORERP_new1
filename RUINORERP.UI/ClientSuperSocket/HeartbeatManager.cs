using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using TransInstruction;
using TransInstruction.DataPortal;

namespace RUINORERP.UI.ClientSuperSocket
{
    public class HeartbeatManager : IDisposable
    {
        private readonly object _lock = new object();
        private volatile bool _isRunning;
        private Thread _heartbeatThread;
        private CancellationTokenSource _cts;



        public void Start()
        {
            lock (_lock)
            {
                if (_isRunning) return;

                _isRunning = true;
                _cts = new CancellationTokenSource();
                _heartbeatThread = new Thread(HeartbeatLoop);
                _heartbeatThread.Name = "Heartbeat Thread";
                _heartbeatThread.IsBackground = true; // 关键！设置为后台线程
                _heartbeatThread.Start();

            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (!_isRunning) return;

                _isRunning = false;
                _cts?.Cancel(); // 触发取消

                // 最多等待2秒（避免无限阻塞）
                if (_heartbeatThread != null && _heartbeatThread.IsAlive)
                {
                    _heartbeatThread.Join(2000);
                }

                _cts?.Dispose();
                _cts = null;
                _heartbeatThread = null;

            }
        }

        private void HeartbeatLoop()
        {
            while (_isRunning && !_cts.IsCancellationRequested)
            {
                try
                {
                    SendHeartbeat();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending heartbeat: {ex.Message}");
                }

                // 使用Cancellation的WaitHandle替代Thread.Sleep
                _cts.Token.WaitHandle.WaitOne(2000); // 可被取消的等待
            }

            //while (true)
            //{
            //    bool isRunning;
            //    lock (_lock)
            //    {
            //        isRunning = _isRunning;
            //    }

            //    if (!isRunning)
            //    {
            //        break;
            //    }

            //    try
            //    {
            //        // 发送心跳包的逻辑
            //        SendHeartbeat();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error sending heartbeat: {ex.Message}");
            //    }

            //    // 等待2秒
            //    Thread.Sleep(2000);
            //}
        }

        private void SendHeartbeat()
        {
            // 在这里实现发送心跳包的逻辑
            //Console.WriteLine($"发送心跳包，线程ID: {Thread.CurrentThread.ManagedThreadId}");

            //心跳包
            if (MainForm.Instance != null && MainForm.Instance.ecs.client.IsConnected && MainForm.Instance.ecs.LoginStatus)
            {
                try
                {
                    if (MainForm.Instance.AppContext.CurUserInfo != null)
                    {
                        OriginalData beatData = HeartbeatCmdBuilder();
                        //Console.WriteLine($"心跳 Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                        MainForm.Instance.ecs.AddSendData(beatData);
                    }
                }
                catch (Exception)
                {


                }
            }

        }


        /// <summary>
        /// 客户端的心跳包
        /// </summary>
        /// <returns></returns>
        public static OriginalData HeartbeatCmdBuilder()
        {
            OriginalData gd = new OriginalData();
            try
            {
                if (MainForm.HeartbeatCounter == 256)
                {
                    MainForm.HeartbeatCounter = 0;
                }
                var tx = new ByteBuff(36);
                tx.PushInt16(MainForm.HeartbeatCounter++); //累加数
                tx.PushInt(0);
                tx.PushInt64(MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);
                if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null)
                {
                    tx.PushString(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name);
                }
                else
                {
                    tx.PushString("");
                }
                tx.PushString(System.Environment.MachineName.PadRight(20) + ":" + MainForm.Instance.AppContext.CurrentUser.客户端版本);
                MainForm.Instance.AppContext.CurrentUser.静止时间 = MainForm.GetLastInputTime();
                tx.PushInt64(MainForm.Instance.AppContext.CurrentUser.静止时间);//电脑空闲时间
                tx.PushString(MainForm.Instance.AppContext.log.Path);
                tx.PushString(MainForm.Instance.AppContext.log.ModName);
                if (MainForm.Instance.ecs != null && MainForm.Instance.AppContext.CurrentUser.在线状态 != MainForm.Instance.ecs.IsConnected)
                {
                    MainForm.Instance.AppContext.CurrentUser.在线状态 = MainForm.Instance.ecs.IsConnected;
                }
                tx.PushBool(MainForm.Instance.AppContext.CurrentUser.在线状态);

                //Console.WriteLine($"esc: {Thread.CurrentThread.ManagedThreadId}");

                tx.PushBool(MainForm.Instance.AppContext.CurrentUser.授权状态);
                tx.PushString(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//客户端时间，用来对比服务器的时间，如果多个客户端时间与服务器不一样。则服务器有问题。相差一个小时以上。就直接断开客户端
                gd.cmd = (byte)ClientCmdEnum.客户端心跳包;
                gd.One = null;
                gd.Two = tx.toByte();
                //  RoleService.实时更新相关数据(PlayerSession);
            }
            catch (Exception)
            {

            }
            return gd;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
