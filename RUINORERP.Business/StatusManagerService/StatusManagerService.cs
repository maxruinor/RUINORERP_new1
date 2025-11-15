using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Global;
using System.ComponentModel;
using RUINORERP.Common.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
namespace RUINORERP.Business.StatusManagerService
{
    /// <summary>
    /// 业务单据状态管理帮助类
    /// 处理数据状态、审批状态和操作权限的逻辑关系
    /// </summary>
    public static class StatusManagerService
    {
        #region 基础状态判断

        /// <summary>
        /// 是否可编辑（草稿状态且未提交审批）
        /// </summary>
        public static bool IsEditable(DataStatus dataStatus, ApprovalStatus approvalStatus)
        {
            return dataStatus == DataStatus.草稿 && approvalStatus == ApprovalStatus.未审核;
        }

        /// <summary>
        /// 是否终态（已结案或已取消）
        /// </summary>
        public static bool IsFinalStatus(DataStatus dataStatus)
        {
            return dataStatus == DataStatus.完结 || dataStatus == DataStatus.作废;
        }

        #endregion

        #region 操作权限判断

        /// <summary>
        /// 是否允许新增操作（始终允许）
        /// </summary>
        public static bool CanCreate => true;

        /// <summary>
        /// 是否允许修改操作
        /// </summary>
        public static bool CanModify(DataStatus dataStatus, ApprovalStatus approvalStatus)
        {
            return IsEditable(dataStatus, approvalStatus);
        }

        /// <summary>
        /// 是否允许删除操作
        /// </summary>
        public static bool CanDelete(DataStatus dataStatus)
        {
            return dataStatus == DataStatus.草稿 || dataStatus == DataStatus.作废;
        }

        /// <summary>
        /// 是否允许提交操作
        /// </summary>
        public static bool CanSubmit(DataStatus dataStatus, ApprovalStatus approvalStatus)
        {
            return dataStatus == DataStatus.草稿 && approvalStatus == ApprovalStatus.未审核;
        }

        /// <summary>
        /// 是否允许审核操作
        /// </summary>
        public static bool CanApprove(DataStatus dataStatus, ApprovalStatus approvalStatus)
        {
            return dataStatus == DataStatus.新建 && approvalStatus == ApprovalStatus.未审核;
        }

        /// <summary>
        /// 是否允许反审操作
        /// </summary>
        public static bool CanReject(DataStatus dataStatus, ApprovalStatus approvalStatus)
        {
            return dataStatus == DataStatus.新建 && approvalStatus == ApprovalStatus.已审核;
        }

        /// <summary>
        /// 是否允许结案操作
        /// </summary>
        public static bool CanClose(DataStatus dataStatus, ApprovalStatus approvalStatus, bool approvalResult)
        {
            return dataStatus == DataStatus.确认 &&
                   approvalStatus == ApprovalStatus.已审核 &&
                   approvalResult; // 需要审批结果为通过
        }

        /// <summary>
        /// 是否允许取消操作
        /// </summary>
        public static bool CanCancel(DataStatus dataStatus)
        {
            return dataStatus == DataStatus.草稿 ||
                   dataStatus == DataStatus.新建 ||
                   dataStatus == DataStatus.确认;
        }

        #endregion

        #region 状态机转换验证

        /// <summary>
        /// 验证状态转换是否合法
        /// </summary>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <param name="targetDataStatus">目标数据状态</param>
        /// <param name="currentApprovalStatus">当前审批状态</param>
        /// <param name="targetApprovalStatus">目标审批状态</param>
        public static void ValidateTransition(
            DataStatus currentDataStatus,
            DataStatus targetDataStatus,
            ApprovalStatus currentApprovalStatus,
            ApprovalStatus targetApprovalStatus)
        {
            // 终态不能修改
            if (IsFinalStatus(currentDataStatus))
            {
                throw new InvalidOperationException("终态单据禁止状态变更");
            }

            // 状态转换规则
            switch (currentDataStatus)
            {
                case DataStatus.草稿:
                    ValidateDraftTransition(targetDataStatus, targetApprovalStatus);
                    break;

                case DataStatus.新建:
                    ValidateNewTransition(targetDataStatus, currentApprovalStatus, targetApprovalStatus);
                    break;

                case DataStatus.确认:
                    ValidateConfirmedTransition(targetDataStatus);
                    break;

                case DataStatus.完结:
                case DataStatus.作废:
                    // 终态已在前面检查
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(currentDataStatus), currentDataStatus, null);
            }
        }

