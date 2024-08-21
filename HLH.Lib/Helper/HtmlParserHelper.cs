using System.Collections.Generic;
using System.Text.RegularExpressions;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Util;



/////
namespace HLH.Lib.Helper
{




    /**
     
     *  public static string GetHtmlStr(string url)
        {    
            try
            {
                WebRequest rGet = WebRequest.Create(url);
                WebResponse rSet = rGet.GetResponse();
                Stream s = rSet.GetResponseStream();
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (WebException)
            {
                //连接失败
                return null;
            }
        }
     * 
     *  HtmlWeb web = new HtmlWeb();
        HtmlAgilityPack.HtmlDocument doc = web.Load(url);
        HtmlNode rootnode = doc.DocumentNode;
     * 
     * 
     *  string htmlstr = GetHtmlStr("http://www.hao123.com");
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(htmlstr);
        HtmlNode rootnode = doc.DocumentNode;    //XPath路径表达式，这里表示选取所有span节点中的font最后一个子节点，其中span节点的class属性值为num
        //根据网页的内容设置XPath路径表达式
        string xpathstring = "//span[@class='num']/font[last()]";    
        HtmlNodeCollection aa = rootnode.SelectNodes(xpathstring);    //所有找到的节点都是一个集合
        
        if(aa != null)
        {
            string innertext = aa[0].InnerText;
            string color = aa[0].GetAttributeValue("color", "");    //获取color属性，第二个参数为默认值
            //其他属性大家自己尝试
        }
     * 
     * **/




    /// <summary>
    /// html解析帮助类   印象 中 这个 速度 慢一点。但是 可以自动闭合标签   
    /// HtmlAgilityPack这个速度快 灵活。需要用xpath选择 高效率
    /// </summary>
    public class HtmlParserHelper
    {
        //private static Lexer lexer = null;
        private static Parser parser = null;
        public static NodeList htmlNodes = null;


        private string _CompleteHtmlText;

        /// <summary>
        /// 完整的html，如果原始标签不完整，这个会自动补全
        /// </summary>
        public string CompleteHtmlText
        {
            get
            {
                _CompleteHtmlText = parser.Lexer.Page.Source.ToString();
                return _CompleteHtmlText;
            }

        }


        private string sourceHtmlText;

        /// <summary>
        /// 原始html
        /// </summary>
        public string SourceHtmlText
        {
            get { return sourceHtmlText; }
            set { sourceHtmlText = value; }
        }

        public HtmlParserHelper(string htmltext)
        {

            //lexer = new Lexer(htmltext);
            //parser = new Parser(lexer);
            sourceHtmlText = htmltext;
            parser = new Parser();
            parser.InputHTML = htmltext;
            htmlNodes = parser.Parse(null);

        }



        /// <summary>
        /// 取页面编码
        /// </summary>
        /// <returns></returns>
        public string GetEncodingForParser()
        {
            return parser.Encoding;
        }






