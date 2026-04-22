using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
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
        /// <param name="isTestMode">是否测试模式(执行但回滚,用于准确统计)</param>
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
                Log($"开始执行级联删除：{typeof(T).Name}, ID 数量：{targetIds.Count}, 测试模式：{isTestMode}");

                // 获取对应的 Controller (复用现有架构)
                var controller = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                if (controller == null)
                {
                    throw new InvalidOperationException($"未找到对应的 Controller: {typeof(T).Name}Controller");
                }

                int totalDeleted = 0;
                int stepIndex = 0; // 提升到 try 块开始位置，catch 块才能访问

                // 使用事务包装，测试模式下会回滚
                using (var scope = new System.Transactions.TransactionScope(
                    System.Transactions.TransactionScopeAsyncFlowOption.Enabled))
                {
                    // 逐个删除(每个都会触发 BaseDeleteByNavAsync 的级联删除)
                    foreach (var id in targetIds)
                    {
                        stepIndex++;
                        ReportProgress(stepIndex, targetIds.Count, $"正在处理 ID: {id}");

                        // 构建实体对象(只需要设置主键)
                        var entity = Activator.CreateInstance<T>();
                        entity.PrimaryKeyID = id;

                        // 无论测试还是正式模式,都执行真正的删除操作
                        // BaseDeleteByNavAsync 内部使用 SqlSugar 的 DeleteNav().Include().ExecuteCommandAsync()
                        // 会自动根据导航属性进行级联删除
                        bool success = await controller.BaseDeleteByNavAsync(entity);
                        
                        if (success)
                        {
                            // 统计实际删除的记录数(包括主记录和所有关联记录)
                            // 注意:这里需要通过查询前后对比来准确统计
                            totalDeleted++;
                            Log($"✓ 成功删除 ID={id} 及其关联数据");
                        }
                        else
                        {
                            Log($"✗ 删除 ID={id} 失败");
                        }
                    }

                    result.IsSuccess = true;
                    result.TotalDeletedCount = totalDeleted;
                    result.Complete();

                    if (isTestMode)
                    {
                        // 测试模式:回滚事务,不真正删除
                        Log($"[测试模式] 事务已回滚,数据未实际删除");
                        // 不调用 scope.Complete(),事务会自动回滚
                    }
                    else
                    {
                        // 正式模式:提交事务
                        scope.Complete();
                        Log($"级联删除完成: 总计删除 {totalDeleted} 条主记录及其关联数据");
                    }
                }
            }
            catch (Exception ex)
            {
                result.MarkAsFailed(ex.Message);
                Log($"级联删除失败：{ex.Message}");
                
                // 详细错误日志
                string detailedError = $"[错误详情]\n" +
                    $"  • 实体类型：{typeof(T).FullName}\n" +
                    $"  • 目标 ID 数：{targetIds.Count}\n" +
                    $"  • 异常类型：{ex.GetType().Name}\n" +
                    $"  • 异常消息：{ex.Message}\n" +
                    $"  • 堆栈跟踪：{ex.StackTrace}";
                
                if (ex.InnerException != null)
                {
                    detailedError += $"\n  • 内部异常：{ex.InnerException.Message}";
                }
                
                Log(detailedError);
                _logger?.LogError(ex, $"级联删除失败：{typeof(T).Name}");
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

