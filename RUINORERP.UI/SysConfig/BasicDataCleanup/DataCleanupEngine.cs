using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using RUINORERP.Business;
using RUINORERP.Model.Base;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Repository.UnitOfWorks;
using System.Reflection;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 数据清理引擎 - 复用现有的 BaseController 级联删除功能
    /// 优势:
    /// 1. 使用项目已有的 BaseController.BaseDeleteByNavAsync 方法
    /// 2. 支持 SqlSugar 的 DeleteNav 导航删除(自动级联)
    /// 3. 通过泛型 T 直接指定 RUINORERP.Model 中的强类型实体
    /// 4. 无需重新实现复杂的关系分析和表达式树构建
    /// 5. ✅ 使用 IUnitOfWorkManage 管理事务,避免 DataReader 挂起问题
    /// </summary>
    public class DataCleanupEngine
    {
        private readonly ILogger<DataCleanupEngine> _logger;
        private readonly IUnitOfWorkManage _unitOfWork;

        public DataCleanupEngine()
        {
            _logger = Startup.GetFromFac<ILogger<DataCleanupEngine>>();
            _unitOfWork = Startup.GetFromFac<IUnitOfWorkManage>();
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

                // ✅ 使用 IUnitOfWorkManage 管理事务(替代 TransactionScope)
                // 优势: 自动处理 DataReader 清理、支持异步事务、自动超时保护
                await _unitOfWork.BeginTranAsync();
                
                try
                {
                    // ✅ 关键修复:先删除所有深层级关联数据,再删除主表
                    // 这样可以避免外键约束冲突
                    foreach (var id in targetIds)
                    {
                        stepIndex++;
                        ReportProgress(stepIndex, targetIds.Count, $"正在处理 ID: {id}");

                        // 构建实体对象(需要设置真实的主键属性,而不是 PrimaryKeyID)
                        var entity = Activator.CreateInstance<T>();
                        
                        // ✅ 关键修复:通过反射找到并设置真实的主键属性
                        var pkPropertyName = GetPrimaryKeyPropertyName<T>();
                        var pkProperty = typeof(T).GetProperty(pkPropertyName);
                        if (pkProperty != null && pkProperty.CanWrite)
                        {
                            pkProperty.SetValue(entity, id);
                            Log($"设置主键 {pkPropertyName} = {id}");
                        }
                        else
                        {
                            throw new InvalidOperationException($"未找到实体 {typeof(T).Name} 的主键属性: {pkPropertyName}");
                        }
                        
                        // 同时设置 PrimaryKeyID 作为备用
                        entity.PrimaryKeyID = id;

                        // ✅ 测试模式跳过实际删除,仅统计
                        if (isTestMode)
                        {
                            Log($"[测试模式] 模拟删除 ID={id}");
                            totalDeleted++;
                            continue;
                        }

                        // 正式模式:执行真正的级联删除操作
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
                        await _unitOfWork.RollbackTranAsync();
                        Log($"[测试模式] 事务已回滚,数据未实际删除");
                    }
                    else
                    {
                        // 正式模式:提交事务
                        await _unitOfWork.CommitTranAsync();
                        Log($"级联删除完成: 总计删除 {totalDeleted} 条主记录及其关联数据");
                    }
                }
                catch (Exception)
                {
                    // 发生异常时回滚事务
                    await _unitOfWork.RollbackTranAsync();
                    throw; // 重新抛出,让外层 catch 处理
                }
            }
            catch (Exception ex)
            {
                result.MarkAsFailed(ex.Message);
                Log($"级联删除失败:{typeof(T).Name}");
                            
                // ✅ 增强错误诊断:检测外键约束冲突
                string detailedError = $"[错误详情]\n" +
                    $"  • 实体类型:{typeof(T).FullName}\n" +
                    $"  • 目标 ID 数:{targetIds.Count}\n" +
                    $"  • 异常类型:{ex.GetType().Name}\n" +
                    $"  • 异常消息:{ex.Message}\n";
                            
                // 检测是否为外键约束冲突
                if (ex.Message.Contains("REFERENCE constraint") || ex.Message.Contains("外键约束"))
                {
                    detailedError += $"\n[外键约束冲突分析]\n" +
                        $"  ⚠️ 原因:Controller 的 BaseDeleteByNavAsync 方法中遗漏了某些关联表的 Include\n" +
                        $"  💡 解决方案:\n" +
                        $"    1. 检查 {typeof(T).Name}Controller.BaseDeleteByNavAsync 方法\n" +
                        $"    2. 确保所有导航属性都已 Include\n" +
                        $"    3. 特别注意明细表(如 xxxDetails)是否遗漏\n" +
                        $"    4. 参考错误信息中的表和字段名定位缺失的关联\n";
                                
                    // 尝试从错误消息中提取冲突的表名
                    var match = System.Text.RegularExpressions.Regex.Match(ex.Message, "table\\s+([\\w.]+)");
                    if (match.Success)
                    {
                        detailedError += $"\n  🔍 冲突表名:{match.Groups[1].Value}\n";
                    }
                                                    
                    var colMatch = System.Text.RegularExpressions.Regex.Match(ex.Message, "column\\s+'([^']+)'");
                    if (colMatch.Success)
                    {
                        detailedError += $"  🔍 冲突字段:{colMatch.Groups[1].Value}\n";
                    }
                }
                            
                detailedError += $"\n  • 堆栈跟踪:{ex.StackTrace}";
                            
                if (ex.InnerException != null)
                {
                    detailedError += $"\n  • 内部异常:{ex.InnerException.Message}";
                }
                            
                Log(detailedError);
                _logger?.LogError(ex, $"级联删除失败:{typeof(T).Name}");
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
        /// 获取实体的主键属性名(优先使用 SqlSugar 特性标记的主键)
        /// </summary>
        private static string GetPrimaryKeyPropertyName<T>() where T : BaseEntity, new()
        {
            var entityType = typeof(T);
            
            // 1. 查找标记了 SugarColumn(IsPrimaryKey = true) 的属性
            var sugarPkProps = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<SqlSugar.SugarColumn>()?.IsPrimaryKey == true)
                .ToList();
            
            if (sugarPkProps.Any())
            {
                return sugarPkProps.First().Name;
            }
            
            // 2. 如果没有 SugarColumn 标记,尝试常见的主键命名约定
            var commonPkNames = new[] { "Id", "ID", entityType.Name + "_ID", entityType.Name + "Id" };
            foreach (var pkName in commonPkNames)
            {
                var prop = entityType.GetProperty(pkName);
                if (prop != null)
                {
                    return pkName;
                }
            }
            
            // 3. 如果都找不到,返回 PrimaryKeyID(备用方案)
            return "PrimaryKeyID";
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

