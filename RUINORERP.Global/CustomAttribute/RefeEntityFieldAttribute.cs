using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Global.CustomAttribute
{
    /// <summary>
    /// ReferenceEntityField.cs
    /// 实体中字段引用于另一个实体产品单位来自单位表 key/value
    ///  理论上两ID可以不相等，目前框架上处理了相等
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class RefeEntityFieldAttribute : Attribute
    {
        private string _fieldNameForPKID = string.Empty;
        private string _fieldNameForID = string.Empty;
        private string _fieldNameForName = string.Empty;
        public RefeEntityFieldAttribute(string fieldNameForPKID = null, string fieldNameForID = null, string fieldNameForName = null)
        {
            _fieldNameForPKID = fieldNameForPKID;
            _fieldNameForID = fieldNameForID;
            _fieldNameForName = fieldNameForName;
        }

        public string FieldNameForPKID { get => _fieldNameForPKID; set => _fieldNameForPKID = value; }
        public string FieldNameForID { get => _fieldNameForID; set => _fieldNameForID = value; }
        public string FieldNameForName { get => _fieldNameForName; set => _fieldNameForName = value; }

    }
}
