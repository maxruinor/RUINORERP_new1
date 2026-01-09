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
using RUINORERP.Business.LogicaService;
using RUINORERP.Business;

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// 提醒对象链路选择窗体
    /// </summary>
    public partial class frmReminderLinkConfig : KryptonForm
    {
        /// <summary>
        /// 选择的链路ID
        /// </summary>
        public long SelectedLinkId { get; set; }

        /// <summary>
        /// 链路列表数据
        /// </summary>
        private List<tb_ReminderObjectLink> _linkList;

        /// <summary>
        /// 链路控制器
        /// </summary>
        private tb_ReminderObjectLinkController<tb_ReminderObjectLink> _linkController;

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmReminderLinkConfig()
        {
            InitializeComponent();
            _linkController = MainForm.Instance.AppContext.GetRequiredService<tb_ReminderObjectLinkController<tb_ReminderObjectLink>>();
            LoadLinkList();
        }

        /// <summary>
        /// 加载链路列表
        /// </summary>
        private async void LoadLinkList()
        {
            try
            {
                // 加载链路数据
                _linkList = await _linkController.QueryAsync();
                dgvLinkList.DataSource = _linkList;
                
                // 设置列标题
                dgvLinkList.Columns["LinkId"].HeaderText = "链路ID";
                dgvLinkList.Columns["LinkName"].HeaderText = "链路名称";
                dgvLinkList.Columns["Description"].HeaderText = "链路描述";
                dgvLinkList.Columns["SourceType"].HeaderText = "提醒源类型";
                dgvLinkList.Columns["BizType"].HeaderText = "单据类型";
                dgvLinkList.Columns["ActionType"].HeaderText = "操作类型";
                dgvLinkList.Columns["TargetType"].HeaderText = "提醒目标类型";
                dgvLinkList.Columns["IsEnabled"].HeaderText = "是否启用";
                
                // 隐藏不必要的列
                dgvLinkList.Columns["SourceValue"].Visible = false;
                dgvLinkList.Columns["TargetValue"].Visible = false;
                dgvLinkList.Columns["BillStatus"].Visible = false;
                dgvLinkList.Columns["CreateTime"].Visible = false;
                dgvLinkList.Columns["CreateUserId"].Visible = false;
                dgvLinkList.Columns["UpdateTime"].Visible = false;
                dgvLinkList.Columns["UpdateUserId"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载链路列表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 确认选择
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvLinkList.SelectedRows.Count > 0)
            {
                var selectedRow = dgvLinkList.SelectedRows[0];
                if (selectedRow.DataBoundItem is tb_ReminderObjectLink selectedLink)
                {
                    SelectedLinkId = selectedLink.LinkId;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("请选择一条链路配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// 双击选择链路
        /// </summary>
        private void dgvLinkList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnOK_Click(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 搜索链路
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                var filteredList = _linkList.Where(l => 
                    l.LinkName.Contains(keyword) || 
                    l.Description.Contains(keyword)).ToList();
                dgvLinkList.DataSource = filteredList;
            }
            else
            {
                dgvLinkList.DataSource = _linkList;
            }
        }
    }
}