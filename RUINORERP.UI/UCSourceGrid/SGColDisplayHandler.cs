﻿using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Common.Extensions;
using System.Text.Json.Serialization;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// SourceGrid 列显示控制器
    /// </summary>
    [Serializable]
    public class SGColDisplayHandler : INotifyPropertyChanged, IEquatable<SGColDisplayHandler>
    {
        // 添加唯一键属性（组合键）
        public string CompositeKey => $"{BelongingObjectName}_{ColName}";

        /// <summary>
        /// 根据大思路 表格数据源是来自公共产品部分和单据明细部分。这里保存了分别所属类型
        /// 这个类来于自定义列。有些表格显示时  有产品公共部分和明细 要用这个来区别
        /// </summary>
        public string BelongingObjectName { get; set; }

        /// <summary>
        /// 唯一标识符
        /// 对应实际grid表格控件的列定义的唯一标识符也对应他的上级SGDefineColumnItem的唯一标识符
        /// 用于唯一标识一个列的定义
        /// </summary>
        public string UniqueId { get; set; }
        private int colDisplayIndex = 0;
        public string ColCaption
        {
            get; set;
        }

        private bool isFixed = false;
        private int _ColWidth = 50;
        private string colEncryptedName = string.Empty;

        private bool _isFixed;

        /// <summary>
        /// 是否固定
        /// </summary>
        public bool IsFixed
        {

            set
            {
                //  isFixed = value;
                SetProperty(ref isFixed, value);
            }
            get
            {
                return _isFixed;
            }
        }


        /// <summary>
        /// 宽度
        /// </summary>
        public int ColWidth
        {
            get { return _ColWidth; }
            set
            {
                SetProperty(ref _ColWidth, value);
            }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ColDisplayIndex
        {
            get { return colDisplayIndex; }
            set
            {
                SetProperty(ref colDisplayIndex, value);
            }
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColName { get; set; }

        public string DataPropertyName { get; set; }

        private bool _Visible;

        /// <summary>
        /// 显示
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                SetProperty(ref _Visible, value);
            }
        }
        /// <summary>
        /// 如果为真，则不参与控制，并且不显示
        /// 不可用
        /// </summary>
        public bool Disable { get; set; } = false;

        #region NotifyProperty

        public event PropertyChangedEventHandler PropertyChanged;



        [Browsable(false)]
        [JsonIgnore]
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
            this.OnPropertyChanged(expr.GetMemberInfo().Name);
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
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return;
            storage = value;
            this.OnPropertyChanged(propertyName);
        }

        // 实现 IEquatable<T>
        public bool Equals(SGColDisplayHandler other)
        {
            if (other == null)
                return false;

            // 比较对象的属性值，根据业务需求选择合适的属性
            return ColName == other.ColName && ColCaption == other.ColCaption;
        }

        // 重写 Equals 和 GetHashCode，确保两者保持一致
        public override bool Equals(object obj)
        {
            return Equals(obj as SGColDisplayHandler);
        }
        public override int GetHashCode()
        {
            // 假设类中有两个重要字段：Field1 和 Field2
            int hash = 17;
            hash = hash * 23 + ColName.GetHashCode();
            hash = hash * 23 + ColCaption.GetHashCode();
            return hash;
        }

        //public override int GetHashCode()
        //{
        //    // 使用一个工具类来生成哈希值
        //    return HashCode.Combine(GridKeyName, ColName);
        //}

        #endregion

    }
}
