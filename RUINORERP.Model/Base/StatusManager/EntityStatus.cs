/**
 * 文件: EntityStatus.cs
 * 版本: V4 - 状态管理核心实体类（优化版）
 * 说明: 实体状态类 - 支持DataStatus与业务状态互斥关系
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - 实现DataStatus与业务状态互斥关系
 * 
 * 版本标识：
 * V4: 支持DataStatus与业务状态互斥关系，一个实体只能使用一种主要状态类型
 * V3: 支持数据状态、操作状态和业务状态的统一管理
 * 公共代码: 状态管理基础实体，所有版本通用
 */

using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.Base;
namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 实体状态类 - 支持DataStatus与业务状态互斥关系
    /// 用于替代过时的DocumentStatus，提供更灵活的状态管理
    /// 一个实体只能使用一种主要状态类型：DataStatus或业务状态
    /// 版本: V4 - 支持DataStatus与业务状态互斥
    /// 
    /// 设计说明:
    /// 1. 所有状态类型(DataStatus、PrePaymentStatus、ARAPStatus、PaymentStatus、StatementStatus)均保存在_businessStatuses字典中
    /// 2. DataStatus作为特殊状态类型，额外使用_dataStatus私有字段存储，提高访问效率
    /// 3. 设置DataStatus时，会自动清除所有非DataStatus类型的业务状态，确保互斥性
    /// 4. 设置业务状态时，会自动清除DataStatus状态，确保互斥性
    /// 5. 提供状态属性管理功能，支持动态扩展状态属性
    /// </summary>
    public class EntityStatus : ICloneable, IEquatable<EntityStatus>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityStatus()
        {
            // 初始化为默认值
            actionStatus = ActionStatus.无操作;
            _businessStatuses = new Dictionary<Type, object>();
            _approvalStatus = 0; // 默认未审核
            _approvalResults = false; // 默认审核结果为false
        }

        #region 私有字段

        /// <summary>
        /// 数据性状态 - 私有字段，与业务状态互斥
        /// 用于提高DataStatus的访问效率，避免每次都从字典中获取
        /// 与_businessStatuses字典中的DataStatus类型保持同步
        /// </summary>
        private DataStatus? _dataStatus;

        /// <summary>
        /// 业务性状态集合 - 私有字段，确保与数据状态互斥
        /// 存储所有类型的状态，包括DataStatus和各种业务状态
        /// Key: 状态类型(Type)
        /// Value: 状态值(object)
        /// </summary>
        private Dictionary<Type, object> _businessStatuses;

        /// <summary>
        /// 审核状态 - 私有字段
        /// </summary>
        private int? _approvalStatus;

        /// <summary>
        /// 审核结果 - 私有字段
        /// </summary>
        private bool? _approvalResults;

        /// <summary>
        /// 状态属性存储
        /// 用于存储状态的额外属性信息，支持动态扩展
        /// Key: 状态类型(Type)
        /// Value: 属性字典(Dictionary<string, object>)
        /// </summary>
        private Dictionary<Type, Dictionary<string, object>> _stateProperties;

        #endregion

        #region 公共属性

        /// <summary>
        /// 数据性状态 - 与业务状态互斥
        /// 表示实体的数据生命周期状态
        /// 只读属性，直接返回_dataStatus字段
        /// </summary>
        public DataStatus? dataStatus
        {
            get
            {
                return _dataStatus;
            }
        }

        /// <summary>
        /// 操作性状态
        /// 表示对实体的操作类型，可与其他状态共存
        /// </summary>
        public ActionStatus? actionStatus { get; set; }

        /// <summary>
        /// 业务性状态集合 - 只读属性，确保与数据状态互斥
        /// 键为业务状态枚举类型，值为业务状态值
        /// </summary>
        public IReadOnlyDictionary<Type, object> BusinessStatuses => _businessStatuses;

        /// <summary>
        /// 获取当前主要状态值（简化访问）
        /// </summary>
        public object CurrentStatus
        {
            get
            {
                // 优先返回DataStatus
                if (_businessStatuses.ContainsKey(typeof(DataStatus)))
                {
                    return _businessStatuses[typeof(DataStatus)];
                }
                
                // 如果没有DataStatus，返回第一个业务状态
                if (_businessStatuses.Count > 0)
                    return _businessStatuses.Values.First();

                return DataStatus.草稿; // 默认状态
            }
        }

        /// <summary>
        /// 获取当前主要状态类型（简化访问）
        /// </summary>
        public Type CurrentStatusType
        {
            get
            {
                // 优先返回DataStatus类型
                if (_businessStatuses.ContainsKey(typeof(DataStatus)))
                    return typeof(DataStatus);

                // 如果没有DataStatus，返回第一个业务状态类型
                if (_businessStatuses.Count > 0)
                    return _businessStatuses.Keys.First();

                return typeof(DataStatus); // 默认类型
            }
        }

        /// <summary>
        /// 获取当前状态的字符串表示
        /// </summary>
        public string StatusString
        {
            get
            {
                // 优先返回DataStatus的字符串表示
                if (_businessStatuses.ContainsKey(typeof(DataStatus)))
                {
                    return _businessStatuses[typeof(DataStatus)].ToString();
                }
                
                // 如果没有DataStatus，返回第一个业务状态的字符串表示
                if (_businessStatuses.Count > 0)
                    return _businessStatuses.Values.First().ToString();

                return DataStatus.草稿.ToString();
            }
        }

        /// <summary>
        /// 审核状态 - 可与其他状态共存
        /// </summary>
        public int? ApprovalStatus
        {
            get => _approvalStatus;
            set => _approvalStatus = value;
        }

        /// <summary>
        /// 审核结果 - 可与其他状态共存
        /// </summary>
        public bool? ApprovalResults
        {
            get => _approvalResults;
            set => _approvalResults = value;
        }



        #endregion

        #region 业务状态管理方法

        /// <summary>
        /// 获取实体状态类型
        /// 根据实体包含的属性判断使用哪种状态类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>状态类型</returns>
        public Type GetStatusType(BaseEntity entity)
        {
            // 检查实体是否包含DataStatus属性
            if (entity.ContainsProperty(typeof(DataStatus).Name))
                return typeof(DataStatus);

            // 检查实体是否包含PrePaymentStatus属性
            if (entity.ContainsProperty(typeof(PrePaymentStatus).Name))
                return typeof(PrePaymentStatus);

            // 检查实体是否包含ARAPStatus属性
            if (entity.ContainsProperty(typeof(ARAPStatus).Name))
                return typeof(ARAPStatus);

            // 检查实体是否包含PaymentStatus属性
            if (entity.ContainsProperty(typeof(PaymentStatus).Name))
                return typeof(PaymentStatus);
            
            // 检查实体是否包含StatementStatus属性
            if (entity.ContainsProperty(typeof(StatementStatus).Name))
                return typeof(StatementStatus);

            return null;
        }

        /// <summary>
        /// 设置数据状态 - 确保与业务状态互斥
        /// 设置DataStatus时，会自动清除所有非DataStatus类型的业务状态
        /// </summary>
        /// <param name="status">数据状态值</param>
        public void SetDataStatus(DataStatus? status)
        {
            // 设置_dataStatus字段
            _dataStatus = status;
            
            // 重置业务状态字典，确保互斥
            _businessStatuses.Clear();
            
            // 如果status有值，将DataStatus添加到业务状态字典
            if (status.HasValue)
            {
                _businessStatuses[typeof(DataStatus)] = status.Value;
            }
        }

        /// <summary>
        /// 获取指定类型的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>业务状态值</returns>
        public T GetBusinessStatus<T>() where T : struct, Enum
        {
            // 尝试从业务状态字典中获取指定类型的状态
            if (_businessStatuses.TryGetValue(typeof(T), out var status))
            {
                return (T)status;
            }

            // 如果不存在，返回默认值
            return default;
        }

        /// <summary>
        /// 设置指定类型的业务状态 - 确保与数据状态互斥
        /// 设置业务状态时，会自动清除其他类型的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="status">业务状态值</param>
        public void SetBusinessStatus<T>(T status) where T : struct, Enum
        {
            // 重置业务状态字典和数据状态，确保互斥性
            _businessStatuses.Clear();
            
            // 根据类型设置状态
            if (typeof(T) == typeof(DataStatus))
            {
                _dataStatus = (DataStatus)(object)status;
            }
            else
            {
                _dataStatus = null;
            }
            
            // 更新业务状态字典
            _businessStatuses[typeof(T)] = status;
        }

        /// <summary>
        /// 设置指定类型的业务状态 - 确保与数据状态互斥
        /// 设置业务状态时，会自动清除其他类型的业务状态
        /// </summary>
        /// <param name="statusType">业务状态枚举类型</param>
        /// <param name="status">业务状态值</param>
        public void SetBusinessStatus(Type statusType, object status)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            if (!statusType.IsEnum)
                throw new ArgumentException("statusType必须是枚举类型", nameof(statusType));

            // 如果是DataStatus类型，同时设置_dataStatus字段
            if (statusType == typeof(DataStatus))
            {
                _dataStatus = (DataStatus)status;
            }
            else
            {
                // 设置其他业务状态时，清除数据状态
                _dataStatus = null;
            }
            // 更新业务状态字典
            _businessStatuses[statusType] = status;
        }

        /// <summary>
        /// 检查是否包含指定类型的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>是否包含</returns>
        public bool HasBusinessStatus<T>() where T : struct, Enum
        {
            return _businessStatuses.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 移除指定类型的业务状态
        /// 如果移除的是DataStatus类型，同时清除_dataStatus字段
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>是否成功移除</returns>
        public bool RemoveBusinessStatus<T>() where T : struct, Enum
        {
            // 如果移除的是DataStatus类型，同时清除_dataStatus字段
            if (typeof(T) == typeof(DataStatus))
            {
                _dataStatus = null;
            }
            
            return _businessStatuses.Remove(typeof(T));
        }

        /// <summary>
        /// 清除所有业务状态
        /// 清除业务状态字典的同时，也清除_dataStatus字段
        /// </summary>
        public void ClearAllBusinessStatuses()
        {
            _businessStatuses.Clear();
            _dataStatus = null; // 同时清除数据状态
        }

        #endregion

        /// <summary>
        /// 创建实体状态的副本
        /// 复制所有状态信息，包括_dataStatus、_businessStatuses等
        /// </summary>
        /// <returns>实体状态副本</returns>
        public EntityStatus Clone()
        {
            var clone = new EntityStatus
            {
                _dataStatus = _dataStatus,
                actionStatus = actionStatus,
                _businessStatuses = new Dictionary<Type, object>(_businessStatuses),
                _approvalStatus = _approvalStatus,
                _approvalResults = _approvalResults
            };
            
            // 如果状态属性字典存在，则复制
            if (_stateProperties != null)
            {
                clone._stateProperties = new Dictionary<Type, Dictionary<string, object>>(_stateProperties);
            }

            return clone;
        }

        /// <summary>
        /// 实现ICloneable接口的Clone方法
        /// 返回object类型以满足接口要求
        /// </summary>
        /// <returns>实体状态副本</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 重写Equals方法
        /// 比较两个EntityStatus对象的所有状态信息是否相等
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            if (obj is EntityStatus other)
            {
                // 比较各个状态字段
                var dataStatusEqual = _dataStatus == other._dataStatus;
                var actionStatusEqual = actionStatus == other.actionStatus;
                var approvalStatusEqual = _approvalStatus == other._approvalStatus;
                var approvalResultsEqual = _approvalResults == other._approvalResults;
                var businessStatusesEqual = _businessStatuses.Count == other._businessStatuses.Count;

                // 比较业务状态字典
                if (businessStatusesEqual)
                {
                    foreach (var kvp in _businessStatuses)
                    {
                        if (!other._businessStatuses.TryGetValue(kvp.Key, out var otherValue) ||
                            !Equals(kvp.Value, otherValue))
                        {
                            businessStatusesEqual = false;
                            break;
                        }
                    }
                }

                return dataStatusEqual && actionStatusEqual && approvalStatusEqual &&
                       approvalResultsEqual && businessStatusesEqual;
            }

            return false;
        }

        /// <summary>
        /// 实现IEquatable<EntityStatus>接口的Equals方法
        /// </summary>
        /// <param name="other">比较的EntityStatus对象</param>
        /// <returns>是否相等</returns>
        public bool Equals(EntityStatus other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// 重写GetHashCode方法
        /// 基于所有状态信息生成哈希码
        /// </summary>
        /// <returns>哈希码</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                // 处理dataStatus
                hash = hash * 31 + (_dataStatus.HasValue ? _dataStatus.Value.GetHashCode() : 0);

                // 处理actionStatus
                hash = hash * 31 + (actionStatus.HasValue ? actionStatus.Value.GetHashCode() : 0);

                // 处理ApprovalStatus
                hash = hash * 31 + (_approvalStatus.HasValue ? _approvalStatus.Value.GetHashCode() : 0);

                // 处理ApprovalResults
                hash = hash * 31 + (_approvalResults.HasValue ? _approvalResults.Value.GetHashCode() : 0);

                // 处理BusinessStatuses
                foreach (var kvp in _businessStatuses)
                {
                    hash = hash * 31 + (kvp.Key?.GetHashCode() ?? 0);
                    hash = hash * 31 + (kvp.Value?.GetHashCode() ?? 0);
                }

                return hash;
            }
        }

        /// <summary>
        /// 重写ToString方法
        /// 返回包含所有状态信息的字符串表示
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            var parts = new List<string>();

            if (_dataStatus.HasValue)
                parts.Add($"DataStatus={_dataStatus.Value}");

            if (actionStatus.HasValue)
                parts.Add($"ActionStatus={actionStatus.Value}");

            if (_approvalStatus.HasValue)
                parts.Add($"ApprovalStatus={_approvalStatus.Value}");

            if (_approvalResults.HasValue)
                parts.Add($"ApprovalResults={_approvalResults.Value}");

            foreach (var kvp in _businessStatuses)
            {
                parts.Add($"{kvp.Key.Name}={kvp.Value}");
            }

            return $"EntityStatus: {{ {string.Join(", ", parts)} }}";
        }

        #region 状态属性管理

        /// <summary>
        /// 获取状态属性
        /// 根据状态类型和属性名称获取属性值
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="statusType">状态类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        public TProperty GetStateProperty<TProperty>(Type statusType, string propertyName)
        {
            // 确保状态属性字典已初始化
            if (_stateProperties == null)
                _stateProperties = new Dictionary<Type, Dictionary<string, object>>();
                
            // 尝试获取指定状态类型的属性字典
            if (_stateProperties.TryGetValue(statusType, out var properties) &&
                properties.TryGetValue(propertyName, out var value))
            {
                // 如果值类型匹配，返回转换后的值，否则返回默认值
                return value is TProperty typedValue ? typedValue : default(TProperty);
            }

            return default(TProperty);
        }

        /// <summary>
        /// 设置状态属性
        /// 根据状态类型和属性名称设置属性值
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        public void SetStateProperty(Type statusType, string propertyName, object value)
        {
            // 确保状态属性字典已初始化
            if (_stateProperties == null)
                _stateProperties = new Dictionary<Type, Dictionary<string, object>>();
                
            // 如果状态类型不存在，创建新的属性字典
            if (!_stateProperties.ContainsKey(statusType))
            {
                _stateProperties[statusType] = new Dictionary<string, object>();
            }

            // 设置属性值
            _stateProperties[statusType][propertyName] = value;
        }

        /// <summary>
        /// 获取所有状态属性
        /// 返回指定状态类型的所有属性副本
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态属性字典</returns>
        public Dictionary<string, object> GetAllStateProperties(Type statusType)
        {
            // 确保状态属性字典已初始化
            if (_stateProperties == null)
                return new Dictionary<string, object>();
                
            return _stateProperties.TryGetValue(statusType, out var properties)
                ? new Dictionary<string, object>(properties)
                : new Dictionary<string, object>();
        }

        /// <summary>
        /// 清除状态属性
        /// 清除指定状态类型的指定属性
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>是否清除成功</returns>
        public bool ClearStateProperty(Type statusType, string propertyName)
        {
            // 确保状态属性字典已初始化
            if (_stateProperties == null)
                return false;
                
            // 尝试获取指定状态类型的属性字典
            if (_stateProperties.TryGetValue(statusType, out var properties))
            {
                // 移除指定属性
                return properties.Remove(propertyName);
            }

            return false;
        }

        /// <summary>
        /// 清除所有状态属性
        /// 清除指定状态类型的所有属性
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <returns>是否清除成功</returns>
        public bool ClearAllStateProperties(Type statusType)
        {
            // 确保状态属性字典已初始化
            if (_stateProperties == null)
                return false;
                
            // 检查状态类型是否存在
            if (_stateProperties.ContainsKey(statusType))
            {
                // 清空属性字典
                _stateProperties[statusType].Clear();
                return true;
            }

            return false;
        }

        #endregion
    }
}