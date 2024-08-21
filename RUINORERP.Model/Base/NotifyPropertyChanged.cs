using RUINORERP.Global;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace RUINORERP.Model.Base
{
    /*
     (1). 在其他属性的 set 方法里给其他属性赋值时最好用小写的属性名，
    而不要直接调用 OtherProperty_set 方法， 否则容易进入死循环

(2). 可以看到step 3 里，添加了 ControlBindingsCollection 的扩展方法，
    这样就不用担心"PropertyName" 之类容易出错的看起来很恶心的写法，
    事实上 IDE 的提示功能使 lambda 表达式写起来非常方便，
    并且在 Build 的时候就可以查出属性名是否对应，提高写代码的效率，减少出错的机会
     */
    /// <summary>
    /// 实现双向绑定
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        [SugarColumn(IsIgnore = true)]
        public bool SuppressNotifyPropertyChanged { get; set; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null && !SuppressNotifyPropertyChanged)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expr)
        {
            this.OnPropertyChanged(Utils.GetMemberName(expr));
        }

        /// <summary>
        /// 如果没有其他的业务逻辑，对 lambda 表达式比较熟悉的同学可以考虑用以下方法实现属性名称传递       
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

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return;
            storage = value;
            this.OnPropertyChanged(propertyName);
        
        }



    }

}
