using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace RUINORERP.UI.UControls
{
    /*
    CanConvertFrom（）――根据类型参数进行测试，判断是否能从这个类型转换成当前类型,在本例中我们只提供转换string和InstanceDescriptor类型的能力。
    CanConvertTo（）――根据类型参数进行测试，判断是否能从当前类型转换成指定的类型。
    ConvertTo()――将参数value的值转换为指定的类型。
    ConvertFrom（）――串换参数value，并返回但书类型的一个对象。
    */

    #region Converter 类
    /// <summary>
    /// 三个方法CanConvertFrom，CanConvertTo，ConvertTo，ConvertFrom
    /// </summary>
    [Serializable]
    public class MenuControllerConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String)) return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String)) return true;

            if (destinationType == typeof(InstanceDescriptor)) return true;

            return base.CanConvertTo(context, destinationType);
        }

        /*
        * ConvertTo的实现，如果转换的目标类型是string，
        * 我将Scope的两个属性转换成string类型，并且用一个“，”连接起来，
        * 这就是我们在属性浏览器里看到的表现形式，
        * 
        * 如果转换的目标类型是实例描述器（InstanceDescriptor，
        * 它负责生成实例化的代码），我们需要构造一个实例描述器，
        * 构造实例描述器的时候，我们要利用反射机制获得Scope类的构造器信息，
        * 并在new的时候传入Scope实例的两个属性值。实例描述器会为我们生成这样的代码：
        * this.myListControl1.Scope = new CustomControlSample.Scope(10, 200)；
        * 在最后不要忘记调用 base.ConvertTo(context, culture, value, destinationType)，
        * 你不需要处理的转换类型，交给基类去做好了。
        */
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            String result;
            if (destinationType == typeof(String))
            {
                ContextMenuController controller = (ContextMenuController)value;
                result = controller.MenuText.ToString() + "," + controller.IsShow.ToString() + "," + controller.IsSeparator.ToString() + "," + controller.ClickEventName.ToString();
                return result;

            }

            if (destinationType == typeof(InstanceDescriptor))
            {
                ConstructorInfo ci = typeof(ContextMenuController).GetConstructor(new Type[] { typeof(string), typeof(bool), typeof(bool), typeof(string) });
                ContextMenuController controller = (ContextMenuController)value;
                return new InstanceDescriptor(ci, new object[] { controller.MenuText, controller.IsShow, controller.IsSeparator, controller.ClickEventName });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }


        /*
        * ConvertFrom的代码，由于系统能够直接将实例描述器转换为 目标 类型，
        * 所以我们就没有必要再写代码，我们只需要关注如何将String（在属性浏览出现的属性值的表达）
        * 类型的值转换为 目标 类型。没有很复杂的转换，只是将这个字符串以“，”分拆开，并串换为Int32类型
        * ，然后new一个目标 类的实例，将分拆后转换的两个整型值赋给 目标 的实例，
        * 然后返回实例。在这段代码里，我们要判断一下用户设定的属性值是否有效。
        * 比如，如果用户在Scope属性那里输入了“10200”，
        * 由于没有输入“，”，我们无法将属性的值分拆为两个字符串，
        * 也就无法进行下面的转换，所以，我们要抛出一个异常，通知用户重新输入。
        */
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                String[] v = ((String)value).Split(',');
                if (v.GetLength(0) != 4)
                {
                    throw new ArgumentException("Invalid parameter format 需要有四个数据");
                }


                ContextMenuController controller = new ContextMenuController();
                controller.MenuText = Convert.ToString(v[0]);
                controller.IsShow = Convert.ToBoolean(v[1]);
                controller.IsSeparator = Convert.ToBoolean(v[2]);
                controller.ClickEventName = Convert.ToString(v[3]);
                //throw new ArgumentException("112121t 需要有四个数据");
                return controller;
            }
            return base.ConvertFrom(context, culture, value);
        }
        /*
        * 为了在属性浏览器里能够独立的编辑子属性
        * ，我们还要重写两个方法：GetPropertiesSupported（）和GetProperties（）；
        * 下面是ScopeConverter的完整代码：
        * 在GetProperties方法里，我用TypeDescriptor获得了Scope类的所有的属性描述器并返回。
        * 如果你对TypeDescriptor还不熟悉的话，可以参考MSDN。
        * 重写这两个方法并编译以后，在测试工程里查看控件的属性，你可以看到Scope是如下的形式
        */

        //下面方法实现对属性的子属性可进行编辑
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(ContextMenuController), attributes);
        }
    }
    #endregion





}
