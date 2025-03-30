using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Models
{
    public class DataField
    {
        public string Id { get; }
        public string DisplayName { get; }
        public FieldType Type { get; }

        public DataField(string id, string displayName, FieldType type)
        {
            Id = id;
            DisplayName = displayName;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            return obj is DataField other &&
                   Id == other.Id &&
                   DisplayName == other.DisplayName &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, DisplayName, Type).GetHashCode();
        }

        public static bool operator ==(DataField left, DataField right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DataField left, DataField right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"DataField {{ Id = {Id}, DisplayName = {DisplayName}, Type = {Type} }}";
        }
    }

    public enum FieldType
    {
        // 这里可根据实际情况添加枚举值
        Type1,
        Type2
    }

}
