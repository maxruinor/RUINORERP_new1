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
    public partial class UCStockCell : UserControl
    {
        public UCStockCell()
        {
            InitializeComponent();
        }

        private void UCStockCell_Load(object sender, EventArgs e)
        {
            QueryStockInfo();
            QueryStockOtherIn();
            QueryStockOtherOut();
            timer1.Start();
        }


        private void QueryStockInfo()
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

                List<Expression<Func<Proc_WorkCenterSale, object>>> expColumns = new List<Expression<Func<Proc_WorkCenterSale, object>>>();
                expColumns.Add(t => t.订单状态);
                expColumns.Add(t => t.数量);
                kryptonTreeGridView1.GridNodes.Clear();
                kryptonTreeGridView1.DataSource = null;
                kryptonTreeGridView1.DataSource = SaleOutList.ToDataTable<Proc_WorkCenterSale>(expColumns);
                //KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridView1.GridNodes[0];
                //kryptonTreeGridView1.GridNodes[1].SetValues("t相同的值");
                //kryptonTreeGridNodeRow.DefaultCellStyle.NullValue = "122345";

                //覆盖性的设置了值。将kryptonTreeGridNodeRow的对应的DataGridView的第1行2列设置值
                //kryptonTreeGridNodeRow.DataGridView.Rows[0].Cells[1].Value = "哈哈";
                string WhereClause = " and 1=1 ";
                if (!strEmployees.IsNullOrEmpty())
                {
                    WhereClause += "and Employee_ID in ( " + strEmployees + " )";
                }
                string sqlquery = string.Empty;

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
                    if (sqlquery.Contains("SELECT CASE"))
                    {
                        KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridView1.GridNodes[0];
                        DataTable dataTable = MainForm.Instance.AppContext.Db.Ado.GetDataTable(sqlquery);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            KryptonTreeGridNodeRow subrow = item.Nodes.Add(dataRow[1].ObjToDate().ToString("MM-dd"));
                            subrow.Tag = dataRow[2];
                            subrow.Cells[1].Value = dataRow[3];
                        }
                    }
                }

                /*

                KryptonTreeGridNodeRow kryptonTreeGridNodeRow1 = kryptonTreeGridNodeRow.Nodes.Add("kkkk");

                //kryptonTreeGridNodeRow1.DataGridView.Rows[1].Cells[1].Value = "啦啦啦";

                kryptonTreeGridNodeRow1.Cells[1].Value = "ooo";

                kryptonTreeGridView1.GridNodes.Add("111");

                kryptonTreeGridView1.GridNodes.Add("111");

                kryptonTreeGridView1.GridNodes.Add("111");


                kryptonTreeGridView1.ExpandAll();*/
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
            }
        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

        private void timer1_Tick(object sender, EventArgs e)
        {
            long lastInputTime = MainForm.GetLastInputTime();
            if (lastInputTime > 10000 && kryptonTreeGridView1.Rows.Count > 0)
            {
                MainForm.Instance.ShowMsg($"您有需要处理的销售订单。");
            }
            //if (MainForm.GetLastInputTime() > 10000 && kryptonTreeGridView1.Rows.Count > 0)
            //{

            //    QueryStockOtherIn();
            //    QueryStockOtherOut();
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

            QueryStockOtherIn();
            QueryStockOtherOut();
        }


        private void QueryStockOtherIn()
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


                var OtherINList = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_WorkCenterOther>("Proc_WorkCenterOtherIn"
                    , Employees, sqloutput);//返回List
                kryptonTreeGridViewOtherIn.ReadOnly = true;
                List<Expression<Func<Proc_WorkCenterOther, object>>> expColumns = new List<Expression<Func<Proc_WorkCenterOther, object>>>();
                expColumns.Add(t => t.单据状态);
                expColumns.Add(t => t.数量);
                kryptonTreeGridViewOtherIn.GridNodes.Clear();
                kryptonTreeGridViewOtherIn.DataSource = null;
                if (OtherINList.Count > 0)
                {
                    kryptonTreeGridViewOtherIn.DataSource = OtherINList.ToDataTable<Proc_WorkCenterOther>(expColumns);
                    string WhereClause = " and 1=1 ";
                    if (!strEmployees.IsNullOrEmpty())
                    {
                        WhereClause += "and Employee_ID in ( " + strEmployees + " )";
                    }
                    string sqlquery = string.Empty;
                    foreach (KryptonTreeGridNodeRow item in kryptonTreeGridViewOtherIn.GridNodes)
                    {
                        switch (item.Cells[0].Value)
                        {
                            //case "未提交":
                            //    sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 1 THEN	'未提交' ELSE '' END AS 单据状态,Enter_Date,MainID ,BillNo FROM	 tb_StockIn WHERE	DataStatus = 1 " + WhereClause);
                            //    break;
                            //case "未审核":
                            //    sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 2 THEN	'未审核' ELSE '' END AS 单据状态,Enter_Date,MainID ,BillNo FROM	 tb_StockIn WHERE	(DataStatus = 2 OR ApprovalStatus = 0) " + WhereClause);
                            //    break;
                            case "未结案":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 4 THEN	'未结案' ELSE '' END AS 单据状态,Enter_Date,MainID ,BillNo FROM	 tb_StockIn WHERE	DataStatus = 4 " + WhereClause);
                                break;
                            default:
                                break;
                        }
                        if (sqlquery.Contains("SELECT CASE"))
                        {
                            KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridViewOtherIn.GridNodes[0];
                            DataTable dataTable = MainForm.Instance.AppContext.Db.Ado.GetDataTable(sqlquery);
                            foreach (DataRow dataRow in dataTable.Rows)
                            {
                                KryptonTreeGridNodeRow subrow = item.Nodes.Add(dataRow[1].ObjToDate().ToString("MM-dd"));
                                subrow.Tag = dataRow[2];
                                subrow.Cells[1].Value = dataRow[3];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                MainForm.Instance.logger.Error(ex);
            }
        }
        private void QueryStockOtherOut()
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


                var OtherOutList = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_WorkCenterOther>("Proc_WorkCenterOtherOut"
                    , Employees, sqloutput);//返回List
                kryptonTreeGridViewOtherOut.ReadOnly = true;
                List<Expression<Func<Proc_WorkCenterOther, object>>> expColumns = new List<Expression<Func<Proc_WorkCenterOther, object>>>();
                expColumns.Add(t => t.单据状态);
                expColumns.Add(t => t.数量);
                kryptonTreeGridViewOtherOut.GridNodes.Clear();
                kryptonTreeGridViewOtherOut.DataSource = null;
                if (OtherOutList.Count > 0)
                {
                    kryptonTreeGridViewOtherOut.DataSource = OtherOutList.ToDataTable<Proc_WorkCenterOther>(expColumns);

                    string WhereClause = " and 1=1 ";
                    if (!strEmployees.IsNullOrEmpty())
                    {
                        WhereClause += "and Employee_ID in ( " + strEmployees + " )";
                    }
                    string sqlquery = string.Empty;

                    foreach (KryptonTreeGridNodeRow item in kryptonTreeGridViewOtherOut.GridNodes)
                    {
                        switch (item.Cells[0].Value)
                        {
                            case "未提交":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 1 THEN	'未提交' ELSE '' END AS 单据状态,Out_Date,MainID ,BillNo FROM	 tb_StockOut WHERE	DataStatus = 1 " + WhereClause);
                                break;
                            case "未审核":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 2 THEN	'未审核' ELSE '' END AS 单据状态,Out_Date,MainID ,BillNo FROM	 tb_StockOut WHERE	(DataStatus = 2 OR ApprovalStatus = 0) " + WhereClause);
                                break;
                            case "未结案":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 4 THEN	'未结案' ELSE '' END AS 单据状态,Out_Date,MainID ,BillNo FROM	 tb_StockOut WHERE	DataStatus = 4 " + WhereClause);
                                break;
                            default:
                                break;
                        }
                        if (sqlquery.Contains("SELECT CASE"))
                        {
                            KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridViewOtherOut.GridNodes[0];
                            DataTable dataTable = MainForm.Instance.AppContext.Db.Ado.GetDataTable(sqlquery);
                            foreach (DataRow dataRow in dataTable.Rows)
                            {
                                KryptonTreeGridNodeRow subrow = item.Nodes.Add(dataRow[1].ObjToDate().ToString("MM-dd"));
                                subrow.Tag = dataRow[2];
                                subrow.Cells[1].Value = dataRow[3];
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
            }
        }

        private async void kryptonTreeGridViewOtherIn_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //导航到指向的单据界面
            //找到要打开的菜单  订单查询
            if (kryptonTreeGridViewOtherIn.CurrentCell != null)
            {
                if (kryptonTreeGridViewOtherIn.CurrentCell.OwningRow.Tag is long pid)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_StockIn).Name && m.ClassPath == ("RUINORERP.UI.PSI.INV.UCStockIn")).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        tb_StockInController<tb_StockIn> controller = Startup.GetFromFac<tb_StockInController<tb_StockIn>>();
                        tb_StockIn entity = await controller.BaseQueryByIdNavAsync(pid);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }

                }
            }
        }

        private async void kryptonTreeGridViewOtherOut_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //导航到指向的单据界面
            //找到要打开的菜单  订单查询
            if (kryptonTreeGridViewOtherOut.CurrentCell != null)
            {
                if (kryptonTreeGridViewOtherOut.CurrentCell.OwningRow.Tag is long pid)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_StockOut).Name && m.ClassPath == ("RUINORERP.UI.PSI.INV.UCStockOut")).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        tb_StockOutController<tb_StockOut> controller = Startup.GetFromFac<tb_StockOutController<tb_StockOut>>();
                        tb_StockOut entity = await controller.BaseQueryByIdNavAsync(pid);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }

                }
            }
        }
    }
}
