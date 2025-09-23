using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Protocol;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;



namespace RUINORERP.UI.SuperSocketClient
{
    /// <summary>
    /// sokcet功能：消息IM，操作状态，认证在线情况。T人，注册企业。人数同时在线限制
    /// </summary>
    public class SocketSession
    {
        #region 当前用户信息
        private List<UserInfo> userInfos = new List<UserInfo>();
        public List<UserInfo> UserInfos { get => userInfos; set => userInfos = value; }

        private UserInfo _currentUser = new UserInfo();
        public UserInfo CurrentUser { get => _currentUser; set => _currentUser = value; }

        #endregion

        public bool Connected { get => _Connected; set => _Connected = value; }

        private bool _Connected = false;


        #region 线程

        private Thread _threadWorker;
        private readonly AutoResetEvent _stopThreadEvent;

        #endregion

        private int _port;

        private readonly SocketClientHelper _socketClientHelper;

        public SocketSession()
        {
            _socketClientHelper = new SocketClientHelper();
            _socketClientHelper.Connected += OnSocketConnected;
            _socketClientHelper.Closed += OnSocketClosed;
            _socketClientHelper.Error += OnSocketError;
            _socketClientHelper.DataReceived += OnDataReceived;
            _stopThreadEvent = new AutoResetEvent(false);

        }

