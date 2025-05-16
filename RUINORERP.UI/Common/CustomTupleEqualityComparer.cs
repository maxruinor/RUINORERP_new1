
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
                if (value is Tuple && value.GetType() == typeof(Tuple<int, string>))
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

    /*
// 自定义元组比较器
private class CustomTupleEqualityComparer<T> : IEqualityComparer<T> where T : Tuple<object[]>
{
public bool Equals(T x, T y)
{
   if (x == null && y == null) return true;
   if (x == null || y == null) return false;
   if (x.Item1.Length != y.Item1.Length) return false;

   for (int i = 0; i < x.Item1.Length; i++)
   {
       var valueX = x.Item1[i];
       var valueY = y.Item1[i];

       if (valueX == null && valueY == null) continue;
       if (valueX == null || valueY == null) return false;
       if (!valueX.Equals(valueY)) return false;
   }

   return true;
}

public int GetHashCode(T obj)
{
   if (obj == null || obj.Item1 == null || obj.Item1.Length == 0)
       return 0;

   int hash = 17;
   foreach (var item in obj.Item1)
   {
       hash = hash * 23 + (item?.GetHashCode() ?? 0);
   }
   return hash;
}
}
*/
}
