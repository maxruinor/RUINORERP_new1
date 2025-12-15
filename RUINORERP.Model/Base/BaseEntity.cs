
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
    public class BaseEntity : INotifyPropertyChanged, IDataErrorInfo//, IStatusProvider
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
        : GetCurrentValue(propertyName);
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


        /// <summary>
        /// 获取属性的当前值
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性的当前值</returns>
        public object GetCurrentValue(string propertyName)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            if (property != null && property.CanRead)
            {
                return property.GetValue(this);
            }

            return null;
        }



        #region 像出库时 成本与分摊双向计算的情况
        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion




        private readonly Stopwatch _performanceStopwatch = new Stopwatch();

        public void StartPerformanceMonitoring()
        {
            _performanceStopwatch.Restart();
        }

        public void StopPerformanceMonitoring(string operationName)
        {
            _performanceStopwatch.Stop();
            Console.WriteLine($"{operationName} took {_performanceStopwatch.ElapsedMilliseconds} ms");
        }

        #region 状态机管理

        /// <summary>
        /// 状态变更事件 - V4版本
        /// 使用StateTransitionEventArgs和StateTransitionResult
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        /// <summary>
        /// 状态变更处理（优化版）
        /// 简化事件触发逻辑，提高执行效率
        /// </summary>
        /// <param name="e">状态转换事件参数</param>
        protected virtual void OnStatusChanged(StateTransitionEventArgs e)
        {
            // 类型安全检查
            if (e == null)
                return;

            try
            {
                // 简化版：只触发本地事件，移除重复的状态管理器事件触发
                // 状态管理器的全局事件应在外部统一管理，避免重复触发
                StatusChanged?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                // 记录异常但不中断流程
                Debug.WriteLine($"触发状态变更事件时发生错误: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 状态变更事件触发方法（高效版）
        /// 提供快速、直接的方式触发状态变更事件
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态值</param>
        /// <param name="newStatus">新状态值</param>
        /// <param name="reason">变更原因（可选）</param>
        /// <param name="userId">用户ID（可选）</param>
        public void TriggerStatusChange(Type statusType, object oldStatus, object newStatus, string reason = null, string userId = null)
        {
            // 优化：快速检查状态是否实际变更，避免不必要的事件触发
            if (oldStatus == null && newStatus == null) return;
            if (oldStatus != null && oldStatus.Equals(newStatus)) return;
                
            try
            {
                // 使用构造函数创建事件参数
                var eventArgs = new StateTransitionEventArgs(
                    this,
                    statusType,
                    oldStatus,
                    newStatus,
                    reason,
                    userId);
                
                // 直接调用OnStatusChanged处理状态变更
                OnStatusChanged(eventArgs);
            }
            catch (Exception ex)
            {
                // 记录异常信息，但不中断执行流程
                Debug.WriteLine($"状态变更触发错误: {ex.Message}");
                Debug.WriteLine($"异常堆栈: {ex.StackTrace}");
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

        private static ConcurrentDictionary<string, string> GenerateFieldNameList(Type type)
        {
            if (_fieldNameListCache.TryGetValue(type, out var fieldNameList))
            {
                return fieldNameList;
            }
            fieldNameList = new ConcurrentDictionary<string, string>();
            foreach (var property in type.GetProperties())
            {
                var sugarColumnAttr = property.GetCustomAttribute<SugarColumn>(false);
                if (sugarColumnAttr != null && !string.IsNullOrEmpty(sugarColumnAttr.ColumnDescription))
                {
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
        /// 表中行数据可能存在的图片列。将来可能会扩展到多张图片
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
        /// 主键值
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
        protected virtual void OnPropertyChanged(string propertyName, object oldValue = null, object newValue = null)
        {
            if (this.PropertyChanged != null && !SuppressNotifyPropertyChanged)
            {
                try
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    HasChanged = true;

                    // 注意：状态变更事件由状态管理器统一管理，不再在此处直接触发
                    // 状态变更事件将在状态属性的setter中通过状态管理器触发
                }
                catch (Exception ex)
                {
                    // 记录异常但不抛出，避免影响主业务流程
                    System.Diagnostics.Debug.WriteLine($"属性变更通知异常: {ex.Message}");
                }
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

            this.OnPropertyChanged(propName, oldValue, value);
            this.OnPropertyChanged(propName);
            HasChanged = true;
        }

        /// <summary>
        ///SetProperty(ref _TypeName, value);
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

            HasChanged = true;

            // 合并通知事件
            OnPropertyChanged(propertyName, oldValue, value);

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
        private ActionStatus _previousActionStatus;
        // 状态变更计数，用于性能监控和调试
        private int _statusChangeCount;
        
        /// <summary>
        /// 获取状态变更次数，用于性能监控
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public int StatusChangeCount => _statusChangeCount;

       
        /// <summary>
        /// 操作状态（ActionStatus）
        /// 优化说明：
        /// 1. 增强状态变更检测
        /// 2. 改进事件通知机制
        /// 3. 添加性能监控统计
        /// 4. 优化频繁变更场景处理
        /// 5. 重要：操作状态变更不会将实体标记为HasChanged，因为它只表示UI操作意图而非实际数据变更
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
                _previousActionStatus = _ActionStatus;
                
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
                    
                    // 手动触发PropertyChanged事件，但不会标记实体为已更改
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ActionStatus"));
                    }
                    
                    // 触发状态变更事件
                    TriggerStatusChange(typeof(ActionStatus), _previousActionStatus, value);
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


        // 用于保存验证错误信息。key 保存所验证的字段名称；value 保存对应的字段的验证错误信息列表
        private Dictionary<String, List<String>> errors = new Dictionary<string, List<string>>();

        private const string NAME_ERROR = "name 不能包含空格";
        private const string ID_ERROR = "id 不能小于 10";



        public void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
                errors[propertyName].Add(error);
        }

        public void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) && errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);

                if (errors[propertyName].Count == 1)
                    errors.Remove(propertyName);
            }
        }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public string Error
        {
            get { return errors.Count > 0 ? "有验证错误" : ""; }
        }



        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public string this[string propertyName]
        {
            get
            {
                if (errors.ContainsKey(propertyName))
                    return string.Join(Environment.NewLine, errors[propertyName]);
                else
                    return null;
            }
        }

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
                AddError("TotalAmount", ID_ERROR);
                isValid = false;
            }
            else
            {
                RemoveError("TotalAmount", ID_ERROR);
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
                Console.WriteLine($"生成数据内容失败: {ex.Message}");
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
        /// 获取实体的数据状态
        /// </summary>
        /// <returns>数据状态</returns>
        public virtual DataStatus GetDataStatus()
        {
            try
            {
                // 首先检查是否有DataStatus属性
                var dataStatusProperty = this.GetType().GetProperty("DataStatus");
                if (dataStatusProperty != null && dataStatusProperty.PropertyType == typeof(DataStatus))
                {
                    return (DataStatus)dataStatusProperty.GetValue(this);
                }

                // 默认返回草稿状态
                return DataStatus.草稿;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取实体数据状态失败: {ex.Message}");
                return DataStatus.草稿;
            }
        }



 





        /// <summary>
        /// 获取实体的状态类型
        /// </summary>
        /// <returns>状态类型</returns>
        public virtual Type GetStatusType()
        {
            try
            {
                // 检查实体是否包含各种状态类型的属性
                if (this.ContainsProperty(typeof(DataStatus).Name))
                    return typeof(DataStatus);

                if (this.ContainsProperty(typeof(PrePaymentStatus).Name))
                    return typeof(PrePaymentStatus);

                if (this.ContainsProperty(typeof(ARAPStatus).Name))
                    return typeof(ARAPStatus);

                if (this.ContainsProperty(typeof(PaymentStatus).Name))
                    return typeof(PaymentStatus);

                if (this.ContainsProperty(typeof(StatementStatus).Name))
                    return typeof(StatementStatus);

                // 默认返回DataStatus类型
                return typeof(DataStatus);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取状态类型失败: {ex.Message}");
                return typeof(DataStatus);
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
                var statusType = GetStatusType();
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
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>是否包含该属性</returns>
        public virtual bool ContainsProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return false;

            return this.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// 获取实体指定属性的值
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        public virtual object GetPropertyValue(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            var property = this.GetType().GetProperty(propertyName);
            return property?.GetValue(this);
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
