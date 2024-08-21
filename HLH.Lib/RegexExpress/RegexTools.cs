using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace HLH.Lib.RegexExpress
{
    /// <summary>
    /// 正式则工具类
    /// </summary>
    public class RegexTools
    {
        /// <summary>
        /// 纯数字提取
        /// </summary>
        /// <param name="sourceText"></param>
        /// <returns></returns>
        public int GetNumberFormStr(string sourceText)
        {
            string str = System.Text.RegularExpressions.Regex.Replace(sourceText, @"[^0-9]+", "");
            if (Helper.RegexHelper.IsNumeric(str))
            {
                return int.Parse(str);
            }
            else
            {
                return 0;
            }
        }



        /// <summary>
        /// 特别字符
        /// </summary>
        public readonly string sPattern杠r杠n特殊字符 = "^\\d{3}-\\d{3}-\\d{4}$";

        /// <summary>
        /// title标签<TITLE>(.*)</TITLE>
        /// </summary>
        public readonly string sPattern标签Title = "<TITLE>(.*)</TITLE>";






        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="sourceText"></param>
        /// <param name="NewReplaceTxt"></param>
        /// <returns></returns>
        public string Replace(string regex, string sourceText, string NewReplaceTxt)
        {
            string rs = string.Empty;
            try
            {
                Regex mRegex = new Regex(@"[^\s]");
                rs = mRegex.Replace(sourceText, NewReplaceTxt);
            }
            catch (Exception e)
            {
                MessageBox.Show("输入的正则式错误，请重新输入！" + e.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return rs;
        }


        /// <summary>
        /// 去掉汉字
        /// </summary>
        /// <param name="sourceStrs">要处理的文本段</param>
        /// <param name="NewTxt">替换后的字符</param>
        /// <returns></returns>
        public string RemoveChinese(string sourceStrs, string NewTxt)
        {
            Regex objRegExp = new Regex("[\u4E00-\u9FA5]+");
            string strOutput = objRegExp.Replace(sourceStrs, NewTxt);
            return strOutput;
        }

        /**/
        /// <summary>
        /// 将Html标签转化为空格
        /// </summary>
        /// <param name="strHtml">待转化的字符串</param>
        /// <returns>经过转化的字符串</returns>
        private string stripHtml(string strHtml)
        {
            Regex objRegExp = new Regex("<(.|\n)+?>");
            string strOutput = objRegExp.Replace(strHtml, "");
            strOutput = strOutput.Replace("<", "&lt;");
            strOutput = strOutput.Replace(">", "&gt;");

            //把所有空格变为一个空格
            Regex r = new Regex(@"\s+");
            strOutput = r.Replace(strOutput, " ");
            strOutput.Trim();

            return strOutput;
        }


        public string NoHTML(string Htmlstring)
        {

            //删除脚本 

            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML 

            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");

            Htmlstring.Replace(">", "");

            Htmlstring.Replace("\r\n", "");

            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;

        }
    }
}
