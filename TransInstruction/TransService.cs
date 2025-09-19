using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TransInstruction.DataPortal;

namespace TransInstruction
{


    //暂时，外部还是外部，服务内部还是内部，外部只是用来检测 解析分析功能
    //。具体数据还是要内部完成
    public delegate string ServerResponse(ClientCmdEnum msg,OriginalData gd);
    public delegate string ResolveServerActions(ServerCmdEnum msg, OriginalData gd);
    public delegate string ResolveServerActionsNew(OriginalData gd);

    public class TransService
    {

        public static TransService Init()
        {
            return new TransService();
        }
        public TransService()
        {
            AddServerActionList();
            AddClientActionList();

            //AddServerActionListNew();
        }


        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void ClientHandler(ClientCmdEnum msg, OriginalData gd);


        [Browsable(true), Description("引发外部事件，补充作用而已")]
        public static event ClientHandler ClientEvent;



        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void ServerHandler(ServerCmdEnum msg, OriginalData gd, object para);


        [Browsable(true), Description("引发外部事件，补充作用而已")]
        public event ServerHandler ServerEvent;


        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void OtherHandler(ClientCmdEnum msg, OriginalData gd);


        [Browsable(true), Description("引发外部事件，补充作用而已")]
        public static event OtherHandler OtherEvent;

        //public InstructionService()
        //{

        //}

        public string ClientActionDefault(ClientCmdEnum msg, OriginalData gd)
        {
            //if (OtherEvent != null)
            //{
            //    OtherEvent(msg, gd);
            //}
            int index = 0;
            string str = string.Empty;
            switch (msg)
            {
                case ClientCmdEnum.准备登陆:
                    break;
                case ClientCmdEnum.客户端心跳包:
                    str = ActionForServer.解析客户端发来的心跳包(gd);
                
                    index = 2;
                    
                    int 攻击者坐标X = ByteDataAnalysis.GetInt(gd.Two, ref index);
                    int 攻击者坐标Y = ByteDataAnalysis.GetInt(gd.Two, ref index);

                    break;
               
                default:
                    str = "没有添加缺省动作的case";
                    break;
            }
            if (ClientEvent != null)
            {
                ClientEvent(msg, gd);
            }
            if (str.Length == 0)
            {
                str += "";
            }

            //if (gd.cmd != 12)
            //{

            //    Dictionary<int, string> rslist = ActionForServer.Try提取封包中文字部分(gd);
            //    if (rslist != null)
            //    {
            //        foreach (var item in rslist)
            //        {
            //            str += item.Value.ToString() + "|";
            //        }
            //    }
            //}

            return msg.ToString() + "--" + str;

        }
        public string ServerActionDefault(ServerCmdEnum msg, OriginalData gd)
        {
            string str = string.Empty;
            object obj = new object();
            ByteBuff bb = new ByteBuff(gd.Two);
            int index = 0;
            switch (msg)
            {
                
                default:
                    str = "没有添加缺省动作的case";
                    break;
            }
            if (str.Length == 0)
            {
                str += "";
            }
            if (ServerEvent != null)
            {
                ServerEvent(msg, gd, obj);
            }

            //Dictionary<int, string> rslist = ActionForServer.Try提取封包中文字部分(gd);
            //if (rslist != null)
            //{
            //    foreach (var item in rslist)
            //    {
            //        str += item.Value.ToString() + "|";
            //    }
            //}

            return msg.ToString();

        }


        public string ServerActionDefaultNew(OriginalData gd, bool showDetail)
        {
            string msg = string.Empty;
            string str = string.Empty;
            int index = 0;
            UInt32 MainCmd = gd.cmd;
            int SubCmd = 0;
            if (gd.Two != null && gd.Two.Length > 0)
            {
                SubCmd <<= 0;
                SubCmd |= gd.Two[1];
                SubCmd <<= 8;
                SubCmd |= gd.Two[0];

                ServerMainCmd sm = (ServerMainCmd)MainCmd;
                msg += sm.ToString() + " ";
                switch (sm)
                {
                    case ServerMainCmd.特殊3:
                        break;
                    case ServerMainCmd.发送角色信息4:

                        break;
                    case ServerMainCmd.角色位置定义7:
                        break;
                    
                    default:
                        msg += "主指令不存在";
                        break;
                }
            }
            return msg + str + " ";
        }

        public static Dictionary<ClientCmdEnum, ServerResponse> ClientActionList = new Dictionary<ClientCmdEnum, ServerResponse>();
        public static Dictionary<ServerCmdEnum, ResolveServerActions> ServerActionList = new Dictionary<ServerCmdEnum, ResolveServerActions>();


        public static Dictionary<string, ResolveServerActionsNew> ServerActionListNew = new Dictionary<string, ResolveServerActionsNew>();

        public static Dictionary<ClientCmdEnum, ServerCmdEnum> RequestResponseList = new Dictionary<ClientCmdEnum, ServerCmdEnum>();

        public void AddClientActionList()
        {
            // InstructionService iss = new InstructionService();
            Array enumValues = Enum.GetValues(typeof(ClientCmdEnum));
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            if (ClientActionList.Count == 0)
            {
                while (e.MoveNext())
                {
                    currentValue = (int)e.Current;
                    //currentName = e.Current.ToString();
                    ClientCmdEnum msg = (ClientCmdEnum)currentValue;
                    if (!ClientActionList.ContainsKey(msg))
                    {
                        ClientActionList.Add(msg, ClientActionDefault);
                    }

                }
            }



        }

