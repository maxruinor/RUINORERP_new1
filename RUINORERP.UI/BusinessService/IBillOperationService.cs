using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.UI.BusinessService
{

    /// <summary>
    /// 单据业务操作结果
    /// </summary>
    public class BillOperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public object OriginalStatus { get; set; } // 用于失败时恢复
    }

    /// <summary>
    /// 单据业务操作服务接口
    /// 统一处理提交、审核、反审核、结案等操作
    /// </summary>
    public interface IBillOperationService
    {
        #region 状态变更事件
        
        /// <summary>
        /// 单据状态变更事件
        /// </summary>
        event EventHandler<BillStatusChangedEventArgs> BillStatusChanged;

        /// <summary>
        /// 单据操作完成事件
        /// </summary>
        event EventHandler<BillOperationCompletedEventArgs> BillOperationCompleted;
        
        #endregion

        /// <summary>
        /// 提交单据
        /// </summary>
        Task<BillOperationResult> SubmitAsync<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 批量提交单据
        /// </summary>
        Task<BillOperationResult> SubmitBatchAsync<T>(List<T> entities) where T : BaseEntity;

        /// <summary>
        /// 审核单据
        /// </summary>
        Task<BillOperationResult> ReviewAsync<T>(T entity, ApprovalEntity approvalInfo) where T : BaseEntity;

        /// <summary>
        /// 批量审核单据
        /// </summary>
        Task<BillOperationResult> ReviewBatchAsync<T>(List<T> entities, ApprovalEntity approvalInfo) where T : BaseEntity;

        /// <summary>
        /// 反审核单据
        /// </summary>
        Task<BillOperationResult> ReReviewAsync<T>(T entity, ApprovalEntity approvalInfo) where T : BaseEntity;

        /// <summary>
        /// 结案单据
        /// </summary>
        Task<BillOperationResult> CloseCaseAsync<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 批量结案单据
        /// </summary>
        Task<BillOperationResult> CloseCaseBatchAsync<T>(List<T> entities) where T : BaseEntity;

        /// <summary>
        /// 反结案单据
        /// </summary>
        Task<BillOperationResult> AntiCloseCaseAsync<T>(T entity) where T : BaseEntity;
    }
}
