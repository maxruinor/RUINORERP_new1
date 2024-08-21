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
using HLH.Lib.List;
using System.Linq.Expressions;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.UserCenter.DataParts
{
    public partial class UCPURCell : UserControl
    {
        public UCPURCell()
        {
            InitializeComponent();
        }

        private void UCPURCell_Load(object sender, EventArgs e)
        {
            QueryPUROrderStatus();
            timer1.Start();
        }


        private void QueryPUROrderStatus()
        {
            try
            {
                string strProjectGroups = string.Empty;
                List<object> ProjectGroupList = new List<object>();

                string strEmployees = string.Empty;
                List<object> EmployeeGroupList = new List<object>();
                //如果限制
                if (AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext))
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
                var PURList = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_WorkCenterPUR>("Proc_WorkCenterPUR"
                    , Employees, sqloutput);//返回List
                string WhereClause = " and 1=1 ";
                string sqlquery = string.Empty;
                kryptonTreeGridView1.ReadOnly = true;
                if (PURList.Count > 0)
                {
                    List<Expression<Func<Proc_WorkCenterPUR, object>>> expColumns = new List<Expression<Func<Proc_WorkCenterPUR, object>>>();
                    expColumns.Add(t => t.订单状态);
                    expColumns.Add(t => t.数量);
                    kryptonTreeGridView1.DataSource = null;
                    kryptonTreeGridView1.GridNodes.Clear();
                    kryptonTreeGridView1.DataSource = PURList.ToDataTable<Proc_WorkCenterPUR>(expColumns);

                    if (!strEmployees.IsNullOrEmpty())
                    {
                        WhereClause += "and Employee_ID in ( " + strEmployees + " )";
                    }
                    foreach (var item in kryptonTreeGridView1.GridNodes)
                    {
                        switch (item.Cells[0].Value)
                        {
                           case "未提交":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 1 THEN	'未提交' ELSE '' END AS 订单状态,PurDate,PurOrder_ID ,PurOrderNo FROM	tb_PurOrder WHERE	DataStatus = 1 " + WhereClause);
                                break;
                            case "未审核":
                              sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 2  THEN	'未审核' ELSE '' END AS 订单状态,PurDate,PurOrder_ID ,PurOrderNo FROM	tb_PurOrder WHERE	(DataStatus = 2 OR ApprovalStatus = 0) " + WhereClause);
                                break;
                            case "未入库":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 6 THEN '未入库' \r\n            ELSE '未入库' \r\n        END AS 订单状态, PurDate, PurOrder_ID,PurOrderNo        FROM \r\n(\r\n\r\nSELECT PurOrderNo,tb_PurOrder.PurOrder_ID,6 as DataStatus,tb_PurOrderDetail.Quantity,tb_PurOrderDetail.DeliveredQuantity,PurDate \r\nFROM tb_PurOrder\r\nJOIN tb_PurOrderDetail ON tb_PurOrder.PurOrder_ID = tb_PurOrderDetail.PurOrder_ID\r\nWHERE \r\ntb_PurOrderDetail.DeliveredQuantity < tb_PurOrderDetail.Quantity \r\nand tb_PurOrderDetail.DeliveredQuantity=0\r\nAND tb_PurOrder.DataStatus=4 {WhereClause}\r\n\r\n)  AS TT GROUP BY DataStatus ,PurOrderNo,PurOrder_ID,PurDate \r\n");
                                break;
                            case "部分入库":
                                sqlquery = string.Format($"SELECT CASE WHEN DataStatus = 5 THEN '部分入库' \r\n            ELSE '部分入库' \r\n        END AS 订单状态, PurDate, PurOrder_ID,PurOrderNo        FROM \r\n(\r\n\r\nSELECT PurOrderNo,tb_PurOrder.PurOrder_ID,5 as DataStatus,tb_PurOrderDetail.Quantity,tb_PurOrderDetail.DeliveredQuantity,PurDate \r\nFROM tb_PurOrder\r\nJOIN tb_PurOrderDetail ON tb_PurOrder.PurOrder_ID = tb_PurOrderDetail.PurOrder_ID\r\nWHERE \r\ntb_PurOrderDetail.DeliveredQuantity < tb_PurOrderDetail.Quantity \r\nand tb_PurOrderDetail.DeliveredQuantity>0\r\nAND tb_PurOrder.DataStatus=4 {WhereClause}\r\n\r\n)  AS TT GROUP BY DataStatus ,PurOrderNo,PurOrder_ID,PurDate \r\n");
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
                                //KryptonTreeGridNodeRow subrow = item.Nodes.Add(dataRow[2]);
                                KryptonTreeGridNodeRow subrow = item.Nodes.Add(dataRow[1].ObjToDate().ToString("yy-MM-dd"));
                                subrow.Tag = dataRow[2];
                                subrow.Cells[1].Value = dataRow[3];
                            }
                        }
                    }
                }




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

        private void timer1_Tick(object sender, EventArgs e)
        {
            long lastInputTime = MainForm.GetLastInputTime();
            if (lastInputTime > 10000 && kryptonTreeGridView1.Rows.Count > 0)
            {
                MainForm.Instance.ShowMsg($"您有需要处理的采购订单。");
            }

            //if (MainForm.GetLastInputTime() > 100000 && kryptonTreeGridView1.Rows.Count > 0)
            //{
            //    //刷新工作台数据？
            //    QueryPUROrderStatus();
            //}

        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private int errorCount = 0;

        private async void kryptonTreeGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //导航到指向的单据界面
            //找到要打开的菜单  订单查询
            if (kryptonTreeGridView1.CurrentCell != null)
            {
                if (kryptonTreeGridView1.CurrentCell.OwningRow.Tag is long pid)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_PurOrder).Name && m.ClassPath == ("RUINORERP.UI.PSI.PUR.UCPurOrder")).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        tb_PurOrderController<tb_PurOrder> controller = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                        tb_PurOrder purOrder = await controller.BaseQueryByIdNavAsync(pid);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, purOrder);
                    }

                }
            }


        }

        private void kryptonCommandRefresh_Execute(object sender, EventArgs e)
        {
            QueryPUROrderStatus();
        }
    }
}
