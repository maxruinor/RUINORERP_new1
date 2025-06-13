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

using RUINORERP.UI.PSI.SAL;
using RUINORERP.UI.UserCenter.DataParts;
using Fireasy.Common.Extensions;
using RUINORERP.UI.UControls;
using RUINORERP.Common.CollectionExtension;
using System.Diagnostics;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.ATechnologyStack;
using Winista.Text.HtmlParser.Tags;
using Org.BouncyCastle.Crypto;
using NPOI.POIFS.Properties;
using RUINORERP.UI.AdvancedUIModule;
using FastReport.DevComponents.WinForms.Drawing;


namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("成本数据校正", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataFixForCost : UserControl
    {
        public UCDataFixForCost()
        {
            InitializeComponent();
        }

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    //case Keys.Escape:
                    //    //Exit(this);//csc关闭窗体
                    //    break;
                    case Keys.Enter:
                        QueryInv();
                        break;
                }

            }
            //return false;
            var key = keyData & Keys.KeyCode;
            var modeifierKey = keyData & Keys.Modifiers;
            if (modeifierKey == Keys.Control && key == Keys.F)
            {
                // MessageBox.Show("Control+F is pressed");
                return true;

            }

            var otherkey = keyData & Keys.KeyCode;
            var othermodeifierKey = keyData & Keys.Modifiers;
            if (othermodeifierKey == Keys.Control && otherkey == Keys.F)
            {
                MessageBox.Show("Control+F is pressed");
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }

        private void UCDataFix_Load(object sender, EventArgs e)
        {
            dataGridViewInv.RowHeadersVisible = false;
            ucAdvDateTimerPickerGroup1.Visible = false;
            ucAdvDateTimerPickerGroup1.dtp1.ShowCheckBox = false;
            ucAdvDateTimerPickerGroup1.dtp2.ShowCheckBox = false;
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
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbdepartment);
        }

        View_Inventory InventoryDto = new View_Inventory();

        BizTypeMapper mapper = new BizTypeMapper();




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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (chk有入库记录成本为0.Checked)
            {
                QueryInvNeedUpdate();
            }
            else
            {
                QueryInv();
            }

        }


        private async void QueryInvNeedUpdate()
        {
            List<View_Inventory> inventories = new List<View_Inventory>();
            //inventories = await MainForm.Instance.AppContext.Db.Queryable<View_Inventory>()
            //              .Includes(a => a.tb_proddetail, b => b.tb_PurEntryDetails)
            //            .Where(a => a.Inv_Cost == 0)
            //            .Where(a => a.tb_proddetail.tb_PurEntryDetails.Count > 0)
            //              .ToListAsync();


            inventories = await MainForm.Instance.AppContext.Db.Queryable<View_Inventory>()
               .Includes(a => a.tb_proddetail, b => b.tb_PurEntryDetails)
               .Where(a => a.Inv_Cost == 0)
               .Where(a => a.tb_proddetail.tb_PurEntryDetails.Any())
               .ToListAsync();


            bindingSourceInv.DataSource = inventories.ToBindingSortCollection();
            dataGridViewInv.DataSource = bindingSourceInv;

            //只显示成本 和详情ID。
            foreach (DataGridViewColumn item in dataGridViewInv.Columns)
            {
                if (item.Name == "Inv_Cost" || item.Name == "ProdDetailID" || item.Name == "Quantity")
                {
                    item.Visible = true;
                    if (item.Name == "Inv_Cost")
                    {
                        item.HeaderText = "成本";
                    }
                    if (item.Name == "ProdDetailID")
                    {
                        item.HeaderText = "产品详情";
                        item.Width = 250;
                    }
                    if (item.Name == "Quantity")
                    {
                        item.HeaderText = "实际数量";
                    }
                }
                else
                {
                    item.Visible = false;
                }
            }
            richTextBoxLog.AppendText($"查询结果{inventories.Count} " + "\r\n");
        }


        private async void QueryInv()
        {
            List<View_Inventory> inventories = new List<View_Inventory>();
            inventories = await MainForm.Instance.AppContext.Db.Queryable<View_Inventory>()
                          .Includes(a => a.tb_inventory)
                          .WhereIF(!string.IsNullOrEmpty(txtCNName.Text), c => c.CNName.Contains(txtCNName.Text))
                          .WhereIF(!string.IsNullOrEmpty(txtProp.Text), c => c.prop.Contains(txtProp.Text))
                          .WhereIF(!string.IsNullOrEmpty(txtSearchKey.Text), c => c.SKU == txtSearchKey.Text)
                          .WhereIF((cmbdepartment.SelectedItem != null && cmbdepartment.SelectedItem is tb_Department), c => c.DepartmentID == (cmbdepartment.SelectedItem as tb_Department).DepartmentID)
                          .WhereIF((InventoryDto.Type_ID != null && InventoryDto.Type_ID != -1), c => c.Type_ID == InventoryDto.Type_ID)
                          .ToListAsync();
            bindingSourceInv.DataSource = inventories.ToBindingSortCollection();
            dataGridViewInv.DataSource = bindingSourceInv;
            dataGridViewInv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //只显示成本 和详情ID。
            foreach (DataGridViewColumn item in dataGridViewInv.Columns)
            {
                if (item.Name == "Inv_Cost" ||
                    item.Name == "ProdDetailID"
                    || item.Name == "Quantity"
                      || item.Name == "SKU"
                    || item.Name == "Notes")
                {
                    item.Visible = true;
                    if (item.Name == "Inv_Cost")
                    {
                        item.HeaderText = "成本";
                        item.Width = 60;
                    }
                    if (item.Name == "ProdDetailID")
                    {
                        item.HeaderText = "产品";
                        item.Width = 250;
                    }
                    if (item.Name == "Quantity")
                    {
                        item.HeaderText = "数量";
                        item.Width = 60;
                    }
                    if (item.Name == "SKU")
                    {
                        item.HeaderText = "SKU";
                        item.Width = 100;
                    }
                    if (item.Name == "Notes")
                    {
                        item.HeaderText = "备注";
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
                e.Value = prodDetail.CNName + prodDetail.prop + prodDetail.Specifications;
            }
        }

        private void dataGridViewInv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            AddLoadRelatedData();
        }

        private void AddLoadRelatedData()
        {
            if (dataGridViewInv.CurrentRow == null)
            {
                MessageBox.Show("请先选中要处理的库存数据");
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
                            成本 = it.UnitPrice,
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


                            #region 销售订单 出库  退货 记录成本修复

                            List<dynamic> SaleOrderItems = MainForm.Instance.AppContext.Db.Queryable<View_SaleOrderItems>()
                            .OrderBy(a => a.SaleDate)
                            .Where(a => a.ProdDetailID == ProdDetailID)
                            .Select(it => (dynamic)new
                            {
                                销售订单号 = it.SOrderNo,
                                订单日期 = it.SaleDate,
                                sku = it.SKU,
                                成本 = it.Cost,
                                成本小计 = it.SubtotalCostAmount,
                                数量 = it.Quantity
                            }).ToList();

                            if (SaleOrderItems.Count > 0)
                            {
                                TabPage page销售订单 = new TabPage();
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
                                page销售订单.Tag = bt;
                                dgv销售订单.Tag = bt;

                                page销售订单.Controls.Add(dgv销售订单);
                                tabControl.TabPages.Add(page销售订单);
                                dgv销售订单.Refresh();
                            }


                            break;
                        case BizType.销售出库单:

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
                            if (SaleOutItems.Count > 0)
                            {
                                TabPage page销售出库单 = new TabPage();
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
                                page销售出库单.Tag = bt;
                                dgv销售出库.Tag = bt;
                                page销售出库单.Controls.Add(dgv销售出库);
                                tabControl.TabPages.Add(page销售出库单);
                            }

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

                            if (list其它出库.Count > 0)
                            {
                                TabPage page其他出库单 = new TabPage();
                                DataGridView dgv其他出库单 = new DataGridView();
                                dgv其他出库单.DataSource = list其它出库.ToDataTable();
                                // 自动调整列宽
                                dgv其他出库单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgv其他出库单.AutoResizeRows();
                                dgv其他出库单.Dock = DockStyle.Fill;
                                dgv其他出库单.Tag = bt;


                                page其他出库单.Text = "其他出库单" + list其它出库.Count;
                                page其他出库单.Tag = bt;
                                dgv其他出库单.Tag = bt;
                                page其他出库单.Controls.Add(dgv其他出库单);
                                tabControl.TabPages.Add(page其他出库单);
                            }

                            #endregion
                            break;
                        case BizType.盘点单:
                            break;
                        case BizType.制令单:
                            #region 制令单

                            List<dynamic> MOItems = MainForm.Instance.AppContext.Db.Queryable<View_ManufacturingOrderItems>()
                       //.OrderBy(a => a.)
                       .Where(a => a.ProdDetailID == ProdDetailID)
                       .Select(it => (dynamic)new
                       {
                           数量 = it.ActualSentQty,
                           配方号 = it.BOM_NO,
                           成本 = it.UnitCost,
                           成本小计 = it.SubtotalUnitCost
                       }).ToList();
                            if (MOItems.Count > 0)
                            {
                                TabPage page制令单 = new TabPage();
                                DataGridView dgv制令单 = new DataGridView();
                                dgv制令单.DataSource = MOItems.ToDataTable();
                                dgv制令单.Dock = DockStyle.Fill;
                                // 自动调整列宽
                                dgv制令单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgv制令单.AutoResizeRows();
                                dgv制令单.Tag = bt;

                                page制令单.Text = "制令单" + MOItems.Count;
                                page制令单.Tag = bt;
                                dgv制令单.Tag = bt;
                                page制令单.Controls.Add(dgv制令单);
                                tabControl.TabPages.Add(page制令单);
                            }
                            #endregion

                            break;
                        case BizType.BOM物料清单:

                            #region 配方清单明细
                            TabPage pageBOMDetail = new TabPage();

                            List<dynamic> ProdbomItems = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_SDetail>()
                       //.Includes(a => a.tb_bom_s)
                       .OrderBy(a => a.SKU)
                       .Where(a => a.ProdDetailID == ProdDetailID)
                       .Select(it => (dynamic)new
                       {
                           BOMID = it.BOM_ID,
                           // 所属配方号 = it.tb_bom_s.BOM_No,
                           sku = it.SKU,
                           成本 = it.UnitCost,
                           成本小计 = it.SubtotalUnitCost,
                           数量 = it.UsedQty
                       })
                       .ToList();

                            if (ProdbomItems.Count > 0)
                            {
                                DataGridView dgvBOMDetail = new DataGridView();
                                dgvBOMDetail.DataSource = ProdbomItems.ToDataTable();
                                dgvBOMDetail.Dock = DockStyle.Fill;
                                dgvBOMDetail.Tag = bt;
                                // 自动调整列宽
                                dgvBOMDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgvBOMDetail.AutoResizeRows();


                                pageBOMDetail.Text = "配方清单明细" + ProdbomItems.Count;
                                pageBOMDetail.Tag = bt;
                                dgvBOMDetail.Tag = bt;
                                pageBOMDetail.Controls.Add(dgvBOMDetail);
                                tabControl.TabPages.Add(pageBOMDetail);
                            }


                            #endregion

                            #region

                            #region 配方清单

                            List<dynamic> boms = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                       //.Includes(a => a.tb_bom_s)
                       .OrderBy(a => a.SKU)
                       .Where(a => a.ProdDetailID == ProdDetailID)
                       .Select(it => (dynamic)new
                       {
                           配方号 = it.BOM_No,
                           母件SKU = it.SKU,
                           配方名称 = it.BOM_Name,
                           材料成本 = it.TotalMaterialCost,
                           外发费用 = it.TotalOutManuCost,
                           自制费用 = it.TotalSelfManuCost,
                           自制分摊费 = it.SelfApportionedCost,
                           外发分摊费 = it.OutApportionedCost,
                           自产总成本 = it.SelfProductionAllCosts,
                           外发总成本 = it.OutProductionAllCosts,
                       })
                       .ToList();
                            if (boms.Count > 0)
                            {
                                TabPage pageBOM = new TabPage();
                                DataGridView dgvBOM = new DataGridView();
                                dgvBOM.DataSource = boms.ToDataTable();
                                dgvBOM.Dock = DockStyle.Fill;
                                dgvBOM.Tag = bt;
                                // 自动调整列宽
                                dgvBOM.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgvBOM.AutoResizeRows();
                                pageBOM.Text = "对应配方" + boms.Count;
                                pageBOM.Tag = bt;
                                dgvBOM.Tag = bt;

                                pageBOM.Controls.Add(dgvBOM);
                                tabControl.TabPages.Add(pageBOM);
                                dgvBOM.ContextMenuStrip = this.contextMenuStripBOMPrice;

                                #endregion

                            }

                            #endregion

                            break;
                        case BizType.生产领料单:


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

                            if (MRBills.Count > 0)
                            {
                                TabPage page生产领料单 = new TabPage();
                                DataGridView dgv生产领料单 = new DataGridView();
                                dgv生产领料单.DataSource = MRBills.ToDataTable();
                                dgv生产领料单.Dock = DockStyle.Fill;
                                dgv生产领料单.Tag = bt;
                                // 自动调整列宽
                                dgv生产领料单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgv生产领料单.AutoResizeRows();


                                page生产领料单.Text = "生产领料单" + MRBills.Count;
                                page生产领料单.Tag = bt;
                                dgv生产领料单.Tag = bt;

                                page生产领料单.Controls.Add(dgv生产领料单);
                                tabControl.TabPages.Add(page生产领料单);
                            }

                            #endregion
                            break;
                        case BizType.生产退料单:
                            break;
                        case BizType.生产补料单:
                            break;
                        case BizType.发料计划单:
                            break;
                        case BizType.退料单:
                            break;
                        case BizType.缴库单:

                            #region 缴库单

                            List<dynamic> ProdFinishedItems = MainForm.Instance.AppContext.Db.Queryable<View_FinishedGoodsInvItems>()
                       //.OrderBy(a => a.)
                       .Where(a => a.ProdDetailID == ProdDetailID)
                       .Select(it => (dynamic)new
                       {
                           数量 = it.Qty,
                           缴库单号 = it.DeliveryBillNo,
                           缴库日期 = it.DeliveryDate,
                           材料成本 = it.MaterialCost,
                           分摊成本 = it.ApportionedCost,
                           制造费 = it.ManuFee,
                           单位成本 = it.UnitCost,
                           成本小计 = it.ProductionAllCost
                       }).ToList();
                            if (ProdFinishedItems.Count > 0)
                            {
                                TabPage page缴库单 = new TabPage();
                                DataGridView dgv缴库单 = new DataGridView();
                                dgv缴库单.DataSource = ProdFinishedItems.ToDataTable();
                                dgv缴库单.Dock = DockStyle.Fill;
                                // 自动调整列宽
                                dgv缴库单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgv缴库单.AutoResizeRows();
                                dgv缴库单.Tag = bt;

                                page缴库单.Text = "缴库单" + ProdFinishedItems.Count;
                                page缴库单.Tag = bt;
                                dgv缴库单.Tag = bt;

                                page缴库单.Controls.Add(dgv缴库单);
                                tabControl.TabPages.Add(page缴库单);
                            }
                            #endregion
                            break;
                        case BizType.请购单:
                            break;
                        case BizType.产品分割单:
                            break;
                        case BizType.产品组合单:
                            #region 产品组合单生成的增加的

                            List<dynamic> ProdMerge = MainForm.Instance.AppContext.Db
                                .Queryable<tb_ProdMerge>()
                       //.OrderBy(a => a.)
                       .Where(a => a.ProdDetailID == ProdDetailID)
                       .Select(it => (dynamic)new
                       {
                           产出数量 = it.MergeTargetQty,
                           组合单号 = it.MergeNo,
                           组合日期 = it.MergeDate,

                       }).ToList();
                            if (ProdMerge.Count > 0)
                            {
                                TabPage page产品组合单 = new TabPage();
                                DataGridView dgv产品组合单 = new DataGridView();
                                dgv产品组合单.DataSource = ProdMerge.ToDataTable();
                                dgv产品组合单.Dock = DockStyle.Fill;
                                // 自动调整列宽
                                dgv产品组合单.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgv产品组合单.AutoResizeRows();
                                dgv产品组合单.Tag = bt;

                                page产品组合单.Text = "产品组合单母件" + ProdMerge.Count;
                                page产品组合单.Tag = bt;
                                dgv产品组合单.Tag = bt;

                                page产品组合单.Controls.Add(dgv产品组合单);
                                tabControl.TabPages.Add(page产品组合单);
                            }
                            #endregion

                            #region 产品组合单消耗的减少的

                            List<dynamic> ProdMergeItems = MainForm.Instance.AppContext.Db
                                .Queryable<View_ProdMergeItems>()
                       //.OrderBy(a => a.)
                       .Where(a => a.ProdDetailID == ProdDetailID)
                       .Select(it => (dynamic)new
                       {
                           子件数量 = it.Qty,
                           组合日期 = it.MergeDate


                       }).ToList();
                            if (ProdMergeItems.Count > 0)
                            {
                                TabPage page产品组合单生成件 = new TabPage();
                                DataGridView dgv产品组合单生成件 = new DataGridView();
                                dgv产品组合单生成件.DataSource = ProdMergeItems.ToDataTable();
                                dgv产品组合单生成件.Dock = DockStyle.Fill;
                                // 自动调整列宽
                                dgv产品组合单生成件.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgv产品组合单生成件.AutoResizeRows();
                                dgv产品组合单生成件.Tag = bt;

                                page产品组合单生成件.Text = "产品组合单生成件" + ProdMergeItems.Count;
                                page产品组合单生成件.Tag = bt;
                                dgv产品组合单生成件.Tag = bt;

                                page产品组合单生成件.Controls.Add(dgv产品组合单生成件);
                                tabControl.TabPages.Add(page产品组合单生成件);
                            }
                            #endregion

                            break;
                        case BizType.借出单:

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

                            if (ProdBorrowingItems.Count > 0)
                            {
                                TabPage page借出单 = new TabPage();
                                DataGridView dgvBorrow = new DataGridView();
                                dgvBorrow.Tag =
                                dgvBorrow.AutoGenerateColumns = true;
                                dgvBorrow.DataSource = ProdBorrowingItems.ToDataTable();
                                dgvBorrow.Dock = DockStyle.Fill;
                                dgvBorrow.Tag = bt;
                                // 自动调整列宽
                                dgvBorrow.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                dgvBorrow.AutoResizeRows();

                                page借出单.Text = "借出单" + ProdBorrowingItems.Count;
                                page借出单.Tag = bt;
                                dgvBorrow.Tag = bt;


                                page借出单.Controls.Add(dgvBorrow);
                                tabControl.TabPages.Add(page借出单);
                            }
                            #endregion
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
                    dgv.AllowUserToAddRows = false;
                    dgv.ContextMenuStrip = contextMenuStripBOMPrice;
                    if (tabControl.SelectedTab.Text.Contains("对应配方"))
                    {
                        //双击母件行能将子件所有库存数据带出来+并且将他

                        //dgv.ContextMenuStrip = contextMenuStripCmd;
                    }
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
        private async Task<List<tb_Inventory>> UpdateInventoryCost
            (List<tb_Inventory> NeedUpdateInvList, decimal targetCost = 0)
        {
            List<tb_Inventory> updateInvList = new List<tb_Inventory>();
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
                long[] ids = NeedUpdateInvList.Select(c => c.Inventory_ID).ToArray();

                Allitems = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                   .AsNavQueryable()
                   .Includes(c => c.tb_proddetail, d => d.tb_PurEntryDetails, e => e.tb_proddetail, f => f.tb_prod)
                   .Includes(c => c.tb_proddetail, d => d.tb_prod)
                    .Where(c => ids.Contains(c.Inventory_ID)).ToListAsync();

                foreach (tb_Inventory item in Allitems)
                {
                    if (item.tb_proddetail.tb_PurEntryDetails.Count > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.UnitPrice) > 0
                        && item.tb_proddetail.tb_PurEntryDetails.Sum(c => c.Quantity) > 0
                        )
                    {
                        //参与成本计算的入库明细记录。要排除单价为0的项
                        var realDetails = item.tb_proddetail.tb_PurEntryDetails.Where(c => c.UnitPrice > 0).ToList();
                        //每笔的入库的数量*成交价/总数量
                        var transPrice = realDetails
                            .Where(c => c.UnitPrice > 0 && c.Quantity > 0 && c.UnitPrice > 0)
                            .Sum(c => c.UnitPrice * c.Quantity) / realDetails.Sum(c => c.Quantity);
                        if (targetCost > 0)
                        {
                            transPrice = targetCost;
                        }
                        richTextBoxLog.AppendText($"要修复:{item.tb_proddetail.SKU} ,new:{transPrice}, old: {item.Inv_Cost}\r\n");
                        item.CostMovingWA = transPrice;
                        item.Inv_AdvCost = item.CostMovingWA;
                        item.Inv_Cost = item.CostMovingWA;
                        item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;
                        item.Notes += $"{System.DateTime.Now.ToString("yyyy-MM-dd")}成本修复为：{transPrice}";
                        updateInvList.Add(item);
                    }
                    else
                    {
                        richTextBoxLog.AppendText($"要修复:{item.tb_proddetail.SKU} ,new:{targetCost}, old: {item.Inv_Cost}\r\n");
                        item.CostMovingWA = targetCost;
                        item.Inv_AdvCost = item.CostMovingWA;
                        item.Inv_Cost = item.CostMovingWA;
                        item.Inv_SubtotalCostMoney = item.Inv_Cost * item.Quantity;
                        item.Notes += $"{System.DateTime.Now.ToString("yyyy-MM-dd")}成本修复为指定值：{targetCost}";
                        updateInvList.Add(item);
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


        private int totalloop;


        /// <summary>
        /// 更新相关出库的明细中的成本
        /// </summary>
        /// <param name="updateInvList"></param>
        private async void UpdateRelatedCost(List<tb_Inventory> updateInvList, BizType SelectBizType = BizType.无对应数据, bool FixSubtotal = false)
        {

            if (!chkTestMode.Checked)
            {
                MainForm.Instance.AppContext.Db.Ado.BeginTran();
            }
            try
            {
                if (chk指定成本.Checked && updateInvList.Count > 1)
                {
                    MessageBox.Show("选择单项成本更新时,更新库存数据不能大于1");
                    return;
                }
                foreach (var child in updateInvList)
                {
                    if (chk指定成本.Checked && updateInvList.Count == 1)
                    {
                        child.Inv_Cost = txtUnitCost.Text.ToDecimal();
                    }
                    #region 更新相关数据

                    foreach (TreeNode item in treeViewNeedUpdateCostList.Nodes)
                    {
                        if (item.Checked && item.Tag is BizType bt)
                        {
                            if (SelectBizType != BizType.无对应数据)
                            {
                                //指定行
                                if (SelectBizType != bt)
                                {
                                    continue;
                                }
                            }

                            switch (bt)
                            {
                                case BizType.BOM物料清单:

                                    #region 更新BOM价格,当前产品存在哪些BOM中，则更新所有BOM的价格包含主子表数据的变化

                                    List<tb_BOM_S> BOM_SOrders = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                                    .InnerJoin<tb_BOM_SDetail>((a, b) => a.BOM_ID == b.BOM_ID)
                                    .Includes(a => a.tb_BOM_SDetails)
                                    .Where(a => a.DataStatus == (int)DataStatus.确认)
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
                                                if (rdb成本为0的才修复.Checked && bomDetail.UnitCost == 0)
                                                {
                                                    bomDetail.UnitCost = child.Inv_Cost;
                                                    bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                                    updateListbomdetail.Add(bomDetail);
                                                }
                                                if (rdb小于指定成本.Checked && bomDetail.UnitCost != 0)
                                                {
                                                    if (bomDetail.UnitCost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        bomDetail.UnitCost = child.Inv_Cost;
                                                        bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                                        updateListbomdetail.Add(bomDetail);
                                                    }
                                                }
                                                if (rdb大于单项成本.Checked && bomDetail.UnitCost != 0)
                                                {
                                                    if (bomDetail.UnitCost > txtUnitCost.Text.ToDecimal())
                                                    {
                                                        bomDetail.UnitCost = child.Inv_Cost;
                                                        bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                                        updateListbomdetail.Add(bomDetail);
                                                    }
                                                }
                                                if (rdb其它.Checked && bomDetail.UnitCost != 0)
                                                {  //如果存在则更新 
                                                    decimal diffpirce = Math.Abs(bomDetail.UnitCost - child.Inv_Cost);
                                                    if (diffpirce > 0.01m)
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

                                            totalloop = 0;
                                            LoopUpdateBom(bill);
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
                                .Includes(b => b.tb_bom_s, c => c.tb_BOM_SDetails)
                                .Includes(a => a.tb_ManufacturingOrderDetails)

                                .Where(a => a.tb_ManufacturingOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID)).ToList();


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

                                                //正常逻辑时
                                                //制令单中明细中物料成本是来自库存表中的实时成本。
                                                //母件材料小计也是实时的，制造费用 和分摊成本来自于配方表数据乘以数量。也可以手动修正。
                                                //缴库时母件成本就是这个总成本除以单位成本

                                                if (rdb成本为0的才修复.Checked && Detail.UnitCost == 0)
                                                {
                                                    Detail.UnitCost = child.Inv_Cost;
                                                    Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                                    updateMOdetail.Add(Detail);
                                                }
                                                if (rdb小于指定成本.Checked && Detail.UnitCost != 0)
                                                {
                                                    if (Detail.UnitCost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.UnitCost = child.Inv_Cost;
                                                        Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                                        updateMOdetail.Add(Detail);
                                                    }
                                                }
                                                if (rdb大于单项成本.Checked && Detail.UnitCost != 0)
                                                {
                                                    if (Detail.UnitCost > txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.UnitCost = child.Inv_Cost;
                                                        Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                                        updateMOdetail.Add(Detail);
                                                    }
                                                }
                                                if (rdb其它.Checked && Detail.UnitCost != 0)
                                                {//如果存在则更新 
                                                    decimal diffpirce = Math.Abs(Detail.UnitCost - child.Inv_Cost);
                                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.UnitCost.ToDouble()) > 10)
                                                    {
                                                        Detail.UnitCost = child.Inv_Cost;
                                                        Detail.SubtotalUnitCost = Detail.UnitCost * Detail.ShouldSendQty;
                                                        updateMOdetail.Add(Detail);
                                                    }
                                                }
                                            }
                                        }

                                        bill.TotalMaterialCost = bill.tb_ManufacturingOrderDetails.Sum(c => c.SubtotalUnitCost);

                                        if (bill.IsOutSourced)
                                        {
                                            bill.ApportionedCost = bill.tb_bom_s.OutApportionedCost * bill.ManufacturingQty;
                                            bill.TotalManuFee = bill.tb_bom_s.TotalOutManuCost * bill.ManufacturingQty;
                                        }
                                        else
                                        {
                                            bill.ApportionedCost = bill.tb_bom_s.SelfApportionedCost * bill.ManufacturingQty;
                                            bill.TotalManuFee = bill.tb_bom_s.TotalSelfManuCost * bill.ManufacturingQty;
                                        }
                                        bill.TotalProductionCost = bill.TotalMaterialCost + bill.ApportionedCost + bill.TotalManuFee;

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
                                    .WhereIF(rdb时间区间.Checked, a => a.SaleDate >= ucAdvDateTimerPickerGroup1.dtp1.Value && a.SaleDate <= ucAdvDateTimerPickerGroup1.dtp2.Value)
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
                                                    Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                                    Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                    if (Detail.TaxRate > 0)
                                                    {
                                                        Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                    }
                                                    needupdateorder = true;
                                                }

                                                if (rdb时间区间.Checked)
                                                {
                                                    Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                                    Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                    if (Detail.TaxRate > 0)
                                                    {
                                                        Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                    }
                                                    needupdateorder = true;
                                                }

                                                if (rdb成本为0的才修复.Checked && Detail.Cost == 0)
                                                {
                                                    Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                                    Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                    if (Detail.TaxRate > 0)
                                                    {
                                                        Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                    }
                                                    needupdateorder = true;
                                                }
                                                if (rdb小于指定成本.Checked && Detail.Cost != 0)
                                                {
                                                    if (Detail.Cost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                                        Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                        if (Detail.TaxRate > 0)
                                                        {
                                                            Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                        }
                                                        needupdateorder = true;
                                                    }
                                                }
                                                if (rdb大于单项成本.Checked && Detail.Cost != 0)
                                                {
                                                    if (Detail.Cost > txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                                        Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                        if (Detail.TaxRate > 0)
                                                        {
                                                            Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                        }
                                                        needupdateorder = true;
                                                    }
                                                }
                                                if (rdb小计总计.Checked && Detail.Cost != 0)
                                                {
                                                    Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                                    Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                    if (Detail.TaxRate > 0)
                                                    {
                                                        Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                    }
                                                    needupdateorder = true;
                                                }
                                                if (rdb其它.Checked && Detail.Cost != 0)
                                                {
                                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = (Detail.Cost + Detail.CustomizedCost) * Detail.Quantity;
                                                        Detail.SubtotalTransAmount = Detail.TransactionPrice * Detail.Quantity;
                                                        if (Detail.TaxRate > 0)
                                                        {
                                                            Detail.SubtotalTaxAmount = Detail.SubtotalTransAmount / (1 + Detail.TaxRate) * Detail.TaxRate;
                                                        }
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
                                            richTextBoxLog.AppendText($"销售订单{order.SOrderNo}总金额：{order.TotalCost} " + "\r\n");

                                            if (!chkTestMode.Checked)
                                            {
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrderDetail>(order.tb_SaleOrderDetails).ExecuteCommandAsync();
                                                await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOrder>(order).ExecuteCommandAsync();
                                            }
                                        }

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
                                    .WhereIF(rdb时间区间.Checked, a => a.OutDate >= ucAdvDateTimerPickerGroup1.dtp1.Value && a.OutDate <= ucAdvDateTimerPickerGroup1.dtp2.Value)
                                    .Where(a => a.tb_SaleOutDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID)).ToList();

                                    var distinctSoutOrders = SOutorders
                                    .GroupBy(o => o.SaleOut_MainID)
                                    .Select(g => g.First())
                                    .ToList();
                                    int sordercounter = 0;
                                    #region 销售出库

                                    int saleoutCounter = 0;
                                    foreach (var SaleOut in distinctSoutOrders)
                                    {

                                        bool needupdateOut = false;
                                        foreach (var saleoutdetails in SaleOut.tb_SaleOutDetails)
                                        {
                                            if (saleoutdetails.ProdDetailID == child.ProdDetailID)
                                            {
                                                //不更新成本只改小计总计
                                                if (rdb小计总计.Checked)
                                                {
                                                    //saleoutdetails.Cost = child.Inv_Cost;
                                                    saleoutdetails.SubtotalCostAmount = (saleoutdetails.Cost + saleoutdetails.CustomizedCost) * saleoutdetails.Quantity;
                                                    saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                    if (saleoutdetails.TaxRate > 0)
                                                    {
                                                        saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                    }
                                                    needupdateOut = true;
                                                }
                                                if (rdb成本为0的才修复.Checked && saleoutdetails.Cost == 0)
                                                {
                                                    saleoutdetails.Cost = child.Inv_Cost;
                                                    saleoutdetails.SubtotalCostAmount = (saleoutdetails.Cost + saleoutdetails.CustomizedCost) * saleoutdetails.Quantity;

                                                    saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                    if (saleoutdetails.TaxRate > 0)
                                                    {
                                                        saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                    }
                                                    needupdateOut = true;
                                                }
                                                if (rdb时间区间.Checked)
                                                {
                                                    saleoutdetails.Cost = child.Inv_Cost;
                                                    saleoutdetails.SubtotalCostAmount = (saleoutdetails.Cost + saleoutdetails.CustomizedCost) * saleoutdetails.Quantity;

                                                    saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                    if (saleoutdetails.TaxRate > 0)
                                                    {
                                                        saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                    }
                                                    needupdateOut = true;
                                                }

                                                if (rdb小于指定成本.Checked && saleoutdetails.Cost != 0)
                                                {
                                                    if (saleoutdetails.Cost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        saleoutdetails.Cost = child.Inv_Cost;
                                                        saleoutdetails.SubtotalCostAmount = (saleoutdetails.Cost + saleoutdetails.CustomizedCost) * saleoutdetails.Quantity;

                                                        saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                        if (saleoutdetails.TaxRate > 0)
                                                        {
                                                            saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                        }
                                                        needupdateOut = true;
                                                    }
                                                }
                                                if (rdb大于单项成本.Checked && saleoutdetails.Cost != 0)
                                                {
                                                    if (saleoutdetails.Cost > txtUnitCost.Text.ToDecimal())
                                                    {
                                                        saleoutdetails.Cost = child.Inv_Cost;
                                                        saleoutdetails.SubtotalCostAmount = (saleoutdetails.Cost + saleoutdetails.CustomizedCost) * saleoutdetails.Quantity;

                                                        saleoutdetails.SubtotalTransAmount = saleoutdetails.TransactionPrice * saleoutdetails.Quantity;
                                                        if (saleoutdetails.TaxRate > 0)
                                                        {
                                                            saleoutdetails.SubtotalTaxAmount = saleoutdetails.SubtotalTransAmount / (1 + saleoutdetails.TaxRate) * saleoutdetails.TaxRate;
                                                        }
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
                                            SaleOut.TotalCommissionAmount = SaleOut.tb_SaleOutDetails.Sum(c => c.CommissionAmount);
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
                                                        if (rdb小于指定成本.Checked && SaleOutReDetail.Cost < txtUnitCost.Text.ToDecimal())
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
                                                        if (rdb大于单项成本.Checked && SaleOutReDetail.Cost > txtUnitCost.Text.ToDecimal())
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
                                                            if (rdb时间区间.Checked)
                                                            {
                                                                Refurbished.Cost = child.Inv_Cost;
                                                                Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                            }
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
                                                            if (rdb小于指定成本.Checked && Refurbished.Cost < txtUnitCost.Text.ToDecimal())
                                                            {
                                                                Refurbished.Cost = child.Inv_Cost;
                                                                Refurbished.SubtotalCostAmount = Refurbished.Cost * Refurbished.Quantity;
                                                            }
                                                            if (rdb大于单项成本.Checked && Refurbished.Cost > txtUnitCost.Text.ToDecimal())
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

                                        if (!chkTestMode.Checked && needupdateOut)
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
                                    .WhereIF(rdb时间区间.Checked, a => a.DeliveryDate >= ucAdvDateTimerPickerGroup1.dtp1.Value && a.DeliveryDate <= ucAdvDateTimerPickerGroup1.dtp2.Value)
                                    .Where(a => a.tb_FinishedGoodsInvDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID)).ToList();

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
                                                if (rdb时间区间.Checked)
                                                {
                                                    Detail.MaterialCost = child.Inv_Cost;
                                                    Detail.UnitCost = Detail.MaterialCost + Detail.ManuFee + Detail.ApportionedCost;
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
                                                if (rdb成本为0的才修复.Checked && Detail.UnitCost == 0)
                                                {
                                                    Detail.MaterialCost = child.Inv_Cost;
                                                    Detail.UnitCost = Detail.MaterialCost + Detail.ManuFee + Detail.ApportionedCost;
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
                                                if (rdb小于指定成本.Checked && Detail.UnitCost != 0)
                                                {
                                                    if (Detail.UnitCost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.MaterialCost = child.Inv_Cost;
                                                        Detail.UnitCost = Detail.MaterialCost + Detail.ManuFee + Detail.ApportionedCost;
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
                                                if (rdb大于单项成本.Checked && Detail.UnitCost != 0)
                                                {
                                                    if (Detail.UnitCost > txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.MaterialCost = child.Inv_Cost;
                                                        Detail.UnitCost = Detail.MaterialCost + Detail.ManuFee + Detail.ApportionedCost;
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
                                                if (rdb小计总计.Checked && Detail.UnitCost != 0)
                                                {    //如果存在则更新 

                                                    //Detail.MaterialCost = child.Inv_Cost;
                                                    Detail.UnitCost = Detail.MaterialCost + Detail.ManuFee + Detail.ApportionedCost;
                                                    Detail.ProductionAllCost = Detail.UnitCost * Detail.Qty;
                                                    //这时可以算出缴库的产品的单位成本
                                                    var nextInv = Detail.tb_proddetail.tb_Inventories.FirstOrDefault(c => c.Location_ID == Detail.Location_ID);
                                                    if (nextInv != null)
                                                    {
                                                        //nextInv.Inv_Cost = Detail.UnitCost;
                                                        if (!chkTestMode.Checked)
                                                        {
                                                            await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(nextInv).ExecuteCommandAsync();
                                                        }
                                                    }

                                                    updateFGListdetail.Add(Detail);

                                                }
                                                if (rdb其它.Checked && Detail.UnitCost != 0)
                                                {    //如果存在则更新 
                                                    decimal diffpirce = Math.Abs(Detail.UnitCost - child.Inv_Cost);
                                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.UnitCost.ToDouble()) > 10)
                                                    {
                                                        if (Detail.UnitCost > txtUnitCost.Text.ToDecimal())
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
                                 .WhereIF(rdb时间区间.Checked, a => a.Out_date >= ucAdvDateTimerPickerGroup1.dtp1.Value && a.Out_date <= ucAdvDateTimerPickerGroup1.dtp2.Value)
                                   .Where(a => a.tb_ProdBorrowingDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID)).ToList();

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
                                                if (rdb时间区间.Checked)
                                                {
                                                    Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                    updateBRListdetail.Add(Detail);
                                                    needupdate = true;
                                                }
                                                if (rdb成本为0的才修复.Checked && Detail.Cost == 0)
                                                {
                                                    Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                    updateBRListdetail.Add(Detail);
                                                    needupdate = true;
                                                }

                                                if (rdb小于指定成本.Checked && Detail.Cost != 0)
                                                {
                                                    if (Detail.Cost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateBRListdetail.Add(Detail);
                                                        needupdate = true;
                                                    }
                                                }
                                                if (rdb大于单项成本.Checked && Detail.Cost != 0)
                                                {
                                                    if (Detail.Cost > txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateBRListdetail.Add(Detail);
                                                        needupdate = true;
                                                    }
                                                }
                                                if (rdb其它.Checked && Detail.Cost != 0)
                                                {
                                                    //如果存在则更新 
                                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateBRListdetail.Add(Detail);
                                                        needupdate = true;
                                                    }
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


                                        if (needupdate)
                                        {
                                            bill.TotalCost = bill.tb_ProdBorrowingDetails.Sum(c => c.SubtotalCostAmount);
                                            updateListMain.Add(bill);
                                        }

                                        #region 归还单
                                        if (bill.tb_ProdReturnings != null)
                                        {
                                            foreach (var borrowReturn in bill.tb_ProdReturnings)
                                            {
                                                foreach (var returning in borrowReturn.tb_ProdReturningDetails)
                                                {
                                                    if (returning.ProdDetailID == child.ProdDetailID)
                                                    {
                                                        if (rdb时间区间.Checked)
                                                        {
                                                            returning.Cost = child.Inv_Cost;
                                                            returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                        }
                                                        if (rdb成本为0的才修复.Checked && returning.Cost == 0)
                                                        {
                                                            returning.Cost = child.Inv_Cost;
                                                            returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                        }
                                                        if (rdb小于指定成本.Checked && returning.Cost != 0)
                                                        {
                                                            if (returning.Cost < txtUnitCost.Text.ToDecimal())
                                                            {
                                                                returning.Cost = child.Inv_Cost;
                                                                returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                            }
                                                        }
                                                        if (rdb大于单项成本.Checked && returning.Cost != 0)
                                                        {
                                                            if (returning.Cost > txtUnitCost.Text.ToDecimal())
                                                            {
                                                                returning.Cost = child.Inv_Cost;
                                                                returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                            }
                                                        }
                                                        if (rdb其它.Checked && returning.Cost != 0)
                                                        {

                                                            returning.Cost = child.Inv_Cost;
                                                            returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                        }
                                                        if (rdb小计总计.Checked && returning.Cost != 0)
                                                        {
                                                            returning.SubtotalCostAmount = returning.Cost * returning.Qty;
                                                        }
                                                    }
                                                    borrowReturn.TotalCost = borrowReturn.tb_ProdReturningDetails.Sum(c => c.SubtotalCostAmount);

                                                    if (!chkTestMode.Checked)
                                                    {

                                                        await MainForm.Instance.AppContext.Db.Updateable<tb_ProdReturning>(borrowReturn).ExecuteCommandAsync();
                                                        await MainForm.Instance.AppContext.Db.Updateable<tb_ProdReturningDetail>(borrowReturn.tb_ProdReturningDetails).ExecuteCommandAsync();
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
                                      .WhereIF(rdb时间区间.Checked, a => a.Bill_Date >= ucAdvDateTimerPickerGroup1.dtp1.Value && a.Bill_Date <= ucAdvDateTimerPickerGroup1.dtp2.Value)
                                        .Where(a => a.tb_StockOutDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID)).ToList();

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
                                                if (rdb时间区间.Checked)
                                                {
                                                    Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                    updateStockOutListdetail.Add(Detail);
                                                }
                                                if (rdb成本为0的才修复.Checked && Detail.Cost == 0)
                                                {
                                                    Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                    updateStockOutListdetail.Add(Detail);
                                                }
                                                if (rdb小于指定成本.Checked && Detail.Cost != 0)
                                                {
                                                    if (Detail.Cost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateStockOutListdetail.Add(Detail);
                                                    }
                                                }
                                                if (rdb大于单项成本.Checked && Detail.Cost != 0)
                                                {
                                                    if (Detail.Cost > txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateStockOutListdetail.Add(Detail);
                                                    }
                                                }
                                                if (rdb其它.Checked && Detail.Cost != 0)
                                                {     //如果存在则更新 
                                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                        updateStockOutListdetail.Add(Detail);
                                                    }
                                                }
                                                if (rdb小计总计.Checked && Detail.Cost != 0)
                                                {
                                                    //Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCostAmount = Detail.Cost * Detail.Qty;
                                                    updateStockOutListdetail.Add(Detail);
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
                                  .WhereIF(rdb时间区间.Checked, a => a.DeliveryDate >= ucAdvDateTimerPickerGroup1.dtp1.Value && a.DeliveryDate <= ucAdvDateTimerPickerGroup1.dtp2.Value)
                                  .Where(a => a.tb_MaterialRequisitionDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID)).ToList();
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
                                                if (rdb时间区间.Checked)
                                                {
                                                    Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                    updateListdetail.Add(Detail);
                                                }
                                                if (rdb成本为0的才修复.Checked && Detail.Cost == 0)
                                                {
                                                    Detail.Cost = child.Inv_Cost;
                                                    Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                    updateListdetail.Add(Detail);
                                                }
                                                if (rdb小于指定成本.Checked && Detail.Cost != 0)
                                                {
                                                    if (Detail.Cost < txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                        updateListdetail.Add(Detail);
                                                    }
                                                }
                                                if (rdb大于单项成本.Checked && Detail.Cost != 0)
                                                {
                                                    if (Detail.Cost > txtUnitCost.Text.ToDecimal())
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                        updateListdetail.Add(Detail);
                                                    }
                                                }
                                                if (rdb其它.Checked && Detail.Cost != 0)
                                                { //如果存在则更新 
                                                    decimal diffpirce = Math.Abs(Detail.Cost - child.Inv_Cost);
                                                    if (ComparePrice(child.Inv_Cost.ToDouble(), Detail.Cost.ToDouble()) > 10)
                                                    {
                                                        Detail.Cost = child.Inv_Cost;
                                                        Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                        updateListdetail.Add(Detail);
                                                    }
                                                }
                                                if (rdb小计总计.Checked && Detail.Cost != 0)
                                                {
                                                    Detail.SubtotalCost = Detail.Cost * Detail.ActualSentQty;
                                                    updateListdetail.Add(Detail);
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
                richTextBoxLog.AppendText($"更新关联成本出错： {ex.Message} " + "\r\n");
                if (!chkTestMode.Checked)
                {
                    MainForm.Instance.AppContext.Db.Ado.RollbackTran();
                }

            }
        }


        /// <summary>
        /// 将母件当子件查询。找他的上级。如果存在则更新他的本身的成本及他的BOM的总成本，直到没有。如果进入循环看最多次数为10层是不可能的
        /// </summary>
        /// <param name="bOM_S"></param>
        private async void LoopUpdateBom(tb_BOM_S bOM_S)
        {
            if (bOM_S.SelfProductionAllCosts != bOM_S.OutProductionAllCosts)
            {
                richTextBoxLog.AppendText($"配方自产和外发成本不一样, {bOM_S.BOM_No} 条" + "\r\n");
            }

            #region 更新BOM价格将母件当子件去查询，如果他存在其它的配方中时，更新成本

            List<tb_BOM_S> BOM_SOrders = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
            .InnerJoin<tb_BOM_SDetail>((a, b) => a.BOM_ID == b.BOM_ID)
            .Includes(a => a.tb_BOM_SDetails)
            .Where(a => a.tb_BOM_SDetails.Any(c => c.ProdDetailID == bOM_S.ProdDetailID)).ToList();

            var distinctBOMbills = BOM_SOrders
            .GroupBy(o => o.BOM_ID)
            .Select(g => g.First())
            .ToList();
            totalloop++;
            if (totalloop > 6)
            {
                richTextBoxLog.AppendText($"BOM配方超出最大循环次数6，请检查BOM是否存在循环引用,SKU{bOM_S.SKU}" + "\r\n");
                throw new Exception($"BOM配方超出最大循环次数6，请检查BOM是否存在循环引用,SKU{bOM_S.SKU}");
            }
            List<tb_BOM_SDetail> updateListbomdetail = new List<tb_BOM_SDetail>();
            foreach (tb_BOM_S bill in distinctBOMbills)
            {
                foreach (var bomDetail in bill.tb_BOM_SDetails)
                {
                    if (bomDetail.ProdDetailID == bOM_S.ProdDetailID)
                    {
                        if (rdb成本为0的才修复.Checked && bomDetail.UnitCost == 0)
                        {
                            bomDetail.UnitCost = bOM_S.SelfProductionAllCosts;
                            bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                            updateListbomdetail.Add(bomDetail);
                        }
                        if (rdb小于指定成本.Checked && bomDetail.UnitCost != 0)
                        {
                            if (bomDetail.UnitCost < txtUnitCost.Text.ToDecimal())
                            {
                                bomDetail.UnitCost = bOM_S.SelfProductionAllCosts;
                                bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                updateListbomdetail.Add(bomDetail);
                            }
                        }
                        if (rdb大于单项成本.Checked && bomDetail.UnitCost != 0)
                        {
                            if (bomDetail.UnitCost > txtUnitCost.Text.ToDecimal())
                            {
                                bomDetail.UnitCost = bOM_S.SelfProductionAllCosts;
                                bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                                updateListbomdetail.Add(bomDetail);
                            }
                        }
                        if (rdb其它.Checked && bomDetail.UnitCost != 0)
                        {  //如果存在则更新 
                            decimal diffpirce = Math.Abs(bomDetail.UnitCost - bOM_S.SelfProductionAllCosts);
                            if (diffpirce > 0.01m)
                            {
                                bomDetail.UnitCost = bOM_S.SelfProductionAllCosts;
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
                if (!chkTestMode.Checked && updateListbomdetail.Count > 0)
                {
                    await MainForm.Instance.AppContext.Db.Updateable<tb_BOM_SDetail>(updateListbomdetail).ExecuteCommandAsync();
                }
                LoopUpdateBom(bill);
            }




            #endregion
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
                                                item.SubtotalCostAmount = (item.Cost + item.CustomizedCost) * item.Quantity;
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
                                                item.SubtotalCostAmount = (item.Cost + item.CustomizedCost) * item.Quantity;
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
            if (dataGridViewInv.CurrentRow != null)
            {
                List<tb_Inventory> inventories = new List<tb_Inventory>();
                if (dataGridViewInv.CurrentRow.DataBoundItem is View_Inventory inventory)
                {
                    inventories.Add(inventory.tb_inventory);
                }

                UpdateRelatedCost(inventories);
                MainForm.Instance.ShowStatusText("更新关联成本完成。");
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
                MainForm.Instance.ShowStatusText("更新库存成本完成。");
            }

        }


        private void kryptonLabel2_Click(object sender, EventArgs e)
        {

        }

        private async void 将配方成本更新到库存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                List<Control> controls = tabControl.SelectedTab.Controls.CastToList<Control>();
                DataGridView dgv = controls.FirstOrDefault(c => c.GetType().Name == "DataGridView") as DataGridView;
                if (dgv != null)
                {
                    dgv.AllowUserToAddRows = false;
                    if (tabControl.SelectedTab.Text.Contains("配方"))
                    {
                        if (tabControl.SelectedTab.Text.Contains("对应配方"))
                        {
                            foreach (DataGridViewRow item in dgv.SelectedRows)
                            {
                                if (dataGridViewInv.SelectedRows != null)
                                {
                                    List<tb_Inventory> inventories = new List<tb_Inventory>();
                                    foreach (DataGridViewRow dr in dataGridViewInv.Rows)
                                    {
                                        if (dr.DataBoundItem is View_Inventory inventory)
                                        {
                                            if (inventory.SKU == item.Cells["母件SKU"].Value.ToString())
                                            {
                                                inventories.Add(inventory.tb_inventory);
                                            }

                                        }
                                    }
                                    await UpdateInventoryCost(inventories);
                                }
                            }
                        }

                    }


                }

            }
        }

        private async void 库存成本更新为指定值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewInv.SelectedRows != null)
            {
                List<tb_Inventory> inventories = new List<tb_Inventory>();

                if (dataGridViewInv.CurrentRow != null && dataGridViewInv.CurrentRow.DataBoundItem is View_Inventory inventory)
                {
                    inventories.Add(inventory.tb_inventory);
                }

                if (inventories.Count == 1)
                {
                    await UpdateInventoryCost(inventories, txtUnitCost.Text.ToDecimal());
                }
                else
                {
                    //只能操作一行库存数据
                    MessageBox.Show($"一次只能操作一行库存数据，目前操作数行数：{inventories.Count}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MainForm.Instance.ShowStatusText("库存成本更新为指定值完成。");
            }
        }

        private void rdb时间区间_CheckedChanged(object sender, EventArgs e)
        {
            ucAdvDateTimerPickerGroup1.Visible = rdb时间区间.Checked;
        }

        private void 加载关联数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddLoadRelatedData();
        }


    }
}
