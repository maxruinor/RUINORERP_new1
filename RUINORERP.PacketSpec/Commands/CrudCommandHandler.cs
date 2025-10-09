using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Requests;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Validation;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// CRUD命令处理器基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class CrudCommandHandler<TEntity> : GenericCommandHandler<CrudRequest<TEntity>, ResponseBase>
        where TEntity : class, new()
    {
        /// <summary>
        /// 支持的命令列表
        /// </summary>
        public new IReadOnlyList<uint> SupportedCommands { get; } = new List<uint>();
    
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<CrudCommandHandler<TEntity>> CrudLogger { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        protected CrudCommandHandler() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected CrudCommandHandler(ILogger<CrudCommandHandler<TEntity>> logger) : base(logger)
        {
            CrudLogger = logger;
        }

        /// <summary>
        /// 请求验证 - 验证CRUD请求的合法性
        /// </summary>
        protected override async Task<FluentValidation.Results.ValidationResult> ValidateRequestAsync(CrudRequest<TEntity> request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure(string.Empty, UnifiedErrorCodes.Biz_DataInvalid.Message)
                    {
                        ErrorCode = UnifiedErrorCodes.Biz_DataInvalid.Code.ToString()
                    }
                });
            }

            // 使用FluentValidation进行验证
            if (request is CrudRequest<TEntity> crudRequest)
            {
                // 创建EntityRequest<TEntity>来适配验证器
                var entityRequest = new EntityRequest<TEntity>
                {
                    Entity = crudRequest.Data,
                    EntityId = crudRequest.Id,
                    OperationType = ConvertOperationType(crudRequest.OperationType)
                };
                
                var validator = new EntityRequestValidator<TEntity>();
                var validationResult = await validator.ValidateAsync(entityRequest, cancellationToken);
                
                if (!validationResult.IsValid)
                {
                    return validationResult;
                }
            }

            return new FluentValidation.Results.ValidationResult();
        }

        /// <summary>
        /// 业务逻辑处理 - 根据操作类型分发到对应的CRUD方法
        /// </summary>
        protected override async Task<ResponseBase> HandleRequestAsync(CrudRequest<TEntity> request, CancellationToken cancellationToken)
        {
            CrudLogger?.LogInformation($"执行{request.OperationType}操作，实体类型：{typeof(TEntity).Name}");

            return request.OperationType switch
            {
                OperationType.Create => await CreateAsync(request.Data, cancellationToken),
                OperationType.Update => await UpdateAsync(request.Data, cancellationToken),
                OperationType.Delete => await DeleteAsync(request.Id, cancellationToken),
                OperationType.Get => await GetAsync(request.Id, cancellationToken),
                OperationType.GetList => await GetListAsync(request.QueryParameters, cancellationToken),
                _ => ResponseBase.CreateError($"不支持的操作类型: {request.OperationType}")
            };
        }

        /// <summary>
        /// 创建实体 - 必须由子类实现
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>创建结果</returns>
        protected abstract Task<ResponseBase<TEntity>> CreateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// 更新实体 - 必须由子类实现
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新结果</returns>
        protected abstract Task<ResponseBase<TEntity>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// 删除实体 - 必须由子类实现
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>删除结果</returns>
        protected abstract Task<ResponseBase<TEntity>> DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// 获取单个实体 - 必须由子类实现
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>查询结果</returns>
        protected abstract Task<ResponseBase<TEntity>> GetAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// 获取实体列表 - 可选实现，默认返回不支持
        /// </summary>
        /// <param name="queryParameters">查询参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>列表查询结果</returns>
        protected virtual Task<ResponseBase<TEntity>> GetListAsync(Dictionary<string, object> queryParameters, CancellationToken cancellationToken)
        {
            return Task.FromResult(ResponseBase<TEntity>.CreateError("列表查询功能未实现"));
        }

        /// <summary>
        /// 操作类型转换 - 将Crud操作类型转换为Entity操作类型
        /// </summary>
        /// <param name="operationType">CRUD操作类型</param>
        /// <returns>实体操作类型</returns>
        private static EntityOperationType ConvertOperationType(OperationType operationType)
        {
            return operationType switch
            {
                OperationType.Create => EntityOperationType.Create,
                OperationType.Update => EntityOperationType.Update,
                OperationType.Delete => EntityOperationType.Delete,
                OperationType.Get => EntityOperationType.Get,
                OperationType.GetList => EntityOperationType.List,
                _ => EntityOperationType.Custom
            };
        }
    }

    /// <summary>
    /// CRUD请求类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    [Serializable]
    public class CrudRequest<TEntity> : RequestBase
        where TEntity : class
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// 实体ID（用于删除和查询）
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 实体数据（用于创建和更新）
        /// </summary>
        public TEntity Data { get; set; }

        /// <summary>
        /// 查询参数（用于列表查询）
        /// </summary>
        public Dictionary<string, object> QueryParameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 创建CRUD请求
        /// </summary>
        public static CrudRequest<TEntity> Create(OperationType operationType, TEntity data = null, string id = null)
        {
            return new CrudRequest<TEntity>
            {
                OperationType = operationType,
                Data = data,
                Id = id
            };
        }
    }

    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create,

        /// <summary>
        /// 更新
        /// </summary>
        Update,

        /// <summary>
        /// 删除
        /// </summary>
        Delete,

        /// <summary>
        /// 获取单个
        /// </summary>
        Get,

        /// <summary>
        /// 获取列表
        /// </summary>
        GetList
    }
}