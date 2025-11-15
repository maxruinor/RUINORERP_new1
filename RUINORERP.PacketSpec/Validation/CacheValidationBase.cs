using FluentValidation;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models.Cache;
using System;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Validation
{
    /// <summary>
    /// 缓存验证基类
    /// 提供统一的缓存请求和响应验证逻辑
    /// 所有缓存相关的服务类和处理类可以继承此类
    /// </summary>
    public abstract class CacheValidationBase
    {
        private readonly IValidator<CacheRequest> _cacheRequestValidator;
        private readonly IValidator<CacheResponse> _cacheResponseValidator;
        private readonly IValidator<CacheData> _cacheDataValidator;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected CacheValidationBase()
        {
            // 初始化验证器
            _cacheRequestValidator = new CacheRequestValidator();
            _cacheResponseValidator = new CacheResponseValidator();
            _cacheDataValidator = new CacheDataValidator();
        }

        /// <summary>
        /// 验证缓存请求（同步）
        /// </summary>
        /// <param name="request">缓存请求对象</param>
        /// <returns>验证结果</returns>
        protected ValidationResult ValidateCacheRequest(CacheRequest request)
        {
            return request.ValidateRequest(_cacheRequestValidator);
        }

        /// <summary>
        /// 验证缓存请求（异步）
        /// </summary>
        /// <param name="request">缓存请求对象</param>
        /// <returns>验证结果</returns>
        protected async Task<ValidationResult> ValidateCacheRequestAsync(CacheRequest request)
        {
            return await request.ValidateRequestAsync(_cacheRequestValidator);
        }

        /// <summary>
        /// 验证缓存响应
        /// </summary>
        /// <param name="response">缓存响应对象</param>
        /// <returns>验证结果</returns>
        protected ValidationResult ValidateCacheResponse(CacheResponse response)
        {
            if (response == null)
            {
                return new ValidationResult(new[]
                {
                    new ValidationFailure(string.Empty, "缓存响应对象不能为空")
                    {
                        ErrorCode = UnifiedErrorCodes.Biz_DataInvalid.Code.ToString()
                    }
                });
            }

            return _cacheResponseValidator.Validate(response);
        }

        /// <summary>
        /// 验证缓存数据
        /// </summary>
        /// <param name="cacheData">缓存数据对象</param>
        /// <returns>验证结果</returns>
        protected ValidationResult ValidateCacheData(CacheData cacheData)
        {
            if (cacheData == null)
            {
                return new ValidationResult(new[]
                {
                    new ValidationFailure(string.Empty, "缓存数据对象不能为空")
                    {
                        ErrorCode = UnifiedErrorCodes.Biz_DataInvalid.Code.ToString()
                    }
                });
            }

            return _cacheDataValidator.Validate(cacheData);
        }

        /// <summary>
        /// 验证表名是否有效
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>验证结果</returns>
        protected ValidationResult ValidateTableName(string tableName)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(tableName))
            {
                result.Errors.Add(new ValidationFailure("TableName", "表名不能为空")
                {
                    ErrorCode = UnifiedErrorCodes.Biz_DataInvalid.Code.ToString()
                });
            }
            else if (tableName.Length > 100)
            {
                result.Errors.Add(new ValidationFailure("TableName", "表名长度不能超过100个字符")
                {
                    ErrorCode = UnifiedErrorCodes.Biz_DataInvalid.Code.ToString()
                });
            }

            return result;
        }

        /// <summary>
        /// 检查验证结果是否有效，如果无效则抛出异常
        /// </summary>
        /// <param name="validationResult">验证结果</param>
        protected void EnsureValidationSuccess(ValidationResult validationResult)
        {
            if (validationResult == null || !validationResult.IsValid)
            {
                string errorMessage = validationResult?.GetValidationErrors() ?? "验证失败: 未知错误";
                throw new ValidationException(errorMessage, validationResult?.Errors);
            }
        }

        /// <summary>
        /// 创建验证失败的缓存响应
        /// </summary>
        /// <param name="validationResult">验证结果</param>
        /// <param name="operation">缓存操作类型</param>
        /// <returns>缓存响应对象</returns>
        protected CacheResponse CreateValidationErrorResponse(ValidationResult validationResult, CacheOperation operation)
        {
            string errorMessage = validationResult?.GetValidationErrors() ?? "验证失败: 未知错误";
            return CacheResponse.CreateError(errorMessage, UnifiedErrorCodes.Command_ValidationFailed.Code, operation);
        }
    }
}
