using System;

namespace Crawler
{
    /// <summary>
    /// Summary description for IWebPageProcessor.
    /// </summary>
    public interface IWebPageProcessor
    {
        bool Process(WebPageState state);

        WebPageContentDelegate ContentHandler { get; set; }
    }

    public delegate void WebPageContentDelegate( WebPageState state );
   // public delegate void WebPageContentDelegate(WebPageState state, params object[] objs);
}
