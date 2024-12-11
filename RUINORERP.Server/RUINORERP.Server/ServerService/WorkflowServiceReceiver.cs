using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.Workflow.WFApproval;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.DataPortal;
using RUINORERP.Global;
using RUINORERP.Server.Workflow.WFPush;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Workflow.WFReminder;
using System.Collections;

namespace RUINORERP.Server.ServerService
{
    public class WorkflowServiceReceiver
    {

        public async static Task<tb_UserInfo> 接收工作流指令(SessionforBiz UserSession, OriginalData gd)
        {
            tb_UserInfo user = null;
            try
            {
                int index = 0;
                string 登陆时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                var UserName = ByteDataAnalysis.GetString(gd.Two, ref index);
                var pwd = ByteDataAnalysis.GetString(gd.Two, ref index);


                string msg = string.Empty;
                tb_UserInfoController<tb_UserInfo> controller = Startup.GetFromFac<tb_UserInfoController<tb_UserInfo>>();
                user = await controller._unitOfWorkManage.GetDbClient().Queryable<tb_UserInfo>()
                    .Where(u => u.UserName == UserName && u.Password == pwd)
                    .Includes(x => x.tb_employee)
                    .Includes(x => x.tb_User_Roles)
                    .SingleAsync();
                if (user != null)
                {
                    //登陆成功
                    UserSession.User.用户名 = user.UserName;
                    UserSession.User.SessionId = UserSession.SessionID;
                    if (user.tb_employee != null)
                    {
                        UserSession.User.姓名 = user.tb_employee.Employee_Name;
                    }

                    UserService.给客户端发提示消息(UserSession, UserSession.User.姓名 + "用户登陆成功");
                }
                else
                {
                    //通知客户端
                    UserService.给客户端发提示消息(UserSession, "用户登陆出错，用户名或密码错误！");
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("用户登陆:" + ex.Message);
            }
            return user;
        }

        public static bool 接收工作流审批(SessionforBiz UserSession, OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                long billID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                int BizType = ByteDataAnalysis.GetInt(gd.Two, ref index);
                bool ApprovalResults = ByteDataAnalysis.Getbool(gd.Two, ref index);
                string opinion = ByteDataAnalysis.GetString(gd.Two, ref index);
                //ApprovalWFData data = new ApprovalWFData();
                //data.Status = "你";
                //data.BillId = billID;
                //data.Url = "";
                //data.WorkflowName = "";
                //data.DocumentName = 时间;
                //WF.WFController wc = Startup.GetFromFac<WF.WFController>();
                //wc.PublishEvent(Program.WorkflowHost, data, frmMain.Instance.workflowlist);
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("用户登陆:" + ex.Message);
            }
            return rs;
        }

        public static bool 接收审批结果事件推送(SessionforBiz UserSession, OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                long billID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                int BizType = ByteDataAnalysis.GetInt(gd.Two, ref index);
                bool ApprovalResults = ByteDataAnalysis.Getbool(gd.Two, ref index);
                string opinion = ByteDataAnalysis.GetString(gd.Two, ref index);

                //这里工作流ID要传送的，这里暂时用了单号去找
                ApprovalWFData data = new ApprovalWFData();

                data.ApprovedDateTime = System.DateTime.Now;

                ApprovalEntity aEntity = new ApprovalEntity();
                aEntity.ApprovalResults = true;
                aEntity.BillID = billID;
                //  aEntity.ApprovalComments = opinion;
                data.approvalEntity = aEntity;
                WFController wc = Startup.GetFromFac<WFController>();
                wc.PublishEvent(Program.WorkflowHost, data, frmMain.Instance.workflowlist);

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("用户登陆:" + ex.Message);
            }
            return rs;
        }

        public static bool 接收工作流提交(SessionforBiz UserSession, OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                long billID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                int BizType = ByteDataAnalysis.GetInt(gd.Two, ref index);
                bool ApprovalResults = ByteDataAnalysis.Getbool(gd.Two, ref index);
                string opinion = ByteDataAnalysis.GetString(gd.Two, ref index);

                ApprovalWFData approvalWFData = new ApprovalWFData();

                ApprovalEntity data = new ApprovalEntity();
                data.BillID = billID;
                data.bizType = (Global.BizType)BizType;
                data.ApprovalResults = ApprovalResults;
                // data.ApprovalComments = opinion;

                approvalWFData.approvalEntity = data;

                WFController wc = Startup.GetFromFac<WFController>();

                string workflowid = wc.StartApprovalWorkflow(Program.WorkflowHost, approvalWFData, frmMain.Instance.workflowlist);
                //回推
                WorkflowServiceSender.通知工作流启动成功(UserSession, workflowid);
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("用户登陆:" + ex.Message);
            }
            return rs;
        }


        public static bool 接收工作流提醒添加(SessionforBiz UserSession, OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                //当前
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                long billID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                int BizType = ByteDataAnalysis.GetInt(gd.Two, ref index);
                string Receiver = ByteDataAnalysis.GetString(gd.Two, ref index);
                string ReminderContent = ByteDataAnalysis.GetString(gd.Two, ref index);
                string StartTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                ReminderBizData reminderBizData = new ReminderBizData();
                reminderBizData.BizID = billID;
                reminderBizData.BizType = BizType;
                reminderBizData.Receiver = Receiver;
                reminderBizData.ReminderContent = ReminderContent;
                reminderBizData.StartTime = Convert.ToDateTime(StartTime);
                //将来的才提醒。过去了就不管一
                if (reminderBizData.StartTime > System.DateTime.Now)
                {
                    frmMain.Instance.ReminderBizDataList.AddOrUpdate(billID, reminderBizData, (key, value) => value);
                }
                

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收工作流提醒添加:" + ex.Message);
            }
            return rs;
        }
        /// <summary>
        /// 根据客户端传过来的数据来启动哪种类型的工作流，再回推
        /// </summary>
        /// <param name="UserSession"></param>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static bool 启动工作流(SessionforBiz UserSession, OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                int workflowBizType = ByteDataAnalysis.GetInt(gd.Two, ref index);

                WorkflowBizType workflowBiz = (WorkflowBizType)workflowBizType;
                switch (workflowBiz)
                {
                    case WorkflowBizType.基础数据信息推送:
                        string tableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                        PushData data = new PushData();
                        data.InputData = tableName;
                        var workflowId = Program.WorkflowHost.StartWorkflow<PushData>("PushBaseInfoWorkflow", data);
                        Comm.CommService.ShowExceptionMsg("启动了工作流:" + workflowId);




                        break;
                    default:
                        break;
                }







                //回推
                //WorkflowServiceSender.通知工作流启动成功(UserSession, workflowid);
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("用户登陆:" + ex.Message);
            }
            return rs;
        }


    }
}
