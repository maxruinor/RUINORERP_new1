
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Model.Context;
using SharpYaml.Tokens;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RUINORERP.Global.Extensions;
using System.Threading;

/*
 * V4版本 - 2025-01-12
 * 
 * 适配V4版本状态管理系统
 * 1. 移除与EntityStatus重复的状态管理代码
 * 2. 更新状态转换事件处理以适配V4版本的StateTransitionEventArgs
 * 3. 确保正确使用V4版本的StateTransitionResult
 * 4. 更新状态管理器初始化代码以适配V4版本
 * 
 * V4版本特性：
 * - 支持DataStatus与业务状态互斥关系
 * - 使用EntityStatus统一管理所有状态
 * - 状态转换规则由StateTransitionRules统一管理
 * - 状态转换事件使用StateTransitionEventArgs和StateTransitionResult
 */
namespace RUINORERP.Model
{
    /// </summary>
    public enum ActionStatus
    {
        无操作,
        新增,
        修改,
        删除,
        加载,
        复制,
    }

    [Serializable()]
    public class BaseEntity : INotifyPropertyChanged//, IDataErrorInfo//, IStatusProvider - 移除IDataErrorInfo接口，避免SqlSugar绑定索引器失败
    {
        #region 属性变更追踪

        private readonly ConcurrentDictionary<string, PropertyChangeRecord> _changedProperties = new ConcurrentDictionary<string, PropertyChangeRecord>();
        private static readonly ConcurrentDictionary<Type, string[]> _skipPropertyCache = new ConcurrentDictionary<Type, string[]>();

        #endregion


        /// <summary>
        /// 获取所有已变更属性的名称集合
        /// 获取所有已修改的属性名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public IEnumerable<string> ChangedProperties => _changedProperties.Keys;



        // 获取变更详情（属性名，原始值，新值）
        public IEnumerable<(string Name, object Original, object Current)> GetPropertyChanges()
        {
            foreach (var kvp in _changedProperties)
            {
                yield return (kvp.Key, kvp.Value.OriginalValue, kvp.Value.CurrentValue);
            }
        }

        // 获取变更的SQLSugar列名映射
        public Dictionary<string, string> GetChangedColumnMappings()
        {
            var mappings = new Dictionary<string, string>();
            var properties = GetCachedProperties();

            foreach (var propName in _changedProperties.Keys)
            {
                var prop = properties.FirstOrDefault(p => p.Name == propName);
                var columnAttr = prop?.GetCustomAttribute<SugarColumn>();
                if (columnAttr != null && !string.IsNullOrEmpty(columnAttr.ColumnName))
                {
                    mappings[propName] = columnAttr.ColumnName;
                }
            }
            return mappings;
        }


        /// <summary>
        /// 是否有真实的变化
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool IsEffectivelyChanged(string propertyName)
        {
            if (_changedProperties.TryGetValue(propertyName, out var record))
            {
                return !object.Equals(record.OriginalValue, record.CurrentValue);
            }
            return false;
        }



        /// <summary>
        /// 检查特定属性是否有变更
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>如果属性有变更返回true，否则返回false</returns>
        public bool HasPropertyChanged(string propertyName)
        {
            return _changedProperties.Keys.Contains(propertyName);
        }

        /// <summary>
        /// 获取属性的原始值
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性的原始值</returns>
        public object GetOriginalValue(string propertyName)
        {
            return _changedProperties.TryGetValue(propertyName, out var record)
        ? record.OriginalValue
        : GetPropertyValue(propertyName);
        }


        /// <summary>
        /// 获取真实的变化
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, (object Original, object Current)> GetEffectiveChanges()
        {
            return _changedProperties
       .Where(kvp => !object.Equals(kvp.Value.OriginalValue, kvp.Value.CurrentValue))
       .ToDictionary(
           kvp => kvp.Key,
           kvp => (kvp.Value.OriginalValue, kvp.Value.CurrentValue)
       );

        }




        #region 状态机管理

