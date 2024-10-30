using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ERPBizService;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.Commands;
using RUINORERP.Server.Lib;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.Server;
using TransInstruction;


namespace RUINORERP.Server.ServerSession
{
    public class SessionforBiz : AppSession
    {
        private UserInfo _User = new UserInfo();
        public UserInfo User
        {
            get {

                if (_User.SessionId == null)
                {
                    _User.SessionId = this.SessionID;
                    _User.客户端IP = this.RemoteEndPoint.ToString();
                }
                return _User;
            }
            set
            {
                if (_User != value)
                {
                    _User = value;
                    if (_User == null)
                    {
                        _User = new UserInfo();
                    }
                    if (_User.SessionId == null)
                    {
                        _User.SessionId = this.SessionID;
                        _User.客户端IP = this.RemoteEndPoint.ToString();
                    }
                }
            }
        }
        /// <summary>
        /// 心跳计数器 他并不是一直增加全是会变化
        /// 检测如果一直不变化 则可能掉线
        /// </summary>
        public int HeartbeatCounter;
        public int HeartbeatCounterCheck;

        private ConcurrentDictionary<int, TransDataEntity> _TransDataEntityList = new ConcurrentDictionary<int, TransDataEntity>();
        /// <summary>
        /// 保存攻击数据
        /// </summary>
        public ConcurrentDictionary<int, TransDataEntity> TransDataEntityList { get => _TransDataEntityList; set => _TransDataEntityList = value; }

        //  public PacketProcess _dataSender;
        private string SessionName => "[SessionforBiz]";




        //暂时用这个方法来判断 这个会话 有没有XT过
        public bool UseXT = false;
        protected override async ValueTask OnSessionConnectedAsync()
        {
            // m_二次登陆 = new Dictionary<int, string>();
            //User = new S游戏玩家();
            string msg;
            byte[] head;
            // 发送 256个固定值
            Tools.StrToHex("310631B5316D315B314231D33170319031D43189313931A231AA314A315731A5316031FB31BD31AF3188318A3126313B31253177317C318531DA316031C631AD31D031F531AE31F0310531173120311331B531D731DD3109313331583030316B31BB317F31F331143120314631B4312D31E331D2318831F1315231BE31F131AD315F31D231F7310C3183311931E4314931BC311831EA31053120318B3129311D31663143313B3114312931E8317631F1315231D4315331F431AD31DF318731E0319131F2310431903116318931FF3196312A314931BB319831C731103126319B310731C8310B31A83165314C31D931DD31CC31903185314F31A6313931A1312E", 0, -1, out head, out msg);
            //(this as IAppSession).Server.DataContext
            //监控输出的数据
            // string str = Tools.byteToHexStr(head, true);
            //ServiceforGame<SephirothServer.Server.GamePackageInfo> sg = (this as IAppSession).Server as ServiceforGame<SephirothServer.Server.GamePackageInfo>;
            //  if (GlobalSettings._debug == DebugController.Debug)
            // {
            //     log4netHelper.debug("一连结就，发送包头固定的：" + head.Length.ToString() + "\r\n" + str);
            // }
            //监控输出的数据end

            await (this as IAppSession).SendAsync(head);

            //Console.WriteLine($@"{DateTime.Now} {SessionName} New Session connected: {RemoteEndPoint}.");

            //发送消息给客户端
            // var msg = $@"Welcome to {SessionName}: {RemoteEndPoint}";
            // await (this as IAppSession).SendAsync(Encoding.UTF8.GetBytes(msg + "\r\n"));
        }

        protected override async ValueTask OnSessionClosedAsync(CloseEventArgs e)
        {
            Console.WriteLine($@"{DateTime.Now} {SessionName} Session {RemoteEndPoint} closed: {e.Reason}.");
            //frmServerUI.Instance.SessionListGame
            if (User != null)
            {
                if (User.Online)
                {
                    // Tools.ShowMsg("非正常退出" + player.GameId);
                    // SephirothServer.CommandServer.RoleService.角色退出(this);
                }
                else
                {
                    //Tools.ShowMsg("正常退出" + player.GameId);
                    // SephirothServer.CommandServer.RoleService.角色退出(this);
                }
            }


            await Task.Delay(0);
        }

        public void Send(byte[] data)
        {
            (this as IAppSession).SendAsync(data);
        }

        /// <summary>
        /// 只是会话中处理的一个自定方法。名字改
        /// </summary>
        /// <param name="temperature"></param>
        /// <param name="humidity"></param>
        public void ReceiveData(float temperature, short humidity)
        {
            Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Receive data: " +
                              $@"temperature = {temperature}℃, " +
                              $@"humidity = {humidity}%.");
        }

        //思路 每个都有对应的队列来控制所有的数据发出
        //在角色上线后执行
        // private Queue<byte[]> DataQueue = new Queue<byte[]>();
        public ConcurrentQueue<byte[]> DataQueue = new ConcurrentQueue<byte[]>();


        /// <summary>
        /// 添加加密后的数据
        /// 通常这个用于广播。一次加密码好多次使用
        /// </summary>
        /// <param name="gde"></param>
        public void AddSendData(EncryptedData gde)
        {
            if (gde.head.Length > 0)
            {
                DataQueue.Enqueue(gde.head);
            }
            else
            {
                Tools.ShowMsg("包头长居然为0!");
            }
            if (gde.one.Length > 0)
            {
                DataQueue.Enqueue(gde.one);
            }
            if (gde.two.Length > 0)
            {
                DataQueue.Enqueue(gde.two);
            }

        }


        public void AddSendData(OriginalData d)
        {
            EncryptedData gde = PacketProcess.EncryptedDataPack(d.cmd, d.One, d.Two);
            AddSendData(gde);
        }

        public void AddSendData(byte cmd, byte[] one, byte[] two)
        {
            //pp.WriteClientData((byte)SephirothInstruction.ServerCmd.ISMainMsg.主动10, null, tx.toByte());
            OriginalData gd = new OriginalData();
            gd.cmd = cmd;
            gd.One = one;
            gd.Two = two;
            EncryptedData gde = PacketProcess.EncryptedDataPack(gd.cmd, gd.One, gd.Two);
            AddSendData(gde);


            //再执行一次就会变成解码了
            //TransPackProcess tpp = new TransPackProcess();
            //byte[] buffer = Tool4DataProcess.HexStrTobyte(tpp.ServerPackingAsHexString(gd));


        }









    }
}
