namespace HLH.Lib.Net
{
    /// <summary>
    /// 网站相关工具类
    /// </summary>
    public class WebSiteHelper
    {
        /// <summary>
        /// 获取站点虚拟目录
        /// </summary>
        /// <returns></returns>
        public static string GetSiteVirtualRoot()
        {
            string url = "http://" + System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Split(new char[] { '/' })[2] + System.Web.HttpContext.Current.Request.ApplicationPath;
            if (url.EndsWith("/"))
                url = url.Substring(0, url.Length - 1);
            return url;

        }
    }
}