        /// <summary>
        /// 状态变更事件 - V4版本
        /// 使用StateTransitionEventArgs和StateTransitionResult
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        /// <summary>
        /// 状态变更处理（增强版）
        /// 提供安全、可靠的事件触发机制，确保状态变更通知能够正确传递给所有订阅者
        /// 关键优化：隔离异常，防止单个订阅者异常影响整体事件传递
        /// </summary>
        /// <param name="e">状态转换事件参数</param>
        protected virtual void OnStatusChanged(StateTransitionEventArgs e)
        {
            // 类型安全检查
            if (e == null)
            {
                return;
            }

            // 安全检查：确保有订阅者才尝试触发事件
            if (StatusChanged == null)
                return;

            try
            {
                // 优化：获取所有订阅者的委托列表，并逐个安全触发
                // 这种方式可以防止某个订阅者抛出异常影响其他订阅者
                Delegate[] subscribers = StatusChanged.GetInvocationList();
                foreach (Delegate subscriber in subscribers)
                {
                    try
                    {
                        // 安全调用单个订阅者
                        subscriber.DynamicInvoke(this, e);
                    }
                    catch (Exception ex)
                    {
                        // 隔离单个订阅者的异常
                        Debug.WriteLine($"状态变更事件订阅者异常: {ex.InnerException?.Message ?? ex.Message}");
                        Debug.WriteLine($"订阅者类型: {subscriber.Method.DeclaringType?.Name}.{subscriber.Method.Name}");
                        // 继续执行，不影响其他订阅者
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕获并记录其他可能的异常，但不中断执行流程
                Debug.WriteLine($"触发状态变更事件时发生系统错误: {ex.Message}");
                Debug.WriteLine($"错误堆栈: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 状态变更事件触发方法（增强版）
        /// 提供安全、高效、可靠的方式触发状态变更事件
        /// 确保任何状态变更都能被正确处理，无论是通过直接属性修改还是状态管理器操作
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态值</param>
        /// <param name="newStatus">新状态值</param>
        /// <param name="reason">变更原因（可选）</param>
        /// <param name="userId">用户ID（可选）</param>
        public void TriggerStatusChange(Type statusType, object oldStatus, object newStatus, string reason = null, string userId = null)
        {
            // 参数有效性验证
            if (statusType == null)
            {
                return;
            }

            // 增强版状态变更检测，避免不必要的事件触发
            bool isChanged;
            if (oldStatus == null && newStatus == null)
            {
                isChanged = false;
            }
            else if (oldStatus == null || newStatus == null)
            {
                isChanged = true; // 一个为null另一个不为null，视为变更
            }
            else
            {
                isChanged = !oldStatus.Equals(newStatus); // 值比较
            }

            // 无变更则直接返回，提高性能
            if (!isChanged)
                return;

            DateTime startTime = DateTime.Now;

            // 设置防重入标志，防止状态变更事件处理过程中再次触发属性变更通知导致的循环调用
            _isProcessingStatusChange = true;
            
            try
            {
                // 创建状态变更事件参数
                var eventArgs = new StateTransitionEventArgs(
                    this,
                    statusType,
                    oldStatus,
                    newStatus,
                    reason,
                    userId);

                // 检查是否有订阅者，避免不必要的事件触发开销
                if (StatusChanged != null)
                {
                    // 直接调用OnStatusChanged处理状态变更
                    OnStatusChanged(eventArgs);
                }

                // 性能监控 - 仅在调试模式下记录
#if DEBUG
                TimeSpan duration = DateTime.Now - startTime;
                if (duration.TotalMilliseconds > 10)
                {
                    Debug.WriteLine($"状态变更事件处理耗时较长: {duration.TotalMilliseconds}ms - 实体类型: {this.GetType().Name}");
                }
#endif
            }
            catch (Exception ex)
            {
                // 增强异常处理，详细记录错误信息
                Debug.WriteLine($"状态变更触发错误: {ex.Message}");
                Debug.WriteLine($"异常堆栈: {ex.StackTrace}");
                Debug.WriteLine($"实体类型: {this.GetType().Name}, 状态类型: {statusType.Name}");

                // 确保异常不会传播，不影响主流程执行
            }
            finally
            {
                // 无论处理成功或失败，都必须清除防重入标志
                _isProcessingStatusChange = false;
            }
        }

        /// <summary>
        /// 基础实体构造函数
        /// </summary>
        public BaseEntity()
        {
            // 初始化属性变更追踪
            BeginOperation();

        }


        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();
        private PropertyInfo[] GetCachedProperties()
        {
            var type = GetType();
            return _propertyCache.GetOrAdd(type, t =>
                t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                 .Where(p => !ShouldSkipTracking(p)).ToArray());
        }


        #region 状态计算 deepseek


        /// <summary>
        /// 开始业务操作，记录当前状态为原始值
        /// </summary>
        public void BeginOperation()
        {
            // 清除历史状态
            _changedProperties.Clear();

            HasChanged = false;
        }

        /// <summary>
        /// 接受变更，将当前值设为新的原始值（保存后调用）
        /// 在保存数据后必须调用AcceptChanges
        /// </summary>
        public void AcceptChanges()
        {
            _changedProperties.Clear();
            HasChanged = false;
        }

        /// <summary>
        /// 回滚变更
        /// </summary>
        void RollbackChanges()
        {
            // 回滚到操作开始时的状态
            //foreach (var prop in _changedProperties)
            //{
            //    var original = _originalValues[prop];
            //    GetType().GetProperty(prop)?.SetValue(this, original);
            //}

            foreach (var propName in _changedProperties.Keys.ToList())
            {
                var property = GetType().GetProperty(propName);
                property?.SetValue(this, _changedProperties[propName].OriginalValue);
            }
            _changedProperties.Clear();
            HasChanged = false;


        }




        // 新增：检查属性是否应跳过跟踪
        private bool ShouldSkipTracking(PropertyInfo property)
        {
            // 跳过标记为忽略的属性
            //var sugarColumnAttr = property.GetCustomAttribute<SugarColumn>();
            //if (sugarColumnAttr != null && sugarColumnAttr.IsIgnore) return true;

            //// 跳过导航属性
            //if (property.GetCustomAttribute<Navigate>() != null) return true;

            var type = GetType();
            var skipProps = _skipPropertyCache.GetOrAdd(type, t =>
            {
                return t.GetProperties()
                    .Where(p =>
                        (p.GetCustomAttribute<SugarColumn>()?.IsIgnore == true) ||
                        (p.GetCustomAttribute<Navigate>() != null) ||
                        _baseSkipProperties.Contains(p.Name))
                    .Select(p => p.Name)
                    .ToArray();
            });

            return skipProps.Contains(property.Name);


            //// 跳过特定属性
            //var skipProperties = new[] { "StatusEvaluator", "FieldNameList", "HelpInfos", "RowImage", "ActionStatus" };
            //return skipProperties.Contains(property.Name);
        }

        // 基类固定跳过的属性
        private static readonly string[] _baseSkipProperties =
        {
    "StatusEvaluator", "FieldNameList", "HelpInfos",
    "RowImage", "ActionStatus", "Selected", "Childs"
};


        // 新增：检查值是否实际变化
        private bool IsValueChanged<T>(string propertyName, object newValue)
        {
            if (!_changedProperties.TryGetValue(propertyName, out var originalValue))
                return true;

            // 特殊处理可空类型
            if (originalValue == null && newValue == null) return false;
            if (originalValue == null || newValue == null) return true;

            // 处理实现了 IEquatable 接口的类型
            if (originalValue is IEquatable<T> equatable && equatable.Equals(newValue))
                return false;

            // 默认比较
            return !originalValue.Equals(newValue);
        }


        // 新增：检查特定属性是否修改
        public bool IsPropertyChanged(string propertyName)
        {
            return _changedProperties.Keys.Contains(propertyName);
        }

        // 新增：重置变更状态（保存后调用）
        public void ResetChangeTracking()
        {
            _changedProperties.Clear();
            HasChanged = false;
            ActionStatus = ActionStatus.无操作;
            //是清空还是保存原值？
            //_originalValues.Clear();

            // 更新原始值为当前值
            //var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //foreach (var property in properties)
            //{
            //    if (!_changedProperties.ContainsKey(property.Name)) continue;
            //    var value = property.GetValue(this);
            //    _originalValues[property.Name] = value is ICloneable cloneable ? cloneable.Clone() : value;
            //}

            //foreach (var propertyName in _changedProperties.Keys.ToList())
            //{
            //    var property = GetType().GetProperty(propertyName);
            //    if (property != null)
            //    {
            //        var value = property.GetValue(this);
            //        _originalValues[propertyName] = value is ICloneable cloneable ? cloneable.Clone() : value;
            //    }
            //}

        }

        #endregion






        #endregion




        #region 字段基类描述对应列表 - 基类实现

        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, string>> _fieldNameListCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, string>>();

        /// <summary>
        /// 外键关系信息缓存
        /// 用于存储每个实体类型的外键关系信息
        /// </summary>
        private static readonly ConcurrentDictionary<Type, List<FKRelationInfo>> _fkRelationsCache = new ConcurrentDictionary<Type, List<FKRelationInfo>>();

        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                var type = this.GetType();
                return _fieldNameListCache.GetOrAdd(type, t => GenerateFieldNameList(t));
            }
            set { /* 如果需要子类覆盖，可以在这里实现 */ }
        }


        //删除时根据这个ID删除图片
        [Description("文件存储信息列表"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [JsonIgnore]
        [XmlIgnore]
        public List<tb_FS_FileStorageInfo> FileStorageInfoList { get; set; } = new List<tb_FS_FileStorageInfo>();


        /// <summary>
        /// 生成字段名称列表（优化版本）
        /// 排除导航属性和忽略字段，提高性能
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>字段名称和描述字典</returns>
        private static ConcurrentDictionary<string, string> GenerateFieldNameList(Type type)
        {
            if (_fieldNameListCache.TryGetValue(type, out var fieldNameList))
            {
                return fieldNameList;
            }
            
            fieldNameList = new ConcurrentDictionary<string, string>();
            
            // 优化：先获取所有属性，然后过滤排除导航属性
            var properties = type.GetProperties()
                .Where(p => !IsNavigationProperty(p))
                .ToList();
            
            foreach (var property in properties)
            {
                var sugarColumnAttr = property.GetCustomAttribute<SugarColumn>(false);
                if (sugarColumnAttr != null && !string.IsNullOrEmpty(sugarColumnAttr.ColumnDescription))
                {
                    // 保留原始逻辑：主键字段的处理保持不变
                    if (sugarColumnAttr.IsPrimaryKey)
                    {
                        //  PrimaryKeyColName = sugarColumnAttr.ColumnName;
                    }
                    fieldNameList.TryAdd(property.Name, sugarColumnAttr.ColumnDescription);
                }
            }
            
            _fieldNameListCache[type] = fieldNameList;
            return fieldNameList;
        }
        
        /// <summary>
        /// 判断属性是否为导航属性（复用UIHelper中的逻辑）
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <returns>是否为导航属性</returns>
        private static bool IsNavigationProperty(PropertyInfo property)
        {
            var sugarColumn = property.GetCustomAttribute<SugarColumn>();
            if (sugarColumn?.IsIgnore == true)
            {
                return true;
            }
            
            var navigateAttr = property.GetCustomAttribute<Navigate>();
            if (navigateAttr != null)
            {
                return true;
            }
            
            // 排除常见的基础属性
            var baseSkipProperties = new[] { "StatusEvaluator", "FieldNameList", "HelpInfos", "RowImage", "ActionStatus", "Selected", "Childs" };
            if (baseSkipProperties.Contains(property.Name))
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 使用表名作为键的外键关系缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, Dictionary<string, FKRelationInfo>> _fkRelationsByTableName = new ConcurrentDictionary<string, Dictionary<string, FKRelationInfo>>();

        /// <summary>
        /// 获取实体的所有外键关系信息
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public virtual List<FKRelationInfo> FKRelations
        {
            get
            {
                var type = this.GetType();
                var tableName = GetTableName(type);

                // 使用表名作为缓存键
                return _fkRelationsByTableName.GetOrAdd(tableName, _ => GenerateFKRelations(type))
                    .Values.ToList();
            }
        }

        /// <summary>
        /// 获取实体类型对应的表名
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>表名</returns>
        private static string GetTableName(Type type)
        {
            // 获取SugarTable特性，如果存在则使用指定的表名，否则使用类型名
            var sugarTable = type.GetCustomAttribute<SugarTable>(false);
            return sugarTable?.TableName ?? type.Name;
        }

        /// <summary>
        /// 生成实体类型的外键关系信息列表
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>外键关系信息字典，键为属性名</returns>
        private static Dictionary<string, FKRelationInfo> GenerateFKRelations(Type type)
        {
            var relations = new Dictionary<string, FKRelationInfo>();

            // 获取所有公共实例属性
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // 获取FKRelationAttribute属性
                var fkAttr = property.GetCustomAttribute<FKRelationAttribute>(false);
                if (fkAttr != null)
                {
                    // 创建外键关系信息对象
                    var relationInfo = new FKRelationInfo
                    {
                        PropertyName = property.Name,
                        FKTableName = fkAttr.FKTableName,
                        FK_IDColName = fkAttr.FK_IDColName,
                        CmbMultiChoice = fkAttr.CmbMultiChoice
                    };

                    // 使用属性名作为字典键，确保不会重复添加
                    if (!relations.ContainsKey(property.Name))
                    {
                        relations.Add(property.Name, relationInfo);
                    }
                }
            }

            return relations;
        }

        /// <summary>
        /// 根据属性名获取外键关系信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>外键关系信息，如果不存在则返回null</returns>
        public virtual FKRelationInfo GetFKRelationByPropertyName(string propertyName)
        {
            var type = this.GetType();
            var tableName = GetTableName(type);

            // 直接从字典中查找，提高性能
            if (_fkRelationsByTableName.TryGetValue(tableName, out var relations))
            {
                relations.TryGetValue(propertyName, out var relationInfo);
                return relationInfo;
            }

            // 如果缓存中没有，则生成并查找
            var generatedRelations = GenerateFKRelations(type);
            _fkRelationsByTableName[tableName] = generatedRelations;

            generatedRelations.TryGetValue(propertyName, out var result);
            return result;
        }

        /// <summary>
        /// 根据外键表名获取外键关系信息列表
        /// </summary>
        /// <param name="tableName">外键表名</param>
        /// <returns>外键关系信息列表</returns>
        public virtual List<FKRelationInfo> GetFKRelationsByTableName(string tableName)
        {
            var entityTypeName = GetTableName(this.GetType());

            // 从缓存中获取当前实体的所有外键关系
            if (_fkRelationsByTableName.TryGetValue(entityTypeName, out var relations))
            {
                // 按外键表名筛选
                return relations.Values.Where(r => r.FKTableName == tableName).ToList();
            }

            // 如果缓存中没有，则生成并筛选
            var generatedRelations = GenerateFKRelations(this.GetType());
            _fkRelationsByTableName[entityTypeName] = generatedRelations;

            return generatedRelations.Values.Where(r => r.FKTableName == tableName).ToList();
        }


        #endregion

        public void SetDetails<C>(List<C> details) where C : class, new()
        {
            // 实现细节
        }

        private DataRowImage _RowImage = new DataRowImage();

        /// <summary>
        /// 表中行数据可能存在的图片列。将来可能会扩展到多张图片11
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public DataRowImage RowImage { get => _RowImage; set => _RowImage = value; }




        private bool? _selected = false;


        /// <summary>
        /// 用于转入单时明细是否选中的逻辑，下面的属性后面优化
        /// 默认不显示，忽略
        /// </summary>
        //[SugarColumn(IsIgnore = true, ColumnDescription = "选择")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(true)]
        public bool? Selected { get => _selected; set => _selected = value; }

        #region 主键相关信息

        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 主键值 通过生成的框架查询时赋值的。新增加保存的逻辑不应该使用他。要注意
        /// </summary>
        [Browsable(false)]
        public long PrimaryKeyID { get; set; }



        //[SugarColumn(IsIgnore = true)]
        ///// <summary>
        ///// 主键值列名
        ///// </summary>
        //[Browsable(false)]
        //public string PrimaryKeyColName { get; set; }


        public string GetPrimaryKeyColName()
        {
            //if (!string.IsNullOrEmpty(PrimaryKeyColName))
            //{
            //    return PrimaryKeyColName;
            //}
            string PrimaryKeyColName = string.Empty;
            // 获取需要跟踪的属性列表（使用缓存）
            var properties = GetCachedProperties();
            // 记录所有需要跟踪的属性的当前值
            foreach (var property in properties)
            {
                var sugarColumnAttr = property.GetCustomAttribute<SugarColumn>(false);
                if (sugarColumnAttr != null)
                {
                    if (sugarColumnAttr.IsPrimaryKey)
                    {
                        PrimaryKeyColName = sugarColumnAttr.ColumnName;
                        return PrimaryKeyColName;
                    }
                }
            }
            return PrimaryKeyColName;
        }



        #endregion




        #region NotifyProperty

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Suppress禁止的意思。 
        /// 禁止通知属性已更改
        /// 默认值为false ，是支持属性更改通知
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public bool SuppressNotifyPropertyChanged { get; set; }
        /// <summary>
        /// 触发属性变更通知
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        // 防重入标志，用于防止状态变更事件处理过程中再次触发属性变更通知导致的循环调用
        private bool _isProcessingStatusChange = false;
        
        protected virtual void OnPropertyChanged(string propertyName, object oldValue = null, object newValue = null)
        {
            try
            {
                // 如果正在处理状态变更且触发源是PropertyChanged事件，则直接返回以避免循环调用
                if (_isProcessingStatusChange && this.PropertyChanged != null)
                {
                    return;
                }
                
                // 标记实体已变更
                HasChanged = true;
                // 检查是否是状态属性
                var statusType = StateManager?.GetStatusType(this);
                bool isStatusProperty = statusType != null && statusType.Name == propertyName;

                // 触发PropertyChanged事件（如果有订阅者且未被抑制）
                if (this.PropertyChanged != null && !SuppressNotifyPropertyChanged)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }

                // 对于状态属性，只有当值确实发生变化时才触发状态变更通知
                if (isStatusProperty)
                {
                    // 检查值是否实际发生变化，避免重复触发
                    bool valueChanged = false;
                    if (oldValue == null && newValue == null)
                    {
                        valueChanged = false;
                    }
                    else if (oldValue == null || newValue == null)
                    {
                        valueChanged = true;
                    }
                    else
                    {
                        valueChanged = !oldValue.Equals(newValue);
                    }
                    
                    if (valueChanged)
                    {
                        TriggerStatusChange(statusType, oldValue, newValue);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，避免影响主业务流程
                System.Diagnostics.Debug.WriteLine($"属性变更通知异常: {ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expr)
        {
            this.OnPropertyChanged(Utils.GetMemberName(expr));
        }

        /// <summary>
        /// 如果没有其他的业务逻辑，对 lambda 表达式比较熟悉的同学可以考虑用以下方法实现属性名称传递 
        ///  SetProperty(ref _TypeName, value, () => this.TypeName);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propField"></param>
        /// <param name="value"></param>
        /// <param name="expr"></param>
        protected void SetProperty<T>(ref T propField, T value, Expression<Func<T>> expr)
        {
            var bodyExpr = expr.Body as System.Linq.Expressions.MemberExpression;
            if (bodyExpr == null)
            {
                throw new ArgumentException("Expression must be a MemberExpression!", "expr");
            }
            var propInfo = bodyExpr.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException("Expression must be a PropertyExpression!", "expr");
            }

            var propName = propInfo.Name;

            T oldValue = propField;
            propField = value;

            // 仅调用一次完整的属性变更通知，包含旧值和新值信息
            this.OnPropertyChanged(propName, oldValue, value);
            HasChanged = true;
        }

        /// <summary>
        ///SetProperty(ref _TypeName, value);设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            // 简化为单次高效比较
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return;

            T oldValue = storage;
            storage = value;
            
            // 添加propertyName空值检查，防止反射调用时抛出异常
            if (!string.IsNullOrEmpty(propertyName))
            {
                // 简化变更记录逻辑
                if (_changedProperties.TryGetValue(propertyName, out var record))
                {
                    record.CurrentValue = value;
                    // 若值变回原始值，移除变更记录
                    if (object.Equals(record.OriginalValue, value))
                    {
                        _changedProperties.TryRemove(propertyName, out _);
                    }
                }
                else if (!object.Equals(oldValue, value)) // 首次变更需检查有效性
                {
                    _changedProperties[propertyName] = new PropertyChangeRecord(originalValue: oldValue, currentValue: value);
                }

                // 合并通知事件
                OnPropertyChanged(propertyName, oldValue, value);
            }
            
            HasChanged = true;
        }

        // 在 BaseEntity 类中添加
        public Dictionary<string, string> GetChangedColumnMappingsWithPK()
        {
            var mappings = GetChangedColumnMappings();

            // 获取主键列名
            string pkColumn = GetPrimaryKeyColName();
            PropertyInfo pkProperty = GetPrimaryKeyProperty();

            // 确保主键列已包含在映射中
            if (!string.IsNullOrEmpty(pkColumn) &&
                !mappings.ContainsKey(pkProperty.Name))
            {
                mappings[pkProperty.Name] = pkColumn;
            }

            return mappings;
        }


        // 新增方法：获取主键属性
        protected PropertyInfo GetPrimaryKeyProperty()
        {
            return GetCachedProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<SugarColumn>()?.IsPrimaryKey == true);
        }

        public Dictionary<string, object> GetPrimaryKeyValues()
        {
            var keys = new Dictionary<string, object>();
            foreach (var prop in GetCachedProperties())
            {
                var attr = prop.GetCustomAttribute<SugarColumn>();
                if (attr?.IsPrimaryKey == true)
                {
                    keys[attr.ColumnName] = prop.GetValue(this);
                }
            }
            return keys;
        }

        #endregion



        public virtual void Save()
        {
            // 保存后重置变更追踪
            ResetChangeTracking();
            HasChanged = false;
        }

        public virtual void Update()
        {
            // 更新后重置变更追踪
            ResetChangeTracking();
            HasChanged = false;

        }


        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public bool HasChanged { get; set; }


        #region 状态管理增强


        private ActionStatus _ActionStatus;
        // 状态变更计数，用于性能监控和调试
        private int _statusChangeCount;



        /// <summary>
        /// 操作状态（ActionStatus）
        /// 增强版说明：
        /// 1. 确保任何状态变更都能触发完整的事件通知链
        /// 2. 统一状态变更事件和属性变更事件的触发机制
        /// 3. 优化事件触发顺序，确保UI能正确响应所有状态变更
        /// 4. 重要：无论状态变更来自直接属性修改还是通过状态管理器，都能触发相同的事件通知
        /// 5. 操作状态变更不会将实体标记为HasChanged，因为它只表示UI操作意图而非实际数据变更
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public ActionStatus ActionStatus
        {
            get { return _ActionStatus; }
            set
            {
                // 性能优化：相同状态快速返回
                if (Equals(_ActionStatus, value))
                {
                    return;
                }

                // 记录前一个状态
                ActionStatus oldStatus = _ActionStatus;

                // 直接设置属性值，不调用SetProperty方法，避免触发变更追踪
                bool wasSuppressed = SuppressNotifyPropertyChanged;
                try
                {
                    // 临时禁止属性变更通知，避免HasChanged被设置为true
                    SuppressNotifyPropertyChanged = true;

                    // 原子操作：设置属性值
                    _ActionStatus = value;

                    // 更新状态变更计数
                    Interlocked.Increment(ref _statusChangeCount);

                    // 再触发PropertyChanged事件，确保任何绑定到该属性的UI元素都能得到更新
                    // 使用安全的事件触发方式
                    OnPropertyChanged(nameof(ActionStatus), oldStatus, value);
                }
                finally
                {
                    // 恢复原始的抑制状态
                    SuppressNotifyPropertyChanged = wasSuppressed;
                }

                // 可选：添加性能警告（当状态变更过于频繁时）
                if (_statusChangeCount > 100 && _statusChangeCount % 50 == 0)
                {
                    // 可以在调试模式下记录频繁状态变更的警告
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"警告：实体 {this.GetType().Name} (ID: {this.PrimaryKeyID}) 的状态变更次数过高：{_statusChangeCount}");
#endif
                }
            }
        }



        #endregion



        /// <summary>
        /// 保存删除的ID
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public List<int> DeleteIDs { get; set; }

        private ConcurrentDictionary<string, string> _HelpInfo;
        /// <summary>
        /// 如果有帮助信息，则在子类的分文件中描写
        /// </summary>
        [Description("对应列帮助信息"), Category("自定属性"), Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        public virtual ConcurrentDictionary<string, string> HelpInfos
        {
            get
            {
                return _HelpInfo;
            }
            set
            {
                _HelpInfo = value;
            }
        }


        // IDataErrorInfo相关代码已移除，避免SqlSugar绑定索引器失败
        // 如果需要验证功能，可以使用其他方式实现，如数据注解或自定义验证方法

        /// <summary>
        /// 取属性名称（列名）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            string pname = string.Empty;
            if (expression != null)
            {
                Expression newexp = expression.Body;
                if (newexp.NodeType == ExpressionType.MemberAccess)
                {
                    if (newexp is MemberExpression member)
                    {
                        pname = member.Member.Name;
                    }
                }
                else
                {
                    if (newexp.NodeType == ExpressionType.Convert)
                    {
                        var cexp = (newexp as UnaryExpression).Operand;
                        if (cexp is MemberExpression member)
                        {
                            pname = member.Member.Name;
                        }
                    }
                }
            }

            return pname;
        }

        public bool IsIdValid(decimal? value)
        {
            bool isValid = true;

            if (value < 0)
            {
                // IDataErrorInfo相关代码已移除
                isValid = false;
            }
            else
            {
                // IDataErrorInfo相关代码已移除
            }

            return isValid;
        }
        public virtual object Clone()
        {
            BaseEntity clone = (BaseEntity)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            clone._changedProperties.Clear();
            clone.HasChanged = false;
            return clone;
        }
        private object CloneValue(object value)
        {
            if (value is ICloneable cloneable) return cloneable.Clone();
            if (value is IEnumerable enumerable && !(value is string))
            {
                // 集合类型创建浅拷贝
                var listType = typeof(List<>).MakeGenericType(value.GetType().GetGenericArguments()[0]);
                return Activator.CreateInstance(listType, enumerable);
            }
            return value;
        }


        #region 提取重点数据

        /// <summary>
        /// 实体，特别是单据有时要保存重点的数据。这里提供一个通用实现
        /// </summary>
        /// <returns></returns>
        public virtual string ToDataContent()
        {
            try
            {
                // 创建一个字典来存储需要记录的重点数据
                var keyData = new Dictionary<string, object>();

                // 获取实体类型
                var type = GetType();

                // 获取所有属性
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // 遍历所有属性，收集需要记录的重点数据
                foreach (var property in properties)
                {
                    // 跳过不应该记录的属性
                    if (ShouldSkipProperty(property))
                        continue;

                    // 获取属性值
                    var value = property.GetValue(this);

                    // 处理集合类型
                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        // 对于集合类型，只记录集合中每个元素的ID和其他关键属性
                        var collection = value as System.Collections.IEnumerable;
                        if (collection != null)
                        {
                            var itemsData = new List<Dictionary<string, object>>();
                            foreach (var item in collection)
                            {
                                if (item != null)
                                {
                                    var itemData = new Dictionary<string, object>();
                                    var itemType = item.GetType();

                                    // 记录ID属性
                                    var idProperty = itemType.GetProperty("ID") ??
                                                     itemType.GetProperty($"{itemType.Name}ID");
                                    if (idProperty != null && idProperty.CanRead)
                                    {
                                        itemData["ID"] = idProperty.GetValue(item);
                                    }

                                    // 记录其他关键属性（根据需要扩展）
                                    var keyProperties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(p => p.Name.Contains("Name") ||
                                                    p.Name.Contains("Code") ||
                                                    p.Name.Contains("Amount") ||
                                                    p.Name.Contains("Quantity"));

                                    foreach (var keyProp in keyProperties)
                                    {
                                        if (keyProp.CanRead && !ShouldSkipProperty(keyProp))
                                        {
                                            itemData[keyProp.Name] = keyProp.GetValue(item);
                                        }
                                    }

                                    itemsData.Add(itemData);
                                }
                            }

                            keyData[property.Name] = itemsData;
                        }
                    }
                    else
                    {
                        // 对于普通属性，直接记录值
                        keyData[property.Name] = value;
                    }
                }

                // 将字典序列化为JSON字符串
                return JsonConvert.SerializeObject(keyData);
            }
            catch (Exception ex)
            {
                // 记录错误，但返回空字符串，不影响主业务流程
                System.Diagnostics.Debug.WriteLine($"生成数据内容失败: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 判断是否应该跳过某个属性
        /// </summary>
        private bool ShouldSkipProperty(PropertyInfo property)
        {
            // 跳过忽略的属性
            var sugarColumnAttr = property.GetCustomAttribute<SugarColumn>();
            if (sugarColumnAttr != null && sugarColumnAttr.IsIgnore)
                return true;

            // 跳过导航属性
            var navigateAttr = property.GetCustomAttribute<Navigate>();
            if (navigateAttr != null)
                return true;

            // 跳过不需要记录的属性（根据需要扩展）
            var skipProperties = new[] { "StatusEvaluator", "FieldNameList", "HelpInfos", "RowImage", "ActionStatus", "Selected", "Childs" };
            if (skipProperties.Contains(property.Name))
                return true;

            return false;
        }
        #endregion

        #region 新状态管理系统方法

        /// <summary>
        /// 状态管理器实例
        /// </summary>
        private IUnifiedStateManager _stateManager;



        /// <summary>
        /// 状态管理器实例（延迟初始化）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [JsonIgnore]
        public IUnifiedStateManager StateManager
        {
            get
            {
                if (_stateManager == null)
                {
                    try
                    {
                        if (ApplicationContext.Current != null)
                        {
                            _stateManager = ApplicationContext.Current.GetRequiredService<IUnifiedStateManager>();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"获取状态管理器失败: {ex.Message}");
                    }
                }
                return _stateManager;
            }
            protected set
            {
                _stateManager = value;
            }
        }



        /// <summary>
        /// 获取实体的当前状态值
        /// </summary>
        /// <returns>当前状态值</returns>
        public virtual Enum GetCurrentStatus()
        {
            try
            {
                var statusType = StateManager.GetStatusType(this);
                var propertyName = statusType.Name;

                if (this.ContainsProperty(propertyName))
                {
                    var value = this.GetPropertyValue(propertyName);
                    if (value != null)
                    {
                        // 将值转换为对应的枚举类型
                        if (Enum.IsDefined(statusType, (int)value))
                        {
                            return (Enum)Enum.ToObject(statusType, (int)value);
                        }
                    }
                }

                // 如果获取失败，返回默认状态
                if (statusType == typeof(DataStatus))
                    return DataStatus.草稿;

                return default(Enum);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取当前状态失败: {ex.Message}");
                return DataStatus.草稿;
            }
        }

        /// <summary>
        /// 检查实体是否包含指定的属性
        /// 优化版本：使用_propertyCache缓存提高性能
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>是否包含该属性</returns>
        public virtual bool ContainsProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return false;

            // 使用缓存的属性列表，避免每次都调用反射
            var cachedProperties = GetCachedProperties();
            return cachedProperties.Any(p => p.Name == propertyName);
        }

        /// <summary>
        /// 获取实体指定属性的值
        /// 优化版本：使用_propertyCache缓存提高性能，并先检查属性是否存在
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值，若属性不存在则返回null</returns>
        public virtual object GetPropertyValue(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            // 使用缓存的属性列表，避免每次都调用反射
            var cachedProperties = GetCachedProperties();
            var property = cachedProperties.FirstOrDefault(p => p.Name == propertyName);

            // 如果属性存在且可读，则获取其值
            return property?.CanRead == true ? property.GetValue(this) : null;
        }

        /// <summary>
        /// 重置状态管理器
        /// </summary>
        public virtual void ResetStateManager()
        {
            try
            {
                _stateManager = null;
                System.Diagnostics.Debug.WriteLine($"实体 {this.GetType().Name} 的状态管理器已重置");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"重置状态管理器失败: {ex.Message}");
            }
        }

        #endregion

    }



    /// <summary>
    /// 属性变更记录类
    /// </summary>
    public class PropertyChangeRecord
    {
        /// <summary>
        /// 原始值
        /// </summary>
        public object OriginalValue { get; }

        /// <summary>
        /// 当前值
        /// </summary>
        public object CurrentValue { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="originalValue">原始值</param>
        /// <param name="currentValue">当前值</param>
        public PropertyChangeRecord(object originalValue, object currentValue)
        {
            OriginalValue = originalValue;
            CurrentValue = currentValue;
        }
    }

}
