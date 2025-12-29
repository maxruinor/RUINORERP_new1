using System.Collections.Generic;

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
    }
}