        /// <summary>
        /// 根据名称得到标签
        /// <para>适合于一个页面只有一个标签的情况 如title </para>
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ITag GetTagForTagName(NodeList Nodes, string tagName)
        {
            ITag rs = null;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] is ITag)
                {
                    ITag tag = (Nodes[i] as ITag);
                    if (tag.TagName == tagName.Trim().ToUpper())
                    {
                        rs = tag;
                        break;
                    }
                    else
                    {
                        if (rs == null)
                        {
                            if (tag.Children != null)
                            {
                                rs = GetTagForTagName(tag.Children, tagName);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return rs;
        }



        /// <summary>
        /// 根据标签名称，属性和属性值的组合 得到标签
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ITag GetTagForTagNameAttributeNameAndAttributeValue(ITag rs, NodeList Nodes, string tagName, params KeyValuePair<string, string>[] attrKV)
        {
            List<ITag> tags = new List<ITag>();
            tags = GetListTagForTagName(Nodes, tagName);

            foreach (ITag var in tags)
            {
                if (rs != null)
                {
                    break;
                }

                ITag tag = var;
                for (int c = 0; c < tag.Attributes.Count; c++)
                {
                    //比较
                    bool IsHasValue = false;
                    foreach (KeyValuePair<string, string> kv in attrKV)
                    {
                        if (tag.Attributes[kv.Key.ToUpper()] != null && tag.Attributes[kv.Key.ToUpper()].ToString() == kv.Value)
                        {
                            IsHasValue = true;
                        }
                        else
                        {
                            IsHasValue = false;
                        }
                    }

                    if (IsHasValue)
                    {
                        rs = tag;
                        //return rs;
                        break;
                    }


                    //if (tag.Attributes[attrName.Trim().ToUpper()] != null)
                    //{
                    //    if (tag.Attributes[attrName.Trim().ToUpper()].ToString() == attrValue)
                    //    {

                    //    }



                    //}


                    //end 比较
                }
                if (rs == null)
                {
                    if (tag.Children != null)
                    {
                        rs = GetTagForTagNameAttributeNameAndAttributeValue(rs, tag.Children, tagName, attrKV);
                    }
                }
                else
                {
                    break;
                }

            }
            return rs;


        }




        /// <summary>
        /// 根据名称，属性，属性值 得到标签
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ITag GetTagForTagNameAttributeNameAndAttributeValue(NodeList Nodes, string tagName, string attrName, string attrValue, ITag rs)
        {
            List<ITag> tags = new List<ITag>();
            tags = GetListTagForTagName(Nodes, tagName);

            foreach (ITag var in tags)
            {
                if (rs != null)
                {
                    break;
                }

                ITag tag = var;
                for (int c = 0; c < tag.Attributes.Count; c++)
                {
                    if (tag.Attributes[attrName.Trim().ToUpper()] != null)
                    {
                        if (tag.Attributes[attrName.Trim().ToUpper()].ToString() == attrValue)
                        {
                            rs = tag;
                            //return rs;
                            break;
                        }

                    }
                }
                if (rs == null)
                {
                    if (tag.Children != null)
                    {
                        rs = GetTagForAttributeNameAndAttributeValue(tag.Children, attrName, attrValue, rs);
                    }
                }
                else
                {
                    break;
                }

            }
            return rs;


        }



        /// <summary>
        /// 根据名称得到标签集合
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public List<ITag> GetListTagForTagName(NodeList Nodes, string tagName)
        {
            List<ITag> rs = new List<ITag>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] is ITag)
                {
                    ITag tag = (Nodes[i] as ITag);
                    if (tag.TagName == tagName.Trim().ToUpper())
                    {
                        rs.Add(tag);

                    }
                    else
                    {
                        if (tag.Children != null)
                        {
                            rs.AddRange(GetListTagForTagName(tag.Children, tagName));
                        }
                    }
                }
            }
            return rs;
        }




        /// <summary>
        /// 根据属性得到标签
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public List<ITag> GetTagForAttributeNameAndAttributeValue(NodeList Nodes, string attrName, string attrValue, List<ITag> rsList)
        {

            for (int i = 0; i < Nodes.Count; i++)
            {

                if (Nodes[i] is ITag)
                {
                    ITag tag = (Nodes[i] as ITag);
                    for (int c = 0; c < tag.Attributes.Count; c++)
                    {
                        if (tag.Attributes[attrName.Trim().ToUpper()] != null)
                        {
                            if (tag.Attributes[attrName.Trim().ToUpper()].ToString() == attrValue)
                            {
                                rsList.Add(tag);
                                //return rs;
                                break;
                            }

                        }
                    }
                    if (tag.Children != null)
                    {
                        GetTagForAttributeNameAndAttributeValue(tag.Children, attrName, attrValue, rsList);
                    }

                }
            }
            return rsList;
        }



