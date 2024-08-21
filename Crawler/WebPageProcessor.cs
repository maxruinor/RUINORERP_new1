using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.IO.Compression;

namespace Crawler
{
    /// <summary>
    /// WebPageProcessor is an implementation of the IWebPageProcessor that 
    /// does the actuall work of reading in the content from an URI and calls
    /// the process content delegates which perform additional work.
    /// </summary>
    public class WebPageProcessor : IWebPageProcessor
    {
        #region public interface
        /// <summary>
        /// Process performs the action of reading in the contents from the URI 
        /// assigned to the WebPageState object that is passed in.
        /// <param name="state">The state object containst the URI to process and will hold onto state regarding the URI as it is processed</param>
        /// <returns>True if the process worked without exception</returns>
        /// </summary>//这个实质没有压缩
        public bool ProcessByCompression(WebPageState state)
        {
            state.ProcessStarted = true;
            state.ProcessSuccessfull = false;

            if (state.Exit)
            {
                return false;
            }
            try
            {
                Console.WriteLine("Process Uri: {0}", state.Uri.AbsoluteUri);

                WebRequest req = WebRequest.Create(state.Uri);

                WebResponse res = null;

                try
                {
                    res = req.GetResponse();

                    if (res is HttpWebResponse)
                    {
                        state.StatusCode = ((HttpWebResponse)res).StatusCode.ToString();
                        state.StatusDescription = ((HttpWebResponse)res).StatusDescription;
                    }
                    if (res is FileWebResponse)
                    {
                        state.StatusCode = "OK";
                        state.StatusDescription = "OK";
                    }

                    if (state.StatusCode.Equals("OK"))
                    {
                        Encoding pageEncoding = Encoding.Default;
                        if (state.HtmlPageEncoding != null && state.HtmlPageEncoding.Length > 0)
                        {
                            if (state.HtmlPageEncoding.ToString() != "自动识别")
                            {
                                pageEncoding = Encoding.GetEncoding(state.HtmlPageEncoding);
                            }
                        }
                        StreamReader sr = new StreamReader(res.GetResponseStream(), pageEncoding);

                        state.Content = sr.ReadToEnd();

                        if (ContentHandler != null)
                        {
                            ContentHandler(state);
                        }
                    }

                    state.ProcessSuccessfull = true;
                }
                catch (Exception ex)
                {
                    //frmMain.InstancePicker.PrintInfoLog(ex);
                    HandleException(ex, state);
                }
                finally
                {
                    if (res != null)
                    {
                        res.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                //frmMain.InstancePicker.PrintInfoLog(ex);
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Successfull: {0}", state.ProcessSuccessfull);

            return state.ProcessSuccessfull;
        }



        private string GetResponseBody(HttpWebResponse response, Encoding pageEncoding)
        {

            string responseBody = string.Empty;



            if (response.ContentEncoding.ToLower().Contains("gzip"))
            {



                using (GZipStream stream = new GZipStream(

                    response.GetResponseStream(), CompressionMode.Decompress))
                {

                    using (StreamReader reader = new StreamReader(stream, pageEncoding))
                    {

                        responseBody = reader.ReadToEnd();

                    }

                }

            }

            else if (response.ContentEncoding.ToLower().Contains("deflate"))
            {

                using (DeflateStream stream = new DeflateStream(

                    response.GetResponseStream(), CompressionMode.Decompress))
                {

                    using (StreamReader reader =

                        new StreamReader(stream, pageEncoding))
                    {

                        responseBody = reader.ReadToEnd();

                    }

                }

            }

            else
            {

                using (Stream stream = response.GetResponseStream())
                {

                    using (StreamReader reader =

                        new StreamReader(stream, pageEncoding))
                    {

                        responseBody = reader.ReadToEnd();

                    }

                }

            }

            return responseBody;


        }
        /// <summary>
        /// Process performs the action of reading in the contents from the URI 
        /// assigned to the WebPageState object that is passed in.
        /// <param name="state">The state object containst the URI to process and will hold onto state regarding the URI as it is processed</param>
        /// <returns>True if the process worked without exception</returns>
        /// </summary>这个压缩了
        public bool Process(WebPageState state)
        {
            state.ProcessStarted = true;
            state.ProcessSuccessfull = false;
     
            if (state.Exit)
            {
                return false;
            }
            try
            {
                Console.WriteLine("Process Uri: {0}", state.Uri.AbsoluteUri);

                HttpWebRequest req = HttpWebRequest.Create(state.Uri) as HttpWebRequest;
                req.KeepAlive = true;
                req.CookieContainer = new CookieContainer();
                
                req.CookieContainer=state.cookies;
                
                req.ProtocolVersion = HttpVersion.Version11;
                req.Method = state.RequestMethod;
                req.Accept = "*/* ";
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.56 Safari/536.5";
                req.Referer = state.Uri.AbsoluteUri;
                HttpWebResponse res = null;
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                try
                {
                    req.Headers.Add("Accept-Encoding", "gzip");
                    res = (HttpWebResponse)req.GetResponse();

                    if (res is HttpWebResponse)
                    {
                        state.StatusCode = ((HttpWebResponse)res).StatusCode.ToString();
                        state.StatusDescription = ((HttpWebResponse)res).StatusDescription;
                    }
                    //if (res is FileWebResponse)
                    //{
                    //    state.StatusCode = "OK";
                    //    state.StatusDescription = "OK";
                    //}

                    if (state.StatusCode.Equals("OK"))
                    {
                        Encoding pageEncoding = Encoding.Default;
                        if (state.HtmlPageEncoding != null && state.HtmlPageEncoding.Length > 0)
                        {
                            if (state.HtmlPageEncoding.ToString() != "自动识别")
                            {
                                pageEncoding = Encoding.GetEncoding(state.HtmlPageEncoding);
                            }
                            else
                            {
                                //根据响应头自动识别
                                pageEncoding = Encoding.GetEncoding(res.ContentEncoding);
                            }

                        }


                        //StreamReader sr = new StreamReader(res.GetResponseStream(), pageEncoding);

                        //  state.Content = sr.ReadToEnd();

                        state.Content = GetResponseBody(res, pageEncoding);

                        if (ContentHandler != null)
                        {
                            ContentHandler(state);
                        }
                    }

                    state.ProcessSuccessfull = true;
                }
                catch (Exception ex)
                {
                    //frmMain.InstancePicker.PrintInfoLog(ex);
                    HandleException(ex, state);
                }
                finally
                {
                    if (res != null)
                    {
                        res.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                //frmMain.InstancePicker.PrintInfoLog(ex);
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Successfull: {0}", state.ProcessSuccessfull);

            return state.ProcessSuccessfull;
        }


        /// <summary>
        /// 处理的时，不重新下载，
        /// </summary>
        /// <param name="state"></param>
        /// <param name="CacheContent"></param>
        /// <returns></returns>
        public bool Process(WebPageState state, string CacheContent)
        {
            state.ProcessStarted = true;
            state.ProcessSuccessfull = true;
            try
            {
                state.StatusCode = "OK";
                state.StatusDescription = "OK";
                state.Content = CacheContent;
                if (ContentHandler != null)
                {
                    ContentHandler(state);
                }
                state.ProcessSuccessfull = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Successfull: {0}", state.ProcessSuccessfull);

            return state.ProcessSuccessfull;
        }

        #endregion






        #region local interface

        // Assign status code and description based on thrown exception
        private void HandleException(Exception ex, WebPageState state)
        {
            if (ex is WebException && LookupWebException(ex.ToString(), state, new String[] {
                                                                                               "(400) Bad Request",
                                                                                               "(401) Unauthorized",
                                                                                               "(402) Payment Required",
                                                                                               "(403) Forbidden",
                                                                                               "(404) Not Found",
                                                                                               "(405) Method not allowed",
                                                                                               "(406) Page format not understood",
                                                                                               "(407) Request must be authorized first",
                                                                                               "(408) Request timed out",
                                                                                               "(409) Conflict, to many requests for resource",
                                                                                               "(410) Page use to be there, but now it's gone",
                                                                                               "(411) Content-length missing",
                                                                                               "(412) Pre-condition not met",
                                                                                               "(413) Too big",
                                                                                               "(414) URL is to long",
                                                                                               "(415) Unsupported media type",
                                                                                               "(500) Internal Error",
                                                                                               "(501) Not implemented",
                                                                                               "(502) Bad Gateway",
                                                                                               "(503) Server Unavailable",
                                                                                               "(504) Gateway Timeout",
                                                                                               "(505) HTTP not supported" }))
            {
                return;
            }

            if (ex.InnerException != null && ex.InnerException is FileNotFoundException)
            {
                state.StatusCode = "FileNotFound";
                state.StatusDescription = ex.InnerException.Message;
            }
            else
            {
                state.StatusDescription = ex.ToString();
            }
        }

        // Each web error such as 404 does not show up as specific error so lookup the code from a WebException
        private bool LookupWebException(string ex, WebPageState state, string[] errors)
        {
            foreach (string error in errors)
            {
                string errCode = error.Substring(0, 5);
                if (ex.IndexOf(errCode) != -1)
                {
                    state.StatusCode = errCode;
                    state.StatusDescription = error;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region properties
        private WebPageContentDelegate m_contentHandler = null;

        public WebPageContentDelegate ContentHandler
        {
            get
            {
                return m_contentHandler;
            }
            set
            {
                m_contentHandler = value;
            }
        }
        #endregion
    }
}
