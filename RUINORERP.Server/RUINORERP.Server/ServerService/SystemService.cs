using Newtonsoft.Json;
using RUINORERP.Business.CommService;
using RUINORERP.Server.Comm;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.DataPortal;
using TransInstruction;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RUINORERP.Model;
using System.Numerics;
using System.Windows.Forms;
using Azure.Core;
using RUINORERP.Model.CommonModel;

namespace RUINORERP.Server.ServerService
{
    /// <summary>
    /// 系统服务
    /// </summary>
    public static class SystemService
    {
        public static void 转发异常数据(SessionforBiz SendSession, SessionforBiz SuperAdminSession, OriginalData gd)
        {
            try
            {
                //先解析异常数据
                int index = 0;
                string datatime = ByteDataAnalysis.GetString(gd.Two, ref index);
                string Employee_Name = ByteDataAnalysis.GetString(gd.Two, ref index);
                string MachineName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string IP = ByteDataAnalysis.GetString(gd.Two, ref index);
                string ExMsg = ByteDataAnalysis.GetString(gd.Two, ref index);
                string ExCode = ByteDataAnalysis.GetString(gd.Two, ref index);

                //再转发到超级管理员 ，如果超级管理员不在线。缓存？上线再发送？
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(datatime);
                tx.PushString(SendSession.SessionID);
                tx.PushString(Employee_Name);
                tx.PushString(MachineName);
                tx.PushString(IP);
                tx.PushString(ExMsg);
                tx.PushString(ExCode);
                tx.PushBool(true);//MustDisplay 是否强制显示
                SuperAdminSession.AddSendData((byte)ServerCmdEnum.转发异常, null, tx.toByte());

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }

        }

        public static void process请求协助处理(SessionforBiz SuperAdminSession, TranMessage MessageInfo)
        {
            try
            {
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(MessageInfo.SendTime);
                tx.PushInt64(MessageInfo.SenderID);//请示的人ID
                tx.PushString(MessageInfo.SenderName);
                tx.PushString(MessageInfo.Content);
                tx.PushString(MessageInfo.EntityType);
                tx.PushString(MessageInfo.BillData);
                SystemService.转发协助处理(SuperAdminSession, tx.toByte());

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }
        }

        public static void process请求协助处理(OriginalData gd)
        {
            try
            {
                bool IsProcess = false;
                foreach (var item in frmMain.Instance.sessionListBiz)
                {
                    SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                    //自己的不会上传。 只转给超级管理员。
                    if (sessionforBiz.User.超级用户)
                    {
                        SystemService.转发协助处理(sessionforBiz, gd.Two);
                        IsProcess = true;
                        return;
                        // break;//一个人处理就可以了
                    }
                }

                //不在线时缓存到服务器。后面再发送
                if (!IsProcess)
                {
                    //先解析数据
                    int index = 0;
                    string sendtime = ByteDataAnalysis.GetString(gd.Two, ref index);
                    long RequestUserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                    string RequestEmpName = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string RequestContent = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string EntityType = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string BillData = ByteDataAnalysis.GetString(gd.Two, ref index);

                    ////再转发到超级管理员 ，如果超级管理员不在线。缓存 上线再发送
                    //ByteBuff tx = new ByteBuff(200);
                    //tx.PushString(sendtime);
                    //tx.PushInt64(RequestUserID);//请示的人ID
                    //tx.PushString(RequestEmpName);//请示的人ID
                    //tx.PushString(RequestContent);
                    //tx.PushString(EntityType);//请示的人姓名。后面单据数据要保存时要名称开头
                    //tx.PushString(BillData);

                    TranMessage MessageInfo = new TranMessage();
                    MessageInfo.SendTime = sendtime;
                    MessageInfo.SenderID = RequestUserID;
                    MessageInfo.SenderName = RequestEmpName;
                    MessageInfo.Content = RequestContent;
                    frmMain.Instance.MessageList.Enqueue(MessageInfo);
                }




            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }
        }

        public static void process单据审核锁定(SessionforBiz Sender, OriginalData gd)
        {
            try
            {
                foreach (var item in frmMain.Instance.sessionListBiz)
                {
                    SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                    //自己的不管。 只转给其它人。要其它人要注意这个情况
                    if (sessionforBiz.User.UserID == Sender.User.UserID)
                    {
                        continue;
                    }
                    else
                    {
                        sessionforBiz.AddSendData((byte)ServerCmdEnum.转发单据审核锁定, null, gd.Two);
                    }
                }





            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }
        }

        public static void process单据审核锁定释放(SessionforBiz Sender, OriginalData gd)
        {
            try
            {
                foreach (var item in frmMain.Instance.sessionListBiz)
                {
                    SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                    //自己的不管。 只转给其它人。要其它人要注意这个情况
                    if (sessionforBiz.User.UserID == Sender.User.UserID)
                    {
                        continue;
                    }
                    else
                    {
                        sessionforBiz.AddSendData((byte)ServerCmdEnum.转发单据审核锁定释放, null, gd.Two);
                    }
                }





            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }
        }
        /// <summary>
        /// 转发到能处理的人
        /// </summary>
        /// <param name="SendSession"></param>
        /// <param name="SuperAdminSession"></param>
        /// <param name="gd"></param>
        private static void 转发协助处理(SessionforBiz SuperAdminSession, byte[] buff)
        {
            try
            {
                SuperAdminSession.AddSendData((byte)ServerCmdEnum.转发协助处理, null, buff);
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }

        }

    }
}
