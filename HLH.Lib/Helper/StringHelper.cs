using System.Globalization;
using System.Text.RegularExpressions;

namespace HLH.Lib.Helper
{
    public class StringHelper
    {






        /// <summary>
        /// 将一组字符串改为驼峰规则的名称
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Delimiter">分割字符 如: - # 空格</param>
        /// <returns></returns>
        public static string stringToHumpName(string str, string Delimiter)
        {
            string[] temp = str.Split(Delimiter.ToCharArray());
            for (var i = 0; i < temp.Length; i++)
            {
                temp[i] = temp[i][0].ToString().ToUpper() + temp[i].Substring(1);
            }

            return string.Join("", temp);
        }


        //Unicode转义(\uXXXX)的编码和解码
        #region   Unicode转义的编码和解码


        static Regex reUnicode = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecodeForUnicode(string s)
        {
            return reUnicode.Replace(s, m =>
            {
                short c;
                if (short.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out c))
                {
                    return "" + (char)c;
                }
                return m.Value;
            });
        }


        static Regex reUnicodeChar = new Regex(@"[^\u0000-\u00ff]", RegexOptions.Compiled);

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeForUnicode(string s)
        {
            return reUnicodeChar.Replace(s, m => string.Format(@"\u{0:x4}", (short)m.Value[0]));
        }

        #endregion



        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="iParam"></param>
        /// <returns></returns>
        public static string InterceptString(string str, int iParam)
        {
            string result = str;
            if (str.Length > iParam)
                result = str.Substring(0, iParam) + "...";
            return result;
        }

        public static string Extract(string strOriginal, string RemoveForStart, string RemoveForEnd)
        {
            if (RemoveForStart.Trim().Length == 0 || RemoveForEnd.Trim().Length == 0)
            {
                return strOriginal;
            }
            else
            {


                string oldTemp = strOriginal;
                int startIndex = oldTemp.IndexOf(RemoveForStart);
                int endIndex = oldTemp.IndexOf(RemoveForEnd);
                if (startIndex == -1 || endIndex == -1)
                {
                    return strOriginal;
                }
                else
                {
                    strOriginal = strOriginal.Replace(oldTemp.Substring(startIndex, endIndex - startIndex + RemoveForEnd.Length), "");
                    return strOriginal;
                }

            }
        }
    }
}
