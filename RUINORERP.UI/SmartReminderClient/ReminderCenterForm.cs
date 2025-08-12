using LiveChartsCore.Measure;
using Org.BouncyCastle.Asn1.Crmf;
using RUINORERP.Business.ReminderService;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.ReminderModel.ReminderResults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.SmartReminderClient
{
    public class ReminderCenterForm : Form
    {
        private readonly ReminderResultManager _resultManager;
        private DataGridView _grid;
        private ToolStrip _toolStrip;

        public ReminderCenterForm(ReminderResultManager manager)
        {
            _resultManager = manager;
            InitializeUI();
            LoadData();

            // 订阅新结果事件
            _resultManager.OnNewResult += OnNewResultAdded;
        }

        private void InitializeUI()
        {
            Text = "智能消息管理中心";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;

            // 创建工具栏
            _toolStrip = new ToolStrip { Dock = DockStyle.Top };

            // 添加过滤选项
            var lblFilter = new ToolStripLabel("过滤:");
            var cmbStatus = new ToolStripComboBox();
            cmbStatus.Items.AddRange(new[] { "所有消息", "未读消息", "未处理消息", "已归档消息" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += (s, e) => LoadData();

            var cmbType = new ToolStripComboBox();
            cmbType.Items.AddRange(Enum.GetNames(typeof(ReminderBizType)));
            cmbType.Items.Insert(0, "所有类型");
            cmbType.SelectedIndex = 0;
            cmbType.SelectedIndexChanged += (s, e) => LoadData();

            var btnRefresh = new ToolStripButton("刷新");
            btnRefresh.Click += (s, e) => LoadData();

            _toolStrip.Items.AddRange(new ToolStripItem[] { lblFilter, cmbStatus, cmbType, btnRefresh });

            // 创建数据表格
            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // 添加列
            _grid.Columns.Add("Title", "标题");
            _grid.Columns.Add("Summary", "摘要");
            _grid.Columns.Add("BusinessType", "类型");
            _grid.Columns.Add("TriggerTime", "提醒时间");
            _grid.Columns.Add("IsRead", "已读");
            _grid.Columns.Add("IsProcessed", "已处理");

            // 设置列格式
            _grid.Columns["TriggerTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            _grid.Columns["IsRead"].ValueType = typeof(bool);
            _grid.Columns["IsProcessed"].ValueType = typeof(bool);

            // 添加双击事件
            _grid.CellDoubleClick += (s, e) => OpenDetailView();

            // 添加上下文菜单
            var contextMenu = new ContextMenuStrip();
            var markAsReadItem = new ToolStripMenuItem("标记为已读");
            markAsReadItem.Click += (s, e) => MarkSelectedAsRead();

            var markAsProcessedItem = new ToolStripMenuItem("标记为已处理");
            markAsProcessedItem.Click += (s, e) => MarkSelectedAsProcessed();

            contextMenu.Items.AddRange(new ToolStripItem[] { markAsReadItem, markAsProcessedItem });
            _grid.ContextMenuStrip = contextMenu;

            // 添加控件到窗体
            Controls.Add(_grid);
            Controls.Add(_toolStrip);
        }

        private void LoadData()
        {
            // 获取过滤条件
            var statusFilter = _toolStrip.Items[1] as ToolStripComboBox;
            var typeFilter = _toolStrip.Items[2] as ToolStripComboBox;

            // 获取结果
            var results = _resultManager.GetResults();

            // 应用状态过滤
            if (statusFilter.SelectedIndex == 1) // 未读消息
                results = results.Where(r => !r.IsRead);
            else if (statusFilter.SelectedIndex == 2) // 未处理消息
                results = results.Where(r => !r.IsProcessed);
            else if (statusFilter.SelectedIndex == 3) // 已归档消息
                results = results.Where(r => r.IsArchived);

            // 应用类型过滤
            if (typeFilter.SelectedIndex > 0)
            {
                var selectedType = (ReminderBizType)Enum.Parse(
                    typeof(ReminderBizType),
                    typeFilter.SelectedItem.ToString());

                results = results.Where(r => r.BusinessType == selectedType);
            }

            // 绑定数据
            _grid.Rows.Clear();
            foreach (var result in results.OrderByDescending(r => r.TriggerTime))
            {
                _grid.Rows.Add(
                    result.Title,
                    result.Summary,
                    result.BusinessType.GetDescription(), // 使用扩展方法获取描述
                    result.TriggerTime,
                    result.IsRead,
                    result.IsProcessed
                );

                // 设置行标记以便后续访问
                _grid.Rows[_grid.Rows.Count - 1].Tag = result;
            }
        }

        private void OpenDetailView()
        {
            if (_grid.SelectedRows.Count > 0)
            {
                var result = _grid.SelectedRows[0].Tag as IReminderResult;
                if (result != null)
                {
                    var detailForm = new DetailForm(result);
                    detailForm.ShowDialog();

                    // 刷新数据
                    LoadData();
                }
            }
        }

        private void MarkSelectedAsRead()
        {
            foreach (DataGridViewRow row in _grid.SelectedRows)
            {
                var result = row.Tag as IReminderResult;
                if (result != null && !result.IsRead)
                {
                    result.IsRead = true;
                    row.Cells["IsRead"].Value = true;
                }
            }
        }

        private void MarkSelectedAsProcessed()
        {
            foreach (DataGridViewRow row in _grid.SelectedRows)
            {
                var result = row.Tag as IReminderResult;
                if (result != null && !result.IsProcessed)
                {
                    result.IsProcessed = true;
                    row.Cells["IsProcessed"].Value = true;
                }
            }
        }

        private void OnNewResultAdded(object sender, IReminderResult result)
        {
            // 在新结果到来时刷新数据
            if (IsHandleCreated)
            {
                Invoke((Action)LoadData);
            }
        }

        // 关闭窗体时取消订阅事件
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _resultManager.OnNewResult -= OnNewResultAdded;
        }
    }
}
