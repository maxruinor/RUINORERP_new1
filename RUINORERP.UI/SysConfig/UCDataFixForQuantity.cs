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


namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("库存数量数据校正", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCDataFixForQuantity : UserControl
    {
        public UCDataFixForQuantity()
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
            List<BizType> list = new List<BizType>();
            //在途数量
            list.Add(BizType.采购订单);
            list.Add(BizType.采购入库单);
            list.Add(BizType.采购退货单);
            list.Add(BizType.采购退货入库);
            list.Add(BizType.返工退库单);
            list.Add(BizType.返工入库单);

            //拟销量
            list.Add(BizType.销售订单);
            list.Add(BizType.销售出库单);
            list.Add(BizType.其他出库单);
            list.Add(BizType.销售出库单);
            //在制量
            list.Add(BizType.制令单);
            list.Add(BizType.缴库单);

            //未发数量
            list.Add(BizType.制令单);
            list.Add(BizType.生产领料单);
            list.Add(BizType.生产退料单);


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
            QueryInv();
        }


        private async Task QueryInv()
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
            dataGridViewInv.EditMode = DataGridViewEditMode.EditOnEnter;
            
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //设置一个集合：列名和显示的名称添加的集合中
            dic = new Dictionary<string, string> {
                { "SKU", "SKU" },
                { "CNName", "产品" },
                { "Quantity", "数量" },
                { "On_the_way_Qty", "在途" },
                { "Sale_Qty", "拟销" },
                { "MakingQty", "在制" },
                { "NotOutQty", "未发" }
            };

            //只显示成本 和详情ID。
            foreach (DataGridViewColumn item in dataGridViewInv.Columns)
            {
                if (dic.Any(c => c.Key.Contains(item.DataPropertyName)))
                {
                    item.Visible = true;
                    item.HeaderText = dic.Where(c => c.Key.Contains(item.DataPropertyName)).FirstOrDefault().Value;
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


        //将当前选择的库存行 为条件去查询所有相关的单据 比方 在途中 是采购，加工 退货等
        //还要整理一下 哪些是记录到在途数量的单据。加工厂的退返
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

                object obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(ProdDetailID);
                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail _prodDetail)
                {
                    prodDetail = _prodDetail;
                    txtSearchKey.Text = prodDetail.SKU;
                }
            }

            //显示选择库存行对应的其它相关数据

            tabControl.TabPages.Clear();
            tabControl.Dock = DockStyle.Fill;
            foreach (TreeNode item in treeViewNeedUpdateCostList.Nodes)
            {
                if (item.Checked && item.Tag is BizType bt)
                {
                    switch (bt)
                    {


                        case BizType.销售订单:

 


                            break;
                        case BizType.销售出库单:

                           
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

                         
                            break;
                        case BizType.盘点单:
                            break;
                        case BizType.制令单:
                          

                            break;
                        case BizType.BOM物料清单:

                        

                            break;
                        case BizType.生产领料单:


                           
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

                        
                            break;
                        case BizType.请购单:
                            break;
                        case BizType.产品分割单:
                            break;
                        case BizType.产品组合单:
                            

                            break;
                        case BizType.借出单:

                          
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


        
        


      

        private async Task 更新为当前成本ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private async Task toolStripMenuItem修复小计总计_Click(object sender, EventArgs e)
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
                
                
            }
        }

        private async void 更新库存成本数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewInv.SelectedRows != null)
            {
                
            }

        }


        private void kryptonLabel2_Click(object sender, EventArgs e)
        {

        }

        private async Task 将配方成本更新到库存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                List<Control> controls = tabControl.SelectedTab.Controls.CastToList<Control>();
                DataGridView dgv = controls.FirstOrDefault(c => c.GetType().Name == "DataGridView") as DataGridView;
                if (dgv != null)
                {
                    dgv.AllowUserToAddRows = false;
                    if (tabControl.SelectedTab.Text.Contains("对应配方"))
                    {
                        foreach (DataGridViewRow item in dgv.SelectedRows)
                        {
                            if (true)
                            {
                                if (dataGridViewInv.SelectedRows != null)
                                {
                                    
                                    
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
                foreach (DataGridViewRow dr in dataGridViewInv.SelectedRows)
                {
                    if (dr.DataBoundItem is View_Inventory inventory)
                    {
                        inventories.Add(inventory.tb_inventory);
                    }
                }
                if (inventories.Count == 1)
                {
                   
                }
                else
                {
                    //只能操作一行库存数据
                    MessageBox.Show("一次只能操作一行库存数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
    }
}
