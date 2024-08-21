using System;
using System.Collections;
using System.Windows.Forms;

namespace HLH.Lib.Helper
{
    /// <summary>
    /// 枚举操作
    /// </summary>
    public class EnumHelper
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
