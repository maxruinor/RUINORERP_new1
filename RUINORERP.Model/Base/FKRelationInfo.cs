using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base
{

    // 使用专用类存储变更信息
    /// <summary>
    /// 外键关系信息类
    /// 用于存储实体属性与外键表的关系信息
    /// </summary>
    public class FKRelationInfo
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 外键表名
        /// </summary>
        public string FKTableName { get; set; }

        /// <summary>
        /// 外键ID列名
        /// </summary>
        public string FK_IDColName { get; set; }

        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool CmbMultiChoice { get; set; }
    }
}
