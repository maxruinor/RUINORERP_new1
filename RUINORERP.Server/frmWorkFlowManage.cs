using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.Comm;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.Workflow.Steps;
using RUINORERP.Server.Workflow.WFApproval;
using RUINORERP.Server.Workflow.WFPush;
using RUINORERP.Server.Workflow.WFReminder;
using SharpYaml.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkflowCore.Interface;

namespace RUINORERP.Server
{
    public partial class frmWorkFlowManage : frmBase
    {
        IWorkflowHost host;
        private static frmWorkFlowManage _main;
        internal static frmWorkFlowManage Instance
        {
            get { return _main; }
        }
        public frmWorkFlowManage(IWorkflowHost workflowHost)
        {
            InitializeComponent();
            _main = this;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            host = workflowHost;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("8888===low" + System.DateTime.Now);

            WFController wc = Startup.GetFromFac<WFController>();
            // wc.StartApproval(Program.WorkflowHost, billID, frmMain.Instance.workflowlist);
            wc.StartApproval(Program.WorkflowHost, textBox1.Text, frmMain.Instance.workflowlist);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("事件推送===low" + System.DateTime.Now);
            //ApprovalWFData data = new ApprovalWFData();
            //data.Status = "你1";
            //data.BillId = 55555;// textBox2.Text;
            //data.Url = "";
            //data.WorkflowName = "";
            //data.DocumentName = textBox2.Text;
            //WF.WFController wc = Startup.GetFromFac<WF.WFController>();
            // wc.PublishEvent(Program.WorkflowHost, data, frmMain.Instance.workflowlist);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartWF1();
            //StarWF();
        }


        private void StartWF1()
        {
            var ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            Task.Run(() =>
            {
                ApprovalWFData approvalWFData = new ApprovalWFData();

                ApprovalEntity data = new ApprovalEntity();
                data.BillID = 888;
                data.bizType = Global.BizType.制令单;
                data.ApprovalResults = false;
                //  data.ApprovalComments = "人不在";
                approvalWFData.approvalEntity = data;

                WFController wc = Startup.GetFromFac<WFController>();
                string workflowId = wc.StartApprovalWorkflow(Program.WorkflowHost, approvalWFData, frmMain.Instance.workflowlist);
                txtworkflowid.Text = workflowId;
            });
        }


        /// <summary>
        /// 如果直接UI线程下启动会卡死。内存时不会。持久化就会。
        /// </summary>
        void StarWF()
        {
            var ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            Task.Run(() =>
            {
                var ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
                Console.WriteLine("这是Run2线程：" + ThreadId);
                Console.WriteLine("Do Work2");
                Console.WriteLine("Starting workflow..1111.");
                int counter = 4;
                if (txteventPara.Text.Trim().Length > 0)
                {
                    int.TryParse(txteventPara.Text, out counter);
                }
                string workflowId = host.StartWorkflow("if-sample", new MyData { Counter = counter }).Result;
                txtworkflowid.Text = workflowId;

            });
        }

        private void btn外部事件_Click(object sender, EventArgs e)
        {
            string folderName = txteventPara.Text;

            //这里工作流ID要传送的，这里暂时用了单号去找
            ApprovalWFData data = new ApprovalWFData();

            data.ApprovedDateTime = System.DateTime.Now;

            ApprovalEntity aEntity = new ApprovalEntity();
            aEntity.ApprovalResults = true;
            aEntity.BillID = 555;
            //aEntity.ApprovalComments = "测试审核事件";
            data.approvalEntity = aEntity;
            WFController wc = Startup.GetFromFac<WFController>();
            wc.PublishEvent(Program.WorkflowHost, data, frmMain.Instance.workflowlist);

            //host.PublishEvent(txteventName.Text, txtworkflowid.Text, folderName);
        }

        private void btnPushTest_Click(object sender, EventArgs e)
        {
            PushData data = new PushData();
            data.InputData = "tb_UserInfo";
            //var workflowId = Program.WorkflowHost.StartWorkflow("PushBaseInfoWorkflow", data);
            //MessageBox.Show("start push：" + workflowId);

            host.StartWorkflow("HelloWorld123", 1, null);
        }

        private void btn缓存测试_Click(object sender, EventArgs e)
        {
            IMemoryCache cache = Startup.GetFromFac<IMemoryCache>();

            object obj = MyCacheManager.Instance.GetValue("tb_CustomerVendor", 1740971599693221888);
            if (obj != null)
            {

            }
            //CacheHelper cacheHelper = Startup.GetFromFac<CacheHelper>();
            //var obj = CacheHelper.Instance.GetEntity<tb_CustomerVendor>(1740971599693221888);

            //string json = JsonConvert.SerializeObject(obj,
            //   new JsonSerializerSettings
            //   {
            //       ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
            //   });

        }

        private async void btnStartReminderWF_Click(object sender, EventArgs e)
        {
            //启动提醒工作流
            ReminderData data = new ReminderData();

            //data.BizKey = "华哥";
            //var workflowId = Program.WorkflowHost.StartWorkflow("PushBaseInfoWorkflow", data);
            //MessageBox.Show("start push：" + workflowId);

            //三个参数是 ID名，版本号，数据对象
            var workflowId = await host.StartWorkflow("ReminderWorkflow", 1, data);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 执行工作流
             host.StartWorkflow<SafetyStockData>("SafetyStockWorkflow", new SafetyStockData());
        }
    }
}
