using System;
using System.ComponentModel;
using TransInstruction.DataPortal;
namespace TransInstruction
{

    /// <summary>
    /// 客户端动作
    /// </summary>
    public class ActionForClient
    {


        public static OriginalData 工作流提交(long billid, int BizType)
        {
            var tx = new ByteBuff(2 + 4);
            //tx.PushInt16((Int16)ClientCmdEnum.用户登陆);
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushInt64(billid);
            tx.PushInt((int)BizType);//加一个其他东西？比方随便时间，或当前时间的到分钟
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.工作流启动;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }


        public static OriginalData 工作流审批(long billid, int BizType, bool ApprovalResults, string opinion)
        {
            var tx = new ByteBuff(2 + 4);
            //tx.PushInt16((Int16)ClientCmdEnum.用户登陆);
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushInt64(billid);
            tx.PushInt((int)BizType);//加一个其他东西？比方随便时间，或当前时间的到分钟
            tx.PushBool(ApprovalResults);
            tx.PushString(opinion);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.工作流审批;
            gd.One = new byte[] { (byte)ClientSubCmdEnum.审批 };
            gd.Two = tx.toByte();
            return gd;
        }


        public static OriginalData UserReayLogin()
        {
            var tx = new ByteBuff(2 + 4);
            //tx.PushInt16((Int16)ClientCmdEnum.准备登陆);
            //tx.PushInt((int)6);//加一个其他东西？比方随便时间，或当前时间的到分钟
            tx.PushString(System.DateTime.Now.ToString());
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.准备登陆;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }

        public static OriginalData UserLogin(string userName, string Pwd)
        {
            var tx = new ByteBuff(2 + 4);
            //tx.PushInt16((Int16)ClientCmdEnum.用户登陆);
            //tx.PushInt((int)6);//加一个其他东西？比方随便时间，或当前时间的到分钟
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushString(userName);
            tx.PushString(Pwd);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.用户登陆;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }


        public static OriginalData SendMessage(string Message, string sessonid, string receiver)
        {
            var tx = new ByteBuff(2 + 4);
            //tx.PushInt16((Int16)ClientCmdEnum.用户登陆);
            //tx.PushInt((int)6);//加一个其他东西？比方随便时间，或当前时间的到分钟
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushString(sessonid);//接收者id
            tx.PushString(receiver);//receiver接收者
            tx.PushString(Message);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.发送消息;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }


        public static OriginalData 角色创建()
        {
            ByteBuff tx = new ByteBuff(2);
            tx.PushInt16(0x13);//2位
            OriginalData gd = new OriginalData();
            gd.cmd = 0x09;
            gd.One = null;
            gd.Two = tx.toByte();
            //反解析服务器名称(gd);
            return gd;
        }

        public static OriginalData 点击角色进入游戏()
        {
            ByteBuff tx = new ByteBuff(2);
            tx.PushInt16(0x13);//2位
            OriginalData gd = new OriginalData();
            gd.cmd = 0x09;
            gd.One = null;
            gd.Two = tx.toByte();
            //反解析服务器名称(gd);
            return gd;
        }
        public static OriginalData 按PK键()
        {
            ByteBuff tx = new ByteBuff(2);
            tx.PushInt16(0x13);//2位
            OriginalData gd = new OriginalData();
            gd.cmd = 0x09;
            gd.One = null;
            gd.Two = tx.toByte();
            //反解析服务器名称(gd);
            return gd;
        }
        public static OriginalData 删除角色(string 角色名)
        {
            ByteBuff tx = new ByteBuff(角色名.Length);
            tx.PushInt16(0x8C);//2位
            tx.PushInt(0);//4位
            tx.PushString(角色名);//12位开始 名称.Length + 1

            OriginalData gd = new OriginalData();
            gd.cmd = 0x09;
            gd.One = null;
            gd.Two = tx.toByte();
            //反解析服务器名称(gd);
            return gd;
        }


        public static OriginalData 角色退出(string 角色名)
        {
            ByteBuff tx = new ByteBuff(角色名.Length);
            tx.PushInt16(0x8C);//2位
            tx.PushInt(0);//4位
            tx.PushString(角色名);//12位开始 名称.Length + 1

            OriginalData gd = new OriginalData();
            gd.cmd = 0x09;
            gd.One = null;
            gd.Two = tx.toByte();
            //反解析服务器名称(gd);
            return gd;
        }



        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="Parameters"></param>
        public delegate void OtherHandler(ClientCmdEnum msg, OriginalData gd);

        [Browsable(true), Description("引发外部事件，补充作用而已")]
        public event OtherHandler OtherEvent;

        /*
                  if (OtherEvent != null)
            {
                OtherEvent(gd);
            }
         */


    }
}
