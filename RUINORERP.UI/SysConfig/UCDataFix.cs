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
using RUINORERP.Extensions.Middlewares;
using SqlSugar;
using RUINORERP.UI.Report;
using System.Reflection;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using System.Collections.Concurrent;
using Org.BouncyCastle.Math.Field;
using RUINORERP.UI.PSI.PUR;
using Netron.GraphLib;

namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("数据修复", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataFix : UserControl
    {
        public UCDataFix()
        {
            InitializeComponent();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
        Type[] ModelTypes;
        /// <summary>
        /// 为了查找明细表名类型，保存所有类型名称方便查找
        /// </summary>
        List<string> typeNames = new List<string>();

        List<SugarTable> stlist = new List<SugarTable>();

        private void UCDataFix_Load(object sender, EventArgs e)
        {
            //这里先提取要找到实体的类型，执行一次
            Assembly dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            ModelTypes = dalAssemble.GetExportedTypes();

            typeNames = ModelTypes.Select(m => m.Name).ToList();
            foreach (var type in ModelTypes)
            {
                var attrs = type.GetCustomAttributes<SugarTable>();
                foreach (var attr in attrs)
                {
                    if (attr is SugarTable st)
                    {
                        //var t = Startup.ServiceProvider.GetService(type);//SugarColumn 或进一步取字段特性也可以
                        //var t = Startup.ServiceProvider.CreateInstance(type);//SugarColumn 或进一步取字段特性也可以
                        if (st.TableName.Contains("tb_"))
                        {
                            stlist.Add(st);
                        }

                        continue;
                        //下面代码不需要
                        //ConcurrentDictionary<string, string> cd = ReflectionHelper.GetPropertyValue(t, "FieldNameList") as ConcurrentDictionary<string, string>;
                        //if (cd == null)
                        //{
                        //    cd = new ConcurrentDictionary<string, string>();
                        //    SugarColumn entityAttr;
                        //    foreach (PropertyInfo field in type.GetProperties())
                        //    {
                        //        foreach (Attribute attrField in field.GetCustomAttributes(true))
                        //        {
                        //            entityAttr = attrField as SugarColumn;
                        //            if (null != entityAttr)
                        //            {
                        //                if (entityAttr.ColumnDescription == null)
                        //                {
                        //                    continue;
                        //                }
                        //                if (entityAttr.IsIdentity)
                        //                {
                        //                    continue;
                        //                }
                        //                if (entityAttr.IsPrimaryKey)
                        //                {
                        //                    continue;
                        //                }
                        //                if (entityAttr.ColumnDescription.Trim().Length > 0)
                        //                {
                        //                    cd.TryAdd(field.Name, entityAttr.ColumnDescription);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}


                    }
                }
            }
            kryptonDataGridView1.DataSource = stlist;
            kryptonDataGridView1.ContextMenuStrip = contextMenuStripFix;
        }

        private async void kryptonDataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (kryptonDataGridView1.CurrentRow != null)
            {
                string tableName = kryptonDataGridView1.CurrentRow.Cells["TableName"].Value.ToString();

                DataTable queryList = null;
                switch (tableName)
                {
                    case "tb_SaleOrder":

                        //检测订单主单总数量 是否等于明细的和
                        //修复：入库单审核后，没有正常把订单中的已交数量写回到订单中，并且把订单状态为结案

                        List<tb_SaleOrder> tb_SaleOrders = new List<tb_SaleOrder>();
                        tb_SaleOrders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                            .Includes(t => t.tb_SaleOrderDetails)
                            .ToList();
                        int tb_SaleOrdersCounter = 0;
                        foreach (tb_SaleOrder _SaleOrder in tb_SaleOrders)
                        {

                            if (_SaleOrder.tb_SaleOrderDetails == null || _SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity * c.TransactionPrice) == 0 && _SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity) == 0)
                            {
                                MainForm.Instance.PrintInfoLog($"销售订单{_SaleOrder.SOrderNo}，明细数量和总数量不对========>价格为0。请修复！");
                            }

                            if (_SaleOrder.tb_SaleOrderDetails != null && _SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount) + _SaleOrder.ShipCost > _SaleOrder.TotalAmount)
                            {
                                MainForm.Instance.PrintInfoLog($"销售订单{_SaleOrder.SOrderNo}，=======有运费情况。请修复！");
                            }


                            foreach (tb_SaleOrderDetail _SaleOrderDetail in _SaleOrder.tb_SaleOrderDetails)
                            {
                                if (_SaleOrderDetail.SubtotalTransAmount != _SaleOrderDetail.TransactionPrice * _SaleOrderDetail.Quantity)
                                {
                                    MainForm.Instance.PrintInfoLog($"销售订单{_SaleOrder.SOrderNo}，成交小计不等于成交价*数量。请修复！");
                                }
                            }

                            if (_SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity) != _SaleOrder.TotalQty &&
                                _SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity * c.TransactionPrice) != _SaleOrder.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount))
                            {
                                tb_SaleOrdersCounter++;
                                if (MessageBox.Show($"销售订单{_SaleOrder.SOrderNo}，明细数量和总数量不对，是否修复？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    _SaleOrder.TotalQty = _SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity);
                                    _SaleOrder.TotalAmount = _SaleOrder.tb_SaleOrderDetails.Sum(c => c.Quantity * c.TransactionPrice);


                                    //更新已交数量
                                    int purupdatequantity = await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrder>(_SaleOrder).ExecuteCommandAsync();
                                    if (purupdatequantity > 0)
                                    {
                                        MainForm.Instance.PrintInfoLog($"销售订单{_SaleOrder.SOrderNo}，明细数量和总数量不对。已修复！");
                                    }
                                }
                                else
                                {
                                    MainForm.Instance.PrintInfoLog($"销售订单{_SaleOrder.SOrderNo}，明细数量和总数量不对！");
                                }

                            }
                        }
                        MainForm.Instance.PrintInfoLog("检测到问题数据行数:" + tb_SaleOrdersCounter);


                        //修复：入库单审核后，没有正常把订单中的已交数量写回到订单中，并且把订单状态为结案

                        int scounter = 0;
                        foreach (tb_SaleOrder SOrder in tb_SaleOrders)
                        {
                            if (SOrder.PlatformOrderNo == null)
                            {
                                continue;
                            }
                            if (SOrder.PlatformOrderNo.EndsWith(" "))
                            {
                                SOrder.PlatformOrderNo = SOrder.PlatformOrderNo.Trim();
                                await MainForm.Instance.AppContext.Db.Updateable(SOrder).UpdateColumns(t => new { t.PlatformOrderNo }).ExecuteCommandAsync();
                            }

                        }
                        MainForm.Instance.PrintInfoLog("tb_SOrder修复行数:" + scounter);
                        break;

                    case "tb_PurOrder":
                        //检测订单主单总数量 是否等于明细的和



                        //修复：入库单审核后，没有正常把订单中的已交数量写回到订单中，并且把订单状态为结案
                        tb_PurOrderController<tb_PurOrder> ctr = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                        List<tb_PurOrder> tb_PurOrders = new List<tb_PurOrder>();
                        tb_PurOrders = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                            .Includes(t => t.tb_PurOrderDetails)
                            .Includes(t => t.tb_PurEntries, d => d.tb_PurEntryDetails)
                            .ToList();
                        int counter = 0;

                        foreach (tb_PurOrder purOrder in tb_PurOrders)
                        {

                            // 有入库，入库明细和等于入库明细的总数量。 && purOrder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity) == 0
                            if (purOrder.tb_PurEntries.Count > 0 && purOrder.DataStatus == (int)DataStatus.确认
                             && purOrder.tb_PurEntries.Sum(c => c.TotalQty) == purOrder.tb_PurOrderDetails.Sum(c => c.Quantity)
                             && purOrder.tb_PurEntries.Sum(c => c.TotalQty) == purOrder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity)
                                && purOrder.tb_PurEntries.Sum(c => c.TotalQty) == purOrder.tb_PurEntries.Select(c => c.tb_PurEntryDetails.Sum(d => d.Quantity)).Sum())
                            {
                                MainForm.Instance.PrintInfoLog($"订单{purOrder.PurOrderNo}:审核过，有对应的入库数据，订单日期为{purOrder.PurDate}。==》 找一下可以修复的情况。");
                            }

                            //查一下 只有订单。没有入库单的数据。特别显示一下订单时间
                            if (purOrder.tb_PurEntries.Count > 0 && purOrder.DataStatus == (int)DataStatus.确认 && purOrder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity) == 0)
                            {
                                //MainForm.Instance.PrintInfoLog($"订单{purOrder.PurOrderNo}:审核过，有对应的入库数据，订单日期为{purOrder.PurDate}。==》 已交为0");
                            }

                            //查一下 只有订单。没有入库单的数据。特别显示一下订单时间
                            if (purOrder.tb_PurEntries.Count > 0 && purOrder.DataStatus == (int)DataStatus.确认 && purOrder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity) > 0)
                            {
                                //MainForm.Instance.PrintInfoLog($"订单{purOrder.PurOrderNo}:审核过，没有对应的入库数据，订单日期为{purOrder.PurDate}。==>部分入库");
                            }

                            //查一下 只有订单。没有入库单的数据。特别显示一下订单时间
                            if (purOrder.tb_PurEntries.Count == 0 && purOrder.DataStatus == (int)DataStatus.确认
                                && purOrder.PurDate <= DateTime.Now.AddDays(-60)
                                && purOrder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity) == 0)
                            {
                                MainForm.Instance.PrintInfoLog($"订单{purOrder.PurOrderNo}:审核过，没有对应的入库数据，订单日期为{purOrder.PurDate}。可以删除？？？？？？？？？？");
                            }


                            if (purOrder.tb_PurOrderDetails.Sum(c => c.Quantity) != purOrder.TotalQty && purOrder.DataStatus == (int)DataStatus.完结)
                            {
                                //MainForm.Instance.PrintInfoLog($"完结的订单{purOrder.PurOrderNo}:订单的总数量和订单的明细之和的数量不对。");
                            }

                            //如果入库单主单总数量 和明细也不对时，提示一下。
                            if (purOrder.tb_PurEntries.Count > 0 && purOrder.tb_PurEntries.Select(c => c.tb_PurEntryDetails.Sum(s => s.Quantity)).Sum() == purOrder.tb_PurEntries.Sum(c => c.TotalQty))
                            {
                                //do something
                                if (purOrder.tb_PurEntries.Select(c => c.tb_PurEntryDetails.Sum(s => s.Quantity)).Sum() > purOrder.tb_PurEntries.Sum(c => c.TotalQty))
                                {
                                    MainForm.Instance.PrintInfoLog($"订单{purOrder.PurOrderNo}，对应的入库单总数量和入库单明细之和的数量不对。==>明细大于总的");
                                }
                                if (purOrder.tb_PurEntries.Select(c => c.tb_PurEntryDetails.Sum(s => s.Quantity)).Sum() < purOrder.tb_PurEntries.Sum(c => c.TotalQty))
                                {
                                    MainForm.Instance.PrintInfoLog($"订单{purOrder.PurOrderNo}，对应的入库单总数量和入库单明细之和的数量不对。===》明细小于总的。部分入库？？？？？？？？？");
                                }
                            }

                            if (purOrder.tb_PurEntries.Count > 0 && purOrder.tb_PurOrderDetails.Sum(c => c.Quantity) != purOrder.tb_PurEntries.Sum(c => c.TotalQty) && purOrder.DataStatus == (int)DataStatus.完结)
                            {
                                counter++;
                                //MainForm.Instance.PrintInfoLog($"已结案订单{purOrder.PurOrderNo}:总数量和对应入库单的总数量数量不对。");
                            }

                        }
                        MainForm.Instance.PrintInfoLog("检测到问题数据行数:" + counter);

                        /*

                        foreach (tb_PurOrder purOrder in tb_PurOrders)
                        {

                            //已经结案的如果订单明细已交数！=入库明细的和。则不对
                            if (purOrder.DataStatus == 8)
                            {
                                if (purOrder.tb_PurOrderDetails.Sum(c => c.Quantity) != purOrder.tb_PurEntries.Sum(c => c.TotalQty))
                                {
                                    MainForm.Instance.PrintInfoLog($"已结案订单{purOrder.PurOrderNo}:数量和和为数量不对。");
                                }
                            }

                            //如果订单没有完结，并且订单总数量等于这个对应的所有入库数量则将入库数量写回到订单中。 
                            if (purOrder.DataStatus == 4)
                            {
                                tb_PurOrder order = await ctr.BaseQueryByIdNavAsync(purOrder.PurOrder_ID);

                                if (order.TotalQty == order.tb_PurEntries.Sum(c => c.TotalQty))
                                {
                                    order.DataStatus = 8;
                                    foreach (var item in order.tb_PurOrderDetails)
                                    {
                                        item.DeliveredQuantity = item.Quantity;
                                    }
                                    //更新已交数量
                                    await MainForm.Instance.AppContext.Db.Updateable<tb_PurOrderDetail>(order.tb_PurOrderDetails).ExecuteCommandAsync();
                                    order.DataStatus = (int)DataStatus.完结;
                                    await MainForm.Instance.AppContext.Db.Updateable(order).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                                    counter++;
                                }
                                if (order.tb_PurOrderRes.Count > 0)
                                {
                                    //采购退货
                                }
                                //break;
                            }

                        }
                        MainForm.Instance.PrintInfoLog("tb_PurOrder修复行数:" + counter);
                        */
                        break;

                    case "tb_PurEntry":
                        //检测订单主单总数量 是否等于明细的和
                        //修复：入库单审核后，没有正常把订单中的已交数量写回到订单中，并且把订单状态为结案
                        tb_PurEntryController<tb_PurEntry> ctrPurEntry = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();
                        List<tb_PurEntry> tb_PurEntrys = new List<tb_PurEntry>();
                        tb_PurEntrys = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                            .Includes(t => t.tb_PurEntryDetails)
                            .Includes(t => t.tb_PurEntryRes, d => d.tb_PurEntryReDetails)
                            .ToList();
                        int PurEntryCounter = 0;
                        foreach (tb_PurEntry purEntry in tb_PurEntrys)
                        {
                            if (purEntry.tb_PurEntryDetails.Sum(c => c.Quantity) != purEntry.TotalQty)
                            {
                                if (MessageBox.Show($"入库单{purEntry.PurEntryNo}，明细数量和总数量不对，是否修复？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    purEntry.TotalQty = purEntry.tb_PurEntryDetails.Sum(c => c.Quantity);
                                    //更新已交数量
                                    int purupdatequantity = await MainForm.Instance.AppContext.Db.Updateable<tb_PurEntry>(purEntry).ExecuteCommandAsync();
                                    if (purupdatequantity > 0)
                                    {
                                        MainForm.Instance.PrintInfoLog($"入库单{purEntry.PurEntryNo}，明细数量和总数量不对。已修复！");
                                    }
                                }

                            }
                        }
                        MainForm.Instance.PrintInfoLog("检测到问题数据行数:" + PurEntryCounter);
                        break;

                    case "":

                        queryList = MainForm.Instance.AppContext.Db.Queryable(tableName, "tn")
                      //.Where(conModels)
                      .ToDataTable();
                        foreach (DataRow dataRow in queryList.Rows)
                        {
                            if (dataRow["UntaxedAmount"].ToString().ToDecimal() + dataRow["TaxSubtotalAmount"].ToString().ToDecimal() != dataRow["SubtotalTransactionAmount"].ToString().ToDecimal())
                            {
                                string SOrder_ID = dataRow["SOrder_ID"].ToString();
                            }
                        }

                        break;

                    case "tb_SaleOut":

                        //查看出库明细和总数量是否一样。
                        List<tb_SaleOut> tb_SaleOuts = new List<tb_SaleOut>();
                        tb_SaleOuts = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                            .Includes(t => t.tb_SaleOutDetails)
                            .Includes(t => t.tb_saleorder, d => d.tb_SaleOrderDetails)
                            .ToList();
                        int SaleOutCouter = 0;
                        foreach (tb_SaleOut saleOut in tb_SaleOuts)
                        {
                            if (saleOut.tb_SaleOutDetails.Sum(c => c.Quantity) != saleOut.TotalQty)
                            {
                                SaleOutCouter++;
                                if (MessageBox.Show($"出库单{saleOut.SaleOutNo}，明细数量和总数量不对，是否修复？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    saleOut.TotalQty = saleOut.tb_SaleOutDetails.Sum(c => c.Quantity);
                                    //更新数量
                                    int purupdatequantity = await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOut>(saleOut).ExecuteCommandAsync();
                                    if (purupdatequantity > 0)
                                    {
                                        MainForm.Instance.PrintInfoLog($"出库单{saleOut.SaleOutNo}，明细数量和总数量不对。已修复！");
                                    }
                                    else
                                    {
                                        MainForm.Instance.PrintInfoLog($"出库单{saleOut.SaleOutNo}，明细数量和总数量不对。已修复！");
                                    }
                                }
                                else
                                {
                                    MainForm.Instance.PrintInfoLog($"出库单{saleOut.SaleOutNo}，明细数量和总数量不对，是否修复？");
                                }
                            }

                            //=======

                            //修复：龙哥批量从销售订单中转换为销售出库单。后面不知道做了一个什么操作。订单中，出库数量 都 对。状态为结案。但是出库单中为未审核。
                            //先把订单状态改为结案

                            //如果对应的订单中已交数量等于出库明细中数量 ，则 出库为审核通过
                            if (saleOut.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity) == saleOut.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty))
                            {
                                if (saleOut.tb_saleorder.DataStatus == (int)DataStatus.完结 && saleOut.ApprovalStatus == (int)ApprovalStatus.未审核)
                                {
                                    if (MessageBox.Show($"销售订单{saleOut.tb_saleorder.SOrderNo}，的总数量等于已出库数量，并且订单为结案，是否将对应的出库单设置为审核？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                    {
                                        //如果订单已完结，则出库单状态改为审核通过
                                        saleOut.DataStatus = (int)DataStatus.确认;
                                        saleOut.Approver_at = DateTime.Now;
                                        saleOut.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                                        saleOut.ApprovalStatus = (int)ApprovalStatus.已审核;
                                        saleOut.ApprovalResults = true;
                                        saleOut.ApprovalOpinions = "数量修复：单已完结，如果对应的订单中已交数量等于出库明细中数量 ，则出库为审核通过";
                                        if (MessageBox.Show($"销售订单{saleOut.tb_saleorder.SOrderNo}，明细数量和总数量不对，是否修复？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                        {
                                            int SaleOutrs = await MainForm.Instance.AppContext.Db.Updateable(saleOut).ExecuteCommandAsync();
                                            if (SaleOutrs > 0)
                                            {
                                                SaleOutCouter++;
                                            }
                                        }
                                    }
                                }

                            }


                        }
                        MainForm.Instance.PrintInfoLog("检测到问题数据行数:" + SaleOutCouter);




                        break;

                    default:
                        break;
                }

                //var conModels = new List<IConditionalModel>();
                //conModels.Add(new ConditionalModel
                //{
                //    FieldName = KeyColName,
                //    ConditionalType = ConditionalType.In,
                //    FieldValue = string.Join(",", logicPKValueList)
                //});



                List<DataRow> drs = new List<DataRow>();


            }

        }

        private void 成本价格修复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CostFix();
        }

        private void kryptonCommand数据修复指令集合_Execute(object sender, EventArgs e)
        {
             
            CostFix();
        }









        #region 数据修复

        private async void CostFix()
        {
            /*有库存数据。包含0的情况下。如果价格为空。为0.使用最后的采购入库价格修复。*/
            //查看出库明细和总数量是否一样。
            List<tb_Inventory> tb_Inventorys = new List<tb_Inventory>();
            tb_Inventorys = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                .Where(c => c.Inv_Cost == 0)
                .ToListAsync();

            for (int i = 0; i < tb_Inventorys.Count; i++)
            {
                List<View_PurEntryItems> purEntryItems = await MainForm.Instance.AppContext.Db.Queryable<View_PurEntryItems>()
                .Where(c => c.ProdDetailID == tb_Inventorys[i].ProdDetailID && c.TransactionPrice != 0)
                .OrderByDescending(c => c.EntryDate)
                .ToListAsync();
                if (purEntryItems.Count > 0)
                {
                    tb_Inventorys[i].CostFIFO = purEntryItems[0].TransactionPrice.Value;
                    tb_Inventorys[i].CostMonthlyWA = purEntryItems[0].TransactionPrice.Value;
                    tb_Inventorys[i].CostMovingWA = purEntryItems[0].TransactionPrice.Value;
                    tb_Inventorys[i].Inv_AdvCost = purEntryItems[0].TransactionPrice.Value;
                    tb_Inventorys[i].Inv_Cost = purEntryItems[0].TransactionPrice.Value;
                }
            }

            int counter = await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(tb_Inventorys)
                        .UpdateColumns(t => new { t.Inv_Cost, t.Inv_AdvCost, t.CostMonthlyWA, t.CostMovingWA, t.CostFIFO }).ExecuteCommandAsync();
            if (counter > 0)
            {
                MainForm.Instance.PrintInfoLog("成本价格修改，更新成功" + counter);
            }
        }

        #endregion
    }
}
