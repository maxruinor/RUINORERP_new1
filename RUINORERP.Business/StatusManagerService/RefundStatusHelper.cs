using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.StatusManagerService
{
    /// <summary>
    /// 销售退回单退款状态管理工具
    /// </summary>
    public static class RefundStatusHelper
    {
        /// <summary>是否为终态（不可修改状态）</summary>
        public static bool IsFinalStatus(RefundStatus status)
        {
            return status == RefundStatus.已退款未退货 ||
                   status == RefundStatus.已退款已退货;
        }

        /// <summary>是否可编辑（初始状态）</summary>
        public static bool CanModify(RefundStatus status)
        {
            return status == RefundStatus.未退款等待退货;
        }

        /// <summary>是否允许取消操作</summary>
        public static bool CanCancel(RefundStatus status, bool hasRelatedRecords)
        {
            // 终态不可取消
            if (IsFinalStatus(status)) return false;

            // 初始状态无关联记录时可取消
            return status == RefundStatus.未退款等待退货 && !hasRelatedRecords;
        }

        /// <summary>验证状态转换是否合法（异常方式）</summary>
        public static void ValidateTransition(RefundStatus current, RefundStatus target)
        {
            if (!TryValidateTransition(current, target, out var errorMessage))
            {
                throw new InvalidOperationException(errorMessage);
            }
        }

        /// <summary>尝试验证状态转换（布尔结果+错误信息）</summary>
        public static bool TryValidateTransition(
            RefundStatus current,
            RefundStatus target,
            out string errorMessage)
        {
            errorMessage = null;

            // 终态禁止转换
            if (IsFinalStatus(current))
            {
                errorMessage = "终态单据禁止状态变更";
                return false;
            }
            if (current == target)
            {
                //没有改变
                return true;
            }
            // 状态转换规则验证
            switch (current)
            {
                case RefundStatus.未退款等待退货:
                    if (target == RefundStatus.未退款已退货 ||
                        target == RefundStatus.已退款等待退货 ||
                        target == RefundStatus.已退款未退货)
                    {
                        return true;
                    }
                    errorMessage = "初始状态只能转换为未退款已退货、已退款等待退货或已退款未退货";
                    return false;

                case RefundStatus.未退款已退货:
                    if (target == RefundStatus.已退款已退货)
                    {
                        return true;
                    }
                    errorMessage = "未退款已退货状态只能转换为退款退货完成";
                    return false;

                case RefundStatus.已退款等待退货:
                    if (target == RefundStatus.已退款已退货 ||
                        target == RefundStatus.已退款未退货)
                    {
                        return true;
                    }
                    errorMessage = "已退款等待退货状态只能转换为退款退货完成或已退款未退货";
                    return false;

                default:
                    errorMessage = $"未知的当前状态: {current}";
                    return false;
            }
        }

        /// <summary>获取状态描述</summary>
        public static string GetDescription(RefundStatus status)
        {
            var field = typeof(RefundStatus).GetField(status.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? status.ToString();
        }

        /// <summary>获取允许的下一步状态列表</summary>
        public static IReadOnlyList<RefundStatus> GetAllowedTransitions(RefundStatus current)
        {
            return current switch
            {
                RefundStatus.未退款等待退货 => new[]
                {
                RefundStatus.未退款已退货,
                RefundStatus.已退款等待退货,
                RefundStatus.已退款未退货
            },
                RefundStatus.未退款已退货 => new[]
                {
                RefundStatus.已退款已退货
            },
                RefundStatus.已退款等待退货 => new[]
                {
                RefundStatus.已退款已退货,
                RefundStatus.已退款未退货
            },
                _ => Array.Empty<RefundStatus>()
            };
        }
    }
}

