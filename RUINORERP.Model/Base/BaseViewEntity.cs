using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base
{
    public class BaseViewEntity : BaseEntity, ICloneable
    {
        public BaseViewEntity()
        {
             
        }


        // 将 RelatedTableTypes 改为静态属性
        public static List<Type> RelatedTableTypes { get; private set; } = new List<Type>();


        [SugarColumn(IsIgnore = true)]
        [Browsable(true)]
        public List<Type> InstanceRelatedTableTypes => RelatedTableTypes;
        /// <summary>
        /// 视图要体现关联的实表类型要实现这个方法
        /// 使用时才加载
        /// </summary>
        public virtual void InitRelatedTableTypes()
        {

        }

        public virtual void SetRelatedTableTypes(Type type)
        {
            if (!RelatedTableTypes.Exists(c => c.Name == type.Name))
            {
                RelatedTableTypes.Add(type);
            }
            
        }
        public virtual void SetRelatedTableTypes<T>() where T : class
        {
            if (!RelatedTableTypes.Exists(c => c.Name == typeof(T).Name))
            {
                RelatedTableTypes.Add(typeof(T));
            }
        }

    }
}
