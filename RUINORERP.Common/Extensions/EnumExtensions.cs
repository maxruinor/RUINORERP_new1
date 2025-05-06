using RUINORERP.Model.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace RUINORERP.Common.Extensions
{
    /// <summary>
    /// 枚举的扩展方法
    /// </summary>
    public static class EnumExtensions
    {
        public static List<EnumEntityMember> GetListByEnumtype(this Type enumObj, int? selectedItem = null, params int[] exclude)
        {
            /////排除一些枚举值
            //  Enum.GetValues(typeof(ProductAttributeType)).OfType<ProductAttributeType>().Where(x => (int)x != (int)ProductAttributeType.虚拟);

            if (Enum.GetValues(enumObj).Length > 0)
            {
                List<EnumEntityMember> listResult = new List<EnumEntityMember>();
                foreach (var e in Enum.GetValues(enumObj))
                {
                    if (exclude.Contains(Convert.ToInt32(e)))
                    {
                        //排除
                        continue;
                    }
                    if (selectedItem != null && selectedItem == Convert.ToInt32(e)) // 选中
                    {
                        EnumEntityMember item = new EnumEntityMember
                        {
                            Value = Convert.ToInt32(e).ToString(),    // 传输值
                            Description = e.ToString(),      // 显示值
                            Selected = true
                        };
                        listResult.Add(item);
                    }
                    else
                    {
                        EnumEntityMember item = new EnumEntityMember     // 不选中
                        {
                            Value = Convert.ToInt32(e).ToString(),     // 传输值
                            Description = e.ToString()      // 显示值
                        };
                        listResult.Add(item);
                    }
                }
                return listResult;
                //if (selectedItem != null)
                //    return new EnumerationMember(listResult, "Value", "Text", selectedItem);
                //else
                //    return new EnumerationMember(listResult, "Value", "Text");
            }
            return null;
        }

        public static string GetDescription(this Enum val)
        {
            var field = val.GetType().GetField(val.ToString());
            var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (customAttribute == null) { return val.ToString(); }
            else { return ((DescriptionAttribute)customAttribute).Description; }
        }

        /// <summary>
        /// 获取到对应枚举的描述-没有描述信息，返回枚举值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string EnumDescription(this Enum @enum)
        {
            Type type = @enum.GetType();
            string name = Enum.GetName(type, @enum);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            if (!(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute))
            {
                return name;
            }
            return attribute?.Description;
        }
        public static int ToEnumInt(this Enum e)
        {
            try
            {
                return e.GetHashCode();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static List<EnumDto> EnumToList<T>()
        {
            return setEnumToList(typeof(T));
        }
        public static List<EnumDto> EnumToList<T>(this List<EnumDto> le)
        {
            return setEnumToList(typeof(T));
        }

        public static List<EnumDto> EnumToList(this Type enumType)
        {
            return setEnumToList(enumType);
        }

        private static List<EnumDto> setEnumToList(Type enumType)
        {
            List<EnumDto> list = new List<EnumDto>();
            foreach (var e in Enum.GetValues(enumType))
            {
                EnumDto m = new EnumDto();
                object[] attacheds = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(EnumAttachedAttribute), true);
                if (attacheds != null && attacheds.Length > 0)
                {
                    EnumAttachedAttribute aa = attacheds[0] as EnumAttachedAttribute;
                    //m.Attached = aa;
                    m.TagType = aa.TagType;
                    m.Description = aa.Description;
                    m.Icon = aa.Icon;
                    m.IconColor = aa.IconColor;
                }

                m.Value = Convert.ToInt32(e);
                m.Name = e.ToString();
                list.Add(m);
            }
            return list;
        }
    }


    /// <summary>
    /// 枚举的扩展方法
    /// </summary>


    /// <summary>
    /// 枚举对象
    /// </summary>
    public class EnumDto
    {
        /// <summary>
        /// 附加属性
        /// </summary>
        public EnumAttachedAttribute Attached { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public string TagType { get; set; }
        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 图标颜色
        /// </summary>
        public string IconColor { get; set; }
    }

    public static class EnumBindExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">typeof(enum)</param>
        /// <param name="selectedItem">要选中的项 Index -1 所以为int型</param>
        /// <param name="exclude">要排除的集合int型</param>
        /// <returns></returns>
        public static List<EnumEntityMember> GetListByEnum(this Type type, int? selectedItem = null, params long[] exclude)
        {
            List<EnumEntityMember> listResult = new List<EnumEntityMember>();
            Array enumValues = Enum.GetValues(type);
            // 获取枚举的基础类型
            Type underlyingType = Enum.GetUnderlyingType(type);

            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();

            object currentValueObj;
            string currentName;
            while (e.MoveNext())
            {
                EnumEntityMember item = new EnumEntityMember();
                //currentValue = (int)e.Current;
                //if (exclude.Contains(currentValue))
                //{
                //    //排除
                //    continue;
                //}
                //currentName = e.Current.ToString();



                currentValueObj = e.Current;
                // 根据基础类型转换值

                long currentValueLong;
                if (underlyingType == typeof(int))
                {
                    currentValueLong = ((int)currentValueObj).ToLong();
                    if (exclude.Contains(currentValueLong))
                    {
                        //排除
                        continue;
                    }

                }
                else if (underlyingType == typeof(long))
                {
                    currentValueLong = (long)currentValueObj;
                    if (exclude.Contains(currentValueLong))
                    {
                        //排除
                        continue;
                    }

                }
                else
                {
                    // 其他基础类型的处理，这里简单跳过
                    continue;
                }

                currentName = e.Current.ToString();

                item.Value = currentValueLong;

                item.Description = currentName;
                if (selectedItem.HasValue && currentValueLong == selectedItem.Value)
                {
                    item.Selected = true;
                }

                listResult.Add(item);
            }
            //EnumEntityMember itemDefault = new EnumEntityMember();
            //itemDefault.Value = -1;
            //itemDefault.Description = "请选择";
            //listResult.Insert(0, itemDefault);
            return listResult;
        }


        /// <summary>
        /// ProductAttributeType pat = ProductAttributeType.单属性;
        /// List<int> exclude = new List<int>();
        /// exclude.Add((int) ProductAttributeType.虚拟);
        /// List<EnumEntityMember> list = new List<EnumEntityMember>();
        /// list = pat.GetListByEnum(2, exclude.ToArray());
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObj"></param>
        /// <param name="selectedItem">选中的值</param>
        /// <param name="exclude">要排除的值</param>
        /// <returns></returns>


        // 优化后的方法
        public static List<EnumEntityMember> GetListByEnum<TEnum>(this Type enumType, int? selectedItem = null, params int[] exclude) where TEnum : struct, Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum 必须为枚举类型。");
            }

            var values = Enum.GetValues(enumType);
            if (values.Length == 0)
            {
                return null;
            }

            var listResult = new List<EnumEntityMember>();
            foreach (TEnum e in values)
            {
                int enumValue = Convert.ToInt32(e);
                if (exclude.Contains(enumValue))
                {
                    continue;
                }

                var item = new EnumEntityMember
                {
                    Value = enumValue.ToString(),
                    Description = e.ToString(),
                    Selected = selectedItem.HasValue && selectedItem.Value == enumValue
                };
                listResult.Add(item);
            }

            return listResult;
        }


        public static List<EnumEntityMember> GetListByEnum<TEnum>(params int[] exclude)
        {
            /////排除一些枚举值
            //  Enum.GetValues(typeof(ProductAttributeType)).OfType<ProductAttributeType>().Where(x => (int)x != (int)ProductAttributeType.虚拟);

            int selectedItem = -1;
            if (Enum.GetValues(typeof(TEnum)).Length > 0)
            {
                List<EnumEntityMember> listResult = new List<EnumEntityMember>();
                foreach (TEnum e in Enum.GetValues(typeof(TEnum)))
                {
                    if (exclude.Contains(Convert.ToInt32(e)))
                    {
                        //排除
                        continue;
                    }
                    if (selectedItem != null && selectedItem == Convert.ToInt32(e)) // 选中
                    {
                        EnumEntityMember item = new EnumEntityMember
                        {
                            Value = Convert.ToInt32(e).ToString(),    // 传输值
                            Description = e.ToString(),      // 显示值
                            Selected = true
                        };
                        listResult.Add(item);
                    }
                    else
                    {
                        EnumEntityMember item = new EnumEntityMember     // 不选中
                        {
                            Value = Convert.ToInt32(e).ToString(),     // 传输值
                            Description = e.ToString()      // 显示值
                        };
                        listResult.Add(item);
                    }
                }
                return listResult;
                //if (selectedItem != null)
                //    return new EnumerationMember(listResult, "Value", "Text", selectedItem);
                //else
                //    return new EnumerationMember(listResult, "Value", "Text");
            }
            return null;
        }


    }

}
