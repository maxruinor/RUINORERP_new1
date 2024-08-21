using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.AdvancedUIModule
{
    public enum fieldtype
    {

    }





    public class SomeAttribute : Attribute
    {
        public SomeAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }


    // for attribute to be injected the property should be "virtual"
    public class ClassA
    {
        public virtual int Value { get; set; }
    }

    class DynamicInputParams : DynamicObject
    {
        Dictionary<string, object> property = new Dictionary<string, object>();
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            return property.TryGetValue(name, out result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            property[binder.Name] = value; return true;
        }

        //static void Main()
        //{
        //    dynamic P = new DynamicInputParams();
        //    P.Name = "张三";
        //    P.Age = 22;
        //    P.Sex = "女"; 
        //    Console.WriteLine(P.Name);
        //    //也可以添加到List集合
        //    List<dynamic> List = new List<dynamic>();
        //    List.Add(P);
        //    foreach (var item in List)
        //    { Console.WriteLine(item.Name); }
        //}  //输出结果:张三 原理是利用了Dynamic的特性， Dynamic介绍请点击：C#中dynamic的正确用法

    }
}
