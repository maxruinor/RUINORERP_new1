using FluentValidation;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Errors;
using System;

namespace RUINORERP.PacketSpec.Validation
{
    /// <summary>
    /// 请求验证扩展方法
    /// </summary>
    public static class RequestValidationExtensions
    {
        /// <summary>
        /// 验证请求并返回验证结果
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="validator">验证器</param>
        /// <returns>验证结果</returns>
        public static ValidationResult ValidateRequest<TRequest>(this TRequest request, IValidator<TRequest> validator)
        {
            if (request == null)
            {
                return new ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure(string.Empty, "请求对象不能为空")
                    {
                        ErrorCode = UnifiedErrorCodes.Biz_DataInvalid.Code.ToString()
                    }
                });
            }

            if (validator == null)
            {
                return new ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure(string.Empty, "验证器未配置")
                    {
                        ErrorCode = UnifiedErrorCodes.System_InternalError.Code.ToString()
                    }
                });
            }

            return validator.Validate(request);
        }

        /// <summary>
        /// 验证请求并返回验证结果（异步）
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="validator">验证器</param>
        /// <returns>验证结果</returns>
        public static async System.Threading.Tasks.Task<ValidationResult> ValidateRequestAsync<TRequest>(this TRequest request, IValidator<TRequest> validator)
        {
            if (request == null)
            {
                return new ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure(string.Empty, "请求对象不能为空")
                    {
                        ErrorCode = UnifiedErrorCodes.Biz_DataInvalid.Code.ToString()
                    }
                });
            }

            if (validator == null)
            {
                return new ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure(string.Empty, "验证器未配置")
                    {
                        ErrorCode = UnifiedErrorCodes.System_InternalError.Code.ToString()
                    }
                });
            }

            return await validator.ValidateAsync(request);
        }

        /// <summary>
        /// 将验证结果转换为统一错误响应
        /// </summary>
        /// <param name="validationResult">验证结果</param>
        /// <returns>错误消息</returns>
        public static string GetValidationErrors(this ValidationResult validationResult)
        {
            if (validationResult == null || validationResult.IsValid)
            {
                return string.Empty;
            }

            var errors = new System.Text.StringBuilder();
            foreach (var error in validationResult.Errors)
            {
                if (errors.Length > 0)
                {
                    errors.Append("; ");
                }
                errors.Append($"{error.PropertyName}: {error.ErrorMessage}");
            }

            return errors.ToString();
        }

        /// <summary>
        /// 获取验证错误代码
        /// </summary>
        /// <param name="validationResult">验证结果</param>
        /// <returns>错误代码</returns>
        public static int GetValidationErrorCode(this ValidationResult validationResult)
        {
            if (validationResult == null || validationResult.IsValid)
            {
                return 0;
            }

            // 优先使用第一个错误的错误代码
            if (validationResult.Errors.Count > 0 && !string.IsNullOrEmpty(validationResult.Errors[0].ErrorCode))
            {
                if (int.TryParse(validationResult.Errors[0].ErrorCode, out int errorCode))
                {
                    return errorCode;
                }
            }

            // 默认返回验证失败错误码
            return UnifiedErrorCodes.Command_ValidationFailed.Code;
        }

        /// <summary>
        /// 验证请求是否有效
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="validator">验证器</param>
        /// <returns>验证是否通过</returns>
        public static bool IsValidRequest<TRequest>(this TRequest request, IValidator<TRequest> validator)
        {
            if (request == null || validator == null)
            {
                return false;
            }

            var result = validator.Validate(request);
            return result.IsValid;
        }

        /// <summary>
        /// 验证请求是否有效（异步）
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="validator">验证器</param>
        /// <returns>验证是否通过</returns>
        public static async System.Threading.Tasks.Task<bool> IsValidRequestAsync<TRequest>(this TRequest request, IValidator<TRequest> validator)
        {
            if (request == null || validator == null)
            {
                return false;
            }

            var result = await validator.ValidateAsync(request);
            return result.IsValid;
        }
    }
}