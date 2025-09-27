using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 字段配置模型
    /// </summary>
    public class EntityFieldConfigBuilder
    {
        public string IdField { get; set; }
        public string NoField { get; set; }
        public string DetailProperty { get; set; }
        public string DescriptionField { get; set; }
        public string DiscriminatorField { get; set; }

        /// <summary>
        /// 创建实体字段配置实例
        /// </summary>
        /// <returns></returns>
        public static EntityFieldConfigBuilder Create()
        {
            return new EntityFieldConfigBuilder();
        }

        /// <summary>
        /// 设置ID字段
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public EntityFieldConfigBuilder WithIdField(string fieldName)
        {
            IdField = fieldName;
            return this;
        }

        /// <summary>
        /// 设置编号字段
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public EntityFieldConfigBuilder WithNoField(string fieldName)
        {
            NoField = fieldName;
            return this;
        }

        /// <summary>
        /// 设置描述字段
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public EntityFieldConfigBuilder WithDescriptionField(string fieldName)
        {
            DescriptionField = fieldName;
            return this;
        }

        /// <summary>
        /// 设置明细属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public EntityFieldConfigBuilder WithDetailProperty(string propertyName)
        {
            DetailProperty = propertyName;
            return this;
        }

        /// <summary>
        /// 设置鉴别器字段
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public EntityFieldConfigBuilder WithDiscriminatorField(string fieldName)
        {
            DiscriminatorField = fieldName;
            return this;
        }
    }
    
   
}
