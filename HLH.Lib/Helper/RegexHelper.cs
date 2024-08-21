using System;
using System.Collections;
using System.Text.RegularExpressions;


/**********************************************************
* 
* 功能描述：主要是处理一些常规字符正则验证操作
* 创建时间：2009-05-21
* 创建者：陈国利 cgli200@gmail.com
* 
* *********************************************************/
namespace HLH.Lib.Helper
{

    /// <summary>
    /// 正则验证表达式，主要使用静态方法
    /// </summary>
    public class RegexHelper
    {
        /// <summary>
        /// 构造函数，防止外部构造
        /// </summary>
        private RegexHelper()
        {

        }

        private static bool IsRegex(string pattern, string input)
        {
            Regex reg = new Regex(pattern, RegexOptions.Compiled);
            return reg.IsMatch(input);
        }

        /// <summary>
        /// 验证是否是数字
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是数字返回true,否则返回false</returns>
        public static bool IsNumeric(string input)
        {
            return IsRegex(@"[0-9]", input);
        }

        /// <summary>
        /// 验证是否整数（正整数 + 负整数） 
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsInteger(string input)
        {
            return IsRegex(@"^\d+$", input);

        }

        /// <summary>
        /// 验证是否是正整数
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsPositiveInteger(string input)
        {
            return IsRegex(@"^[1-9]\d*$", input);

        }

        /// <summary>
        /// 验证是否是负整数
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsNativeInteger(string input)
        {
            return IsRegex(@"^-[0-9]*[1-9][0-9]*$", input);
        }

        /// <summary>
        /// 验证是否非负整数（包括负整数和0）
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsNoNativeInteger(string input)
        {
            return IsRegex(@"^-?\d+$", input);
        }

        /// <summary>
        /// 验证非正整数（负整数 + 0） 
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsNoPositiveInteger(string input)
        {
            return IsRegex(@"^((-\d+)|(0+))$", input);
        }

        /// <summary>
        /// 验证是否浮点数
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsFloat(string input)
        {
            return IsRegex(@"^(-?\d+)(\.\d+)?$", input);
        }

        /// <summary>
        /// 判断是否为double类型
        /// </summary>
        /// <param name="input">被验证字符串。</param>
        /// <returns></returns>
        public static bool IsDouble(string input)
        {
            return IsRegex(@"^([0-9])[0-9]*(\.\w*)?$", input);
            //if (input != null)
            //{
            //    return Regex.IsMatch(input.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            //}
            //return false;
        }


        /// <summary>
        /// 验证是否为16进制字符串
        /// </summary>
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsHexNumber(string input)
        {
            return IsRegex(@"[^0-9a-fA-F]", input);
        }

        ///// <summary>
        ///// 判断是否16进制编码
        ///// </summary>
        ///// <param name="input">被验证字符串。</param>
        ///// <returns></returns>
        //public static bool IsHexNum(string input)
        //{
        //    input = input.Replace("0X", "").Replace("0x", "");
        //    int n = Int32.Parse(input, System.Globalization.NumberStyles.HexNumber);
        //    return IsNumeric(n.ToString());
        //}

        /// <summary>
        /// 验证字符串是否由字母组成
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsWord(string input)
        {
            return IsRegex(@"^[A-Za-z]+$", input);
        }

        /// <summary>
        /// 验证是否由数字和字母组成的
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsNumbericAndWord(string input)
        {
            return IsRegex(@"^[A-Za-z0-9]+$", input);
        }

        /// <summary>
        /// 验证是否为IP地址
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsIPAddress(string input)
        {
            return IsRegex(@"(?<![\d\.])((25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)\.){3}(25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)(?![\d\.])", input);
        }

        /// <summary>
        /// 验证是否url地址
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsUrl(string input)
        {
            return IsRegex(@"\b(\S+)://(\S+)\b", input);
        }

        /// <summary>
        /// 验证是否EMail地址
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsEMail(string input)
        {
            return IsRegex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", input);
        }

        /// <summary>
        /// 验证是否为指定格式化的日期格式如（ 年-月-日 2009-05-22 ）
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsFormatDate(string input)
        {
            string strTemp = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
            //|(/^((0([1-9]{1}))|(1[1|2]))/(([0-2]([1-9]{1}))|(3[0|1]))/(d{2}|d{4})$/)
            return IsRegex(strTemp, input);
        }

