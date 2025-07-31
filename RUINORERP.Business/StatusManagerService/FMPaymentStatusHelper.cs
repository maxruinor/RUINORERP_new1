using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
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

namespace RUINORERP.Business.StatusManagerService
{
    public static class FMPaymentStatusHelper
    {

        /// <summary>获取实体的状态类型</summary>
        public static Type GetStatusType(BaseEntity entity)
        {
            if (entity.ContainsProperty(typeof(DataStatus).Name))
                return typeof(DataStatus);

            if (entity.ContainsProperty(typeof(PrePaymentStatus).Name))
                return typeof(PrePaymentStatus);

            if (entity.ContainsProperty(typeof(ARAPStatus).Name))
                return typeof(ARAPStatus);

            if (entity.ContainsProperty(typeof(PaymentStatus).Name))
                return typeof(PaymentStatus);

            return null;
        }


        /// <summary>是否是终态（不可修改状态）</summary>
        /// <param name="status">是状态枚举值类型</param>
        public static bool IsFinalStatus<T>(T status) where T : Enum
        {
            if (status is DataStatus dataStatus)
                return dataStatus == DataStatus.完结 ||
                       dataStatus == DataStatus.作废;


            if (status is PrePaymentStatus pre)
                return pre == PrePaymentStatus.全额核销 ||
                       pre == PrePaymentStatus.已退款;

            if (status is ARAPStatus arap)
                return arap == ARAPStatus.全部支付 ||
                       arap == ARAPStatus.坏账 ||
                       arap == ARAPStatus.已冲销;

            if (status is PaymentStatus pay)
                return pay == PaymentStatus.已支付;

            return false;
        }

        /// <summary>是否可编辑（草稿或待审核状态）</summary>
        public static bool CanModify<T>(T status) where T : Enum
        {
            if (status is DataStatus dataStatus)
                return dataStatus == DataStatus.草稿 ||
                       dataStatus == DataStatus.新建;

            if (status is PrePaymentStatus pre)
                return pre == PrePaymentStatus.草稿 ||
                       pre == PrePaymentStatus.待审核;

            if (status is ARAPStatus arap)
                return arap == ARAPStatus.草稿 ||
                       arap == ARAPStatus.待审核;

            if (status is PaymentStatus pay)
                return pay == PaymentStatus.草稿 ||
                       pay == PaymentStatus.待审核;

            return false;
        }


 
 



        /// <summary>是否允许取消操作</summary>
        public static bool CanCancel<T>(T status, bool hasRelatedRecords) where T : Enum
        {
            if (IsFinalStatus(status)) return false;


            if (status is PrePaymentStatus pre)
                return pre != PrePaymentStatus.部分核销 && !hasRelatedRecords;

            if (status is ARAPStatus arap)
                return arap != ARAPStatus.部分支付 &&
                       arap != ARAPStatus.坏账;

            if (status is PaymentStatus pay)
                return pay != PaymentStatus.已支付 && !hasRelatedRecords;

            if (status is DataStatus dataStatus)
                return dataStatus != DataStatus.确认 && !hasRelatedRecords;


            return false;
        }

        /// <summary>是否允许反审操作</summary>
        public static bool CanReReview<T>(T status, bool hasRelatedRecords) where T : Enum
        {
            if (IsFinalStatus(status)) return false;

            if (status is PrePaymentStatus pre)
                return pre != PrePaymentStatus.待核销 && !hasRelatedRecords;

            if (status is ARAPStatus arap)
                return arap != ARAPStatus.部分支付 &&
                       arap != ARAPStatus.坏账;

            if (status is PaymentStatus pay)
                return pay != PaymentStatus.已支付 && !hasRelatedRecords;

            if (status is DataStatus dataStatus)
                return dataStatus != DataStatus.新建 && dataStatus != DataStatus.草稿 && !hasRelatedRecords;


            return false;
        }


        //CanReverse 只对 ARAPStatus 有效
        /// <summary>是否允许冲销操作</summary>
        public static bool CanReverse<T>(T status) where T : Enum
        {
            if (status is ARAPStatus arap)
            {
                return arap == ARAPStatus.待支付 ||
                       arap == ARAPStatus.部分支付;
            }
            return false;
        }


        /// <summary>是否允许标记为坏账</summary>
        public static bool CanWriteOffBadDebt<T>(T status) where T : Enum
        {
            if (status is ARAPStatus arap)
            {
                return arap == ARAPStatus.待支付 ||
                       arap == ARAPStatus.部分支付;
            }
            return false;
        }

        /// <summary>是否允许提交操作</summary>
        public static bool CanSubmit<TEnum>(TEnum status) where TEnum : Enum
        {
            return status switch
            {
                DataStatus pre => pre == DataStatus.草稿,
                PrePaymentStatus pre => pre == PrePaymentStatus.草稿,
                ARAPStatus arap => arap == ARAPStatus.草稿,
                PaymentStatus pay => pay == PaymentStatus.草稿,
                _ => false
            };
        }


        //public virtual bool CanSubmit<TEnum>(TEnum status) where TEnum : Enum
        //{
        //    return status switch
        //    {
        //        DataStatus dataStatus => dataStatus == DataStatus.草稿,
        //        PrePaymentStatus pre => pre == PrePaymentStatus.草稿,
        //        ARAPStatus arap => arap == ARAPStatus.草稿,
        //        PaymentStatus pay => pay == PaymentStatus.草稿,
        //        _ => false
        //    };
        //}


