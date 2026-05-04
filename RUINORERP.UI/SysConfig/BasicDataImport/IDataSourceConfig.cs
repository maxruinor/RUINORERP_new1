using RUINORERP.Model.ImportEngine.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据源配置接口
    /// 所有数据来源类型配置的统一接口
    /// 符合Windows设计器标准的配置模式
    /// </summary>
    public interface IDataSourceConfig : INotifyPropertyChanged
    {
        /// <summary>
        /// 数据来源类型
        /// </summary>
        DataSourceType DataSourceType { get; }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否有效</returns>
        bool Validate(out string errorMessage);

        /// <summary>
        /// 克隆配置对象
        /// </summary>
        /// <returns>克隆后的配置对象</returns>
        IDataSourceConfig Clone();
    }

    /// <summary>
    /// 数据源配置基类
    /// 提供默认实现、通用功能和属性变更通知
    /// </summary>
    [Serializable]
    public abstract class DataSourceConfigBase : IDataSourceConfig
    {
        /// <summary>
        /// 数据来源类型
        /// </summary>
        public abstract DataSourceType DataSourceType { get; }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名称（调用者无需传递，编译器自动填充）</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置属性值并触发变更事件
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名称（调用者无需传递，编译器自动填充）</param>
        /// <returns>如果值发生变化返回 true，否则返回 false</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 验证配置是否有效
        /// 默认实现返回true，子类可重写
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否有效</returns>
        public virtual bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// 克隆配置对象
        /// 使用XML序列化实现深拷贝
        /// </summary>
        /// <returns>克隆后的配置对象</returns>
        public virtual IDataSourceConfig Clone()
        {
            var type = GetType();
            var serializer = new System.Xml.Serialization.XmlSerializer(type);
            using (var ms = new System.IO.MemoryStream())
            {
                serializer.Serialize(ms, this);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return (IDataSourceConfig)serializer.Deserialize(ms);
            }
        }
    }
}