        private static void ValidateDraftTransition(DataStatus targetDataStatus, ApprovalStatus targetApprovalStatus)
        {
            // 草稿只能转为新建
            if (targetDataStatus != DataStatus.新建 || targetApprovalStatus != ApprovalStatus.未审核)
            {
                throw new InvalidOperationException("草稿状态只能提交为新建状态");
            }
        }

        private static void ValidateNewTransition(DataStatus targetDataStatus,
            ApprovalStatus currentApprovalStatus,
            ApprovalStatus targetApprovalStatus)
        {
            // 新建状态的审批状态转换规则
            if (currentApprovalStatus == ApprovalStatus.未审核)
            {
                if (targetApprovalStatus != ApprovalStatus.已审核 &&
                    targetApprovalStatus != ApprovalStatus.驳回)
                {
                    throw new InvalidOperationException("新建状态审批必须转为已审核或驳回");
                }
            }

            // 数据状态只能转为确认或取消
            if (targetDataStatus != DataStatus.确认 && targetDataStatus != DataStatus.作废)
            {
                throw new InvalidOperationException("新建状态只能转为确认或取消");
            }
        }

        private static void ValidateConfirmedTransition(DataStatus targetDataStatus)
        {
            // 确认状态只能转为完结
            if (targetDataStatus != DataStatus.完结)
            {
                throw new InvalidOperationException("确认状态只能转为完结状态");
            }
        }

        #endregion

        #region UI状态获取

        /// <summary>
        /// 获取单据状态显示文本（组合数据状态和审批状态）
        /// </summary>
        public static string GetStatusText(DataStatus dataStatus, ApprovalStatus approvalStatus)
        {
            if (dataStatus == DataStatus.新建)
            {
                var approvalText = GetApprovalStatusText(approvalStatus);
                return $"{dataStatus.GetDescription()} ({approvalText})";
            }
            return dataStatus.GetDescription();
        }

        /// <summary>
        /// 获取审批状态显示文本
        /// </summary>
        public static string GetApprovalStatusText(ApprovalStatus approvalStatus)
        {
            return approvalStatus switch
            {
                ApprovalStatus.未审核 => "待审批",
                ApprovalStatus.已审核 => "审批通过",
                ApprovalStatus.驳回 => "审批驳回",
                _ => throw new ArgumentOutOfRangeException(nameof(approvalStatus), approvalStatus, null)
            };
        }

        #endregion

    }


  
    public class StatusTransitionRules
    {
        private static readonly Dictionary<DataStatus, HashSet<DataStatus>> _allowedTransitions =
            new Dictionary<DataStatus, HashSet<DataStatus>>
            {
            { DataStatus.草稿, new HashSet<DataStatus> { DataStatus.新建 } },
            { DataStatus.新建, new HashSet<DataStatus> { DataStatus.确认, DataStatus.作废 } },
            { DataStatus.确认, new HashSet<DataStatus> { DataStatus.完结, DataStatus.作废 } },
            { DataStatus.完结, new HashSet<DataStatus>() },
            { DataStatus.作废, new HashSet<DataStatus>() }
            };

        public static bool IsTransitionAllowed(DataStatus from, DataStatus to)
        {
            return _allowedTransitions.TryGetValue(from, out var allowed) &&
                   allowed.Contains(to);
        }
    }





    // 配置化的通知规则
    //public interface INotificationRuleRepository
    //{
    //   // IEnumerable<NotificationRule> GetRules(WorkflowEvent workflowEvent);
    //}

    public class NotificationRule
    {
        // public WorkflowEvent TriggerEvent { get; set; }
        public string TemplateId { get; set; }
        public string RecipientType { get; set; } // User/Group/Role
        public string RecipientValue { get; set; }
    }


}
