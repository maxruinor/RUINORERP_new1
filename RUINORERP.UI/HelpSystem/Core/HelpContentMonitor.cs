using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 帮助内容缺失监控器
    /// 用于监控和记录缺失的帮助内容,为后续完善提供依据
    /// </summary>
    public static class HelpContentMonitor
    {
        #region 字段

        /// <summary>
        /// 缺失的帮助内容记录
        /// Key: 帮助键
        /// Value: 记录次数
        /// </summary>
        private static readonly Dictionary<string, HelpMissingRecord> _missingHelpRecords = new Dictionary<string, HelpMissingRecord>();

        /// <summary>
        /// 监控日志文件路径
        /// </summary>
        private static readonly string LogFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Logs",
            "HelpContentMonitor.log");

        /// <summary>
        /// 监控统计文件路径
        /// </summary>
        private static readonly string StatisticsFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Logs",
            "HelpContentStatistics.json");

        /// <summary>
        /// 监控开关
        /// </summary>
        public static bool IsMonitoringEnabled { get; set; } = true;

        /// <summary>
        /// 自动保存间隔(秒)
        /// </summary>
        public static int AutoSaveInterval { get; set; } = 300; // 默认5分钟

        #endregion

        #region 公共方法

        /// <summary>
        /// 记录缺失的帮助内容
        /// </summary>
        /// <param name="helpKey">帮助键</param>
        /// <param name="helpLevel">帮助级别</param>
        /// <param name="controlName">控件名称(可选)</param>
        /// <param name="entityType">实体类型(可选)</param>
        public static void LogMissingHelp(string helpKey, HelpLevel helpLevel, string controlName = null, Type entityType = null)
        {
            if (!IsMonitoringEnabled || string.IsNullOrEmpty(helpKey))
            {
                return;
            }

            try
            {
                // 创建或更新记录
                if (!_missingHelpRecords.ContainsKey(helpKey))
                {
                    _missingHelpRecords[helpKey] = new HelpMissingRecord
                    {
                        HelpKey = helpKey,
                        HelpLevel = helpLevel,
                        ControlName = controlName,
                        EntityType = entityType?.Name,
                        FirstMissingTime = DateTime.Now,
                        LastMissingTime = DateTime.Now,
                        MissingCount = 0
                    };
                }

                // 更新记录
                var record = _missingHelpRecords[helpKey];
                record.MissingCount++;
                record.LastMissingTime = DateTime.Now;
                if (!string.IsNullOrEmpty(controlName))
                {
                    record.ControlName = controlName;
                }
                if (entityType != null)
                {
                    record.EntityType = entityType.Name;
                }

                // 写入日志
                WriteLog(helpKey, helpLevel, controlName, entityType?.Name, record.MissingCount);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"记录缺失帮助失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取所有缺失的帮助内容
        /// </summary>
        /// <returns>缺失帮助列表</returns>
        public static List<HelpMissingRecord> GetMissingHelpList()
        {
            return _missingHelpRecords.Values
                .OrderByDescending(r => r.MissingCount)
                .ThenBy(r => r.HelpLevel)
                .ToList();
        }

        /// <summary>
        /// 获取高频缺失帮助(缺失次数大于指定阈值)
        /// </summary>
        /// <param name="threshold">阈值,默认为5</param>
        /// <returns>高频缺失帮助列表</returns>
        public static List<HelpMissingRecord> GetHighFrequencyMissingHelp(int threshold = 5)
        {
            return _missingHelpRecords.Values
                .Where(r => r.MissingCount >= threshold)
                .OrderByDescending(r => r.MissingCount)
                .ToList();
        }

        /// <summary>
        /// 导出缺失帮助报告
        /// </summary>
        /// <param name="outputPath">输出文件路径</param>
        public static void ExportMissingHelpReport(string outputPath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(outputPath))
                {
                    outputPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Logs",
                        $"HelpMissingReport_{DateTime.Now:yyyyMMdd_HHmmss}.md");
                }

                var missingList = GetMissingHelpList();

                if (missingList.Count == 0)
                {
                    File.WriteAllText(outputPath, "# 帮助内容缺失报告\n\n暂无缺失的帮助内容记录。\n");
                    return;
                }

                var report = new System.Text.StringBuilder();
                report.AppendLine("# 帮助内容缺失报告");
                report.AppendLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine($"总记录数: {missingList.Count}");
                report.AppendLine();

                // 统计信息
                report.AppendLine("## 统计信息");
                report.AppendLine();
                report.AppendLine($"- 控件级别缺失: {missingList.Count(r => r.HelpLevel == HelpLevel.Control)}");
                report.AppendLine($"- 窗体级别缺失: {missingList.Count(r => r.HelpLevel == HelpLevel.Form)}");
                report.AppendLine($"- 模块级别缺失: {missingList.Count(r => r.HelpLevel == HelpLevel.Module)}");
                report.AppendLine();

                // 高频缺失
                var highFreqList = GetHighFrequencyMissingHelp(5);
                if (highFreqList.Count > 0)
                {
                    report.AppendLine("## 高频缺失帮助 (缺失次数 >= 5)");
                    report.AppendLine();
                    foreach (var record in highFreqList)
                    {
                        report.AppendLine($"### {record.HelpKey}");
                        report.AppendLine($"- 缺失次数: {record.MissingCount}");
                        report.AppendLine($"- 帮助级别: {record.HelpLevel}");
                        if (!string.IsNullOrEmpty(record.ControlName))
                        {
                            report.AppendLine($"- 控件名称: {record.ControlName}");
                        }
                        if (!string.IsNullOrEmpty(record.EntityType))
                        {
                            report.AppendLine($"- 实体类型: {record.EntityType}");
                        }
                        report.AppendLine($"- 首次缺失: {record.FirstMissingTime:yyyy-MM-dd HH:mm:ss}");
                        report.AppendLine($"- 最后缺失: {record.LastMissingTime:yyyy-MM-dd HH:mm:ss}");
                        report.AppendLine();
                    }
                }

                // 完整列表
                report.AppendLine("## 完整缺失列表");
                report.AppendLine();
                foreach (var record in missingList)
                {
                    report.AppendLine($"### {record.HelpKey}");
                    report.AppendLine($"- 缺失次数: {record.MissingCount}");
                    report.AppendLine($"- 帮助级别: {record.HelpLevel}");
                    if (!string.IsNullOrEmpty(record.ControlName))
                    {
                        report.AppendLine($"- 控件名称: {record.ControlName}");
                    }
                    if (!string.IsNullOrEmpty(record.EntityType))
                    {
                        report.AppendLine($"- 实体类型: {record.EntityType}");
                    }
                    report.AppendLine($"- 首次缺失: {record.FirstMissingTime:yyyy-MM-dd HH:mm:ss}");
                    report.AppendLine($"- 最后缺失: {record.LastMissingTime:yyyy-MM-dd HH:mm:ss}");
                    report.AppendLine();
                }

                File.WriteAllText(outputPath, report.ToString());

                System.Diagnostics.Debug.WriteLine($"帮助内容缺失报告已导出: {outputPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"导出缺失帮助报告失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 清除所有缺失记录
        /// </summary>
        public static void ClearAllRecords()
        {
            _missingHelpRecords.Clear();
            System.Diagnostics.Debug.WriteLine("已清除所有缺失帮助记录");
        }

        /// <summary>
        /// 保存监控数据
        /// </summary>
        public static void SaveMonitoringData()
        {
            try
            {
                // 确保目录存在
                var directory = Path.GetDirectoryName(StatisticsFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 序列化为JSON
                var json = System.Text.Json.JsonSerializer.Serialize(_missingHelpRecords, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(StatisticsFilePath, json);

                System.Diagnostics.Debug.WriteLine($"监控数据已保存: {StatisticsFilePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存监控数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载监控数据
        /// </summary>
        public static void LoadMonitoringData()
        {
            try
            {
                if (File.Exists(StatisticsFilePath))
                {
                    var json = File.ReadAllText(StatisticsFilePath);
                    var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, HelpMissingRecord>>(json);

                    if (data != null)
                    {
                        // 清空现有数据并添加新数据
                        _missingHelpRecords.Clear();
                        foreach (var item in data)
                        {
                            _missingHelpRecords[item.Key] = item.Value;
                        }
                        System.Diagnostics.Debug.WriteLine($"监控数据已加载: {data.Count} 条记录");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载监控数据失败: {ex.Message}");
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 写入日志
        /// </summary>
        private static void WriteLog(string helpKey, HelpLevel helpLevel, string controlName, string entityType, int count)
        {
            try
            {
                // 确保目录存在
                var directory = Path.GetDirectoryName(LogFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 缺失帮助: {helpKey}, 级别: {helpLevel}, 控件: {controlName ?? "N/A"}, 实体: {entityType ?? "N/A"}, 次数: {count}";

                File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"写入帮助监控日志失败: {ex.Message}");
            }
        }

        #endregion

        #region 内部类

        /// <summary>
        /// 帮助缺失记录
        /// </summary>
        public class HelpMissingRecord
        {
            /// <summary>
            /// 帮助键
            /// </summary>
            public string HelpKey { get; set; }

            /// <summary>
            /// 帮助级别
            /// </summary>
            public HelpLevel HelpLevel { get; set; }

            /// <summary>
            /// 控件名称
            /// </summary>
            public string ControlName { get; set; }

            /// <summary>
            /// 实体类型
            /// </summary>
            public string EntityType { get; set; }

            /// <summary>
            /// 缺失次数
            /// </summary>
            public int MissingCount { get; set; }

            /// <summary>
            /// 首次缺失时间
            /// </summary>
            public DateTime FirstMissingTime { get; set; }

            /// <summary>
            /// 最后缺失时间
            /// </summary>
            public DateTime LastMissingTime { get; set; }
        }

        #endregion
    }
}
