
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.Base;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace RUINORERP.Model.Base
{

    /// <summary>
    /// 用于查询传参数条件的实体基类
    /// </summary>
    public class BaseEntityDto : DynamicEntityDto,INotifyPropertyChanged//, IDataErrorInfo - 移除IDataErrorInfo接口，避免SqlSugar绑定索引器失败
    {
       


        #region

        private string querymsg = string.Empty;


        #endregion



        [SugarColumn(IsIgnore = true)]
        public bool HasChanged { get; set; }
        #region NotifyProperty

        public event PropertyChangedEventHandler PropertyChanged;


        [SugarColumn(IsIgnore = true)]
        public bool SuppressNotifyPropertyChanged { get; set; }





        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null && !SuppressNotifyPropertyChanged)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                HasChanged = true;
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
            if (object.Equals(storage, value)) return;
            storage = value;
            this.OnPropertyChanged(propertyName);
            HasChanged = true;
        }
        #endregion


 
    


        private ConcurrentDictionary<string, string> _HelpInfo;
        /// <summary>
        /// 如果有帮助信息，则在子类的分文件中描写
        /// </summary>
        [Description("对应列帮助信息"), Category("自定属性"), Browsable(true)]
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



 

        [SugarColumn(IsIgnore = true)]
        public string Querymsg { get => querymsg; set => querymsg = value; }

        // IDataErrorInfo相关代码已移除，避免SqlSugar绑定索引器失败
        // 如果需要验证功能，可以使用其他方式实现，如数据注解或自定义验证方法

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

    }
}
