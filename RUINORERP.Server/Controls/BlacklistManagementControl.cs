using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Network.CommandHandlers;

/// <summary>
/// 黑名单管理控件
/// 功能：显示和管理系统的IP黑名单
/// 
/// ⚠️ NAT环境风险提示：
/// 1. 外网环境下，多台客户端通过路由器/NAT上网时会共享同一公网IP
/// 2. 封禁某个外网IP会导致该IP下的所有正常用户无法访问系统
/// 3. 建议优先使用用户名级别的限制（如登录次数限制），谨慎使用IP封禁功能
/// 4. 仅在确认是恶意攻击且已验证不会影响正常用户时，才使用IP封禁
/// 
/// 适用场景：
/// - 内网环境：可以安全使用IP封禁（每个设备有独立IP）
/// - 外网环境：需要特别谨慎，建议结合用户名+IP进行综合判断
/// </summary>
namespace RUINORERP.Server.Controls
{
    public partial class BlacklistManagementControl : UserControl
    {
        public BlacklistManagementControl()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            // 初始化数据
            RefreshBlacklistData();
        }

        private void BlacklistManagementControl_Load(object sender, EventArgs e)
        {
            // 设置DataGridView属性
            dataGridViewBlacklist.AutoGenerateColumns = false;
            dataGridViewBlacklist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBlacklist.MultiSelect = true;
        }

        /// <summary>
        /// 刷新黑名单数据
        /// </summary>
        public void RefreshBlacklistData()
        {
            try
            {
                // 绑定黑名单数据到DataGridView
                var blacklistEntries = BlacklistManager.GetBlacklistEntries();
                dataGridViewBlacklist.DataSource = new BindingList<BlacklistEntry>(blacklistEntries);
                
                // 更新状态标签
                if (toolStripStatusLabelBlacklistCount != null)
                {
                    toolStripStatusLabelBlacklistCount.Text = $"黑名单总数: {blacklistEntries.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新黑名单数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 按钮事件处理

        private void btnAddIP_Click(object sender, EventArgs e)
        {
            try
            {
                string ipAddress = textBoxIPAddress.Text.Trim();
                string reason = textBoxReason.Text.Trim();
                
                if (string.IsNullOrEmpty(ipAddress))
                {
                    MessageBox.Show("请输入IP地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 验证IP地址格式
                if (!IsValidIP(ipAddress))
                {
                    MessageBox.Show("请输入有效的IP地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 添加到黑名单
                BlacklistManager.AddIpToBlacklist(ipAddress, reason);
                
                // 刷新显示
                RefreshBlacklistData();
                
                // 清空输入框
                textBoxIPAddress.Clear();
                textBoxReason.Clear();
                
                MessageBox.Show("IP地址已添加到黑名单", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加IP到黑名单时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewBlacklist.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要移除的黑名单记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 确认删除
                DialogResult result = MessageBox.Show($"确定要移除选中的 {dataGridViewBlacklist.SelectedRows.Count} 条黑名单记录吗？", 
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dataGridViewBlacklist.SelectedRows)
                    {
                        if (row.DataBoundItem is BlacklistEntry entry)
                        {
                            BlacklistManager.RemoveFromBlacklist(entry.IPAddress);
                        }
                    }
                    
                    // 刷新显示
                    RefreshBlacklistData();
                    
                    MessageBox.Show("选中的黑名单记录已移除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移除黑名单记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                // 确认清空
                DialogResult result = MessageBox.Show("确定要清空所有黑名单记录吗？", 
                    "确认清空", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    BlacklistManager.ClearBlacklist();
                    
                    // 刷新显示
                    RefreshBlacklistData();
                    
                    MessageBox.Show("所有黑名单记录已清空", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清空黑名单时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshBlacklistData();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "文本文件|*.txt|所有文件|*.*";
                openFileDialog.Title = "选择要导入的IP黑名单文件";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] lines = System.IO.File.ReadAllLines(openFileDialog.FileName);
                    int successCount = 0;
                    
                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim();
                        if (!string.IsNullOrEmpty(trimmedLine) && IsValidIP(trimmedLine))
                        {
                            BlacklistManager.AddIpToBlacklist(trimmedLine, "批量导入");
                            successCount++;
                        }
                    }
                    
                    // 刷新显示
                    RefreshBlacklistData();
                    
                    MessageBox.Show($"成功导入 {successCount} 条IP地址到黑名单", "导入完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入黑名单时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 解除用户名登录限制（重置登录尝试次数）
        /// 实时生效，管理员可即时解除用户的登录限制
        /// </summary>
        private void btnUnlockUser_Click(object sender, EventArgs e)
        {
            try
            {
                string username = textBoxUsername.Text.Trim();
                
                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("请输入要解除限制的用户名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 调用LoginCommandHandler的公开方法解除用户限制
                bool success = LoginCommandHandler.UnlockUserLogin(username);
                
                if (success)
                {
                    MessageBox.Show($"用户名 \"{username}\" 的登录限制已解除，用户可立即尝试登录", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxUsername.Clear();
                }
                else
                {
                    MessageBox.Show($"未找到用户名 \"{username}\" 的登录限制记录，或用户名格式无效", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解除用户限制时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "文本文件|*.txt|所有文件|*.*";
                saveFileDialog.Title = "导出黑名单到文件";
                saveFileDialog.FileName = "blacklist.txt";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var blacklistEntries = BlacklistManager.GetBlacklistEntries();
                    List<string> lines = new List<string>();
                    
                    foreach (var entry in blacklistEntries)
                    {
                        lines.Add($"{entry.IPAddress} # {entry.Reason} # {entry.BanTime}");
                    }
                    
                    System.IO.File.WriteAllLines(saveFileDialog.FileName, lines);
                    
                    MessageBox.Show($"黑名单已导出到 {saveFileDialog.FileName}", "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出黑名单时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 验证IP地址格式是否有效
        /// </summary>
        /// <param name="ipAddress">IP地址字符串</param>
        /// <returns>是否有效的IP地址</returns>
        private bool IsValidIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return false;
            
            string[] parts = ipAddress.Split('.');
            if (parts.Length != 4)
                return false;
            
            foreach (string part in parts)
            {
                if (!int.TryParse(part, out int num) || num < 0 || num > 255)
                    return false;
            }
            
            return true;
        }

        #endregion

        #region DataGridView事件处理
        /// <summary>
        /// 处理DataGridView双击事件，显示选中行的详细信息
        /// </summary>
        /// <param name="sender">事件发送对象</param>
        /// <param name="e">事件参数</param>
        private void dataGridViewBlacklist_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewBlacklist.Rows.Count)
            {
                if (dataGridViewBlacklist.Rows[e.RowIndex].DataBoundItem is BlacklistEntry entry)
                {
                    textBoxIPAddress.Text = entry.IPAddress;
                    textBoxReason.Text = entry.Reason;
                }
            }
        }

        #endregion
    }
}