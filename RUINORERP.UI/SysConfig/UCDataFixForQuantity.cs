using AutoMapper;
using FastReport.DevComponents.DotNetBar.Controls;
using FluentValidation.Results;
using HLH.Lib.Security;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using NPOI.POIFS.Properties;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math.Field;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Context;
using RUINORERP.Repository.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.Common;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.PSI.PUR;
using RUINORERP.UI.PSI.SAL;
using RUINORERP.UI.Report;
using RUINORERP.UI.SS;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UserCenter.DataParts;
using RUINORERP.UI.WorkFlowTester;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Instrumentation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Tags;
using WorkflowCore.Interface;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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
        // 在类开始处添加：
        private static IEntityCacheManager _cacheManager;
        private static IEntityCacheManager CacheManager => _cacheManager ?? (_cacheManager = Startup.GetFromFac<IEntityCacheManager>());
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
            object obj = CacheManager.GetEntity<View_ProdDetail>(e.Value);
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

                object obj = CacheManager.GetEntity<View_ProdDetail>(ProdDetailID);
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

  
        private async void btnUpdateQuantity_Click(object sender, EventArgs e)
        {
            UpdateSelectedInventoryQuantity();
        }

        private async void UpdateSelectedInventoryQuantity()
        {
            if (dataGridViewInv.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要修改的库存记录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int newQuantity) || newQuantity < 0)
            {
                MessageBox.Show("请输入有效的库存数量（正整数）。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRows = dataGridViewInv.SelectedRows.Cast<DataGridViewRow>().ToList();
            int totalCount = selectedRows.Count;
            StringBuilder previewMsg = new StringBuilder();
            previewMsg.AppendLine($"即将修改 {totalCount} 条库存记录：");
            previewMsg.AppendLine();

            foreach (var row in selectedRows.Take(5))
            {
                if (row.DataBoundItem is View_Inventory inv)
                {
                    previewMsg.AppendLine($"SKU: {inv.SKU}, 当前数量: {inv.Quantity}, 新数量: {newQuantity}");
                }
            }

            if (totalCount > 5)
            {
                previewMsg.AppendLine($"... 还有 {totalCount - 5} 条记录");
            }

            previewMsg.AppendLine();
            previewMsg.AppendLine("是否继续？");

            if (chkTestMode.Checked)
            {
                previewMsg.Insert(0, "【测试模式】仅显示修改预览，不执行实际修改：\n\n");
                MessageBox.Show(previewMsg.ToString(), "测试模式预览", MessageBoxButtons.OK, MessageBoxIcon.Information);
                richTextBoxLog.AppendText($"[测试模式] 预览修改 {totalCount} 条记录，数量将改为 {newQuantity}\n");
                return;
            }

            DialogResult confirmResult = MessageBox.Show(previewMsg.ToString(), "确认修改", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            try
            {
                int successCount = 0;
                int failCount = 0;
                StringBuilder errorLog = new StringBuilder();

                foreach (var row in selectedRows)
                {
                    if (row.DataBoundItem is View_Inventory inv)
                    {
                        try
                        {
                            long inventoryId = inv.Inventory_ID;
                            long prodDetailId = inv.ProdDetailID.Value;
                            int oldQuantity = inv.Quantity.Value;

                            var inventoryEntity = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>()
                                .Where(it => it.Inventory_ID == inventoryId)
                                .FirstAsync();

                            if (inventoryEntity != null)
                            {
                                inventoryEntity.Quantity = newQuantity;
                                inventoryEntity.Modified_at = DateTime.Now;
                                inventoryEntity.Modified_by = MainForm.Instance.AppContext.CurUserInfo?.UserInfo?.User_ID;

                                int result = await MainForm.Instance.AppContext.Db.Updateable(inventoryEntity)
                                    .Where(it => it.Inventory_ID == inventoryId)
                                    .ExecuteCommandAsync();

                                if (result > 0)
                                {
                                    string auditDesc = $"库存手动修正：SKU[{inv.SKU}] 数量从 {oldQuantity} 变更为 {newQuantity}";
                                    await MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_Inventory>("库存数量修正", inventoryEntity, auditDesc);

                                    successCount++;
                                    richTextBoxLog.AppendText($"[成功] SKU:{inv.SKU} 数量 {oldQuantity}->{newQuantity}\n");
                                }
                                else
                                {
                                    failCount++;
                                    errorLog.AppendLine($"SKU:{inv.SKU} 更新失败");
                                }
                            }
                            else
                            {
                                failCount++;
                                errorLog.AppendLine($"SKU:{inv.SKU} 未找到库存记录");
                            }
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            errorLog.AppendLine($"SKU:{inv.SKU} 异常:{ex.Message}");
                            richTextBoxLog.AppendText($"[异常] SKU:{inv.SKU} {ex.Message}\n");
                        }
                    }
                }

                string resultMsg = $"修改完成：成功 {successCount} 条，失败 {failCount} 条";
                if (failCount > 0)
                {
                    resultMsg += $"\n错误信息：{errorLog.ToString()}";
                }

                MessageBox.Show(resultMsg, "操作完成", MessageBoxButtons.OK, failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                richTextBoxLog.AppendText($"[完成] {resultMsg}\n");

                await QueryInv();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改库存数量时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                richTextBoxLog.AppendText($"[错误] {ex.Message}\n");
            }
        }
    }
}
