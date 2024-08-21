using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.CustomAttribute
{
    /// <summary>
    /// 标记实体字段中的外键关联性
    /// 主要应用于 可以通过这个找到对应的外键表名，字段ID名 字段名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class FKRelationAttribute : Attribute
    {
        /// <summary>
        /// 标记实体字段中的外键关联性
        /// </summary>
        /// <param name="_FKTableName">外键表名</param>
        /// <param name="_FK_IDColName">外键值的列名</param>
        public FKRelationAttribute(string _FKTableName, string _FK_IDColName)
        {
            FKTableName = _FKTableName;
            FK_IDColName = _FK_IDColName;
        }


        /// <summary>
        /// 标记实体字段中的外键关联性,多选式，改为程序控制
        /// </summary>
        /// <param name="_FKTableName">外键表名</param>
        /// <param name="_FK_IDColName">外键值的列名</param>
        public FKRelationAttribute(string _FKTableName, string _FK_IDColName, bool _CmbMultiChoice)
        {
            FKTableName = _FKTableName;
            FK_IDColName = _FK_IDColName;
            CmbMultiChoice = _CmbMultiChoice;
        }

        /// <summary>
        /// 如果为假，则是下拉式，如果为真。则是下拉checkboxes
        /// </summary>
        public bool CmbMultiChoice { get; set; } = false;

        /// <summary>
        /// FKTableName
        /// </summary>
        public string FKTableName { get; set; }

        /// <summary>
        /// ex: id key
        /// </summary>
        public string FK_IDColName { get; set; }


        /// <summary>
        /// 事务传播方式
        /// </summary>
        // public Propagation Propagation { get; set; } = Propagation.Required;
 


    }


}
