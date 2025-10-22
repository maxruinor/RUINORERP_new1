using FluentValidation;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using System;

namespace RUINORERP.PacketSpec.Validation
{
    /// <summary>
    /// 缓存请求验证器
    /// </summary>
    public class CacheRequestValidator : AbstractValidator<CacheRequest>
    {
        public CacheRequestValidator()
        {
            // 继承基类验证规则
            RuleFor(request => (RequestBase)request)
                .SetValidator(new RequestBaseValidator());

            // 缓存操作类型验证
            RuleFor(request => request.Operation)
                .IsInEnum()
                .WithMessage("无效的缓存操作类型")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());

            // 根据操作类型验证表名
            When(request => 
                request.Operation == CacheOperation.Get || 
                request.Operation == CacheOperation.Remove ||
                request.Operation == CacheOperation.Set,
                () => {
                    RuleFor(request => request.TableName)
                        .NotEmpty()
                        .WithMessage("表名不能为空")
                        .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                        .Length(1, 100)
                        .WithMessage("表名长度必须在1-100个字符之间")
                        .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
                });

            // 订阅操作时的验证
            When(request => request.Operation == CacheOperation.Manage && 
                request.SubscribeAction != SubscribeAction.None,
                () => {
                    RuleFor(request => request.TableName)
                        .NotEmpty()
                        .WithMessage("订阅操作时表名不能为空")
                        .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
                    
                    RuleFor(request => request.SubscribeAction)
                        .IsInEnum()
                        .WithMessage("无效的订阅操作类型")
                        .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());
                });

            // 设置缓存时验证数据
            When(request => request.Operation == CacheOperation.Set,
                () => {
                    RuleFor(request => request.CacheData)
                        .NotNull()
                        .WithMessage("设置缓存时数据不能为空")
                        .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
                });

            // 根据主键操作时的验证
            When(request => !string.IsNullOrEmpty(request.PrimaryKeyName),
                () => {
                    RuleFor(request => request.PrimaryKeyValue)
                        .NotNull()
                        .WithMessage("主键名存在时主键值不能为空")
                        .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
                });
        }
    }

    /// <summary>
    /// 缓存响应验证器
    /// </summary>
    public class CacheResponseValidator : AbstractValidator<CacheResponse>
    {
        public CacheResponseValidator()
        {
            // 缓存操作类型验证
            RuleFor(response => response.Operation)
                .IsInEnum()
                .WithMessage("无效的缓存操作类型")
                .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());

            // 成功响应时的验证
            When(response => response.IsSuccess,
                () => {
                    // Get操作成功时必须返回数据
                    When(response => response.Operation == CacheOperation.Get,
                        () => {
                            RuleFor(response => response.CacheData)
                                .NotNull()
                                .WithMessage("获取缓存时返回的数据不能为空")
                                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
                        });

                    // 验证表名一致性
                    When(response => response.CacheData != null && 
                        !string.IsNullOrEmpty(response.CacheData.TableName),
                        () => {
                            RuleFor(response => response.TableName)
                                .Equal(response => response.CacheData.TableName)
                                .WithMessage("响应表名与缓存数据表名不一致")
                                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
                        });
                });

            // 错误响应时的验证
            When(response => !response.IsSuccess,
                () => {
                    RuleFor(response => response.Message)
                        .NotEmpty()
                        .WithMessage("错误响应必须包含错误消息")
                        .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());

                    RuleFor(response => response.ErrorCode)
                        .GreaterThan(0)
                        .WithMessage("错误响应必须包含错误代码")
                        .WithErrorCode(UnifiedErrorCodes.Command_InvalidFormat.Code.ToString());
                });

            // 验证时间戳
            RuleFor(response => response.CacheTime)
                .NotEqual(default(DateTime))
                .WithMessage("缓存时间不能为默认值")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());

            RuleFor(response => response.ExpirationTime)
                .GreaterThan(response => response.CacheTime)
                .WithMessage("过期时间必须大于缓存时间")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
        }
    }

    /// <summary>
    /// 缓存数据验证器
    /// </summary>
    public class CacheDataValidator : AbstractValidator<CacheData>
    {
        public CacheDataValidator()
        {
            // 表名验证
            RuleFor(data => data.TableName)
                .NotEmpty()
                .WithMessage("缓存数据表名不能为空")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                .Length(1, 100)
                .WithMessage("缓存数据表名长度必须在1-100个字符之间")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());

            // 时间戳验证
            RuleFor(data => data.CacheTime)
                .NotEqual(default(DateTime))
                .WithMessage("缓存创建时间不能为默认值")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());

            RuleFor(data => data.ExpirationTime)
                .GreaterThan(data => data.CacheTime)
                .WithMessage("缓存过期时间必须大于创建时间")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());

            // 版本验证
            RuleFor(data => data.Version)
                .NotEmpty()
                .WithMessage("缓存版本不能为空")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString())
                .Matches(@"^\d+\.\d+\.\d+(-\w+)?$")
                .WithMessage("缓存版本格式无效，应为X.Y.Z格式")
                .WithErrorCode(UnifiedErrorCodes.Biz_DataInvalid.Code.ToString());
        }
    }
}
