namespace RUINORERP.Model.ProductAttribute
{
    /// <summary>
    /// 属性值对类，包含属性和属性值
    /// </summary>
    public class AttributeValuePair
    {
        /// <summary>
        /// 属性对象
        /// </summary>
        public tb_ProdProperty Property { get; set; }
        
        /// <summary>
        /// 属性值对象
        /// </summary>
        public tb_ProdPropertyValue PropertyValue { get; set; }
        
        /// <summary>
        /// 将属性值对转换为字符串表示，格式为：属性名称:属性值名称
        /// </summary>
        /// <returns>属性值对的字符串表示</returns>
        public override string ToString()
        {
            if (Property != null && PropertyValue != null)
            {
                return $"{Property.PropertyName}:{PropertyValue.PropertyValueName}";
            }
            
            return "无效属性值对";
        }
    }
}