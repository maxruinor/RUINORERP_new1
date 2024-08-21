using System;
using System.Net;
using System.Collections;

namespace Crawler
{
    /// <summary>
    /// Summary description for WebPage.
    /// ҳ��״̬��Ҳ������Ϊ�ɼ���ÿ��ҳʱ������ȥ�Ĳ���
    /// </summary>
    public class WebPageState
    {

        /// <summary>
        /// �������Ҫ�ɼ�urlΪ��ʱ��ʹ�����
        /// </summary>
        public WebPageState() { }

        public WebPageState(Uri uri)
        {
            m_uri = uri;
        }



        public WebPageState(string uri)
            : this(new Uri(uri)) { }


        //���ݲ���
        private CookieContainer _cookies = new CookieContainer();
        public CookieContainer cookies { get => _cookies; set => _cookies = value; }



        private int rowID = 0;

        /// <summary>
        /// ���ݿ��е��к�  ���µȲ���������͵�ַΪ����
        /// �κβɼ� ������ַ������ ����Ϊ����ҳ����ȣ�Ŀǰ����·��Щ�������ݿ��е�����
        /// ���Զ���Щ�����������IDΪ��׼
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
        /// ���浱ǰҳ����ȡ�ĵ�ַ  key Ϊ������ͼ���ַ  value  valueΪһ���ϼ���ַ
        /// </summary>
        public Hashtable WebPageUrls
        {
            get { return m_WebPageUrl; }
            set { m_WebPageUrl = value; }
        }



        private string _HtmlPageEncoding = string.Empty;

        /// <summary>
        /// �ɼ�ҳ�����
        /// </summary>
        public string HtmlPageEncoding
        {
            get { return _HtmlPageEncoding; }
            set { _HtmlPageEncoding = value; }
        }

        private object _PagePara = new object();
        /// <summary>
        /// �����¼��лص���������
        /// </summary>
        public object PagePara
        {
            get { return _PagePara; }
            set { _PagePara = value; }
        }

        private int currentLevel = 0;

        /// <summary>
        /// ��ǰ����ĵ�ַ�ļ����� ��ϸҳ����0������ȵݼ�
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
