using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using SqlSugar;
namespace RUINORERP.Business.CommService
{

    public static class SugarAttributeHelper
    {
        /// <summary>
        /// 检查类型是否具有 SugarTable 特性
        /// </summary>
        public static bool HasSugarTableAttribute(Type type)
        {
            return type.GetCustomAttributes(typeof(SugarTable), inherit: false).Any();
        }

        /// <summary>
        /// 获取类型的 Description 特性值
        /// </summary>
        public static string GetTypeDescription(Type type)
        {
            var descriptionAttr = type.GetCustomAttribute<DescriptionAttribute>(inherit: false);
            return descriptionAttr?.Description ?? string.Empty;
        }

        /// <summary>
        /// 获取 SugarTable 特性的表名
        /// </summary>
        public static string GetSugarTableName(Type type)
        {
            var sugarTableAttr = type.GetCustomAttribute<SugarTable>(inherit: false);
            return sugarTableAttr?.TableName ?? type.Name;
        }

        /// <summary>
        /// 获取类型的完整特性信息
        /// </summary>
        public static (bool HasSugarTable, string Description, string TableName) GetTypeAttributes(Type type)
        {
            var hasSugarTable = HasSugarTableAttribute(type);
            var description = GetTypeDescription(type);
            var tableName = hasSugarTable ? GetSugarTableName(type) : null;

            return (hasSugarTable, description, tableName);
        }
    }
}
