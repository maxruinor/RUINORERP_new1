using Krypton.Toolkit;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 数据清理执行结果对话框
    /// </summary>
    public partial class FrmCleanupResult : KryptonForm
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public CleanupExecutionResult ExecutionResult { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCleanupResult()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmCleanupResult_Load(object sender, EventArgs e)
        {
            if (ExecutionResult == null)
            {
                MessageBox.Show("执行结果为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadExecutionResult();
        }

        /// <summary>
        /// 加载执行结果
        /// </summary>
        private void LoadExecutionResult()
        {
            try
            {
                // 设置基本信息
                klblConfigName.Text = $"配置名称: {ExecutionResult.ConfigName}";
                klblExecuteMode.Text = $"执行模式: {(ExecutionResult.IsTestMode ? "测试模式" : "正式执行")}";
                klblExecuteResult.Text = $"执行结果: {(ExecutionResult.IsSuccess ? "成功" : "失败")}";
                klblExecuteTime.Text = $"执行时间: {ExecutionResult.StartTime:yyyy-MM-dd HH:mm:ss} ~ {ExecutionResult.EndTime:yyyy-MM-dd HH:mm:ss}";
                klblElapsedTime.Text = $"耗时: {ExecutionResult.ElapsedMilliseconds} 毫秒";

                // 加载统计信息
                LoadStatistics();

                // 加载规则执行结果
                LoadRuleResults();

                // 加载日志
                LoadLog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载执行结果失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载统计信息
        /// </summary>
        private void LoadStatistics()
        {
            try
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("统计项", typeof(string));
                dataTable.Columns.Add("数值", typeof(string));

                dataTable.Rows.Add("总记录数", ExecutionResult.TotalRecordCount.ToString("N0"));
                dataTable.Rows.Add("匹配清理条件", ExecutionResult.MatchedRecordCount.ToString("N0"));
                dataTable.Rows.Add("成功处理", ExecutionResult.SuccessCount.ToString("N0"));
                dataTable.Rows.Add("处理失败", ExecutionResult.FailedCount.ToString("N0"));
                dataTable.Rows.Add("跳过记录", ExecutionResult.SkippedCount.ToString("N0"));
                dataTable.Rows.Add("", "");
                dataTable.Rows.Add("删除记录", ExecutionResult.DeletedCount.ToString("N0"));
                dataTable.Rows.Add("标记无效", ExecutionResult.MarkedInvalidCount.ToString("N0"));
                dataTable.Rows.Add("归档记录", ExecutionResult.ArchivedCount.ToString("N0"));
                dataTable.Rows.Add("更新记录", ExecutionResult.UpdatedCount.ToString("N0"));
                dataTable.Rows.Add("仅记录", ExecutionResult.LogOnlyCount.ToString("N0"));

                if (!string.IsNullOrEmpty(ExecutionResult.BackupTableName))
                {
                    dataTable.Rows.Add("", "");
                    dataTable.Rows.Add("备份表名", ExecutionResult.BackupTableName);
                }

                dgvStatistics.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载统计信息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载规则执行结果
        /// </summary>
        private void LoadRuleResults()
        {
            try
            {
                dgvRuleResults.Rows.Clear();

                if (ExecutionResult.RuleResults == null || ExecutionResult.RuleResults.Count == 0)
                {
                    return;
                }

                foreach (var ruleResult in ExecutionResult.RuleResults)
                {
                    int rowIndex = dgvRuleResults.Rows.Add();
                    var row = dgvRuleResults.Rows[rowIndex];
                    row.Cells["colRuleName"].Value = ruleResult.RuleName;
                    row.Cells["colRuleType"].Value = GetRuleTypeDisplayName(ruleResult.RuleType);
                    row.Cells["colMatchedCount"].Value = ruleResult.MatchedCount;
                    row.Cells["colSuccessCount"].Value = ruleResult.SuccessCount;
                    row.Cells["colFailedCount"].Value = ruleResult.FailedCount;
                    row.Cells["colStatus"].Value = ruleResult.IsSuccess ? "成功" : "失败";
                    row.Cells["colElapsedTime"].Value = $"{ruleResult.ElapsedMilliseconds}ms";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载规则执行结果失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载日志
        /// </summary>
        private void LoadLog()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"执行ID: {ExecutionResult.ExecutionId}");
                sb.AppendLine($"配置ID: {ExecutionResult.ConfigId}");
                sb.AppendLine($"配置名称: {ExecutionResult.ConfigName}");
                sb.AppendLine($"执行模式: {(ExecutionResult.IsTestMode ? "测试模式" : "正式执行")}");
                sb.AppendLine($"执行结果: {(ExecutionResult.IsSuccess ? "成功" : "失败")}");
                sb.AppendLine($"执行时间: {ExecutionResult.StartTime:yyyy-MM-dd HH:mm:ss} ~ {ExecutionResult.EndTime:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"耗时: {ExecutionResult.ElapsedMilliseconds} 毫秒");
                sb.AppendLine($"执行机器: {ExecutionResult.MachineName}");
                sb.AppendLine();
                sb.AppendLine("统计信息:");
                sb.AppendLine($"  总记录数: {ExecutionResult.TotalRecordCount}");
                sb.AppendLine($"  匹配清理条件: {ExecutionResult.MatchedRecordCount}");
                sb.AppendLine($"  成功处理: {ExecutionResult.SuccessCount}");
                sb.AppendLine($"  处理失败: {ExecutionResult.FailedCount}");
                sb.AppendLine($"  跳过记录: {ExecutionResult.SkippedCount}");
                sb.AppendLine();
                sb.AppendLine("操作明细:");
                sb.AppendLine($"  删除: {ExecutionResult.DeletedCount}");
                sb.AppendLine($"  标记无效: {ExecutionResult.MarkedInvalidCount}");
                sb.AppendLine($"  归档: {ExecutionResult.ArchivedCount}");
                sb.AppendLine($"  更新: {ExecutionResult.UpdatedCount}");
                sb.AppendLine($"  仅记录: {ExecutionResult.LogOnlyCount}");

                if (!string.IsNullOrEmpty(ExecutionResult.BackupTableName))
                {
                    sb.AppendLine();
                    sb.AppendLine($"备份表名: {ExecutionResult.BackupTableName}");
                }

                if (!string.IsNullOrEmpty(ExecutionResult.ErrorMessage))
                {
                    sb.AppendLine();
                    sb.AppendLine($"错误信息: {ExecutionResult.ErrorMessage}");
                }

                ktxtLog.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载日志失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取规则类型显示名称
        /// </summary>
        private string GetRuleTypeDisplayName(CleanupRuleType ruleType)
        {
            return CleanupDisplayNames.GetRuleTypeDisplayName(ruleType);
        }

        /// <summary>
        /// 导出按钮点击事件
        /// </summary>
        private void KbtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "文本文件|*.txt|所有文件|*.*";
                    saveDialog.FileName = $"清理结果_{ExecutionResult.ConfigName}_{DateTime.Now:yyyyMMddHHmmss}.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(saveDialog.FileName, ktxtLog.Text);
                        MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
