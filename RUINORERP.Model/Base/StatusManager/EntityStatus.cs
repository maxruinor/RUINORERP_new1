/**
 * 文件: EntityStatus.cs
 * 版本: V3 - 状态管理核心实体类
 * 说明: 实体状态类 - 包含数据性状态和操作性状态的组合
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3: 支持数据状态、操作状态和业务状态的统一管理
 * 公共代码: 状态管理基础实体，所有版本通用
 */

using System;
using System.Collections.Generic;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 实体状态类 - 包含数据性状态和操作性状态的组合
    /// 用于替代过时的DocumentStatus，提供更灵活的状态管理
    /// </summary>
    public class EntityStatus
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityStatus()
        {
            // 初始化为默认值
            dataStatus = DataStatus.草稿;
            actionStatus = ActionStatus.无操作;
            BusinessStatuses = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 数据性状态
        /// 表示实体的数据生命周期状态
        /// </summary>
        public DataStatus? dataStatus { get; set; }

        /// <summary>
        /// 操作性状态
        /// 表示对实体的操作类型
        /// </summary>
        public ActionStatus? actionStatus { get; set; }

        /// <summary>
        /// 业务性状态集合
        /// 键为业务状态枚举类型，值为业务状态值
        /// </summary>
        public Dictionary<Type, object> BusinessStatuses { get; set; }

        /// <summary>
        /// 获取指定类型的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>业务状态值</returns>
        public T GetBusinessStatus<T>() where T : struct, Enum
        {
            if (BusinessStatuses.TryGetValue(typeof(T), out var status))
            {
                return (T)status;
            }

            // 返回默认值
            return default;
        }

        /// <summary>
        /// 设置指定类型的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="status">业务状态值</param>
        public void SetBusinessStatus<T>(T status) where T : struct, Enum
        {
            BusinessStatuses[typeof(T)] = status;
        }

        /// <summary>
        /// 设置指定类型的业务状态
        /// </summary>
        /// <param name="statusType">业务状态枚举类型</param>
        /// <param name="status">业务状态值</param>
        public void SetBusinessStatus(Type statusType, object status)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            if (!statusType.IsEnum)
                throw new ArgumentException("statusType必须是枚举类型", nameof(statusType));

            BusinessStatuses[statusType] = status;
        }

        /// <summary>
        /// 检查是否包含指定类型的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>是否包含</returns>
        public bool HasBusinessStatus<T>() where T : struct, Enum
        {
            return BusinessStatuses.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 移除指定类型的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>是否成功移除</returns>
        public bool RemoveBusinessStatus<T>() where T : struct, Enum
        {
            return BusinessStatuses.Remove(typeof(T));
        }

        /// <summary>
        /// 创建实体状态的副本
        /// </summary>
        /// <returns>实体状态副本</returns>
        public EntityStatus Clone()
        {
            var clone = new EntityStatus
            {
                dataStatus = dataStatus,
                actionStatus = actionStatus,
                BusinessStatuses = new Dictionary<Type, object>(BusinessStatuses)
            };

            return clone;
        }

        /// <summary>
        /// 重写Equals方法
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityStatus other)
            {
                var dataStatusEqual = dataStatus == other.dataStatus;
                var actionStatusEqual = actionStatus == other.actionStatus;
                var businessStatusesEqual = BusinessStatuses.Count == other.BusinessStatuses.Count;

                if (businessStatusesEqual)
                {
                    foreach (var kvp in BusinessStatuses)
                    {
                        if (!other.BusinessStatuses.TryGetValue(kvp.Key, out var otherValue) ||
                            !Equals(kvp.Value, otherValue))
                        {
                            businessStatusesEqual = false;
                            break;
                        }
                    }
                }

                return dataStatusEqual && actionStatusEqual && businessStatusesEqual;
            }

            return false;
        }

        /// <summary>
        /// 重写GetHashCode方法
        /// </summary>
        /// <returns>哈希码</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                // 处理dataStatus
                hash = hash * 31 + (dataStatus.HasValue ? dataStatus.Value.GetHashCode() : 0);

                // 处理actionStatus
                hash = hash * 31 + (actionStatus.HasValue ? actionStatus.Value.GetHashCode() : 0);

                // 处理BusinessStatuses
                foreach (var kvp in BusinessStatuses)
                {
                    hash = hash * 31 + (kvp.Key?.GetHashCode() ?? 0);
                    hash = hash * 31 + (kvp.Value?.GetHashCode() ?? 0);
                }

                return hash;
            }
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            var parts = new List<string>();

            if (dataStatus.HasValue)
                parts.Add($"DataStatus={dataStatus.Value}");

            if (actionStatus.HasValue)
                parts.Add($"ActionStatus={actionStatus.Value}");

            foreach (var kvp in BusinessStatuses)
            {
                parts.Add($"{kvp.Key.Name}={kvp.Value}");
            }

            return $"EntityStatus: {{ {string.Join(", ", parts)} }}";
        }

        #region 状态属性管理

        /// <summary>
        /// 状态属性存储
        /// </summary>
        private Dictionary<StatusType, Dictionary<string, object>> _stateProperties = new Dictionary<StatusType, Dictionary<string, object>>();

        /// <summary>
        /// 获取状态属性
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="statusType">状态类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        public TProperty GetStateProperty<TProperty>(StatusType statusType, string propertyName)
        {
            if (_stateProperties.TryGetValue(statusType, out var properties) &&
                properties.TryGetValue(propertyName, out var value))
            {
                return value is TProperty typedValue ? typedValue : default(TProperty);
            }

            return default(TProperty);
        }

        /// <summary>
        /// 设置状态属性
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        public void SetStateProperty(StatusType statusType, string propertyName, object value)
        {
            if (!_stateProperties.ContainsKey(statusType))
            {
                _stateProperties[statusType] = new Dictionary<string, object>();
            }

            _stateProperties[statusType][propertyName] = value;
        }

        /// <summary>
        /// 获取所有状态属性
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态属性字典</returns>
        public Dictionary<string, object> GetAllStateProperties(StatusType statusType)
        {
            return _stateProperties.TryGetValue(statusType, out var properties) 
                ? new Dictionary<string, object>(properties) 
                : new Dictionary<string, object>();
        }

        /// <summary>
        /// 清除状态属性
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>是否清除成功</returns>
        public bool ClearStateProperty(StatusType statusType, string propertyName)
        {
            if (_stateProperties.TryGetValue(statusType, out var properties))
            {
                return properties.Remove(propertyName);
            }

            return false;
        }

        /// <summary>
        /// 清除所有状态属性
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <returns>是否清除成功</returns>
        public bool ClearAllStateProperties(StatusType statusType)
        {
            if (_stateProperties.ContainsKey(statusType))
            {
                _stateProperties[statusType].Clear();
                return true;
            }

            return false;
        }

        #endregion
    }
}