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
    }
}