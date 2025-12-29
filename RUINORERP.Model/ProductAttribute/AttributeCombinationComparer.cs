using System.Collections.Generic;

namespace RUINORERP.Model.ProductAttribute
{
    /// <summary>
    /// 属性组合比较器，用于比较两个属性组合是否相同
    /// </summary>
    public class AttributeCombinationComparer : IEqualityComparer<AttributeCombination>
    {
        /// <summary>
        /// 比较两个属性组合是否相同
        /// </summary>
        /// <param name="x">第一个属性组合</param>
        /// <param name="y">第二个属性组合</param>
        /// <returns>如果两个属性组合相同返回true，否则返回false</returns>
        public bool Equals(AttributeCombination x, AttributeCombination y)
        {
            if (x == null || y == null)
                return false;

            if (x.Properties.Count != y.Properties.Count)
                return false;

            // 检查两个组合是否包含相同的属性值对
            foreach (var xProp in x.Properties)
            {
                bool found = false;
                foreach (var yProp in y.Properties)
                {
                    if (xProp.Property.Property_ID == yProp.Property.Property_ID &&
                        xProp.PropertyValue.PropertyValueID == yProp.PropertyValue.PropertyValueID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 生成属性组合的哈希码
        /// </summary>
        /// <param name="obj">属性组合对象</param>
        /// <returns>属性组合的哈希码</returns>
        public int GetHashCode(AttributeCombination obj)
        {
            if (obj == null || obj.Properties.Count == 0)
                return 0;

            // 生成哈希码，基于属性ID和属性值ID的组合
            int hashCode = 17;
            foreach (var prop in obj.Properties)
            {
                hashCode = hashCode * 23 + prop.Property.Property_ID.GetHashCode();
                hashCode = hashCode * 23 + prop.PropertyValue.PropertyValueID.GetHashCode();
            }
            return hashCode;
        }
    }
}