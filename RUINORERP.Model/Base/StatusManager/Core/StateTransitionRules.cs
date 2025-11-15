/**
 * 文件: StateTransitionRules.cs
 * 说明: 状态转换规则管理类
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using RUINORERP.Global;
using System;
using System.Collections.Generic;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态转换规则管理类
    /// </summary>
    public static class StateTransitionRules
    {
        /// <summary>
        /// 初始化默认状态转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        public static void InitializeDefaultRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            if (transitionRules == null)
                throw new ArgumentNullException(nameof(transitionRules));

            // 初始化DataStatus转换规则
            InitializeDataStatusRules(transitionRules);
            
            // 初始化ActionStatus转换规则
            InitializeActionStatusRules(transitionRules);
        }

        /// <summary>
        /// 初始化DataStatus转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializeDataStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            transitionRules[typeof(DataStatus)] = new Dictionary<object, List<object>>
            {
                [DataStatus.草稿] = new List<object> { DataStatus.新建, DataStatus.作废 },
                [DataStatus.新建] = new List<object> { DataStatus.确认, DataStatus.作废 },
                [DataStatus.确认] = new List<object> { DataStatus.完结, DataStatus.作废 },
                [DataStatus.完结] = new List<object> { }, // 完结状态不能再转换
                [DataStatus.作废] = new List<object> { DataStatus.草稿 } // 作废状态可以重新激活为草稿
            };
        }

        /// <summary>
        /// 初始化ActionStatus转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializeActionStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            transitionRules[typeof(ActionStatus)] = new Dictionary<object, List<object>>
            {
                [ActionStatus.无操作] = new List<object> { ActionStatus.新增, ActionStatus.加载, ActionStatus.复制 },
                [ActionStatus.新增] = new List<object> { ActionStatus.修改, ActionStatus.删除 },
                [ActionStatus.修改] = new List<object> { ActionStatus.修改, ActionStatus.删除 },
                [ActionStatus.删除] = new List<object> { ActionStatus.新增 },
                [ActionStatus.加载] = new List<object> { ActionStatus.修改, ActionStatus.删除, ActionStatus.复制 },
                [ActionStatus.复制] = new List<object> { ActionStatus.新增, ActionStatus.修改, ActionStatus.删除 }
            };
        }

        /// <summary>
        /// 添加自定义状态转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="transitionRules">转换规则字典</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatuses">目标状态列表</param>
        public static void AddTransitionRule<T>(Dictionary<Type, Dictionary<object, List<object>>> transitionRules, T fromStatus, params T[] toStatuses) where T : Enum
        {
            if (transitionRules == null)
                throw new ArgumentNullException(nameof(transitionRules));

            var statusType = typeof(T);
            
            if (!transitionRules.ContainsKey(statusType))
            {
                transitionRules[statusType] = new Dictionary<object, List<object>>();
            }

            transitionRules[statusType][fromStatus] = new List<object>();
            foreach (var status in toStatuses)
            {
                transitionRules[statusType][fromStatus].Add(status);
            }
        }

        /// <summary>
        /// 移除状态转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="transitionRules">转换规则字典</param>
        /// <param name="fromStatus">源状态</param>
        public static void RemoveTransitionRule<T>(Dictionary<Type, Dictionary<object, List<object>>> transitionRules, T fromStatus) where T : Enum
        {
            if (transitionRules == null)
                throw new ArgumentNullException(nameof(transitionRules));

            var statusType = typeof(T);
            
            if (transitionRules.ContainsKey(statusType))
            {
                transitionRules[statusType].Remove(fromStatus);
            }
        }

        /// <summary>
        /// 检查状态转换是否被允许
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="transitionRules">转换规则字典</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>是否允许转换</returns>
        public static bool IsTransitionAllowed<T>(Dictionary<Type, Dictionary<object, List<object>>> transitionRules, T fromStatus, T toStatus) where T : Enum
        {
            if (transitionRules == null)
                throw new ArgumentNullException(nameof(transitionRules));

            var statusType = typeof(T);
            
            if (!transitionRules.ContainsKey(statusType))
            {
                return false;
            }

            var rules = transitionRules[statusType];
            
            if (!rules.ContainsKey(fromStatus))
            {
                return false;
            }

            return rules[fromStatus].Contains(toStatus);
        }
    }
}