        /// <summary>
        /// 根据属性得到标签
        /// <para>如果整个页面有属性及值相同的标签则返回第一个</para>
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public ITag GetTagForAttributeNameAndAttributeValue(NodeList Nodes, string attrName, string attrValue, ITag rs)
        {
            if (rs != null)
            {
                return rs;
            }
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (rs != null)
                {
                    break;
                }
                if (Nodes[i] is ITag)
                {
                    ITag tag = (Nodes[i] as ITag);
                    for (int c = 0; c < tag.Attributes.Count; c++)
                    {
                        if (tag.Attributes[attrName.Trim().ToUpper()] != null)
                        {
                            if (tag.Attributes[attrName.Trim().ToUpper()].ToString() == attrValue)
                            {
                                rs = tag;
                                //return rs;
                                break;
                            }

                        }
                    }
                    if (rs == null)
                    {
                        if (tag.Children != null)
                        {
                            rs = GetTagForAttributeNameAndAttributeValue(tag.Children, attrName, attrValue, rs);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return rs;
        }




        /// <summary>
        /// 根据属性得到标签
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public ITag GetTagForAttributeName(NodeList Nodes, string attrName)
        {
            ITag rs = null;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] is ITag)
                {
                    ITag tag = (Nodes[i] as ITag);
                    for (int c = 0; c < tag.Attributes.Count; c++)
                    {
                        if (tag.Attributes[attrName.Trim().ToUpper()] != null)
                        {
                            rs = tag;
                            break;
                        }
                    }
                    if (rs == null)
                    {
                        if (tag.Children != null)
                        {
                            rs = GetTagForAttributeName(tag.Children, attrName);
                            if (rs != null)
                            {
                                return rs;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return rs;
        }


        /// <summary>
        /// 控件字段类型
        /// </summary>
        public enum ControlEnum
        {
            /// <summary>
            /// 普通输入字段
            /// </summary>
            文本框,

            /// <summary>
            /// select
            /// </summary>
            列表框,

            复选框,

            单选框,


            键值对,

            键值副本,

            验证码,

            文件,

            JS方法,

            按钮

        }

        /// <summary>
        /// 根据tag标签找到描述他的文字
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static string GetTagDescription(ITag tag, ControlEnum controlType)
        {
            string rs = string.Empty;
            if (string.IsNullOrEmpty(rs))
            {
                //从找到的控件属性找描述内容
                for (int a = 0; a < tag.AttributesEx.Count; a++)
                {
                    rs += tag.AttributesEx[a].ToString();
                }


            }


            switch (controlType)
            {
                case ControlEnum.文本框:
                    #region 文本框

                    //找前面的控件,找不到，再前一级
                    if (string.IsNullOrEmpty(rs))
                    {
                        if (tag.PreviousSibling is ITag && tag.PreviousSibling != null)
                        {
                            ITag desctag = (tag.PreviousSibling as ITag);
                            rs = desctag.ToPlainTextString();
                        }
                        else
                        {
                            if (tag.PreviousSibling != null)
                            {
                                if (tag.PreviousSibling is Winista.Text.HtmlParser.Nodes.TextNode)
                                {
                                    rs = tag.PreviousSibling.ToPlainTextString();
                                }
                                else if (tag.PreviousSibling.PreviousSibling is ITag && tag.PreviousSibling.PreviousSibling != null)
                                {
                                    ITag desctag = (tag.PreviousSibling.PreviousSibling as ITag);
                                    rs = desctag.ToPlainTextString();
                                }
                            }

                        }

                    }


                    if (string.IsNullOrEmpty(rs))
                    {
                        //层
                        if (tag.Parent.ToPlainTextString().Contains("*"))
                        {
                            rs += "*";//必填项
                        }
                        rs += tag.Parent.ToPlainTextString();
                    }
                    //层


                    if (string.IsNullOrEmpty(rs))
                    {
                        if (tag.Parent.Parent.ToPlainTextString().Contains("*"))
                        {
                            rs += "*";//必填项
                        }
                        rs += tag.Parent.Parent.ToPlainTextString();
                    }
                    string tempdesc = string.Empty;
                    for (int a = 0; a < tag.AttributesEx.Count; a++)
                    {
                        tempdesc += tag.AttributesEx[a].ToString();
                    }
                    if (tempdesc.Trim() != "")
                    {
                        rs = tempdesc.Trim();
                    }



                    //层
                    if (string.IsNullOrEmpty(rs))
                    {
                        if (tag.Parent.Parent.Parent.ToPlainTextString().Contains("*"))
                        {
                            rs += "*";//必填项
                        }
                        rs += tag.Parent.Parent.Parent.ToPlainTextString();
                    }

                    #endregion
                    break;
                case ControlEnum.列表框:
                    break;
                case ControlEnum.复选框:
                    break;
                case ControlEnum.单选框:
                    #region 单选框
                    //找前面的控件,找不到，再后一级
                    if (string.IsNullOrEmpty(rs))
                    {
                        if (tag.NextSibling is ITag && tag.NextSibling != null)
                        {
                            ITag desctag = (tag.NextSibling as ITag);
                            rs = desctag.ToPlainTextString();
                        }
                        else
                        {
                            if (tag.NextSibling != null)
                            {
                                if (tag.NextSibling is Winista.Text.HtmlParser.Nodes.TextNode)
                                {
                                    rs = tag.NextSibling.ToPlainTextString();
                                }
                                else if (tag.NextSibling.NextSibling is ITag && tag.NextSibling.NextSibling != null)
                                {
                                    ITag desctag = (tag.NextSibling.NextSibling as ITag);
                                    rs = desctag.ToPlainTextString();
                                }
                            }

                        }

                    }
                    #endregion
                    break;
                case ControlEnum.JS方法:
                    break;
                case ControlEnum.验证码:
                    break;
                case ControlEnum.文件:
                    break;
                case ControlEnum.按钮:
                    break;

                default:
                    break;
            }


            if (string.IsNullOrEmpty(rs))
            {
                rs = tag.GetAttribute("name");
            }
            if (rs.Length > 40)
            {
                rs = rs.Substring(0, 40);
            }
            return rs;
        }





        /// <summary>
        /// 根据文字得到标签(不是公共的。应该移出这个类)
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="DescContent">"验证码|注册码|输入图中字符"</param>
        /// <returns></returns>
        public ITag GetTagForDescChar(NodeList Nodes, string DescContent)
        {
            int targetIndex = 0;
            string strRs = string.Empty;
            string myRegex = @"(\s){0,}(""|)(验证码|注册码|输入图中字符)";
            Regex mRegex = new Regex(myRegex);
            MatchCollection mMactchCol = mRegex.Matches(this.SourceHtmlText.ToLower());
            foreach (Match mMatch in mMactchCol)
            {
                string myrs = string.Format("{0} {1}", mMatch, mMatch.Index);
                targetIndex = mMatch.Index;
                strRs = myrs;

                break;
            }
            //取
            HLH.Lib.List.SortableList<TagIndex> queue = new HLH.Lib.List.SortableList<TagIndex>();


            List<ITag> Listtag = GetListTagForTagName(htmlNodes, "IMG");
            for (int i = 0; i < Listtag.Count; i++)
            {
                ITag tag = (Listtag[i] as ITag);
                TagIndex t = new TagIndex();
                t.Tag = tag;
                t.Pindex = tag.StartPosition;
                queue.Add(t);
            }

            queue.Sort("Pindex", true);

            // 如果找到图片。看再src属性


            ITag rs = null;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] is ITag)
                {
                    ITag tag = (Nodes[i] as ITag);

                    for (int a = 0; a < tag.AttributesEx.Count; a++)
                    {
                        if (tag.AttributesEx[a].ToString().Trim().Contains(DescContent))
                        {
                            rs = tag;
                        }
                        if (rs == null)
                        {
                            if (tag.Children != null)
                            {
                                rs = GetTagForAttributeName(tag.Children, DescContent);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (rs != null)
                    {
                        break;
                    }
                }
            }
            return rs;
        }



        /// <summary>
        /// 根据属性得到标签集合
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public List<ITag> GetListTagForAttributeName(NodeList Nodes, string attrName)
        {
            List<ITag> rs = new List<ITag>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] is ITag)
                {
                    ITag tag = (Nodes[i] as ITag);
                    for (int c = 0; c < tag.Attributes.Count; c++)
                    {
                        if (tag.Attributes[attrName.Trim().ToUpper()] != null)
                        {
                            rs.Add(tag);

                        }
                    }
                    if (tag.Children != null)
                    {
                        rs.AddRange(GetListTagForAttributeName(tag.Children, attrName));
                    }
                }
            }
            return rs;
        }






        /// <summary>
        /// 取得提交页码的提交方法
        /// </summary>
        /// <returns></returns>
        public string RequestMethod()
        {
            string rs = string.Empty;
            ITag tag = GetTagForTagName(htmlNodes, "FORM");
            if (tag != null)
            {
                if (tag.Attributes["METHOD"] != null)
                {
                    rs = tag.Attributes["METHOD"].ToString();
                }

            }

            //如果上面还没有取到，则用正则式去取


            return rs;
        }



        /// <summary>
        /// 当页是否需要验证码(不是公共的。应该移出这个类)
        /// </summary>
        /// <returns></returns>
        public bool IsNeedCheckCode()
        {
            bool pd = false;
            string txt = SourceHtmlText;



            if (txt.IndexOf("验证码") != -1)
            {
                pd = true;
            }

            if (txt.IndexOf("输入图中字符") != -1)
            {
                pd = true;
            }

            if (txt.IndexOf("输入右图中的文字") != -1)
            {
                pd = true;
            }

            if (txt.IndexOf("字符验证") != -1)
            {
                pd = true;
            }
            if (txt.IndexOf("校验码") != -1)
            {
                pd = true;
            }
            if (txt.IndexOf("验 证 码") != -1)
            {
                pd = true;
            }
            if (txt.IndexOf("认证码") != -1)
            {
                pd = true;
            }
            return pd;
        }

        /// <summary>
        /// 取得提交地址(不是公共的。应该移出这个类)
        /// </summary>
        /// <returns></returns>
        public string GetSubmitURL()
        {

            string rs = string.Empty;
            ITag tag = GetTagForAttributeName(htmlNodes, "action");
            if (tag != null)
            {
                if (tag.GetAttribute("action") != null)
                {
                    rs = tag.GetAttribute("action").ToString();
                }

            }
            return rs;
        }



        /// <summary>
        /// 取得验证码地址(不是公共的。应该移出这个类)
        /// </summary>
        /// <returns></returns>
        public string GetCheckCodeURL()
        {

            //思路 找中文验证码附近的元素，再分析连接
            //






            string rs = string.Empty;
            ITag tag = GetTagForDescChar(htmlNodes, "验证码");
            if (tag != null)
            {
                if (tag.GetAttribute("action") != null)
                {
                    rs = tag.GetAttribute("action").ToString();
                }

            }
            return rs;
        }


        /// <summary>
        /// 取得页面所有可见输入框的标签(不是公共的。应该移出这个类)
        /// </summary>
        /// <returns></returns>
        public List<ITag> GetInutTags()
        {
            List<ITag> rs = new List<ITag>();
            List<ITag> Listtag = GetListTagForTagName(htmlNodes, "input");
            for (int i = 0; i < Listtag.Count; i++)
            {
                if (Listtag[i].GetAttribute("type") == null)
                {
                    continue;
                }
                if (Listtag[i].GetAttribute("type").ToString().ToLower() == "submit" || Listtag[i].GetAttribute("type").ToLower() == "hidden" || Listtag[i].GetAttribute("type").ToLower() == "image" || Listtag[i].GetAttribute("type").ToLower() == "radio" || Listtag[i].GetAttribute("type").ToLower() == "button")
                {
                    continue;
                }

                rs.Add(Listtag[i]);
            }
            return rs;
        }


        public void GetTest()
        {
            NodeFilter filter = new StringFilter("span");
            NodeFilter filterID = new HasAttributeFilter("class");

            NodeFilter filtertest = new TagNameFilter("HTML");

            NodeList nodes = htmlNodes.ExtractAllNodesThatMatch(filtertest);
        }


        /// <summary>
        /// 取得页面所有的标签
        /// <para></para>
        /// </summary>
        /// <returns></returns>
        public List<ITag> GetPageElement()
        {
            NodeFilter filtertest = new TagNameFilter("div");

            NodeList nodess = htmlNodes.ExtractAllNodesThatMatch(filtertest);


            NodeFilter filterJS = new NodeClassFilter(typeof(Winista.Text.HtmlParser.Tags.ScriptTag));
            NodeList nodelistJS = null;
            try
            {
                nodelistJS = parser.ExtractAllNodesThatMatch(filterJS);
            }
            catch (ParserException e1)
            {


            }

            List<ITag> rs = new List<ITag>();

            //用自带的过滤方法
            NodeFilter filter1 = new TagNameFilter("INPUT");
            NodeFilter filter2 = new TagNameFilter("SELECT");
            NodeFilter filter3 = new TagNameFilter("textarea");


            //创建OrFilter实例 
            NodeFilter filteror1 = new OrFilter(filter1, filter2);
            NodeFilter filter = new OrFilter(filteror1, filter3);

            NodeList nodes = htmlNodes.ExtractAllNodesThatMatch(filter, true);
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i] is ITag)
                    {
                        rs.Add(nodes[i] as ITag);
                    }

                }

            }


            //filter = new StringFilter("校");
            //NodeList nl = htmlNodes.ExtractAllNodesThatMatch(filter, true);
            //OrFilter orfilter = new OrFilter("");
            //==


            //自己遍历的方法
            //List<ITag> rs = new List<ITag>();
            //List<ITag> Listtag = GetListTagForTagName(htmlNodes, "input");
            //for (int i = 0; i < Listtag.Count; i++)
            //{
            //    rs.Add(Listtag[i]);
            //}
            //Listtag = GetListTagForTagName(htmlNodes, "select");
            //{
            //    for (int b = 0; b < Listtag.Count; b++)
            //    {
            //        rs.Add(Listtag[b]);
            //    }
            //}

            return rs;
        }



        /// <summary>
        /// 取得页面指定类型的标签
        /// <para></para>
        /// </summary>
        /// <returns></returns>
        public List<ITag> GetPageElement(params string[] tagName)
        {
            List<ITag> rs = new List<ITag>();
            for (int i = 0; i < tagName.Length; i++)
            {
                List<ITag> Listtag = GetListTagForTagName(htmlNodes, tagName[i].ToUpper());
                for (int n = 0; n < Listtag.Count; n++)
                {
                    rs.Add(Listtag[n]);
                }
            }

            return rs;
        }


        public List<ITag> getstring()
        {
            List<ITag> rs = new List<ITag>();


            List<ITag> Listtag = GetListTagForTagName(htmlNodes, "input");
            for (int i = 0; i < Listtag.Count; i++)
            {
                if (Listtag[i].GetAttribute("type") == null)
                {
                    continue;
                }
                if (Listtag[i].GetAttribute("type").ToLower() == "hidden")
                {
                    continue;
                }
                if (Listtag[i].Parent.ToPlainTextString().Contains("*"))
                {
                    rs.Add(Listtag[i]);
                }
                else
                {
                    if (Listtag[i].Parent.Parent.ToPlainTextString().Contains("*"))
                    {
                        KeyValuePair<string, string> kp = new KeyValuePair<string, string>(Listtag[i].TagName, Listtag[i].Parent.Parent.ToPlainTextString());
                        rs.Add(Listtag[i]);
                    }
                    else
                    {
                        if (Listtag[i].Parent.Parent.Parent.ToPlainTextString().Contains("*"))
                        {
                            KeyValuePair<string, string> kp = new KeyValuePair<string, string>(Listtag[i].TagName, Listtag[i].Parent.Parent.Parent.ToPlainTextString());
                            rs.Add(Listtag[i]);
                        }
                    }
                }

            }

            return rs;

        }

        public enum RegUrlType
        {
            A_href,
            Input_button
        }



        /// <summary>
        /// 找到普通网页上的注册页链接(不是公共的。应该移出这个类)
        /// </summary>
        /// <returns></returns>
        public List<ITag> GetRegisterLinks()
        {
            List<ITag> rs = new List<ITag>();

            List<ITag> Listtag = GetListTagForTagName(htmlNodes, "A");
            for (int i = 0; i < Listtag.Count; i++)
            {
                if (Listtag[i].GetAttribute("href") != null)
                {
                    if (Listtag[i].GetAttribute("href").ToString().Trim().Length == 0)
                    {
                        continue;
                    }
                    //判断是否为注册的链接
                    if (Listtag[i].GetAttribute("href").ToString().ToLower().Contains("reg") && Listtag[i].ToHtml().Contains("注册"))
                    {
                        rs.Add(Listtag[i]);
                    }
                    //免费注册
                    if (Listtag[i].ToHtml().Contains("免费注册"))
                    {
                        rs.Add(Listtag[i]);
                    }
                }
            }

            //优先级
            //如果经过上面还是没有则
            for (int i = 0; i < Listtag.Count; i++)
            {
                if (Listtag[i].GetAttribute("href") != null)
                {
                    if (Listtag[i].GetAttribute("href").ToString().Trim().Length == 0)
                    {
                        continue;
                    }
                    //判断是否为注册的链接
                    if (Listtag[i].GetAttribute("href").ToString().ToLower().Contains("reg") || Listtag[i].ToHtml().Contains("注册"))
                    {
                        rs.Add(Listtag[i]);
                    }
                }
            }



            return rs;
        }




        /// <summary>
        /// 找到普通网页上的注册页链接(不是公共的。应该移出这个类)
        /// </summary>
        /// <returns></returns>
        public List<ITag> GetRegisterLinks(RegUrlType type)
        {
            List<ITag> rs = new List<ITag>();

            switch (type)
            {
                case RegUrlType.A_href:
                    List<ITag> ListtagA_href = GetListTagForTagName(htmlNodes, "A");
                    for (int i = 0; i < ListtagA_href.Count; i++)
                    {
                        if (ListtagA_href[i].GetAttribute("href") != null)
                        {
                            //判断是否为注册的链接
                            if (ListtagA_href[i].GetAttribute("href").ToString().ToLower().Contains("reg") && ListtagA_href[i].ToHtml().Contains("注册"))
                            {
                                rs.Add(ListtagA_href[i]);
                            }
                        }
                    }

                    break;
                case RegUrlType.Input_button:
                    List<ITag> ListtagInput_button = GetListTagForTagName(htmlNodes, "INPUT");
                    for (int i = 0; i < ListtagInput_button.Count; i++)
                    {

                        if (ListtagInput_button[i].GetAttribute("type") != null)
                        {
                            //判断是否为注册的链接
                            if (ListtagInput_button[i].GetAttribute("type").ToLower().Contains("button") && ListtagInput_button[i].ToHtml().Contains("同意"))
                            {
                                rs.Add(ListtagInput_button[i]);
                            }
                            if (ListtagInput_button[i].GetAttribute("type").ToLower().Contains("button") && ListtagInput_button[i].ToHtml().Contains("注册"))
                            {
                                rs.Add(ListtagInput_button[i]);
                            }
                        }
                    }

                    break;
                default:
                    break;
            }


            return rs;
        }

    }

    public class TagIndex
    {
        private ITag tag;
        private int pindex;

        public Winista.Text.HtmlParser.ITag Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public int Pindex
        {
            get { return pindex; }
            set { pindex = value; }
        }
    }

}