        public void AddServerActionList()
        {
            Array enumValues = Enum.GetValues(typeof(ServerCmdEnum));
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            if (ServerActionList.Count == 0)
            {
                while (e.MoveNext())
                {
                    currentValue = (int)e.Current;
                    //currentName = e.Current.ToString();
                    ServerCmdEnum msg = (ServerCmdEnum)currentValue;
                    if (!ServerActionList.ContainsKey(msg))
                    {
                        ServerActionList.Add(msg, ServerActionDefault);
                    }

                }
            }



        }

        /*
        public void AddServerActionListNew()
        {
            string Key = string.Empty;
            Array enumValues = Enum.GetValues(typeof(ISMainMsg));
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            if (ServerActionListNew.Count == 0)
            {
                while (e.MoveNext())
                {
                    //currentValue = (int)e.Current;
                    //currentName = e.Current.ToString();
                    ISMainMsg mainCmd = (ISMainMsg)e.Current;
                    Key = mainCmd.ToString();
                    switch (mainCmd)
                    {
                        case ISMainMsg.特殊3:
                            ServerActionListNew.Add(Key, ServerActionDefaultNew);
                            break;
                        case ISMainMsg.发送角色信息4:
                            Array enumValues4 = Enum.GetValues(typeof(ISSubMsg发送角色信息4));
                            IEnumerator e4 = enumValues4.GetEnumerator();
                            e4.Reset();
                            while (e4.MoveNext())
                            {
                                ISSubMsg发送角色信息4 subCmd4 = (ISSubMsg发送角色信息4)e4.Current;
                                Key = mainCmd.ToString() + "|" + subCmd4;
                                if (!ServerActionListNew.ContainsKey(Key))
                                {
                                    ServerActionListNew.Add(Key, ServerActionDefaultNew);
                                }
                            }
                            break;
                        case ISMainMsg.角色位置定义7:
                            break;
                        case ISMainMsg.设置方面9:
                            Array enumValues9 = Enum.GetValues(typeof(ISSubMsg设置9));
                            IEnumerator e9 = enumValues9.GetEnumerator();
                            e9.Reset();
                            while (e9.MoveNext())
                            {
                                ISSubMsg设置9 subCmd17 = (ISSubMsg设置9)e9.Current;
                                Key = mainCmd.ToString() + "|" + subCmd17;
                                if (!ServerActionListNew.ContainsKey(Key))
                                {
                                    ServerActionListNew.Add(Key, ServerActionDefaultNew);
                                }
                            }
                            break;
                        case ISMainMsg.主动10:
                            Array enumValues10 = Enum.GetValues(typeof(ISSubMsg主动10));
                            IEnumerator e10 = enumValues10.GetEnumerator();
                            e10.Reset();
                            while (e10.MoveNext())
                            {
                                ISSubMsg主动10 subCmd10 = (ISSubMsg主动10)e10.Current;
                                Key = mainCmd.ToString() + "|" + subCmd10;
                                if (!ServerActionListNew.ContainsKey(Key))
                                {
                                    ServerActionListNew.Add(Key, ServerActionDefaultNew);
                                }
                            }

                            break;
                        case ISMainMsg.地图11:
                            Array enumValues11 = Enum.GetValues(typeof(ISSubMsg地图11));
                            IEnumerator e11 = enumValues11.GetEnumerator();
                            e11.Reset();
                            while (e11.MoveNext())
                            {

                                ISSubMsg地图11 subCmd11 = (ISSubMsg地图11)e11.Current;
                                Key = mainCmd.ToString() + "|" + subCmd11;
                                if (!ServerActionListNew.ContainsKey(Key))
                                {
                                    ServerActionListNew.Add(Key, ServerActionDefaultNew);
                                }
                            }

                            break;
                        case ISMainMsg.回应客户端心跳包13:
                            break;
                        case ISMainMsg.x14:
                            break;
                        case ISMainMsg.消失16:
                            break;
                        case ISMainMsg.显示方面17:
                            Array enumValues17 = Enum.GetValues(typeof(ISSubMsg显示方面17));
                            IEnumerator e17 = enumValues17.GetEnumerator();
                            e17.Reset();
                            while (e17.MoveNext())
                            {

                                ISSubMsg显示方面17 subCmd17 = (ISSubMsg显示方面17)e17.Current;
                                Key = mainCmd.ToString() + "|" + subCmd17;
                                if (!ServerActionListNew.ContainsKey(Key))
                                {
                                    ServerActionListNew.Add(Key, ServerActionDefaultNew);
                                }
                            }
                            break;
                        default:
                            ServerActionListNew.Add(Key, ServerActionDefaultNew);
                            break;
                    }


                }
            }



        }
        */
        /// <summary>
        /// 调用
        /// </summary>
        public static string PorcessClientMsg(ClientCmdEnum msg, ServerResponse sr, OriginalData gd)
        {

            //Invoke 调用
            string str = sr.Invoke(msg, gd);
            return str;
        }

        /// <summary>
        /// 调用
        /// </summary>
        public static string PorcessServerMsg(ServerCmdEnum msg, ResolveServerActions sr, OriginalData gd)
        {
            //Invoke 调用
            string str = sr.Invoke(msg, gd);
            return str;
        }



        /// <summary>
        /// 调用
        /// </summary>
        public static string PorcessServerMsgNew(ResolveServerActionsNew sr, OriginalData gd)
        {
            //Invoke 调用
            string str = sr.Invoke(gd);
            return str;
        }




    }
}
