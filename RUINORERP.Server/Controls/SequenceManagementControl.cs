using RUINORERP.Business.BNR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server.Controls
{
    /// <summary>
    /// 序号管理控件
    /// 用于管理系统中的各种业务单据序号，支持查看、编辑、重置序号等操作
    /// </summary>
    public partial class SequenceManagementControl : UserControl
    {
        private readonly DatabaseSequenceService _sequenceService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sequenceService">数据库序列服务</param>
        public SequenceManagementControl()
        {
            _sequenceService = Startup.GetFromFac<DatabaseSequenceService>();
            InitializeComponent();
            LoadSequences();
        }

        /// <summary>
        /// 加载所有序号信息
        /// </summary>
        private void LoadSequences()
        {
            try
            {
                // 清空现有数据
                dgvSequences.Rows.Clear();
                
                // 获取所有序号记录
                var sequences = _sequenceService.GetAllSequences();
                
                // 填充到DataGridView
                foreach (var sequence in sequences)
                {
                    int rowIndex = dgvSequences.Rows.Add();
                    var row = dgvSequences.Rows[rowIndex];
                    
                    row.Cells["colId"].Value = sequence.Id;
                    row.Cells["colSequenceKey"].Value = sequence.SequenceKey;
                    row.Cells["colCurrentValue"].Value = sequence.CurrentValue;
                    row.Cells["colLastUpdated"].Value = sequence.LastUpdated;
                    row.Cells["colCreatedAt"].Value = sequence.CreatedAt;
                    row.Cells["colResetType"].Value = sequence.ResetType ?? "None";
                    row.Cells["colFormatMask"].Value = sequence.FormatMask ?? "";
                    row.Cells["colDescription"].Value = sequence.Description ?? "";
                    row.Cells["colBusinessType"].Value = sequence.BusinessType ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载序号信息失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 重置选中序号
        /// </summary>
        private void btnResetSelected_Click(object sender, EventArgs e)
        {
            if (dgvSequences.SelectedRows.Count > 0)
            {
                var selectedRow = dgvSequences.SelectedRows[0];
                string sequenceKey = selectedRow.Cells["colSequenceKey"].Value?.ToString();
                
                if (!string.IsNullOrEmpty(sequenceKey))
                {
                    if (MessageBox.Show($"确定要重置序号 '{sequenceKey}' 吗？这将把当前值重置为1。", "确认重置", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            _sequenceService.ResetSequence(sequenceKey);
                            MessageBox.Show("序号重置成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSequences(); // 重新加载数据
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"重置序号失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择要重置的序号记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 刷新序号列表
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSequences();
        }

        /// <summary>
        /// 按业务类型筛选
        /// </summary>
        private void btnFilterByType_Click(object sender, EventArgs e)
        {
            string businessType = txtBusinessTypeFilter.Text.Trim();
            
            if (string.IsNullOrEmpty(businessType))
            {
                LoadSequences(); // 如果筛选条件为空，加载所有数据
                return;
            }
            
            try
            {
                var sequences = _sequenceService.GetSequencesByBusinessType(businessType);
                
                dgvSequences.Rows.Clear();
                foreach (var sequence in sequences)
                {
                    int rowIndex = dgvSequences.Rows.Add();
                    var row = dgvSequences.Rows[rowIndex];
                    
                    row.Cells["colId"].Value = sequence.Id;
                    row.Cells["colSequenceKey"].Value = sequence.SequenceKey;
                    row.Cells["colCurrentValue"].Value = sequence.CurrentValue;
                    row.Cells["colLastUpdated"].Value = sequence.LastUpdated;
                    row.Cells["colCreatedAt"].Value = sequence.CreatedAt;
                    row.Cells["colResetType"].Value = sequence.ResetType ?? "None";
                    row.Cells["colFormatMask"].Value = sequence.FormatMask ?? "";
                    row.Cells["colDescription"].Value = sequence.Description ?? "";
                    row.Cells["colBusinessType"].Value = sequence.BusinessType ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"筛选序号失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 清空筛选条件
        /// </summary>
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtBusinessTypeFilter.Text = string.Empty;
            LoadSequences();
        }
    }
}