using RUINORERP.Business.Security;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.Common;
using System.Web.WebSockets;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using System.Linq.Expressions;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.UserCenter.DataParts
{
    public partial class UCSalePerformanceCell : UserControl
    {
        public UCSalePerformanceCell()
        {
            InitializeComponent();
        }

        private void UCSaleCell_Load(object sender, EventArgs e)
        {
            QuerySaleOrderStatus();
            timer1.Start();
        }


        int errorCount = 0;
        private async void QuerySaleOrderStatus()
        {
            try
            {
                string strProjectGroups = string.Empty;
                List<object> ProjectGroupList = new List<object>();

                string strEmployees = string.Empty;

                List<object> EmployeeGroupList = new List<object>();
                //如果限制
                if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
                {
                    if (!EmployeeGroupList.Contains(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString()))
                    {
                        EmployeeGroupList.Add(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString());
                    }
                }

                if (EmployeeGroupList.Count > 0)
                {
                    strEmployees = string.Join(",", EmployeeGroupList.ToArray());
                }

                var Employees = new SugarParameter("@Employees ", strEmployees == string.Empty ? null : strEmployees);
                var sqloutput = new SugarParameter("@sqlOutput", null, true);//设置为output
                                                                             //var list = db.Ado.UseStoredProcedure().SqlQuery<Class1>("sp_school", nameP, ageP);//返回List
                //var SaleOutList = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_WorkCenterSale>("Proc_WorkCenterSale"
                //    , Employees, sqloutput);//返回List

                string sqlquery = string.Empty;
                string WhereClause = " and 1=1 ";
                if (!strEmployees.IsNullOrEmpty())
                {
                    WhereClause += "and Employee_ID in ( " + strEmployees + " )";
                }
                #region 显示销售业绩

                tb_RoleInfo CurrentRole = MainForm.Instance.AppContext.CurrentRole;
                tb_UserInfo CurrentUser = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
                //先取人，无人再取角色。
                tb_WorkCenterConfig centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID && c.User_ID == CurrentUser.User_ID);
                if (centerConfig == null)
                {
                    centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID);
                }
                if (centerConfig != null)
                {
                    List<string> DataOverviewItems = centerConfig.DataOverview.Split(',').ToList();
                    if (DataOverviewItems.Contains(数据概览.销售情况概览.ToString()))
                    {
                        lblMonthlyOrderPerformance.Text = string.Empty;
                        sqlquery = string.Format("SELECT sum(c.TransactionPrice*(c.Quantity-c.TotalReturnedQty)) as 订单金额  from  tb_SaleOrder m RIGHT JOIN tb_SaleOrderDetail c on m.SOrder_ID=c.SOrder_ID WHERE (m.DataStatus=4 or m.DataStatus=8) and m.ApprovalStatus=1 and m.ApprovalResults=1 and  YEAR(m.SaleDate) = YEAR('{0}') and  MONTH(m.SaleDate) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        decimal orderAmount =await MainForm.Instance.AppContext.Db.Ado.GetDecimalAsync(sqlquery);
                        lblMonthlyOrderPerformance.Text = "本月订单业绩:" + orderAmount.ToString("##,###0元");

                        lblMonthlySalePerformance.Text = string.Empty;
                        sqlquery = string.Format("SELECT sum(c.TransactionPrice*(c.Quantity-c.TotalReturnedQty)) as 订单金额  from  tb_SaleOut m RIGHT JOIN tb_SaleOutDetail c on m.SaleOut_MainID=c.SaleOut_MainID WHERE (m.DataStatus=4 or m.DataStatus=8) and m.ApprovalStatus=1 and m.ApprovalResults=1 and YEAR(m.OutDate) = YEAR('{0}') and  MONTH(m.OutDate) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        decimal RealAmount = await MainForm.Instance.AppContext.Db.Ado.GetDecimalAsync(sqlquery);
                        lblMonthlySalePerformance.Text = "本月实际业绩:" + RealAmount.ToString("##,###0元");

                        //客户数量
                        lblMonthlyCustomer.Text = string.Empty;
                        sqlquery = string.Format(" SELECT COUNT(CustomerVendor_ID) as 新增客户数 from tb_CustomerVendor  WHERE IsCustomer=1 and  YEAR(Created_at) = YEAR('{0}') and  MONTH(Created_at) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        decimal NewCustomerQty = await MainForm.Instance.AppContext.Db.Ado.GetDecimalAsync(sqlquery);
                        lblMonthlyCustomer.Text = "本月新增客户:" + NewCustomerQty.ToString("##,###0个");

                        //如果CRM启用了
                        if (MainForm.Instance.AppContext.CanUsefunctionModules != null && MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM))
                        {
                            //本月新增线索数
                            lblMonthly商机.Text = string.Empty;
                            sqlquery = string.Format(" SELECT COUNT(LeadID) as 新增线索数 from tb_CRM_Leads  WHERE  YEAR(Created_at) = YEAR('{0}') and  MONTH(Created_at) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                            decimal Leads = await MainForm.Instance.AppContext.Db.Ado.GetDecimalAsync(sqlquery);
                            lblMonthly商机.Text += "本月新增线索:" + Leads.ToString("##,###0个");

                            //本月新增潜客数
                            lblMonthly潜客数.Text = string.Empty;
                            sqlquery = string.Format(" SELECT COUNT(Customer_id) as 新增客户数 from tb_CRM_Customer  WHERE   YEAR(Created_at) = YEAR('{0}') and  MONTH(Created_at) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                            decimal NewCustomers = await MainForm.Instance.AppContext.Db.Ado.GetDecimalAsync(sqlquery);
                            lblMonthly潜客数.Text = "本月新增潜客:" + NewCustomerQty.ToString("##,###0个");


                            //本月跟进记录数
                            lblMonthlyFollowupRecords.Text = string.Empty;
                            sqlquery = string.Format(" SELECT COUNT(RecordID) as 跟进记录数 from tb_CRM_FollowUpRecords  WHERE  YEAR(Created_at) = YEAR('{0}') and  MONTH(Created_at) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                            decimal Records = await MainForm.Instance.AppContext.Db.Ado.GetDecimalAsync(sqlquery);
                            lblMonthlyFollowupRecords.Text = "本月跟进记录数:" + Records.ToString("##,###0个");

                            //本月制定计划数
                            lblMonthlyplans.Text = string.Empty;
                            sqlquery = string.Format(" SELECT COUNT(PlanID) as 制定计划数 from tb_CRM_FollowUpPlans  WHERE   YEAR(Created_at) = YEAR('{0}') and  MONTH(Created_at) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                            decimal plans = await MainForm.Instance.AppContext.Db.Ado.GetDecimalAsync(sqlquery);
                            lblMonthlyplans.Text = "本月制定计划数:" + plans.ToString("##,###0个");
                        }
                        else
                        {
                            lblMonthly商机.Visible = false;
                            lblMonthly潜客数.Visible = false;
                            lblMonthlyFollowupRecords.Visible = false;
                            lblMonthlyplans.Visible = false;
                        }

                    }




                }

                #endregion


                #region 显示CRM数据
                //如果启用了CRM
                //检测CRM如果没有购买则不会显示
                if (MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM))
                {

                }
                #endregion

            }
            catch (Exception ex)
            {
                errorCount++;
                // MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                MainForm.Instance.logger.Error(ex);
                if (errorCount > 10)
                {
                    timer1.Stop();
                }
            }
        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();


        private void timer1_Tick(object sender, EventArgs e)
        {
            long lastInputTime = MainForm.GetLastInputTime();
            //if (lastInputTime > 10000 && kryptonTreeGridView1.Rows.Count > 0)
            //{
            //    MainForm.Instance.ShowMsg($"您有需要处理的销售订单。");
            //    MainForm.Instance.PrintInfoLog("您有需要处理的销售订单。");
            //}
        }

        private void kryptonCommand1_Execute(object sender, EventArgs e)
        {
            QuerySaleOrderStatus();
        }
    }
}
