using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using RUINORERP.Business;
using RUINORERP.Model.Base;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 数据清理引擎 - 复用现有的 BaseController 级联删除功能
    /// 优势:
    /// 1. 使用项目已有的 BaseController.BaseDeleteByNavAsync 方法
    /// 2. 支持 SqlSugar 的 DeleteNav 导航删除(自动级联)
    /// 3. 通过泛型 T 直接指定 RUINORERP.Model 中的强类型实体
    /// 4. 无需重新实现复杂的关系分析和表达式树构建
    /// </summary>
    public class DataCleanupEngine
    {
        private readonly ILogger<DataCleanupEngine> _logger;

        public DataCleanupEngine()
        {
            _logger = Startup.GetFromFac<ILogger<DataCleanupEngine>>();
        }

        /// <summary>
        /// 执行级联删除 - 复用 BaseController.BaseDeleteByNavAsync
        /// </summary>
        /// <typeparam name="T">实体类型(来自RUINORERP.Model)</typeparam>
        /// <param name="targetIds">目标记录ID列表</param>
        /// <param name="isTestMode">是否测试模式(仅统计不删除)</param>
        /// <returns>清理结果</returns>
        public async Task<CascadeDeleteResult> ExecuteCascadeDeleteAsync<T>(List<long> targetIds, bool isTestMode = false) where T : BaseEntity, new()
        {
            var result = new CascadeDeleteResult
            {
                IsTestMode = isTestMode,
                StartTime = DateTime.Now
            };

            try
            {
                Log($"开始执行级联删除: {typeof(T).Name}, ID数量: {targetIds.Count}, 测试模式: {isTestMode}");

                // 获取对应的 Controller (复用现有架构)
                var controller = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                if (controller == null)
                {
                    throw new InvalidOperationException($"未找到对应的 Controller: {typeof(T).Name}Controller");
                }

                int totalDeleted = 0;
                int stepIndex = 0;

                // 逐个删除(每个都会触发 BaseDeleteByNavAsync 的级联删除)
                foreach (var id in targetIds)
                {
                    stepIndex++;
                    ReportProgress(stepIndex, targetIds.Count, $"正在处理 ID: {id}");

                    // 构建实体对象(只需要设置主键)
                    var entity = Activator.CreateInstance<T>();
                    entity.PrimaryKeyID = id;

                    if (isTestMode)
                    {
                        // 测试模式:只统计,不实际删除
                        Log($"[测试模式] 将删除 ID={id} 及其关联数据(通过导航属性自动级联)");
                        totalDeleted++; // 简化统计
                    }
                    else
                    {
                        // 正式模式:调用现有的级联删除方法
                        // BaseDeleteByNavAsync 内部使用 SqlSugar 的 DeleteNav().Include().ExecuteCommandAsync()
                        // 会自动根据导航属性进行级联删除
                        bool success = await controller.BaseDeleteByNavAsync(entity);
                        if (success)
                        {
                            totalDeleted++;
                            Log($"✓ 成功删除 ID={id} 及其关联数据");
                        }
                        else
                        {
                            Log($"✗ 删除 ID={id} 失败");
                        }
                    }
                }

                result.IsSuccess = true;
                result.TotalDeletedCount = totalDeleted;
                result.Complete();

                Log($"级联删除完成: 总计删除 {totalDeleted} 条主记录及其关联数据");
            }
            catch (Exception ex)
            {
                result.MarkAsFailed(ex.Message);
                Log($"级联删除失败: {ex.Message}");
                _logger?.LogError(ex, $"级联删除失败: {typeof(T).Name}");
                throw;
            }

            return result;
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        private void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            OnLog?.Invoke(this, logEntry);
            _logger?.LogInformation(message);
        }

        /// <summary>
        /// 报告进度
        /// </summary>
        private void ReportProgress(int current, int total, string message)
        {
            OnProgressChanged?.Invoke(this, new CleanupProgressEventArgs
            {
                Current = current,
                Total = total,
                Message = message,
                Percentage = total > 0 ? (int)((double)current / total * 100) : 0
            });
        }

        /// <summary>
        /// 日志事件
        /// </summary>
        public event EventHandler<string> OnLog;

        /// <summary>
        /// 进度变化事件
        /// </summary>
        public event EventHandler<CleanupProgressEventArgs> OnProgressChanged;
    }
}

