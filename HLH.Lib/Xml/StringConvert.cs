/*
 * wiki: http://code.google.com/p/zsharedcode/wiki/StringConvert
 * ������޷����д��ļ�, ��������ȱ��������ļ�, �����ؽ������������, ������ο�: http://code.google.com/p/zsharedcode/wiki/HowToDownloadAndUse
 * ԭʼ����: http://zsharedcode.googlecode.com/svn/trunk/zsharedcode/panzer/.class/code/StringConvert.cs
 * �汾: .net 4.0, �����汾����������ͬ
 * 
 * ʹ�����: ���ļ��ǿ�Դ������ѵ�, ������Ȼ��Ҫ����, ���ز��� panzer ���֤ http://zsharedcode.googlecode.com/svn/trunk/zsharedcode/panzer/panzer.license.txt ��������Ĳ�Ʒ��.
 * */

// HACK: ������벻�ܱ���, �볢������Ŀ�ж��������� V4, V3_5, V3, V2 �Ա�ʾ��ͬ�� .NET �汾

using System;
using System.Drawing;

namespace HLH.Lib.Xml
{

    /// <summary>
    /// �ַ���ת����.
    /// </summary>
    public sealed class StringConvert
    {

        /// <summary>
        /// ������ת��Ϊ�ַ���.
        /// </summary>
        /// <param name="value">��Ҫת���Ķ���.</param>
        /// <returns>ת������ַ���.</returns>
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
        /// ���ַ���ת��Ϊָ�����͵Ķ���.
        /// </summary>
        /// <param name="value">��Ҫת�����ַ���.</param>
        /// <returns>ת����Ķ���.</returns>
        public static T ToObject<T>(string value)
        {

            if (null == value)
                return default(T);

            Type type = typeof(T);

            try
            {

                // HACK: ������Ҫ��� V5
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