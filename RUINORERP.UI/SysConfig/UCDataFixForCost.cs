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
using RUINORERP.Common.Helper;
using System.Reflection;
using RUINORERP.Business;
using System.Collections.Concurrent;
using Org.BouncyCastle.Math.Field;
using RUINORERP.UI.PSI.PUR;
using FastReport.DevComponents.DotNetBar.Controls;
using RUINORERP.UI.SS;
using System.Management.Instrumentation;
using RUINORERP.Business.CommService;
using RUINORERP.Model.CommonModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Netron.GraphLib;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Global.EnumExt.CRM;
using HLH.Lib.Security;
using NPOI.SS.Formula.Functions;
using RUINORERP.UI.PSI.SAL;
using HLH.WinControl.MyTypeConverter;
using RUINORERP.UI.UserCenter.DataParts;
using Fireasy.Common.Extensions;
using RUINORERP.UI.UControls;
using RUINORERP.Common.CollectionExtension;
using System.Diagnostics;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.ATechnologyStack;
using Winista.Text.HtmlParser.Tags;


namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("成本数据校正", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataFixForCost : UserControl
    {
        public UCDataFixForCost()
        {
            InitializeComponent();
        }



        private void UCDataFix_Load(object sender, EventArgs e)
        {
            List<BizType> list = new List<BizType>();
            list.Add(BizType.BOM物料清单);
            list.Add(BizType.制令单);
            list.Add(BizType.缴库单);
            list.Add(BizType.销售订单);
            list.Add(BizType.生产领料单);
            list.Add(BizType.借出单);
            list.Add(BizType.其他出库单);
            list.Add(BizType.销售出库单);

            foreach (var item in list)
            {
                TreeNode tn = new TreeNode();
                tn.Tag = item;
                tn.Text = item.ToString();
                tn.Checked = true;
                treeViewNeedUpdateCostList.Nodes.Add(tn);
            }

            DataBindingHelper.BindData4Cmb<tb_ProductType>(InventoryDto, k => k.Type_ID, v => v.TypeName, cmbType);
        }

        View_Inventory InventoryDto = new View_Inventory();

        BizTypeMapper mapper = new BizTypeMapper();


        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateRelatedDataRows">更新有关系的出库数据</param>
        /// <param name="SKU"></param>
        /// <param name="ProdDetailID"></param>
        /// <returns></returns>
        private async Task<List<tb_Inventory>> CostFix(bool updateRelatedDataRows = false, string SKU = "", long ProdDetailID = 0)
        {
            List<tb_Inventory> Allitems = new List<tb_Inventory>();
            try
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.BeginTran();
                }
                //MainForm.Instance.AppContext.Db.Insertable(new Order() { .....}).ExecuteCommand();
                // MainForm.Instance.AppContext.Db.Insertable(new Order() { .....}).ExecuteCommand();
                //成本修复思路
                //1）成本本身修复，将所有入库明细按加权平均算一下。更新到库存里面。
                //2）修复所有出库明细，主要是销售出库，当然还有其它，比方借出，成本金额是重要的指标数据
                //3）成本修复 分  成品 外采和生产  因为这两种成本产生的方式不一样
                #region 成本本身修复
                Allitems = MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                           .AsNavQueryable()
                           .Includes(c => c.tb_proddetail, d => d.tb_PurEntryDetails, e => e.tb_proddetail, f => f.tb_prod)
                           .Includes(c => c.tb_proddetail, d => d.tb_prod)
                           .WhereIF(!string.IsNullOrEmpty(SKU), c => c.tb_proddetail.SKU == SKU)
                            .WhereIF(ProdDetailID > 0, c => c.ProdDetailID == ProdDetailID)
                           // .IFWhere(c => c.tb_proddetail.SKU == "SKU7E881B4629")
                           .ToList();

                List<tb_Inventory> updateInvList = new List<tb_Inventory>();
                foreach (tb_Inventory item in Allitems)
                {
                    if (item.tb_proddetail.tb_PurEntryDetails.Count > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.TransactionPrice) > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.Quantity) > 0
                        )
                    {
                        //参与成本计算的入库明细记录。要排除单价为0的项
                        var realDetails = item.tb_proddetail.tb_PurEntryDetails.Where(c => c.UnitPrice > 0).ToList();

                        //每笔的入库的数量*成交价/总数量
                        var transPrice = realDetails
                            .Where(c => c.TransactionPrice > 0 && c.Quantity > 0 && c.UnitPrice > 0)
                            .Sum(c => c.TransactionPrice * c.Quantity) / realDetails.Sum(c => c.Quantity);
                        if (transPrice > 0)
                        {
                            //百分比
                            decimal diffpirce = Math.Abs(transPrice - item.Inv_Cost);
                            diffpirce = Math.Round(diffpirce, 2);
                            double percentDiff = ComparePrice(item.Inv_Cost.ToDouble(), transPrice.ToDouble());
                            if (percentDiff > 10)
                            {
                                //}
                                //transPrice = Math.Round(transPrice, 3);
                                //decimal diffpirce = Math.Abs(transPrice - item.Inv_Cost);
                                //if (diffpirce > 0.2m)
                                //{
                                if (rdb成本为0的才修复.Checked && item.Inv_Cost == 0)
                                {
                                    richTextBoxLog.AppendText($"产品{item.tb_proddetail.tb_prod.CNName} " +
                                        $"{item.ProdDetailID}  SKU:{item.tb_proddetail.SKU}   旧成本{item.Inv_Cost},  相差为{diffpirce}   百分比为{percentDiff}%,    修复为：{transPrice}：" + "\r\n");

                                    item.CostMovingWA = transPrice;
                                    item.Inv_AdvCost = item.CostMovingWA;
                                    item.Inv_Cost = item.CostMovingWA;
                                    item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;
                                    item.Notes += $"{System.DateTime.Now.ToString("yyyy-MM-dd")}成本修复为：{transPrice}";
                                    updateInvList.Add(item);
                                }
                                if (rdb小于单项成本才更新.Checked && item.Inv_Cost != 0)
                                {
                                    richTextBoxLog.AppendText($"产品{item.tb_proddetail.tb_prod.CNName} " +
                                     $"{item.ProdDetailID}  SKU:{item.tb_proddetail.SKU}   旧成本{item.Inv_Cost},  相差为{diffpirce}   百分比为{percentDiff}%,    修复为：{transPrice}：" + "\r\n");

                                    item.CostMovingWA = transPrice;
                                    item.Inv_AdvCost = item.CostMovingWA;
                                    item.Inv_Cost = item.CostMovingWA;
                                    item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;
                                    updateInvList.Add(item);
                                }
                                if (rdb按库存成本比例.Checked && item.Inv_Cost != 0)
                                {
                                    richTextBoxLog.AppendText($"产品{item.tb_proddetail.tb_prod.CNName} " +
                                     $"{item.ProdDetailID}  SKU:{item.tb_proddetail.SKU}   旧成本{item.Inv_Cost},  相差为{diffpirce}   百分比为{percentDiff}%,    修复为：{transPrice}：" + "\r\n");

                                    item.CostMovingWA = transPrice;
                                    item.Inv_AdvCost = item.CostMovingWA;
                                    item.Inv_Cost = item.CostMovingWA;
                                    item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;
                                    updateInvList.Add(item);
                                }
                            }
                        }
                    }
                }
                if (chkTestMode.Checked)
                {
                    richTextBoxLog.AppendText($"要修复的行数为:{Allitems.Count}" + "\r\n");
                }
                if (!chkTestMode.Checked )
                {
                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updateInvList).UpdateColumns(t => new { t.CostMovingWA, t.Inv_AdvCost, t.Inv_Cost }).ExecuteCommandAsync();
                    richTextBoxLog.AppendText($"修复成本价格成功：{totalamountCounter} " + "\r\n");
                }
                #endregion
                if (updateRelatedDataRows)
                {
                    UpdateRelatedCost(updateInvList);
                }

            }
            catch (Exception ex)
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.RollbackTran();
                }
                throw ex;
            }
            return Allitems;
        }

        */


        //写一个方法来实现两个价格的比较 前一个为原价，后一个为最新价格。求最新价格大于前的价格的百分比。价格是ecimal类型
        private double ComparePrice(double oldPrice, double newPrice)
        {
            if (oldPrice < 0 || newPrice < 0)
            {
                //如果有负 直接要求更新
                return 100;
                //throw new ArgumentException("Prices cannot be negative.");
            }

            if (oldPrice == 0)
            {
                // 如果原价为 0，无法计算百分比增长
                // 根据需求返回 -1 或 throw 异常
                return -1; // 或者抛出定制异常
            }
            double diffpirce = Math.Abs(newPrice - oldPrice);
            double percentage = (diffpirce / oldPrice * 100);
            return Math.Round(percentage, 2); // 四舍五入到 2 位小数
        }

        private async void btnQuery_Click(object sender, EventArgs e)
        {
            List<View_Inventory> inventories = new List<View_Inventory>();
            inventories = await MainForm.Instance.AppContext.Db.Queryable<View_Inventory>()
                          .Includes(a => a.tb_inventory)
                          .WhereIF(!string.IsNullOrEmpty(txtCNName.Text), c => c.CNName.Contains(txtCNName.Text))
                          .WhereIF(!string.IsNullOrEmpty(txtProp.Text), c => c.prop.Contains(txtProp.Text))
                          .WhereIF(!string.IsNullOrEmpty(txtSearchKey.Text), c => c.SKU == txtSearchKey.Text)
                          .WhereIF((InventoryDto.Type_ID != null && InventoryDto.Type_ID != -1), c => c.Type_ID == InventoryDto.Type_ID)
                          .ToListAsync();
            bindingSourceInv.DataSource = inventories.ToBindingSortCollection();
            dataGridViewInv.DataSource = bindingSourceInv;

            //只显示成本 和详情ID。
            foreach (DataGridViewColumn item in dataGridViewInv.Columns)
            {
                if (item.Name == "Inv_Cost" || item.Name == "ProdDetailID")
                {
                    item.Visible = true;
                    if (item.Name == "Inv_Cost")
                    {
                        item.HeaderText = "成本";
                    }
                    if (item.Name == "ProdDetailID")
                    {
                        item.HeaderText = "产品详情";
                        item.Width = 200;
                    }
                }
                else
                {
                    item.Visible = false;
                }
            }
            richTextBoxLog.AppendText($"查询结果{inventories.Count} " + "\r\n");
        }









        private static void CancelCheckedExceptOne(TreeNodeCollection tnc, TreeNode tn)
        {
            foreach (TreeNode item in tnc)
            {
                if (item != tn)
                    item.Checked = false;
                if (item.Nodes.Count > 0)
                    CancelCheckedExceptOne(item.Nodes, tn);
            }
        }

        private async void 全部更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewInv.SelectedRows != null)
            {
                List<tb_Inventory> inventories = new List<tb_Inventory>();

                foreach (DataGridViewRow dr in dataGridViewInv.SelectedRows)
                {
                    if (dr.DataBoundItem is View_Inventory inventory)
                    {
                        inventories.Add(inventory.tb_inventory);
                    }
                }
                await UpdateInventoryCost(inventories);
                UpdateRelatedCost(inventories);
            }

            //if (dataGridViewInv.CurrentRow != null
            //    && dataGridViewInv.CurrentRow.DataBoundItem is View_Inventory inventory)
            //{
            //    await CostFix(true, string.Empty, inventory.ProdDetailID.Value);
            //}
        }

        private void txtSearchKey_TextChanged(object sender, EventArgs e)
        {

            foreach (DataGridViewRow dr in dataGridViewInv.Rows)
            {
                if (dr.DataBoundItem is tb_Inventory Inventory)
                {
                    string keywords = txtSearchKey.Text.ToLower().Trim();
                    if (keywords.Length > 0)
                    {
                        if (dr.Cells["ProdDetailID"].Value.ToString().Contains(keywords))
                        {
                            dr.Selected = true;
                            // 滚动到选中行
                            dataGridViewInv.CurrentCell = dr.Cells[0]; // 设置当前单元格为该行的第一个单元格
                        }
                        else if (Inventory.tb_proddetail != null && Inventory.tb_proddetail.SKU.ToString().Contains(keywords))
                        {
                            dr.Selected = true;
                            // 滚动到选中行
                            dataGridViewInv.CurrentCell = dr.Cells[0]; // 设置当前单元格为该行的第一个单元格
                        }
                        {
                            dr.Selected = false;
                        }
                    }

                }
            }
        }



        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (TreeNode item in treeViewNeedUpdateCostList.Nodes)
            {
                item.Checked = true;
            }
        }

        private void 全不选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode item in treeViewNeedUpdateCostList.Nodes)
            {
                item.Checked = false;
            }
        }

        private void 返选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode item in treeViewNeedUpdateCostList.Nodes)
            {
                item.Checked = !item.Checked;
            }
        }

        private void dataGridViewInv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            // 如果列是隐藏的，直接返回
            if (!dataGridView.Columns[e.ColumnIndex].Visible)
            {
                return;
            }

            if (e.Value == null)
            {
                e.Value = "";
                return;
            }

            // 获取列名
            string columnName = dataGridView.Columns[e.ColumnIndex].Name;
            object obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(e.Value);
            if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
            {
                e.Value = prodDetail.SKU + " " + prodDetail.CNName + prodDetail.prop + prodDetail.Specifications;
            }
        }

        private void dataGridViewInv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            View_Inventory child = null;
            long ProdDetailID = 0;
            View_ProdDetail prodDetail = null;
            if (dataGridViewInv.CurrentRow.DataBoundItem is View_Inventory inventory)
            {
                ProdDetailID = inventory.ProdDetailID.Value;
                child = inventory;
                #region 采购入库明细
                List<dynamic> PurEntryItems = MainForm.Instance.AppContext.Db.Queryable<View_PurEntryItems>()
                         .OrderBy(a => a.EntryDate)
                        .Where(a => a.ProdDetailID == ProdDetailID)
                        .Select(it => (dynamic)new
                        {
                            入库日期 = it.EntryDate,
                            采购入库单号 = it.PurEntryNo,
                            SKU码 = it.SKU,
                            成本 = it.TransactionPrice,
                            数量 = it.Quantity
                        })
                        .ToList();

                dataGridViewPurEntryItems.Dock = DockStyle.Fill;
                dataGridViewPurEntryItems.DataSource = PurEntryItems.ToDataTable();
                // 自动调整列宽
                dataGridViewPurEntryItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridViewPurEntryItems.AutoResizeRows();
                txtUnitCost.Text = inventory.Inv_Cost.ToString();
                #endregion

                object obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(ProdDetailID);
                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail _prodDetail)
                {
                    prodDetail = _prodDetail;
                    txtSearchKey.Text = prodDetail.SKU;
                }
            }

            //显示选择库存行对应的其它相关数据
            panel相关数据.Controls.Clear();
            tabControl.TabPages.Clear();
            tabControl.Dock = DockStyle.Fill;
            foreach (TreeNode item in treeViewNeedUpdateCostList.Nodes)
            {
                if (item.Checked && item.Tag is BizType bt)
                {
                    switch (bt)
                    {
                        case BizType.销售订单:

                            TabPage page销售订单 = new TabPage();
                            #region 销售订单 出库  退货 记录成本修复

                            List<dynamic> SaleOrderItems = MainForm.Instance.AppContext.Db.Queryable<View_SaleOrderItems>()
                            .OrderBy(a => a.SaleDate)
                            .Where(a => a.ProdDetailID == ProdDetailID)
                            .Select(it => (dynamic)new
                            {
                                出库单号 = it.SOrderNo,
                                出库日期 = it.SaleDate,
                                sku = it.SKU,
                                成本 = it.Cost,
                                成本小计 = it.SubtotalCostAmount,
                                数量 = it.Quantity
                            }).ToList();



                            DataGridView dgv销售订单 = new DataGridView();
                            dgv销售订单.AutoGenerateColumns = true;
                            dgv销售订单.DataSource = SaleOrderItems.ToDataTable();
                            dgv销售订单.Tag = bt;

                            // 自动调整列宽
                            dgv销售订单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            dgv销售订单.AutoResizeRows();
                            dgv销售订单.Dock = DockStyle.Fill;
                            #endregion
                            page销售订单.Text = "销售订单" + SaleOrderItems.Count;
                            page销售订单.Controls.Add(dgv销售订单);
                            tabControl.TabPages.Add(page销售订单);
                            dgv销售订单.Refresh();
                            break;
                        case BizType.销售出库单:
                            TabPage page销售出库单 = new TabPage();
                            #region 销售出库

                            List<dynamic> SaleOutItems = MainForm.Instance.AppContext.Db.Queryable<View_SaleOutItems>()
                            .OrderBy(a => a.OutDate)
                            .Where(a => a.ProdDetailID == ProdDetailID)
                            .Select(it => (dynamic)new
                            {
                                出库单号 = it.SaleOutNo,
                                出库日期 = it.OutDate,
                                sku = it.SKU,
                                成本 = it.Cost,
                                成本小计 = it.SubtotalCostAmount,
                                数量 = it.Quantity
                            }).ToList();

                            DataGridView dgv销售出库 = new DataGridView();
                            dgv销售出库.AutoGenerateColumns = true;
                            dgv销售出库.DataSource = SaleOutItems.ToDataTable();
                            dgv销售出库.Tag = bt;

                            // 自动调整列宽
                            dgv销售出库.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            dgv销售出库.AutoResizeRows();
                            dgv销售出库.Dock = DockStyle.Fill;
                            #endregion
                            page销售出库单.Text = "销售出库" + SaleOutItems.Count;
                            page销售出库单.Controls.Add(dgv销售出库);
                            tabControl.TabPages.Add(page销售出库单);
                            break;
                        case BizType.销售退回单:
                            break;
                        case BizType.采购订单:
                            break;
                        case BizType.采购入库单:
                            break;
                        case BizType.采购退货单:
                            break;
                        case BizType.其他入库单:
                            break;
                        case BizType.其他出库单:
                            TabPage page其他出库单 = new TabPage();
                            #region 其它出库

                            List<dynamic> list其它出库 = MainForm.Instance.AppContext.Db.Queryable<View_StockOutItems>()
                            .Where(a => a.ProdDetailID == ProdDetailID)
                             .Select(it => (dynamic)new
                             {
                                 单号 = it.BillNo,
                                 单据日期 = it.Bill_Date,
                                 sku = it.SKU,
                                 数量 = it.Qty,
                                 成本 = it.Cost,
                                 成本小计 = it.SubtotalCostAmount
                             }).ToList();



                            DataGridView dgv其他出库单 = new DataGridView();
                            dgv其他出库单.DataSource = list其它出库.ToDataTable();
                            // 自动调整列宽
                            dgv其他出库单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            dgv其他出库单.AutoResizeRows();
                            dgv其他出库单.Dock = DockStyle.Fill;
                            dgv其他出库单.Tag = bt;

                            #endregion
                            page其他出库单.Text = "其他出库单" + list其它出库.Count;
                            page其他出库单.Controls.Add(dgv其他出库单);
                            tabControl.TabPages.Add(page其他出库单);
                            break;
                        case BizType.盘点单:
                            break;
                        case BizType.制令单:
                            break;
                        case BizType.BOM物料清单:
                            TabPage pageBOM = new TabPage();
                            #region BOM物料清单

                            List<dynamic> ProdbomItems = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_SDetail>()
                       .OrderBy(a => a.SKU)
                       .Where(a => a.ProdDetailID == ProdDetailID)
                       .Select(it => (dynamic)new
                       {
                           BOMID = it.BOM_ID,
                           sku = it.SKU,
                           成本 = it.UnitCost,
                           成本小计 = it.SubtotalUnitCost,
                           数量 = it.UsedQty
                       }).ToList();


                            DataGridView dgvBOM = new DataGridView();
                            dgvBOM.DataSource = ProdbomItems.ToDataTable();
                            dgvBOM.Dock = DockStyle.Fill;
                            dgvBOM.Tag = bt;
                            // 自动调整列宽
                            dgvBOM.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            dgvBOM.AutoResizeRows();

                            #endregion

                            pageBOM.Text = "配方清单" + ProdbomItems.Count;
                            pageBOM.Controls.Add(dgvBOM);
                            tabControl.TabPages.Add(pageBOM);

                            break;
                        case BizType.生产领料单:
                            TabPage page生产领料单 = new TabPage();

                            #region 领料单

                            List<dynamic> MRBills = MainForm.Instance.AppContext.Db.Queryable<View_MaterialRequisitionItems>()
                          .Where(a => a.ProdDetailID == ProdDetailID)
                          .Select(it => (dynamic)new
                          {
                              领料单号 = it.MaterialRequisitionNO,
                              领料日期 = it.DeliveryDate,
                              SKU码 = it.SKU,
                              成本 = it.Cost,
                              成本小计 = it.SubtotalCost,
                              数量 = it.ActualSentQty
                          }).ToList();



                            DataGridView dgv生产领料单 = new DataGridView();
                            dgv生产领料单.DataSource = MRBills.ToDataTable();
                            dgv生产领料单.Dock = DockStyle.Fill;
                            dgv生产领料单.Tag = bt;
                            // 自动调整列宽
                            dgv生产领料单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            dgv生产领料单.AutoResizeRows();

                            #endregion
                            page生产领料单.Text = "生产领料单" + MRBills.Count;
                            page生产领料单.Controls.Add(dgv生产领料单);
                            tabControl.TabPages.Add(page生产领料单);
                            break;
                        case BizType.生产退料单:
                            break;
                        case BizType.生产补料单:
                            break;
                        case BizType.发料计划单:
                            break;
                        case BizType.成品缴库:
                            break;

                        case BizType.退料单:
                            break;
                        case BizType.缴库单:

                            TabPage page缴库单 = new TabPage();
                            #region 缴库单

                            List<dynamic> ProdFinishedItems = MainForm.Instance.AppContext.Db.Queryable<View_FinishedGoodsInvItems>()
                       //.OrderBy(a => a.)
                       .Where(a => a.ProdDetailID == ProdDetailID)
                       .Select(it => (dynamic)new
                       {
                           缴库单号 = it.DeliveryBillNo,
                           缴库日期 = it.DeliveryDate,
                           成本 = it.UnitCost,
                           成本小计 = it.ProductionAllCost,
                           数量 = it.Qty
                       }).ToList();

                            DataGridView dgv缴库单 = new DataGridView();
                            dgv缴库单.DataSource = ProdFinishedItems.ToDataTable();
                            dgv缴库单.Dock = DockStyle.Fill;
                            // 自动调整列宽
                            dgv缴库单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            dgv缴库单.AutoResizeRows();
                            dgv缴库单.Tag = bt;
                            #endregion

                            page缴库单.Text = "缴库单" + ProdFinishedItems.Count;
                            page缴库单.Controls.Add(dgv缴库单);
                            tabControl.TabPages.Add(page缴库单);

                            break;
                        case BizType.请购单:
                            break;
                        case BizType.产品分割单:
                            break;
                        case BizType.产品组合单:
                            break;
                        case BizType.借出单:
                            TabPage page借出单 = new TabPage();
                            #region 借出单 归还

                            List<dynamic> ProdBorrowingItems = MainForm.Instance.AppContext.Db.Queryable<View_ProdBorrowing>()
                               .OrderBy(a => a.Out_date)
                               .Where(a => a.ProdDetailID == ProdDetailID)
                               .Select(it => (dynamic)new
                               {
                                   借出单号 = it.BorrowNo,
                                   出库日期 = it.Out_date,
                                   sku = it.SKU,
                                   成本 = it.Cost,
                                   成本小计 = it.SubtotalCostAmount,
                                   数量 = it.Qty
                               }).ToList();


                            DataGridView dgvBorrow = new DataGridView();
                            dgvBorrow.Tag =
                            dgvBorrow.AutoGenerateColumns = true;
                            dgvBorrow.DataSource = ProdBorrowingItems.ToDataTable();
                            dgvBorrow.Dock = DockStyle.Fill;
                            dgvBorrow.Tag = bt;
                            // 自动调整列宽
                            dgvBorrow.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            dgvBorrow.AutoResizeRows();
                            #endregion

                            page借出单.Text = "借出单" + ProdBorrowingItems.Count;
                            page借出单.Controls.Add(dgvBorrow);
                            tabControl.TabPages.Add(page借出单);
                            break;
                        case BizType.归还单:
                            break;


                        case BizType.产品转换单:
                            break;
                        case BizType.调拨单:
                            break;
                        case BizType.采购退货入库:
                            break;
                        case BizType.售后返厂退回:
                            break;
                        case BizType.售后返厂入库:
                            break;


                        case BizType.返工退库单:
                            break;
                        case BizType.返工退库统计:
                            break;
                        case BizType.返工入库单:
                            break;
                        case BizType.返工入库统计:
                            break;

                        default:
                            break;
                    }
                }
            }
            panel相关数据.Controls.Add(tabControl);

        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                List<Control> controls = tabControl.SelectedTab.Controls.CastToList<Control>();
                DataGridView dgv = controls.FirstOrDefault(c => c.GetType().Name == "DataGridView") as DataGridView;
                if (dgv != null)
                {
                    dgv.ContextMenuStrip = null;
                    //dgv.ContextMenuStrip = contextMenuStripCmd;
                    //dgv.Dock = DockStyle.Fill;
                    // dgv.Refresh();
                }

            }
        }


        /// <summary>
        /// 更新库存成本
        /// </summary>
        /// <param name="updateInvList">要更新的对象集合</param>
        /// <returns></returns>
        private async Task<List<tb_Inventory>> UpdateInventoryCost(List<tb_Inventory> updateInvList)
        {
            List<tb_Inventory> Allitems = new List<tb_Inventory>();
            try
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.BeginTran();
                }
                //MainForm.Instance.AppContext.Db.Insertable(new Order() { .....}).ExecuteCommand();
                // MainForm.Instance.AppContext.Db.Insertable(new Order() { .....}).ExecuteCommand();
                //成本修复思路
                //1）成本本身修复，将所有入库明细按加权平均算一下。更新到库存里面。
                //2）修复所有出库明细，主要是销售出库，当然还有其它，比方借出，成本金额是重要的指标数据
                //3）成本修复 分  成品 外采和生产  因为这两种成本产生的方式不一样
                #region 成本本身修复

                foreach (tb_Inventory item in updateInvList)
                {
                    if (item.tb_proddetail.tb_PurEntryDetails.Count > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.TransactionPrice) > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.Quantity) > 0
                        )
                    {
                        //参与成本计算的入库明细记录。要排除单价为0的项
                        var realDetails = item.tb_proddetail.tb_PurEntryDetails.Where(c => c.UnitPrice > 0).ToList();

                        //每笔的入库的数量*成交价/总数量
                        var transPrice = realDetails
                            .Where(c => c.TransactionPrice > 0 && c.Quantity > 0 && c.UnitPrice > 0)
                            .Sum(c => c.TransactionPrice * c.Quantity) / realDetails.Sum(c => c.Quantity);
                        if (transPrice > 0)
                        {
                            //百分比
                            decimal diffpirce = Math.Abs(transPrice - item.Inv_Cost);
                            diffpirce = Math.Round(diffpirce, 2);
                            double percentDiff = ComparePrice(item.Inv_Cost.ToDouble(), transPrice.ToDouble());
                            if (percentDiff > 10)
                            {
                                //}
                                //transPrice = Math.Round(transPrice, 3);
                                //decimal diffpirce = Math.Abs(transPrice - item.Inv_Cost);
                                //if (diffpirce > 0.2m)
                                //{
                                if (rdb成本为0的才修复.Checked && item.Inv_Cost == 0)
                                {
                                    richTextBoxLog.AppendText($"产品{item.tb_proddetail.tb_prod.CNName} " +
                                        $"{item.ProdDetailID}  SKU:{item.tb_proddetail.SKU}   旧成本{item.Inv_Cost},  相差为{diffpirce}   百分比为{percentDiff}%,    修复为：{transPrice}：" + "\r\n");

                                    item.CostMovingWA = transPrice;
                                    item.Inv_AdvCost = item.CostMovingWA;
                                    item.Inv_Cost = item.CostMovingWA;
                                    item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;
                                    item.Notes += $"{System.DateTime.Now.ToString("yyyy-MM-dd")}成本修复为：{transPrice}";
                                    updateInvList.Add(item);
                                }
                                if (rdb小于单项成本才更新.Checked && item.Inv_Cost < txtUnitCost.Text.ToDecimal())
                                {
                                    richTextBoxLog.AppendText($"产品{item.tb_proddetail.tb_prod.CNName} " +
                                     $"{item.ProdDetailID}  SKU:{item.tb_proddetail.SKU}   旧成本{item.Inv_Cost},  相差为{diffpirce}   百分比为{percentDiff}%,    修复为：{transPrice}：" + "\r\n");

                                    item.CostMovingWA = transPrice;
                                    item.Inv_AdvCost = item.CostMovingWA;
                                    item.Inv_Cost = item.CostMovingWA;
                                    item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;
                                    updateInvList.Add(item);
                                }

                            }
                        }
                    }
                }
                if (chkTestMode.Checked)
                {
                    richTextBoxLog.AppendText($"要修复的行数为:{Allitems.Count}" + "\r\n");
                }
                if (!chkTestMode.Checked)
                {
                    int totalamountCounter = await MainForm.Instance.AppContext.Db.Updateable(updateInvList).UpdateColumns(t => new { t.CostMovingWA, t.Inv_AdvCost, t.Inv_Cost }).ExecuteCommandAsync();
                    richTextBoxLog.AppendText($"修复成本价格成功：{totalamountCounter} " + "\r\n");
                }
                #endregion
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.CommitTran();
                }

            }
            catch (Exception ex)
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.RollbackTran();
                }
                throw ex;
            }
            return Allitems;

        }
        /// <summary>
        /// 更新相关出库的明细中的成本
        /// </summary>
        /// <param name="updateInvList"></param>
        private async void UpdateRelatedCost(List<tb_Inventory> updateInvList, bool FixSubtotal = false)
        {

            if (!chkTestMode.Checked)
            {
                MainForm.Instance.AppContext.Db.Ado.BeginTran();
            }
            try
            {
                if (chk单项成本更新.Checked && updateInvList.Count > 1)
                {
                    MessageBox.Show("选择单项成本更新时,更新库存数据不能大于1");
                    return;
                }
                foreach (var child in updateInvList)
                {
                    if (chk单项成本更新.Checked && updateInvList.Count == 1)
                    {
                        child.Inv_Cost = txtUnitCost.Text.ToDecimal();
                    }
                    #region 更新相关数据

                    foreach (TreeNode item in treeViewNeedUpdateCostList.Nodes)
                    {
                        if (item.Checked && item.Tag is BizType bt)
                        {
                            switch (bt)
                            {
                                case BizType.BOM物料清单:

                                    #region 更新BOM价格,当前产品存在哪些BOM中，则更新所有BOM的价格包含主子表数据的变化

                                    List<tb_BOM_S> BOM_SOrders = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                                    .InnerJoin<tb_BOM_SDetail>((a, b) => a.BOM_ID == b.BOM_ID)
                                    .Includes(a => a.tb_BOM_SDetails)
                                    .Where(a => a.tb_BOM_SDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                                    var distinctBOMbills = BOM_SOrders
                                    .GroupBy(o => o.BOM_ID)
                                    .Select(g => g.First())
                                    .ToList();

                                    List<tb_BOM_SDetail> updateListbomdetail = new List<tb_BOM_SDetail>();
                                    foreach (var bill in distinctBOMbills)
                                    {
                                        foreach (var bomDetail in bill.tb_BOM_SDetails)
                                        {
                                            if (bomDetail.ProdDetailID == child.ProdDetailID)
                                            {
                                                //如果存在则更新 
                                                decimal diffpirce = Math.Abs(bomDetail.UnitCost - child.Inv_Cost);
                                                if (diffpirce > 0.2m)
                                                {
                                                    if (rdb成本为0的才修复.Checked && bomDetail.UnitCost == 0)
                                                    {
                                                        bomDetail.UnitCost = child.Inv_Cost;
                                                        bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                                        updateListbomdetail.Add(bomDetail);
                                                    }
                                                    if (rdb小于单项成本才更新.Checked && bomDetail.UnitCost != 0)
                                                    {
                                                        if (bomDetail.UnitCost < txtUnitCost.Text.ToDecimal())
                                                        {
                                                            bomDetail.UnitCost = child.Inv_Cost;
                                                            bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                                            updateListbomdetail.Add(bomDetail);
                                                        }
                                                    }
                                                    if (rdb其它.Checked && bomDetail.UnitCost != 0)
                                                    {
                                                        bomDetail.UnitCost = child.Inv_Cost;
                                                        bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                                        updateListbomdetail.Add(bomDetail);
                                                    }
                                                }
                                            }
                                        }

                                        if (updateListbomdetail.Count > 0)
                                        {
                                            bill.TotalMaterialCost = bill.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                                            bill.OutProductionAllCosts = bill.TotalMaterialCost + bill.TotalOutManuCost + bill.OutApportionedCost;
                                            bill.SelfProductionAllCosts = bill.TotalMaterialCost + bill.TotalSelfManuCost + bill.SelfApportionedCost;
                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_BOM_S>(bill).ExecuteCommandAsync();
                                            }
                                        }
                                    }

                                    if (!chkTestMode.Checked && updateListbomdetail.Count > 0)
                                    {
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_BOM_SDetail>(updateListbomdetail).ExecuteCommandAsync();
                                    }


                                    #endregion

                                    break;

                                case BizType.制令单:
                                    #region 更新制令单价格,和BOM类似


                                    List<tb_ManufacturingOrder> MOs = MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                                .InnerJoin<tb_ManufacturingOrderDetail>((a, b) => a.MOID == b.MOID)
                                .Includes(a => a.tb_ManufacturingOrderDetails)
                                .Where(a => a.tb_ManufacturingOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();


                                    var distinctMObills = MOs
                                    .GroupBy(o => o.MOID)
                                    .Select(g => g.First())
                                    .ToList();

                                    List<tb_ManufacturingOrderDetail> updateMOdetail = new List<tb_ManufacturingOrderDetail>();
                                    foreach (var bill in distinctMObills)
                                    {
                                        foreach (tb_ManufacturingOrderDetail Detail in bill.tb_ManufacturingOrderDetails)
                                        {
                                            if (Detail.ProdDetailID == child.ProdDetailID)
                                            {
                                                //如果存在则更新 
                                                decimal diffpirce = Math.Abs(Detail.UnitCost - child.Inv_Cost);
                                                if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.UnitCost.ToDouble()) > 10)
                                                {
                                                    if (rdb成本为0的才修复.Checked && Detail.UnitCost == 0)
                                                    {
                                                        Detail.UnitCost = child.Inv_Cost;
                                                        Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                                        updateMOdetail.Add(Detail);
                                                    }
                                                    if (rdb小于单项成本才更新.Checked && Detail.UnitCost != 0)
                                                    {
                                                        if (Detail.UnitCost < txtUnitCost.Text.ToDecimal())
                                                        {
                                                            Detail.UnitCost = child.Inv_Cost;
                                                            Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                                            updateMOdetail.Add(Detail);
                                                        }
                                                    }
                                                    if (rdb其它.Checked && Detail.UnitCost != 0)
                                                    {
                                                        Detail.UnitCost = child.Inv_Cost;
                                                        Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                                        updateMOdetail.Add(Detail);
                                                    }
                                                }
                                            }
                                        }

                                        bill.TotalMaterialCost = bill.tb_ManufacturingOrderDetails.Sum(c => c.SubtotalUnitCost);
                                        bill.TotalProductionCost = bill.TotalMaterialCost + bill.TotalManuFee;

                                        if (!chkTestMode.Checked && updateMOdetail.Count > 0)
                                        {
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_ManufacturingOrder>(bill).ExecuteCommandAsync();
                                        }
                                    }
                                    if (updateMOdetail.Count > 0)
                                    {
                                        if (!chkTestMode.Checked)
                                        {
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_ManufacturingOrderDetail>(updateMOdetail).ExecuteCommandAsync();
                                        }
                                    }


                                    #endregion

                                    break;
                                case BizType.销售订单:
                                    #region 销售订单 出库  退货 记录成本修复

                                    List<tb_SaleOrder> SOorders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                                     .InnerJoin<tb_SaleOrderDetail>((a, b) => a.SOrder_ID == b.SOrder_ID)
                                    .Includes(a => a.tb_SaleOrderDetails)
                                    .Includes(a => a.tb_SaleOuts, c => c.tb_SaleOutDetails)
                                    .Includes(a => a.tb_SaleOuts, c => c.tb_SaleOutRes, d => d.tb_SaleOutReDetails)
                                    .Where(a => a.tb_SaleOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();
                                    var distinctSoOrders = SOorders
                                    .GroupBy(o => o.SOrder_ID)
                                    .Select(g => g.First())
                                    .ToList();
                                    int ordercounter = 0;
                                    foreach (var order in distinctSoOrders)
                                    {
                                        #region new
                                        bool needupdateorder = false;
                                        foreach (var Detail in order.tb_SaleOrderDetails)
                                        {
                                            if (Detail.ProdDetailID == child.ProdDetailID)
                                            {
                                                //不更新成本只改小计总计
                                                if (rdb小计总计.Checked)
                                                {
                                                    // Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCostAmount = Detail.Cost * Detail.Quantity;
                                                    Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                    if (Detail.TaxRate > 0)
                                                    {
                                                        Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                    }
                                                    Detail.SubtotalUntaxedAmount = Detail.SubtotalTransAmount - Detail.SubtotalTaxAmount;
                                                    needupdateorder = true;
                                                }

                                                decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                                if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                                {
                                                    if (rdb成本为0的才修复.Checked && Detail.Cost == 0)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Quantity;
                                                        Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                        if (Detail.TaxRate > 0)
                                                        {
                                                            Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                        }
                                                        Detail.SubtotalUntaxedAmount = Detail.SubtotalTransAmount - Detail.SubtotalTaxAmount;
                                                        needupdateorder = true;
                                                    }
                                                    if (rdb小于单项成本才更新.Checked && Detail.Cost != 0)
                                                    {
                                                        if (Detail.Cost < txtUnitCost.Text.ToDecimal())
                                                        {
                                                            Detail.Cost = child.Inv_Cost;
                                                            Detail.SubtotalCostAmount = Detail.Cost * Detail.Quantity;
                                                            Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                            if (Detail.TaxRate > 0)
                                                            {
                                                                Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                            }
                                                            Detail.SubtotalUntaxedAmount = Detail.SubtotalTransAmount - Detail.SubtotalTaxAmount;
                                                            needupdateorder = true;
                                                        }
                                                    }
                                                    if (rdb小于单项成本才更新.Checked && Detail.Cost != 0)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Quantity;
                                                        Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                        if (Detail.TaxRate > 0)
                                                        {
                                                            Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                        }
                                                        Detail.SubtotalUntaxedAmount = Detail.SubtotalTransAmount - Detail.SubtotalTaxAmount;
                                                        needupdateorder = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (needupdateorder)
                                        {
                                            ordercounter++;
                                            order.TotalCost = order.tb_SaleOrderDetails.Sum(c => c.SubtotalCostAmount);
                                            order.TotalAmount = order.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount);
                                            order.TotalQty = order.tb_SaleOrderDetails.Sum(c => c.Quantity);
                                            order.TotalTaxAmount = order.tb_SaleOrderDetails.Sum(c => c.SubtotalTaxAmount);
                                            order.TotalUntaxedAmount = order.tb_SaleOrderDetails.Sum(c => c.SubtotalUntaxedAmount);
                                            richTextBoxLog.AppendText($"销售订单{order.SOrderNo}总金额：{order.TotalCost} " + "\r\n");

                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrderDetail>(order.tb_SaleOrderDetails).ExecuteCommandAsync();
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrder>(order).ExecuteCommandAsync();
                                            }
                                        }
                                        /*
                                        #region 销售出库
                                        if (order.tb_SaleOuts != null)
                                        {
                                            int saleoutCounter = 0;
                                            foreach (var SaleOut in order.tb_SaleOuts)
                                            {
                                                if (SaleOut.SaleOutNo == "SOD7E8C121774" || SaleOut.SaleOutNo == "SOD7E92190226")
                                                {

                                                }
                                                bool needupdateOut = false;
                                                foreach (var saleoutdetails in SaleOut.tb_SaleOutDetails)
                                                {
                                                    if (saleoutdetails.ProdDetailID == child.ProdDetailID)
                                                    {
                                                        //不更新成本只改小计总计
                                                        if (rdb小计总计.Checked)
                                                        {
                                                            //saleoutdetails.Cost = child.Inv_Cost;
                                                            saleoutdetails.SubtotalCostAmount = saleoutdetails.Cost * saleoutdetails.Quantity;

                                                            saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                            if (saleoutdetails.TaxRate > 0)
                                                            {
                                                                saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                            }
                                                            saleoutdetails.SubtotalUntaxedAmount = saleoutdetails.SubtotalTransAmount - saleoutdetails.SubtotalTaxAmount;
                                                            needupdateOut = true;
                                                        }
                                                        if (rdb成本为0的才修复.Checked && saleoutdetails.Cost == 0)
                                                        {
                                                            saleoutdetails.Cost = child.Inv_Cost;
                                                            saleoutdetails.SubtotalCostAmount = saleoutdetails.Cost * saleoutdetails.Quantity;

                                                            saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                            if (saleoutdetails.TaxRate > 0)
                                                            {
                                                                saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                            }
                                                            saleoutdetails.SubtotalUntaxedAmount = saleoutdetails.SubtotalTransAmount - saleoutdetails.SubtotalTaxAmount;
                                                            needupdateOut = true;
                                                        }
                                                        if (rdb小于单项成本才更新.Checked && saleoutdetails.Cost != 0)
                                                        {
                                                            if (saleoutdetails.Cost < txtUnitCost.Text.ToDecimal())
                                                            {
                                                                saleoutdetails.Cost = child.Inv_Cost;
                                                                saleoutdetails.SubtotalCostAmount = saleoutdetails.Cost * saleoutdetails.Quantity;

                                                                saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                                if (saleoutdetails.TaxRate > 0)
                                                                {
                                                                    saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                                }
                                                                saleoutdetails.SubtotalUntaxedAmount = saleoutdetails.SubtotalTransAmount - saleoutdetails.SubtotalTaxAmount;
                                                                needupdateOut = true;
                                                            }
                                                        }
                                                        if (rdb按库存成本比例.Checked && saleoutdetails.Cost != 0)
                                                        {
                                                            saleoutdetails.Cost = child.Inv_Cost;
                                                            saleoutdetails.SubtotalCostAmount = saleoutdetails.Cost * saleoutdetails.Quantity;

                                                            saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                            if (saleoutdetails.TaxRate > 0)
                                                            {
                                                                saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                            }
                                                            saleoutdetails.SubtotalUntaxedAmount = saleoutdetails.SubtotalTransAmount - saleoutdetails.SubtotalTaxAmount;
                                                            needupdateOut = true;
                                                        }
                                                    }
                                                }
                                                if (needupdateOut)
                                                {
                                                    saleoutCounter++;
                                                    SaleOut.TotalCost = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount);
                                                    SaleOut.TotalAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalTransAmount);
                                                    SaleOut.TotalQty = SaleOut.tb_SaleOutDetails.Sum(c => c.Quantity);
                                                    SaleOut.TotalTaxAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount);
                                                    SaleOut.TotalUntaxedAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalUntaxedAmount);

                                                    richTextBoxLog.AppendText($"销售出库{SaleOut.SaleOutNo}总金额：{SaleOut.TotalCost} " + "\r\n");
                                                }
                                                if (saleoutCounter > 0)
                                                {
                                                    richTextBoxLog.AppendText($"更新销售出库单 {saleoutCounter} 条" + "\r\n");
                                                }


                                                #region 销售退回
                                                if (SaleOut.tb_SaleOutRes != null)
                                                {
                                                    int saleoutresCounter = 0;
                                                    foreach (var SaleOutRe in SaleOut.tb_SaleOutRes)
                                                    {
                                                        bool needupdateback = false;
                                                        foreach (var SaleOutReDetail in SaleOutRe.tb_SaleOutReDetails)
                                                        {
                                                            if (SaleOutReDetail.ProdDetailID == child.ProdDetailID)
                                                            {
                                                                //不改成本 
                                                                if (rdb小计总计.Checked)
                                                                {
                                                                    SaleOutReDetail.SubtotalCostAmount = SaleOutReDetail.Cost * SaleOutReDetail.Quantity;
                                                                    SaleOutReDetail.SubtotalTransAmount = SaleOutReDetail.TransactionPrice * SaleOutReDetail.Quantity;
                                                                    if (SaleOutReDetail.TaxRate > 0)
                                                                    {
                                                                        SaleOutReDetail.SubtotalTaxAmount = SaleOutReDetail.SubtotalTransAmount / (1 + SaleOutReDetail.TaxRate) * SaleOutReDetail.TaxRate;
                                                                    }
                                                                    SaleOutReDetail.SubtotalUntaxedAmount = SaleOutReDetail.SubtotalTransAmount - SaleOutReDetail.SubtotalTaxAmount;
                                                                    needupdateback = true;
                                                                }
                                                                if (rdb成本为0的才修复.Checked && SaleOutReDetail.Cost == 0)
                                                                {

                                                                    SaleOutReDetail.Cost = child.Inv_Cost;
                                                                    SaleOutReDetail.SubtotalCostAmount = SaleOutReDetail.Cost * SaleOutReDetail.Quantity;
                                                                    SaleOutReDetail.SubtotalTransAmount = SaleOutReDetail.TransactionPrice * SaleOutReDetail.Quantity;
                                                                    if (SaleOutReDetail.TaxRate > 0)
                                                                    {
                                                                        SaleOutReDetail.SubtotalTaxAmount = SaleOutReDetail.SubtotalTransAmount / (1 + SaleOutReDetail.TaxRate) * SaleOutReDetail.TaxRate;
                                                                    }
                                                                    SaleOutReDetail.SubtotalUntaxedAmount = SaleOutReDetail.SubtotalTransAmount - SaleOutReDetail.SubtotalTaxAmount;
                                                                    needupdateback = true;
                                                                }
                                                                if (rdb小于单项成本才更新.Checked && SaleOutReDetail.Cost < txtUnitCost.Text.ToDecimal())
                                                                {

                                                                    SaleOutReDetail.Cost = child.Inv_Cost;
                                                                    SaleOutReDetail.SubtotalCostAmount = SaleOutReDetail.Cost * SaleOutReDetail.Quantity;
                                                                    SaleOutReDetail.SubtotalTransAmount = SaleOutReDetail.TransactionPrice * SaleOutReDetail.Quantity;
                                                                    if (SaleOutReDetail.TaxRate > 0)
                                                                    {
                                                                        SaleOutReDetail.SubtotalTaxAmount = SaleOutReDetail.SubtotalTransAmount / (1 + SaleOutReDetail.TaxRate) * SaleOutReDetail.TaxRate;
                                                                    }
                                                                    SaleOutReDetail.SubtotalUntaxedAmount = SaleOutReDetail.SubtotalTransAmount - SaleOutReDetail.SubtotalTaxAmount;
                                                                    needupdateback = true;
                                                                }
                                                            }
                                                        }

                                                        if (needupdateback)
                                                        {
                                                            saleoutresCounter++;
                                                            //SaleOutRe.TotalCost = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalCostAmount);
                                                            SaleOutRe.TotalAmount = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalTransAmount);
                                                            SaleOutRe.TotalQty = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.Quantity);
                                                        }


                                                        if (SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails != null)
                                                        {
                                                            foreach (var Refurbished in SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails)
                                                            {
                                                                if (Refurbished.ProdDetailID == child.ProdDetailID)
                                                                {
                                                                    //不改成本 
                                                                    if (rdb小计总计.Checked)
                                                                    {
                                                                        Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                                    }
                                                                    if (rdb成本为0的才修复.Checked && Refurbished.Cost == 0)
                                                                    {
                                                                        Refurbished.Cost = child.Inv_Cost;
                                                                        Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                                    }
                                                                    if (rdb小于单项成本才更新.Checked && Refurbished.Cost < txtUnitCost.Text.ToDecimal())
                                                                    {
                                                                        Refurbished.Cost = child.Inv_Cost;
                                                                        Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (needupdateback)
                                                        {
                                                            if (!chkTestMode.Checked)
                                                            {
                                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutRe>(SaleOutRe).ExecuteCommandAsync();
                                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutReDetail>(SaleOutRe.tb_SaleOutReDetails).ExecuteCommandAsync();
                                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutReRefurbishedMaterialsDetail>(SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails).ExecuteCommandAsync();
                                                            }
                                                            richTextBoxLog.AppendText($"销售退回{SaleOutRe.ReturnNo}总金额：{SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalCostAmount)} " + "\r\n");
                                                        }

                                                    }
                                                    if (saleoutresCounter > 0)
                                                    {
                                                        richTextBoxLog.AppendText($"更新销售出库退库单 {saleoutresCounter} 条" + "\r\n");
                                                    }
                                                }

                                                #endregion

                                                if (!chkTestMode.Checked)
                                                {
                                                    await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOut>(SaleOut).ExecuteCommandAsync();
                                                    await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutDetail>(SaleOut.tb_SaleOutDetails).ExecuteCommandAsync();
                                                }
                                            }
                                        }
                                        #endregion
                                        */
                                        #endregion
                                    }
                                    if (ordercounter > 0)
                                    {
                                        richTextBoxLog.AppendText($"更新销售订单 {ordercounter} 条" + "\r\n");
                                    }

                                    #endregion
                                    break;
                                case BizType.销售出库单:
                                    #region 销售出库单

                                    List<tb_SaleOut> SOutorders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                                     .InnerJoin<tb_SaleOutDetail>((a, b) => a.SaleOut_MainID == b.SaleOut_MainID)
                                    .Includes(a => a.tb_SaleOutDetails)
                                    .Includes(c => c.tb_SaleOutRes, d => d.tb_SaleOutReDetails)
                                    .Where(a => a.tb_SaleOutDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                                    var distinctSoutOrders = SOutorders
                                    .GroupBy(o => o.SaleOut_MainID)
                                    .Select(g => g.First())
                                    .ToList();
                                    int sordercounter = 0;
                                    #region 销售出库

                                    int saleoutCounter = 0;
                                    foreach (var SaleOut in distinctSoutOrders)
                                    {
                                        if (SaleOut.SaleOutNo == "SOD7E8C121774" || SaleOut.SaleOutNo == "SOD7E92190226")
                                        {

                                        }
                                        bool needupdateOut = false;
                                        foreach (var saleoutdetails in SaleOut.tb_SaleOutDetails)
                                        {
                                            if (saleoutdetails.ProdDetailID == child.ProdDetailID)
                                            {
                                                //不更新成本只改小计总计
                                                if (rdb小计总计.Checked)
                                                {
                                                    //saleoutdetails.Cost = child.Inv_Cost;
                                                    saleoutdetails.SubtotalCostAmount = saleoutdetails.Cost * saleoutdetails.Quantity;

                                                    saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                    if (saleoutdetails.TaxRate > 0)
                                                    {
                                                        saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                    }
                                                    saleoutdetails.SubtotalUntaxedAmount = saleoutdetails.SubtotalTransAmount - saleoutdetails.SubtotalTaxAmount;
                                                    needupdateOut = true;
                                                }
                                                if (rdb成本为0的才修复.Checked && saleoutdetails.Cost == 0)
                                                {
                                                    saleoutdetails.Cost = child.Inv_Cost;
                                                    saleoutdetails.SubtotalCostAmount = saleoutdetails.Cost * saleoutdetails.Quantity;

                                                    saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                    if (saleoutdetails.TaxRate > 0)
                                                    {
                                                        saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                    }
                                                    saleoutdetails.SubtotalUntaxedAmount = saleoutdetails.SubtotalTransAmount - saleoutdetails.SubtotalTaxAmount;
                                                    needupdateOut = true;
                                                }
                                                if (rdb小于单项成本才更新.Checked && saleoutdetails.Cost != 0)
                                                {
                                                    if (saleoutdetails.Cost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        saleoutdetails.Cost = child.Inv_Cost;
                                                        saleoutdetails.SubtotalCostAmount = saleoutdetails.Cost * saleoutdetails.Quantity;

                                                        saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                        if (saleoutdetails.TaxRate > 0)
                                                        {
                                                            saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                        }
                                                        saleoutdetails.SubtotalUntaxedAmount = saleoutdetails.SubtotalTransAmount - saleoutdetails.SubtotalTaxAmount;
                                                        needupdateOut = true;
                                                    }
                                                }
                                                if (rdb其它.Checked && saleoutdetails.Cost != 0)
                                                {
                                                    saleoutdetails.Cost = child.Inv_Cost;
                                                    saleoutdetails.SubtotalCostAmount = saleoutdetails.Cost * saleoutdetails.Quantity;

                                                    saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                    if (saleoutdetails.TaxRate > 0)
                                                    {
                                                        saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                    }
                                                    saleoutdetails.SubtotalUntaxedAmount = saleoutdetails.SubtotalTransAmount - saleoutdetails.SubtotalTaxAmount;
                                                    needupdateOut = true;
                                                }
                                            }
                                        }
                                        if (needupdateOut)
                                        {
                                            saleoutCounter++;
                                            SaleOut.TotalCost = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount);
                                            SaleOut.TotalAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalTransAmount);
                                            SaleOut.TotalQty = SaleOut.tb_SaleOutDetails.Sum(c => c.Quantity);
                                            SaleOut.TotalTaxAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalTaxAmount);
                                            SaleOut.TotalUntaxedAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.SubtotalUntaxedAmount);

                                            richTextBoxLog.AppendText($"销售出库{SaleOut.SaleOutNo}总金额：{SaleOut.TotalCost} " + "\r\n");
                                        }
                                        if (saleoutCounter > 0)
                                        {
                                            richTextBoxLog.AppendText($"更新销售出库单 {saleoutCounter} 条" + "\r\n");
                                        }


                                        #region 销售退回
                                        if (SaleOut.tb_SaleOutRes != null)
                                        {
                                            int saleoutresCounter = 0;
                                            foreach (var SaleOutRe in SaleOut.tb_SaleOutRes)
                                            {
                                                bool needupdateback = false;
                                                foreach (var SaleOutReDetail in SaleOutRe.tb_SaleOutReDetails)
                                                {
                                                    if (SaleOutReDetail.ProdDetailID == child.ProdDetailID)
                                                    {
                                                        //不改成本 
                                                        if (rdb小计总计.Checked)
                                                        {
                                                            SaleOutReDetail.SubtotalCostAmount = SaleOutReDetail.Cost * SaleOutReDetail.Quantity;
                                                            SaleOutReDetail.SubtotalTransAmount = SaleOutReDetail.TransactionPrice * SaleOutReDetail.Quantity;
                                                            if (SaleOutReDetail.TaxRate > 0)
                                                            {
                                                                SaleOutReDetail.SubtotalTaxAmount = SaleOutReDetail.SubtotalTransAmount / (1 + SaleOutReDetail.TaxRate) * SaleOutReDetail.TaxRate;
                                                            }
                                                            SaleOutReDetail.SubtotalUntaxedAmount = SaleOutReDetail.SubtotalTransAmount - SaleOutReDetail.SubtotalTaxAmount;
                                                            needupdateback = true;
                                                        }
                                                        if (rdb成本为0的才修复.Checked && SaleOutReDetail.Cost == 0)
                                                        {

                                                            SaleOutReDetail.Cost = child.Inv_Cost;
                                                            SaleOutReDetail.SubtotalCostAmount = SaleOutReDetail.Cost * SaleOutReDetail.Quantity;
                                                            SaleOutReDetail.SubtotalTransAmount = SaleOutReDetail.TransactionPrice * SaleOutReDetail.Quantity;
                                                            if (SaleOutReDetail.TaxRate > 0)
                                                            {
                                                                SaleOutReDetail.SubtotalTaxAmount = SaleOutReDetail.SubtotalTransAmount / (1 + SaleOutReDetail.TaxRate) * SaleOutReDetail.TaxRate;
                                                            }
                                                            SaleOutReDetail.SubtotalUntaxedAmount = SaleOutReDetail.SubtotalTransAmount - SaleOutReDetail.SubtotalTaxAmount;
                                                            needupdateback = true;
                                                        }
                                                        if (rdb小于单项成本才更新.Checked && SaleOutReDetail.Cost < txtUnitCost.Text.ToDecimal())
                                                        {

                                                            SaleOutReDetail.Cost = child.Inv_Cost;
                                                            SaleOutReDetail.SubtotalCostAmount = SaleOutReDetail.Cost * SaleOutReDetail.Quantity;
                                                            SaleOutReDetail.SubtotalTransAmount = SaleOutReDetail.TransactionPrice * SaleOutReDetail.Quantity;
                                                            if (SaleOutReDetail.TaxRate > 0)
                                                            {
                                                                SaleOutReDetail.SubtotalTaxAmount = SaleOutReDetail.SubtotalTransAmount / (1 + SaleOutReDetail.TaxRate) * SaleOutReDetail.TaxRate;
                                                            }
                                                            SaleOutReDetail.SubtotalUntaxedAmount = SaleOutReDetail.SubtotalTransAmount - SaleOutReDetail.SubtotalTaxAmount;
                                                            needupdateback = true;
                                                        }
                                                    }
                                                }

                                                if (needupdateback)
                                                {
                                                    saleoutresCounter++;
                                                    //SaleOutRe.TotalCost = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalCostAmount);
                                                    SaleOutRe.TotalAmount = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalTransAmount);
                                                    SaleOutRe.TotalQty = SaleOutRe.tb_SaleOutReDetails.Sum(c => c.Quantity);
                                                }


                                                if (SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails != null)
                                                {
                                                    foreach (var Refurbished in SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails)
                                                    {
                                                        if (Refurbished.ProdDetailID == child.ProdDetailID)
                                                        {
                                                            //不改成本 
                                                            if (rdb小计总计.Checked)
                                                            {
                                                                Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                            }
                                                            if (rdb成本为0的才修复.Checked && Refurbished.Cost == 0)
                                                            {
                                                                Refurbished.Cost = child.Inv_Cost;
                                                                Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                            }
                                                            if (rdb小于单项成本才更新.Checked && Refurbished.Cost < txtUnitCost.Text.ToDecimal())
                                                            {
                                                                Refurbished.Cost = child.Inv_Cost;
                                                                Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (needupdateback)
                                                {
                                                    if (!chkTestMode.Checked)
                                                    {
                                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutRe>(SaleOutRe).ExecuteCommandAsync();
                                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutReDetail>(SaleOutRe.tb_SaleOutReDetails).ExecuteCommandAsync();
                                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutReRefurbishedMaterialsDetail>(SaleOutRe.tb_SaleOutReRefurbishedMaterialsDetails).ExecuteCommandAsync();
                                                    }
                                                    richTextBoxLog.AppendText($"销售退回{SaleOutRe.ReturnNo}总金额：{SaleOutRe.tb_SaleOutReDetails.Sum(c => c.SubtotalCostAmount)} " + "\r\n");
                                                }
                                            }
                                            if (saleoutresCounter > 0)
                                            {
                                                richTextBoxLog.AppendText($"更新销售出库退库单 {saleoutresCounter} 条" + "\r\n");
                                            }
                                        }

                                        #endregion

                                        if (!chkTestMode.Checked)
                                        {
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOut>(SaleOut).ExecuteCommandAsync();
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutDetail>(SaleOut.tb_SaleOutDetails).ExecuteCommandAsync();
                                        }
                                    }

                                    #endregion
                                    if (sordercounter > 0)
                                    {
                                        richTextBoxLog.AppendText($"更新销售出库单 {sordercounter} 条" + "\r\n");
                                    }

                                    #endregion
                                    break;
                                case BizType.缴库单:
                                    #region 更新缴库单价格,和BOM类似,  要再计算缴款的成品的成本 再反向更新库存的成本 这种一般是有BOM的

                                    List<tb_FinishedGoodsInv> orders = MainForm.Instance.AppContext.Db.Queryable<tb_FinishedGoodsInv>()
                                    .InnerJoin<tb_FinishedGoodsInvDetail>((a, b) => a.FG_ID == b.FG_ID)
                                    .Includes(a => a.tb_FinishedGoodsInvDetails, b => b.tb_proddetail, c => c.tb_Inventories)
                                    .Where(a => a.tb_FinishedGoodsInvDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                                    var distinctbills = orders
                                    .GroupBy(o => o.FG_ID)
                                    .Select(g => g.First())
                                    .ToList();

                                    List<tb_FinishedGoodsInvDetail> updateFGListdetail = new List<tb_FinishedGoodsInvDetail>();

                                    foreach (var bill in distinctbills)
                                    {
                                        foreach (tb_FinishedGoodsInvDetail Detail in bill.tb_FinishedGoodsInvDetails)
                                        {
                                            if (Detail.ProdDetailID == child.ProdDetailID)
                                            {
                                                //如果存在则更新 
                                                decimal diffpirce = Math.Abs(Detail.UnitCost - child.Inv_Cost);
                                                if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.UnitCost.ToDouble()) > 10)
                                                {
                                                    if (rdb成本为0的才修复.Checked && Detail.MaterialCost == 0)
                                                    {
                                                        Detail.MaterialCost = child.Inv_Cost;
                                                        Detail.UnitCost = Detail.MaterialCost * Detail.ManuFee + Detail.ApportionedCost;
                                                        Detail.ProductionAllCost = Detail.UnitCost * Detail.Qty;
                                                        //这时可以算出缴库的产品的单位成本
                                                        var nextInv = Detail.tb_proddetail.tb_Inventories.FirstOrDefault(c => c.Location_ID == Detail.Location_ID);
                                                        if (nextInv != null)
                                                        {
                                                            nextInv.Inv_Cost = Detail.UnitCost;
                                                            if (!chkTestMode.Checked)
                                                            {
                                                                await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(nextInv).ExecuteCommandAsync();
                                                            }
                                                        }

                                                        updateFGListdetail.Add(Detail);
                                                    }
                                                    if (rdb小于单项成本才更新.Checked && Detail.UnitCost != 0)
                                                    {
                                                        if (Detail.UnitCost < txtUnitCost.Text.ToDecimal())
                                                        {
                                                            Detail.MaterialCost = child.Inv_Cost;
                                                            Detail.UnitCost = Detail.MaterialCost * Detail.ManuFee + Detail.ApportionedCost;
                                                            Detail.ProductionAllCost = Detail.UnitCost * Detail.Qty;
                                                            //这时可以算出缴库的产品的单位成本
                                                            var nextInv = Detail.tb_proddetail.tb_Inventories.FirstOrDefault(c => c.Location_ID == Detail.Location_ID);
                                                            if (nextInv != null)
                                                            {
                                                                nextInv.Inv_Cost = Detail.UnitCost;
                                                                if (!chkTestMode.Checked)
                                                                {
                                                                    await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(nextInv).ExecuteCommandAsync();
                                                                }
                                                            }

                                                            updateFGListdetail.Add(Detail);
                                                        }
                                                    }
                                                    if (rdb其它.Checked && Detail.UnitCost != 0)
                                                    {
                                                        Detail.MaterialCost = child.Inv_Cost;
                                                        Detail.UnitCost = Detail.MaterialCost * Detail.ManuFee + Detail.ApportionedCost;
                                                        Detail.ProductionAllCost = Detail.UnitCost * Detail.Qty;
                                                        //这时可以算出缴库的产品的单位成本
                                                        var nextInv = Detail.tb_proddetail.tb_Inventories.FirstOrDefault(c => c.Location_ID == Detail.Location_ID);
                                                        if (nextInv != null)
                                                        {
                                                            nextInv.Inv_Cost = Detail.UnitCost;
                                                            if (!chkTestMode.Checked)
                                                            {
                                                                await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(nextInv).ExecuteCommandAsync();
                                                            }
                                                        }

                                                        updateFGListdetail.Add(Detail);
                                                    }

                                                }
                                            }
                                        }

                                        bill.TotalMaterialCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.MaterialCost * c.Qty);
                                        bill.TotalManuFee = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ManuFee * c.Qty);
                                        bill.TotalApportionedCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ApportionedCost * c.Qty);
                                        bill.TotalProductionCost = bill.tb_FinishedGoodsInvDetails.Sum(c => c.ProductionAllCost);
                                        //又进入下一轮更新了
                                        if (!chkTestMode.Checked && updateFGListdetail.Count > 0)
                                        {
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_FinishedGoodsInv>(bill).ExecuteCommandAsync();
                                        }
                                    }
                                    if (updateFGListdetail.Count > 0)
                                    {
                                        if (!chkTestMode.Checked)
                                        {
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_FinishedGoodsInvDetail>(updateFGListdetail).ExecuteCommandAsync();
                                        }
                                        richTextBoxLog.AppendText($"更新缴库单 {updateFGListdetail.Count} 条" + "\r\n");
                                    }


                                    #endregion

                                    break;
                                case BizType.借出单:
                                    #region 借出单 归还

                                    List<tb_ProdBorrowing> Brorders = MainForm.Instance.AppContext.Db.Queryable<tb_ProdBorrowing>()
                                    .InnerJoin<tb_ProdBorrowingDetail>((a, b) => a.BorrowID == b.BorrowID)
                                   .Includes(a => a.tb_ProdBorrowingDetails)
                                   .Includes(a => a.tb_ProdReturnings, c => c.tb_ProdReturningDetails)
                                   .Where(a => a.tb_ProdBorrowingDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                                    var distinctBRbills = Brorders
                                    .GroupBy(o => o.BorrowID)
                                    .Select(g => g.First())
                                    .ToList();
                                    List<tb_ProdBorrowingDetail> updateBRListdetail = new List<tb_ProdBorrowingDetail>();
                                    List<tb_ProdBorrowing> updateListMain = new List<tb_ProdBorrowing>();
                                    foreach (var bill in distinctBRbills)
                                    {
                                        bool needupdate = false;
                                        foreach (var Detail in bill.tb_ProdBorrowingDetails)
                                        {
                                            if (Detail.ProdDetailID == child.ProdDetailID)
                                            {
                                                //如果存在则更新 
                                                decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                                if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                                {
                                                    if (rdb成本为0的才修复.Checked && Detail.Cost == 0)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateBRListdetail.Add(Detail);
                                                        needupdate = true;
                                                    }

                                                    if (rdb小于单项成本才更新.Checked && Detail.Cost != 0)
                                                    {
                                                        if (Detail.Cost < txtUnitCost.Text.ToDecimal())
                                                        {
                                                            Detail.Cost = child.Inv_Cost;
                                                            Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                            updateBRListdetail.Add(Detail);
                                                            needupdate = true;
                                                        }
                                                    }
                                                    if (rdb其它.Checked && Detail.Cost != 0)
                                                    {

                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateBRListdetail.Add(Detail);
                                                        needupdate = true;
                                                    }
                                                    if (rdb小计总计.Checked && Detail.Cost != 0)
                                                    {
                                                        //Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateBRListdetail.Add(Detail);
                                                        needupdate = true;
                                                    }
                                                }
                                            }
                                        }

                                        if (needupdate)
                                        {
                                            bill.TotalCost = bill.tb_ProdBorrowingDetails.Sum(c => c.SubtotalCostAmount);
                                            updateListMain.Add(bill);
                                        }

                                        #region 归还单
                                        if (bill.tb_ProdReturnings != null)
                                        {
                                            foreach (var borrow in bill.tb_ProdReturnings)
                                            {
                                                foreach (var returning in borrow.tb_ProdReturningDetails)
                                                {
                                                    if (returning.ProdDetailID == child.ProdDetailID)
                                                    {
                                                        if (rdb成本为0的才修复.Checked && returning.Cost == 0)
                                                        {
                                                            returning.Cost = child.Inv_Cost;
                                                            returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                        }
                                                        if (rdb小于单项成本才更新.Checked && returning.Cost != 0)
                                                        {
                                                            if (returning.Cost < txtUnitCost.Text.ToDecimal())
                                                            {
                                                                returning.Cost = child.Inv_Cost;
                                                                returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                            }
                                                        }
                                                        if (rdb其它.Checked && returning.Cost != 0)
                                                        {
                                                            {
                                                                returning.Cost = child.Inv_Cost;
                                                                returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                            }
                                                        }
                                                    }
                                                    borrow.TotalCost = borrow.tb_ProdReturningDetails.Sum(c => c.SubtotalCostAmount);

                                                    if (!chkTestMode.Checked)
                                                    {
                                                        await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowing>(borrow).ExecuteCommandAsync();
                                                        await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowingDetail>(borrow.tb_ProdReturningDetails).ExecuteCommandAsync();
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        if (updateBRListdetail.Count > 0)
                                        {
                                            richTextBoxLog.AppendText($"更新借出单 {updateBRListdetail.Count} 条" + "\r\n");
                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowing>(updateListMain).ExecuteCommandAsync();
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_ProdBorrowingDetail>(updateBRListdetail).ExecuteCommandAsync();
                                            }
                                        }
                                        

                                    }
                                    #endregion
                                    break;
                                case BizType.其他出库单:
                                    #region 其它出库

                                    List<tb_StockOut> StockOutorders = MainForm.Instance.AppContext.Db.Queryable<tb_StockOut>()
                                        .InnerJoin<tb_StockOutDetail>((a, b) => a.MainID == b.MainID)
                                        .Includes(a => a.tb_StockOutDetails)
                                        .Where(a => a.tb_StockOutDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();

                                    var distinctStockOutbills = StockOutorders
                                    .GroupBy(o => o.MainID)
                                    .Select(g => g.First())
                                    .ToList();

                                    foreach (var bill in distinctStockOutbills)
                                    {
                                        List<tb_StockOutDetail> updateStockOutListdetail = new List<tb_StockOutDetail>();
                                        foreach (tb_StockOutDetail Detail in bill.tb_StockOutDetails)
                                        {
                                            if (Detail.ProdDetailID == child.ProdDetailID)
                                            {
                                                //如果存在则更新 
                                                decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                                if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                                {
                                                    if (rdb成本为0的才修复.Checked && Detail.Cost == 0)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateStockOutListdetail.Add(Detail);
                                                    }
                                                    if (rdb小于单项成本才更新.Checked && Detail.Cost != 0)
                                                    {
                                                        if (Detail.Cost < txtUnitCost.Text.ToDecimal())
                                                        {
                                                            Detail.Cost = child.Inv_Cost;
                                                            Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                            updateStockOutListdetail.Add(Detail);
                                                        }
                                                    }
                                                    if (rdb其它.Checked && Detail.Cost != 0)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateStockOutListdetail.Add(Detail);
                                                    }
                                                    if (rdb小计总计.Checked && Detail.Cost != 0)
                                                    {
                                                        //Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateStockOutListdetail.Add(Detail);
                                                    }
                                                }
                                            }
                                        }

                                        bill.TotalCost = bill.tb_StockOutDetails.Sum(c => c.SubtotalCostAmount);
                                        if (updateStockOutListdetail.Count > 0)
                                        {
                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_StockOutDetail>(updateStockOutListdetail).ExecuteCommandAsync();
                                            }

                                            richTextBoxLog.AppendText($"更新其它出库单 {updateStockOutListdetail.Count} 条" + "\r\n");

                                        }

                                        if (!chkTestMode.Checked)
                                        {
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_StockOut>(bill).ExecuteCommandAsync();
                                        }

                                    }

                                    #endregion
                                    break;
                                case BizType.生产领料单:
                                    #region 领料单


                                    List<tb_MaterialRequisition> MRorders = MainForm.Instance.AppContext.Db.Queryable<tb_MaterialRequisition>()
                                  .InnerJoin<tb_MaterialRequisitionDetail>((a, b) => a.MR_ID == b.MR_ID)
                                  .Includes(a => a.tb_MaterialRequisitionDetails)
                                  .Where(a => a.tb_MaterialRequisitionDetails.Any(c => c.ProdDetailID == child.ProdDetailID)).ToList();
                                    var distinctMRbills = MRorders
                                    .GroupBy(o => o.MR_ID)
                                    .Select(g => g.First())
                                    .ToList();

                                    List<tb_MaterialRequisitionDetail> updateListdetail = new List<tb_MaterialRequisitionDetail>();
                                    foreach (var bill in distinctMRbills)
                                    {
                                        foreach (tb_MaterialRequisitionDetail Detail in bill.tb_MaterialRequisitionDetails)
                                        {
                                            if (Detail.ProdDetailID == child.ProdDetailID)
                                            {
                                                //如果存在则更新 
                                                decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                                if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                                {
                                                    if (rdb成本为0的才修复.Checked && Detail.Cost == 0)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                        updateListdetail.Add(Detail);
                                                    }
                                                    if (rdb小于单项成本才更新.Checked && Detail.Cost != 0)
                                                    {
                                                        if (Detail.Cost < txtUnitCost.Text.ToDecimal())
                                                        {
                                                            Detail.Cost = child.Inv_Cost;
                                                            Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                            updateListdetail.Add(Detail);
                                                        }
                                                    }
                                                    if (rdb其它.Checked && Detail.Cost != 0)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                        updateListdetail.Add(Detail);
                                                    }
                                                    if (rdb小计总计.Checked && Detail.Cost != 0)
                                                    {
                                                        Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                        updateListdetail.Add(Detail);
                                                    }
                                                }
                                            }
                                        }
                                        if (updateListdetail.Count > 0)
                                        {
                                            bill.TotalCost = bill.tb_MaterialRequisitionDetails.Sum(c => c.SubtotalCost);
                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_MaterialRequisition>(bill).ExecuteCommandAsync();
                                            }
                                        }
                                    }

                                    if (updateListdetail.Count > 0)
                                    {
                                        if (!chkTestMode.Checked)
                                        {
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_MaterialRequisitionDetail>(updateListdetail).ExecuteCommandAsync();
                                        }
                                        richTextBoxLog.AppendText($"更新生产领料单 {updateListdetail.Count} 条" + "\r\n");
                                    }

                                    #endregion

                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    #endregion
                }

                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.CommitTran();
                }
            }
            catch (Exception ex)
            {
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.RollbackTran();
                }
            }
        }

        private async void 更新为当前成本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem toolStripMenu)
            {
                if (tabControl.SelectedTab != null)
                {
                    List<Control> controls = tabControl.SelectedTab.Controls.CastToList<Control>();
                    DataGridView dgv = controls.FirstOrDefault(c => c.GetType().Name == "DataGridView") as DataGridView;
                    if (dgv != null)
                    {
                        dgv.ContextMenuStrip = null;
                        // dgv.ContextMenuStrip = contextMenuStripCmd;
                        dgv.Dock = DockStyle.Fill;
                        if (!chkTestMode.Checked)
                        {
                            MainForm.Instance.AppContext.Db.Ado.BeginTran();
                        }
                        if (dgv.Tag is BizType bt)
                        {
                            switch (bt)
                            {
                                case BizType.销售出库单:
                                    List<string> strings = new List<string>();
                                    foreach (DataGridViewRow dr in dgv.SelectedRows)
                                    {
                                        string saleoutNo = dr.Cells[0].Value.ToString();
                                        if (!strings.Contains(saleoutNo))
                                        {
                                            strings.Add(saleoutNo);
                                        }
                                    }

                                    List<tb_SaleOut> orders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                                        .Includes(a => a.tb_SaleOutDetails)
                                        .Includes(c => c.tb_SaleOutRes, d => d.tb_SaleOutReDetails)
                                        .Where(a => strings.Contains(a.SaleOutNo)).ToList();

                                    if (!chkTestMode.Checked)
                                    {
                                        foreach (var order in orders)
                                        {
                                            foreach (var item in order.tb_SaleOutDetails)
                                            {
                                                item.SubtotalCostAmount = item.Cost * item.Quantity;
                                            }
                                            order.TotalCost = order.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount);
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutDetail>(order.tb_SaleOutDetails).ExecuteCommandAsync();
                                        }
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOut>(orders).ExecuteCommandAsync();
                                    }
                                    break;
                            }

                        }

                        if (!chkTestMode.Checked)
                        {
                            MainForm.Instance.AppContext.Db.Ado.CommitTran();
                        }
                    }

                }
            }
        }

        private async void toolStripMenuItem修复小计总计_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem toolStripMenu)
            {
                if (tabControl.SelectedTab != null)
                {
                    List<Control> controls = tabControl.SelectedTab.Controls.CastToList<Control>();
                    DataGridView dgv = controls.FirstOrDefault(c => c.GetType().Name == "DataGridView") as DataGridView;
                    if (dgv != null)
                    {
                        dgv.ContextMenuStrip = null;
                        // dgv.ContextMenuStrip = contextMenuStripCmd;
                        dgv.Dock = DockStyle.Fill;
                        if (!chkTestMode.Checked)
                        {
                            MainForm.Instance.AppContext.Db.Ado.BeginTran();
                        }
                        if (dgv.Tag is BizType bt)
                        {
                            switch (bt)
                            {
                                case BizType.销售出库单:
                                    List<string> strings = new List<string>();
                                    foreach (DataGridViewRow dr in dgv.SelectedRows)
                                    {
                                        string saleoutNo = dr.Cells[0].Value.ToString();
                                        if (!strings.Contains(saleoutNo))
                                        {
                                            strings.Add(saleoutNo);
                                        }
                                    }

                                    List<tb_SaleOut> orders = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                                        .Includes(a => a.tb_SaleOutDetails)
                                        .Includes(c => c.tb_SaleOutRes, d => d.tb_SaleOutReDetails)
                                        .Where(a => strings.Contains(a.SaleOutNo)).ToList();

                                    if (!chkTestMode.Checked)
                                    {
                                        foreach (var order in orders)
                                        {
                                            foreach (var item in order.tb_SaleOutDetails)
                                            {
                                                item.SubtotalCostAmount = item.Cost * item.Quantity;
                                            }
                                            order.TotalCost = order.tb_SaleOutDetails.Sum(c => c.SubtotalCostAmount);
                                            await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutDetail>(order.tb_SaleOutDetails).ExecuteCommandAsync();
                                        }
                                        await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOut>(orders).ExecuteCommandAsync();
                                    }
                                    break;
                            }

                        }

                        if (!chkTestMode.Checked)
                        {
                            MainForm.Instance.AppContext.Db.Ado.CommitTran();
                        }
                    }

                }
            }
        }

        private void 更新关联成本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewInv.SelectedRows != null)
            {
                List<tb_Inventory> inventories = new List<tb_Inventory>();

                foreach (DataGridViewRow dr in dataGridViewInv.SelectedRows)
                {
                    if (dr.DataBoundItem is View_Inventory inventory)
                    {
                        inventories.Add(inventory.tb_inventory);
                    }
                }
                UpdateRelatedCost(inventories);
            }
        }

        private async void 更新库存成本数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewInv.SelectedRows != null)
            {
                List<tb_Inventory> inventories = new List<tb_Inventory>();

                foreach (DataGridViewRow dr in dataGridViewInv.SelectedRows)
                {
                    if (dr.DataBoundItem is View_Inventory inventory)
                    {
                        inventories.Add(inventory.tb_inventory);
                    }
                }
                await UpdateInventoryCost(inventories);
            }

        }

        private void 修复关联小计总计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //以选中的库存产品为根据 将他名下的 小计总计修复
            if (dataGridViewInv.SelectedRows != null)
            {
                List<tb_Inventory> inventories = new List<tb_Inventory>();

                foreach (DataGridViewRow dr in dataGridViewInv.SelectedRows)
                {
                    if (dr.DataBoundItem is View_Inventory inventory)
                    {
                        inventories.Add(inventory.tb_inventory);
                    }
                }
                UpdateRelatedCost(inventories);
            }
        }

        private void kryptonLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
