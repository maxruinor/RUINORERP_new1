
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Model.Base.StatusManager.Core;
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

            //// 如果原始值字典中没有，尝试从当前属性获取
            //PropertyInfo property = GetType().GetProperty(propertyName);
            //if (property != null && property.CanRead)
            //{
            //    return property.GetValue(this);
            //}
            //return null;
        }


        /// <summary>
        /// 获取真实的变化
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, (object Original, object Current)> GetEffectiveChanges()
        {
            //return _changedProperties.ToDictionary(
            //    kvp => kvp.Key,
            //    kvp => (kvp.Value.OriginalValue, kvp.Value.CurrentValue)
            //);

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

    
        #region 状态字段事件通知





 
        

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



     
        public BaseEntity()
        {
            // 初始化新的状态管理系统
            InitializeStateManager(GetType());
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
            //// 获取需要跟踪的属性列表（使用缓存）
            //var properties = GetCachedProperties();

            //// 记录所有需要跟踪的属性的当前值
            //foreach (var property in properties)
            //{
            //    try
            //    {
            //        var value = property.GetValue(this);

            //        // 处理特殊类型：深拷贝可克隆对象
            //        if (value is ICloneable cloneable)
            //        {
            //            _originalValues[property.Name] = cloneable.Clone();
            //        }
            //        // 处理集合类型：创建副本
            //        else if (value is System.Collections.IEnumerable enumerable &&
            //                 !(value is string))
            //        {
            //            // 特殊处理List类型
            //            if (value is System.Collections.IList list)
            //            {
            //                var newList = (System.Collections.IList)Activator.CreateInstance(list.GetType());
            //                foreach (var item in list)
            //                {
            //                    newList.Add(item);
            //                }
            //                _originalValues[property.Name] = newList;
            //            }
            //            else
            //            {
            //                _originalValues[property.Name] = enumerable.Cast<object>().ToList();
            //            }
            //        }
            //        // 其他类型直接存储
            //        else
            //        {
            //            _originalValues[property.Name] = value;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // 记录错误但继续执行
            //        Debug.WriteLine($"记录属性 {property.Name} 原始值时出错: {ex.Message}");
            //    }
            //}

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




        private List<object> childs = new List<object>();

        //[SugarColumn(IsIgnore = true)]
        //public List<object> Childs { get => childs; set => childs = value; }

        #region NotifyProperty

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 操作性状态变更事件
        /// </summary>
        public event EventHandler<ActionStatusChangedEventArgs> ActionStatusChanged;

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
                    
                    // 如果是ActionStatus属性变更，触发ActionStatusChanged事件
                    if (propertyName == nameof(ActionStatus) && ActionStatusChanged != null)
                    {
                        var eventArgs = new ActionStatusChangedEventArgs(
                            (ActionStatus)oldValue, 
                            (ActionStatus)newValue);
                        ActionStatusChanged(this, eventArgs);
                    }
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
        
        /// <summary>
        /// 操作状态码,实际的属性变化事件中，调用OnPropertyChanged方法
        /// 【已过时】请使用新的状态管理体系 - 参见RUINORERP.Model.Base.StateManager命名空间
        /// 替代方案：使用IStatusProvider.GetOperationStatus()
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public ActionStatus ActionStatus
        {
            get { return _ActionStatus; }
            set
            {
                _previousActionStatus = _ActionStatus;
                SetProperty(ref _ActionStatus, value);
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
        /// 初始化状态管理系统
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="initialStatus">初始状态</param>
        public virtual void InitializeStateManager(Type entityType, DataStatus initialStatus = DataStatus.新建)
        {
            // 如果没有指定初始状态，尝试从子类的DataStatus属性获取
            if (initialStatus == DataStatus.新建)
            {
                var dataStatusProperty = GetDataStatusProperty();
                if (dataStatusProperty != null)
                {
                    var value = (DataStatus?)dataStatusProperty.GetValue(this);
                    if (value.HasValue)
                    {
                        initialStatus = value.Value;
                    }
                }
            }
            
            // 创建状态管理器
            var options = new StateManagerOptions
            {
                EntityType = entityType,
                EnableTransitionLogging = true,
                EnableTransitionValidation = true,
                EnableStatusChangedEvents = true
            };
            
            // 初始化状态转换规则
            StateTransitionRules.InitializeDefaultRules(options.TransitionRules);
            
            // 初始化状态管理器
            // 注意：这里暂时不直接实例化状态管理器，而是提供初始化逻辑
            // 实际的状态管理器实例化将在UI层通过工厂模式完成
            
            // 记录状态管理器初始化信息
            _stateManagerInitialized = true;
        }
        
        /// <summary>
        /// 状态管理器是否已初始化
        /// </summary>
        private bool _stateManagerInitialized = false;

        /// <summary>
        /// 获取状态管理器是否已初始化
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [JsonIgnore]
        [XmlIgnore]
        public bool IsStateManagerInitialized => _stateManagerInitialized;

        /// <summary>
        /// 获取DataStatus属性信息
        /// </summary>
        /// <returns>DataStatus属性信息，如果不存在则返回null</returns>
        protected virtual PropertyInfo GetDataStatusProperty()
        {
            var type = GetType();
            var property = type.GetProperty("DataStatus");
            
            // 验证属性类型是否为DataStatus?
            if (property != null && property.PropertyType == typeof(DataStatus?))
            {
                return property;
            }
            
            return null;
        }

        /// <summary>
        /// 获取当前实体的数据状态
        /// </summary>
        /// <returns>当前数据状态</returns>
        public virtual DataStatus? GetCurrentDataStatus()
        {
            var dataStatusProperty = GetDataStatusProperty();
            if (dataStatusProperty != null)
            {
                return (DataStatus?)dataStatusProperty.GetValue(this);
            }
            
            return null;
        }

        /// <summary>
        /// 设置当前实体的数据状态
        /// </summary>
        /// <param name="status">要设置的状态</param>
        /// <returns>设置是否成功</returns>
        public virtual bool SetCurrentDataStatus(DataStatus status)
        {
            var dataStatusProperty = GetDataStatusProperty();
            if (dataStatusProperty != null && dataStatusProperty.CanWrite)
            {
                dataStatusProperty.SetValue(this, status);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 检查是否可以转换到指定的数据状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public virtual bool CanTransitionToStatus(DataStatus targetStatus)
        {
            var currentStatus = GetCurrentDataStatus();
            if (currentStatus == null)
            {
                // 如果当前状态为空，只允许转换到新建状态
                return targetStatus == DataStatus.新建;
            }
            
            // 这里可以添加更复杂的状态转换逻辑
            // 基本的状态转换规则
            switch (currentStatus.Value)
            {
                case DataStatus.新建:
                    return targetStatus == DataStatus.草稿 || targetStatus == DataStatus.确认;
                    
                case DataStatus.草稿:
                    return targetStatus == DataStatus.新建 || targetStatus == DataStatus.确认 || targetStatus == DataStatus.作废;
                    
                case DataStatus.确认:
                    return targetStatus == DataStatus.完结 || targetStatus == DataStatus.作废;
                    
                case DataStatus.完结:
                    return false; // 完结状态不能再转换
                    
                case DataStatus.作废:
                    return false; // 作废状态不能再转换
                    
                default:
                    return false;
            }
        }

        /// <summary>
        /// 尝试转换到指定的数据状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>转换是否成功</returns>
        public virtual bool TryTransitionToStatus(DataStatus targetStatus)
        {
            if (!CanTransitionToStatus(targetStatus))
            {
                return false;
            }
            
            return SetCurrentDataStatus(targetStatus);
        }

        /// <summary>
        /// 获取状态转换历史记录
        /// </summary>
        /// <returns>状态转换历史记录</returns>
        public virtual List<StateTransitionRecord> GetStateTransitionHistory()
        {
            // 这里应该从状态管理器获取历史记录
            // 暂时返回空列表
            return new List<StateTransitionRecord>();
        }
        
        /// <summary>
        /// 获取实体状态描述
        /// </summary>
        /// <returns>状态描述文本</returns>
        public virtual string GetStatusDescription()
        {
            var status = GetCurrentDataStatus();
            if (status == null)
            {
                return "未知状态";
            }
            
            switch (status.Value)
            {
                case DataStatus.新建:
                    return "新建";
                case DataStatus.草稿:
                    return "草稿";
                case DataStatus.确认:
                    return "已确认";
                case DataStatus.完结:
                    return "已完结";
                case DataStatus.作废:
                    return "已作废";
                default:
                    return status.Value.ToString();
            }
        }
        
        /// <summary>
        /// 检查实体是否处于可编辑状态
        /// </summary>
        /// <returns>是否可编辑</returns>
        public virtual bool IsEditable()
        {
            var status = GetCurrentDataStatus();
            if (status == null)
            {
                return true; // 无状态时默认可编辑
            }
            
            // 新建和草稿状态可编辑
            return status.Value == DataStatus.新建 || status.Value == DataStatus.草稿;
        }
        
        /// <summary>
        /// 检查实体是否处于终态（完结或作废）
        /// </summary>
        /// <returns>是否处于终态</returns>
        public virtual bool IsFinalState()
        {
            var status = GetCurrentDataStatus();
            if (status == null)
            {
                return false;
            }
            
            return status.Value == DataStatus.完结 || status.Value == DataStatus.作废;
        }
        
        /// <summary>
        /// 检查实体是否已确认（已确认、完结或作废）
        /// </summary>
        /// <returns>是否已确认</returns>
        public virtual bool IsConfirmed()
        {
            var status = GetCurrentDataStatus();
            if (status == null)
            {
                return false;
            }
            
            return status.Value == DataStatus.确认 || 
                   status.Value == DataStatus.完结 || 
                   status.Value == DataStatus.作废;
        }
        
        /// <summary>
        /// 获取可执行的操作列表
        /// </summary>
        /// <returns>可执行的操作列表</returns>
        public virtual List<string> GetAvailableActions()
        {
            var actions = new List<string>();
            var status = GetCurrentDataStatus();
            
            if (status == null)
            {
                // 无状态时默认可执行所有操作
                actions.AddRange(new[] { "保存", "提交", "删除", "修改" });
                return actions;
            }
            
            switch (status.Value)
            {
                case DataStatus.新建:
                    actions.AddRange(new[] { "保存", "提交", "删除", "修改" });
                    break;
                    
                case DataStatus.草稿:
                    actions.AddRange(new[] { "保存", "提交", "删除", "修改" });
                    break;
                    
                case DataStatus.确认:
                    actions.AddRange(new[] { "完结", "作废" });
                    break;
                    
                case DataStatus.完结:
                    // 完结状态下不可执行任何操作
                    break;
                    
                case DataStatus.作废:
                    // 作废状态下不可执行任何操作
                    break;
            }
            
            return actions;
        }

        #endregion

    }



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





}
