using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.PacketSpec.Validation
{
    /// <summary>
    /// 命令验证服务
    /// </summary>
    public class ValidationService
    {
        private static readonly ConcurrentDictionary<Type, IValidator> _validators = new ConcurrentDictionary<Type, IValidator>();
        private static ValidationService _instance;
        private static readonly object _lock = new object();

        private ValidationService()
        {
        }

        /// <summary>
        /// 获取验证服务实例
        /// </summary>
        public static ValidationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ValidationService();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 注册验证器
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        /// <param name="validator">验证器实例</param>
        public void RegisterValidator<TCommand>(IValidator<TCommand> validator)
            where TCommand : ICommand
        {
            _validators[typeof(TCommand)] = validator;
        }

        /// <summary>
        /// 验证命令
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <returns>验证结果</returns>
        public async Task<ValidationResult> ValidateAsync(ICommand command)
        {
            if (command == null)
                return new ValidationResult(new[] { new ValidationFailure("", "命令不能为空") });

            var commandType = command.GetType();
            if (_validators.TryGetValue(commandType, out var validator))
            {
                // 使用反射调用适当的验证方法
                var validateMethod = validator.GetType().GetMethod("ValidateAsync", new[] { commandType, typeof(System.Threading.CancellationToken) });
                if (validateMethod != null)
                {
                    var result = await (Task<ValidationResult>)validateMethod.Invoke(validator, new object[] { command, System.Threading.CancellationToken.None });
                    return result;
                }
            }

            // 如果没有找到特定的验证器，则使用默认验证
            return await ValidateDefaultAsync(command);
        }

        /// <summary>
        /// 默认验证方法
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <returns>验证结果</returns>
        private async Task<ValidationResult> ValidateDefaultAsync(ICommand command)
        {
            var failures = new System.Collections.Generic.List<ValidationFailure>();

            // 基本验证
            if (command.CommandIdentifier != 0)
            {
                failures.Add(new ValidationFailure(command.ToString(), "命令ID不能为空"));
            }

            if (command.CommandIdentifier == default(CommandId))
            {
                failures.Add(new ValidationFailure(nameof(command.CommandIdentifier), "命令标识符不能为默认值"));
            }

            if (command.TimeoutMs <= 0)
            {
                failures.Add(new ValidationFailure(nameof(command.TimeoutMs), "超时时间必须大于0"));
            }

            return await Task.FromResult(new ValidationResult(failures));
        }
    }
}
