using System;
using System.Text;

namespace RUINORERP.Common.Extensions

{
    /// <summary>
    /// INT扩展
    /// </summary>
    public static partial class ExtObject
    {

        /*
                if (coldata.ColDataType.IsGenericType && coldata.ColDataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                    coldata.ColDataType = coldata.ColDataType.GetGenericArguments()[0];
                }
                if (coldata.ColDataType.IsGenericType && coldata.ColDataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    coldata.ColDataType = Nullable.GetUnderlyingType(coldata.ColDataType);
                    //如果类型是例如此代码可为空，返回int部分(底层类型)。如果只需要将对象转换为特定类型，则可以使用System.Convert.ChangeType方法。
                }
         */

        /// <summary>
        /// 获取基础类型
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public static Type GetBaseType(this Type DataType)
        {
            Type dataType = DataType;
            // We need to check whether the property is NULLABLE
            if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                dataType = dataType.GetGenericArguments()[0];
            }
            return dataType;

        }
    }
}