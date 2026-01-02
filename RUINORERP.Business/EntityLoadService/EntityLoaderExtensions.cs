using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.EntityLoadService
{
    // File: EntityLoaderExtensions.cs
    public static class EntityLoaderExtensions
    {
        /// <summary>
        /// 加载实体并转换为指定类型
        /// </summary>
        public static T LoadAs<T>(this EntityLoader loader, string tableName, object billNo)
            where T : class
        {
            dynamic entity = loader.LoadEntity(tableName, billNo);
            return entity as T;
        }

        /// <summary>
        /// 安全转换动态对象
        /// </summary>
        public static T As<T>(this object dynamicObject) where T : class
        {
            return dynamicObject as T;
        }
    }
}
