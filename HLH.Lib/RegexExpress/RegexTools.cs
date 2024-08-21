using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace HLH.Lib.RegexExpress
{
    /// <summary>
    /// ��ʽ�򹤾���
    /// </summary>
    public class RegexTools
    {
        /// <summary>
        /// ��������ȡ
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
        /// �ر��ַ�
        /// </summary>
        public readonly string sPattern��r��n�����ַ� = "^\\d{3}-\\d{3}-\\d{4}$";

        /// <summary>
        /// title��ǩ<TITLE>(.*)</TITLE>
        /// </summary>
        public readonly string sPattern��ǩTitle = "<TITLE>(.*)</TITLE>";






        /// <summary>
        /// �滻
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
                MessageBox.Show("���������ʽ�������������룡" + e.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return rs;
        }


        /// <summary>
        /// ȥ������
        /// </summary>
        /// <param name="sourceStrs">Ҫ������ı���</param>
        /// <param name="NewTxt">�滻����ַ�</param>
        /// <returns></returns>
        public string RemoveChinese(string sourceStrs, string NewTxt)
        {
            Regex objRegExp = new Regex("[\u4E00-\u9FA5]+");
            string strOutput = objRegExp.Replace(sourceStrs, NewTxt);
            return strOutput;
        }

        /**/
        /// <summary>
        /// ��Html��ǩת��Ϊ�ո�
        /// </summary>
        /// <param name="strHtml">��ת�����ַ���</param>
        /// <returns>����ת�����ַ���</returns>
        private string stripHtml(string strHtml)
        {
            Regex objRegExp = new Regex("<(.|\n)+?>");
            string strOutput = objRegExp.Replace(strHtml, "");
            strOutput = strOutput.Replace("<", "&lt;");
            strOutput = strOutput.Replace(">", "&gt;");

            //�����пո��Ϊһ���ո�
            Regex r = new Regex(@"\s+");
            strOutput = r.Replace(strOutput, " ");
            strOutput.Trim();

            return strOutput;
        }


        public string NoHTML(string Htmlstring)
        {

            //ɾ���ű� 

            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //ɾ��HTML 

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
