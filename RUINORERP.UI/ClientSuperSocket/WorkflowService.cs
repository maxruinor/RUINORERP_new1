using RUINORERP.Business.CommService;
using RUINORERP.PacketSpec.Core.DataProcessing;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.UI.Common;
using RUINORERP.UI.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


using static RUINORERP.UI.MainForm;
using RUINORERP.Extensions.Middlewares;

namespace RUINORERP.UI.SuperSocketClient
{
    public class WorkflowService
    {

        /// <summary>
        /// 启动工作流反返回ID
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static bool 接收服务器发起成功通知(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string Workflowid = ByteOperations.GetString(gd.Two, ref index);
                string msg = ByteOperations.GetString(gd.Two, ref index);
                //这里要弹出窗口提示 需要做一个队列处理。不要阻塞UI界面
                MainForm.Instance.PrintInfoLog(msg, System.Drawing.Color.Red);
                WorkflowItem item = new WorkflowItem();
                item.WorkflowId = Workflowid;
                MainForm.Instance.WorkflowItemlist.TryAdd(Workflowid, "");
                var para = "";
                ShowHandler handler = new ShowHandler(MainForm.Instance.ShowTips);
                //异步操作接口(注意BeginInvoke方法的不同！)
                IAsyncResult result = handler.BeginInvoke("审核通知", msg, para, new AsyncCallback(MainForm.Instance.ShowCallback), "AsycState:OK");
                Thread.Sleep(100);

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器审核通知:" + ex.Message);
            }
            return rs;

        }

        public static bool 接收服务器审核通知(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string Workflowd = ByteOperations.GetString(gd.Two, ref index);
                string msg = ByteOperations.GetString(gd.Two, ref index);
                //这里要弹出窗口提示 需要做一个队列处理。不要阻塞UI界面
                MainForm.Instance.PrintInfoLog(msg, System.Drawing.Color.Red);
                var para = "";
                ShowHandler handler = new ShowHandler(MainForm.Instance.ShowTips);
                //异步操作接口(注意BeginInvoke方法的不同！)
                IAsyncResult result = handler.BeginInvoke("审核通知", msg, para, new AsyncCallback(MainForm.Instance.ShowCallback), "AsycState:OK");
                Thread.Sleep(100);

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器审核通知:" + ex.Message);
            }
            return rs;

        }

        public static bool 接收服务器审核完成通知(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string Workflowd = ByteOperations.GetString(gd.Two, ref index);
                string msg = ByteOperations.GetString(gd.Two, ref index);
                //这里要弹出窗口提示 需要做一个队列处理。不要阻塞UI界面
                MainForm.Instance.PrintInfoLog(msg, System.Drawing.Color.Red);

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器审核完成通知:" + ex.Message);
            }
            return rs;

        }


        public static bool 接收工作流数据(OriginalData gd)
        {
            bool rs = false;
            if (gd.Two==null)
            {
                return false;
            }
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);

                //string Workflowd = ByteOperations.GetString(gd.Two, ref index);
                string Time = ByteOperations.GetString(gd.Two, ref index);
                //int dataLen = ByteOperations.GetInt(gd.Two, ref index);
                //byte[] data = new byte[dataLen];
                //data=ByteOperations.Getbytes(gd.Two,dataLen,ref index);
                string tableName = ByteOperations.GetString(gd.Two, ref index);

                //这里要弹出窗口提示 需要做一个队列处理。不要阻塞UI界面
                MainForm.Instance.PrintInfoLog(tableName, System.Drawing.Color.Red);


                //这里要弹出窗口提示 需要做一个队列处理。不要阻塞UI界面
                //ShowHandler handler = new ShowHandler(MainForm.Instance.ShowTips);
                //异步操作接口(注意BeginInvoke方法的不同！)
                //  IAsyncResult result = handler.BeginInvoke("服务器数据推送通知", tableName, tableName, new AsyncCallback//(MainForm.Instance.ShowCallback), "AsycState:OK");
                //  Thread.Sleep(100);

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("服务器数据推送通知:" + ex.Message);
            }
            return rs;

        }
    }
}
