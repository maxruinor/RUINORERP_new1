using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Commands;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Common;

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
        public void RegisterValidator<TPacket>(IValidator<TPacket> validator)
            where TPacket : PacketModel
        {
            _validators[typeof(TPacket)] = validator;
        }

        /// <summary>
        /// 异步验证数据包
        /// </summary>
        /// <param name="packet">要验证的数据包对象</param>
        /// <returns>验证结果</returns>
        public async Task<ValidationResult> ValidateAsync(PacketModel packet)
        {
            if (packet == null)
                return new ValidationResult(new[] { new ValidationFailure("", "数据包不能为空") });

            var packetType = packet.GetType();
            if (_validators.TryGetValue(packetType, out var validator))
            {
                // 使用反射调用适当的验证方法
                var validateMethod = validator.GetType().GetMethod("ValidateAsync", new[] { packetType, typeof(System.Threading.CancellationToken) });
                if (validateMethod != null)
                {
                    var result = await (Task<ValidationResult>)validateMethod.Invoke(validator, new object[] { packet, System.Threading.CancellationToken.None });
                    return result;
                }
            }

            // 如果没有找到特定的验证器，则使用默认验证
            return await ValidateDefaultAsync(packet);
        }

        /// <summary>
        /// 默认验证方法
        /// </summary>
        /// <param name="packet">要验证的数据包对象</param>
        /// <returns>验证结果</returns>
        private async Task<ValidationResult> ValidateDefaultAsync(PacketModel packet)
        {
            var failures = new System.Collections.Generic.List<ValidationFailure>();

            // 基本验证
            if (packet.CommandId.FullCode == 0)
            {
                failures.Add(new ValidationFailure(packet.ToString(), "命令ID不能为空"));
            }

            if (packet.CommandId == default(CommandId))
            {
                failures.Add(new ValidationFailure(nameof(packet.CommandId.Name), "命令标识符不能为默认值"));
            }


            return await Task.FromResult(new ValidationResult(failures));
        }
    }
}
