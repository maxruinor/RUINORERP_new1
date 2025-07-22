using RUINORERP.Common;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.DB;

namespace RUINORERP.Extensions.AOP
{
    /// <summary>
    /// 事务拦截器BlogTranAOP
    /// </summary>
    public class BlogTranAOP : IInterceptor
    {
        private readonly ILogger<BlogTranAOP> _logger;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private bool _isTransactionStartedByThisMethod = false;
        private UnitOfWorkManage.TransactionState _originalState;

        public BlogTranAOP(IUnitOfWorkManage unitOfWorkManage, ILogger<BlogTranAOP> logger)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            // 检查事务特性
            if (method.GetCustomAttribute<UseTranAttribute>(true) is { } uta)
            {
                HandleTransaction(invocation, method, uta);
            }
            else
            {
                invocation.Proceed(); // 直接执行无事务方法
            }
        }

        private void HandleTransaction(IInvocation invocation, MethodInfo method, UseTranAttribute uta)
        {
            try
            {
                // 保存原始事务状态
                _originalState = _unitOfWorkManage.GetTransactionState();
                _isTransactionStartedByThisMethod = false;

                // 根据传播行为处理事务
                HandlePropagationBehavior(method, uta.Propagation);

                // 执行实际方法
                invocation.Proceed();

                // 处理异步方法
                HandleAsyncMethod(invocation);

                // 提交事务（如果由此方法启动）
                CommitIfNeeded(method);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "事务处理异常");
                RollbackIfNeeded(method, ex);
                throw;
            }
            finally
            {
                // 恢复原始事务状态（如果未提交）
                RestoreOriginalStateIfNeeded();
            }
        }

        private void HandlePropagationBehavior(MethodInfo method, Propagation propagation)
        {
            switch (propagation)
            {
                case Propagation.Required:
                    HandleRequiredPropagation(method);
                    break;

                case Propagation.Mandatory:
                    HandleMandatoryPropagation(method);
                    break;

                case Propagation.Nested:
                    HandleNestedPropagation(method);
                    break;

                case Propagation.RequiresNew:
                    HandleRequiresNewPropagation(method);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(propagation), propagation, "不支持的事务传播行为");
            }
        }

        private void HandleRequiredPropagation(MethodInfo method)
        {
            if (_unitOfWorkManage.TranCount <= 0)
            {
                _logger.LogInformation($"[{method.Name}] 开启新事务 (Required)");
                _unitOfWorkManage.BeginTran();
                _isTransactionStartedByThisMethod = true;
            }
            else
            {
                _logger.LogDebug($"[{method.Name}] 加入现有事务 (Required)");
            }
        }

        private void HandleMandatoryPropagation(MethodInfo method)
        {
            if (_unitOfWorkManage.TranCount <= 0)
            {
                throw new InvalidOperationException($"[{method.Name}] 事务传播机制为Mandatory, 但当前不存在事务");
            }
            _logger.LogDebug($"[{method.Name}] 使用现有事务 (Mandatory)");
        }

        private void HandleNestedPropagation(MethodInfo method)
        {
            _logger.LogInformation($"[{method.Name}] 开启嵌套事务 (Nested)");
            _unitOfWorkManage.BeginTran();
            _isTransactionStartedByThisMethod = true;
        }

        private void HandleRequiresNewPropagation(MethodInfo method)
        {
            // 保存当前状态
            var currentState = _unitOfWorkManage.GetTransactionState();

            // 开启新事务
            _logger.LogInformation($"[{method.Name}] 开启全新事务 (RequiresNew)");
            _unitOfWorkManage.BeginTran();
            _isTransactionStartedByThisMethod = true;

            // 存储原始状态以便恢复
            _originalState = currentState;
        }

        private void HandleAsyncMethod(IInvocation invocation)
        {
            if (IsAsyncMethod(invocation.Method))
            {
                var result = invocation.ReturnValue;
                if (result is Task task)
                {
                    task.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            _logger.LogError(t.Exception, "异步方法执行失败");
                            RollbackIfNeeded(invocation.Method, t.Exception);
                        }
                    });
                }
            }
        }

        private void CommitIfNeeded(MethodInfo method)
        {
            if (_isTransactionStartedByThisMethod)
            {
                _logger.LogInformation($"[{method.Name}] 提交事务");
                _unitOfWorkManage.CommitTran();
            }
        }

        private void RollbackIfNeeded(MethodInfo method, Exception ex)
        {
            if (_isTransactionStartedByThisMethod)
            {
                _logger.LogError($"[{method.Name}] 回滚事务: {ex.Message}");
                _unitOfWorkManage.RollbackTran();
            }
            else if (_unitOfWorkManage.TranCount > 0)
            {
                _logger.LogWarning($"[{method.Name}] 标记事务需要回滚");
                // 在UnitOfWorkManage中添加此方法
                _unitOfWorkManage.MarkForRollback();
            }
        }

        private void RestoreOriginalStateIfNeeded()
        {
            // 仅RequiresNew传播需要恢复状态
            if (!_isTransactionStartedByThisMethod) return;

            try
            {
                _logger.LogDebug("恢复原始事务状态");
                _unitOfWorkManage.RestoreTransactionState(_originalState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "恢复事务状态失败");
            }
        }

        public static bool IsAsyncMethod(MethodInfo method)
        {
            return method.ReturnType == typeof(Task) ||
                   (method.ReturnType.IsGenericType &&
                    method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));
        }
    }
}