﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Extensions
{
    public static class StringExtension
    {
        public static T Convert<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static object Convert(this string input, Type type)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter != null)
                {
                    return converter.ConvertFromString(input);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
