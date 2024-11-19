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
    public partial class UCSaleCell : UserControl
    {
        public UCSaleCell()
        {
            InitializeComponent();
        }

        private void UCSaleCell_Load(object sender, EventArgs e)
        {
            QuerySaleOrderStatus();
            timer1.Start();
        }


        int errorCount = 0;
        private  void QuerySaleOrderStatus()
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
                var SaleOutList = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_WorkCenterSale>("Proc_WorkCenterSale"
                    , Employees, sqloutput);//返回List
                kryptonTreeGridView1.ReadOnly = true;

                //========
                string sqlquery = string.Empty;
                string WhereClause = " and 1=1 ";
                if (!strEmployees.IsNullOrEmpty())
                {
                    WhereClause += "and Employee_ID in ( " + strEmployees + " )";
                }
                List<Expression<Func<Proc_WorkCenterSale, object>>> expColumns = new List<Expression<Func<Proc_WorkCenterSale, object>>>();
                expColumns.Add(t => t.订单状态);
                expColumns.Add(t => t.数量);
                kryptonTreeGridView1.DataSource = null;
                kryptonTreeGridView1.GridNodes.Clear();
                if (SaleOutList.Count > 0)
                {
                    DataTable dataTablesale = SaleOutList.ToDataTable<Proc_WorkCenterSale>(expColumns);
                    kryptonTreeGridView1.DataSource = dataTablesale;
                    foreach (KryptonTreeGridNodeRow item in kryptonTreeGridView1.GridNodes)
                    {
                        switch (item.Cells[0].Value)
                        {
                            case "未提交":
                               sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 1 THEN	'未提交' ELSE '' END AS 订单状态,SaleDate,SOrder_ID ,SOrderNo FROM	 tb_SaleOrder WHERE	DataStatus = 1 " + WhereClause);
                                break;
                            case "未审核":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 2 THEN	'未审核' ELSE '' END AS 订单状态,SaleDate,SOrder_ID ,SOrderNo FROM	 tb_SaleOrder WHERE	(DataStatus = 2 OR ApprovalStatus = 0) " + WhereClause);
                             break;
                            case "未出库":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 4 THEN	'未出库' ELSE '' END AS 订单状态,SaleDate,SOrder_ID ,SOrderNo FROM	 tb_SaleOrder WHERE	DataStatus = 4 " + WhereClause);
                                break;
                            default:
                                break;
                        }

                        //部分发货的情况。也是可以查询出来的。TODO

                        if (sqlquery.Contains("SELECT CASE"))
                        {
                            //Random rand = new Random();
                            //Color foreColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                            ////Color backColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                            ////kryptonTreeGridView1.Rows[i].Cells[1].Style.BackColor = foreColor;
                            //item.Cells[1].Style.ForeColor = foreColor;
                            KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridView1.GridNodes[0];
                            DataTable dataTable = MainForm.Instance.AppContext.Db.Ado.GetDataTable(sqlquery);
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                KryptonTreeGridNodeRow subrow = item.Nodes.Add(dataTable.Rows[i][1].ObjToDate().ToString("yy-MM-dd"));
                                subrow.Tag = dataTable.Rows[i][2];
                                subrow.Cells[1].Value = dataTable.Rows[i][3];
                            }
                        }
                    }
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
                    if (DataOverviewItems.Contains(数据概览.销售业绩.ToString()))
                    {
                        lblMonthlyPerformance.Text = string.Empty;
                        sqlquery = string.Format("SELECT sum(c.TransactionPrice*(c.Quantity-c.TotalReturnedQty)) as 订单金额  from  tb_SaleOrder m RIGHT JOIN tb_SaleOrderDetail c on m.SOrder_ID=c.SOrder_ID WHERE m.DataStatus != 1 and  MONTH(m.SaleDate) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        decimal orderAmount = MainForm.Instance.AppContext.Db.Ado.GetDecimal(sqlquery);
                        lblMonthlyPerformance.Text = "本月订单业绩:" + orderAmount.ToString("##,###0元");
                    }

                    if (DataOverviewItems.Contains(数据概览.客户数量.ToString()))
                    {
                        lblMonthlyCustomer.Text = string.Empty;
                        sqlquery = string.Format("SELECT COUNT(CustomerVendor_ID) as 新增客户数 from tb_CustomerVendor  WHERE IsCustomer=1 and  MONTH(Created_at) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        decimal NewCustomerQty = MainForm.Instance.AppContext.Db.Ado.GetDecimal(sqlquery);
                        lblMonthlyCustomer.Text = "本月新增客户:" + NewCustomerQty.ToString("##,###0个");
                    }
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
            if (lastInputTime > 10000 && kryptonTreeGridView1.Rows.Count > 0)
            {
               // MainForm.Instance.ShowMsg($"您有需要处理的销售订单。");
               // MainForm.Instance.PrintInfoLog("您有需要处理的销售订单。");
            }
            //if (MainForm.GetLastInputTime() > 10000 && kryptonTreeGridView1.Rows.Count > 0)
            //{
            //    QuerySaleOrderStatus();
            //}
        }

        private async void kryptonTreeGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //导航到指向的单据界面
            //找到要打开的菜单  订单查询
            if (kryptonTreeGridView1.CurrentCell != null)
            {
                if (kryptonTreeGridView1.CurrentCell.OwningRow.Tag is long pid)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_SaleOrder).Name && m.ClassPath == ("RUINORERP.UI.PSI.SAL.UCSaleOrder")).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        tb_SaleOrderController<tb_SaleOrder> controller = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                        tb_SaleOrder saleOrder = await controller.BaseQueryByIdNavAsync(pid);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, saleOrder);
                    }

                }
            }
        }

        private void kryptonCommand1_Execute(object sender, EventArgs e)
        {
            QuerySaleOrderStatus();
        }
    }
}