        public void Start(string ip, int port)
        {
            _port = port;
            MainForm.Instance.PrintInfoLog($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 正在连接Socket服务器");
            _socketClientHelper.Start(ip, _port);

            _stopThreadEvent.Reset();
            _threadWorker = new Thread(DoWork)
            {
                IsBackground = true
            };
            _threadWorker.Start();
        }

        public void Stop()
        {
            _socketClientHelper.Stop();
            _stopThreadEvent.Set();
            if (_threadWorker != null)
            {
                _threadWorker.Join();
                _threadWorker = null;
            }
        }

        #region Socket event handlers

        private void OnSocketConnected()
        {
            Connected = true;
            MainForm.Instance.PrintInfoLog($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Socket服务器连接成功");
        }

        private void OnSocketClosed()
        {
            Connected = false;
            MainForm.Instance.PrintInfoLog($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Socket服务器断开，正在重新连接");
            Thread.Sleep(10);
            //重新连接
            _socketClientHelper.Start(@"127.0.0.1", _port);
        }

        private void OnSocketError(Exception e)
        {
            Connected = false;
            //Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Socket异常:{e.Message}，重新连接服务器");
            MainForm.Instance.PrintInfoLog($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Socket异常:{e.Message}，重新连接服务器");
            Thread.Sleep(10);
            _socketClientHelper.Start(@"127.0.0.1", _port);
        }



        private void OnDataReceived(byte[] data, int offset, int length)
        {
            var buffer = new byte[length];
            Buffer.BlockCopy(data, offset, buffer, 0, length);
            if (true)
            {
                #region  

                PackageSourceType pst = new PackageSourceType();
                pst = PackageSourceType.Server;
                try
                {
                    TransProtocol tester = new TransProtocol();
                    TransPackProcess gpp = new TransPackProcess();
                    TransService ise = new TransService();

                    OriginalData gd = gpp.UnServerPack(buffer);
                    if (pst == PackageSourceType.Server)
                    {
                        string rs = string.Empty;// = tester.CheckServerCmd(gd);
                                                 //ServerCmdEnum msg = tester.GetServerCmd(gd);
                        ServerCmdEnum msg = (ServerCmdEnum)gd.Cmd;
                        switch (msg)
                        {
                            case ServerCmdEnum.未知指令:
                                break;
                            case ServerCmdEnum.用户登陆回复:
                               // ClientService.用户登陆回复(this, gd);
                                break;
                            case ServerCmdEnum.发送在线列表:
                                //ClientService.接收在线用户列表(this, gd);
                                break;
                            case ServerCmdEnum.心跳回复:
                                break;
                            default:
                                break;
                        }
                        //ise.ClientActionDefault
                        rs += TransService.PorcessServerMsg(msg, TransService.ServerActionList[msg], gd);
                        rs += msg.ToString() + "|";
                        MainForm.Instance.PrintInfoLog("【收到服务器数据】" + rs.ToString());

                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.PrintInfoLog(ex.Message);
                }
                #endregion
            }
        }




        #endregion
        public void Send(byte[] sendData)
        {
            _socketClientHelper.Send(sendData);
        }
      

        #region GetRandomData

        private static byte[] GetRandomData(Random random)
        {
            var value1 = random.Next(0, 500) / 10f; //0~50℃
            var value2 = random.Next(30, 60); //30~60%
            var temperature = (short)(value1 * 10); //温度
            var humidity = (short)(value2 * 10); //湿度
            Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} temperature: {value1}℃, humidity: {value2}%.");

            var dataBuffer = new byte[4];
            dataBuffer[0] = (byte)(temperature >> 8);
            dataBuffer[1] = (byte)(temperature & 0xFF);
            dataBuffer[2] = (byte)(humidity >> 8);
            dataBuffer[3] = (byte)(humidity & 0xFF);

            var header = new byte[] { 0xFF, 0xFE };
            var len = (short)(dataBuffer.Length + 5);
            var lenBuffer = GetBuffer(len);

            var cks = GetSumData(header);
            cks += GetSumData(lenBuffer);
            cks += GetSumData(dataBuffer);

            var buffer = new byte[len];

            //Header
            var offset = 0;
            Buffer.BlockCopy(header, 0, buffer, offset, header.Length);
            //Length
            offset += header.Length;
            Buffer.BlockCopy(lenBuffer, 0, buffer, offset, lenBuffer.Length);
            //Data
            offset += lenBuffer.Length;
            Buffer.BlockCopy(dataBuffer, 0, buffer, offset, dataBuffer.Length);
            //CKS
            offset += dataBuffer.Length;
            buffer[offset] = cks;

            return buffer;
        }

        private static byte[] GetBuffer(short value)
        {
            var buffer = new byte[2];
            buffer[0] = (byte)(value << 8 & 0xFF); //高位
            buffer[1] = (byte)(value & 0x00FF); //低位
            return buffer;
        }

        private static byte GetSumData(byte[] data)
        {
            return data.Aggregate<byte, byte>(0, (current, item) => (byte)(current + item));
        }

        #endregion

        public ConcurrentQueue<byte[]> DataQueue = new ConcurrentQueue<byte[]>();

        private void DoWork()
        {
            var random = new Random(DateTime.Now.Millisecond);
            while (true)
            {
                Thread.Sleep(300);
                //var data = GetRandomData(random);
                while (DataQueue.Count > 0)
                {
                    byte[] sendData;
                    bool rs = DataQueue.TryDequeue(out sendData);
                    if (sendData != null && rs)
                    {
                        if (sendData.Length > 0)
                        {
                            _socketClientHelper.Send(sendData);
                        }
                    }
                }

                //10s发送一次数据
                if (_stopThreadEvent.WaitOne(100))
                {
                    break;
                }
            }

            Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} DoWork exit.");
        }


        #region 发送数据

        /// <summary>
        /// 添加加密后的数据
        /// 通常这个用于广播。一次加密码好多次使用
        /// </summary>
        /// <param name="gde"></param>
        public void AddSendData(byte[] buffer)
        {
            if (buffer.Length > 0)
            {
                DataQueue.Enqueue(buffer);
            }
        }


        public void AddSendData(OriginalData d)
        {
          
            byte[] buffer = CryptoProtocol.EncryptClientPackToServer(d);
            AddSendData(buffer);
        }

        public void AddSendData(byte cmd, byte[] one, byte[] two)
        {
            //pp.WriteClientData((byte)SephirothInstruction.ServerCmd.ISMainMsg.主动10, null, tx.ToByteArray());
            OriginalData gd = new OriginalData();
            gd.Cmd = cmd;
            gd.One = one;
            gd.Two = two;
    
            byte[] buffer = CryptoProtocol.EncryptClientPackToServer(gd);
            AddSendData(buffer);
        }

        #endregion

    }
}
