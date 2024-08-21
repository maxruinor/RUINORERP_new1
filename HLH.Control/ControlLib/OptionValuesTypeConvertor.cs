using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HLH.WinControl.ControlLib
{
    public partial class OptionValuesTypeConvertor : StringConverter
    {

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;  //是否有下拉列表
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;  //是否不允许编辑
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context.Instance != null)
            {
                //找到当前操作的属性
                PropertyInfo currentProperty = context.Instance.GetType().GetProperties().FirstOrDefault(info => info.Name == context.PropertyDescriptor.Name);
                if (currentProperty != null)
                {
                    //找到当前操作的属性的OptionCollectionAttribute特性，并根据该特性的值得到可选集合
                    object[] optionCollectionAttributes = currentProperty.GetCustomAttributes(typeof(OptionCollectionAttribute), false);
                    if (optionCollectionAttributes != null)
                    {
                        object myAttribute = optionCollectionAttributes.FirstOrDefault(attribute => attribute is OptionCollectionAttribute);
                        if (myAttribute != null)
                        {
                            return new StandardValuesCollection((myAttribute as OptionCollectionAttribute).OptionCollection);
                        }
                    }
                }
            }
            return base.GetStandardValues(context);
        }
    }
}
