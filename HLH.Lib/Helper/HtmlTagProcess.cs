using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HLH.Lib.Helper
{
    public class HtmlTagProcess
    {
        public static List<KeyValuePair<string, string>> Tags
        {
            get
            {
                List<KeyValuePair<string, string>> m_tags = new List<KeyValuePair<string, string>>();
                m_tags.Add(new KeyValuePair<string, string>("链接 <a", @"<a[\s\S]*?>|</a[\s\S]*?>")); // "(?<=<a(.*?)>)(.*?)(?=</a>)"));这个只取里面的包括了内容
                m_tags.Add(new KeyValuePair<string, string>("表格体 <tbody", @"<tbody[\s\S]*?>|</tbody[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("表单 <form", @"<form[\s\S]*?>|</form[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("表格 <table", @"<table[\s\S]*?>|</table[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("图片 <img", @"<img[\s\S]*?>|</img[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("框架 <frame", @"<frame[\s\S]*?>|</frame[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("表格行 <tr", @"<tr[\s\S]*?>|</tr[\s\S]*?>"));//<td(.+?)*?>|</td(.+?)*?>这个没有包括换行符
                m_tags.Add(new KeyValuePair<string, string>("脚本 <script", @"<script[\s\S]*?>([\s\S]*?)</script[\s\S]*?>")); // @"<script[\s\S]*?>|</script[\s\S]*?>"));这个没有包括内容脚本要包括内容
                m_tags.Add(new KeyValuePair<string, string>("列表 <li<ul<dd<dt", @"<li[\s\S]*?>|</li[\s\S]*?>|<ul[\s\S]*?>|</ul[\s\S]*?>|<dd[\s\S]*?>|</dd[\s\S]*?>|<dt[\s\S]*?>|</dt[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("单元 <td", @"<td[\s\S]*?>|</td[\s\S]*?>"));//(?<=<td>)(.*?)(?=</td>) 这个是提取了内容
                m_tags.Add(new KeyValuePair<string, string>("加粗 <b<strong", ""));
                m_tags.Add(new KeyValuePair<string, string>("换行 |tab\r\n\t", ""));
                m_tags.Add(new KeyValuePair<string, string>("段落 <p", ""));
                m_tags.Add(new KeyValuePair<string, string>("换行 <br", ""));
                m_tags.Add(new KeyValuePair<string, string>("去首尾空白字符", ""));
                m_tags.Add(new KeyValuePair<string, string>("字体 <font", ""));
                m_tags.Add(new KeyValuePair<string, string>("空格 &nbsp", ""));
                m_tags.Add(new KeyValuePair<string, string>("框架 <iframe", ""));
                m_tags.Add(new KeyValuePair<string, string>("层 <div", ""));
                m_tags.Add(new KeyValuePair<string, string>("H标签 <h1-7", @"<h\d[\s\S]*?>|</h\d[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("上下标 <sub<sup", ""));
                m_tags.Add(new KeyValuePair<string, string>("Span <span", @"<span[\s\S]*?>|</span[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("hr标签 <hr>", @"<hr[\s\S]*?>|</hr[\s\S]*?>"));
                m_tags.Add(new KeyValuePair<string, string>("所有标签 <", "</?.+?>"));//<(.|\n)+?>
                return m_tags;
            }

        }


        public static void LoadHtmlTag(ListView lv)
        {
            lv.Clear();
            foreach (KeyValuePair<string, string> item in Tags)
            {
                lv.Items.Add(item.Key.ToString());
            }
        }

        public static string RemoveHtmltag(string HtmlTagName, string Source)
        {
            string strOutput = Source;
            foreach (KeyValuePair<string, string> item in Tags)
            {
                if (HtmlTagName == item.Key.ToString() && item.Value.ToString().Trim().Length > 0)
                {
                    Regex objRegExp = new Regex(item.Value.ToString(), RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    strOutput = objRegExp.Replace(strOutput, "");
                }
                else
                {
                    switch (item.Key.ToString())
                    {
                        case "去首尾空白字符":
                            strOutput = strOutput.Trim();
                            break;
                        default:
                            break;
                    }
                }
            }


            //把所有空格变为一个空格
            Regex r = new Regex(@"\s+");
            strOutput = r.Replace(strOutput, " ");
            strOutput.Trim();



            return strOutput;
        }

    }
}
