/*
 * wiki: http://code.google.com/p/zsharedcode/wiki/StringConvert
 * 如果您无法运行此文件, 可能由于缺少相关类文件, 请下载解决方案后重试, 具体请参考: http://code.google.com/p/zsharedcode/wiki/HowToDownloadAndUse
 * 原始代码: http://zsharedcode.googlecode.com/svn/trunk/zsharedcode/panzer/.class/code/StringConvert.cs
 * 版本: .net 4.0, 其他版本可能有所不同
 * 
 * 使用许可: 此文件是开源共享免费的, 但您仍然需要遵守, 下载并将 panzer 许可证 http://zsharedcode.googlecode.com/svn/trunk/zsharedcode/panzer/panzer.license.txt 包含在你的产品中.
 * */

// HACK: 如果代码不能编译, 请尝试在项目中定义编译符号 V4, V3_5, V3, V2 以表示不同的 .NET 版本

using System;
using System.Drawing;

namespace HLH.Lib.Xml
{

    /// <summary>
    /// 字符串转换器.
    /// </summary>
    public sealed class StringConvert
    {

        /// <summary>
        /// 将对象转化为字符串.
        /// </summary>
        /// <param name="value">需要转化的对象.</param>
        /// <returns>转化后的字符串.</returns>
        public static string ToString(object value)
        {

            if (null == value)
                return string.Empty;

            if (value.GetType() == typeof(Color))
                return ((Color)value).ToArgb().ToString();
            else
                return value.ToString();

        }

        /// <summary>
        /// 将字符串转化为指定类型的对象.
        /// </summary>
        /// <param name="value">需要转化的字符串.</param>
        /// <returns>转化后的对象.</returns>
        public static T ToObject<T>(string value)
        {

            if (null == value)
                return default(T);

            Type type = typeof(T);

            try
            {

                // HACK: 可能需要添加 V5
#if V4
				if ( type == typeof ( Guid ) )
					return ( T ) ( object ) new Guid ( value );
				else if ( type == typeof ( Color ) )
					return ( T ) ( object ) Color.FromArgb ( Convert.ToInt32 ( value ) );
				else if ( type == typeof ( string ) )
					return ( T ) ( object ) value.ToString ( );
				else if ( type == typeof ( int ) )
					return ( T ) ( object ) int.Parse ( value );
				else if ( type == typeof ( short ) )
					return ( T ) ( object ) short.Parse ( value );
				else if ( type == typeof ( long ) )
					return ( T ) ( object ) long.Parse ( value );
				else if ( type == typeof ( decimal ) )
					return ( T ) ( object ) decimal.Parse ( value );
				else if ( type == typeof ( bool ) )
					return ( T ) ( object ) bool.Parse ( value );
				else if ( type == typeof ( DateTime ) )
					return ( T ) ( object ) DateTime.Parse ( value );
				else if ( type == typeof ( float ) )
					return ( T ) ( object ) float.Parse ( value );
				else if ( type == typeof ( double ) )
					return ( T ) ( object ) double.Parse ( value );
				else
					return ( T ) ( object ) value;
#else
                if (object.ReferenceEquals(type, typeof(Guid)))
                    return (T)(object)new Guid(value);
                else if (object.ReferenceEquals(type, typeof(Color)))
                    return (T)(object)Color.FromArgb(Convert.ToInt32(value));
                else if (object.ReferenceEquals(type, typeof(string)))
                    return (T)(object)value.ToString();
                else if (object.ReferenceEquals(type, typeof(int)))
                    return (T)(object)int.Parse(value);
                else if (object.ReferenceEquals(type, typeof(short)))
                    return (T)(object)short.Parse(value);
                else if (object.ReferenceEquals(type, typeof(long)))
                    return (T)(object)long.Parse(value);
                else if (object.ReferenceEquals(type, typeof(decimal)))
                    return (T)(object)decimal.Parse(value);
                else if (object.ReferenceEquals(type, typeof(bool)))
                    return (T)(object)bool.Parse(value);
                else if (object.ReferenceEquals(type, typeof(DateTime)))
                    return (T)(object)DateTime.Parse(value);
                else if (object.ReferenceEquals(type, typeof(float)))
                    return (T)(object)float.Parse(value);
                else if (object.ReferenceEquals(type, typeof(double)))
                    return (T)(object)double.Parse(value);
                else
                    return (T)(object)value;
#endif

            }
            catch
            { return default(T); }

        }

    }

}