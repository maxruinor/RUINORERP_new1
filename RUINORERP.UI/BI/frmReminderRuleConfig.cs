using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// 提醒规则选择窗体
    /// </summary>
    public partial class frmReminderRuleConfig : KryptonForm
    {
        /// <summary>
        /// 选择的规则ID
        /// </summary>
        public long SelectedRuleId { get; set; }

        /// <summary>
        /// 规则列表数据
        /// </summary>
        private List<tb_ReminderRule> _ruleList;

        /// <summary>
        /// 规则控制器
        /// </summary>
        private tb_ReminderRuleController<tb_ReminderRule> _ruleController;

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmReminderRuleConfig()
        {
            InitializeComponent();
            
            // 设计时不执行运行时逻辑
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                _ruleController = MainForm.Instance.AppContext.GetRequiredService<tb_ReminderRuleController<tb_ReminderRule>>();
                LoadRuleList();
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void frmReminderRuleConfig_Load(object sender, EventArgs e)
        {
            // 在运行时加载规则列表
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadRuleList();
            }
        }

        /// <summary>
        /// 加载规则列表
        /// </summary>
        private async void LoadRuleList()
        {
            try
            {
                // 加载规则数据
                _ruleList = await _ruleController.QueryAsync();
                dgvRuleList.DataSource = _ruleList;
                
                // 设置列标题
                dgvRuleList.Columns["RuleId"].HeaderText = "规则ID";
                dgvRuleList.Columns["RuleName"].HeaderText = "规则名称";
                dgvRuleList.Columns["Description"].HeaderText = "规则描述";
                dgvRuleList.Columns["RuleEngineType"].HeaderText = "引擎类型";
                dgvRuleList.Columns["ReminderBizType"].HeaderText = "业务类型";
                dgvRuleList.Columns["ReminderPriority"].HeaderText = "优先级";
                dgvRuleList.Columns["IsEnabled"].HeaderText = "是否启用";
                dgvRuleList.Columns["EffectiveDate"].HeaderText = "生效日期";
                dgvRuleList.Columns["ExpireDate"].HeaderText = "过期日期";
                
                // 隐藏不必要的列
                dgvRuleList.Columns["NotifyChannels"].Visible = false;
                dgvRuleList.Columns["NotifyRecipients"].Visible = false;
                dgvRuleList.Columns["JsonConfig"].Visible = false;
                dgvRuleList.Columns["Created_at"].Visible = false;
                dgvRuleList.Columns["Created_by"].Visible = false;
                dgvRuleList.Columns["Updated_at"].Visible = false;
                dgvRuleList.Columns["Updated_by"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载规则列表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 确认选择
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvRuleList.SelectedRows.Count > 0)
            {
                var selectedRow = dgvRuleList.SelectedRows[0];
                if (selectedRow.DataBoundItem is tb_ReminderRule selectedRule)
                {
                    SelectedRuleId = selectedRule.RuleId;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("请选择一条规则配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 取消选择
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 双击选择规则
        /// </summary>
        private void dgvRuleList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnOK_Click(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 搜索规则
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                var filteredList = _ruleList.Where(r => 
                    r.RuleName.Contains(keyword) || 
                    r.Description.Contains(keyword)).ToList();
                dgvRuleList.DataSource = filteredList;
            }
            else
            {
                dgvRuleList.DataSource = _ruleList;
            }
        }
    }
}