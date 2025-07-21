
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.Model;
using RUINORERP.Model.Base;
using SharpYaml.Tokens;
using SqlSugar;
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
using System.Xml.Serialization;
namespace RUINORERP.Model
{
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


        // 记录原始值的字典
        //private readonly ConcurrentDictionary<string, object> _originalValues = new ConcurrentDictionary<string, object>();
        private readonly Dictionary<string, object> _originalValues = new Dictionary<string, object>();
        // 记录已变更属性的集合  
        // 跟踪已修改的属性（只需要知道哪些属性被修改过）
        //private readonly ConcurrentDictionary<string, object> _changedProperties = new ConcurrentDictionary<string, object>();
        //private readonly Dictionary<string, object> _changedProperties = new Dictionary<string, object>();
       // private readonly HashSet<string> _changedProperties = new HashSet<string>();
        // 获取变更属性集合


        /// <summary>
        /// 获取所有已变更属性的名称集合
        /// 获取所有已修改的属性名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public IEnumerable<string> ChangedProperties => _changedProperties.Keys;

 

        // 标记属性已修改（供特殊映射场景使用）
        public void SetPropertyModified(string propertyName)
        {
            if (!_changedProperties.Keys.Contains(propertyName))
            {
                var property = GetType().GetProperty(propertyName);
                if (property != null)
                {
                    // 记录原始值（如果尚未记录）
                    if (!_originalValues.ContainsKey(propertyName))
                    {
                        _originalValues[propertyName] =
                            CloneValue(property.GetValue(this));
                    }

                    _changedProperties.Keys.Add(propertyName);
                    HasChanged = true;
                }
            }
        }



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


        // 追踪属性变更的标志
        private bool _trackChanges = true;

