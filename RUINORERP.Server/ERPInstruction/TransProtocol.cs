using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TransInstruction
{

    /// <summary>
    /// 各种指令的检测
    public class TransProtocol
    {
        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void CallbackHandler(OriginalData gd);

        [Browsable(true), Description("引发回调事件，补充作用而已")]
        public event CallbackHandler CallbackEvent;


        public static PackageSourceType CheckType(UInt32 cmd, out string msg)
        {
            msg = string.Empty;
            PackageSourceType pst = new PackageSourceType();
            //尝试转枚举
            ServerCmdEnum sm = (ServerCmdEnum)cmd;
            int tempMj = 0;
            if (int.TryParse(sm.ToString(), out tempMj))
            {
                if (tempMj > 0)
                {
                    ClientCmdEnum cm = (ClientCmdEnum)cmd;
                    pst = PackageSourceType.Client;
                    msg += "Client:" + cm.ToString() + "|" + cm.ToString("X") + " ";
                }
                else
                {
                    pst = PackageSourceType.Server;
                    msg += "Server:" + sm.ToString() + "|" + sm.ToString("X") + " ";
                }

            }
            else
            {
                pst = PackageSourceType.Server;
                msg += "Server:" + sm.ToString() + "|" + sm.ToString("X") + " ";
            }

            return pst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public string CheckClientCmdBy(OriginalData gd)
        {
            string rs = string.Empty;
            #region subcmd

            int MainCmd = gd.cmd;

            UInt32 SubCmd = 0;
            if (gd.Two != null && gd.Two.Length > 0)
            {
                SubCmd <<= 8;
                SubCmd |= gd.Two[1];
                SubCmd <<= 8;
                SubCmd |= gd.Two[0];
                ClientCmdEnum msg = (ClientCmdEnum)SubCmd;

            }
            else
            {
                ClientCmdEnum msg = (ClientCmdEnum)SubCmd;
            }

            #endregion
            return rs;
        }







        public string CheckClientCmd(OriginalData gd)
        {
            return CheckClientCmd(gd, false);
        }


        public string CheckClientCmd(OriginalData gd, bool showDetail)
        {
            //CheckClientCmdBy(gd);

            string rs = string.Empty;
            try
            {
                TransService.Init();
                rs += string.Format("cmd:{0}", gd.cmd.ToString()) + " | ";

                switch (gd.cmd)
                {
                    case 1:
                        rs += "固定特殊指令1";
                        break;
                    case 9:
                        break;
                    case 12:
                        //心跳 手动跳起
                        rs += "客户端发出心跳";
                        break;
                    default:
                        rs += "没有处理的default";
                        break;
                }


                UInt32 cmd = gd.cmd;
                if (gd.Two != null)
                {
                    cmd <<= 8;
                    cmd |= gd.Two[1];
                    cmd <<= 8;
                    cmd |= gd.Two[0];
                    ClientCmdEnum msg = (ClientCmdEnum)cmd;
                    if (showDetail)
                    {
                        rs += cmd.ToString() + " | " + cmd.ToString("X") + "  ";
                    }
                    rs += TransService.PorcessClientMsg(msg, TransService.ClientActionList[msg], gd);
                }
                else
                {
                    ClientCmdEnum msg = (ClientCmdEnum)cmd;
                    if (showDetail)
                    {
                        rs += cmd.ToString() + " | " + cmd.ToString("X") + "  ";
                    }
                    rs += TransService.PorcessClientMsg(msg, TransService.ClientActionList[msg], gd);
                }

            }
            catch (Exception ex)
            {
                rs += ex.Message;
            }



            if (showDetail)
            {
                Dictionary<int, string> rslist = ActionForServer.Try提取封包中文字部分(gd);
                if (rslist != null)
                {
                    foreach (var item in rslist)
                    {
                        rs += item.Value.ToString() + "|";
                    }
                }
            }


            return rs;
        }



        public ClientCmdEnum GetClientCmd(OriginalData gd)
        {
            ClientCmdEnum msg = new ClientCmdEnum();
            try
            {
                //InstructionService.AddClientActionList();
                UInt32 cmd = gd.cmd;
                if (gd.Two != null)
                {
                    cmd <<= 8;
                    cmd |= gd.Two[1];
                    cmd <<= 8;
                    cmd |= gd.Two[0];
                    msg = (ClientCmdEnum)cmd;
                    //rs += cmd.ToString() + " | " + cmd.ToString("X") + "  ";
                    //rs += InstructionService.PorcessClientMsg(msg, InstructionService.ClientActionList[msg], gd);
                }
                else
                {
                    msg = (ClientCmdEnum)cmd;
                    //rs += cmd.ToString() + " | " + cmd.ToString("X") + "  ";
                    //rs += InstructionService.PorcessClientMsg(msg, InstructionService.ClientActionList[msg], gd);
                }

            }
            catch (Exception ex)
            {

            }
            return msg;
        }


        public ServerCmdEnum GetServerCmd(OriginalData gd)
        {
            ServerCmdEnum msg = new ServerCmdEnum();
            try
            {
                //InstructionService.AddServerActionList();
                UInt32 cmd = gd.cmd;
                if (gd.Two != null && gd.Two.Length > 0)
                {
                    cmd <<= 8;
                    cmd |= gd.Two[1];
                    cmd <<= 8;
                    cmd |= gd.Two[0];
                    msg = (ServerCmdEnum)cmd;
                }

            }
            catch (Exception ex)
            {
                msg = (ServerCmdEnum)0;
            }
            return msg;
        }

     
        public string CheckServerCmd(OriginalData gd)
        {
            return CheckServerCmd(gd, false);
        }

        public string CheckServerCmd(OriginalData gd, bool showDetail)
        {
            string rs = string.Empty;
            try
            {

                UInt32 MainCmd = gd.cmd;
                ServerMainCmd sm = (ServerMainCmd)MainCmd;
                rs += sm.ToString() + "|";
                int SubCmd = 0;
                if (gd.Two != null && gd.Two.Length > 0)
                {
                    SubCmd <<= 0;
                    SubCmd |= gd.Two[1];
                    SubCmd <<= 8;
                    SubCmd |= gd.Two[0];

                    switch (sm)
                    {
                        case ServerMainCmd.特殊3:
                            break;
                        case ServerMainCmd.发送角色信息4:
                            break;
                        case ServerMainCmd.角色位置定义7:
                            break;
                        
                        case ServerMainCmd.主动10:
                            ISSubMsg主动10 msg10 = (ISSubMsg主动10)SubCmd;
                            rs += msg10.ToString() + " | " + ((int)msg10).ToString("X") + "  ";
                            break;
                        default:
                            ISSubMsg msg = (ISSubMsg)SubCmd;
                            rs += msg.ToString() + " | " + ((int)msg).ToString("X") + "  ";
                            break;
                    }

                }


                //=============
                //InstructionService.AddServerActionList();


                //rs += string.Format("cmd:{0}", gd.cmd.ToString()) + " | ";
                UInt32 cmd = gd.cmd;
                if (gd.Two != null && gd.Two.Length > 0)
                {
                    cmd <<= 8;
                    cmd |= gd.Two[1];
                    cmd <<= 8;
                    cmd |= gd.Two[0];
                    ServerCmdEnum msg = (ServerCmdEnum)cmd;
                    rs += cmd.ToString() + " | " + ((int)cmd).ToString("X") + "  ";
                    try
                    {
                        //                        rs += InstructionService.PorcessServerMsg(msg, InstructionService.ServerActionList[msg], gd);
                    }
                    catch (Exception ex)
                    {
                        rs += msg.ToString();
                    }

                }
                switch (gd.cmd)
                {
                    case 3:
                        rs += "固定特殊指令1";
                        break;
                    case 7:
                        rs += "角色位置定义？";
                        break;
                    case 17:
                        rs += "";
                        break;
                    case 16:
                        int id = 0;
                        id = BitConverter.ToInt32(gd.One, 0);
                        rs += id + "消失在视野中";
                        break;
                    case 10:
                        //
                        if (cmd == 655802)
                        {
                            rs += "背包时间？";
                        }
                        //rs += "角色有关？";
                        break;
                    case 11:
                        rs += "";//"0xBD怪物移动";
                        break;
                    case 12:
                        //心跳
                        rs += "心跳";
                        break;
                    case 13:
                        //心跳
                        ServerCmdEnum msg = (ServerCmdEnum)cmd;
                        rs += cmd.ToString() + " | " + ((int)cmd).ToString("X") + "  ";
                        rs += TransService.PorcessServerMsg(msg, TransService.ServerActionList[msg], gd);
                        rs += "服务器回应心跳";
                        break;
                    default:
                        //心跳
                        rs += "未知";
                        break;
                }
            }
            catch (Exception ex)
            {
                rs += ex.Message;
            }

            if (showDetail)
            {
                Dictionary<int, string> rslist = ActionForServer.Try提取封包中文字部分(gd);
                if (rslist != null)
                {
                    foreach (var item in rslist)
                    {
                        rs += item.Value.ToString() + "|";
                    }
                }
            }

            return rs;
        }


        public string CheckServerCmdNew(OriginalData gd, bool showDetail)
        {
            string Key = string.Empty;
            //分析他的cmd得到 子枚举。再传入子枚举得到指令意义
            string rs = string.Empty;
            try
            {
                //InstructionService.Init();
                UInt32 MainCmd = gd.cmd;
                ServerMainCmd sm = (ServerMainCmd)MainCmd;
                Key = sm.ToString() + "|";
                int SubCmd = 0;
                if (gd.Two != null && gd.Two.Length > 0)
                {
                    SubCmd <<= 0;
                    SubCmd |= gd.Two[1];
                    SubCmd <<= 8;
                    SubCmd |= gd.Two[0];
                }
                //=============
                //rs += string.Format("cmd:{0}", gd.cmd.ToString()) + " | ";
                UInt32 cmd = gd.cmd;
                if (gd.Two != null && gd.Two.Length > 0)
                {
                    cmd <<= 8;
                    cmd |= gd.Two[1];
                    cmd <<= 8;
                    cmd |= gd.Two[0];
                    ServerCmdEnum msg = (ServerCmdEnum)cmd;
                    try
                    {
                        rs = TransService.Init().ServerActionDefaultNew(gd, showDetail);
                        //rs = InstructionService.PorcessServerMsgNew(InstructionService.ServerActionListNew[Key], gd);
                    }
                    catch (Exception ex)
                    {
                        rs += "请进一步分析" + msg.ToString();
                    }
                    if (showDetail)
                    {
                        //rs += cmd.ToString() + " | " + cmd.ToString("X") + "  ";
                    }
                }
                switch (gd.cmd)
                {
                    case 3:
                        rs += "固定特殊指令1";
                        break;
                    case 7:
                        rs += "角色位置定义？";
                        break;
                    case 17:
                        rs += "";
                        break;
                    case 16:
                        int id = 0;
                        id = BitConverter.ToInt32(gd.One, 0);
                        rs += id + "消失在视野中";
                        break;
                    case 10:
                        //
                        if (cmd == 655802)
                        {
                            rs += "背包时间？";
                        }
                        //rs += "角色有关？";
                        break;
                    case 11:
                        rs += "";//"0xBD怪物移动";
                        break;
                    case 12:
                        //心跳
                        rs += "心跳";
                        break;
                    case 13:
                        //心跳
                        ServerCmdEnum msg = (ServerCmdEnum)cmd;
                        rs += cmd.ToString() + " | " + ((int)cmd).ToString("X") + "  ";
                        rs += TransService.PorcessServerMsg(msg, TransService.ServerActionList[msg], gd);
                        rs += "服务器回应心跳";
                        break;
                    case 14:
                        if (showDetail)
                        {
                            //心跳
                            ServerCmdEnum msg14 = (ServerCmdEnum)cmd;
                            rs += cmd.ToString() + " | " + ((int)cmd).ToString("X") + "  ";
                            //rs += InstructionService.PorcessServerMsg(msg14, InstructionService.ServerActionList[msg14], gd);
                            int index = 0;
                            int GameID = ByteDataAnalysis.GetInt(gd.One, ref index);
                            int X = ByteDataAnalysis.GetInt(gd.One, ref index);
                            int Y = ByteDataAnalysis.GetInt(gd.One, ref index);
                            int Mapx = X / 100 + 26009;
                            int Mapy = Y / 100 + 26009;
                            byte 装备位置 = ByteDataAnalysis.GetByte(gd.Two, ref index);
                            rs += "显示周边" + string.Format("id:{0},x:{1},y:{2}", GameID, Mapx, Mapy);
                        }
                        break;
                    default:
                        //心跳
                        rs += "未知";
                        break;
                }

                if (showDetail)
                {
                    switch (sm)
                    {
                        case ServerMainCmd.特殊3:
                            //Key += msg10.ToString();
                            break;
                        case ServerMainCmd.发送角色信息4:
                            break;
                        
                        default:
                            ISSubMsg msg = (ISSubMsg)SubCmd;
                            rs += msg.ToString() + " | " + ((int)msg).ToString("X") + "  ";
                            break;
                    }

                    Dictionary<int, string> rslist = ActionForServer.Try提取封包中文字部分(gd);
                    if (rslist != null)
                    {
                        foreach (var item in rslist)
                        {
                            rs += item.Value.ToString() + "|";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rs += rs + Key;
                rs += ex.Message;
            }

            return rs;
        }



        

    }
}

