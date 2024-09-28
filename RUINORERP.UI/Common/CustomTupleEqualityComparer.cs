
using FastReport.DevComponents.DotNetBar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public class CustomTupleEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly string[] _ignoredProperties;

        public CustomTupleEqualityComparer(string[] ignoredProperties)
        {
            _ignoredProperties = ignoredProperties;
        }

        public bool Equals(T x, T y)
        {
            return CompareTuples(x, y);
        }

        private bool CompareTuples(object x, object y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            if (x.GetType() != y.GetType()) return false;
            var xType = x.GetType();
            if (x is Tuple<object[]> tx && y is Tuple<object[]> ty)
            {
                var fields1 = tx.Item1;
                var fields2 = ty.Item1;
                for (int i = 0; i < fields1.Length; i++)
                {
                    var value1 = fields1[i];
                    var value2 = fields2[i];
                    if (!Equals(value1, value2))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int GetHashCode(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var type = obj.GetType();
            int hash = 17;

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                if (value is Tuple)
                {
                    hash = hash * 23 + GetHashCode((T)value);
                }
                else if (value != null)
                {
                    hash = hash * 23 + value.GetHashCode();
                }
            }

            return hash;
        }

        private bool IsSugarColumnProperty(PropertyInfo property)
        {
            var sugarColumnAttribute = property.GetCustomAttributes(true)
                                             .OfType<SugarColumn>()
                                             .FirstOrDefault();
            return sugarColumnAttribute != null && !sugarColumnAttribute.IsIgnore;
        }
    }
}
