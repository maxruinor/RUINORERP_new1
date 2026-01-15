using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 帮助上下文类
    /// 描述帮助请求的完整上下文信息,包括帮助级别、目标对象、实体类型等
    /// 帮助管理器根据上下文信息定位和显示相应的帮助内容
    /// </summary>
    public class HelpContext
    {
        #region 公共属性

        /// <summary>
        /// 帮助级别
        /// 决定帮助的粒度和展示方式
        /// </summary>
        public HelpLevel Level { get; set; }

        /// <summary>
        /// 目标控件
        /// 用于定位控件在屏幕上的位置,便于显示帮助提示
        /// </summary>
        public Control TargetControl { get; set; }

        /// <summary>
        /// 窗体类型
        /// 用于生成窗体级别的帮助键
        /// </summary>
        public Type FormType { get; set; }

        /// <summary>
        /// 实体类型
        /// 用于从实体层获取字段描述信息
        /// 通常与数据绑定的实体类型对应
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 字段名称
        /// 用于定位字段级别的帮助
        /// 对应实体的属性名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 控件名称
        /// 用于生成控件级别的帮助键
        /// 通常对应控件的Name属性
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// 模块名称
        /// 用于生成模块级别的帮助键
        /// 通常对应菜单模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 帮助键
        /// 唯一标识帮助内容的键值
        /// 可手动指定,也可根据上下文自动生成
        /// </summary>
        public string HelpKey { get; set; }

        /// <summary>
        /// 额外信息
        /// 用于存储帮助上下文的扩展信息
        /// 例如: 当前状态、操作类型等
        /// </summary>
        public Dictionary<string, object> AdditionalInfo { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// 初始化帮助上下文对象
        /// </summary>
        public HelpContext()
        {
            AdditionalInfo = new Dictionary<string, object>();
        }

        #endregion

        #region 静态工厂方法

        /// <summary>
        /// 从控件创建帮助上下文
        /// 自动识别控件所属的窗体、绑定的实体和字段信息
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <returns>帮助上下文对象</returns>
        public static HelpContext FromControl(Control control)
        {
            if (control == null)
            {
                return null;
            }

            var context = new HelpContext
            {
                Level = HelpLevel.Control,
                TargetControl = control,
                ControlName = control.Name
            };

            // 尝试获取窗体类型
            var form = control.FindForm();
            if (form != null)
            {
                context.FormType = form.GetType();
            }

            // 尝试从DataBindings获取实体和字段信息
            if (control.DataBindings != null && control.DataBindings.Count > 0)
            {
                var binding = control.DataBindings[0];
                context.FieldName = binding.BindingMemberInfo.BindingField;

                try
                {
                    if (binding.BindingManagerBase?.Current != null)
                    {
                        // 从当前绑定的对象获取实体类型
                        var typeName = binding.BindingManagerBase.Current.GetType().Name;
                        // 构造完整的实体类型名称
                        context.EntityType = Type.GetType($"RUINORERP.Model.{typeName}");
                    }
                }
                catch
                {
                    // 忽略异常,继续其他处理
                }
            }

            // 尝试从Tag获取帮助键
            if (control.Tag != null)
            {
                try
                {
                    // 支持多种Tag格式
                    if (control.Tag is string tagString && tagString.StartsWith("HelpKey:"))
                    {
                        context.HelpKey = tagString.Substring("HelpKey:".Length);
                    }
                }
                catch
                {
                    // 忽略异常
                }
            }

            return context;
        }

        /// <summary>
        /// 从窗体创建帮助上下文
        /// 用于生成窗体级别的帮助请求
        /// </summary>
        /// <param name="form">目标窗体</param>
        /// <returns>帮助上下文对象</returns>
        public static HelpContext FromForm(Form form)
        {
            if (form == null)
            {
                return null;
            }

            return new HelpContext
            {
                Level = HelpLevel.Form,
                TargetControl = form,
                FormType = form.GetType(),
                HelpKey = form.GetType().Name
            };
        }

        /// <summary>
        /// 从字段创建帮助上下文
        /// 用于生成字段级别的帮助请求
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>帮助上下文对象</returns>
        public static HelpContext FromField(Type entityType, string fieldName)
        {
            if (entityType == null || string.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            return new HelpContext
            {
                Level = HelpLevel.Field,
                EntityType = entityType,
                FieldName = fieldName,
                HelpKey = $"{entityType.Name}.{fieldName}"
            };
        }

        /// <summary>
        /// 从模块创建帮助上下文
        /// 用于生成模块级别的帮助请求
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>帮助上下文对象</returns>
        public static HelpContext FromModule(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                return null;
            }

            return new HelpContext
            {
                Level = HelpLevel.Module,
                ModuleName = moduleName,
                HelpKey = moduleName
            };
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 设置额外信息
        /// </summary>
        /// <param name="key">信息键</param>
        /// <param name="value">信息值</param>
        public void SetAdditionalInfo(string key, object value)
        {
            if (AdditionalInfo.ContainsKey(key))
            {
                AdditionalInfo[key] = value;
            }
            else
            {
                AdditionalInfo.Add(key, value);
            }
        }

        /// <summary>
        /// 获取额外信息
        /// </summary>
        /// <param name="key">信息键</param>
        /// <returns>信息值,不存在则返回null</returns>
        public object GetAdditionalInfo(string key)
        {
            return AdditionalInfo.TryGetValue(key, out object value) ? value : null;
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 重写ToString方法,返回帮助上下文的字符串表示
        /// </summary>
        /// <returns>帮助上下文字符串</returns>
        public override string ToString()
        {
            return $"[{Level}] {HelpKey ?? "N/A"}";
        }

        #endregion
    }
}