        /// <summary>
        /// 是否启用属性变更追踪，默认为true
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public bool TrackChanges
        {
            get => _trackChanges;
            set => _trackChanges = value;
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
            if (_originalValues.TryGetValue(propertyName, out object value))
            {
                return value;
            }

            // 如果原始值字典中没有，尝试从当前属性获取
            PropertyInfo property = GetType().GetProperty(propertyName);
            if (property != null && property.CanRead)
            {
                return property.GetValue(this);
            }

            return null;
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

        #endregion


        #region 像出库时 成本与分摊双向计算的情况
        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region  状态管理相关代码
        #region 状态字段事件通知





        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        protected virtual void OnPropertyChangedStatus(string propertyName, object oldValue, object newValue)
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(propertyName, oldValue, newValue));
        }
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            // 触发常规属性变更事件 已经调用 了。
            // 合并通知逻辑
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // 检查是否是状态属性变更
            if (propertyName == _statusPropertyName && !oldValue.Equals(newValue))
            {
                // 状态变更处理
                //OnPropertyChangedStatus(propertyName, oldValue, newValue);
            }
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



        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public IStatusEvaluator StatusEvaluator { get => _statusEvaluator; set => _statusEvaluator = value; }

        private string _statusPropertyName;
        private Type _statusEnumType;


        private IStatusEvaluator _statusEvaluator;

        public BaseEntity()
        {
            /*
            // 默认使用基础状态提供器
            if (this.FieldNameList.Keys.Contains(nameof(DataStatus)))
            {
                StatusEvaluator = new BusinessStatusEvaluator();
                StatusEvaluator.CurrentStatus = (DataStatus)ReflectionHelper.GetPropertyValue(this, "DataStatus").ToInt();
                StatusEvaluator.ApprovalResult = ReflectionHelper.GetPropertyValue(this, "ApprovalResults").ToBool();
                _statusPropertyName = nameof(DataStatus);
                _statusEnumType = typeof(DataStatus);
                //ApprovalResult = StatusEvaluator.ApprovalResult;
                StatusEvaluator.StatusChanged += (s, e) => OnPropertyChanged(nameof(StatusEvaluator.CurrentStatus));
            }
            else if (this.FieldNameList.Keys.Contains(nameof(PaymentStatus)))
            {
                StatusEvaluator = new FinancialStatusEvaluator();
                StatusEvaluator.CurrentStatus = (PaymentStatus)ReflectionHelper.GetPropertyValue(this, "PaymentStatus").ToInt();
                StatusEvaluator.ApprovalResult = ReflectionHelper.GetPropertyValue(this, "ApprovalResults").ToBool();
                _statusPropertyName = nameof(PaymentStatus);
                _statusEnumType = typeof(PaymentStatus);
                //ApprovalResult = StatusEvaluator.ApprovalResult;
                StatusEvaluator.StatusChanged += (s, e) => OnPropertyChanged(nameof(StatusEvaluator.CurrentStatus));
            }
            else
            {
                //是不是要给一个基础啥都没有的？ 或要判断_statusEvaluator空值情况
            }

            // 绑定状态变化事件,实体中的字段变化就会反应到到评估计算器中
            this.StatusChanged += (s, e) =>
            {
                if (e.PropertyName == _statusPropertyName && _statusEnumType != null)
                {
                    // 将新值转换为对应的枚举类型
                    object newStatus = Enum.ToObject(_statusEnumType, e.NewValue);
                    StatusEvaluator.CurrentStatus = (Enum)newStatus;
                }

                //if (e.PropertyName == nameof(DataStatus))
                //{
                //    _statusEvaluator.CurrentStatus = (Enum)Convert.ChangeType(e.NewValue, typeof(Enum));
                //}

                //if (e.PropertyName == nameof(PaymentStatus))
                //{
                //    _statusEvaluator.CurrentStatus = (Enum)Convert.ChangeType(e.NewValue, typeof(Enum));
                //}
            };
            */

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
        /// 标记业务操作开始（如加载单据后），记录当前状态为原始值
        /// 在加载数据后必须调用BeginOperation
        /// </summary>
        //public void BeginOperation()
        //{
        //    // 清除历史记录
        //    _originalValues.Clear();
        //    _changedProperties.Clear();

        //    // 获取需要跟踪的属性列表（使用缓存）
        //    var properties = GetCachedProperties();

        //    // 记录所有需要跟踪的属性的当前值
        //    foreach (var property in properties)
        //    {
        //        var value = property.GetValue(this);
        //        _originalValues[property.Name] = value is ICloneable cloneable
        //            ? cloneable.Clone()
        //            : value;
        //    }

        //    HasChanged = false;
        //}

        /// <summary>
        /// 开始业务操作，记录当前状态为原始值
        /// </summary>
        public void BeginOperation()
        {
            // 清除历史状态
            _originalValues.Clear();
            _changedProperties.Clear();

            // 获取需要跟踪的属性列表（使用缓存）
            var properties = GetCachedProperties();

            // 记录所有需要跟踪的属性的当前值
            foreach (var property in properties)
            {
                try
                {
                    var value = property.GetValue(this);

                    // 处理特殊类型：深拷贝可克隆对象
                    if (value is ICloneable cloneable)
                    {
                        _originalValues[property.Name] = cloneable.Clone();
                    }
                    // 处理集合类型：创建副本
                    else if (value is System.Collections.IEnumerable enumerable &&
                             !(value is string))
                    {
                        // 特殊处理List类型
                        if (value is System.Collections.IList list)
                        {
                            var newList = (System.Collections.IList)Activator.CreateInstance(list.GetType());
                            foreach (var item in list)
                            {
                                newList.Add(item);
                            }
                            _originalValues[property.Name] = newList;
                        }
                        else
                        {
                            _originalValues[property.Name] = enumerable.Cast<object>().ToList();
                        }
                    }
                    // 其他类型直接存储
                    else
                    {
                        _originalValues[property.Name] = value;
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但继续执行
                    Debug.WriteLine($"记录属性 {property.Name} 原始值时出错: {ex.Message}");
                }
            }

            HasChanged = false;
        }

        /// <summary>
        /// 接受变更，将当前值设为新的原始值（保存后调用）
        /// 在保存数据后必须调用AcceptChanges
        /// </summary>
        public void AcceptChanges()
        {
            foreach (var propName in _changedProperties.Keys.ToList())
            {
                var currentValue = GetCurrentValue(propName);
                _originalValues[propName] = CloneValue(currentValue);


                //var property = GetType().GetProperty(propName);
                //if (property != null)
                //{
                //    var value = property.GetValue(this);
                //    _originalValues[propName] = value is ICloneable cloneable
                //        ? cloneable.Clone()
                //        : value;
                //}
            }
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


        /// <summary>
        /// 重置变更追踪状态
        /// </summary>
        //public void ResetChanges()
        //{
        //    foreach (var propName in _changedProperties)
        //    {
        //        if (_originalValues.TryGetValue(propName, out var original))
        //        {
        //            var property = GetType().GetProperty(propName);
        //            property?.SetValue(this, original);
        //        }
        //    }
        //    //ResetChangeTracking();
        //    _changedProperties.Clear();
        //}


        // 新增：初始化原始值快照
        private void InitializeOriginalValues()
        {
            // var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var properties = GetCachedProperties();
            foreach (var property in properties)
            {
                // 忽略不应跟踪的属性
                if (ShouldSkipTracking(property)) continue;

                var value = property.GetValue(this);
                _originalValues[property.Name] = value is ICloneable cloneable ? cloneable.Clone() : value;
            }
        }
        // 新增：检查属性是否应跳过跟踪
        private bool ShouldSkipTracking(PropertyInfo property)
        {
            // 跳过标记为忽略的属性
            var sugarColumnAttr = property.GetCustomAttribute<SugarColumn>();
            if (sugarColumnAttr != null && sugarColumnAttr.IsIgnore) return true;

            // 跳过导航属性
            if (property.GetCustomAttribute<Navigate>() != null) return true;

            // 跳过特定属性
            var skipProperties = new[] { "StatusEvaluator", "FieldNameList", "HelpInfos", "RowImage", "ActionStatus" };
            return skipProperties.Contains(property.Name);
        }
        // 新增：检查值是否实际变化
        private bool IsValueChanged<T>(string propertyName, object newValue)
        {
            if (!_originalValues.TryGetValue(propertyName, out var originalValue))
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


        // 允许子类替换状态提供器
        protected void SetStatusProvider(IStatusEvaluator statusEvaluator)
        {
            StatusEvaluator = statusEvaluator;
            StatusEvaluator.StatusChanged += (s, e) => OnPropertyChanged(nameof(StatusEvaluator.CurrentStatus));
        }


        //private Enum _currentStatus;

        //[SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        //public Enum CurrentStatus
        //{
        //    get
        //    {
        //        if (StatusEvaluator == null)
        //        {
        //            return null;
        //        }
        //        return _currentStatus;
        //    }
        //    set
        //    {
        //        SetProperty(ref _currentStatus, value);
        //    }
        //}

        //public Enum CurrentStatus => _statusProvider.CurrentStatus;

        //[SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        //public bool ApprovalResult { get; set; }

        //event EventHandler<StatusChangedEventArgs> IStatusProvider.StatusChanged
        //{
        //    add => StatusEvaluator.StatusChanged += value;
        //    remove => StatusEvaluator.StatusChanged -= value;
        //}


        #endregion

        #endregion




        #region 字段基类描述对应列表 - 基类实现

        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, string>> _fieldNameListCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, string>>();

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




        private List<object> childs = new List<object>();

        //[SugarColumn(IsIgnore = true)]
        //public List<object> Childs { get => childs; set => childs = value; }

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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            // 删除此方法或保留为空
            //if (this.PropertyChanged != null && !SuppressNotifyPropertyChanged)
            //{
            //    try
            //    {
            //        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //        HasChanged = true;
            //    }
            //    catch (Exception ex)
            //    {


            //    }

            //}
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
            /*
            #region 检测判断是否是相同的值
            if (EqualityComparer<T>.Default.Equals(storage, value)) return;

            if (object.Equals(storage, value)) return;

            // 首先检查引用是否相等（同一实例）
            if (ReferenceEquals(storage, value))
                return;

            // 如果都是 null，认为相等
            if (storage == null && value == null)
                return;

            // 使用适当的比较方法
            bool areEqual;
            // 特殊处理字符串类型
            if (typeof(T) == typeof(string))
            {
                areEqual = string.Equals(storage as string, value as string);
            }
            // 特殊处理集合类型（如果需要）
            else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(typeof(T)))
            {
                // 这里可以实现更复杂的集合比较逻辑
                areEqual = object.Equals(storage, value);
            }
            else
            {
                // 使用默认比较器
                areEqual = EqualityComparer<T>.Default.Equals(storage, value);
            }

            if (areEqual)
                return;

            #endregion
            */

    


            // 简化为单次高效比较
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return;

            T oldValue = storage;
            storage = value;

            // 首次变更时记录原始值
            //if (!_originalValues.ContainsKey(propertyName))
            //    _originalValues[propertyName] = oldValue;

            //_changedProperties.Add(propertyName);

            // 自动记录原始值（首次变更时）
            if (!_changedProperties.ContainsKey(propertyName))
            {
                _changedProperties[propertyName] = new PropertyChangeRecord(
                    originalValue: CloneValue(storage),
                    currentValue: value
                );
            }
            else
            {
                _changedProperties[propertyName].CurrentValue = value;
            }

            storage = value;
            HasChanged = true;

            // 合并通知事件
            OnPropertyChanged(propertyName, oldValue, value);

            /*
            // 检查值是否实际变化
            bool isValueChanged = IsValueChanged<T>(propertyName, value);

            storage = value;
            //T oldValue = storage;

            if (isValueChanged)
            {
                // 标记属性为已修改
                _changedProperties[propertyName] = true;
                HasChanged = true;
                _changeHistory[propertyName] = new ValueChangeRecord((object)storage, value);
            }
            else if (_changedProperties.ContainsKey(propertyName))
            {
                // 如果值改回原始值，移除修改标记
                _changedProperties.TryRemove(propertyName, out _);
                // 如果没有其他修改，重置HasChanged
                if (_changedProperties.IsEmpty) HasChanged = false;
            }

            OnPropertyChanged(propertyName, (object)storage, value);
            this.OnPropertyChanged(propertyName);*/
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


        // 定义一个事件，用于通知外部实体属性值的变化
        public event EventHandler<ActionStatusChangedEventArgs> ActionStatusChanged;

        private ActionStatus _ActionStatus;
        private ActionStatus _previousActionStatus;
        /// <summary>
        /// 操作状态码,实际的属性变化事件中，调用OnPropertyChanged方法
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public ActionStatus ActionStatus
        {
            get { return _ActionStatus; }
            set
            {
                _previousActionStatus = _ActionStatus;
                OnActionStatusChanged(_previousActionStatus, value);
                SetProperty(ref _ActionStatus, value);
            }
        }


        protected virtual void OnActionStatusChanged(ActionStatus oldValue, ActionStatus newValue)
        {
            if (object.Equals(oldValue, newValue)) return;
            // 触发事件，通知外部实体属性值的变化
            ActionStatusChanged?.Invoke(this, new ActionStatusChangedEventArgs(oldValue, newValue));
        }



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

            //clone.ResetChangeTracking(); // 重置克隆对象的变更状态

            // 重置克隆对象的变更追踪状态
            clone._originalValues.Clear();
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
            var skipProperties = new[] { "StatusEvaluator", "FieldNameList", "HelpInfos", "RowImage" };
            if (skipProperties.Contains(property.Name))
                return true;

            return false;
        }
        #endregion

    }


    // 使用专用类存储变更信息
    public class PropertyChangeRecord
    {
        public object OriginalValue { get; }
        public object CurrentValue { get; set; }

        public PropertyChangeRecord(object originalValue, object currentValue)
        {
            OriginalValue = originalValue;
            CurrentValue = currentValue;
        }
    }



    // 定义一个事件参数类，用于传递新旧值
    public class ActionStatusChangedEventArgs : EventArgs
    {
        public ActionStatus OldValue { get; }
        public ActionStatus NewValue { get; }

        public ActionStatusChangedEventArgs(ActionStatus oldValue, ActionStatus newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

}
