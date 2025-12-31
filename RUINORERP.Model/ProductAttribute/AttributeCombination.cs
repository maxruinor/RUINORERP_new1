using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.Model.ProductAttribute
{
    /// <summary>
    /// 属性组合类，包含一组属性值对和对应的产品详情
    /// </summary>
    public class AttributeCombination
    {
        /// <summary>
        /// 属性值对列表
        /// </summary>
        public List<AttributeValuePair> Properties { get; set; }

        /// <summary>
        /// 对应的产品详情
        /// </summary>
        public tb_ProdDetail ProductDetail { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AttributeCombination()
        {
            Properties = new List<AttributeValuePair>();
        }

        /// <summary>
        /// 将属性组合转换为字符串表示，格式为：属性名称1:属性值名称1,属性名称2:属性值名称2,...
        /// </summary>
        /// <returns>属性组合的字符串表示</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            foreach (var attributeValuePair in Properties)
            {
                if (attributeValuePair.Property != null && attributeValuePair.PropertyValue != null)
                {
                    // 格式化为：属性名称:属性值名称
                    stringBuilder.Append($"{attributeValuePair.Property.PropertyName}:{attributeValuePair.PropertyValue.PropertyValueName},");
                }
            }
            
            // 移除末尾的逗号
            string result = stringBuilder.ToString().TrimEnd(',');
            
            // 如果没有属性，返回默认值
            return string.IsNullOrEmpty(result) ? "无属性" : result;
        }
    }
}