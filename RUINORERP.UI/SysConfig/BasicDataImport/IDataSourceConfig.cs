using RUINORERP.Model.ImportEngine.Enums;
using System;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据源配置接口
    /// 所有数据来源类型配置的统一接口
    /// 符合Windows设计器标准的配置模式
    /// </summary>
    public interface IDataSourceConfig
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
    /// 提供默认实现和通用功能
    /// </summary>
    [Serializable]
    public abstract class DataSourceConfigBase : IDataSourceConfig
    {
        /// <summary>
        /// 数据来源类型
        /// </summary>
        public abstract DataSourceType DataSourceType { get; }

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