using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizMapperService
{
    public class EntityMappingConfig
    {
        public Type EntityType { get; set; }
        public EntityFieldConfig FieldConfig { get; set; }
    }

    // 字段配置模型
    public class EntityFieldConfig
    {
        public string IdField { get; set; }
        public string NoField { get; set; }
        public string DetailProperty { get; set; }

        //如果还要关联查询其它子表可以多一个属性
        //public DiscriminatorConfig Discriminator { get; set; }

        //// 添加子表配置
        //public DetailTableConfig DetailTable { get; set; }
    }

    /// <summary>
    /// 共用表配置
    /// </summary>
    public class SharedTableConfig
    {
        public string DiscriminatorField { get; set; }
        public EntityFieldConfig entityFieldConfig { get; set; }

        /// <summary>
        /// 通过bool ,int类型来区别 指定到更具体的业务类型
        /// </summary>
        public Func<object, BizType> TypeResolver { get; set; }
    }


 
 

}