        /// <summary>
        /// 验证是否是电话号码（固定电话如：020-38366688）
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsPhoneNumber(string input)
        {
            // return IsRegex(@"(d+-)?(d{4}-?d{7}|d{3}-?d{8}|^d{7,8})(-d+)?", input);
            return IsRegex(@"(^((\+86)|(86))?-?(\d{3,4}-?)?\d{7,8})$", input);
        }

        /// <summary>
        /// 验证是否中文字符。
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsMobileNumber(string input)
        {
            return IsRegex(@"^((\+86)|(86))?1(3|5|8)\d{9}$", input);
        }

        /// <summary>
        /// 判断是否为Unicode码
        /// </summary>
        /// <param name="input">被验证字符串。</param>
        /// <returns>返回bool类型</returns>
        public static bool IsUnicode(string input)
        {
            return IsRegex(@"^[\u4E00-\u9FA5\uE815-\uFA29]+$", input);
            //string pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            //return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// 验证是否中文字符
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsChinese(string input)
        {
            return IsRegex(@"^([\u4e00-\u9fa5]+|[a-zA-Z0-9]+)$", input);
        }

        /// <summary>
        ///验证匹配双字节字符(包括汉字在内)
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsDoubleByte(string input)
        {
            return IsRegex(@"[^\x00-\xff] ", input);
        }

        /// <summary>
        /// 匹配空行的正则表达式
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsBlankLine(string input)
        {
            return IsRegex(@"\n[\s| ]*\r", input);
        }

        /// <summary>
        /// 验证是否是为HTML标记（还需要改进的地方，如含有空隔或者有属性的时候不行）
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsHTML(string input)
        {
            return IsRegex(@"<(\S*?)[^>]*>.*?</\1>|<.*? />", input);
        }

        /// <summary>
        /// 判断字符串是否存在操作数据库的安全隐患
        /// </summary>
        /// <param name="input">被验证字符串。</param>
        /// <returns>返回bool类型</returns>
        public static bool IsSafety(string input)
        {
            string str = input.Replace("%20", " ");
            str = Regex.Replace(str, @"\s", " ");
            string pattern = @"select |insert |delete from |count\(|drop table|update |truncate |asc\(|mid\(|char\(|xp_cmdshell|exec master|net localgroup administrators|:|net user|""|\'| or ";
            return !IsRegex(pattern, input);//!Regex.IsMatch(str, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证是否为QQ号码
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsQQNumber(string input)
        {
            return IsRegex(@"[1-9][0-9]{4,}", input);
        }

        /// <summary>
        /// 验证是否邮政编码
        /// </summary>
        /// <param name="input">输了待验证字符串</param>
        /// <returns>如果是返回true,否则返回false</returns>
        public static bool IsPostalCode(string input)
        {
            return IsRegex(@"[1-9]\d{5}(?!\d)", input);
        }


        #region 判断是否为相对地址（虚拟地址）
        /// <summary>
        /// 判断是否为相对地址（虚拟地址）
        /// </summary>
        /// <param name="input">被验证字符串。</param>
        /// <returns>返回bool类型</returns>
        public static bool IsRelativePath(string input)
        {
            if (input == null || input == string.Empty)
            {
                return false;
            }
            if (input.StartsWith("/") || input.StartsWith("?"))
            {
                return false;
            }
            if (Regex.IsMatch(input, @"^\s*[a-zA-Z]{1,10}:.*$"))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 判断是否为绝对地址（物理地址）
        /// <summary>
        /// 判断是否为绝对地址（物理地址）
        /// </summary>
        /// <param name="input">被验证字符串。</param>
        /// <returns>返回bool类型</returns>
        public static bool IsPhysicalPath(string input)
        {
            return IsRegex(@"^\s*[a-zA-Z]:.*$", input);
            //string pattern = @"^\s*[a-zA-Z]:.*$";
            //return Regex.IsMatch(input, pattern);
        }
        #endregion

        #region 判断用户IP地址段是否在指定数组当中
        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip">用户输入的IP</param>
        /// <param name="ipArray">指定的IP地址段数组</param>
        /// <returns>返回bool类型</returns>
        public static bool IsInIPArray(string ip, string[] ipArray)
        {
            string[] userip = ip.Split('.');
            for (int j = 0; j < ipArray.Length; j++)
            {
                string[] tmpip = ipArray[j].Split('.');
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }
                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (r == 4)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

    }



    namespace Xrinehart.Framework.Common.Validate
    {
        /**/
        /// <summary>
        /// RegularMatch 的摘要说明。
        /// </summary>    
        public class RegularMatch
        {
            private string _String;
            private bool _IsEntirety;
            /**/
            /// <summary>
            /// 可以进行判断的类型
            /// </summary>
            public enum Operation
            {
                Byte, SByte, Int16, Int32, Int64, Single, Double, Boolean, Char, Decimal, DateTime, Date, Time,
                EMail, URL, ChinaPhone, ChineseWord, ChinesePostalCode, Number, StringModel_01, StringModel_02, WideWord, NarrowWord, IPAddress,
                ChineseMobile, ChineseID
            };
            public RegularMatch() { }




            #region 用于判断字符串是否是对应类型（默认为包含匹配）
            public bool IsAccordType(string strVerifyString, Operation op)
            {
                return IsAccordType(strVerifyString, op, false);
            }
            #endregion



            #region 用于判断字符串是否是对应类型（或是否包含对应类型的字符）
            /**/
            /// <summary>
            /// 用于判断字符串是否是对应类型
            /// </summary>
            /// <param name="strVerifyString">String，需要判断的字符串</param>
            /// <param name="op">Operation枚举，用于选择需要进行的操作</param>
            /// <param name="IsEntirety">Boolean，判断是完全匹配还是包含匹配模式（仅适用于非类型判断时）</param>
            /// <returns></returns>
            public bool IsAccordType(string strVerifyString, Operation op, bool IsEntirety)
            {
                _String = strVerifyString;
                _IsEntirety = IsEntirety;
                switch (op)
                {
                    case Operation.Byte:
                        {
                            return IsByte();
                        }
                    case Operation.SByte:
                        {
                            return IsSByte();
                        }
                    case Operation.Int16:
                        {
                            return IsInt16();
                        }
                    case Operation.Int32:
                        {
                            return IsInt32();
                        }
                    case Operation.Int64:
                        {
                            return IsInt64();
                        }
                    case Operation.Single:
                        {
                            return IsSingle();
                        }
                    case Operation.Double:
                        {
                            return IsDouble();
                        }
                    case Operation.Boolean:
                        {
                            return IsBoolean();
                        }
                    case Operation.Char:
                        {
                            return IsChar();
                        }
                    case Operation.Decimal:
                        {
                            return IsDecimal();
                        }
                    case Operation.DateTime:
                        {
                            return IsDateTime();
                        }
                    case Operation.Date:
                        {
                            return IsDate();
                        }
                    case Operation.Time:
                        {
                            return IsTime();
                        }
                    case Operation.IPAddress:
                        {
                            return IsIPAddress();
                        }
                    case Operation.ChinaPhone:
                        {
                            return IsChinaPhone();
                        }
                    case Operation.ChinesePostalCode:
                        {
                            return IsChinesePostalCode();
                        }
                    case Operation.ChineseMobile:
                        {
                            return IsChineseMobile();
                        }
                    case Operation.EMail:
                        {
                            return IsEmail();
                        }
                    case Operation.URL:
                        {
                            return IsURL();
                        }
                    case Operation.ChineseWord:
                        {
                            return IsChineseWord();
                        }
                    case Operation.Number:
                        {
                            return IsNumber();
                        }
                    case Operation.StringModel_01:
                        {
                            return IsStringModel_01();
                        }
                    case Operation.StringModel_02:
                        {
                            return IsStringModel_02();
                        }
                    case Operation.WideWord:
                        {
                            return IsWideWord();
                        }
                    case Operation.NarrowWord:
                        {
                            return IsNarrowWord();
                        }
                    case Operation.ChineseID:
                        {
                            return IsChineseID();
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
            #endregion



            #region 具体验证方法

            #region 是否Byte类型（8 位的无符号整数）： 0 和 255 之间的无符号整数
            /**/
            /// <summary>
            /// 是否Byte类型（8 位的无符号整数）： 0 和 255 之间的无符号整数
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsByte()
            {
                try
                {
                    Byte.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion


            #region 是否SByte类型（8 位的有符号整数）： -128 到 +127 之间的整数
            /**/
            /// <summary>
            /// 是否SByte类型（8 位的有符号整数）： -128 到 +127 之间的整数
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsSByte()
            {
                try
                {
                    SByte.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion

            #region 是否Int16类型（16 位的有符号整数）： -32768 到 +32767 之间的有符号整数
            /**/
            /// <summary>
            /// 是否Int16类型（16 位的有符号整数）： -32768 到 +32767 之间的有符号整数
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsInt16()
            {
                try
                {
                    Int16.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion

            #region 是否Int32类型（32 位的有符号整数）：-2,147,483,648 到 +2,147,483,647 之间的有符号整数
            /**/
            /// <summary>
            /// 是否Int32类型（32 位的有符号整数）：-2,147,483,648 到 +2,147,483,647 之间的有符号整数
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsInt32()
            {
                try
                {
                    Int32.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion

            #region 是否Int64类型（64 位的有符号整数）： -9,223,372,036,854,775,808 到 +9,223,372,036,854,775,807 之间的整数
            /**/
            /// <summary>
            /// 是否Int64类型（64 位的有符号整数）： -9,223,372,036,854,775,808 到 +9,223,372,036,854,775,807 之间的整数
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsInt64()
            {
                try
                {
                    Int64.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion

            #region 是否Single类型（单精度（32 位）浮点数字）： -3.402823e38 和 +3.402823e38 之间的单精度 32 位数字
            /**/
            /// <summary>
            /// 是否Single类型（单精度（32 位）浮点数字）： -3.402823e38 和 +3.402823e38 之间的单精度 32 位数字
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsSingle()
            {
                try
                {
                    Single.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion


            #region 是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字
            /**/
            /// <summary>
            /// 是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsDouble()
            {
                try
                {
                    Double.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion



            #region 是否Boolean类型（布尔值）：true 或 false
            /**/
            /// <summary>
            /// 是否Double类型（单精度（64 位）浮点数字）： -1.79769313486232e308 和 +1.79769313486232e308 之间的双精度 64 位数字
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsBoolean()
            {
                try
                {
                    Boolean.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion


            #region 是否Char类型（Unicode（16 位）字符）：该 16 位数字的值范围为从十六进制值 0x0000 到 0xFFFF
            /**/
            /// <summary>
            /// 是否Char类型（Unicode（16 位）字符）：该 16 位数字的值范围为从十六进制值 0x0000 到 0xFFFF
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsChar()
            {
                try
                {
                    Char.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion

            #region 是否Char类型（96 位十进制值）：从正 79,228,162,514,264,337,593,543,950,335 到负 79,228,162,514,264,337,593,543,950,335 之间的十进制数
            /**/
            /// <summary>
            /// 是否Char类型（96 位十进制值）：从正 79,228,162,514,264,337,593,543,950,335 到负 79,228,162,514,264,337,593,543,950,335 之间的十进制数
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsDecimal()
            {
                try
                {
                    Decimal.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion


            #region 是否DateTime类型（表示时间上的一刻）： 范围在公元（基督纪元）0001 年 1 月 1 日午夜 12:00:00 到公元 (C.E.) 9999 年 12 月 31 日晚上 11:59:59 之间的日期和时间
            /**/
            /// <summary>
            /// 是否DateTime类型（表示时间上的一刻）： 范围在公元（基督纪元）0001 年 1 月 1 日午夜 12:00:00 到公元 (C.E.) 9999 年 12 月 31 日晚上 11:59:59 之间的日期和时间
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsDateTime()
            {
                try
                {
                    DateTime.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion



            #region 是否Date类型（表示时间的日期部分）： 范围在公元（基督纪元）0001 年 1 月 1 日 到公元 (C.E.) 9999 年 12 月 31 日之间的日期
            /**/
            /// <summary>
            /// 是否Date类型（表示时间的日期部分）： 范围在公元（基督纪元）0001 年 1 月 1 日 到公元 (C.E.) 9999 年 12 月 31 日之间的日期
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsDate()
            {
                DateTime Value;
                try
                {
                    Value = DateTime.Parse(_String);
                }
                catch
                {
                    return false;
                }
                if (Value.Date.ToString() == _String)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #endregion


            #region 是否Time类型（表示时间部分HHMMSS）： 范围在夜 12:00:00 到晚上 11:59:59 之间的时间
            /**/
            /// <summary>
            /// 是否Time类型（表示时间部分HHMMSS）： 范围在夜 12:00:00 到晚上 11:59:59 之间的时间
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsTime()
            {
                DateTime Value;
                try
                {
                    Value = DateTime.Parse(_String);
                }
                catch
                {
                    return false;
                }
                if (Value.Year == 1 && Value.Month == 1 && Value.Day == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #endregion



            #region 是否IPAddress类型（IPv4 的情况下使用以点分隔的四部分表示法格式表示，IPv6 的情况下使用冒号与十六进制格式表示）
            /**/
            /// <summary>
            /// 是否IPAddress类型（IPv4 的情况下使用以点分隔的四部分表示法格式表示，IPv6 的情况下使用冒号与十六进制格式表示）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsIPAddress()
            {
                try
                {
                    System.Net.IPAddress.Parse(_String);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            #endregion

            #region 是否中国电话号码类型（XXX/XXXX-XXXXXXX/XXXXXXXX (\d{3,4})-?\d{7,8}）：判断是否是（区号：3或4位）-（电话号码：7或8位）
            /**/
            /// <summary>
            /// 是否中国电话号码类型（XXX/XXXX-XXXXXXX/XXXXXXXX (\d{3,4})-?\d{7,8}）：判断是否是（区号：3或4位）-（电话号码：7或8位）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsChinaPhone()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"(\d{3,4})-?\d{7,8}", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion


            #region 是否中国邮政编码（6位数字 \d{6}）
            /**/
            /// <summary>
            /// 是否中国邮政编码（6位数字 \d{6}）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsChinesePostalCode()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"\d{6}", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion


            #region 是否中国移动电话号码（13开头的总11位数字 13\d{9}）
            /**/
            /// <summary>
            /// 是否中国移动电话号码（13开头的总11位数字 13\d{9}）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsChineseMobile()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"13\d{9}", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion

            #region 是否EMail类型（XXX@XXX.XXX \w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*）
            /**/
            /// <summary>
            /// 是否EMail类型（XXX@XXX.XXX \w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsEmail()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion


            #region 是否Internet URL地址类型（http://）
            /**/
            /// <summary>
            /// 是否Internet URL地址类型（http://）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsURL()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion

            #region 是否中文字符（[\u4e00-\u9fa5]）
            /**/
            /// <summary>
            /// 是否中文字符（[\u4e00-\u9fa5]）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsChineseWord()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"[\u4e00-\u9fa5]", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion


            #region 是否是数字（0到9的数字[\d]+）：不包括符号"."和"-"
            /**/
            /// <summary>
            /// 是否是数字（0到9的数字[\d]+）：不包括符号"."和"-"
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsNumber()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"[\d]+", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion


            #region 是否只包含数字，英文和下划线（[\w]+）
            /**/
            /// <summary>
            /// 是否只包含数字，英文和下划线（[\w]+）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsStringModel_01()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"[\w]+", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion


            #region 是否大写首字母的英文字母（[A-Z][a-z]+）
            /**/
            /// <summary>
            /// 是否大写首字母的英文字母（[A-Z][a-z]+）
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsStringModel_02()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"[A-Z][a-z]+", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion




            #region 是否全角字符（[^\x00-\xff]）：包括汉字在内
            /**/
            /// <summary>
            /// 是否全角字符（[^\x00-\xff]）：包括汉字在内
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsWideWord()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"[^\x00-\xff]", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion


            #region 是否半角字符（[\x00-\xff]）
            /**/
            /// <summary>
            /// 是否半角字符（[^\x00-\xff]）：包括汉字在内
            /// </summary>
            /// <returns>Boolean</returns>
            protected bool IsNarrowWord()
            {
                ArrayList aryResult = new ArrayList();
                return CommRegularMatch(_String, @"[\x00-\xff]", RegexOptions.None, ref aryResult, _IsEntirety);
            }
            #endregion


            #region 是否合法的中国身份证号码
            protected bool IsChineseID()
            {
                if (_String.Length == 15)
                {
                    _String = CidUpdate(_String);
                }
                if (_String.Length == 18)
                {
                    string strResult = CheckCidInfo(_String);
                    if (strResult == "非法地区" || strResult == "非法生日" || strResult == "非法证号")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            #endregion
            #endregion


            #region 通用正则表达式判断函数
            /**/
            /// <summary>
            /// 通用正则表达式判断函数
            /// </summary>
            /// <param name="strVerifyString">String，用于匹配的字符串</param>
            /// <param name="strRegular">String，正则表达式</param>
            /// <param name="regOption">RegexOptions，配置正则表达式的选项</param>
            /// <param name="aryResult">ArrayList，分解的字符串内容</param>
            /// <param name="IsEntirety">Boolean，是否需要完全匹配</param>
            /// <returns></returns>
            public bool CommRegularMatch(string strVerifyString, string strRegular, System.Text.RegularExpressions.RegexOptions regOption, ref System.Collections.ArrayList aryResult, bool IsEntirety)
            {
                System.Text.RegularExpressions.Regex r;
                System.Text.RegularExpressions.Match m;


                #region 如果需要完全匹配的处理
                if (IsEntirety)
                {
                    strRegular = strRegular.Insert(0, @"\A");
                    strRegular = strRegular.Insert(strRegular.Length, @"\z");
                }
                #endregion
                try
                {
                    r = new System.Text.RegularExpressions.Regex(strRegular, regOption);
                }
                catch (System.Exception e)
                {
                    throw (e);
                }
                for (m = r.Match(strVerifyString); m.Success; m = m.NextMatch())
                {
                    aryResult.Add(m);
                }
                if (aryResult.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            #endregion



            #region 中国身份证号码验证
            private string CheckCidInfo(string cid)
            {
                string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null, null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江", "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆", "四川", "贵州", "云南", "西藏", null, null, null, null, null, null, "陕西", "甘肃", "青海", "宁夏", "新疆", null, null, null, null, null, "台湾", null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null, null, null, null, null, null, null, "国外" };
                double iSum = 0;
                string info = string.Empty;
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$");
                System.Text.RegularExpressions.Match mc = rg.Match(cid);
                if (!mc.Success)
                {
                    return string.Empty;
                }
                cid = cid.ToLower();
                cid = cid.Replace("x", "a");
                if (aCity[int.Parse(cid.Substring(0, 2))] == null)
                {
                    return "非法地区";
                }
                try
                {
                    DateTime.Parse(cid.Substring(6, 4) + " - " + cid.Substring(10, 2) + " - " + cid.Substring(12, 2));
                }
                catch
                {
                    return "非法生日";
                }
                for (int i = 17; i >= 0; i--)
                {
                    iSum += (System.Math.Pow(2, i) % 11) * int.Parse(cid[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);
                }
                if (iSum % 11 != 1)
                {
                    return ("非法证号");
                }
                else
                {
                    return (aCity[int.Parse(cid.Substring(0, 2))] + "," + cid.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + cid.Substring(12, 2) + "," + (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女"));
                }
            }
            #endregion


            #region 身份证号码15升级为18位
            private string CidUpdate(string ShortCid)
            {
                char[] strJiaoYan = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
                int[] intQuan = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
                string strTemp;
                int intTemp = 0;
                strTemp = ShortCid.Substring(0, 6) + "19" + ShortCid.Substring(6);
                for (int i = 0; i <= strTemp.Length - 1; i++)
                {
                    intTemp += int.Parse(strTemp.Substring(i, 1)) * intQuan[i];
                }
                intTemp = intTemp % 11;
                return strTemp + strJiaoYan[intTemp];
            }
            #endregion
        }
    }


}
