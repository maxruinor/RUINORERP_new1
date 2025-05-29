
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
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
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // 检查是否是状态属性变更
            if (propertyName == _statusPropertyName && !oldValue.Equals(newValue))
            {
                //OnPropertyChangedStatus(propertyName, oldValue, newValue);
            }
        }


        #endregion
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


        #region 字段列表
        //private ConcurrentDictionary<string, string> fieldNameList;

        ///// <summary>
        ///// 表列名的中文描述集合
        ///// </summary>
        //[Description("列名中文描述"), Category("自定属性")]
        //[SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        //[JsonIgnore]
        //[XmlIgnore]
        //public virtual ConcurrentDictionary<string, string> FieldNameList
        //{
        //    get
        //    {
        //        return fieldNameList;
        //    }
        //    set
        //    {
        //        fieldNameList = value;
        //    }

        //}





        #endregion

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
            var fieldNameList = new ConcurrentDictionary<string, string>();
            foreach (var property in type.GetProperties())
            {
                var sugarColumnAttr = property.GetCustomAttribute<SugarColumn>(false);
                if (sugarColumnAttr != null && !string.IsNullOrEmpty(sugarColumnAttr.ColumnDescription))
                {
                    fieldNameList.TryAdd(property.Name, sugarColumnAttr.ColumnDescription);
                }
            }
            return fieldNameList;
        }

        #endregion

        public void SetDetails<C>(List<C> details) where C : class, new()
        {

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
        //        [SugarColumn(IsIgnore = true, ColumnDescription = "选择")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(true)]
        public bool? Selected { get => _selected; set => _selected = value; }

        /// <summary>
        /// 审核数据
        /// </summary>
        // [SugarColumn(IsIgnore = true)]
        // public ApprovalData approvalData { get; set; }

        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 主键值
        /// </summary>
        [Browsable(false)]
        public long PrimaryKeyID { get; set; }

        private List<object> childs = new List<object>();

        //[SugarColumn(IsIgnore = true)]
        //public List<object> Childs { get => childs; set => childs = value; }

        #region NotifyProperty

        public event PropertyChangedEventHandler PropertyChanged;


        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public bool SuppressNotifyPropertyChanged { get; set; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null && !SuppressNotifyPropertyChanged)
            {
                try
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    HasChanged = true;
                }
                catch (Exception ex)
                {


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
            propField = value;
            this.OnPropertyChanged(propName);
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
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return;

            if (object.Equals(storage, value)) return;
            storage = value;
            T oldValue = storage;
            OnPropertyChanged(propertyName, oldValue, value);
            this.OnPropertyChanged(propertyName);
            HasChanged = true;
        }
        #endregion
        public virtual void Save()
        {
            HasChanged = false;
        }

        public virtual void Update()
        {
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

            //string pname = string.Empty;
            //var unary = expression.Body as UnaryExpression;
            //var exp = (unary as UnaryExpression).Operand;
            //if (exp is MemberExpression member)
            //{
            //    pname = member.Member.Name;
            //}
            // return pname;
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
            BaseEntity loctype = (BaseEntity)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
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
