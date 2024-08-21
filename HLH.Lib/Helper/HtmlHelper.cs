using System.Text.RegularExpressions;

namespace HLH.Lib.Helper
{
    public class HtmlHelper
    {

        public static string NoJSscript(string Htmlstring)
        {
            //ɾ���ű� 
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            return Htmlstring;
        }

        /// <summary>
        /// ���˵�html,�ȱ�ǩ
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FilterHtmlStr(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, "");
            html = regex2.Replace(html, "");
            html = regex3.Replace(html, " _disibledevent=");
            html = regex4.Replace(html, "");
            html = regex5.Replace(html, "");
            html = regex6.Replace(html, "");
            html = regex7.Replace(html, "");
            html = regex8.Replace(html, "");
            html = regex9.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }

        /// <summary>
        /// ȥ��HTML���
        /// </summary>
        /// <param name="Htmlstring">����HTML��Դ�� </param>
        /// <returns>�Ѿ�ȥ���������</returns>
        public static string NoHTML(string Htmlstring)
        {
            //ɾ���ű�
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Regex.Replace(Htmlstring, @"<script.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style.*?</style>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);
            //ɾ��HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");

            return Htmlstring;
        }

        /// <summary>
        /// ȥ��html�����е�ָ����html��ǩ
        /// </summary>
        /// <param name="content">html����</param>
        /// <param name="tagName">html��ǩ</param>
        /// <returns>ȥ����ǩ������</returns>
        public static string DropHtmlTag(string content, string tagName)
        {
            //ȥ��<tagname>��</tagname>
            return Drop(content, "<[/]{0,1}" + tagName + "[^\\>]*\\>");
        }

        /// <summary>
        /// ȥ��html������ȫ����ǩ
        /// </summary>
        /// <param name="content">html����</param>
        /// <returns>ȥ��html��ǩ������</returns>
        public static string DropHtmlTag(string content)
        {
            //ȥ��<*>
            return Drop(content, "<[^\\>]*>");
        }

        /// <summary>
        /// ɾ���ַ�����ָ��������
        /// </summary>
        /// <param name="src">Ҫ�޸ĵ��ַ���</param>
        /// <param name="pattern">Ҫɾ����������ʽģʽ</param>
        /// <returns>��ɾ��ָ�����ݵ��ַ���</returns>
        public static string Drop(string src, string pattern)
        {
            return Regex.Replace(src, pattern, "");
        }

        private string StripHtml(string strHtml)
        {
            Regex objRegExp = new Regex("<(.|\n)+?>");
            string strOutput = objRegExp.Replace(strHtml, "");
            strOutput = strOutput.Replace("<", "&lt;");
            strOutput = strOutput.Replace(">", "&gt;");
            return strOutput;
        }



    }
}
