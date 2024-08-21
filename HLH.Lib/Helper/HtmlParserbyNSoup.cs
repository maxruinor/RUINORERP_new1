namespace HLH.Lib.Helper
{
    public class HtmlParserbyNSoup
    {
        public HtmlParserbyNSoup(string HtmlString)
        {
            //NSoup.Nodes.Document doc = NSoupClient.Parse(HtmlString);

            //NSoup.Nodes.Document doc = NSoup.NSoupClient.Connect("http://www.oschina.net/").Get();

            // ebClient webClient = new WebClient();
            //String HtmlString = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData("http://www.oschina.net/"));
            // NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);

            //WebRequest webRequest = WebRequest.Create("http://www.oschina.net/");
            // NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(webRequest.GetResponse().GetResponseStream(), "utf-8");
        }
    }
}
