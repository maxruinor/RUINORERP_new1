using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINOR.Framework.Core.Common.Extention
{
    /// <summary>
    /// 为参数验证提供有用的方法
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// 如果argumentToValidate为空，则抛出一个ArgumentNullException异常
        /// </summary>
        public static void ThrowIfNull(object argumentToValidate, string argumentName)
        {
            if (null == argumentName)
            {
                throw new ArgumentNullException("argumentName");
            }
            if (null == argumentToValidate)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
        /// <summary>
        /// 如果argumentToValidate为空，则抛出一个ArgumentException异常
        /// </summary>
        public static void ThrowIfNullOrEmpty(string argumentToValidate, string argumentName)
        {
            ThrowIfNull(argumentToValidate, argumentName);
            if (argumentToValidate == string.Empty)
            {
                throw new ArgumentException(argumentName);
            }
        }
        /// <summary>
        /// 如果condition为真，则抛出ArgumentException异常
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        public static void ThrowIfTrue(bool condition, string msg)
        {
            ThrowIfNullOrEmpty(msg, "msg");
            if (condition)
            {
                throw new ArgumentException(msg);
            }
        }
        /// <summary>
        /// 如果指定目录存在该文件则抛出FileNotFoundException异常
        /// </summary>
        /// <param name="fileSytemObject"></param>
        /// <param name="argumentName"></param>
        public static void ThrowIfDoesNotExist(FileSystemInfo fileSytemObject, String argumentName)
        {
            ThrowIfNull(fileSytemObject, "fileSytemObject");
            ThrowIfNullOrEmpty(argumentName, "argumentName");
            if (!fileSytemObject.Exists)
            {
                throw new FileNotFoundException("'{0}' not found".Fi(fileSytemObject.FullName));
            }
        }
        public static string Fi(this string format, params object[] args)
        {
            return FormatInvariant(format, args);
        }
        /// <summary>
        /// 格式化字符串和使用<see cref="CultureInfo.InvariantCulture">不变的文化</see>.
        /// </summary>
        /// <remarks>
        /// <para>这应该是用于显示给用户的任何字符串时使用的“B”>“B”>“”。它意味着日志
        ///消息，异常消息，和其他类型的信息，不使其进入用户界面，或不会
        ///无论如何，对用户都有意义；）.</para>
        /// </remarks>
        public static string FormatInvariant(this string format, params object[] args)
        {
            ThrowIfNull(format, "format");
            return 0 == args.Length ? format : string.Format(CultureInfo.InvariantCulture, format, args);
        }
        /// <summary>
        /// 如果时间不为DateTimeKind.Utc，则抛出ArgumentException异常
        /// </summary>
        /// <param name="argumentToValidate"></param>
        /// <param name="argumentName"></param>
        public static void ThrowIfNotUtc(DateTime argumentToValidate, String argumentName)
        {
            ThrowIfNullOrEmpty(argumentName, "argumentName");
            if (argumentToValidate.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("You must pass an UTC DateTime value", argumentName);
            }
        }
    }
}
