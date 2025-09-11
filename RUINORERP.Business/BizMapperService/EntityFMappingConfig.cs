using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizMapperService
{

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

 

}
