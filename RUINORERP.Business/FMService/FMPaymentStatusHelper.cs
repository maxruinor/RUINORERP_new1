using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.FMService
{

    public static class FMPaymentStatusHelper
    {
        // 通用状态判断 -------------------------------------

        /// <summary>是否基础状态（非扩展状态）</summary>
        public static bool IsBaseStatus(this Enum status)
            => status.GetType() == typeof(BaseFMPaymentStatus);

        /// <summary>是否是终态（不可修改状态）</summary>
        public static bool IsFinalStatus(this Enum status)
        {
            var statusValue = Convert.ToInt64(status);
            return (statusValue & (long)BaseFMPaymentStatus.已取消) != 0 ||
                   (statusValue & (long)BaseFMPaymentStatus.已冲销) != 0 ||
                   (statusValue & (long)PrePaymentStatus.全额核销) != 0 ||
                   (statusValue & (long)ARAPStatus.已结清) != 0 ||
                   (statusValue & (long)ARAPStatus.坏账) != 0;
        }

        /// <summary>
        /// 是否可编辑（草稿或流程终止前）
        /// </summary>
        public static bool IsEditable(this Enum status)
        {
            // 根据不同类型判断
            switch (status)
            {
                case BaseFMPaymentStatus baseStatus:
                    return baseStatus.HasFlag(BaseFMPaymentStatus.草稿) ||
                           baseStatus.HasFlag(BaseFMPaymentStatus.待审核);

                case PrePaymentStatus preStatus:
                    return !preStatus.IsFinalStatus();

                case ARAPStatus arapStatus:
                    return !arapStatus.IsFinalStatus() &&
                           !arapStatus.HasFlag(ARAPStatus.部分支付) &&
                           !arapStatus.HasFlag(ARAPStatus.坏账);

                case PaymentStatus payStatus:
                    return !payStatus.IsFinalStatus();

                default:
                    throw new ArgumentException("未知状态类型");
            }
        }

        // 新增：判断支付记录能否取消
        /// <summary>是否允许取消操作</summary>
        public static bool CanCancel(this Enum status, bool hasRelatedRecords)
        {
            // 终态不可取消
            if (status.IsFinalStatus()) return false;

            // 根据不同类型判断
            switch (status)
            {
                case BaseFMPaymentStatus baseStatus:
                    return baseStatus.HasFlag(BaseFMPaymentStatus.草稿) ||
                           baseStatus.HasFlag(BaseFMPaymentStatus.待审核);

                case PrePaymentStatus preStatus:
                    return !preStatus.HasFlag(PrePaymentStatus.部分核销) &&
                           !hasRelatedRecords;

                case ARAPStatus arapStatus:
                    return !arapStatus.HasFlag(ARAPStatus.部分支付) &&
                           !arapStatus.HasFlag(ARAPStatus.坏账);

                case PaymentStatus payStatus:
                    return !payStatus.HasFlag(PaymentStatus.已支付) &&
                           !hasRelatedRecords;

                default:
                    throw new ArgumentException("未知状态类型");
            }
        }

        // 预收付款状态扩展 --------------------------------
        /// <summary>是否生效可用状态</summary>
        public static bool IsPrePaymentActive(this PrePaymentStatus status)
            => status.HasFlag(BaseFMPaymentStatus.已生效) &&
               !status.IsFinalStatus();

        /// <summary>是否可核销（生效且未完全核销）</summary>
        public static bool CanSettlePrepayment(this PrePaymentStatus status)
            => status.HasFlag(BaseFMPaymentStatus.已生效) &&
               !status.HasFlag(PrePaymentStatus.全额核销);

        /// <summary>是否允许反冲销（已核销状态）</summary>
        public static bool AllowReverseSettle(this PrePaymentStatus status)
            => status.HasFlag(PrePaymentStatus.部分核销) ||
               status.HasFlag(PrePaymentStatus.全额核销);

        // 应收应付状态扩展 --------------------------------
        /// <summary>是否可发起支付（生效未结清）</summary>
        public static bool CanCreatePayment(this ARAPStatus status)
            => status.HasFlag(BaseFMPaymentStatus.已生效) &&
               !status.HasFlag(ARAPStatus.已结清) &&
               !status.HasFlag(ARAPStatus.坏账);

        /// <summary>是否允许标记坏账（生效且未结清）</summary>
        public static bool CanMarkBadDebt(this ARAPStatus status)
            => status.HasFlag(BaseFMPaymentStatus.已生效) &&
               !status.HasFlag(ARAPStatus.已结清);

        /// <summary>是否允许部分支付（非坏账状态）</summary>
        public static bool AllowPartialPayment(this ARAPStatus status)
            => status.HasFlag(BaseFMPaymentStatus.已生效) &&
               !status.HasFlag(ARAPStatus.坏账);

        // 收付款状态扩展 ----------------------------------
        /// <summary>是否可关联核销单（生效未核销）</summary>
        public static bool CanSettlePayment(this PaymentStatus status)
            => status.HasFlag(BaseFMPaymentStatus.已生效) &&
               !status.HasFlag(PaymentStatus.已支付);

        /// <summary>是否允许修改金额（未关联核销单）</summary>
        public static bool AllowAmountChange(this PaymentStatus status)
            => status.HasFlag(BaseFMPaymentStatus.已生效) &&
               !status.HasFlag(PaymentStatus.已支付);

        // 复合状态判断 ------------------------------------
        /// <summary>是否处于中间流程状态（非开始非结束）</summary>
        public static bool IsInProcess(this Enum status)
            => !status.HasFlag(BaseFMPaymentStatus.草稿) &&
               !status.IsFinalStatus();

        /// <summary>是否允许生成会计凭证</summary>
        public static bool CanGenerateVoucher(this Enum status)
        {
            return status switch
            {
                BaseFMPaymentStatus s => s.HasFlag(BaseFMPaymentStatus.已生效),
                PrePaymentStatus s => s.HasFlag(BaseFMPaymentStatus.已生效) &&
                                      !s.HasFlag(PrePaymentStatus.全额核销),
                ARAPStatus s => s.HasFlag(BaseFMPaymentStatus.已生效) &&
                               !s.HasFlag(ARAPStatus.已结清),
                PaymentStatus s => s.HasFlag(BaseFMPaymentStatus.已生效) &&
                                   !s.HasFlag(PaymentStatus.已支付),
                _ => false
            };
        }

        // 状态机验证 --------------------------------------
        public static void ValidateTransition(Enum current, Enum target, bool hasRelatedRecords = false)
        {
            if (current.IsFinalStatus())
                throw new InvalidOperationException("终态单据禁止状态变更");

            // 通用取消规则
            if (target.HasFlag(BaseFMPaymentStatus.已取消))
            {
                if (!current.CanCancel(hasRelatedRecords))
                    throw new InvalidOperationException("当前状态不可取消");
            }

            // 类型特定规则
            switch (current)
            {
                case PrePaymentStatus preCurrent when target is PrePaymentStatus preTarget:
                    if (preCurrent.HasFlag(PrePaymentStatus.全额核销) &&
                        preTarget.HasFlag(PrePaymentStatus.部分核销))
                        throw new InvalidOperationException("已全额核销不可回退");
                    break;

                case ARAPStatus arapCurrent when target is ARAPStatus arapTarget:
                    if (arapCurrent.HasFlag(ARAPStatus.坏账) &&
                        !arapTarget.HasFlag(ARAPStatus.坏账))
                        throw new InvalidOperationException("坏账状态不可移除");

                    if (arapTarget.HasFlag(ARAPStatus.已结清) &&
                        arapCurrent.HasFlag(ARAPStatus.部分支付))
                        throw new InvalidOperationException("部分支付需先完成结算");
                    break;

                case PaymentStatus payCurrent when target is PaymentStatus payTarget:
                    if (payCurrent.HasFlag(PaymentStatus.已支付) &&
                        !payTarget.HasFlag(PaymentStatus.已支付))
                        throw new InvalidOperationException("已核销状态不可逆");
                    break;
            }

            // 防止跨类型转换
            if (current.GetType() != target.GetType())
                throw new InvalidOperationException("禁止跨状态类型转换");
        }

        public static void CheckStatusConflict(this Enum status)
        {
            var statusValue = Convert.ToInt64(status);

            // 基础状态互斥检测（兼容所有.NET版本）
            var baseFlags = statusValue & 0xFF;
            if (CountBits(baseFlags) > 1)
            {
                throw new InvalidDataException("基础状态存在冲突");
            }

            // 类型专属冲突检测
            switch (status)
            {
                case PrePaymentStatus pre:
                    if (pre.HasFlag(PrePaymentStatus.部分核销) &&
                        pre.HasFlag(PrePaymentStatus.全额核销))
                        throw new InvalidDataException("核销状态冲突");
                    break;

                case ARAPStatus arap:
                    if (arap.HasFlag(ARAPStatus.已结清) &&
                        (arap.HasFlag(ARAPStatus.部分支付) || arap.HasFlag(ARAPStatus.坏账)))
                        throw new InvalidDataException("已结清状态冲突");
                    break;

                case PaymentStatus pay:
                    if (pay.HasFlag(PaymentStatus.已冲销) &&
                        pay.HasFlag(PaymentStatus.已支付))
                        throw new InvalidDataException("冲销与核销状态冲突");
                    break;
            }
        }

        // 手动实现位计数（兼容旧版本）
        private static int CountBits(long value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1; // 清除最低位的1
            }
            return count;
        }

        // 获取状态描述
        public static string GetDescription(this Enum status)
        {
            var field = status.GetType().GetField(status.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .Cast<DescriptionAttribute>()
                                 .FirstOrDefault();
            return attribute?.Description ?? status.ToString();
        }
    }
}
