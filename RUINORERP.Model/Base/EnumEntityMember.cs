using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base
{
    /// <summary>
    /// 2023年10-15 解决了枚举绑定同步数据源的问题。思路是动态生成一个类。字段名对应起数据库名 类型相同
    /// 2023-10-24  优化，先固定一个类，将枚举绑定到上面，key为object 满足枚举不同的值情况，将他转换为list后可以select where 功能，绑定时
    /// 动态生成一个key列，根据类型生成，再把key值给过去。列名为对应实体名
    /// </summary>
    public class EnumEntityMember
    {
        public string Description { get; set; }

        public object Value { get; set; }

        public bool Selected { get; set; }
    }
}