        /// <summary>验证状态转换是否合法</summary>
        public static void ValidateTransition<T>(T current, T target) where T : Enum
        {
            if (IsFinalStatus(current))
                throw new InvalidOperationException("终态单据禁止状态变更");

            if (current is PrePaymentStatus preCurrent &&
                target is PrePaymentStatus preTarget)
            {
                ValidatePrePaymentTransition(preCurrent, preTarget);
            }
            else if (current is ARAPStatus arapCurrent &&
                     target is ARAPStatus arapTarget)
            {
                ValidateARAPTransition(arapCurrent, arapTarget);
            }
            else if (current is PaymentStatus payCurrent &&
                     target is PaymentStatus payTarget)
            {
                ValidatePaymentTransition(payCurrent, payTarget);
            }
            else if (current is DataStatus dataCurrent &&
                    target is DataStatus dataTarget)
            {
                ValidatePaymentTransition(dataCurrent, dataTarget);
            }
            else
            {
                throw new InvalidOperationException("禁止跨状态类型转换");
            }
        }

        private static void ValidatePrePaymentTransition(PrePaymentStatus current, PrePaymentStatus target)
        {


            // 终态不能转换
            if (IsFinalStatus(current))
                throw new InvalidOperationException("终态单据禁止状态变更");

            // 退款后不能修改
            if (current == PrePaymentStatus.已退款)
                throw new InvalidOperationException("已退款状态禁止变更");

            // 已核销不能回退
            if ((current == PrePaymentStatus.部分核销 || current == PrePaymentStatus.全额核销) &&
                target == PrePaymentStatus.待核销)
                throw new InvalidOperationException("已核销状态不可回退");

            // 允许从草稿直接到已生效（自动审核）
            if (current == PrePaymentStatus.草稿 && target == PrePaymentStatus.已生效)
                return;

            // 允许从已生效到待核销（支付完成）
            if (current == PrePaymentStatus.已生效 && target == PrePaymentStatus.待核销)
                return;
        }

        private static void ValidateARAPTransition(ARAPStatus current, ARAPStatus target)
        {
            // 终态不能转换
            if (IsFinalStatus(current))
                throw new InvalidOperationException("终态单据禁止状态变更");

            // 冲销后不能修改
            if (current == ARAPStatus.已冲销)
                throw new InvalidOperationException("已冲销状态禁止变更");

            // 已支付不能回退
            if ((current == ARAPStatus.部分支付 || current == ARAPStatus.全部支付) &&
                target == ARAPStatus.待支付)
                throw new InvalidOperationException("已支付状态不可回退");
        }

        private static void ValidatePaymentTransition(PaymentStatus current, PaymentStatus target)
        {
            if (current == PaymentStatus.已支付)
                throw new InvalidOperationException("已支付状态禁止转换");

            if (current == PaymentStatus.草稿 && target != PaymentStatus.待审核)
                throw new InvalidOperationException("草稿状态只能转为待审核");

            if (current == PaymentStatus.待审核 && target != PaymentStatus.已支付)
                throw new InvalidOperationException("待审核状态只能转为已支付");
        }


        private static void ValidatePaymentTransition(DataStatus current, DataStatus target)
        {
            if (current == DataStatus.完结 || current == DataStatus.作废)
                throw new InvalidOperationException("完结及作废状态禁止转换");

            if (current == DataStatus.草稿 && target != DataStatus.新建)
                throw new InvalidOperationException("草稿状态只能转为新建【已提交】");

            if (current == DataStatus.新建 && target != DataStatus.确认)
                throw new InvalidOperationException("新建【已提交】核状态只能转为确认【待审核】");
        }


        /// <summary>获取状态描述</summary>
        public static string GetDescription<T>(T status) where T : Enum
        {
            var field = typeof(T).GetField(status.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? status.ToString();
        }
    }

    public static class StatusReflectionHelper
    {
        /// <summary>获取实体的状态值</summary>
        public static TStatus? GetStatusValue<TStatus>(this BaseEntity entity)
            where TStatus : struct, Enum
        {
            string propertyName = typeof(TStatus).Name;
            if (entity.ContainsProperty(propertyName))
            {
                object value = entity.GetPropertyValue(propertyName);
                if (value != null && Enum.IsDefined(typeof(TStatus), (int)value))
                {
                    return (TStatus)Enum.ToObject(typeof(TStatus), (int)value);
                }
            }
            return null;
        }

        /// <summary>设置实体状态值</summary>
        public static void SetStatusValue<TStatus>(this BaseEntity entity, TStatus status)
            where TStatus : Enum
        {
            string propertyName = typeof(TStatus).Name;
            if (entity.ContainsProperty(propertyName))
            {
                ReflectionHelper.SetPropertyValue(entity, propertyName, (int)(object)status);
            }
        }

        /// <summary>获取实体状态类型</summary>
        public static Type GetStatusType(this BaseEntity entity)
        {
            string[] statusTypes = {
            typeof(PrePaymentStatus).Name,
            typeof(ARAPStatus).Name,
            typeof(DataStatus).Name,
            typeof(PaymentStatus).Name
        };

            foreach (var typeName in statusTypes)
            {
                if (entity.ContainsProperty(typeName))
                {
                    return Type.GetType($"YourNamespace.{typeName}, YourAssembly");
                }
            }
            return null;
        }
    }


}
