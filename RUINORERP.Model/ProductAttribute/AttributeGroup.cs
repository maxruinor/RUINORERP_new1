using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Model.ProductAttribute
{
    /// <summary>
    /// 属性组类，包含属性信息和选中的属性值
    /// </summary>
    public class AttributeGroup
    {
        /// <summary>
        /// 属性对象
        /// </summary>
        public tb_ProdProperty Property { get; set; }
        
        /// <summary>
        /// 选中的属性值列表
        /// </summary>
        public List<tb_ProdPropertyValue> SelectedValues { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AttributeGroup()
        {
            SelectedValues = new List<tb_ProdPropertyValue>();
        }
        
        /// <summary>
        /// 转换为AttributeValuePair列表，兼容AttributeCombination的使用场景
        /// </summary>
        /// <returns>AttributeValuePair列表</returns>
        public List<AttributeValuePair> ToAttributeValuePairList()
        {
            return SelectedValues.Select(v => new AttributeValuePair
            {
                Property = Property,
                PropertyValue = v
            }).ToList();
        }
        
        /// <summary>
        /// 将属性组转换为字符串表示，格式为：属性名称: 属性值1, 属性值2, ...
        /// </summary>
        /// <returns>属性组的字符串表示</returns>
        public override string ToString()
        {
            if (Property != null)
            {
                string propertyName = Property.PropertyName;
                
                if (SelectedValues != null && SelectedValues.Any())
                {
                    string values = string.Join(", ", SelectedValues.Select(v => v.PropertyValueName));
                    return $"{propertyName}: {values}";
                }
                else
                {
                    return $"{propertyName}: 未选择属性值";
                }
            }
            
            return "无效属性组";
        }
    }
}