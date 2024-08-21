using Crawler.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Crawler
{
    public class MyWebSpider : WebSpider
    {
        public MyWebSpider(string startUri) : base(startUri, -1) { }


        public override void HandleLinks(WebPageState state)
        {
            Match m = RegExUtil.GetMatchRegEx(RegularExpression.UrlExtractor, state.Content);
            while (m.Success)
            {
                //if (AddWebPage(state.Uri, m.Groups["url"].ToString()))
                //{
                //    counter++;
                //}
               
                m = m.NextMatch();
            }

            //base.HandleLinks(state);

        }
    }
}
