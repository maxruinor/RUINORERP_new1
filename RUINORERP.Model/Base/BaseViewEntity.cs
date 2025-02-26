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
            InitRelatedTableTypes();
        }
        private List<Type> _RelatedTableTypes = new List<Type>();


        [SugarColumn(IsIgnore = true)]
        [Browsable(true)]
        public List<Type> RelatedTableTypes { get => _RelatedTableTypes; set => _RelatedTableTypes = value; }

        /// <summary>
        /// 视图要体现关联的实表类型要实现这个方法
        /// </summary>
        public virtual void InitRelatedTableTypes()
        {
          
        }

        public virtual void SetRelatedTableTypes(Type type)
        {
            RelatedTableTypes.Add(type);
        }
        public virtual void SetRelatedTableTypes<T>() where T : class
        {
            RelatedTableTypes.Add(typeof(T));
        }

    }
}
