using Newtonsoft.Json;
using RUINORERP.Model;
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

        public static OriginalData 单据锁定释放(long billid, long lockUserID, string lockName, int BizType)
        {
            var tx = new ByteBuff(2 + 4);
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushInt64(lockUserID);
            tx.PushString(lockName);
            tx.PushInt64(billid);
            tx.PushInt((int)BizType);//加一个其他东西？比方随便时间，或当前时间的到分钟
        
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.单据锁定释放;
            gd.One = new byte[] { (byte)ClientSubCmdEnum.反审 };
            gd.Two = tx.toByte();

            return gd;
        }
        public static OriginalData 单据锁定(long billid, long lockUserID, 
            string lockName, int BizType, long MenuID)
        {
            var tx = new ByteBuff(2 + 4);
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushInt64(lockUserID);
            tx.PushString(lockName);
            tx.PushInt64(billid);
            tx.PushInt((int)BizType);//加一个其他东西？比方随便时间，或当前时间的到分钟
            tx.PushInt64(MenuID);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.单据锁定;
            gd.One = new byte[] { (byte)ClientSubCmdEnum.审批 };
            gd.Two = tx.toByte();
            return gd;
        }

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

        /// <summary>
        /// 表名为空则是所有表
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static OriginalData 请求发送缓存(string TableName)
        {
            var tx = new ByteBuff(2 + 4);
            //tx.PushInt16((Int16)ClientCmdEnum.用户登陆);
            //tx.PushInt((int)6);//加一个其他东西？比方随便时间，或当前时间的到分钟
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushString(TableName);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.请求缓存;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }

 

        /// <summary>
        /// 表名为空则是所有表
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static OriginalData 更新缓存<T>(object entity)
        {
            var tx = new ByteBuff(2 + 4);
            //发送缓存数据
            string json = JsonConvert.SerializeObject(entity,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
               });
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushString(typeof(T).Name);
            tx.PushString(json);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.更新缓存;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }

        /// <summary>
        /// 管理员编辑了一些模板或系统级的配置参数时要上传到服务器再分发到其它客户端。
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static OriginalData 更新动态配置<T>(object entity)
        {
            var tx = new ByteBuff(2 + 4);
            //发送缓存数据
            string json = JsonConvert.SerializeObject(entity,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
               });
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushString(typeof(T).Name);
            tx.PushString(json);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.更新动态配置;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }

        /// <summary>
        /// 表名为空则是所有表
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static OriginalData 删除缓存<T>(string PKColName, long PKvalue)
        {
            var tx = new ByteBuff(2 + 4);

            tx.PushString(System.DateTime.Now.ToString());
            tx.PushString(typeof(T).Name);
            tx.PushString(PKColName);
            tx.PushInt64(PKvalue);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.删除缓存;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }

        /// <summary>
        /// 客户机向服务器发送请求协助处理,协助的内容。相关的单据数据
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static OriginalData 请求协助处理(long RequestUserID, string RequestEmpName, string RequestContent, string BillData, string EntityType)
        {
            var tx = new ByteBuff(2 + 4);

            tx.PushString(System.DateTime.Now.ToString());
            tx.PushInt64(RequestUserID);//请示的人姓名。后面单据数据要保存时要名称开头
            tx.PushString(RequestEmpName);//请示的人姓名
            tx.PushString(RequestContent);
            tx.PushString(EntityType);
            tx.PushString(BillData);

            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.请求协助处理;
            gd.One = null;
            gd.Two = tx.toByte();
            return gd;
        }


        /// <summary>
        /// 发送弹窗消息 用于客户端指定接收者的弹窗消息，或服务器根据规则发送的弹窗消息
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="sessonid"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static OriginalData SendPopMessage(string Message, string sessonid, string receiver)
        {
            var tx = new ByteBuff(2 + 4);
            //tx.PushInt16((Int16)ClientCmdEnum.用户登陆);
            //tx.PushInt((int)6);//加一个其他东西？比方随便时间，或当前时间的到分钟
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushString(sessonid);//接收者id
            tx.PushString(receiver);//receiver接收者
            tx.PushString(Message);
            OriginalData gd = new OriginalData();
            gd.cmd = (byte)ClientCmdEnum.发送弹窗消息;
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
