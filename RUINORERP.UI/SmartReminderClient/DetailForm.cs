using LiveChartsCore.Measure;
using Org.BouncyCastle.Asn1.Crmf;
using RUINORERP.Model.ReminderModel.ReminderResults;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.SmartReminderClient
{
    public class DetailForm : Form
    {
        private readonly IReminderResult _result;
        private DataGridView _dataGrid;

        public DetailForm(IReminderResult result)
        {
            _result = result;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Text = _result.Title;
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            // 创建数据表格
            _dataGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            Controls.Add(_dataGrid);

            // 添加操作按钮
            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft
            };

            var btnClose = new Button { Text = "关闭", Size = new Size(100, 40) };
            btnClose.Click += (s, e) => Close();

            var btnProcess = new Button { Text = "标记为已处理", Size = new Size(120, 40) };
            btnProcess.Click += (s, e) => MarkAsProcessed();

            btnPanel.Controls.AddRange(new Control[] { btnClose, btnProcess });
            Controls.Add(btnPanel);
        }

        private void LoadData()
        {
            // 创建数据表
            var table = new DataTable();

            // 添加列
            foreach (var column in _result.GetDataColumns())
            {
                table.Columns.Add(column);
            }

            // 添加行
            foreach (var row in _result.GetDataRows())
            {
                table.Rows.Add(row);
            }

            _dataGrid.DataSource = table;

            // 格式化列
            FormatColumns();
        }

        private void FormatColumns()
        {
            foreach (DataGridViewColumn column in _dataGrid.Columns)
            {
                if (column.ValueType == typeof(DateTime))
                {
                    column.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                }
                else if (column.ValueType == typeof(decimal))
                {
                    column.DefaultCellStyle.Format = "N2";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void MarkAsProcessed()
        {
            _result.IsProcessed = true;
            MessageBox.Show("已标记为已处理", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
