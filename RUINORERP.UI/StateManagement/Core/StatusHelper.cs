/**
 * 文件: StatusHelper.cs
 * 版本: V3 - 状态辅助工具类
 * 说明: 状态辅助工具类 - V3状态管理系统，提供通用的状态处理方法
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3: 支持多状态类型的通用处理方法
 * 公共代码: UI层状态管理辅助工具，所有版本通用
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.UI.StateManagement.Core
{
    /// <summary>
    /// 状态辅助工具类 - V3状态管理系统
    /// 提供通用的状态处理方法，减少重复代码
    /// </summary>
    public static class StatusHelper
    {
        #region 状态判断方法

        /// <summary>
        /// 判断状态是否为可编辑状态
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可编辑</returns>
        public static bool IsEditableStatus(object status)
        {
            if (status == null) return false;

            return status switch
            {
                DataStatus dataStatus => dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建,
                PrePaymentStatus preStatus => preStatus == PrePaymentStatus.草稿,
                ARAPStatus arapStatus => arapStatus == ARAPStatus.草稿,
                PaymentStatus paymentStatus => paymentStatus == PaymentStatus.草稿,
                _ => false
            };
        }

        /// <summary>
        /// 判断状态是否为已审核状态
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否已审核</returns>
        public static bool IsApprovedStatus(object status)
        {
            if (status == null) return false;

            return status switch
            {
                DataStatus dataStatus => dataStatus == DataStatus.确认 || dataStatus == DataStatus.完结,
                PrePaymentStatus preStatus => preStatus == PrePaymentStatus.已生效,
                ARAPStatus arapStatus => arapStatus == ARAPStatus.待支付 || arapStatus == ARAPStatus.部分支付 || arapStatus == ARAPStatus.全部支付,
                PaymentStatus paymentStatus => paymentStatus == PaymentStatus.已支付,
                _ => false
            };
        }

        /// <summary>
        /// 判断状态是否为已禁用状态
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否已禁用</returns>
        public static bool IsDisabledStatus(object status)
        {
            if (status == null) return false;

            return status switch
            {
                DataStatus dataStatus => dataStatus == DataStatus.作废,
                PrePaymentStatus preStatus => preStatus == PrePaymentStatus.已结案,
                ARAPStatus arapStatus => arapStatus == ARAPStatus.坏账 || arapStatus == ARAPStatus.已冲销,
                PaymentStatus => false, // PaymentStatus没有明确的禁用状态
                _ => false
            };
        }

        #endregion

        #region 状态显示名称

        /// <summary>
        /// 获取状态对应的中文显示名称
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>中文显示名称</returns>
        public static string GetStatusDisplayName(object status)
        {
            if (status == null) return string.Empty;

            // 优先使用Description特性
            var type = status.GetType();
            var field = type.GetField(status.ToString());
            var description = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .Cast<DescriptionAttribute>()
                                 .FirstOrDefault();
            
            return description?.Description ?? status.ToString();
        }

        #endregion

        #region 状态操作

        /// <summary>
        /// 获取状态对应的可用操作列表
        /// </summary>
        /// <param name="status">当前状态</param>
        /// <returns>可用操作列表</returns>
        public static List<MenuItemEnums> GetAvailableActions(object status)
        {
            if (status == null) return new List<MenuItemEnums>();

            var actions = new List<MenuItemEnums> { MenuItemEnums.查询, MenuItemEnums.刷新 };
            
            // 根据不同状态添加特定操作
            switch (status)
            {
                case DataStatus dataStatus:
                    AddDataStatusActions(actions, dataStatus);
                    break;
                case PrePaymentStatus preStatus:
                    AddPrePaymentStatusActions(actions, preStatus);
                    break;
                case ARAPStatus arapStatus:
                    AddARAPStatusActions(actions, arapStatus);
                    break;
                case PaymentStatus paymentStatus:
                    AddPaymentStatusActions(actions, paymentStatus);
                    break;
            }

            return actions;
        }

        /// <summary>
        /// 为DataStatus添加可用操作
        /// </summary>
        private static void AddDataStatusActions(List<MenuItemEnums> actions, DataStatus status)
        {
            actions.AddRange(status switch
            {
                DataStatus.新建 or DataStatus.草稿 => new[] { MenuItemEnums.修改, MenuItemEnums.审核 },
                DataStatus.确认 => new[] { MenuItemEnums.反审, MenuItemEnums.结案 },
                DataStatus.完结 => new[] { MenuItemEnums.反结案 },
                _ => Array.Empty<MenuItemEnums>()
            });
            
            // 所有非作废状态都可以作废
            if (status != DataStatus.作废)
                actions.Add(MenuItemEnums.删除);
        }

        /// <summary>
        /// 为PrePaymentStatus添加可用操作
        /// </summary>
        private static void AddPrePaymentStatusActions(List<MenuItemEnums> actions, PrePaymentStatus status)
        {
            actions.AddRange(status switch
            {
                PrePaymentStatus.草稿 => new[] { MenuItemEnums.修改, MenuItemEnums.提交 },
                PrePaymentStatus.待审核 => new[] { MenuItemEnums.审核 },
                PrePaymentStatus.已生效 => new[] { MenuItemEnums.反审 },
                _ => Array.Empty<MenuItemEnums>()
            });
        }

        /// <summary>
        /// 为ARAPStatus添加可用操作
        /// </summary>
        private static void AddARAPStatusActions(List<MenuItemEnums> actions, ARAPStatus status)
        {
            actions.AddRange(status switch
            {
                ARAPStatus.草稿 => new[] { MenuItemEnums.修改, MenuItemEnums.提交 },
                ARAPStatus.待审核 => new[] { MenuItemEnums.审核 },
                ARAPStatus.待支付 or ARAPStatus.部分支付 => new[] { MenuItemEnums.反审 },
                _ => Array.Empty<MenuItemEnums>()
            });
        }

        /// <summary>
        /// 为PaymentStatus添加可用操作
        /// </summary>
        private static void AddPaymentStatusActions(List<MenuItemEnums> actions, PaymentStatus status)
        {
            actions.AddRange(status switch
            {
                PaymentStatus.草稿 => new[] { MenuItemEnums.修改, MenuItemEnums.提交 },
                PaymentStatus.待审核 => new[] { MenuItemEnums.审核 },
                _ => Array.Empty<MenuItemEnums>()
            });
        }

        #endregion

        #region 状态转换验证

        /// <summary>
        /// 验证状态转换是否合法
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>是否合法转换</returns>
        public static bool IsValidStateTransition(object fromStatus, object toStatus)
        {
            // 基本验证
            if (fromStatus?.GetType() != toStatus?.GetType()) return false;
            if (fromStatus.Equals(toStatus)) return true;

            // 使用模式匹配进行状态转换验证
            return (fromStatus, toStatus) switch
            {
                (DataStatus dataFrom, DataStatus dataTo) => IsValidDataStatusTransition(dataFrom, dataTo),
                (PrePaymentStatus preFrom, PrePaymentStatus preTo) => IsValidPrePaymentStatusTransition(preFrom, preTo),
                (ARAPStatus arapFrom, ARAPStatus arapTo) => IsValidARAPStatusTransition(arapFrom, arapTo),
                (PaymentStatus payFrom, PaymentStatus payTo) => IsValidPaymentStatusTransition(payFrom, payTo),
                _ => false
            };
        }

        /// <summary>
        /// 验证DataStatus状态转换是否合法
        /// </summary>
        private static bool IsValidDataStatusTransition(DataStatus from, DataStatus to)
        {
            return (from, to) switch
            {
                (DataStatus.新建 or DataStatus.草稿, DataStatus.确认 or DataStatus.作废) => true,
                (DataStatus.确认, DataStatus.完结 or DataStatus.作废) => true,
                (DataStatus.作废, DataStatus.草稿) => true, // 作废后可以重新编辑
                _ => false
            };
        }

        /// <summary>
        /// 验证PrePaymentStatus状态转换是否合法
        /// </summary>
        private static bool IsValidPrePaymentStatusTransition(PrePaymentStatus from, PrePaymentStatus to)
        {
            return (from, to) switch
            {
                (PrePaymentStatus.草稿, PrePaymentStatus.待审核 or PrePaymentStatus.已结案) => true,
                (PrePaymentStatus.待审核, PrePaymentStatus.已生效 or PrePaymentStatus.草稿 or PrePaymentStatus.已结案) => true,
                (PrePaymentStatus.已生效, PrePaymentStatus.待核销 or PrePaymentStatus.部分核销 or PrePaymentStatus.全额核销) => true,
                (PrePaymentStatus.待核销, PrePaymentStatus.部分核销 or PrePaymentStatus.全额核销) => true,
                (PrePaymentStatus.部分核销, PrePaymentStatus.全额核销) => true,
                (PrePaymentStatus.全额核销, PrePaymentStatus.已结案) => true,
                _ => false
            };
        }

        /// <summary>
        /// 验证ARAPStatus状态转换是否合法
        /// </summary>
        private static bool IsValidARAPStatusTransition(ARAPStatus from, ARAPStatus to)
        {
            return (from, to) switch
            {
                (ARAPStatus.草稿, ARAPStatus.待审核 or ARAPStatus.坏账 or ARAPStatus.已冲销) => true,
                (ARAPStatus.待审核, ARAPStatus.待支付 or ARAPStatus.草稿 or ARAPStatus.坏账 or ARAPStatus.已冲销) => true,
                (ARAPStatus.待支付, ARAPStatus.部分支付 or ARAPStatus.全部支付 or ARAPStatus.坏账 or ARAPStatus.已冲销) => true,
                (ARAPStatus.部分支付, ARAPStatus.全部支付 or ARAPStatus.坏账 or ARAPStatus.已冲销) => true,
                (ARAPStatus.全部支付, ARAPStatus.坏账 or ARAPStatus.已冲销) => true,
                _ => false
            };
        }

        /// <summary>
        /// 验证PaymentStatus状态转换是否合法
        /// </summary>
        private static bool IsValidPaymentStatusTransition(PaymentStatus from, PaymentStatus to)
        {
            return (from, to) switch
            {
                (PaymentStatus.草稿, PaymentStatus.待审核) => true,
                (PaymentStatus.待审核, PaymentStatus.已支付 or PaymentStatus.草稿) => true,
                _ => false
            };
        }

        #endregion
    }
}