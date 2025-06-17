using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.FMService
{

    public static class FMPaymentStatusHelper
    {
        private const long BaseStatusMask = 0b1111; // 基础状态位掩码

        #region 通用状态判断
        /// <summary>是否处于中间流程状态</summary>
        public static bool IsInProcess(this Enum status)
            => !status.HasFlag(BaseFMStatus.草稿) && !status.IsFinalStatus();

        /// <summary>是否是终态（不可修改状态）</summary>
        public static bool IsFinalStatus(this Enum status)
        {
            long value = Convert.ToInt64(status);
            return (value & (long)BaseFMStatus.已关闭) != 0 ||
                   (value & (long)PrePaymentStatus.全额核销) != 0 ||
                   (value & (long)ARAPStatus.全部支付) != 0 ||
                   (value & (long)ARAPStatus.坏账) != 0;
        }

        /// <summary>是否可编辑（草稿或待审核状态）</summary>
        public static bool IsEditable(this Enum status)
        {
            long value = Convert.ToInt64(status);
            return (value & (long)(BaseFMStatus.草稿 | BaseFMStatus.待审核)) != 0;
        }

        /// <summary>是否允许取消操作</summary>
        public static bool CanCancel(this Enum status, bool hasRelatedRecords)
        {
            if (status.IsFinalStatus()) return false;

            return status switch
            {
                BaseFMStatus => true,
                PrePaymentStatus pre => !pre.HasFlag(PrePaymentStatus.部分核销) && !hasRelatedRecords,
                ARAPStatus arap => !arap.HasFlag(ARAPStatus.部分支付) && !arap.HasFlag(ARAPStatus.坏账),
                PaymentStatus pay => !pay.HasFlag(PaymentStatus.已支付) && !hasRelatedRecords,
                _ => throw new ArgumentException("未知状态类型")
            };
        }
        #endregion

        #region 状态特定方法
        public static bool IsPrePaymentActive(this PrePaymentStatus status)
            => status.HasFlag(PrePaymentStatus.已生效) && !status.IsFinalStatus();

        public static bool CanSettlePrepayment(this PrePaymentStatus status)
            => status.HasFlag(PrePaymentStatus.已生效) && !status.HasFlag(PrePaymentStatus.全额核销);

        public static bool CanCreatePayment(this ARAPStatus status)
            => status.HasFlag(ARAPStatus.已生效) &&
               !status.HasFlag(ARAPStatus.全部支付) &&
               !status.HasFlag(ARAPStatus.坏账);

        public static bool CanMarkBadDebt(this ARAPStatus status)
            => status.HasFlag(ARAPStatus.已生效) && !status.HasFlag(ARAPStatus.全部支付);

        public static bool CanSettlePayment(this PaymentStatus status)
            => status.HasFlag(PaymentStatus.已生效) && !status.HasFlag(PaymentStatus.已支付);

        public static bool AllowAmountChange(this PaymentStatus status)
            => status.HasFlag(PaymentStatus.已生效) && !status.HasFlag(PaymentStatus.已支付);
        #endregion

        #region 状态机验证
        public static void ValidateTransition(Enum current, Enum target)
        {
            if (current.IsFinalStatus())
                throw new InvalidOperationException("终态单据禁止状态变更");

            if (current.GetType() != target.GetType())
                throw new InvalidOperationException("禁止跨状态类型转换");

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
                    break;

                case PaymentStatus payCurrent when target is PaymentStatus payTarget:
                    if (payCurrent.HasFlag(PaymentStatus.已支付) &&
                        !payTarget.HasFlag(PaymentStatus.已支付))
                        throw new InvalidOperationException("已支付状态不可逆");
                    break;
            }
        }
        #endregion

        #region 状态冲突检测
        public static void CheckStatusConflict(this Enum status)
        {
            long value = Convert.ToInt64(status);
            CheckBaseStatusConflict(value);

            switch (status)
            {
                case PrePaymentStatus pre:
                    if (pre.HasFlag(PrePaymentStatus.部分核销) &&
                        pre.HasFlag(PrePaymentStatus.全额核销))
                        throw new InvalidDataException("预付款状态冲突：部分核销与全额核销不能共存");
                    break;

                case ARAPStatus arap:
                    if (arap.HasFlag(ARAPStatus.全部支付) &&
                        (arap.HasFlag(ARAPStatus.部分支付) || arap.HasFlag(ARAPStatus.坏账)))
                        throw new InvalidDataException("应收付状态冲突：已结清不能与部分支付/坏账共存");
                    break;
            }
        }

        private static void CheckBaseStatusConflict(long statusValue)
        {
            long baseStatus = statusValue & BaseStatusMask;
            if (CountBits(baseStatus) > 1)
                throw new InvalidDataException("基础状态存在冲突：多个基础状态同时生效");
        }

        private static int CountBits(long value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1;
            }
            return count;
        }
        #endregion

        #region 工具方法
        public static string GetDescription(this Enum status)
        {
            var field = status.GetType().GetField(status.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? status.ToString();
        }

        public static TStatus GetStatusValue<TStatus>(this BaseEntity entity) where TStatus : Enum
        {
            if (entity == null) return default;
            var statusProperty = typeof(TStatus).Name;
            if (entity.ContainsProperty(statusProperty))
            {
                object value = entity.GetPropertyValue(statusProperty);
                return (TStatus)Enum.ToObject(typeof(TStatus), value);
            }
            return default;
        }
        #endregion
    }
}
