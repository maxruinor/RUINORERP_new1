using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using WorkflowCore.Interface;
using RUINORERP.UI.WorkFlowTester;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices;
using RUINORERP.Services;
using RUINORERP.Repository.Base;
using RUINORERP.IRepository.Base;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using System.Linq.Expressions;

using SqlSugar;
using RUINORERP.UI.Report;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using WorkflowCore.Services;
using WorkflowCore.Models;
using Microsoft.Extensions.DependencyInjection;
using AutoUpdateTools;
using RUINORERP.WF.BizOperation;
using Microsoft.Extensions.Hosting;
using RUINORERP.Business;

namespace RUINORERP.UI.UControls
{

    [MenuAttrAssemblyInfo("流程测试", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.流程设计)]
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {

            


            var initialData = new MyNameClass();


            //var host = Startup.GetFromFac<IWorkflowHost>();
            //host.OnStepError += Host_OnStepError;
            Program.AppContextData.workflowHost.StartWorkflow("mywork", 1, null);


            var data = new MyNameClass { /* 初始化数据 */ };
            var workflowId = Program.AppContextData.workflowHost.StartWorkflow("mywork2", version: 2, data: data);

            //host.StartWorkflow("mywork2", initialData, null);
            string value = txtUnitName.Text;



            // host.PublishEvent("MyEvent", workflowId, value);




        }

        private static void Host_OnStepError(WorkflowCore.Models.WorkflowInstance workflow, WorkflowCore.Models.WorkflowStep step, Exception exception)
        {

        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            List<string> _queryConditions = new List<string>();
            _queryConditions.Add("SaleDate");

            tb_SaleOrder so = new tb_SaleOrder();
            so.SaleDate = System.DateTime.Now;


            var querySqlQueryable = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                       //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                       .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                       .WhereAdv(false, _queryConditions, so);

        }

        private void btnPrintTest_Click(object sender, EventArgs e)
        {
            RptPrintConfig config = new RptPrintConfig();
            config.ShowDialog();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            // 创建 DataTable 对象
            DataTable table = new DataTable("TestTable");

            // 添加列
            DataColumn column1 = table.Columns.Add("Column1");
            DataColumn column2 = table.Columns.Add("Column2");

            // 添加行数据
            DataRow row1 = table.NewRow();
            row1["Column1"] = "Value1";
            row1["Column2"] = "Value2";
            table.Rows.Add(row1);

            DataRow row2 = table.NewRow();
            row2["Column1"] = "Value3";
            row2["Column2"] = "Value4";
            table.Rows.Add(row2);

            kryptonTreeGridViewTest.DataSource = table;


            //            KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridViewTest.GridNodes.Add("1节点", "2节点");
            KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridViewTest.GridNodes[0];

            //kryptonTreeGridView1.GridNodes[1].SetValues("t相同的值");

            //kryptonTreeGridNodeRow.DefaultCellStyle.NullValue = "122345";

            kryptonTreeGridNodeRow.DataGridView.Rows[0].Cells[1].Value = "哈哈";

            KryptonTreeGridNodeRow kryptonTreeGridNodeRow1 = kryptonTreeGridNodeRow.Nodes.Add("kkkk");

            //kryptonTreeGridNodeRow1.DataGridView.Rows[1].Cells[1].Value = "啦啦啦";

            kryptonTreeGridNodeRow1.Cells[1].Value = "ooo";

            kryptonTreeGridViewTest.GridNodes.Add("111");

            kryptonTreeGridViewTest.GridNodes.Add("111");

            kryptonTreeGridViewTest.GridNodes.Add("111");

            kryptonTreeGridViewTest.ExpandAll();
            //========

            //LoadGrid();

            // 原文链接：https://blog.csdn.net/m0_53104033/article/details/129006538
        }


        private void LoadGrid()
        {
            KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridView2.GridNodes.Add("1节点");

            //kryptonTreeGridView1.GridNodes[1].SetValues("t相同的值");

            //kryptonTreeGridNodeRow.DefaultCellStyle.NullValue = "122345";

            kryptonTreeGridNodeRow.DataGridView.Rows[0].Cells[1].Value = true;

            KryptonTreeGridNodeRow kryptonTreeGridNodeRow1 = kryptonTreeGridNodeRow.Nodes.Add("kkkk");

            //kryptonTreeGridNodeRow1.DataGridView.Rows[1].Cells[1].Value = "啦啦啦";

            kryptonTreeGridNodeRow1.Cells[0].Value = "sub";
            kryptonTreeGridNodeRow1.Cells[1].Value = false;

            kryptonTreeGridView2.GridNodes.Add("111");

            kryptonTreeGridView2.GridNodes.Add("111");

            kryptonTreeGridView2.GridNodes.Add("111");

            kryptonTreeGridView2.ExpandAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {




            //var data = new BizOperationData { /* 初始化数据 */ };
            //data.BizType = BizType.销售订单;
            //data.BizID = 8899;
            //var workflowId = MainForm.Instance.AppContext.workflowHost.StartWorkflow("WFSO", version: 3, data: data);
            //txtUnitName.Text = await workflowId;


        }

        private async void btnWFTest_Click(object sender, EventArgs e)
        {
            var data = new SOProcessData { /* 初始化数据 */ };
            data.EmploryeeID = 111;
            data.OrderID = 8899;
            data.OrderAmount = 2000;
            var workflowId = MainForm.Instance.AppContext.workflowHost.StartWorkflow("WFSOProcess", version: 1, data: data);
            txtWorkflowID.Text = await workflowId;
        }

        private void btnEventTest_Click(object sender, EventArgs e)
        {
            string inputValue = txtEventData.Text;
            var eventData = new ApproveResultEventData();
            eventData.ApprovalStatus = 1;
            eventData.ApprovalOpinions = txtEventData.Text;
            //传入事件数据
            MainForm.Instance.AppContext.workflowHost.PublishEvent("ApproveResultEvent", txtWorkflowID.Text, eventData);
        }
    }
}
