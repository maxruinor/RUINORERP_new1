using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Commands;
using SuperSocket;
using SuperSocket.Server.Abstractions.Session;
using SuperSocket.Server;
using TransInstruction;
using TransInstruction.DataPortal;
using SuperSocket.Connection;


namespace RUINORERP.Server.ServerSession
{
    public class SessionforBiz : AppSession
    {
        private UserInfo _User = new UserInfo();
        public UserInfo User
        {
            get
            {

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


        //  public PacketProcess _dataSender;
        private string SessionName => "[SessionforBiz]";




        //暂时用这个方法来判断 这个会话 有没有XT过
        public bool UseXT = false;
        protected override async ValueTask OnSessionConnectedAsync()
        {
            try
            {
                //通知客户端一条消息
                OriginalData WelcomeMsg = new OriginalData();
                WelcomeMsg.cmd = (byte)ServerCmdEnum.首次连接欢迎消息;
                WelcomeMsg.One = null;
                ByteBuff tx = new ByteBuff(100);
                tx.PushString("欢迎连接到贝思特服务器，请登录。");
                WelcomeMsg.Two = tx.toByte();
                EncryptedData MsgByte = CryptoProtocol.EncryptionServerPackToClient(WelcomeMsg);
                //三个字节数组合并成一个
                byte[] buffer = new byte[MsgByte.head.Length + MsgByte.one.Length + MsgByte.two.Length];
                Buffer.BlockCopy(MsgByte.head, 0, buffer, 0, MsgByte.head.Length);
                Buffer.BlockCopy(MsgByte.one, 0, buffer, MsgByte.head.Length, MsgByte.one.Length);
                Buffer.BlockCopy(MsgByte.two, 0, buffer, MsgByte.head.Length + MsgByte.one.Length, MsgByte.two.Length);
                await (this as IAppSession).SendAsync(buffer);

                //string msg;
                //byte[] head;
                //// 发送 256个固定值
                //Tool4DataProcess.StrToHex("310631B5316D315B314231D33170319031D43189313931A231AA314A315731A5316031FB31BD31AF3188318A3126313B31253177317C318531DA316031C631AD31D031F531AE31F0310531173120311331B531D731DD3109313331583030316B31BB317F31F331143120314631B4312D31E331D2318831F1315231BE31F131AD315F31D231F7310C3183311931E4314931BC311831EA31053120318B3129311D31663143313B3114312931E8317631F1315231D4315331F431AD31DF318731E0319131F2310431903116318931FF3196312A314931BB319831C731103126319B310731C8310B31A83165314C31D931DD31CC31903185314F31A6313931A1312E", 0, -1, out head, out msg);
                //await (this as IAppSession).SendAsync(head);

                ////发送欢迎消息：在第一次连接时，服务器可能会发送一条欢迎消息或登录提示给客户端，例如：“欢迎连接到服务器，请登录。”
                ////Console.WriteLine($@"{DateTime.Now} {SessionName} New Session connected: {RemoteEndPoint}.");

                ////发送消息给客户端
                //// var msg = $@"Welcome to {SessionName}: {RemoteEndPoint}";
                //// await (this as IAppSession).SendAsync(Encoding.UTF8.GetBytes(msg + "\r\n"));
            }
            catch (Exception exx)
            {

            }

        }

        protected override async ValueTask OnSessionClosedAsync(CloseEventArgs e)
        {
            Console.WriteLine($@"{DateTime.Now} {SessionName} Session {RemoteEndPoint} closed: {e.Reason}.");
            if (User != null)
            {
                if (User.在线状态)
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
         
        public ValueTask SendAsync(byte[] data)
        {
          return  (this as IAppSession).SendAsync(data);
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
            try
            {

          
            if (gde.head.Length > 0)
            {
                DataQueue.Enqueue(gde.head);
            }
            else
            {
                Comm.CommService.ShowExceptionMsg("包头长居然为0!");
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
            catch (Exception ex)
            {
                Console.WriteLine("发送数据时出错：DataQueue AddSendData" + ex.Message);
            }
        }

        public void AddSendData(OriginalData d)
        {
            try
            {
                EncryptedData gde = CryptoProtocol.EncryptionServerPackToClient(d.cmd, d.One, d.Two);
                AddSendData(gde);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送数据时出错：AddSendData" + ex.Message);
            }
        }

        public void AddSendData(byte cmd, byte[] one, byte[] two)
        {
            //pp.WriteClientData((byte)SephirothInstruction.ServerCmd.ISMainMsg.主动10, null, tx.toByte());
            OriginalData gd = new OriginalData();
            gd.cmd = cmd;
            gd.One = one;
            gd.Two = two;
            try
            {
                EncryptedData gde = CryptoProtocol.EncryptionServerPackToClient(gd.cmd, gd.One, gd.Two);
                AddSendData(gde);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送数据时出错：AddSendData" + ex.Message);
            }

            //再执行一次就会变成解码了
            //TransPackProcess tpp = new TransPackProcess();
            //byte[] buffer = Tool4DataProcess.HexStrTobyte(tpp.ServerPackingAsHexString(gd));


        }









    }
}
