using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RUINORERP.Model;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.Common;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace RUINORERP.Common.Helper
{
    /*
    public class MyConverter : MarkupExtension, System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Enum)value).GetAttributeOfType<DisplayAttribute>().Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
    */
    /// <summary>
    /// 2023年10-15 解决了枚举绑定同步数据源的问题。思路是动态生成一个类。字段名对应起数据库名 类型相同
    /// </summary>
    public class CmbItem
    {
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public string desc { get; set; }


        public CmbItem(string pkey, string pname)
        {
            key = pkey;
            name = pname;
        }



    }





    /// <summary>
    /// 枚举操作
    /// </summary>
    public static class EnumHelper
    {



        //  public static Object Parse(Type enumType, string value)
        /*
         * 
         * String-->Enum

  (1)利用Enum的静态方法Parse：

     public static Object Parse(Type enumType,string value)

     例如：(Colors)Enum.Parse(typeof(Colors), "Red")

   

  Enum-->Int

  (1)因为枚举的基类型是除 Char 外的整型，所以可以进行强制转换。

     例如：(int)Colors.Red, (byte)Colors.Green

   

  Int-->Enum

  (1)可以强制转换将整型转换成枚举类型。

     例如：Colors color = (Colors)2 ，那么color即为Colors.Blue

  (2)利用Enum的静态方法ToObject。

     public static Object ToObject(Type enumType,int value)

     例如：Colors color = (Colors)Enum.ToObject(typeof(Colors), 2)，那么color即为Colors.Blue

   

  判断某个整型是否定义在枚举中的方法：Enum.IsDefined

  public static bool IsDefined(Type enumType,Object value)

  例如：Enum.IsDefined(typeof(Colors), n))
  ————————————————
  版权声明：本文为CSDN博主「pzhtpf」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
  原文链接：https://blog.csdn.net/pzhtpf/java/article/details/9419191
  
             * **/

        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="status">枚举的值</param>
        /// <returns></returns>
        public static T GetEnumByString<T>(string Text)
        {
            //  public static Object Parse(Type enumType, string value)
            /*
             * 
             * String-->Enum

            (1)利用Enum的静态方法Parse：

               public static Object Parse(Type enumType,string value)

               例如：(Colors)Enum.Parse(typeof(Colors), "Red")
            */
            return (T)Enum.Parse(typeof(T), Text);

        }


        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="status">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumName<T>(this int status)
        {
            return Enum.GetName(typeof(T), status);
        }
        /// <summary>
        /// 获取枚举名称集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetNamesArr<T>()
        {
            return Enum.GetNames(typeof(T));
        }
        /// <summary>
        /// 将枚举转换成字典集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> getEnumDic<T>()
        {
            Dictionary<string, int> resultList = new Dictionary<string, int>();
            Type type = typeof(T);
            var strList = GetNamesArr<T>().ToList();
            foreach (string key in strList)
            {
                string val = Enum.Format(type, Enum.Parse(type, key), "d");
                resultList.Add(key, int.Parse(val));
            }
            return resultList;
        }
        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetDic<TEnum>()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                dic.Add(item.ToString(), (int)item);
            }
            return dic;
        }

        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetDicKeyValue<TEnum>()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                dic.Add((int)item, item.ToString());
            }
            return dic;
        }

        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetDicKeyValue(Type enumType)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            // 确保传入的类型是枚举
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("提供的类型必须是枚举类型。", nameof(enumType));
            }
            Array enumValues = Enum.GetValues(enumType);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            while (e.MoveNext())
            {
                //object eobj = Activator.CreateInstance(newType);
                //currentValue = (int)e.Current;
                //currentName = e.Current.ToString();
                //eobj.SetPropertyValue(keyName, currentValue);
                //eobj.SetPropertyValue("Name", currentName);
                //list.Add(eobj);
            }


            var arr = Enum.GetValues(enumType);
            foreach (var item in arr)
            {
                dic.Add((int)item, item.ToString());
            }
            return dic;
        }


        public static string GetEnumbyName(Type type, Enum enumValue)
        {
            string str = string.Empty;
            str = Enum.GetName(type, enumValue);
            return str;
        }


        /// <summary>
        /// 提取枚举值属性的对应描述 [System.ComponentModel.Description("描述意思")]
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }


        /// <summary>
        /// 没有验证！
        /// Gets an attribute on an enum field value
        /// [Display(Name = "新名称")]
        ///  New,
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static string GetEnumName(Type type, Enum enumValue)
        {
            string str = string.Empty;
            str = Enum.GetName(type, enumValue);
            return str;
        }


        /// <summary>
        /// 根据枚举类型，为下拉框绑定数据
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="enumTypeName"></param>
        public static void InitDropListForListBox(ListBox listbox, Type enumType)
        {
            listbox.Items.Clear();
            Array enumValues = Enum.GetValues(enumType);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;

            CmbItem item;


            while (e.MoveNext())
            {
                currentValue = (int)e.Current;
                currentName = e.Current.ToString();
                item = new CmbItem(currentValue.ToString(), currentName);
                object obj = enumType.Assembly.CreateInstance(enumType.FullName);
                Enum temp = Enum.Parse(enumType, currentName) as Enum;
                string desc = EnumHelper.GetEnumDescription(temp);
                item.desc = desc;
                listbox.Items.Add(item);
            }
            listbox.SelectedIndexChanged += listbox_SelectedIndexChanged;



        }

        static void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listbox = sender as ListBox;
            ToolTip tt = new ToolTip();
            string desc = (listbox.SelectedItem as CmbItem).desc;
            if (!string.IsNullOrEmpty(desc))
            {
                tt.SetToolTip(listbox, desc);
            }
        }


    }
}
