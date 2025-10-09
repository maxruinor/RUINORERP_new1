using FluentValidation;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Errors;
using System;

namespace RUINORERP.PacketSpec.Validation
{
    /// <summary>
    /// 请求基类验证器
    /// </summary>
    public class RequestBaseValidator : AbstractValidator<RequestBase>
    {
        public RequestBaseValidator()
        {
            // 请求ID验证
            RuleFor(request => request.RequestId)
                .NotEmpty()
                .WithMessage("请求ID不能为空")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString())
                .Length(1, 50)
                .WithMessage("请求ID长度必须在1-50个字符之间")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());

            // 操作类型验证
            RuleFor(request => request.OperationType)
                .NotEmpty()
                .WithMessage("操作类型不能为空")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString())
                .Length(1, 100)
                .WithMessage("操作类型长度必须在1-100个字符之间")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());

            // 客户端信息验证
            RuleFor(request => request.ClientInfo)
                .MaximumLength(500)
                .WithMessage("客户端信息长度不能超过500个字符")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                .When(request => !string.IsNullOrEmpty(request.ClientInfo));

            // 时间戳验证
            RuleFor(request => request.TimestampUtc)
                .NotEqual(default(DateTime))
                .WithMessage("时间戳不能为默认值")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                .Must((request, timestamp) => timestamp <= DateTime.UtcNow.AddMinutes(5))
                .WithMessage("时间戳不能超前当前时间超过5分钟")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                .Must((request, timestamp) => timestamp >= DateTime.UtcNow.AddHours(-24))
                .WithMessage("时间戳不能早于当前时间超过24小时")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
        }
    }

    /// <summary>
    /// 实体请求验证器
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class EntityRequestValidator<TEntity> : AbstractValidator<RequestBase<TEntity>>
        where TEntity : class
    {
        public EntityRequestValidator()
        {
            // 继承基类验证规则
            RuleFor(request => (RequestBase)request)
                .SetValidator(new RequestBaseValidator());

            // 操作类型验证
            RuleFor(request => request.OperationType)
                .IsInEnum()
                .WithMessage("无效的操作类型")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());

            // 根据操作类型验证不同字段
            When(request => request.OperationType == EntityOperationType.Create, () =>
            {
                RuleFor(request => request.Entity)
                    .NotNull()
                    .WithMessage("创建操作时实体数据不能为空")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
            });

            When(request => request.OperationType == EntityOperationType.Update, () =>
            {
                RuleFor(request => request.Entity)
                    .NotNull()
                    .WithMessage("更新操作时实体数据不能为空")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());

                RuleFor(request => request.EntityId)
                    .NotEmpty()
                    .WithMessage("更新操作时实体ID不能为空")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                    .Length(1, 100)
                    .WithMessage("实体ID长度必须在1-100个字符之间")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
            });

            When(request => request.OperationType == EntityOperationType.Delete, () =>
            {
                RuleFor(request => request.EntityId)
                    .NotEmpty()
                    .WithMessage("删除操作时实体ID不能为空")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                    .Length(1, 100)
                    .WithMessage("实体ID长度必须在1-100个字符之间")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
            });

            When(request => request.OperationType == EntityOperationType.Get, () =>
            {
                RuleFor(request => request.EntityId)
                    .NotEmpty()
                    .WithMessage("获取操作时实体ID不能为空")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                    .Length(1, 100)
                    .WithMessage("实体ID长度必须在1-100个字符之间")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
            });

            When(request => request.OperationType == EntityOperationType.List || 
                         request.OperationType == EntityOperationType.Custom, () =>
            {
                RuleFor(request => request.QueryParameters)
                    .NotNull()
                    .WithMessage("查询参数不能为空")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
            });

            // 数据版本号验证（用于乐观锁）
            When(request => !string.IsNullOrEmpty(request.DataVersion), () =>
            {
                RuleFor(request => request.DataVersion)
                    .Length(1, 50)
                    .WithMessage("数据版本号长度必须在1-50个字符之间")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
            });
        }
    }

    /// <summary>
    /// 批量实体请求验证器
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class BatchEntityRequestValidator<TEntity> : AbstractValidator<BatchEntityRequest<TEntity>>
        where TEntity : class
    {
        public BatchEntityRequestValidator()
        {
            // 继承基类验证规则
            RuleFor(request => (RequestBase)request)
                .SetValidator(new RequestBaseValidator());

            // 操作类型验证
            RuleFor(request => request.OperationType)
                .IsInEnum()
                .WithMessage("无效的操作类型")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString())
                .Must(operationType => operationType == EntityOperationType.BatchCreate || 
                                       operationType == EntityOperationType.BatchUpdate || 
                                       operationType == EntityOperationType.BatchDelete)
                .WithMessage("批量操作只支持批量创建、更新和删除")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());

            // 实体列表验证
            When(request => request.OperationType == EntityOperationType.BatchCreate || 
                         request.OperationType == EntityOperationType.BatchUpdate, () =>
            {
                RuleFor(request => request.Entities)
                    .NotNull()
                    .WithMessage("实体列表不能为空")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                    .Must(entities => entities != null && entities.Count > 0)
                    .WithMessage("实体列表必须包含至少一个实体")
                    .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                    .Must(entities => entities != null && entities.Count <= 1000)
                    .WithMessage("实体列表数量不能超过1000个")
                    .WithErrorCode(UnifiedErrorCodes.Biz_LimitExceeded.Code.ToString());
            });

            // 事务处理标志验证
            RuleFor(request => request.UseTransaction)
                .Must((request, useTransaction) => useTransaction || request.OperationType == EntityOperationType.BatchDelete)
                .WithMessage("批量删除操作必须使用事务处理")
                .WithErrorCode(UnifiedErrorCodes.Biz_StateError.Code.ToString());
        }
    }
}