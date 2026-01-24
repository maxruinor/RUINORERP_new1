using System;
using System.Collections.Generic;
using System.IO;

namespace RUINORERP.Server.Configuration
{
    /// <summary>
    /// 定时任务配置管理类
    /// 集中管理所有工作流的执行时间点
    /// </summary>
    public class ScheduledTaskConfiguration
    {
        /// <summary>
        /// 默认配置文件路径
        /// </summary>
        private const string DefaultConfigFileName = "ScheduledTasks.json";

        /// <summary>
        /// 配置缓存
        /// </summary>
        private static ScheduledTaskConfiguration _instance;

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigFilePath { get; set; }

        /// <summary>
        /// 定时任务列表
        /// </summary>
        public List<ScheduledTask> Tasks { get; set; } = new List<ScheduledTask>();

        /// <summary>
        /// 私有构造函数，实现单例模式
        /// </summary>
        private ScheduledTaskConfiguration()
        {
            // 默认配置文件路径
            ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultConfigFileName);
        }

        /// <summary>
        /// 获取配置实例
        /// </summary>
        /// <returns>配置实例</returns>
        public static ScheduledTaskConfiguration GetInstance()
        {
            if (_instance == null)
            {
                _instance = LoadOrCreate();
            }
            return _instance;
        }

        /// <summary>
        /// 从文件加载配置，如果文件不存在则创建默认配置
        /// </summary>
        /// <returns>配置实例</returns>
        public static ScheduledTaskConfiguration LoadOrCreate()
        {
            var config = new ScheduledTaskConfiguration();
            var configPath = config.ConfigFilePath;

            if (File.Exists(configPath))
            {
                try
                {
                    var json = File.ReadAllText(configPath);
                    config = System.Text.Json.JsonSerializer.Deserialize<ScheduledTaskConfiguration>(json,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            WriteIndented = true
                        });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"加载定时任务配置失败: {ex.Message}");
                    config = CreateDefault();
                }
            }
            else
            {
                config = CreateDefault();
                config.Save();
            }

            _instance = config;
            return config;
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <returns>默认配置</returns>
        public static ScheduledTaskConfiguration CreateDefault()
        {
            return new ScheduledTaskConfiguration
            {
                Tasks = new List<ScheduledTask>
                {
                    // 库存快照 - 凌晨3点
                    new ScheduledTask
                    {
                        Id = "InventorySnapshot",
                        Name = "库存快照",
                        Description = "每日生成库存快照并清理过期快照",
                        ExecutionTime = "01:00:00",
                        Enabled = true,
                        WorkflowId = "InventorySnapshotWorkflow"
                    },
                    // 文件清理 - 凌晨3点
                    new ScheduledTask
                    {
                        Id = "FileCleanup",
                        Name = "文件清理",
                        Description = "清理过期文件、孤立文件和物理孤立文件",
                        ExecutionTime = "02:00:00",
                        Enabled = true,
                        WorkflowId = "FileCleanupWorkflow"
                    },
                    // 临时图片清理 - 凌晨3点
                    new ScheduledTask
                    {
                        Id = "TempImageCleanup",
                        Name = "临时图片清理",
                        Description = "清理上传目录中的临时图片",
                        ExecutionTime = "03:00:00",
                        Enabled = true,
                        WorkflowId = "TempImageCleanupWorkflow"
                    },
                    // 安全库存计算 - 凌晨2点
                    new ScheduledTask
                    {
                        Id = "SafetyStockCalculation",
                        Name = "安全库存计算",
                        Description = "计算所有产品的安全库存和预警库存",
                        ExecutionTime = "04:00:00",
                        Enabled = true,
                        WorkflowId = "SafetyStockWorkflow"
                    },
                    // 提醒检查 - 每分钟
                    new ScheduledTask
                    {
                        Id = "ReminderCheck",
                        Name = "提醒检查",
                        Description = "检查需要提醒的业务数据并触发提醒工作流",
                        ExecutionTime = "00:01:00",
                        IntervalType = IntervalType.Recurring,
                        Enabled = true
                    }
                }
            };
        }

        /// <summary>
        /// 保存配置到文件
        /// </summary>
        public void Save()
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(this,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                    });
                File.WriteAllText(ConfigFilePath, json);
                System.Diagnostics.Debug.WriteLine($"定时任务配置已保存到: {ConfigFilePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存定时任务配置失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 根据任务ID获取执行时间
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>执行时间字符串</returns>
        public string GetExecutionTime(string taskId)
        {
            var task = Tasks.Find(t => t.Id == taskId);
            return task?.ExecutionTime ?? "00:00:00";
        }

        /// <summary>
        /// 根据任务ID获取TimeSpan
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>执行时间TimeSpan</returns>
        public TimeSpan GetExecutionTimeSpan(string taskId)
        {
            var timeString = GetExecutionTime(taskId);
            if (TimeSpan.TryParse(timeString, out var timeSpan))
            {
                return timeSpan;
            }
            return TimeSpan.Zero;
        }

        /// <summary>
        /// 设置任务执行时间
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="executionTime">执行时间</param>
        public void SetExecutionTime(string taskId, string executionTime)
        {
            var task = Tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.ExecutionTime = executionTime;
                Save();
            }
        }

        /// <summary>
        /// 设置任务启用状态
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="enabled">是否启用</param>
        public void SetTaskEnabled(string taskId, bool enabled)
        {
            var task = Tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.Enabled = enabled;
                Save();
            }
        }

        /// <summary>
        /// 重新加载配置
        /// </summary>
        public void Reload()
        {
            _instance = LoadOrCreate();
        }
    }

    /// <summary>
    /// 定时任务信息
    /// </summary>
    public class ScheduledTask
    {
        /// <summary>
        /// 任务ID（唯一标识）
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 执行时间（HH:mm:ss格式）
        /// 对于每日任务，表示每天的执行时间
        /// 对于循环任务，表示间隔时间
        /// </summary>
        public string ExecutionTime { get; set; }

        /// <summary>
        /// 间隔类型
        /// </summary>
        public IntervalType IntervalType { get; set; } = IntervalType.Daily;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 关联的工作流ID（可选）
        /// </summary>
        public string WorkflowId { get; set; }
    }

    /// <summary>
    /// 间隔类型
    /// </summary>
    public enum IntervalType
    {
        /// <summary>
        /// 每日执行
        /// </summary>
        Daily = 0,

        /// <summary>
        /// 循环执行（如每分钟）
        /// </summary>
        Recurring = 1
    }
}
