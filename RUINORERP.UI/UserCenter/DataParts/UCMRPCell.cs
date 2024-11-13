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
using RUINORERP.Common.Extensions;
using System.Collections.Concurrent;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 生产计划
    /// </summary>
    public partial class UCMRPCell : UserControl
    {
        public UCMRPCell()
        {
            InitializeComponent();
        }

        private void UCPURCell_Load(object sender, EventArgs e)
        {
            QueryMRPDataStatus();
            timer1.Start();
        }


        private async void QueryMRPDataStatus()
        {
            try
            {
                List<tb_ProductionPlan> PURList = await MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                    .Where(c => (c.DataStatus == 2 || c.DataStatus == 4)).OrderBy(c => c.RequirementDate).ToListAsync();

                kryptonTreeGridView1.ReadOnly = true;
                if (PURList.Count > 0)
                {
                    List<Expression<Func<tb_ProductionPlan, object>>> expColumns = new List<Expression<Func<tb_ProductionPlan, object>>>();
                    expColumns.Add(t => t.PPNo);
                    expColumns.Add(t => t.RequirementDate);
                    expColumns.Add(t => t.TotalQuantity);
                    expColumns.Add(t => t.TotalCompletedQuantity);

                    List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
                    ConcurrentDictionary<string, KeyValuePair<string, bool>> ColDisplay = UIHelper.GetFieldNameColList(typeof(tb_ProductionPlan));
                    foreach (var item in ColDisplay)
                    {
                        var exp = expColumns.FirstOrDefault(c => c.GetMemberInfo().Name == item.Key);
                        if (exp != null)
                        {
                            pairs.Add(new KeyValuePair<string, string>(item.Key, item.Value.Key));
                        }
                    }

                    kryptonTreeGridView1.DataSource = null;
                    kryptonTreeGridView1.GridNodes.Clear();
                    kryptonTreeGridView1.SortFilter= "RequirementDate ASC";
                    kryptonTreeGridView1.DataSource = PURList.ToDataTable(true, pairs.ToArray()); // PURList.ToDataTable<Proc_WorkCenterPUR>(expColumns);


                    foreach (var item in kryptonTreeGridView1.GridNodes)
                    {
                        //如果他是来自于订单。特殊标记一下
                        switch (item.Cells[0].Value)
                        {
                            case "未提交":

                                break;
                            case "未审核":

                                break;
                            case "未入库":

                                break;
                            case "部分入库":

                                break;
                            default:
                                break;
                        }

                        //需求
                        //if (sqlquery.Contains("SELECT CASE"))
                        //{
                        string sqlquery = string.Empty;
                        if (sqlquery.Length == 0)
                        {
                            return;
                        }
                        KryptonTreeGridNodeRow kryptonTreeGridNodeRow = kryptonTreeGridView1.GridNodes[0];
                        DataTable dataTable = MainForm.Instance.AppContext.Db.Ado.GetDataTable(sqlquery);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            //KryptonTreeGridNodeRow subrow = item.Nodes.Add(dataRow[2]);
                            KryptonTreeGridNodeRow subrow = item.Nodes.Add(dataRow[1].ObjToDate().ToString("yy-MM-dd"));
                            subrow.Tag = dataRow[2];
                            subrow.Cells[1].Value = dataRow[3];
                        }
                        //}
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
                // MainForm.Instance.ShowMsg($"您有需要处理的采购订单。");
            }

            //if (MainForm.GetLastInputTime() > 100000 && kryptonTreeGridView1.Rows.Count > 0)
            //{
            //    //刷新工作台数据？
            //    QueryMRPDataStatus();
            //}

        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private int errorCount = 0;

        private async void kryptonTreeGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //导航到指向的单据界面
            //找到要打开的菜单  查询下级数据
            if (kryptonTreeGridView1.CurrentCell != null)
            {
                if (kryptonTreeGridView1.CurrentCell.OwningRow.Tag is long pid)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_ProductionPlan).Name && m.ClassPath == ("RUINORERP.UI.PSI.PUR.UCMRPData")).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        tb_ProductionPlanController<tb_ProductionPlan> controller = Startup.GetFromFac<tb_ProductionPlanController<tb_ProductionPlan>>();
                        tb_ProductionPlan MRPData = await controller.BaseQueryByIdNavAsync(pid);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, MRPData);
                    }
                }
            }


        }

        private void kryptonCommandRefresh_Execute(object sender, EventArgs e)
        {
            QueryMRPDataStatus();
        }
    }
}
