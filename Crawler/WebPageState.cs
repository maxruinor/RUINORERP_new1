using System;
using System.Net;
using System.Collections;

namespace Crawler
{
    /// <summary>
    /// Summary description for WebPage.
    /// 页面状态，也可以作为采集的每个页时，传过去的参数
    /// </summary>
    public class WebPageState
    {

        /// <summary>
        /// 如果不需要采集url为空时，使用这个
        /// </summary>
        public WebPageState() { }

        public WebPageState(Uri uri)
        {
            m_uri = uri;
        }



        public WebPageState(string uri)
            : this(new Uri(uri)) { }


        //传递参数
        private CookieContainer _cookies = new CookieContainer();
        public CookieContainer cookies { get => _cookies; set => _cookies = value; }



        private int rowID = 0;

        /// <summary>
        /// 数据库中的行号  更新等操作以这个和地址为条件
        /// 任何采集 包括地址和内容 不管为几级页，深度，目前的四路这些都是数据库中的数据
        /// 所以对这些操作都以这个ID为标准
        /// </summary>
        public int RowID
        {
            get { return rowID; }
            set { rowID = value; }
        }



        #region properties

        private bool exit = false;

        public bool Exit
        {
            get { return exit; }
            set { exit = value; }
        }

        Uri m_uri;
        string m_content;
        string m_processInstructions = "";
        bool m_processStarted = false;
        bool m_processSuccessfull = false;
        string m_statusCode;
        string m_statusDescription;

        //Lib.PickTaskParas m_pickTaskParas = new Lib.PickTaskParas();

        private Hashtable m_WebPageUrl = new Hashtable();

        /// <summary>
        /// 保存当前页中提取的地址  key 为内容最低级地址  value  value为一级上级地址
        /// </summary>
        public Hashtable WebPageUrls
        {
            get { return m_WebPageUrl; }
            set { m_WebPageUrl = value; }
        }



        private string _HtmlPageEncoding = string.Empty;

        /// <summary>
        /// 采集页面代码
        /// </summary>
        public string HtmlPageEncoding
        {
            get { return _HtmlPageEncoding; }
            set { _HtmlPageEncoding = value; }
        }

        private object _PagePara = new object();
        /// <summary>
        /// 用来事件中回调传参数用
        /// </summary>
        public object PagePara
        {
            get { return _PagePara; }
            set { _PagePara = value; }
        }

        private int currentLevel = 0;

        /// <summary>
        /// 当前处理的地址的级别，如 详细页就是0，按深度递减
        /// </summary>
        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }


        public Uri Uri
        {
            get
            {
                return m_uri;
            }
        }
        public string Content
        {
            get
            {
                return m_content;
            }
            set
            {
                m_content = value;
            }
        }
        public bool ProcessStarted
        {
            get
            {
                return m_processStarted;
            }
            set
            {
                m_processStarted = value;
            }
        }
        public bool ProcessSuccessfull
        {
            get
            {
                return m_processSuccessfull;
            }
            set
            {
                m_processSuccessfull = value;
            }
        }
        public string ProcessInstructions
        {
            get
            {
                return (m_processInstructions == null ? "" : m_processInstructions);
            }
            set
            {
                m_processInstructions = value;
            }
        }
        public string StatusCode
        {
            get
            {
                return m_statusCode;
            }
            set
            {
                m_statusCode = value;
            }
        }
        public string StatusDescription
        {
            get
            {
                return m_statusDescription;
            }
            set
            {
                m_statusDescription = value;
            }
        }
        private string _requestMethod = "GET";
        public string RequestMethod { get => _requestMethod; set => _requestMethod = value; }
        #endregion
    }
}
