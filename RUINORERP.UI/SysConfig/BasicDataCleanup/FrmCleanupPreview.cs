using Krypton.Toolkit;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 数据清理预览对话框
    /// </summary>
    public partial class FrmCleanupPreview : KryptonForm
    {
        /// <summary>
        /// 预览结果
        /// </summary>
        public CleanupPreviewResult PreviewResult { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCleanupPreview()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmCleanupPreview_Load(object sender, EventArgs e)
        {
            if (PreviewResult == null)
            {
                MessageBox.Show("预览结果为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadPreviewData();
        }

        /// <summary>
        /// 加载预览数据
        /// </summary>
        private void LoadPreviewData()
        {
            try
            {
                // 设置基本信息
                klblConfigName.Text = $"配置名称: {PreviewResult.ConfigName}";
                klblTotalRecords.Text = $"总记录数: {PreviewResult.TotalRecordCount}";
                klblPreviewTime.Text = $"预览时间: {PreviewResult.PreviewTime:yyyy-MM-dd HH:mm:ss}";

                // 加载规则匹配统计
                LoadRuleStatistics();

                // 加载数据预览
                LoadDataPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载预览数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载规则统计
        /// </summary>
        private void LoadRuleStatistics()
        {
            try
            {
                dgvRuleStats.Rows.Clear();

                if (PreviewResult.RuleMatchCounts == null || PreviewResult.RuleMatchCounts.Count == 0)
                {
                    return;
                }

                foreach (var kvp in PreviewResult.RuleMatchCounts)
                {
                    int rowIndex = dgvRuleStats.Rows.Add();
                    var row = dgvRuleStats.Rows[rowIndex];
                    row.Cells["colRuleName"].Value = kvp.Key;
                    row.Cells["colMatchCount"].Value = kvp.Value;

                    // 计算百分比
                    double percentage = PreviewResult.TotalRecordCount > 0
                        ? (double)kvp.Value / PreviewResult.TotalRecordCount * 100
                        : 0;
                    row.Cells["colPercentage"].Value = $"{percentage:F2}%";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载规则统计失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载数据预览
        /// </summary>
        private void LoadDataPreview()
        {
            try
            {
                // 创建数据表
                var dataTable = new DataTable();

                // 添加列
                dataTable.Columns.Add("操作类型", typeof(string));
                dataTable.Columns.Add("记录数", typeof(int));

                // 添加行
                dataTable.Rows.Add("删除", PreviewResult.RecordsToDelete?.Count ?? 0);
                dataTable.Rows.Add("更新", PreviewResult.RecordsToUpdate?.Count ?? 0);
                dataTable.Rows.Add("标记无效", PreviewResult.RecordsToMark?.Count ?? 0);
                dataTable.Rows.Add("归档", PreviewResult.RecordsToArchive?.Count ?? 0);

                dgvSummary.DataSource = dataTable;

                // 加载详细数据预览（显示第一条记录作为示例）
                LoadDetailPreview();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载数据预览失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载详细预览
        /// </summary>
        private void LoadDetailPreview()
        {
            try
            {
                // 这里简化处理，实际应该根据选择的操作类型显示对应的预览数据
                var previewData = PreviewResult.RecordsToDelete;
                if (previewData != null && previewData.Count > 0)
                {
                    // 创建数据表
                    var dataTable = new DataTable();

                    // 从第一条记录获取列名
                    var firstRecord = previewData.First();
                    foreach (var key in firstRecord.Keys.Take(10)) // 只取前10列
                    {
                        dataTable.Columns.Add(key, typeof(object));
                    }

                    // 添加数据行
                    foreach (var record in previewData.Take(10)) // 只显示前10条
                    {
                        var row = dataTable.NewRow();
                        foreach (var column in dataTable.Columns.Cast<DataColumn>())
                        {
                            if (record.ContainsKey(column.ColumnName))
                            {
                                row[column.ColumnName] = record[column.ColumnName];
                            }
                        }
                        dataTable.Rows.Add(row);
                    }

                    dgvDetailPreview.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载详细预览失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void KbtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
